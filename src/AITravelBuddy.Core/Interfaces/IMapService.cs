using AITravelBuddy.Core.Models;

namespace AITravelBuddy.Core.Interfaces;

/// <summary>
/// Interface for map-related services
/// </summary>
public interface IMapService
{
    /// <summary>
    /// Generates a Google Maps URL for a single location
    /// </summary>
    /// <param name="coordinate">GPS coordinate</param>
    /// <returns>Google Maps URL</returns>
    string GenerateMapUrl(GpsCoordinate coordinate);

    /// <summary>
    /// Generates a Google Maps URL with multiple waypoints
    /// </summary>
    /// <param name="coordinates">List of GPS coordinates</param>
    /// <returns>Google Maps URL with route</returns>
    string GenerateRouteUrl(List<GpsCoordinate> coordinates);

    /// <summary>
    /// Calculates distance between two GPS coordinates using Haversine formula
    /// </summary>
    /// <param name="start">Starting coordinate</param>
    /// <param name="end">Ending coordinate</param>
    /// <returns>Distance in kilometers</returns>
    double CalculateDistance(GpsCoordinate start, GpsCoordinate end);

    /// <summary>
    /// Exports itinerary locations to KML format
    /// </summary>
    /// <param name="itinerary">The itinerary to export</param>
    /// <param name="fileName">Name of the KML file to create</param>
    Task ExportToKmlAsync(Itinerary itinerary, string fileName);

    /// <summary>
    /// Exports itinerary locations to GPX format
    /// </summary>
    /// <param name="itinerary">The itinerary to export</param>
    /// <param name="fileName">Name of the GPX file to create</param>
    Task ExportToGpxAsync(Itinerary itinerary, string fileName);

    /// <summary>
    /// Generates an HTML file with embedded map visualization
    /// </summary>
    /// <param name="itinerary">The itinerary to visualize</param>
    /// <param name="fileName">Name of the HTML file to create</param>
    Task GenerateMapHtmlAsync(Itinerary itinerary, string fileName);
}
