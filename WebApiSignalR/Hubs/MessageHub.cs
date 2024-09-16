using Microsoft.AspNetCore.SignalR;
using WebApiSignalR.Helpers;

namespace WebApiSignalR.Hubs
{
    public class MessageHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveConnectInfo", "User Connected");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Others.SendAsync("DisconnectInfo", "User disconnected");
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message + "'s Offer is ", FileHelper.Read());
        }

        public async Task JoinRoom(string room, string user)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
            await Clients.OthersInGroup(room).SendAsync("ReceiveJoinInfo", user);
        }

        public async Task LeaveRoom(string room, string user)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
            await Clients.OthersInGroup(room).SendAsync("RecieveLeaveInfo", user);
        }

        public async Task SendMessageRoom(string room, string user)
        {
            await Clients.OthersInGroup(room).SendAsync("ReceiveInfoRoom", user, FileHelper.Read(room));
        }

        public async Task SendWinnerMessageRoom(string room, string message)
        {
            await Clients.Group(room).SendAsync("ReceiveWinInfoRoom", message, FileHelper.Read(room));
        }
    }
}
