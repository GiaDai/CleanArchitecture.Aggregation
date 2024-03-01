using CleanArchitecture.Aggregation.WebApi.Data;
using CleanArchitecture.Aggregation.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
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
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Generate instance id for cache key
            var instanceId = "321";
            var cacheKey = $"Games_Cache_{instanceId}";

            //Games = await _redisCacheClient.GetDbFromConfiguration().GetAsync<Game[]>(cacheKey);
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
            return Ok(new { IsFromCache, GamePaging });
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
