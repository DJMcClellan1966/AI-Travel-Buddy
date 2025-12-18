using AITravelBuddy.Core.Interfaces;
using AITravelBuddy.Core.Models;
using System.Text;
using System.Globalization;

namespace AITravelBuddy.Core.Services;

/// <summary>
/// Service for map-related operations
/// </summary>
public class MapService : IMapService
{
    /// <inheritdoc/>
    public string GenerateMapUrl(GpsCoordinate coordinate)
    {
        return $"https://www.google.com/maps?q={coordinate.Latitude:F6},{coordinate.Longitude:F6}";
    }

    /// <inheritdoc/>
    public string GenerateRouteUrl(List<GpsCoordinate> coordinates)
    {
        if (coordinates.Count == 0)
            return string.Empty;

        if (coordinates.Count == 1)
            return GenerateMapUrl(coordinates[0]);

        var origin = coordinates.First();
        var destination = coordinates.Last();
        var waypoints = coordinates.Skip(1).Take(coordinates.Count - 2).ToList();

        var url = new StringBuilder();
        url.Append($"https://www.google.com/maps/dir/?api=1");
        url.Append($"&origin={origin.Latitude:F6},{origin.Longitude:F6}");
        url.Append($"&destination={destination.Latitude:F6},{destination.Longitude:F6}");

        if (waypoints.Count > 0)
        {
            url.Append("&waypoints=");
            url.Append(string.Join("|", waypoints.Select(w => $"{w.Latitude:F6},{w.Longitude:F6}")));
        }

        return url.ToString();
    }

    /// <inheritdoc/>
    public double CalculateDistance(GpsCoordinate start, GpsCoordinate end)
    {
        // Haversine formula for calculating distance between two GPS coordinates
        const double earthRadiusKm = 6371.0;

        var lat1 = DegreesToRadians(start.Latitude);
        var lat2 = DegreesToRadians(end.Latitude);
        var lon1 = DegreesToRadians(start.Longitude);
        var lon2 = DegreesToRadians(end.Longitude);

        var dLat = lat2 - lat1;
        var dLon = lon2 - lon1;

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return earthRadiusKm * c;
    }

    /// <inheritdoc/>
    public async Task ExportToKmlAsync(Itinerary itinerary, string fileName)
    {
        var kml = new StringBuilder();
        kml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        kml.AppendLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
        kml.AppendLine("  <Document>");
        kml.AppendLine($"    <name>{EscapeXml(itinerary.Title)}</name>");
        kml.AppendLine($"    <description>Travel itinerary for {EscapeXml(itinerary.Preferences.Destination)}</description>");

        foreach (var dayPlan in itinerary.DayPlans)
        {
            kml.AppendLine("    <Folder>");
            kml.AppendLine($"      <name>Day {dayPlan.DayNumber}: {EscapeXml(dayPlan.Title)}</name>");

            foreach (var activity in dayPlan.Activities)
            {
                if (activity.Location?.Coordinate != null)
                {
                    kml.AppendLine("      <Placemark>");
                    kml.AppendLine($"        <name>{EscapeXml(activity.Name)}</name>");
                    kml.AppendLine($"        <description>{EscapeXml(activity.Description)}</description>");
                    kml.AppendLine("        <Point>");
                    kml.AppendLine($"          <coordinates>{activity.Location.Coordinate.Longitude:F6},{activity.Location.Coordinate.Latitude:F6},0</coordinates>");
                    kml.AppendLine("        </Point>");
                    kml.AppendLine("      </Placemark>");
                }
            }

            kml.AppendLine("    </Folder>");
        }

        kml.AppendLine("  </Document>");
        kml.AppendLine("</kml>");

        await File.WriteAllTextAsync(fileName, kml.ToString());
    }

