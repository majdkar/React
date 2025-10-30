using SchoolV01.Domain.Models.Chat;
using SchoolV01.Shared.Constants.Application;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SchoolV01.Domain.Interfaces.Chat;
using System.Collections.Generic;
using System.Security.Claims;
using System.Collections.Concurrent;
using System;

namespace SchoolV01.Server.Hubs
{



    //public class CustomUserIdProvider : IUserIdProvider
    //{
    //    public string? GetUserId(HubConnectionContext connection)
    //    {
    //        // Pull user ID from query string, e.g. ?userId=abc
    //        var userId = connection.GetHttpContext()?.Request.Query["userId"];
    //        return userId;
    //    }
    //}


    public class SignalRHub : Hub
    {
        private static readonly Dictionary<string, string> UserConnections = new();


        public override Task OnConnectedAsync()
        {
            //var userId = Context.User?.Identity?.Name; // أو من Claims
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            var httpContext = Context.GetHttpContext();
            if (userId != null)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, userId);
                
                
            }
            
            if (!string.IsNullOrEmpty(userId))
            {
                UserConnections[userId] = Context.ConnectionId;
                //TrackingUsers[userId] = false;
            }
            return base.OnConnectedAsync();
        }



        public static string GetConnectionId(string userId)
        {
            //if (TrackingUsers[userId] == true)
            //{
                return UserConnections.TryGetValue(userId, out var connectionId) ? connectionId : null;
            //}
            //else
            //{
            //    return null;
            //}
        }


        //public static string GetConnectionId(string userId)
        //{
        //    return UserConnections.TryGetValue(userId, out var connectionId) ? connectionId : null;
        //}

        //public async Task OnConnectAsync(string userId)
        //{
        //    await Clients.All.SendAsync(ApplicationConstants.SignalR.ConnectUser, userId);
        //} 


        //public async Task OnConnectAsync(string userId)
        //{
        //    if (!string.IsNullOrEmpty(userId))
        //    {
        //        UserConnections[userId] = Context.ConnectionId;
        //    }
        //}

        public async Task OnDisconnectAsync(string userId)
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.DisconnectUser, userId);
        }

        public async Task OnChangeRolePermissions(string userId, string roleId)
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.LogoutUsersByRole, userId, roleId);
        }

        //public async Task SendMessageAsync(ChatHistory<IChatUser> chatHistory, string userName)
        //{
        //    await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveMessage, chatHistory, userName);
        //}


        public async Task SendMessageAsync(string userId, string message)
        {
            await Clients.User(userId).SendAsync(ApplicationConstants.SignalR.ReceiveMessage, message, userId);
        }


        public async Task SendTestAsync(string message, string username, string receiverUserId, string senderUserId)
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveTest, message, username, receiverUserId, senderUserId);
        }

        public async Task ChatNotificationAsync(string message, string receiverUserId, string senderUserId)
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveChatNotification, message, receiverUserId, senderUserId);
        }


        public async Task SendNotificationAsync()
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveNotification);
        }
        public async Task UpdateDashboardAsync()
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveUpdateDashboard);
        }

        public async Task UpdateMatchTableAsync()
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveUpdateMatchTable);
        }
        public async Task UpdateMPlayerTableAsync()
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveUpdateMPlayerTable);
        }

        public async Task RegenerateTokensAsync()
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveRegenerateTokens);
        }




        //====================================================

        private static ConcurrentDictionary<string, bool> TrackingUsers = new();


        public async Task StartTrack()
        {
            TrackingUsers[Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value] = true;
            await Clients.Caller.SendAsync("TrackStarted", "Tracking started.");
        }

        public async Task StopTrack()
        {
            //TrackingUsers.TryRemove(Context.UserIdentifier, out _);
            TrackingUsers[Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value] = false;
            await Clients.All.SendAsync("TrackStopped", "Tracking stopped.");
        }

        //public async Task SendLocation(double latitude, double longitude)
        //{
        //    if (TrackingUsers.TryGetValue(Context.UserIdentifier, out var isTracking) && isTracking)
        //    {
        //        // Broadcast location update to other clients or save it in DB
        //        await Clients.All.SendAsync("ReceiveLocation", Context.UserIdentifier, latitude, longitude);
        //    }
        //}


        public async Task SendLocation(string vehicleId, double latitude, double longitude)
        {
            await Clients.Group(vehicleId).SendAsync("ReceiveLocation", vehicleId, latitude, longitude);
        }

        //public override async Task OnConnectedAsync()
        //{
        //    var httpContext = Context.GetHttpContext();
        //    var vehicleId = httpContext.Request.Query["vehicleId"];
        //    await Groups.AddToGroupAsync(Context.ConnectionId, vehicleId);
        //    await base.OnConnectedAsync();
        //}

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var vehicleId = httpContext.Request.Query["vehicleId"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, vehicleId);
            await base.OnDisconnectedAsync(exception);
        }


        public async Task StartTracking(string vehicleId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, vehicleId);
            await Clients.Group(vehicleId).SendAsync("TrackingStarted", vehicleId);
        }

        public async Task StopTracking(string vehicleId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, vehicleId);
            await Clients.Group(vehicleId).SendAsync("TrackingStopped", vehicleId);
        }

        //public async Task SendLocation(string vehicleId, double latitude, double longitude)
        //{
        //    await Clients.Group(vehicleId).SendAsync("ReceiveLocation", vehicleId, latitude, longitude);
        //}


    }
}