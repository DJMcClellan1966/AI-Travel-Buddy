namespace AITravelBuddy.Core.Models;

/// <summary>
/// Represents a complete travel itinerary
/// </summary>
public class Itinerary
{
    /// <summary>
    /// Unique identifier for the itinerary
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Title of the itinerary
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Travel preferences used to generate this itinerary
    /// </summary>
    public TravelPreferences Preferences { get; set; } = new();

    /// <summary>
    /// Day-by-day plans for the trip
    /// </summary>
    public List<DayPlan> DayPlans { get; set; } = new();

    /// <summary>
    /// When this itinerary was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// General tips for the destination
    /// </summary>
    public string? GeneralTips { get; set; }

    /// <summary>
    /// Packing list suggestions
    /// </summary>
    public List<string> PackingList { get; set; } = new();

    /// <summary>
    /// Total estimated cost for the entire trip
    /// </summary>
    public decimal TotalEstimatedCost { get; set; }

    /// <summary>
    /// Currency code for costs
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Local time zone information
    /// </summary>
    public string? TimeZone { get; set; }
}
