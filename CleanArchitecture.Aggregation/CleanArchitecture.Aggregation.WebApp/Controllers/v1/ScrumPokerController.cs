using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.WebApp.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CleanArchitecture.Aggregation.WebApp.Controllers.v1
{
    [Route("api/v{version:apiVersion}/scrum-poker")]
    public class ScrumPokerController : BaseApiController
    {
        private readonly IScrumRepository scrumRepository;
        private readonly IHubContext<ScrumBoardHub> hub;

        public ScrumPokerController(
            IScrumRepository scrumRepository, 
            IHubContext<ScrumBoardHub> hub
            )
        {
            this.scrumRepository = scrumRepository;
            this.hub = hub;
        }

        [HttpPost("boards")]
        public async Task<IActionResult> Post([FromBody] ScrumBoard scrumBoard)
        {
            var boradId = Guid.NewGuid();
            scrumBoard.Id = boradId;

            var isCreated = await scrumRepository.AddBoard(scrumBoard);
            if (isCreated)
            {
                return Ok(boradId);
            }
            return BadRequest();
        }

        // Update user point
        [HttpPost("boards/{boardId}")]
        public async Task<IActionResult> UpdateUsersPoint(Guid boardId)
        {
            var isAdded = await scrumRepository.ClearUsersPoint(boardId);
            await hub.Clients.Group(boardId.ToString()).SendAsync("UpdateUsersPoint", 
                await scrumRepository.GetUsersFromBoard(boardId));
            if (isAdded)
            {
                return Ok(isAdded);
            }
            return BadRequest();
        }

        [HttpPost("boards/{boardId}/{state}")]
        public async Task<IActionResult> UpdateUsersPointVisibility(Guid boardId, bool state)
        {
            var isToggled = await scrumRepository.TogglePoints(boardId, state);
            await hub.Clients.Group(boardId.ToString()).SendAsync("UsersAdded",
                await scrumRepository.GetUsersFromBoard(boardId));

            if (isToggled)
            {
                return Ok(isToggled);
            }
            return BadRequest();
        }

        [HttpPost("boards/{boardId}/users")]
        public async Task<IActionResult> AddUserToBoard(Guid boardId, [FromBody] UserPoker user)
        {
            user.UserId = Guid.NewGuid();
            var isAdded = await scrumRepository.AddUserToBoard(boardId, user);
            await hub.Clients.Group(boardId.ToString()).SendAsync("UsersAdded",
                               await scrumRepository.GetUsersFromBoard(boardId));
            if (isAdded)
            {
                return Ok(isAdded);
            }
            return BadRequest();
        }

        [HttpGet("boards/{boardId}/users")]
        public async Task<IActionResult> GetUsersFromBoard(Guid boardId)
        {
            var users = await scrumRepository.GetUsersFromBoard(boardId);
            if (users != null)
            {
                return Ok(users);
            }
            return BadRequest();
        }

        [HttpDelete("boards/{boardId}/users/{userId}")]
        public async Task<IActionResult> RemoveUserFromBoard(Guid boardId, Guid userId)
        {
            var isRemoved = await scrumRepository.RemoveUserFromBoard(boardId, userId);
            await hub.Clients.Group(boardId.ToString()).SendAsync("UsersAdded",
                                              await scrumRepository.GetUsersFromBoard(boardId));
            if (isRemoved)
            {
                return Ok(isRemoved);
            }
            return BadRequest();
        }
    }
}
