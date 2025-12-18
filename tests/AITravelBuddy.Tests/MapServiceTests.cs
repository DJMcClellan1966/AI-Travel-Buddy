using AITravelBuddy.Core.Models;
using AITravelBuddy.Core.Services;
using Xunit;

namespace AITravelBuddy.Tests;

public class MapServiceTests
{
    [Fact]
    public void GenerateMapUrl_ValidCoordinate_ReturnsCorrectUrl()
    {
        // Arrange
        var mapService = new MapService();
        var coordinate = new GpsCoordinate(35.7148, 139.7967);

        // Act
        var url = mapService.GenerateMapUrl(coordinate);

        // Assert
        Assert.Contains("https://www.google.com/maps?q=", url);
        Assert.Contains("35.7148", url);
        Assert.Contains("139.7967", url);
    }

    [Fact]
    public void CalculateDistance_TwoCoordinates_ReturnsCorrectDistance()
    {
        // Arrange
        var mapService = new MapService();
        var tokyo = new GpsCoordinate(35.6762, 139.6503);
        var osaka = new GpsCoordinate(34.6937, 135.5023);

        // Act
        var distance = mapService.CalculateDistance(tokyo, osaka);

        // Assert
        // Distance between Tokyo and Osaka is approximately 400km
        Assert.InRange(distance, 380, 420);
    }

    [Fact]
    public void GenerateRouteUrl_MultipleCoordinates_ReturnsValidUrl()
    {
        // Arrange
        var mapService = new MapService();
        var coordinates = new List<GpsCoordinate>
        {
            new GpsCoordinate(35.6762, 139.6503),
            new GpsCoordinate(35.6895, 139.6917),
            new GpsCoordinate(35.7148, 139.7967)
        };

        // Act
        var url = mapService.GenerateRouteUrl(coordinates);

        // Assert
        Assert.Contains("https://www.google.com/maps/dir/", url);
        Assert.Contains("origin=", url);
        Assert.Contains("destination=", url);
        Assert.Contains("waypoints=", url);
    }

    [Fact]
    public void CalculateDistance_SameCoordinate_ReturnsZero()
    {
        // Arrange
        var mapService = new MapService();
        var coordinate = new GpsCoordinate(35.6762, 139.6503);

        // Act
        var distance = mapService.CalculateDistance(coordinate, coordinate);

        // Assert
        Assert.Equal(0, distance, precision: 1);
    }
}
