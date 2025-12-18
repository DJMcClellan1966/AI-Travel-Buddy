using AITravelBuddy.Core.Models;

namespace AITravelBuddy.Core.Services;

/// <summary>
/// Defines the contract for the main itinerary service that orchestrates AI generation and storage.
/// </summary>
public interface IItineraryService
{
    /// <summary>
    /// Creates a new itinerary based on user preferences.
    /// </summary>
    /// <param name="preferences">The travel preferences.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A newly generated itinerary.</returns>
    Task<Itinerary> CreateItineraryAsync(TravelPreferences preferences, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves an itinerary to storage.
    /// </summary>
    /// <param name="itinerary">The itinerary to save.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SaveItineraryAsync(Itinerary itinerary, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads a saved itinerary by ID.
    /// </summary>
    /// <param name="id">The itinerary ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The loaded itinerary, or null if not found.</returns>
    Task<Itinerary?> LoadItineraryAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all saved itineraries.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of itinerary summaries.</returns>
    Task<List<(string Id, string Destination, DateTime CreatedAt)>> ListItinerariesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports an itinerary to a text file.
    /// </summary>
    /// <param name="itinerary">The itinerary to export.</param>
    /// <param name="filePath">The output file path.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ExportToTextAsync(Itinerary itinerary, string filePath, CancellationToken cancellationToken = default);
}
