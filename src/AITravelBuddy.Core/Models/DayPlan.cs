namespace AITravelBuddy.Core.Models;

/// <summary>
/// Represents a single day's travel plan
/// </summary>
public class DayPlan
{
    /// <summary>
    /// Day number in the itinerary
    /// </summary>
    public int DayNumber { get; set; }

    /// <summary>
    /// Date of this day's plan
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Title or theme for the day
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Activities planned for this day
    /// </summary>
    public List<Activity> Activities { get; set; } = new();

    /// <summary>
    /// General notes or tips for the day
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Weather forecast or considerations
    /// </summary>
    public string? Weather { get; set; }

    /// <summary>
    /// Google Maps URL with all locations for this day
    /// </summary>
    public string? DayRouteMapUrl { get; set; }

    /// <summary>
    /// Total estimated cost for the day
    /// </summary>
    public decimal TotalEstimatedCost { get; set; }

    /// <summary>
    /// Total distance to be traveled during the day in kilometers
    /// </summary>
    public double TotalDistanceKm { get; set; }
}
