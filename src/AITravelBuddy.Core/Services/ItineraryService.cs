using AITravelBuddy.Core.Interfaces;
using AITravelBuddy.Core.Models;

namespace AITravelBuddy.Core.Services;

/// <summary>
/// Main service for orchestrating itinerary creation and management
/// </summary>
public class ItineraryService : IItineraryService
{
    private readonly IAIService _aiService;
    private readonly IStorageService _storageService;
    private readonly IMapService _mapService;

    public ItineraryService(
        IAIService aiService,
        IStorageService storageService,
        IMapService mapService)
    {
        _aiService = aiService;
        _storageService = storageService;
        _mapService = mapService;
    }

    /// <inheritdoc/>
    public async Task<Itinerary> CreateItineraryAsync(TravelPreferences preferences)
    {
        // Generate itinerary using AI
        var itinerary = await _aiService.GenerateItineraryAsync(preferences);

        // Process each day to add map URLs and calculate distances
        foreach (var dayPlan in itinerary.DayPlans)
        {
            var coordinates = new List<GpsCoordinate>();
            Activity? previousActivity = null;

            foreach (var activity in dayPlan.Activities)
            {
                if (activity.Location?.Coordinate != null)
                {
                    // Generate Google Maps URL for the location
                    activity.Location.GoogleMapsUrl = _mapService.GenerateMapUrl(activity.Location.Coordinate);
                    coordinates.Add(activity.Location.Coordinate);

                    // Calculate distance from previous activity
                    if (previousActivity?.Location?.Coordinate != null)
                    {
                        var distance = _mapService.CalculateDistance(
                            previousActivity.Location.Coordinate,
                            activity.Location.Coordinate);
                        activity.DistanceFromPrevious = distance;
                        
                        // Estimate travel time (assuming 5 km/h walking speed)
                        activity.TravelTimeFromPrevious = (int)(distance / 5.0 * 60);
                    }
                }

                previousActivity = activity;
            }

            // Generate route map URL for the entire day
            if (coordinates.Count > 0)
            {
                dayPlan.DayRouteMapUrl = _mapService.GenerateRouteUrl(coordinates);
                dayPlan.TotalDistanceKm = dayPlan.Activities
                    .Where(a => a.DistanceFromPrevious.HasValue)
                    .Sum(a => a.DistanceFromPrevious!.Value);
            }

            // Calculate total cost for the day
            dayPlan.TotalEstimatedCost = dayPlan.Activities.Sum(a => a.EstimatedCost);
        }

        // Calculate total trip cost
        itinerary.TotalEstimatedCost = itinerary.DayPlans.Sum(d => d.TotalEstimatedCost);

        return itinerary;
    }

    /// <inheritdoc/>
    public Task SaveItineraryAsync(Itinerary itinerary)
    {
        return _storageService.SaveItineraryAsync(itinerary);
    }

    /// <inheritdoc/>
    public Task<Itinerary?> LoadItineraryAsync(string id)
    {
        return _storageService.LoadItineraryAsync(id);
    }

    /// <inheritdoc/>
    public Task<List<(string Id, string Title, DateTime CreatedAt)>> GetSavedItinerariesAsync()
    {
        return _storageService.GetItineraryListAsync();
    }
}
