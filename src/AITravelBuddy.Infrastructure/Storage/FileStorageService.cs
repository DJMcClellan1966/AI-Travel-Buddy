using AITravelBuddy.Core.Interfaces;
using AITravelBuddy.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace AITravelBuddy.Infrastructure.Storage;

/// <summary>
/// File-based implementation of the storage service.
/// </summary>
public class FileStorageService : IStorageService
{
    private readonly ILogger<FileStorageService> _logger;
    private readonly StorageSettings _settings;
    private readonly string _itinerariesPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileStorageService"/> class.
    /// </summary>
    public FileStorageService(
        IOptions<StorageSettings> settings,
        ILogger<FileStorageService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        _itinerariesPath = Path.GetFullPath(_settings.ItinerariesPath);

        // Ensure directory exists
        if (!Directory.Exists(_itinerariesPath))
        {
            Directory.CreateDirectory(_itinerariesPath);
            _logger.LogInformation("Created itineraries directory at {Path}", _itinerariesPath);
        }
    }

    /// <inheritdoc/>
    public async Task SaveItineraryAsync(Itinerary itinerary, CancellationToken cancellationToken = default)
    {
        var filePath = GetItineraryFilePath(itinerary.Id);
        
        try
        {
            var json = JsonSerializer.Serialize(itinerary, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(filePath, json, cancellationToken);
            _logger.LogInformation("Saved itinerary {Id} to {Path}", itinerary.Id, filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save itinerary {Id}", itinerary.Id);
            throw new InvalidOperationException($"Failed to save itinerary to {filePath}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<Itinerary?> LoadItineraryAsync(string id, CancellationToken cancellationToken = default)
    {
        var filePath = GetItineraryFilePath(id);

        if (!File.Exists(filePath))
        {
            _logger.LogWarning("Itinerary file not found: {Path}", filePath);
            return null;
        }

        try
        {
            var json = await File.ReadAllTextAsync(filePath, cancellationToken);
            var itinerary = JsonSerializer.Deserialize<Itinerary>(json);
            _logger.LogInformation("Loaded itinerary {Id} from {Path}", id, filePath);
            return itinerary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load itinerary {Id}", id);
            throw new InvalidOperationException($"Failed to load itinerary from {filePath}", ex);
        }
    }

    /// <inheritdoc/>
    public async Task<List<(string Id, string Destination, DateTime CreatedAt)>> ListItinerariesAsync(CancellationToken cancellationToken = default)
    {
        var result = new List<(string Id, string Destination, DateTime CreatedAt)>();

        try
        {
            var files = Directory.GetFiles(_itinerariesPath, "*.json");
            
            foreach (var file in files)
            {
                try
                {
                    var json = await File.ReadAllTextAsync(file, cancellationToken);
                    var itinerary = JsonSerializer.Deserialize<Itinerary>(json);
                    
                    if (itinerary != null)
                    {
                        result.Add((itinerary.Id, itinerary.Destination, itinerary.CreatedAt));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to read itinerary file {File}", file);
                }
            }

            result = result.OrderByDescending(i => i.CreatedAt).ToList();
            _logger.LogInformation("Listed {Count} itineraries", result.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list itineraries");
            throw new InvalidOperationException("Failed to list itineraries", ex);
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task ExportToTextAsync(Itinerary itinerary, string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("═══════════════════════════════════════════════════════════════");
            sb.AppendLine($"   {itinerary.Destination.ToUpper()} - TRAVEL ITINERARY");
            sb.AppendLine("═══════════════════════════════════════════════════════════════");
            sb.AppendLine();
            sb.AppendLine($"Created: {itinerary.CreatedAt:MMMM dd, yyyy}");
            sb.AppendLine($"Duration: {itinerary.Preferences.DurationDays} days");
            sb.AppendLine($"Budget: {itinerary.Preferences.BudgetLevel}");
            sb.AppendLine($"Travel Style: {itinerary.Preferences.TravelStyle}");
            sb.AppendLine($"Interests: {string.Join(", ", itinerary.Preferences.Interests)}");
            
            if (!string.IsNullOrEmpty(itinerary.EstimatedTotalCost))
            {
                sb.AppendLine($"Estimated Total Cost: {itinerary.EstimatedTotalCost}");
            }
            
            sb.AppendLine();

            // Day-by-day itinerary
            sb.AppendLine("───────────────────────────────────────────────────────────────");
            sb.AppendLine("   DAILY ITINERARY");
            sb.AppendLine("───────────────────────────────────────────────────────────────");
            sb.AppendLine();

            foreach (var day in itinerary.DayPlans)
            {
                sb.AppendLine($"▶ DAY {day.DayNumber}: {day.Title}");
                if (!string.IsNullOrEmpty(day.EstimatedDailyCost))
                {
                    sb.AppendLine($"  Estimated Cost: {day.EstimatedDailyCost}");
                }
                sb.AppendLine();

                foreach (var activity in day.Activities)
                {
                    sb.AppendLine($"  ⏰ {activity.Time} - {activity.Name}");
                    sb.AppendLine($"     {activity.Description}");
                    
                    if (!string.IsNullOrEmpty(activity.Location))
                    {
                        sb.AppendLine($"     📍 Location: {activity.Location}");
                    }
                    
                    if (!string.IsNullOrEmpty(activity.EstimatedCost))
                    {
                        sb.AppendLine($"     💰 Cost: {activity.EstimatedCost}");
                    }
                    
                    if (!string.IsNullOrEmpty(activity.Duration))
                    {
                        sb.AppendLine($"     ⌛ Duration: {activity.Duration}");
                    }
                    
                    if (!string.IsNullOrEmpty(activity.Tips))
                    {
                        sb.AppendLine($"     💡 Tips: {activity.Tips}");
                    }
                    
                    sb.AppendLine();
                }

                if (!string.IsNullOrEmpty(day.Notes))
                {
                    sb.AppendLine($"  📝 Notes: {day.Notes}");
                    sb.AppendLine();
                }

                sb.AppendLine();
            }

            // General Information
            if (!string.IsNullOrEmpty(itinerary.GeneralTips))
            {
                sb.AppendLine("───────────────────────────────────────────────────────────────");
                sb.AppendLine("   GENERAL TIPS & INSIGHTS");
                sb.AppendLine("───────────────────────────────────────────────────────────────");
                sb.AppendLine();
                sb.AppendLine(itinerary.GeneralTips);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(itinerary.TransportationInfo))
            {
                sb.AppendLine("───────────────────────────────────────────────────────────────");
                sb.AppendLine("   TRANSPORTATION");
                sb.AppendLine("───────────────────────────────────────────────────────────────");
                sb.AppendLine();
                sb.AppendLine(itinerary.TransportationInfo);
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(itinerary.WeatherInfo))
            {
                sb.AppendLine("───────────────────────────────────────────────────────────────");
                sb.AppendLine("   WEATHER & CLIMATE");
                sb.AppendLine("───────────────────────────────────────────────────────────────");
                sb.AppendLine();
                sb.AppendLine(itinerary.WeatherInfo);
                sb.AppendLine();
            }

            if (itinerary.PackingList.Any())
            {
                sb.AppendLine("───────────────────────────────────────────────────────────────");
                sb.AppendLine("   PACKING LIST");
                sb.AppendLine("───────────────────────────────────────────────────────────────");
                sb.AppendLine();
                foreach (var item in itinerary.PackingList)
                {
                    sb.AppendLine($"  ☐ {item}");
                }
                sb.AppendLine();
            }

            sb.AppendLine("═══════════════════════════════════════════════════════════════");
            sb.AppendLine("         Have a wonderful trip! ✈️");
            sb.AppendLine("═══════════════════════════════════════════════════════════════");

            await File.WriteAllTextAsync(filePath, sb.ToString(), cancellationToken);
            _logger.LogInformation("Exported itinerary {Id} to {Path}", itinerary.Id, filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export itinerary {Id}", itinerary.Id);
            throw new InvalidOperationException($"Failed to export itinerary to {filePath}", ex);
        }
    }

    private string GetItineraryFilePath(string id)
    {
        return Path.Combine(_itinerariesPath, $"{id}.json");
    }
}

/// <summary>
/// Configuration settings for storage service.
/// </summary>
public class StorageSettings
{
    /// <summary>
    /// Gets or sets the path where itineraries will be stored.
    /// </summary>
    public required string ItinerariesPath { get; set; }
}
