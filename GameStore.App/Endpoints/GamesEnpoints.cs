using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;

public static class GamesEnpoints
{
    const string GetGameEndpoint = "GetGame";

    private static List<GameDto> games =
    [
        new(1, "The Witcher 3: Wild Hunt", "RPG", 39.99m, new DateOnly(2015, 5, 19)),
        new(2, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
        new(3, "Minecraft", "Sandbox", 26.95m, new DateOnly(2011, 11, 18)),
        new(4, "Among Us", "Party", 4.99m, new DateOnly(2018, 6, 15)),
        new(5, "Hades", "Roguelike", 24.99m, new DateOnly(2020, 9, 17))
    ];

    public static void MapGamesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games");

        group.MapGet("", () => games);

        group.MapGet("/{id}", (int id) =>
        {
            var game = games.FirstOrDefault(_ => _.Id == id);

            if (game is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(game);
        })
        .WithName(GetGameEndpoint);


        group.MapPost("", (CreateGameDto request) =>
        {
            var gameDto = new GameDto(
                games.Count + 1,
                request.Name,
                request.Genre,
                request.Price,
                request.RealeaseDate);

            games.Add(gameDto);

            return Results.CreatedAtRoute(GetGameEndpoint, new { id = gameDto.Id }, gameDto);
        });

        group.MapPut("/{id}", (int id, UpdateGameDto request) =>
        {
            var gameIndex = games.FindIndex(_ => _.Id == id);

            if (gameIndex == -1)
            {
                return Results.NotFound();
            }

            games[gameIndex] = new GameDto(
                id,
                request.Name,
                request.Genre,
                request.Price,
                request.RealeaseDate);

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            var gameIndex = games.FindIndex(_ => _.Id == id);

            if (gameIndex == -1)
            {
                return Results.NotFound();
            }

            games.RemoveAt(gameIndex);

            return Results.NoContent();
        });
    }
}
