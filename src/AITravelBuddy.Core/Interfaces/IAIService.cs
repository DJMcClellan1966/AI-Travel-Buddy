using AITravelBuddy.Core.Models;

namespace AITravelBuddy.Core.Interfaces;

/// <summary>
/// Interface for AI service that generates travel itineraries
/// </summary>
public interface IAIService
{
    /// <summary>
    /// Generates a travel itinerary based on user preferences
    /// </summary>
    /// <param name="preferences">User's travel preferences</param>
    /// <returns>A complete itinerary with GPS coordinates</returns>
    Task<Itinerary> GenerateItineraryAsync(TravelPreferences preferences);
}
