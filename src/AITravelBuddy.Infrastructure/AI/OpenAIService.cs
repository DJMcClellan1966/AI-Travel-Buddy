using AITravelBuddy.Core.Interfaces;
using AITravelBuddy.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using System.Text.Json;

namespace AITravelBuddy.Infrastructure.AI;

/// <summary>
/// OpenAI-based implementation of the AI service for generating travel itineraries.
/// </summary>
public class OpenAIService : IAIService
{
    private readonly ChatClient _chatClient;
    private readonly ILogger<OpenAIService> _logger;
    private readonly OpenAISettings _settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenAIService"/> class.
    /// </summary>
    public OpenAIService(
        IOptions<OpenAISettings> settings,
        ILogger<OpenAIService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        _chatClient = new ChatClient(model: _settings.Model, apiKey: _settings.ApiKey);
    }

    /// <inheritdoc/>
    public async Task<Itinerary> GenerateItineraryAsync(TravelPreferences preferences, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating itinerary using OpenAI for {Destination}", preferences.Destination);

        var systemPrompt = BuildSystemPrompt();
        var userPrompt = BuildUserPrompt(preferences);

        try
        {
            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt)
            };

            var options = new ChatCompletionOptions
            {
                MaxOutputTokenCount = _settings.MaxTokens,
                Temperature = 0.7f
            };

            var completion = await _chatClient.CompleteChatAsync(messages, options, cancellationToken);
            var response = completion.Value.Content[0].Text;

            var itinerary = ParseItineraryResponse(response, preferences);
            _logger.LogInformation("Successfully generated itinerary with {DayCount} days", itinerary.DayPlans.Count);

