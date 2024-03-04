using CleanArchitecture.Aggregation.WebApi.Data;
using CleanArchitecture.Aggregation.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IRedisClient _redisCacheClient;
        private IRedisDatabase _redisDatabase;
        public bool IsFromCache { get; set; } = false;
        private readonly GamesService _gamesService = new GamesService();
        public Game[]? Games { get; set; }
        public GamesController(
            GamesService gamesService,
            IRedisClient redisCacheClient
            )
        {
            _gamesService = gamesService;
            _redisCacheClient = redisCacheClient;
            _redisDatabase = _redisCacheClient.GetDefaultDatabase();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Generate instance id for cache key
            var instanceId = "321";
            var cacheKey = $"Games_Cache_{instanceId}";

            //Games = await _redisCacheClient.GetDefaultDatabase().GetAsync<Game[]>(cacheKey);
            // Redis load data sort set from cache
            Games = (await GetPaginatedFromSortedSet(cacheKey, 1, 10)).ToArray();

            if (Games == null)
            {
                Games = _gamesService.LoadGames();
                //await _redisCacheClient.GetDbFromConfiguration().AddAsync(cacheKey, Games, TimeSpan.FromSeconds(60));
                await AddToSortedSet(cacheKey, Games.ToList());
                IsFromCache = false;
            }
            else
            {
                IsFromCache = true;
            }
            var pageNumber = 2;
            var pageSize = 10;
            var GamePaging = GetPaginatedFromSortedSet(cacheKey, pageNumber, pageSize);
            //Games = _gamesService.LoadGames();
            return Ok(new { IsFromCache, Games });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Game game)
        {
            var cacheKey = $"Games_Cache_{game.Id}";
            var publishedDate = System.DateTime.Now;
            var gameDetail = new HashEntry[]
            {
                new HashEntry("Id", game.Id),
                new HashEntry("Title", game.Title),
                new HashEntry("Genre", game.Genre),
                new HashEntry("Platform", game.Platform),
                new HashEntry("ReleaseYear", game.ReleaseYear),
                new HashEntry("PublishedDate", publishedDate.ToString("yyyy-MM-ddTHH:mm:ssZ"))
            };

            await _redisCacheClient.GetDefaultDatabase().HashSetAsync(cacheKey, $"game_{game.Id}", gameDetail);
            // Thêm postId vào Sorted Set với thời gian xuất bản làm điểm số
            await _redisCacheClient.GetDefaultDatabase().SortedSetAddAsync("game_sorted_by_published_date", game.Id, publishedDate.Ticks);
            return Ok();
        }
        
        private async Task AddToSortedSet(string key, List<Game> objects)
        {
            foreach (var obj in objects)
            {
                await _redisCacheClient.GetDefaultDatabase().SortedSetAddAsync(key, obj, obj.Id);
            }
        }

        private async Task<List<Game>> GetPaginatedFromSortedSet(string key, int pageNumber, int pageSize)
        {
            var start = (pageNumber - 1) * pageSize;
            var stop = start + pageSize - 1;

            var result = await _redisCacheClient.GetDefaultDatabase().SortedSetRangeByScoreAsync<Game>(key, start, stop, Exclude.None, Order.Ascending);
            return result.ToList();
        }
    }
}
