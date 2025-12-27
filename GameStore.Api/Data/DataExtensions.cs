using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider
            .GetRequiredService<GameStoreContext>();

        dbContext.Database.Migrate();
    }

    public static void ConfigureGameStoreDb(this WebApplicationBuilder builder)
    {
        builder
            .Services.AddSqlite<GameStoreContext>(builder.Configuration.GetConnectionString("GameStoreConnection"), optionsAction:
             options => options.UseSeeding((context, _) =>
             {
                 if (!context.Set<Genre>().Any())
                 {
                     context.Set<Genre>().AddRange(
                         new Genre { Name = "Action" },
                         new Genre { Name = "Adventure" },
                         new Genre { Name = "RPG" },
                         new Genre { Name = "Strategy" },
                         new Genre { Name = "Simulation" },
                         new Genre { Name = "Humor" }
                     );
                     context.SaveChanges();
                 }
             }));
    }
}
