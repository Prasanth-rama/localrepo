using System;
using GameStore.API.Data;
using GameStore.API.DTOs;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Endpoints;

public static class GamesEndPoints
{
    const string GetGameEndpointName = "GetGame";
    public static void MapGamesEndpoint(this WebApplication app)
    {
        var group=app.MapGroup("/games");
        group.MapGet("/", async (GameStoreContext dbContext) =>
         await dbContext.Games.
         Include(game=>game.Genre).Select(game=> new GameSummaryDto
         (
             game.Id,
             game.Name,
             game.Genre!.Name,
             game.Price,
             game.ReleaseDate

        )).AsNoTracking().ToListAsync());

        //GET /games/{id}
        group.MapGet("/{id}",async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            return game is not null ? Results.Ok(new GameDetailsDto(game.Id,game.Name,game.GenreId,game.Price,game.ReleaseDate)) : Results.NotFound();
        }).WithName(GetGameEndpointName);


        group.MapPost("/", async (CreateandUpdateGameDto createGameDto, GameStoreContext dbContext) =>
        {
            Game game = new (){
                Name=createGameDto.Name,
                GenreId=createGameDto.GenreId,
                Price=createGameDto.Price,
                ReleaseDate=createGameDto.ReleaseDate
        };
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            GameDetailsDto gameDto=new(
                game.Id,game.Name,game.GenreId,game.Price,game.ReleaseDate
            );
            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, gameDto);
        });
        group.MapPut("/{id}", async(int id, 
        CreateandUpdateGameDto updateGameDto,
        GameStoreContext dbContext) =>
        {
          var existingGame=await dbContext.Games.FindAsync(id);
            if (existingGame is null)
            {
                return Results.NotFound();
            }
            existingGame.Name=updateGameDto.Name;
            existingGame.GenreId=updateGameDto.GenreId;
            existingGame.Price=updateGameDto.Price;
            existingGame.ReleaseDate=updateGameDto.ReleaseDate;
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });
        //DELETE /games/1
        group.MapDelete("/{id}", async (int id,GameStoreContext dbContext) =>
        {
            await dbContext.Games
            .Where(g=>g.Id==id)
            .ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }

}
