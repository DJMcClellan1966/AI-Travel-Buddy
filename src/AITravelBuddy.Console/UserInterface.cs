using AITravelBuddy.Core.Interfaces;
using AITravelBuddy.Core.Models;
using AITravelBuddy.Core.Services;

namespace AITravelBuddy.Console;

/// <summary>
/// Handles console user interface interactions
/// </summary>
public class UserInterface
{
    private readonly IItineraryService _itineraryService;
    private readonly IStorageService _storageService;
    private readonly IMapService _mapService;

    public UserInterface(
        IItineraryService itineraryService,
        IStorageService storageService,
        IMapService mapService)
    {
        _itineraryService = itineraryService;
        _storageService = storageService;
        _mapService = mapService;
    }

    public async Task RunAsync()
    {
        System.Console.Clear();
        PrintHeader();

        while (true)
        {
            PrintMainMenu();
            var choice = System.Console.ReadLine();

            try
            {
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
                        PrintSuccess("\n👋 Thank you for using AI Travel Buddy! Safe travels!");
                        return;
                    default:
                        PrintError("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintError($"An error occurred: {ex.Message}");
                System.Console.WriteLine("\nPress any key to continue...");
                System.Console.ReadKey();
            }
        }
    }

    private void PrintHeader()
    {
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════╗
║                                                               ║
║            ✈️  AI Travel Buddy with GPS Integration  ✈️       ║
║                                                               ║
║          Your Personal Travel Itinerary Generator             ║
║                                                               ║
╚═══════════════════════════════════════════════════════════════╝
");
        System.Console.ResetColor();
    }

    private void PrintMainMenu()
    {
        System.Console.WriteLine();
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("Main Menu:");
        System.Console.ResetColor();
        System.Console.WriteLine("1. 🆕 Create New Itinerary");
        System.Console.WriteLine("2. 📂 Load Saved Itinerary");
        System.Console.WriteLine("3. 📋 View Saved Itineraries");
        System.Console.WriteLine("4. 🚪 Exit");
        System.Console.Write("\nEnter your choice (1-4): ");
    }

    private async Task CreateNewItineraryAsync()
    {
        System.Console.Clear();
        PrintHeader();
        
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine("Let's plan your trip! 🗺️\n");
        System.Console.ResetColor();

        var preferences = new TravelPreferences();

        System.Console.Write("Destination (city/country): ");
        preferences.Destination = System.Console.ReadLine() ?? "";

        System.Console.Write("Start date (yyyy-mm-dd): ");
        if (DateTime.TryParse(System.Console.ReadLine(), out var startDate))
            preferences.StartDate = startDate;
        else
            preferences.StartDate = DateTime.Now.AddDays(7);

        System.Console.Write("Duration (number of days): ");
        if (int.TryParse(System.Console.ReadLine(), out var days))
            preferences.DurationDays = days;
        else
            preferences.DurationDays = 3;

        System.Console.Write("Budget level (budget/moderate/luxury): ");
        preferences.BudgetLevel = System.Console.ReadLine()?.ToLower() ?? "moderate";

        System.Console.Write("Travel style (solo/couple/family/group): ");
        preferences.TravelStyle = System.Console.ReadLine()?.ToLower() ?? "solo";

        System.Console.Write("Interests (comma-separated, e.g., culture,food,adventure): ");
        var interestsInput = System.Console.ReadLine();
        if (!string.IsNullOrEmpty(interestsInput))
        {
            preferences.Interests = interestsInput.Split(',')
                .Select(i => i.Trim())
                .Where(i => !string.IsNullOrEmpty(i))
                .ToList();
        }

        System.Console.Write("Dietary restrictions (optional): ");
        preferences.DietaryRestrictions = System.Console.ReadLine();

        System.Console.Write("Accessibility requirements (optional): ");
        preferences.AccessibilityRequirements = System.Console.ReadLine();

        System.Console.WriteLine();
        PrintInfo("🤖 Generating your personalized itinerary with GPS coordinates...");
        PrintInfo("This may take a minute or two...\n");

        try
        {
            var itinerary = await _itineraryService.CreateItineraryAsync(preferences);
            
            System.Console.Clear();
            PrintHeader();
            PrintSuccess("✅ Itinerary generated successfully!\n");

            DisplayItinerary(itinerary);

            await HandleItineraryActionsAsync(itinerary);
        }
        catch (Exception ex)
        {
            PrintError($"Failed to generate itinerary: {ex.Message}");
            System.Console.WriteLine("\nPress any key to continue...");
            System.Console.ReadKey();
        }
    }

    private void DisplayItinerary(Itinerary itinerary)
    {
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine($"📋 {itinerary.Title}");
        System.Console.WriteLine($"Destination: {itinerary.Preferences.Destination}");
        System.Console.WriteLine($"Duration: {itinerary.Preferences.DurationDays} days");
        System.Console.WriteLine($"Total Estimated Cost: {itinerary.TotalEstimatedCost:C} {itinerary.Currency}");
        System.Console.ResetColor();
        System.Console.WriteLine();

        foreach (var dayPlan in itinerary.DayPlans)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            System.Console.WriteLine($"Day {dayPlan.DayNumber}: {dayPlan.Title}");
            System.Console.WriteLine($"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            System.Console.ResetColor();

            foreach (var activity in dayPlan.Activities)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine($"\n📍 {activity.Name}");
                System.Console.ResetColor();
                
                if (!string.IsNullOrEmpty(activity.SuggestedTime))
                    System.Console.WriteLine($"   ⏰ {activity.SuggestedTime}");
                
                System.Console.WriteLine($"   {activity.Description}");
                
                if (activity.Location != null)
                {
                    System.Console.WriteLine($"   📫 {activity.Location.Address}");
                    if (activity.Location.Coordinate != null)
                    {
                        System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                        System.Console.WriteLine($"   🌍 GPS: {activity.Location.Coordinate}");
                        System.Console.WriteLine($"   🗺️  {activity.Location.GoogleMapsUrl}");
                        System.Console.ResetColor();
                    }
                }

                System.Console.WriteLine($"   ⏱️  {activity.DurationHours} hours | 💰 {activity.EstimatedCost:C} {activity.Currency}");

                if (activity.DistanceFromPrevious.HasValue)
                {
                    System.Console.WriteLine($"   🚶 {activity.DistanceFromPrevious.Value:F2} km from previous ({activity.TravelTimeFromPrevious} min)");
                }
            }

            if (!string.IsNullOrEmpty(dayPlan.DayRouteMapUrl))
            {
                System.Console.ForegroundColor = ConsoleColor.Magenta;
                System.Console.WriteLine($"\n🗺️  Full day route: {dayPlan.DayRouteMapUrl}");
                System.Console.ResetColor();
            }

            System.Console.WriteLine($"\n📊 Day Summary: {dayPlan.TotalDistanceKm:F2} km | {dayPlan.TotalEstimatedCost:C}\n");
        }

        if (!string.IsNullOrEmpty(itinerary.GeneralTips))
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("\n💡 General Tips:");
            System.Console.ResetColor();
            System.Console.WriteLine(itinerary.GeneralTips);
        }
    }

    private async Task HandleItineraryActionsAsync(Itinerary itinerary)
    {
        while (true)
        {
            System.Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            System.Console.WriteLine("What would you like to do?");
            System.Console.WriteLine("1. 💾 Save itinerary");
            System.Console.WriteLine("2. 📄 Export to text file");
            System.Console.WriteLine("3. 📊 Export GPS coordinates (CSV)");
            System.Console.WriteLine("4. 🗺️  Export to KML (GPS devices)");
            System.Console.WriteLine("5. 🗺️  Export to GPX (GPS devices)");
            System.Console.WriteLine("6. 🌐 Generate interactive map (HTML)");
            System.Console.WriteLine("7. 🔙 Return to main menu");
            System.Console.Write("\nEnter your choice (1-7): ");

            var choice = System.Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await _itineraryService.SaveItineraryAsync(itinerary);
                        PrintSuccess($"✅ Itinerary saved successfully! ID: {itinerary.Id}");
                        break;
                    case "2":
                        var txtFileName = $"itinerary_{itinerary.Id}.txt";
                        await _storageService.ExportToTextAsync(itinerary, txtFileName);
                        PrintSuccess($"✅ Exported to: exports/{txtFileName}");
                        break;
                    case "3":
                        var csvFileName = $"coordinates_{itinerary.Id}.csv";
                        await _storageService.ExportToCsvAsync(itinerary, csvFileName);
                        PrintSuccess($"✅ Exported to: exports/{csvFileName}");
                        break;
                    case "4":
                        var kmlFileName = Path.Combine("exports", $"route_{itinerary.Id}.kml");
                        await _mapService.ExportToKmlAsync(itinerary, kmlFileName);
                        PrintSuccess($"✅ Exported to: {kmlFileName}");
                        break;
                    case "5":
                        var gpxFileName = Path.Combine("exports", $"route_{itinerary.Id}.gpx");
                        await _mapService.ExportToGpxAsync(itinerary, gpxFileName);
                        PrintSuccess($"✅ Exported to: {gpxFileName}");
                        break;
                    case "6":
                        var htmlFileName = Path.Combine("exports", $"map_{itinerary.Id}.html");
                        await _mapService.GenerateMapHtmlAsync(itinerary, htmlFileName);
                        PrintSuccess($"✅ Generated map: {htmlFileName}");
                        PrintInfo("   Open this file in a web browser to view the interactive map!");
                        break;
                    case "7":
                        return;
                    default:
                        PrintError("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                PrintError($"Error: {ex.Message}");
            }
        }
    }

    private async Task LoadSavedItineraryAsync()
    {
        System.Console.Clear();
        PrintHeader();

        var itineraries = await _itineraryService.GetSavedItinerariesAsync();

        if (!itineraries.Any())
        {
            PrintInfo("No saved itineraries found.");
            System.Console.WriteLine("\nPress any key to continue...");
            System.Console.ReadKey();
            return;
        }

        System.Console.WriteLine("Saved Itineraries:\n");
        for (int i = 0; i < itineraries.Count; i++)
        {
            var (id, title, createdAt) = itineraries[i];
            System.Console.WriteLine($"{i + 1}. {title}");
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"   Created: {createdAt:yyyy-MM-dd HH:mm} | ID: {id}");
            System.Console.ResetColor();
        }

        System.Console.Write("\nEnter number to load (or 0 to cancel): ");
        if (int.TryParse(System.Console.ReadLine(), out var selection) && selection > 0 && selection <= itineraries.Count)
        {
            var selectedId = itineraries[selection - 1].Id;
            var itinerary = await _itineraryService.LoadItineraryAsync(selectedId);

            if (itinerary != null)
            {
                System.Console.Clear();
                PrintHeader();
                DisplayItinerary(itinerary);
                await HandleItineraryActionsAsync(itinerary);
            }
            else
            {
                PrintError("Failed to load itinerary.");
                System.Console.WriteLine("\nPress any key to continue...");
                System.Console.ReadKey();
            }
        }
    }

    private async Task ViewSavedItinerariesAsync()
    {
        System.Console.Clear();
        PrintHeader();

        var itineraries = await _itineraryService.GetSavedItinerariesAsync();

        if (!itineraries.Any())
        {
            PrintInfo("No saved itineraries found.");
        }
        else
        {
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine($"Found {itineraries.Count} saved itinerary(ies):\n");
            System.Console.ResetColor();

            foreach (var (id, title, createdAt) in itineraries)
            {
                System.Console.WriteLine($"📋 {title}");
                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                System.Console.WriteLine($"   Created: {createdAt:yyyy-MM-dd HH:mm}");
                System.Console.WriteLine($"   ID: {id}");
                System.Console.ResetColor();
                System.Console.WriteLine();
            }
        }

        System.Console.WriteLine("\nPress any key to continue...");
        System.Console.ReadKey();
    }

    private void PrintSuccess(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(message);
        System.Console.ResetColor();
    }

    private void PrintError(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine(message);
        System.Console.ResetColor();
    }

    private void PrintInfo(string message)
    {
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine(message);
        System.Console.ResetColor();
    }
}