            return itinerary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate itinerary using OpenAI");
            throw new InvalidOperationException("Failed to generate itinerary. Please check your API key and network connection.", ex);
        }
    }

    private static string BuildSystemPrompt()
    {
        return @"You are an expert travel planner and local guide. Your role is to create personalized, realistic, and 
well-balanced travel itineraries. Follow these guidelines:

1. Create day-by-day schedules that are realistic and not overly packed
2. Balance different types of activities throughout each day
3. Include specific restaurant recommendations with cuisine types
4. Provide estimated costs in local currency
5. Include practical information like opening hours and transportation tips
6. Add local insights and cultural tips
7. Consider the user's budget level and adjust recommendations accordingly
8. Respect dietary restrictions and accessibility requirements
9. Include hidden gems and local favorites, not just tourist traps
10. Provide a packing list based on the destination and planned activities

Generate your response in a structured JSON format with the following schema:
{
  ""dayPlans"": [
    {
      ""dayNumber"": 1,
      ""title"": ""Day title/theme"",
      ""activities"": [
        {
          ""time"": ""9:00 AM"",
          ""name"": ""Activity name"",
          ""description"": ""Detailed description"",
          ""location"": ""Address or area"",
          ""estimatedCost"": ""$20-30"",
          ""duration"": ""2 hours"",
          ""tips"": ""Helpful tips""
        }
      ],
      ""notes"": ""General notes for the day"",
      ""estimatedDailyCost"": ""$100-150""
    }
  ],
  ""generalTips"": ""Overall tips for the destination including cultural insights, safety tips, best times to visit attractions, local customs, etc."",
  ""packingList"": [""item1"", ""item2"", ""item3""],
  ""transportationInfo"": ""Information about getting around the destination"",
  ""weatherInfo"": ""Expected weather conditions and what to prepare for"",
  ""estimatedTotalCost"": ""$500-700""
}

Be specific, practical, and helpful. Make the itinerary feel personalized to the user's preferences.";
    }

    private static string BuildUserPrompt(TravelPreferences preferences)
    {
        var interests = string.Join(", ", preferences.Interests);
        var dietary = string.IsNullOrEmpty(preferences.DietaryRestrictions) ? "None" : preferences.DietaryRestrictions;
        var accessibility = string.IsNullOrEmpty(preferences.AccessibilityRequirements) ? "None" : preferences.AccessibilityRequirements;

        return $@"Create a personalized travel itinerary with the following preferences:

Destination: {preferences.Destination}
Duration: {preferences.DurationDays} days
Budget Level: {preferences.BudgetLevel}
Travel Style: {preferences.TravelStyle}
Interests: {interests}
Dietary Restrictions: {dietary}
Accessibility Requirements: {accessibility}

Please generate a detailed, day-by-day itinerary in JSON format as specified in your system instructions.";
    }

    private static Itinerary ParseItineraryResponse(string response, TravelPreferences preferences)
    {
        try
        {
            // Clean up the response to extract JSON if wrapped in markdown code blocks
            var jsonContent = response.Trim();
            
            // Remove markdown code blocks more robustly
            if (jsonContent.StartsWith("```json"))
            {
                jsonContent = jsonContent[7..]; // Remove ```json
            }
            else if (jsonContent.StartsWith("```"))
            {
                jsonContent = jsonContent[3..]; // Remove ```
            }
            
            if (jsonContent.EndsWith("```"))
            {
                jsonContent = jsonContent[..^3]; // Remove trailing ```
            }
            
            jsonContent = jsonContent.Trim();

            var jsonDoc = JsonDocument.Parse(jsonContent);
            var root = jsonDoc.RootElement;

            var itinerary = new Itinerary
            {
                Destination = preferences.Destination,
                Preferences = preferences,
                GeneralTips = root.TryGetProperty("generalTips", out var tips) ? tips.GetString() : null,
                TransportationInfo = root.TryGetProperty("transportationInfo", out var transport) ? transport.GetString() : null,
                WeatherInfo = root.TryGetProperty("weatherInfo", out var weather) ? weather.GetString() : null,
                EstimatedTotalCost = root.TryGetProperty("estimatedTotalCost", out var cost) ? cost.GetString() : null
            };

            if (root.TryGetProperty("packingList", out var packingList))
            {
                foreach (var item in packingList.EnumerateArray())
                {
                    itinerary.PackingList.Add(item.GetString() ?? "");
                }
            }

            if (root.TryGetProperty("dayPlans", out var dayPlans))
            {
                foreach (var dayPlan in dayPlans.EnumerateArray())
                {
                    var day = new DayPlan
                    {
                        DayNumber = dayPlan.GetProperty("dayNumber").GetInt32(),
                        Title = dayPlan.GetProperty("title").GetString() ?? "",
                        Notes = dayPlan.TryGetProperty("notes", out var notes) ? notes.GetString() : null,
                        EstimatedDailyCost = dayPlan.TryGetProperty("estimatedDailyCost", out var dailyCost) ? dailyCost.GetString() : null
                    };

                    if (dayPlan.TryGetProperty("activities", out var activities))
                    {
                        foreach (var activity in activities.EnumerateArray())
                        {
                            day.Activities.Add(new Activity
                            {
                                Time = activity.GetProperty("time").GetString() ?? "",
                                Name = activity.GetProperty("name").GetString() ?? "",
                                Description = activity.GetProperty("description").GetString() ?? "",
                                Location = activity.TryGetProperty("location", out var loc) ? loc.GetString() : null,
                                EstimatedCost = activity.TryGetProperty("estimatedCost", out var actCost) ? actCost.GetString() : null,
                                Duration = activity.TryGetProperty("duration", out var dur) ? dur.GetString() : null,
                                Tips = activity.TryGetProperty("tips", out var actTips) ? actTips.GetString() : null
                            });
                        }
                    }

                    itinerary.DayPlans.Add(day);
                }
            }

            return itinerary;
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse AI response. The response format was invalid.", ex);
        }
    }
}

/// <summary>
/// Configuration settings for OpenAI service.
/// </summary>
public class OpenAISettings
{
    /// <summary>
    /// Gets or sets the OpenAI API key.
    /// </summary>
    public required string ApiKey { get; set; }

    /// <summary>
    /// Gets or sets the model to use (e.g., "gpt-4", "gpt-3.5-turbo").
    /// </summary>
    public required string Model { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of tokens in the response.
    /// </summary>
    public int MaxTokens { get; set; } = 4000;
}
