namespace AITravelBuddy.Core.Models;

/// <summary>
/// Represents a GPS coordinate with latitude and longitude
/// </summary>
public class GpsCoordinate
{
    /// <summary>
    /// Latitude in decimal degrees
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude in decimal degrees
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Creates a new GPS coordinate
    /// </summary>
    public GpsCoordinate()
    {
    }

    /// <summary>
    /// Creates a new GPS coordinate with specified latitude and longitude
    /// </summary>
    /// <param name="latitude">Latitude in decimal degrees</param>
    /// <param name="longitude">Longitude in decimal degrees</param>
    public GpsCoordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Returns a string representation of the GPS coordinate
    /// </summary>
    public override string ToString()
    {
        var latDirection = Latitude >= 0 ? "N" : "S";
        var lonDirection = Longitude >= 0 ? "E" : "W";
        return $"{Math.Abs(Latitude):F4}° {latDirection}, {Math.Abs(Longitude):F4}° {lonDirection}";
    }
}
