using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Domain.Entities;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories
{
    public class ScrumRepository : IScrumRepository
    {
        private readonly IRedisClient _redisCacheClient;
        private readonly IRedisDatabase _redisDatabase;
        private string _prefix = "SCRUM_";
        public ScrumRepository(
                       IRedisClient redisCacheClient
                       )
        {
            _redisCacheClient = redisCacheClient;
            _redisDatabase = _redisCacheClient.GetDefaultDatabase();
        }
        public async Task<bool> AddBoard(ScrumBoard scrumBoard)
        {
            // Add the board to the cache
            return await _redisDatabase.AddAsync($"{_prefix}{scrumBoard.Id}", scrumBoard);
        }

        public async Task<bool> AddUserToBoard(Guid boardId, UserPoker user)
        {
            // Get the board from the cache
            var board = await _redisDatabase.GetAsync<ScrumBoard>($"{_prefix}{boardId}");
            if (board == null)
            {
                return false;
            }
            // Add the user to the board
            board.Users.Add(user);
            // Update the board in the cache
            return await _redisDatabase.AddAsync($"{_prefix}{boardId}", board);
        }

        public async Task<bool> ClearUsersPoint(Guid boardId)
        {
           // Get the board from the cache
            var board = await _redisDatabase.GetAsync<ScrumBoard>($"{_prefix}{boardId}");
            if (board == null)
            {
                return false;
            }
            // Clear the points of all users
            foreach (var user in board.Users)
            {
                user.Point = 0;
            }
            // Update the board in the cache
            return await _redisDatabase.AddAsync($"{_prefix}{boardId}", board);
        }

        public async Task<List<UserPoker>> GetUsersFromBoard(Guid boardId)
        {
            // Get the board from the cache
            var board = await _redisDatabase.GetAsync<ScrumBoard>($"{_prefix}{boardId}");
            if (board == null)
            {
                return new List<UserPoker>();
            }
            // Return the users of the board
            return board.Users;
        }

        public async Task<bool> RemoveUserFromBoard(Guid boardId, Guid userId)
        {
            // Get the board from the cache
            var board = await _redisDatabase.GetAsync<ScrumBoard>($"{_prefix}{boardId}");
            if (board == null)
            {
                return false;
            }
            // Remove the user from the board
            board.Users.RemoveAll(u => u.UserId == userId);
            // Update the board in the cache
            return await _redisDatabase.AddAsync($"{_prefix}{boardId}", board);
        }

        public async Task<bool> TogglePoints(Guid boardId, bool state)
        {
            // Get the board from the cache
            var board = await _redisDatabase.GetAsync<ScrumBoard>($"{_prefix}{boardId}");
            if (board == null)
            {
                return false;
            }
            // Toggle the state of the points
            board.Users.ForEach(u => u.ShowPoint = state);
            // Update the board in the cache
            return await _redisDatabase.AddAsync($"{_prefix}{boardId}", board);
        }

        public async Task<bool> UpdateUserPoint(Guid boardId, Guid userId, int point)
        {
            // Get the board from the cache
            var board = await _redisDatabase.GetAsync<ScrumBoard>($"{_prefix}{boardId}");
            if (board == null)
            {
                return false;
            }
            // Update the point of the user
            var user = board.Users.Find(u => u.UserId == userId);
            if (user == null)
            {
                return false;
            }
            user.Point = point;
            // Update the board in the cache
            return await _redisDatabase.AddAsync($"{_prefix}{boardId}", board);
        }
    }
}
