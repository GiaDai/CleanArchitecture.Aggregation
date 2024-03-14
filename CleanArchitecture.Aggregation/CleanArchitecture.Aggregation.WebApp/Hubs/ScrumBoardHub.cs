using Microsoft.AspNetCore.SignalR;

namespace CleanArchitecture.Aggregation.WebApp.Hubs
{
    public class ScrumBoardHub:Hub
    {
        public async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("ReceiveMessage", "Welcome to the Scrum Board");
        }

        public async Task SubscribeToBoard(Guid boardId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, boardId.ToString());
            await Clients.Caller.SendAsync("ReceiveMessage", $"You are now subscribed to board {boardId}");
        }
    }
}
