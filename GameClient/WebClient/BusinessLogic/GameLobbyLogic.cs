using Newtonsoft.Json;
using WebClient.Models;
using WebClient.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class GameLobbyLogic : IGameLobbyLogic
{
    private readonly IHttpClientService _httpClientService;

    public GameLobbyLogic(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<IEnumerable<GameLobbyModel>> GenerateRandomGameLobbies(int amountOfLobbies)
    {
        var random = new Random();
        var gameLobbies = new List<GameLobbyModel>();

        for (int i = 0; i < amountOfLobbies; i++)
        {
            var amountOfPlayers = random.Next(1, 10);
            var lobbyPlayers = new List<PlayerModel>();

            for (int j = 0; j < random.Next(1,amountOfPlayers); j++)
            {
                PlayerModel player = new PlayerModel
                {
                    Username = $"Player{j}",
                    Password = $"password{j}",
                    InGameName = $"InGameName{j}",
                    Email = $"email{j}@example.com",
                    Birthday = DateTime.Now.AddYears(-20).AddDays(j), // Example birthday
                    Elo = random.Next(1000, 2000),
                    Banned = false,
                    CurrencyAmount = random.Next(100, 1000)
                };

                lobbyPlayers.Add(player);
            }

            gameLobbies.Add(new GameLobbyModel
            {
                GameLobbyId = random.Next(1, 1000),
                LobbyName = $"Test Lobby {random.Next(1, 1000)}",
                AmountOfPlayers = amountOfPlayers,
                lobbyOwner = lobbyPlayers.First(), // The first player is the owner
                Password = random.Next(2) == 0 ? $"password{random.Next(1, 1000)}" : null,
                InviteLink = $"http://example.com/invite{random.Next(1, 1000)}",
                LobbyChat = new LobbyChatModel { /* Initialize lobby chat model properties here */ },
                LobbyPlayers = lobbyPlayers
            });
        }

        return gameLobbies;
    }

    public async Task<GameLobbyModel> GetGameLobbyById(int lobbyId)
    {
        return new GameLobbyModel();
    }
}