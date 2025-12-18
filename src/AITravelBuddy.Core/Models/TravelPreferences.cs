namespace AITravelBuddy.Core.Models;

/// <summary>
/// Represents user preferences for generating a travel itinerary.
/// </summary>
public class TravelPreferences
{
    /// <summary>
    /// Gets or sets the destination city/country.
    /// </summary>
    public required string Destination { get; set; }

    /// <summary>
    /// Gets or sets the duration of the trip in days.
    /// </summary>
    public required int DurationDays { get; set; }

    /// <summary>
    /// Gets or sets the budget level (budget, moderate, luxury).
    /// </summary>
    public required string BudgetLevel { get; set; }

    /// <summary>
    /// Gets or sets the travel style (solo, couple, family, group).
    /// </summary>
    public required string TravelStyle { get; set; }

    /// <summary>
    /// Gets or sets the list of user interests.
    /// </summary>
    public List<string> Interests { get; set; } = new();

    /// <summary>
    /// Gets or sets dietary restrictions or preferences.
    /// </summary>
    public string? DietaryRestrictions { get; set; }

    /// <summary>
    /// Gets or sets accessibility requirements.
    /// </summary>
    public string? AccessibilityRequirements { get; set; }
}
