using AITravelBuddy.Core.Models;
using AITravelBuddy.Core.Services;
using AITravelBuddy.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace AITravelBuddy.Tests;

public class ItineraryServiceTests
{
    [Fact]
    public async Task CreateItineraryAsync_ShouldCallAIService()
    {
        // Arrange
        var mockAIService = new Mock<IAIService>();
        var mockStorageService = new Mock<IStorageService>();
        var mockLogger = new Mock<ILogger<ItineraryService>>();

        var preferences = new TravelPreferences
        {
            Destination = "Paris, France",
            DurationDays = 3,
            BudgetLevel = "moderate",
            TravelStyle = "couple",
            Interests = new List<string> { "culture", "food" }
        };

        var expectedItinerary = new Itinerary
        {
            Destination = "Paris, France",
            Preferences = preferences
        };

        mockAIService
            .Setup(x => x.GenerateItineraryAsync(preferences, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedItinerary);

        var service = new ItineraryService(mockAIService.Object, mockStorageService.Object, mockLogger.Object);

        // Act
        var result = await service.CreateItineraryAsync(preferences);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Paris, France", result.Destination);
        mockAIService.Verify(x => x.GenerateItineraryAsync(preferences, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SaveItineraryAsync_ShouldCallStorageService()
    {
        // Arrange
        var mockAIService = new Mock<IAIService>();
        var mockStorageService = new Mock<IStorageService>();
        var mockLogger = new Mock<ILogger<ItineraryService>>();

        var itinerary = new Itinerary
        {
            Destination = "Tokyo, Japan",
            Preferences = new TravelPreferences
            {
                Destination = "Tokyo, Japan",
                DurationDays = 5,
                BudgetLevel = "moderate",
                TravelStyle = "solo"
            }
        };

        var service = new ItineraryService(mockAIService.Object, mockStorageService.Object, mockLogger.Object);

        // Act
        await service.SaveItineraryAsync(itinerary);

        // Assert
        mockStorageService.Verify(x => x.SaveItineraryAsync(itinerary, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task LoadItineraryAsync_ShouldReturnItineraryFromStorage()
    {
        // Arrange
        var mockAIService = new Mock<IAIService>();
        var mockStorageService = new Mock<IStorageService>();
        var mockLogger = new Mock<ILogger<ItineraryService>>();

        var expectedItinerary = new Itinerary
        {
            Id = "test-id",
            Destination = "London, UK",
            Preferences = new TravelPreferences
            {
                Destination = "London, UK",
                DurationDays = 4,
                BudgetLevel = "luxury",
                TravelStyle = "couple"
            }
        };

        mockStorageService
            .Setup(x => x.LoadItineraryAsync("test-id", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedItinerary);

        var service = new ItineraryService(mockAIService.Object, mockStorageService.Object, mockLogger.Object);

        // Act
        var result = await service.LoadItineraryAsync("test-id");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test-id", result.Id);
        Assert.Equal("London, UK", result.Destination);
        mockStorageService.Verify(x => x.LoadItineraryAsync("test-id", It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class ModelTests
{
    [Fact]
    public void TravelPreferences_ShouldBeCreatedWithRequiredProperties()
    {
        // Arrange & Act
        var preferences = new TravelPreferences
        {
            Destination = "Barcelona, Spain",
            DurationDays = 7,
            BudgetLevel = "budget",
            TravelStyle = "family"
        };

        // Assert
        Assert.Equal("Barcelona, Spain", preferences.Destination);
        Assert.Equal(7, preferences.DurationDays);
        Assert.Equal("budget", preferences.BudgetLevel);
        Assert.Equal("family", preferences.TravelStyle);
        Assert.NotNull(preferences.Interests);
        Assert.Empty(preferences.Interests);
    }

    [Fact]
    public void Itinerary_ShouldGenerateUniqueId()
    {
        // Arrange & Act
        var itinerary1 = new Itinerary
        {
            Destination = "Test1",
            Preferences = new TravelPreferences
            {
                Destination = "Test1",
                DurationDays = 1,
                BudgetLevel = "budget",
                TravelStyle = "solo"
            }
        };

        var itinerary2 = new Itinerary
        {
            Destination = "Test2",
            Preferences = new TravelPreferences
            {
                Destination = "Test2",
                DurationDays = 1,
                BudgetLevel = "budget",
                TravelStyle = "solo"
            }
        };

        // Assert
        Assert.NotEqual(itinerary1.Id, itinerary2.Id);
        Assert.False(string.IsNullOrEmpty(itinerary1.Id));
        Assert.False(string.IsNullOrEmpty(itinerary2.Id));
    }

    [Fact]
    public void DayPlan_ShouldInitializeWithEmptyActivities()
    {
        // Arrange & Act
        var dayPlan = new DayPlan
        {
            DayNumber = 1,
            Title = "Test Day"
        };

        // Assert
        Assert.Equal(1, dayPlan.DayNumber);
        Assert.Equal("Test Day", dayPlan.Title);
        Assert.NotNull(dayPlan.Activities);
        Assert.Empty(dayPlan.Activities);
    }

    [Fact]
    public void Activity_ShouldBeCreatedWithRequiredProperties()
    {
        // Arrange & Act
        var activity = new Activity
        {
            Time = "10:00 AM",
            Name = "Visit Museum",
            Description = "Explore the art collection"
        };

        // Assert
        Assert.Equal("10:00 AM", activity.Time);
        Assert.Equal("Visit Museum", activity.Name);
        Assert.Equal("Explore the art collection", activity.Description);
    }
}