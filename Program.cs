using GameStore.API.Data;
using GameStore.API.DTOs;
using GameStore.API.Endpoints;
using GameStore.API.Models;
var builder = WebApplication.CreateBuilder(args);
builder.AddGameStoreDB();
builder.Services.AddValidation();
var app = builder.Build();

app.MapGamesEndpoint();
app.MapGenresEndpoints();
app.MigrateDb();
app.Run(); 