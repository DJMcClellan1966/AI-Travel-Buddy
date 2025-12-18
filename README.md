# AI Travel Buddy ✈️🗺️

**Your Personal AI-Powered Travel Itinerary Generator with GPS and Map Integration**

AI Travel Buddy is a comprehensive C# console application that leverages the power of AI to create personalized, day-by-day travel itineraries complete with **GPS coordinates**, **interactive maps**, and **route planning**. Whether you're a solo traveler, planning a family vacation, or organizing a group adventure, AI Travel Buddy has you covered!

## 🌟 Key Features

### Core Functionality
- 🤖 **AI-Powered Itinerary Generation** - Leverages OpenAI's GPT models to create intelligent, personalized travel plans
- 🗺️ **GPS Coordinates** - Every location includes real latitude/longitude coordinates
- 📍 **Google Maps Integration** - Instant access to map URLs for all locations
- 🚶 **Distance Calculations** - Automatic calculation of distances between activities using the Haversine formula
- ⏱️ **Travel Time Estimates** - Walking and transit time estimates between locations
- 📊 **Budget Tracking** - Cost estimates for activities, meals, and daily totals

### Map & GPS Features
- 🌐 **Multiple Export Formats**:
  - **KML** - For Google Earth and GPS devices
  - **GPX** - For hiking GPS units and navigation apps
  - **CSV** - Spreadsheet format with all coordinates
  - **TXT** - Beautifully formatted text reports
  - **HTML** - Interactive web maps with markers
- 🗺️ **Route Visualization** - Multi-stop Google Maps URLs for entire day routes
- 📱 **Mobile-Ready** - All map links open directly in Google Maps app

### Personalization
- 🎯 **Interest-Based Planning** - Culture, food, adventure, relaxation, nightlife, shopping, nature, and more
- 💰 **Budget Levels** - Budget, moderate, or luxury options
- 👥 **Travel Styles** - Solo, couple, family, or group accommodations
- 🥗 **Dietary Restrictions** - Vegetarian, vegan, gluten-free, and other preferences
- ♿ **Accessibility Support** - Custom accessibility requirements

### Data Management
- 💾 **Save & Load** - Persist itineraries as JSON files
- 📤 **Multiple Export Options** - Text, CSV, KML, GPX, and HTML formats
- 📋 **Itinerary Library** - View and manage all saved travel plans
- 🎨 **Colorful Console UI** - Easy-to-read, emoji-enhanced interface

## 📋 Prerequisites

