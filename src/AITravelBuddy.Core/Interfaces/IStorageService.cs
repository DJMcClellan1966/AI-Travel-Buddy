using AITravelBuddy.Core.Models;

namespace AITravelBuddy.Core.Interfaces;

/// <summary>
/// Defines the contract for storing and retrieving itineraries.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Saves an itinerary to storage.
    /// </summary>
    /// <param name="itinerary">The itinerary to save.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SaveItineraryAsync(Itinerary itinerary, CancellationToken cancellationToken = default);

    /// <summary>
    /// Loads an itinerary by its ID.
    /// </summary>
    /// <param name="id">The itinerary ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The loaded itinerary, or null if not found.</returns>
    Task<Itinerary?> LoadItineraryAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of all saved itineraries.
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
