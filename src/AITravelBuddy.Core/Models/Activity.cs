namespace AITravelBuddy.Core.Models;

/// <summary>
/// Represents a travel activity or event
/// </summary>
public class Activity
{
    /// <summary>
    /// Name of the activity
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the activity
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Location where the activity takes place
    /// </summary>
    public Location? Location { get; set; }

    /// <summary>
    /// Suggested duration for the activity (in hours)
    /// </summary>
    public double DurationHours { get; set; }

    /// <summary>
    /// Estimated cost for the activity
    /// </summary>
    public decimal EstimatedCost { get; set; }

    /// <summary>
    /// Currency code (e.g., USD, EUR, JPY)
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Suggested start time for the activity
    /// </summary>
    public string? SuggestedTime { get; set; }

    /// <summary>
    /// Category or type of activity
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Distance from previous activity in kilometers
    /// </summary>
    public double? DistanceFromPrevious { get; set; }

    /// <summary>
    /// Estimated travel time from previous location in minutes
    /// </summary>
    public int? TravelTimeFromPrevious { get; set; }

    /// <summary>
    /// Tips or recommendations for this activity
    /// </summary>
    public string? Tips { get; set; }
}
