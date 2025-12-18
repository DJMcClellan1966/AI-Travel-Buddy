using AITravelBuddy.Core.Interfaces;
using AITravelBuddy.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace AITravelBuddy.Infrastructure.Storage;

/// <summary>
/// File-based storage service for itineraries
/// </summary>
public class FileStorageService : IStorageService
{
    private readonly string _itinerariesPath;
    private readonly string _exportsPath;
    private readonly JsonSerializerOptions _jsonOptions;

    public FileStorageService(IConfiguration configuration)
    {
        _itinerariesPath = configuration["Storage:ItinerariesPath"] ?? "./itineraries";
        _exportsPath = configuration["Storage:ExportsPath"] ?? "./exports";

        // Ensure directories exist
        Directory.CreateDirectory(_itinerariesPath);
        Directory.CreateDirectory(_exportsPath);

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    /// <inheritdoc/>
    public async Task SaveItineraryAsync(Itinerary itinerary)
    {
        var fileName = Path.Combine(_itinerariesPath, $"{itinerary.Id}.json");
        var json = JsonSerializer.Serialize(itinerary, _jsonOptions);
        await File.WriteAllTextAsync(fileName, json);
    }

    /// <inheritdoc/>
    public async Task<Itinerary?> LoadItineraryAsync(string id)
    {
        var fileName = Path.Combine(_itinerariesPath, $"{id}.json");
        
        if (!File.Exists(fileName))
            return null;

        var json = await File.ReadAllTextAsync(fileName);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        return JsonSerializer.Deserialize<Itinerary>(json, options);
    }

    /// <inheritdoc/>
    public async Task<List<(string Id, string Title, DateTime CreatedAt)>> GetItineraryListAsync()
    {
        var result = new List<(string Id, string Title, DateTime CreatedAt)>();
        var files = Directory.GetFiles(_itinerariesPath, "*.json");

        foreach (var file in files)
        {
            try
            {
                var json = await File.ReadAllTextAsync(file);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var id = root.GetProperty("id").GetString() ?? Path.GetFileNameWithoutExtension(file);
                var title = root.GetProperty("title").GetString() ?? "Untitled";
                var createdAt = root.GetProperty("createdAt").GetDateTime();

                result.Add((id, title, createdAt));
            }
            catch
            {
                // Skip invalid files
                continue;
            }
        }

        return result.OrderByDescending(x => x.CreatedAt).ToList();
    }

    /// <inheritdoc/>
    public async Task ExportToTextAsync(Itinerary itinerary, string fileName)
    {
        var fullPath = Path.Combine(_exportsPath, fileName);
        var sb = new StringBuilder();

        sb.AppendLine("╔════════════════════════════════════════════════════════════════════╗");
        sb.AppendLine($"  {itinerary.Title}");
        sb.AppendLine("╚════════════════════════════════════════════════════════════════════╝");
        sb.AppendLine();
        sb.AppendLine($"Destination: {itinerary.Preferences.Destination}");
        sb.AppendLine($"Duration: {itinerary.Preferences.DurationDays} days");
        sb.AppendLine($"Budget Level: {itinerary.Preferences.BudgetLevel}");
        sb.AppendLine($"Travel Style: {itinerary.Preferences.TravelStyle}");
        sb.AppendLine($"Total Estimated Cost: {itinerary.TotalEstimatedCost:C} {itinerary.Currency}");
        sb.AppendLine();

        foreach (var dayPlan in itinerary.DayPlans)
        {
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            sb.AppendLine($"Day {dayPlan.DayNumber}: {dayPlan.Title}");
            sb.AppendLine($"Date: {dayPlan.Date:yyyy-MM-dd}");
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(dayPlan.Weather))
            {
                sb.AppendLine($"Weather: {dayPlan.Weather}");
                sb.AppendLine();
            }

            foreach (var activity in dayPlan.Activities)
            {
                sb.AppendLine($"📍 {activity.Name}");
                if (!string.IsNullOrEmpty(activity.SuggestedTime))
                    sb.AppendLine($"   Time: {activity.SuggestedTime}");
                
                sb.AppendLine($"   {activity.Description}");
                
                if (activity.Location != null)
                {
                    sb.AppendLine($"   Address: {activity.Location.Address}");
                    if (activity.Location.Coordinate != null)
                    {
                        sb.AppendLine($"   GPS: {activity.Location.Coordinate}");
                        sb.AppendLine($"   🗺️  Map: {activity.Location.GoogleMapsUrl}");
                    }
                }

                sb.AppendLine($"   ⏱️  Duration: {activity.DurationHours} hours");
                sb.AppendLine($"   💰 Cost: {activity.EstimatedCost:C} {activity.Currency}");

                if (activity.DistanceFromPrevious.HasValue)
                {
                    sb.AppendLine($"   🚶 Distance from previous: {activity.DistanceFromPrevious.Value:F2} km");
                    if (activity.TravelTimeFromPrevious.HasValue)
                        sb.AppendLine($"   ⏰ Travel time: ~{activity.TravelTimeFromPrevious.Value} minutes");
                }

                if (!string.IsNullOrEmpty(activity.Tips))
                    sb.AppendLine($"   💡 Tips: {activity.Tips}");

                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(dayPlan.DayRouteMapUrl))
            {
                sb.AppendLine($"🗺️  Full Day Route: {dayPlan.DayRouteMapUrl}");
                sb.AppendLine();
            }

            sb.AppendLine($"Day {dayPlan.DayNumber} Summary:");
            sb.AppendLine($"  Total Distance: {dayPlan.TotalDistanceKm:F2} km");
            sb.AppendLine($"  Total Cost: {dayPlan.TotalEstimatedCost:C} {itinerary.Currency}");
            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(itinerary.GeneralTips))
        {
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            sb.AppendLine("GENERAL TIPS");
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            sb.AppendLine(itinerary.GeneralTips);
            sb.AppendLine();
        }

        if (itinerary.PackingList.Any())
        {
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            sb.AppendLine("PACKING LIST");
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            foreach (var item in itinerary.PackingList)
            {
                sb.AppendLine($"  ☑ {item}");
            }
            sb.AppendLine();
        }

        sb.AppendLine($"Generated on: {itinerary.CreatedAt:yyyy-MM-dd HH:mm:ss} UTC");

        await File.WriteAllTextAsync(fullPath, sb.ToString());
    }

