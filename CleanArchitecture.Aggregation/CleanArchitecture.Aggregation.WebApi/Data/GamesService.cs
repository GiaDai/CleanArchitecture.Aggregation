using CleanArchitecture.Aggregation.WebApi.Models;
using System.IO;
using System.Text.Json;

namespace CleanArchitecture.Aggregation.WebApi.Data
{
    public class GamesService
    {
        public Game[] LoadGames()
        {
            using var streamReader = new StreamReader("Data/GamesData.json");
            var gamesData = streamReader.ReadToEnd();

            var games = JsonSerializer.Deserialize<Game[]>(gamesData);

            return games;
        }
    }
}
