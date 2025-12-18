using AITravelBuddy.Core.Models;

namespace AITravelBuddy.Core.Interfaces;

/// <summary>
/// Interface for storing and retrieving itineraries
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Saves an itinerary to storage
    /// </summary>
    /// <param name="itinerary">The itinerary to save</param>
    Task SaveItineraryAsync(Itinerary itinerary);

    /// <summary>
    /// Loads an itinerary from storage
    /// </summary>
    /// <param name="id">The ID of the itinerary to load</param>
    /// <returns>The loaded itinerary</returns>
    Task<Itinerary?> LoadItineraryAsync(string id);

    /// <summary>
    /// Gets a list of all saved itineraries
    /// </summary>
    /// <returns>List of itinerary summaries</returns>
    Task<List<(string Id, string Title, DateTime CreatedAt)>> GetItineraryListAsync();

    /// <summary>
    /// Exports itinerary to a text file
    /// </summary>
    /// <param name="itinerary">The itinerary to export</param>
    /// <param name="fileName">Name of the file to create</param>
    Task ExportToTextAsync(Itinerary itinerary, string fileName);

    /// <summary>
    /// Exports GPS coordinates to CSV format
    /// </summary>
    /// <param name="itinerary">The itinerary to export</param>
    /// <param name="fileName">Name of the CSV file to create</param>
    Task ExportToCsvAsync(Itinerary itinerary, string fileName);
}
