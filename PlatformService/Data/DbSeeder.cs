using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class DbSeeder
    {
        public static void seedPopulation(IApplicationBuilder application, IWebHostEnvironment environment)
        {
            using (var serviceScope = application.ApplicationServices.CreateScope())
            {
                seedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), environment);
            }
        }

        private static void seedData(AppDbContext context, IWebHostEnvironment environment)
        {
            if (environment.IsProduction())
            {
                Console.WriteLine("Attempting to apply migrations");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Could not run migrations: {e.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("<-- Seeding data started -->");
                context.Platforms.AddRange(
                    new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing", Cost = "Free" }
                );
                context.SaveChanges();
                Console.WriteLine("<-- Seeding data ended -->");
            }
            else
            {
                Console.WriteLine("We already have data");
            }
        }
    }
}