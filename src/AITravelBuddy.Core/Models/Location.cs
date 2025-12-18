namespace AITravelBuddy.Core.Models;

/// <summary>
/// Represents a physical location with GPS coordinates and address information
/// </summary>
public class Location
{
    /// <summary>
    /// Name of the location
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Physical address of the location
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// GPS coordinates of the location
    /// </summary>
    public GpsCoordinate? Coordinate { get; set; }

    /// <summary>
    /// Google Maps URL for the location
    /// </summary>
    public string GoogleMapsUrl { get; set; } = string.Empty;

    /// <summary>
    /// Type of location (e.g., Restaurant, Activity, Hotel, Attraction)
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Additional notes or description about the location
    /// </summary>
    public string? Notes { get; set; }
}
