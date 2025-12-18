using AITravelBuddy.Core.Models;

namespace AITravelBuddy.Core.Services;

/// <summary>
/// Interface for the main itinerary orchestration service
/// </summary>
public interface IItineraryService
{
    /// <summary>
    /// Creates a new itinerary based on user preferences
    /// </summary>
    /// <param name="preferences">User's travel preferences</param>
    /// <returns>A complete itinerary</returns>
    Task<Itinerary> CreateItineraryAsync(TravelPreferences preferences);

    /// <summary>
    /// Saves an itinerary
    /// </summary>
    /// <param name="itinerary">The itinerary to save</param>
    Task SaveItineraryAsync(Itinerary itinerary);

    /// <summary>
    /// Loads a saved itinerary
    /// </summary>
    /// <param name="id">The ID of the itinerary</param>
    /// <returns>The loaded itinerary</returns>
    Task<Itinerary?> LoadItineraryAsync(string id);

    /// <summary>
    /// Gets a list of all saved itineraries
    /// </summary>
    /// <returns>List of itinerary summaries</returns>
    Task<List<(string Id, string Title, DateTime CreatedAt)>> GetSavedItinerariesAsync();
}
