using System;
using MarsRover.Ports;
using MarsRover.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MarsRover.App
{
    class Program
    {
        static void Main()
        {
            // Setup Dependency Injection
            var serviceProvider = new ServiceCollection()
                .AddLogging(configure =>
                {
                    configure.AddConsole(x => x.TimestampFormat = "[HH:mm:ss.fff] ");
                })
                .AddScoped<IMissionControl, MissionControl>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            const string header = @"
        |
       / \
      / _ \
     |.o '.|
     |'._.'|
     |     |
   ,'|  |  |`.
  /  |  |  |  \
  |,-'--|--'-.| NASA MARS ROVERS PROGRAM";

            logger.LogInformation(header);

            logger.LogInformation("Please enter the commands:");

            // Read user input lines
            var command = "";
            string line;
            while(!string.IsNullOrWhiteSpace(line=Console.ReadLine()))
            {
                command += line + Environment.NewLine;
            }

            var missionControl = serviceProvider.GetService<IMissionControl>();

            try
            {
                missionControl.Execute(command);
            }
            catch (Exception e)
            {
                logger.LogError($"Error initializing mission: {e.Message}");
            }

            logger.LogInformation("Press any key to exit the process...");
            Console.ReadKey(); 

        }
    }
}
