using AITravelBuddy.Core.Interfaces;
using AITravelBuddy.Core.Models;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.Text.Json;

namespace AITravelBuddy.Infrastructure.AI;

/// <summary>
/// OpenAI implementation of the AI service for generating travel itineraries
/// </summary>
public class OpenAIService : IAIService
{
    private readonly ChatClient _chatClient;
    private readonly int _maxTokens;

    public OpenAIService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"] 
            ?? throw new InvalidOperationException("OpenAI API key not configured");
        var model = configuration["OpenAI:Model"] ?? "gpt-4";
        _maxTokens = int.Parse(configuration["OpenAI:MaxTokens"] ?? "4000");

        _chatClient = new ChatClient(model, apiKey);
    }

    /// <inheritdoc/>
    public async Task<Itinerary> GenerateItineraryAsync(TravelPreferences preferences)
    {
        var systemPrompt = @"You are an expert travel planner AI. Generate detailed, personalized travel itineraries with ACTUAL GPS coordinates for every location.

CRITICAL REQUIREMENTS:
1. Include REAL GPS coordinates (latitude/longitude) for EVERY restaurant, activity, and location
2. Include actual street addresses for all locations
3. Group nearby activities together to minimize travel time
4. Include walking/transit distances between consecutive activities
5. Provide specific, actionable recommendations with estimated costs

Return your response as a valid JSON object with this exact structure:
{
  ""title"": ""string"",
  ""preferences"": {...},
  ""dayPlans"": [
    {
      ""dayNumber"": 1,
      ""date"": ""2025-01-15T00:00:00Z"",
      ""title"": ""string"",
      ""activities"": [
        {
          ""name"": ""string"",
          ""description"": ""string"",
          ""location"": {
            ""name"": ""string"",
            ""address"": ""full street address"",
            ""coordinate"": {
              ""latitude"": 35.7148,
              ""longitude"": 139.7967
            },
            ""type"": ""Restaurant|Activity|Hotel|Attraction""
          },
          ""durationHours"": 2.0,
          ""estimatedCost"": 25.00,
          ""currency"": ""USD"",
          ""suggestedTime"": ""9:00 AM"",
          ""category"": ""Culture|Food|Adventure|etc"",
          ""tips"": ""helpful tips""
        }
      ],
      ""notes"": ""string"",
      ""weather"": ""string""
    }
  ],
  ""generalTips"": ""string"",
  ""packingList"": [""item1"", ""item2""],
  ""currency"": ""USD"",
  ""timeZone"": ""string""
}";

        var userPrompt = $@"Create a {preferences.DurationDays}-day travel itinerary for {preferences.Destination}.

Travel Details:
- Start Date: {preferences.StartDate:yyyy-MM-dd}
- Budget Level: {preferences.BudgetLevel}
- Travel Style: {preferences.TravelStyle}
- Interests: {string.Join(", ", preferences.Interests)}
- Dietary Restrictions: {preferences.DietaryRestrictions ?? "None"}
- Accessibility Requirements: {preferences.AccessibilityRequirements ?? "None"}
- Special Requests: {preferences.SpecialRequests ?? "None"}

For EACH activity, restaurant, and attraction:
1. Provide the EXACT real-world GPS coordinates (latitude and longitude)
2. Include the full street address
3. Estimate realistic costs in local currency
4. Consider proximity to group nearby activities together
5. Include specific tips and recommendations

Generate a complete, detailed itinerary with all GPS coordinates filled in.";

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userPrompt)
        };

        var chatCompletionOptions = new ChatCompletionOptions
        {
            MaxOutputTokenCount = _maxTokens,
            Temperature = 0.7f
        };

        var response = await _chatClient.CompleteChatAsync(messages, chatCompletionOptions);
        var content = response.Value.Content[0].Text;

        // Try to extract JSON from markdown code blocks if present
        var jsonContent = content;
        if (content.Contains("```json"))
        {
            var startIndex = content.IndexOf("```json") + 7;
            var endIndex = content.LastIndexOf("```");
            if (endIndex > startIndex)
            {
                jsonContent = content.Substring(startIndex, endIndex - startIndex).Trim();
            }
        }
        else if (content.Contains("```"))
        {
            var startIndex = content.IndexOf("```") + 3;
            var endIndex = content.LastIndexOf("```");
            if (endIndex > startIndex)
            {
                jsonContent = content.Substring(startIndex, endIndex - startIndex).Trim();
            }
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var itinerary = JsonSerializer.Deserialize<Itinerary>(jsonContent, options)
            ?? throw new InvalidOperationException("Failed to parse AI response");

        // Ensure ID and timestamps are set
        if (string.IsNullOrEmpty(itinerary.Id))
            itinerary.Id = Guid.NewGuid().ToString();
        
        if (itinerary.CreatedAt == default)
            itinerary.CreatedAt = DateTime.UtcNow;

        // Ensure preferences are included
        itinerary.Preferences = preferences;

        return itinerary;
    }
}
