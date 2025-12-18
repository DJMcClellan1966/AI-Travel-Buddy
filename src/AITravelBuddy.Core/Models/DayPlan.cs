namespace AITravelBuddy.Core.Models;

/// <summary>
/// Represents a single day's plan in a travel itinerary.
/// </summary>
public class DayPlan
{
    /// <summary>
    /// Gets or sets the day number (1-based).
    /// </summary>
    public required int DayNumber { get; set; }

    /// <summary>
    /// Gets or sets the title or theme of the day.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Gets or sets the list of activities for this day.
    /// </summary>
    public List<Activity> Activities { get; set; } = new();

    /// <summary>
    /// Gets or sets general notes for the day.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the estimated total cost for the day.
    /// </summary>
    public string? EstimatedDailyCost { get; set; }
}
