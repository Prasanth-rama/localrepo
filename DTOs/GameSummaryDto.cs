namespace GameStore.API.DTOs;
//DTO is a Data Transfer Object, which is used to transfer data between different layers of an application. In this case, the GameDto class is likely used to transfer game-related data between the API and the client.
public record  GameSummaryDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);
