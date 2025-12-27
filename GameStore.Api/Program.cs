using GameStore.Api.Data;
using GameStore.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

builder.ConfigureGameStoreDb();

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDatabase();

await app.RunAsync();
