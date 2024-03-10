using CleanArchitecture.Aggregation.WebApp.Models;
using Microsoft.AspNetCore.SignalR;

namespace CleanArchitecture.Aggregation.WebApp.Hubs
{
    public class JoinChat: Hub
    {
        // Join chat 
        public async Task JoinChatRoom(UserConnection conn)
        {
            await Clients.All.SendAsync("ReceiveMessage","admin",$"${conn.UserName} has joined");
        }

        public async Task JoinSpecificChatRoom(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);
            await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage","admin", $"{conn.UserName} has joined the group {conn.ChatRoom}.");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }
    }
}
