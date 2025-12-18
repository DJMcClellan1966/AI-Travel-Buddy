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
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// Setup dependency injection
var services = new ServiceCollection();

// Add logging
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Warning);
});

// Register configuration
services.AddSingleton<IConfiguration>(configuration);

// Register services
services.AddSingleton<IAIService, OpenAIService>();
services.AddSingleton<IStorageService, FileStorageService>();
services.AddSingleton<IMapService, MapService>();
services.AddSingleton<IItineraryService, ItineraryService>();
services.AddSingleton<UserInterface>();

// Build service provider
var serviceProvider = services.BuildServiceProvider();

// Run the application
try
{
    var ui = serviceProvider.GetRequiredService<UserInterface>();
    await ui.RunAsync();
}
catch (Exception ex)
{
    System.Console.ForegroundColor = ConsoleColor.Red;
    System.Console.WriteLine($"\n❌ Fatal error: {ex.Message}");
    System.Console.ResetColor();
    System.Console.WriteLine("\nPlease ensure:");
    System.Console.WriteLine("1. You have created an appsettings.json file with your OpenAI API key");
    System.Console.WriteLine("2. The API key is valid and has sufficient credits");
    System.Console.WriteLine("3. You have internet connectivity");
    System.Console.WriteLine("\nPress any key to exit...");
    System.Console.ReadKey();
    return 1;
}

return 0;