    /// <inheritdoc/>
    public async Task ExportToCsvAsync(Itinerary itinerary, string fileName)
    {
        var fullPath = Path.Combine(_exportsPath, fileName);
        var sb = new StringBuilder();

        // CSV Header
        sb.AppendLine("Day,Activity,Type,Address,Latitude,Longitude,Duration (hours),Cost,Currency,Google Maps URL");

        foreach (var dayPlan in itinerary.DayPlans)
        {
            foreach (var activity in dayPlan.Activities)
            {
                var location = activity.Location;
                sb.Append($"{dayPlan.DayNumber},");
                sb.Append($"\"{activity.Name.Replace("\"", "\"\"")}\",");
                sb.Append($"\"{location?.Type ?? ""}\",");
                sb.Append($"\"{location?.Address?.Replace("\"", "\"\"") ?? ""}\",");
                sb.Append($"{location?.Coordinate?.Latitude ?? 0},");
                sb.Append($"{location?.Coordinate?.Longitude ?? 0},");
                sb.Append($"{activity.DurationHours},");
                sb.Append($"{activity.EstimatedCost},");
                sb.Append($"{activity.Currency},");
                sb.AppendLine($"\"{location?.GoogleMapsUrl ?? ""}\"");
            }
        }

        await File.WriteAllTextAsync(fullPath, sb.ToString());
    }
}