    /// <inheritdoc/>
    public async Task ExportToGpxAsync(Itinerary itinerary, string fileName)
    {
        var gpx = new StringBuilder();
        gpx.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        gpx.AppendLine("<gpx version=\"1.1\" creator=\"AI Travel Buddy\"");
        gpx.AppendLine("  xmlns=\"http://www.topografix.com/GPX/1/1\"");
        gpx.AppendLine("  xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
        gpx.AppendLine("  xsi:schemaLocation=\"http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd\">");
        gpx.AppendLine($"  <metadata>");
        gpx.AppendLine($"    <name>{EscapeXml(itinerary.Title)}</name>");
        gpx.AppendLine($"    <desc>Travel itinerary for {EscapeXml(itinerary.Preferences.Destination)}</desc>");
        gpx.AppendLine($"  </metadata>");

        foreach (var dayPlan in itinerary.DayPlans)
        {
            foreach (var activity in dayPlan.Activities)
            {
                if (activity.Location?.Coordinate != null)
                {
                    gpx.AppendLine("  <wpt lat=\"" + activity.Location.Coordinate.Latitude.ToString("F6", CultureInfo.InvariantCulture) +
                                   "\" lon=\"" + activity.Location.Coordinate.Longitude.ToString("F6", CultureInfo.InvariantCulture) + "\">");
                    gpx.AppendLine($"    <name>{EscapeXml(activity.Name)}</name>");
                    gpx.AppendLine($"    <desc>Day {dayPlan.DayNumber}: {EscapeXml(activity.Description)}</desc>");
                    gpx.AppendLine("  </wpt>");
                }
            }
        }

        gpx.AppendLine("</gpx>");

        await File.WriteAllTextAsync(fileName, gpx.ToString());
    }

    /// <inheritdoc/>
    public async Task GenerateMapHtmlAsync(Itinerary itinerary, string fileName)
    {
        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html>");
        html.AppendLine("<head>");
        html.AppendLine($"  <title>{itinerary.Title}</title>");
        html.AppendLine("  <meta charset=\"utf-8\" />");
        html.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        html.AppendLine("  <link rel=\"stylesheet\" href=\"https://unpkg.com/leaflet@1.9.4/dist/leaflet.css\" />");
        html.AppendLine("  <script src=\"https://unpkg.com/leaflet@1.9.4/dist/leaflet.js\"></script>");
        html.AppendLine("  <style>");
        html.AppendLine("    body { margin: 0; padding: 0; }");
        html.AppendLine("    #map { height: 100vh; width: 100%; }");
        html.AppendLine("  </style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        html.AppendLine("  <div id=\"map\"></div>");
        html.AppendLine("  <script>");
        
        // Find center point (average of all coordinates)
        var allCoordinates = itinerary.DayPlans
            .SelectMany(d => d.Activities)
            .Where(a => a.Location?.Coordinate != null)
            .Select(a => a.Location!.Coordinate!)
            .ToList();

        if (allCoordinates.Any())
        {
            var centerLat = allCoordinates.Average(c => c.Latitude);
            var centerLon = allCoordinates.Average(c => c.Longitude);

            html.AppendLine($"    var map = L.map('map').setView([{centerLat:F6}, {centerLon:F6}], 13);");
            html.AppendLine("    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {");
            html.AppendLine("      attribution: '&copy; <a href=\"https://www.openstreetmap.org/copyright\">OpenStreetMap</a> contributors'");
            html.AppendLine("    }).addTo(map);");

            // Add markers for each location
            foreach (var dayPlan in itinerary.DayPlans)
            {
                foreach (var activity in dayPlan.Activities)
                {
                    if (activity.Location?.Coordinate != null)
                    {
                        var escapedName = activity.Name.Replace("'", "\\'").Replace("\"", "\\\"");
                        var escapedDesc = activity.Description.Replace("'", "\\'").Replace("\"", "\\\"");
                        html.AppendLine($"    L.marker([{activity.Location.Coordinate.Latitude:F6}, {activity.Location.Coordinate.Longitude:F6}])");
                        html.AppendLine($"      .addTo(map)");
                        html.AppendLine($"      .bindPopup('<b>Day {dayPlan.DayNumber}: {escapedName}</b><br>{escapedDesc}');");
                    }
                }
            }

            // Fit bounds to show all markers
            html.AppendLine("    var bounds = L.latLngBounds([");
            html.AppendLine(string.Join(",\n      ", allCoordinates.Select(c => $"[{c.Latitude:F6}, {c.Longitude:F6}]")));
            html.AppendLine("    ]);");
            html.AppendLine("    map.fitBounds(bounds);");
        }

        html.AppendLine("  </script>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        await File.WriteAllTextAsync(fileName, html.ToString());
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static string EscapeXml(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        return text
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&apos;");
    }
}
