using AITravelBuddy.Core.Models;
using Xunit;

namespace AITravelBuddy.Tests;

public class GpsCoordinateTests
{
    [Fact]
    public void Constructor_WithParameters_SetsProperties()
    {
        // Arrange & Act
        var coordinate = new GpsCoordinate(35.6762, 139.6503);

        // Assert
        Assert.Equal(35.6762, coordinate.Latitude);
        Assert.Equal(139.6503, coordinate.Longitude);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var coordinate = new GpsCoordinate(35.6762, 139.6503);

        // Act
        var result = coordinate.ToString();

        // Assert
        Assert.Contains("35.6762", result);
        Assert.Contains("139.6503", result);
        Assert.Contains("N", result);
        Assert.Contains("E", result);
    }

    [Fact]
    public void DefaultConstructor_InitializesWithZeroValues()
    {
        // Arrange & Act
        var coordinate = new GpsCoordinate();

        // Assert
        Assert.Equal(0, coordinate.Latitude);
        Assert.Equal(0, coordinate.Longitude);
    }
}
