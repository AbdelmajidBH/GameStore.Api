using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.Services
    .AddDbContext<GameStoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("GameStoreConnection")));

var app = builder.Build();

app.MapGamesEndpoints();

await app.RunAsync();
