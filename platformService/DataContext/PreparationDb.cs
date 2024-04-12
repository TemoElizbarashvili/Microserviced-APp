using platformService.Models;

namespace platformService.DataContext;

public static class PreparationDb
{
    public async static Task EnsurePopulated(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        await SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>()!);
    }

    private async static Task SeedData(ApplicationDbContext context)
    {
        if (!context.Platforms.Any())
        {
            Console.WriteLine("--> Seeding Data into DataBase!");
            context.Platforms.AddRange(
                new Platform()
                {
                    Name = "Dotnet",
                    Publisher = "Microsoft",
                    Cost = "Free"
                },
                new Platform()
                {
                    Name = "Kubernetes",
                    Publisher = "Cloud Native Computing Foundation",
                    Cost = "Free"
                },
                new Platform()
                {
                    Name = "Angular",
                    Publisher = "Google",
                    Cost = "Free"
                });
            await context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("--> DataBase is Populated!");
        }
    }
}
