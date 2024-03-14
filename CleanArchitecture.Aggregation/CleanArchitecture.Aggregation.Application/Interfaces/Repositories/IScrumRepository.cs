using CleanArchitecture.Aggregation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Interfaces.Repositories
{
    public interface IScrumRepository
    {
        Task<bool> AddBoard(ScrumBoard scrumBoard);

        Task<bool> AddUserToBoard(Guid boardId, UserPoker user);

        Task<bool> RemoveUserFromBoard(Guid boardId, Guid userId);

        Task<List<UserPoker>> GetUsersFromBoard(Guid boardId);

        Task<bool> UpdateUserPoint(Guid boardId, Guid userId, int point);

        Task<bool> ClearUsersPoint(Guid boardId);

        Task<bool> TogglePoints(Guid boardId, bool state);
    }
}
