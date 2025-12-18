namespace AITravelBuddy.Core.Models;

/// <summary>
/// Represents user's travel preferences for itinerary generation
/// </summary>
public class TravelPreferences
{
    /// <summary>
    /// Destination city or country
    /// </summary>
    public string Destination { get; set; } = string.Empty;

    /// <summary>
    /// Start date of the trip
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Duration of the trip in days
    /// </summary>
    public int DurationDays { get; set; }

    /// <summary>
    /// Budget level: budget, moderate, or luxury
    /// </summary>
    public string BudgetLevel { get; set; } = "moderate";

    /// <summary>
    /// Travel style: solo, couple, family, or group
    /// </summary>
    public string TravelStyle { get; set; } = "solo";

    /// <summary>
    /// Interests (e.g., culture, food, adventure, relaxation)
    /// </summary>
    public List<string> Interests { get; set; } = new();

    /// <summary>
    /// Dietary restrictions or preferences
    /// </summary>
    public string? DietaryRestrictions { get; set; }

    /// <summary>
    /// Accessibility requirements
    /// </summary>
    public string? AccessibilityRequirements { get; set; }

    /// <summary>
    /// Any special requests or notes
    /// </summary>
    public string? SpecialRequests { get; set; }
}
