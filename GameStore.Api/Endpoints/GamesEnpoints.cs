using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;

namespace GameStore.Api.Endpoints;

public static class GamesEnpoints
{
    const string GetGameEndpoint = "GetGame";


    public static void MapGamesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games");

        group.MapGet("", (GameStoreContext context) =>
        {
            var gameDetails = context.Games.Select(game =>
            new GameDetailsDto(game.Id, game.Name, game.GenreId, game.Price, game.RealeaseDate));

            return Results.Ok(gameDetails);

        });

        group.MapGet("/{id}", (int id, GameStoreContext context) =>
        {
            var game = context.Games.FirstOrDefault(_ => _.Id == id);

            if (game is null)
            {
                return Results.NotFound();
            }

            var gameDetails = new GameDetailsDto(id, game.Name, game.GenreId, game.Price, game.RealeaseDate);

            return Results.Ok(gameDetails);
        })
        .WithName(GetGameEndpoint);


        group.MapPost("", async (CreateGameDto request, GameStoreContext context) =>
        {
            var game = new Game
            {
                Name = request.Name,
                GenreId = request.GenreId,
                Price = request.Price,
                RealeaseDate = request.RealeaseDate
            };

            context.Games.Add(game);

            await context.SaveChangesAsync();

            var addedgame = new GameDetailsDto
            (
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.RealeaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpoint, new { id = addedgame.Id }, addedgame);
        });

        group.MapPut("/{id}", async (int id, UpdateGameDto request, GameStoreContext context) =>
        {
            var gameIndex = context.Games.Find(id);

            if (gameIndex is null)
            {
                return Results.NotFound();
            }

            gameIndex.Name = request.Name;
            gameIndex.GenreId = request.GenreId;
            gameIndex.Price = request.Price;
            gameIndex.RealeaseDate = request.RealeaseDate;

            context.Games.Add(gameIndex);

            await context.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, GameStoreContext context) =>
        {
            var gameIndex = context.Games.Find(id);

            if (gameIndex is null)
            {
                return Results.NotFound();
            }

            context.Games.Remove(gameIndex);

            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
