using AITravelBuddy.Core.Models;
using AITravelBuddy.Core.Services;

namespace AITravelBuddy.Console;

/// <summary>
/// Handles console-based user interaction for the AI Travel Buddy application.
/// </summary>
public class UserInterface
{
    private readonly IItineraryService _itineraryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserInterface"/> class.
    /// </summary>
    public UserInterface(IItineraryService itineraryService)
    {
        _itineraryService = itineraryService;
    }

    /// <summary>
    /// Runs the main application loop.
    /// </summary>
    public async Task RunAsync()
    {
        PrintWelcomeBanner();

        while (true)
        {
            PrintMainMenu();
            var choice = System.Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    await CreateNewItineraryAsync();
                    break;
                case "2":
                    await LoadSavedItineraryAsync();
                    break;
                case "3":
                    await ViewSavedItinerariesAsync();
                    break;
                case "4":
                    PrintColoredLine("\n👋 Thank you for using AI Travel Buddy! Safe travels! ✈️\n", ConsoleColor.Cyan);
                    return;
                default:
                    PrintColoredLine("❌ Invalid option. Please try again.\n", ConsoleColor.Red);
                    break;
            }
        }
    }

    private static void PrintWelcomeBanner()
    {
        System.Console.Clear();
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        System.Console.WriteLine("║                                                            ║");
        System.Console.WriteLine("║              ✈️  AI TRAVEL BUDDY ✈️                        ║");
        System.Console.WriteLine("║                                                            ║");
        System.Console.WriteLine("║       Your Personal AI-Powered Travel Companion            ║");
        System.Console.WriteLine("║                                                            ║");
        System.Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        System.Console.ResetColor();
        System.Console.WriteLine();
    }

    private static void PrintMainMenu()
    {
        System.Console.WriteLine("┌────────────────────────────────────────┐");
        System.Console.WriteLine("│           MAIN MENU                    │");
        System.Console.WriteLine("├────────────────────────────────────────┤");
        System.Console.WriteLine("│  1. 🗺️  Create New Itinerary          │");
        System.Console.WriteLine("│  2. 📂 Load Saved Itinerary           │");
        System.Console.WriteLine("│  3. 📋 View Saved Itineraries         │");
        System.Console.WriteLine("│  4. 🚪 Exit                            │");
        System.Console.WriteLine("└────────────────────────────────────────┘");
        System.Console.Write("\nSelect an option: ");
    }

    private async Task CreateNewItineraryAsync()
    {
        System.Console.WriteLine();
        PrintColoredLine("🗺️  Let's plan your trip!", ConsoleColor.Green);
        System.Console.WriteLine();

        try
        {
            var preferences = CollectTravelPreferences();
            
            PrintColoredLine("\n🤖 Generating your personalized itinerary...", ConsoleColor.Yellow);
            PrintColoredLine("This may take a moment...\n", ConsoleColor.Gray);

            var itinerary = await _itineraryService.CreateItineraryAsync(preferences);

            System.Console.WriteLine();
            DisplayItinerary(itinerary);

            await HandleItineraryActionsAsync(itinerary);
        }
        catch (Exception ex)
        {
            PrintColoredLine($"\n❌ Error: {ex.Message}\n", ConsoleColor.Red);
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }

    private static TravelPreferences CollectTravelPreferences()
    {
        System.Console.Write("🌍 Destination (city/country): ");
        var destination = System.Console.ReadLine()?.Trim() ?? "";
        while (string.IsNullOrWhiteSpace(destination))
        {
            PrintColoredLine("❌ Destination cannot be empty. Please try again.", ConsoleColor.Red);
            System.Console.Write("🌍 Destination: ");
            destination = System.Console.ReadLine()?.Trim() ?? "";
        }

        System.Console.Write("📅 Duration (number of days): ");
        int duration;
        while (!int.TryParse(System.Console.ReadLine(), out duration) || duration <= 0)
        {
            PrintColoredLine("❌ Please enter a valid number of days (greater than 0).", ConsoleColor.Red);
            System.Console.Write("📅 Duration (days): ");
        }

        System.Console.Write("💰 Budget level (budget/moderate/luxury): ");
        var budget = System.Console.ReadLine()?.Trim().ToLower() ?? "moderate";
        while (budget != "budget" && budget != "moderate" && budget != "luxury")
        {
            PrintColoredLine("❌ Please enter: budget, moderate, or luxury", ConsoleColor.Red);
            System.Console.Write("💰 Budget level: ");
            budget = System.Console.ReadLine()?.Trim().ToLower() ?? "moderate";
        }

        System.Console.Write("👥 Travel style (solo/couple/family/group): ");
        var style = System.Console.ReadLine()?.Trim().ToLower() ?? "solo";
        while (style != "solo" && style != "couple" && style != "family" && style != "group")
        {
            PrintColoredLine("❌ Please enter: solo, couple, family, or group", ConsoleColor.Red);
            System.Console.Write("👥 Travel style: ");
            style = System.Console.ReadLine()?.Trim().ToLower() ?? "solo";
        }

        System.Console.Write("🎯 Interests (comma-separated, e.g., culture, food, adventure): ");
        var interestsInput = System.Console.ReadLine()?.Trim() ?? "";
        var interests = interestsInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(i => i.Trim())
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .ToList();

        System.Console.Write("🍽️  Dietary restrictions (optional, press Enter to skip): ");
        var dietary = System.Console.ReadLine()?.Trim();

        System.Console.Write("♿ Accessibility requirements (optional, press Enter to skip): ");
        var accessibility = System.Console.ReadLine()?.Trim();

        return new TravelPreferences
        {
            Destination = destination,
            DurationDays = duration,
            BudgetLevel = budget,
            TravelStyle = style,
            Interests = interests,
            DietaryRestrictions = dietary,
            AccessibilityRequirements = accessibility
        };
    }

    private static void DisplayItinerary(Itinerary itinerary)
    {
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine("═══════════════════════════════════════════════════════════════");
        System.Console.WriteLine($"   {itinerary.Destination.ToUpper()} - YOUR TRAVEL ITINERARY");
        System.Console.WriteLine("═══════════════════════════════════════════════════════════════");
        System.Console.ResetColor();
        System.Console.WriteLine();

        if (!string.IsNullOrEmpty(itinerary.EstimatedTotalCost))
        {
            PrintColoredLine($"💰 Estimated Total Cost: {itinerary.EstimatedTotalCost}", ConsoleColor.Yellow);
            System.Console.WriteLine();
        }

        foreach (var day in itinerary.DayPlans)
        {
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine($"▶ DAY {day.DayNumber}: {day.Title}");
            System.Console.ResetColor();
            
            if (!string.IsNullOrEmpty(day.EstimatedDailyCost))
            {
                PrintColoredLine($"  Daily Budget: {day.EstimatedDailyCost}", ConsoleColor.Yellow);
            }
            System.Console.WriteLine();

            foreach (var activity in day.Activities)
            {
                PrintColoredLine($"  ⏰ {activity.Time} - {activity.Name}", ConsoleColor.White);
                System.Console.WriteLine($"     {activity.Description}");
                
                if (!string.IsNullOrEmpty(activity.Location))
                {
                    System.Console.WriteLine($"     📍 {activity.Location}");
                }
                if (!string.IsNullOrEmpty(activity.EstimatedCost))
                {
                    PrintColoredLine($"     💰 {activity.EstimatedCost}", ConsoleColor.Yellow);
                }
                if (!string.IsNullOrEmpty(activity.Tips))
                {
                    PrintColoredLine($"     💡 {activity.Tips}", ConsoleColor.Green);
                }
                System.Console.WriteLine();
            }

            if (!string.IsNullOrEmpty(day.Notes))
            {
                PrintColoredLine($"  📝 {day.Notes}", ConsoleColor.Gray);
                System.Console.WriteLine();
            }
        }

        if (!string.IsNullOrEmpty(itinerary.GeneralTips))
        {
            System.Console.WriteLine("───────────────────────────────────────────────────────────────");
            PrintColoredLine("💡 GENERAL TIPS & INSIGHTS", ConsoleColor.Green);
            System.Console.WriteLine("───────────────────────────────────────────────────────────────");
            System.Console.WriteLine(itinerary.GeneralTips);
            System.Console.WriteLine();
        }

        if (!string.IsNullOrEmpty(itinerary.TransportationInfo))
        {
            System.Console.WriteLine("───────────────────────────────────────────────────────────────");
            PrintColoredLine("🚇 TRANSPORTATION", ConsoleColor.Cyan);
            System.Console.WriteLine("───────────────────────────────────────────────────────────────");
            System.Console.WriteLine(itinerary.TransportationInfo);
            System.Console.WriteLine();
        }

        if (!string.IsNullOrEmpty(itinerary.WeatherInfo))
        {
            System.Console.WriteLine("───────────────────────────────────────────────────────────────");
            PrintColoredLine("🌤️  WEATHER & CLIMATE", ConsoleColor.Yellow);
            System.Console.WriteLine("───────────────────────────────────────────────────────────────");
            System.Console.WriteLine(itinerary.WeatherInfo);
            System.Console.WriteLine();
        }

        if (itinerary.PackingList.Any())
        {
            System.Console.WriteLine("───────────────────────────────────────────────────────────────");
            PrintColoredLine("🎒 PACKING LIST", ConsoleColor.Magenta);
            System.Console.WriteLine("───────────────────────────────────────────────────────────────");
            foreach (var item in itinerary.PackingList)
            {
                System.Console.WriteLine($"  ☐ {item}");
            }
            System.Console.WriteLine();
        }
    }

    private async Task HandleItineraryActionsAsync(Itinerary itinerary)
    {
        while (true)
        {
            System.Console.WriteLine("┌────────────────────────────────────────┐");
            System.Console.WriteLine("│  What would you like to do?            │");
            System.Console.WriteLine("├────────────────────────────────────────┤");
            System.Console.WriteLine("│  1. 💾 Save itinerary                  │");
            System.Console.WriteLine("│  2. 📄 Export to text file             │");
            System.Console.WriteLine("│  3. 🔄 Generate new itinerary          │");
            System.Console.WriteLine("│  4. ◀️  Return to main menu            │");
            System.Console.WriteLine("└────────────────────────────────────────┘");
            System.Console.Write("\nSelect an option: ");

            var choice = System.Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    await SaveItineraryAsync(itinerary);
                    return;
                case "2":
                    await ExportItineraryAsync(itinerary);
                    break;
                case "3":
                    await CreateNewItineraryAsync();
                    return;
                case "4":
                    return;
                default:
                    PrintColoredLine("❌ Invalid option. Please try again.\n", ConsoleColor.Red);
                    break;
            }
        }
    }

    private async Task SaveItineraryAsync(Itinerary itinerary)
    {
        try
        {
            await _itineraryService.SaveItineraryAsync(itinerary);
            PrintColoredLine($"\n✅ Itinerary saved successfully! (ID: {itinerary.Id})\n", ConsoleColor.Green);
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
        catch (Exception ex)
        {
            PrintColoredLine($"\n❌ Failed to save itinerary: {ex.Message}\n", ConsoleColor.Red);
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }

    private async Task ExportItineraryAsync(Itinerary itinerary)
    {
        System.Console.Write("\n📄 Enter file path to export (e.g., my-trip.txt): ");
        var filePath = System.Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(filePath))
        {
            filePath = $"{itinerary.Destination.Replace(" ", "_")}_itinerary.txt";
        }

        try
        {
            await _itineraryService.ExportToTextAsync(itinerary, filePath);
            PrintColoredLine($"\n✅ Itinerary exported to: {Path.GetFullPath(filePath)}\n", ConsoleColor.Green);
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
        catch (Exception ex)
        {
            PrintColoredLine($"\n❌ Failed to export itinerary: {ex.Message}\n", ConsoleColor.Red);
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }

    private async Task LoadSavedItineraryAsync()
    {
        System.Console.WriteLine();
        System.Console.Write("📂 Enter itinerary ID to load: ");
        var id = System.Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(id))
        {
            PrintColoredLine("\n❌ Invalid ID.\n", ConsoleColor.Red);
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
            return;
        }

        try
        {
            var itinerary = await _itineraryService.LoadItineraryAsync(id);

            if (itinerary == null)
            {
                PrintColoredLine($"\n❌ Itinerary with ID '{id}' not found.\n", ConsoleColor.Red);
                System.Console.WriteLine("Press any key to continue...");
                System.Console.ReadKey();
                return;
            }

            DisplayItinerary(itinerary);

            System.Console.WriteLine("\n┌────────────────────────────────────────┐");
            System.Console.WriteLine("│  Options:                              │");
            System.Console.WriteLine("├────────────────────────────────────────┤");
            System.Console.WriteLine("│  1. 📄 Export to text file             │");
            System.Console.WriteLine("│  2. ◀️  Return to main menu            │");
            System.Console.WriteLine("└────────────────────────────────────────┘");
            System.Console.Write("\nSelect an option: ");

            var choice = System.Console.ReadLine()?.Trim();

            if (choice == "1")
            {
                await ExportItineraryAsync(itinerary);
            }
        }
        catch (Exception ex)
        {
            PrintColoredLine($"\n❌ Error loading itinerary: {ex.Message}\n", ConsoleColor.Red);
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }

    private async Task ViewSavedItinerariesAsync()
    {
        try
        {
            var itineraries = await _itineraryService.ListItinerariesAsync();

            System.Console.WriteLine();
            PrintColoredLine("📋 Saved Itineraries", ConsoleColor.Cyan);
            System.Console.WriteLine("═══════════════════════════════════════════════════════════════");

            if (itineraries.Count == 0)
            {
                PrintColoredLine("\nNo saved itineraries found.\n", ConsoleColor.Yellow);
            }
            else
            {
                System.Console.WriteLine();
                foreach (var (Id, Destination, CreatedAt) in itineraries)
                {
                    System.Console.WriteLine($"🗺️  {Destination}");
                    System.Console.WriteLine($"   ID: {Id}");
                    System.Console.WriteLine($"   Created: {CreatedAt:MMMM dd, yyyy 'at' HH:mm}");
                    System.Console.WriteLine();
                }
            }

            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
        catch (Exception ex)
        {
            PrintColoredLine($"\n❌ Error listing itineraries: {ex.Message}\n", ConsoleColor.Red);
            System.Console.WriteLine("Press any key to continue...");
            System.Console.ReadKey();
        }
    }

    private static void PrintColoredLine(string text, ConsoleColor color)
    {
        System.Console.ForegroundColor = color;
        System.Console.WriteLine(text);
        System.Console.ResetColor();
    }
}
