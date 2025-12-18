using AITravelBuddy.Core.Models;

namespace AITravelBuddy.Core.Interfaces;

/// <summary>
/// Defines the contract for AI-powered itinerary generation.
/// </summary>
public interface IAIService
{
    /// <summary>
    /// Generates a travel itinerary based on user preferences.
    /// </summary>
    /// <param name="preferences">The travel preferences.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A generated itinerary.</returns>
    Task<Itinerary> GenerateItineraryAsync(TravelPreferences preferences, CancellationToken cancellationToken = default);
}
