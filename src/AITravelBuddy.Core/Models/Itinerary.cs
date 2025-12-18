namespace AITravelBuddy.Core.Models;

/// <summary>
/// Represents a complete travel itinerary.
/// </summary>
public class Itinerary
{
    /// <summary>
    /// Gets or sets the unique identifier for the itinerary.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the destination.
    /// </summary>
    public required string Destination { get; set; }

    /// <summary>
    /// Gets or sets when the itinerary was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the travel preferences used to generate this itinerary.
    /// </summary>
    public required TravelPreferences Preferences { get; set; }

    /// <summary>
    /// Gets or sets the list of day plans.
    /// </summary>
    public List<DayPlan> DayPlans { get; set; } = new();

    /// <summary>
    /// Gets or sets general tips for the destination.
    /// </summary>
    public string? GeneralTips { get; set; }

    /// <summary>
    /// Gets or sets packing list suggestions.
    /// </summary>
    public List<string> PackingList { get; set; } = new();

    /// <summary>
    /// Gets or sets transportation suggestions.
    /// </summary>
    public string? TransportationInfo { get; set; }

    /// <summary>
    /// Gets or sets weather information.
    /// </summary>
    public string? WeatherInfo { get; set; }

    /// <summary>
    /// Gets or sets the estimated total trip cost.
    /// </summary>
    public string? EstimatedTotalCost { get; set; }
}