- **.NET 8.0 SDK** or later ([Download here](https://dotnet.microsoft.com/download/dotnet/8.0))
- **OpenAI API Key** ([Get one here](https://platform.openai.com/api-keys))
- Internet connection for AI generation

## 🚀 Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/DJMcClellan1966/AI-Travel-Buddy.git
cd AI-Travel-Buddy
```

### 2. Configure API Key

Create an `appsettings.json` file in the root directory (or copy from the example):

```bash
cp appsettings.json.example appsettings.json
```

Edit `appsettings.json` and add your OpenAI API key:

```json
{
  "OpenAI": {
    "ApiKey": "sk-your-actual-api-key-here",
    "Model": "gpt-4o",
    "MaxTokens": 4000
  },
  "Storage": {
    "ItinerariesPath": "./itineraries",
    "ExportsPath": "./exports"
  }
}
```

### 3. Build the Application

```bash
dotnet build
```

### 4. Run the Application

```bash
dotnet run --project src/AITravelBuddy.Console/AITravelBuddy.Console.csproj
```

## 📖 Usage Guide

### Creating a New Itinerary

1. Launch the application
2. Select **"Create New Itinerary"** from the main menu
3. Provide your travel preferences:
   - **Destination**: e.g., "Tokyo, Japan"
   - **Start Date**: When your trip begins
   - **Duration**: Number of days
   - **Budget Level**: budget, moderate, or luxury
   - **Travel Style**: solo, couple, family, or group
   - **Interests**: Comma-separated (e.g., "culture, food, nature")
   - **Dietary Restrictions**: Optional
   - **Accessibility Requirements**: Optional

4. Wait while AI generates your personalized itinerary (typically 30-60 seconds)

5. Review your itinerary with:
   - Day-by-day activity schedules
   - GPS coordinates for every location
   - Google Maps links
   - Distance and travel time between activities
   - Cost estimates
   - Local tips and recommendations

### Managing Itineraries

After generating an itinerary, you can:

1. **💾 Save** - Store itinerary for later access
2. **📄 Export to Text** - Beautiful formatted text file with all details
3. **📊 Export to CSV** - Spreadsheet with GPS coordinates
4. **🗺️ Export to KML** - For Google Earth and GPS devices
5. **🗺️ Export to GPX** - For hiking GPS and navigation apps
6. **🌐 Generate HTML Map** - Interactive web map with all locations

### Sample Output

```
Day 1: Exploring Central Tokyo
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

📍 Senso-ji Temple
   ⏰ 9:00 AM
   Ancient Buddhist temple in Asakusa, Tokyo's oldest temple
   📫 2-3-1 Asakusa, Taito City, Tokyo 111-0032
   🌍 GPS: 35.7148° N, 139.7967° E
   🗺️  https://www.google.com/maps?q=35.714764,139.796738
   ⏱️  2.0 hours | 💰 $0.00 USD

📍 Nakamise Shopping Street
   ⏰ 11:00 AM
   Traditional shopping street leading to Senso-ji
   📫 1-36-3 Asakusa, Taito City, Tokyo
   🌍 GPS: 35.7119° N, 139.7965° E
   🗺️  https://www.google.com/maps?q=35.711870,139.796524
   ⏱️  1.5 hours | 💰 $30.00 USD
   🚶 0.32 km from previous (4 min)

🗺️  Full day route: https://www.google.com/maps/dir/?api=1&origin=35.714764,139.796738&destination=35.711870,139.796524...

📊 Day Summary: 5.20 km | $85.00
```

## 🏗️ Project Structure

```
AITravelBuddy/
├── src/
│   ├── AITravelBuddy.Console/          # Console UI application
│   │   ├── Program.cs                  # Entry point with DI setup
│   │   ├── UserInterface.cs            # Interactive console UI
│   │   └── AITravelBuddy.Console.csproj
│   ├── AITravelBuddy.Core/             # Core business logic
│   │   ├── Models/                     # Domain models
│   │   │   ├── GpsCoordinate.cs       # GPS coordinate model
│   │   │   ├── Location.cs            # Location with GPS data
│   │   │   ├── Activity.cs            # Activity/attraction model
│   │   │   ├── DayPlan.cs             # Single day itinerary
│   │   │   ├── Itinerary.cs           # Complete trip itinerary
│   │   │   └── TravelPreferences.cs   # User preferences
│   │   ├── Services/                   # Core services
│   │   │   ├── IItineraryService.cs   # Itinerary orchestration
│   │   │   ├── ItineraryService.cs
│   │   │   ├── IMapService.cs         # Map operations
│   │   │   └── MapService.cs          # Distance, routes, exports
│   │   ├── Interfaces/                 # Service contracts
│   │   │   ├── IAIService.cs          # AI generation contract
│   │   │   └── IStorageService.cs     # Storage contract
│   │   └── AITravelBuddy.Core.csproj
│   └── AITravelBuddy.Infrastructure/   # Infrastructure implementations
│       ├── AI/
│       │   └── OpenAIService.cs       # OpenAI API integration
│       ├── Storage/
│       │   └── FileStorageService.cs  # JSON/file operations
│       └── AITravelBuddy.Infrastructure.csproj
├── tests/
│   └── AITravelBuddy.Tests/           # Unit tests
├── appsettings.json.example           # Configuration template
├── .gitignore                         # Git ignore rules
├── LICENSE                            # MIT License
└── README.md                          # This file
```

## 🔧 Architecture

The application follows **Clean Architecture** principles:

- **Core Layer**: Contains domain models and business logic (no dependencies)
- **Infrastructure Layer**: Implements external concerns (OpenAI, file I/O)
- **Presentation Layer**: Console UI with dependency injection

### Key Design Patterns

- **Dependency Injection** - Loose coupling via interfaces
- **Repository Pattern** - Abstract data access via IStorageService
- **Service Layer** - Business logic encapsulation
- **SOLID Principles** - Clean, maintainable code design

## 🗺️ GPS & Mapping Features Explained

### Distance Calculation

Uses the **Haversine formula** to calculate great-circle distances between GPS coordinates:

```csharp
double CalculateDistance(GpsCoordinate start, GpsCoordinate end)
{
    // Returns distance in kilometers
}
```

### Map URL Generation

- **Single Location**: `https://www.google.com/maps?q=lat,lng`
- **Route with Waypoints**: Multi-stop directions with origin, destination, and waypoints

### Export Formats

#### KML (Keyhole Markup Language)
- Used by Google Earth
- Organized by day folders
- Includes names and descriptions

#### GPX (GPS Exchange Format)
- Standard GPS format
- Compatible with hiking apps
- Includes waypoint metadata

#### HTML Interactive Map
- Uses Leaflet.js library
- Shows all locations with markers
- Popup information on click
- Automatically fits bounds

## 📦 NuGet Packages

- **OpenAI** (2.8.0) - Official OpenAI .NET SDK
- **Microsoft.Extensions.Configuration** - Configuration management
- **Microsoft.Extensions.DependencyInjection** - Dependency injection
- **Microsoft.Extensions.Logging** - Logging infrastructure
- **System.Text.Json** - JSON serialization

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Development Guidelines

- Follow C# coding conventions
- Add XML documentation comments for public APIs
- Write unit tests for new features
- Update README for significant changes

## 🐛 Troubleshooting

### Common Issues

**"OpenAI API key not configured"**
- Ensure `appsettings.json` exists in the application directory
- Verify your API key is correctly entered
- Check that the file is being copied to the output directory

**"Failed to generate itinerary"**
- Check your internet connection
- Verify your OpenAI API key is valid and has available credits
- Ensure you're using a supported model (gpt-4o, gpt-4, gpt-3.5-turbo)

**No GPS coordinates in output**
- The AI occasionally may not provide coordinates for all locations
- Try regenerating the itinerary
- Verify you're using gpt-4 or gpt-4o for best results

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **OpenAI** - For the powerful GPT models
- **Leaflet** - For the interactive map visualization
- **.NET Team** - For the excellent framework and tools

## 📞 Support

For questions, issues, or suggestions:
- Open an issue on GitHub
- Check existing issues for solutions

## 🚀 Future Enhancements

Potential features for future versions:

- [ ] Real-time weather integration
- [ ] Multi-language support
- [ ] Currency conversion API integration
- [ ] Booking links for hotels and activities
- [ ] Mobile app version
- [ ] Photo recommendations from Instagram/Unsplash
- [ ] Public transport integration
- [ ] Collaborative trip planning
- [ ] AI-powered packing list optimization
- [ ] Travel insurance recommendations

---

**Happy Travels! ✈️🌍**
