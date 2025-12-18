namespace AITravelBuddy.Core.Models;

/// <summary>
/// Represents a single activity or event in a travel itinerary.
/// </summary>
public class Activity
{
    /// <summary>
    /// Gets or sets the time of the activity.
    /// </summary>
    public required string Time { get; set; }

    /// <summary>
    /// Gets or sets the name or title of the activity.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the activity.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Gets or sets the location or address.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Gets or sets the estimated cost.
    /// </summary>
    public string? EstimatedCost { get; set; }

    /// <summary>
    /// Gets or sets the duration of the activity.
    /// </summary>
    public string? Duration { get; set; }

    /// <summary>
    /// Gets or sets special tips or notes.
    /// </summary>
    public string? Tips { get; set; }
}
