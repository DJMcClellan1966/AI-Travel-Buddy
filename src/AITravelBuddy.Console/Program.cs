using AITravelBuddy.Console;
using AITravelBuddy.Core.Interfaces;
using AITravelBuddy.Core.Services;
using AITravelBuddy.Infrastructure.AI;
using AITravelBuddy.Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Setup dependency injection
var services = new ServiceCollection();

// Configuration
services.Configure<OpenAISettings>(configuration.GetSection("OpenAI"));
services.Configure<StorageSettings>(configuration.GetSection("Storage"));

// Logging
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Warning); // Only show warnings and errors
});

// Services
services.AddSingleton<IAIService, OpenAIService>();
services.AddSingleton<IStorageService, FileStorageService>();
services.AddSingleton<IItineraryService, ItineraryService>();
services.AddSingleton<UserInterface>();

// Build service provider
var serviceProvider = services.BuildServiceProvider();

try
{
    // Run the application
    var ui = serviceProvider.GetRequiredService<UserInterface>();
    await ui.RunAsync();
}
catch (Exception ex)
{
    System.Console.ForegroundColor = ConsoleColor.Red;
    System.Console.WriteLine($"\n❌ Fatal error: {ex.Message}");
    System.Console.ResetColor();
    
    if (ex.InnerException != null)
    {
        System.Console.WriteLine($"   Details: {ex.InnerException.Message}");
    }
    
    System.Console.WriteLine("\nPlease check:");
    System.Console.WriteLine("1. Your appsettings.json file exists and is properly configured");
    System.Console.WriteLine("2. Your OpenAI API key is valid");
    System.Console.WriteLine("3. You have an active internet connection");
    
    return 1;
}

return 0;
