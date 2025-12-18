# ✈️ AI Travel Buddy

> Your Personal AI-Powered Travel Companion

AI Travel Buddy is an intelligent console application that generates personalized travel itineraries using OpenAI's GPT models. Simply tell it where you want to go, your preferences, and budget - and let AI plan your perfect trip!

## 🌟 Features

- **🤖 AI-Powered Itinerary Generation**: Leverages OpenAI's GPT models to create detailed, personalized travel plans
- **📅 Day-by-Day Schedules**: Get structured daily plans with activities, restaurants, and attractions
- **💰 Budget-Aware Planning**: Choose from budget, moderate, or luxury travel styles
- **🎯 Interest-Based Recommendations**: Tailored suggestions based on your interests (culture, food, adventure, etc.)
- **🍽️ Dietary Considerations**: Respects dietary restrictions and preferences
- **♿ Accessibility Support**: Takes accessibility requirements into account
- **💾 Save & Load Itineraries**: Store your travel plans and access them anytime
- **📄 Export to Text**: Generate beautifully formatted text files of your itineraries
- **🎒 Packing Lists**: Get suggested packing lists based on destination and activities
- **🚇 Transportation Tips**: Receive local transportation recommendations
- **🌤️ Weather Information**: Get weather insights for your destination
- **💡 Local Insights**: Discover hidden gems and cultural tips from AI

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [OpenAI API Key](https://platform.openai.com/api-keys)

## 🚀 Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/DJMcClellan1966/AI-Travel-Buddy.git
cd AI-Travel-Buddy
```

### 2. Configure API Key

Copy the example configuration file:

```bash
cp appsettings.json.example src/AITravelBuddy.Console/appsettings.json
```

Edit `src/AITravelBuddy.Console/appsettings.json` and add your OpenAI API key:

```json
{
  "OpenAI": {
    "ApiKey": "sk-your-actual-api-key-here",
    "Model": "gpt-4",
    "MaxTokens": 4000
  },
  "Storage": {
    "ItinerariesPath": "./itineraries"
  }
}
```

**Note**: You can also use `gpt-3.5-turbo` for faster and cheaper responses, though quality may vary.

### 3. Build the Application

```bash
dotnet build
```

### 4. Run the Application

```bash
dotnet run --project src/AITravelBuddy.Console
```

## 💻 Usage

### Creating a New Itinerary

1. Select **"Create New Itinerary"** from the main menu
2. Provide your travel preferences:
   - **Destination**: Where do you want to go? (e.g., "Tokyo, Japan")
   - **Duration**: How many days? (e.g., 5)
   - **Budget Level**: budget, moderate, or luxury
   - **Travel Style**: solo, couple, family, or group
   - **Interests**: Comma-separated list (e.g., "culture, food, nature")
   - **Dietary Restrictions**: Optional (e.g., "vegetarian", "gluten-free")
   - **Accessibility Requirements**: Optional (e.g., "wheelchair accessible")
3. Wait while AI generates your personalized itinerary
4. Review your itinerary and choose to save, export, or generate a new one

### Sample User Flow

```
Welcome to AI Travel Buddy! ✈️

┌────────────────────────────────────────┐
│           MAIN MENU                    │
├────────────────────────────────────────┤
│  1. 🗺️  Create New Itinerary          │
│  2. 📂 Load Saved Itinerary           │
│  3. 📋 View Saved Itineraries         │
│  4. 🚪 Exit                            │
└────────────────────────────────────────┘

Select an option: 1

🗺️  Let's plan your trip!

🌍 Destination (city/country): Tokyo, Japan
📅 Duration (number of days): 5
💰 Budget level (budget/moderate/luxury): moderate
👥 Travel style (solo/couple/family/group): couple
🎯 Interests (comma-separated, e.g., culture, food, adventure): culture, food, nature
🍽️  Dietary restrictions (optional, press Enter to skip): vegetarian
♿ Accessibility requirements (optional, press Enter to skip): 

🤖 Generating your personalized itinerary...
This may take a moment...

[Your detailed itinerary will be displayed here]

┌────────────────────────────────────────┐
│  What would you like to do?            │
├────────────────────────────────────────┤
│  1. 💾 Save itinerary                  │
│  2. 📄 Export to text file             │
│  3. 🔄 Generate new itinerary          │
│  4. ◀️  Return to main menu            │
└────────────────────────────────────────┘
```

### Sample Output

Here's an example of what a generated itinerary looks like:

```
═══════════════════════════════════════════════════════════════
   TOKYO, JAPAN - YOUR TRAVEL ITINERARY
═══════════════════════════════════════════════════════════════

💰 Estimated Total Cost: $800-1200

▶ DAY 1: Arrival and Shibuya Exploration
  Daily Budget: $150-200

  ⏰ 10:00 AM - Hotel Check-in and Rest
     Settle into your accommodation and freshen up after your flight.
     📍 Shibuya district
     💰 Included in accommodation
     💡 Book accommodation near Shibuya Station for convenience

  ⏰ 2:00 PM - Shibuya Crossing and Shopping
     Experience the world's busiest pedestrian crossing and explore trendy shops
     📍 Shibuya Crossing, Shibuya
     💰 Free (shopping costs vary)
     ⌛ 2-3 hours
     💡 Best photo spot is from Starbucks overlooking the crossing

  ⏰ 6:00 PM - Vegetarian Ramen Dinner
     Enjoy delicious vegetarian ramen at T's Tantan
     📍 Tokyo Station, 1 Chome-9 Marunouchi
     💰 $12-18
     💡 Try their sesame tantan men - it's their signature dish

[... more days ...]

───────────────────────────────────────────────────────────────
💡 GENERAL TIPS & INSIGHTS
───────────────────────────────────────────────────────────────
Tokyo is incredibly safe and clean. The public transportation system is 
excellent but can be overwhelming - consider getting a Suica or Pasmo card...

───────────────────────────────────────────────────────────────
🚇 TRANSPORTATION
───────────────────────────────────────────────────────────────
Tokyo's train and subway system is the best way to get around. Purchase a 
Suica or Pasmo card at any station for easy payment...

───────────────────────────────────────────────────────────────
🌤️  WEATHER & CLIMATE
───────────────────────────────────────────────────────────────
Tokyo has four distinct seasons. Spring (March-May) and Fall (September-
November) offer mild temperatures and are the most popular times to visit...

───────────────────────────────────────────────────────────────
🎒 PACKING LIST
───────────────────────────────────────────────────────────────
  ☐ Comfortable walking shoes
  ☐ Light jacket or cardigan
  ☐ Portable Wi-Fi or SIM card
  ☐ Power adapter (Type A plug)
  ☐ Reusable water bottle
  ☐ Small backpack for day trips
```

## 🏗️ Architecture

The application follows clean architecture principles with clear separation of concerns:

```
AITravelBuddy/
├── src/
│   ├── AITravelBuddy.Console/      # User interface and application entry point
│   ├── AITravelBuddy.Core/         # Domain models, interfaces, and business logic
│   └── AITravelBuddy.Infrastructure/ # External service implementations (OpenAI, File Storage)
└── tests/
    └── AITravelBuddy.Tests/        # Unit and integration tests
```

### Key Components

- **Models** (`Core/Models`): Domain entities like `Itinerary`, `DayPlan`, `Activity`, `TravelPreferences`
- **Interfaces** (`Core/Interfaces`): Contracts for AI service and storage
- **Services** (`Core/Services`): Business logic orchestration
- **Infrastructure** (`Infrastructure/AI`): OpenAI integration
- **Infrastructure** (`Infrastructure/Storage`): File-based storage with JSON serialization
- **Console UI** (`Console`): Interactive terminal interface

### Design Patterns Used

- **Dependency Injection**: Loose coupling between components
- **Repository Pattern**: Abstract data access through `IStorageService`
- **Service Layer Pattern**: Business logic encapsulation in `ItineraryService`
- **Options Pattern**: Type-safe configuration binding

## 🔧 Configuration

### OpenAI Settings

- `ApiKey`: Your OpenAI API key (required)
- `Model`: The GPT model to use (e.g., "gpt-4", "gpt-3.5-turbo")
- `MaxTokens`: Maximum response length (default: 4000)

### Storage Settings

- `ItinerariesPath`: Directory where itineraries will be saved (default: "./itineraries")

## 🧪 Testing

Run the test suite:

```bash
dotnet test
```

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. **Fork the repository**
2. **Create a feature branch** (`git checkout -b feature/amazing-feature`)
3. **Commit your changes** (`git commit -m 'Add amazing feature'`)
4. **Push to the branch** (`git push origin feature/amazing-feature`)
5. **Open a Pull Request**

### Development Guidelines

- Follow C# coding conventions
- Add XML documentation comments for public APIs
- Write unit tests for new features
- Update README if adding new features

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- OpenAI for providing the GPT API
- The .NET community for excellent libraries and tools

## 💬 Support

If you have questions or run into issues:

1. Check the [Issues](https://github.com/DJMcClellan1966/AI-Travel-Buddy/issues) page
2. Create a new issue with details about your problem
3. Provide your configuration (without API keys!) and error messages

## 🗺️ Roadmap

Future enhancements planned:

- [ ] Currency conversion estimates
- [ ] Language phrase helper
- [ ] Travel safety tips
- [ ] Budget tracking features
- [ ] Multiple itinerary comparison
- [ ] Web interface
- [ ] Mobile app
- [ ] Integration with booking services
- [ ] Collaborative trip planning

## ⚠️ Important Notes

- **API Costs**: Using OpenAI's API incurs costs. Monitor your usage at [OpenAI Platform](https://platform.openai.com/usage)
- **Rate Limits**: Be aware of OpenAI's rate limits on your API key
- **Data Privacy**: Your API key and itineraries are stored locally and never shared
- **AI Accuracy**: While AI-generated itineraries are helpful, always verify recommendations and check current information before traveling

## 📚 Additional Resources

- [OpenAI API Documentation](https://platform.openai.com/docs)
- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [C# Programming Guide](https://learn.microsoft.com/en-us/dotnet/csharp/)

---

**Happy Travels! ✈️🌍**
