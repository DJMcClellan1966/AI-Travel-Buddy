using AITravelBuddy.Core.Interfaces;
using AITravelBuddy.Core.Models;
using Microsoft.Extensions.Logging;

namespace AITravelBuddy.Core.Services;

/// <summary>
/// Main service for orchestrating itinerary generation and management.
/// </summary>
public class ItineraryService : IItineraryService
{
    private readonly IAIService _aiService;
    private readonly IStorageService _storageService;
    private readonly ILogger<ItineraryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItineraryService"/> class.
    /// </summary>
    public ItineraryService(
        IAIService aiService,
        IStorageService storageService,
        ILogger<ItineraryService> logger)
    {
        _aiService = aiService;
        _storageService = storageService;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<Itinerary> CreateItineraryAsync(TravelPreferences preferences, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating itinerary for {Destination}", preferences.Destination);
        
        try
        {
            var itinerary = await _aiService.GenerateItineraryAsync(preferences, cancellationToken);
            _logger.LogInformation("Successfully generated itinerary for {Destination}", preferences.Destination);
            return itinerary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate itinerary for {Destination}", preferences.Destination);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task SaveItineraryAsync(Itinerary itinerary, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving itinerary {Id} for {Destination}", itinerary.Id, itinerary.Destination);
        
        try
        {
            await _storageService.SaveItineraryAsync(itinerary, cancellationToken);
            _logger.LogInformation("Successfully saved itinerary {Id}", itinerary.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save itinerary {Id}", itinerary.Id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Itinerary?> LoadItineraryAsync(string id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Loading itinerary {Id}", id);
        
        try
        {
            var itinerary = await _storageService.LoadItineraryAsync(id, cancellationToken);
            if (itinerary != null)
            {
                _logger.LogInformation("Successfully loaded itinerary {Id}", id);
            }
            else
            {
                _logger.LogWarning("Itinerary {Id} not found", id);
            }
            return itinerary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load itinerary {Id}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<(string Id, string Destination, DateTime CreatedAt)>> ListItinerariesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Listing all itineraries");
        
        try
        {
            var itineraries = await _storageService.ListItinerariesAsync(cancellationToken);
            _logger.LogInformation("Found {Count} itineraries", itineraries.Count);
            return itineraries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list itineraries");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task ExportToTextAsync(Itinerary itinerary, string filePath, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Exporting itinerary {Id} to {FilePath}", itinerary.Id, filePath);
        
        try
        {
            await _storageService.ExportToTextAsync(itinerary, filePath, cancellationToken);
            _logger.LogInformation("Successfully exported itinerary {Id}", itinerary.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export itinerary {Id}", itinerary.Id);
            throw;
        }
    }
}
