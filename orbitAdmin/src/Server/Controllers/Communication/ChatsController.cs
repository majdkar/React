 using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Domain.Models.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SchoolV01.Domain.Interfaces.Chat;
using SchoolV01.Shared.Constants.Permission;
using SchoolV01.Shared.Constants.Application;
using Microsoft.AspNetCore.SignalR;
using SchoolV01.Server.Hubs;
using System.Collections.Concurrent;
using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Components;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Application.Interfaces.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Server.Controllers.Communication
{
    [AllowAnonymous]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IUnitOfWork<int> uow;

        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IChatService _chatService;
        //private HubConnection HubConnection { get; set; }
        public ChatsController(ICurrentUserService currentUserService, IChatService chatService, IHubContext<SignalRHub> hubContext, IUnitOfWork<int> uow)
        {
            _currentUserService = currentUserService;
            _chatService = chatService;
            _hubContext = hubContext;
            this.uow = uow;

        }

        /// <summary>
        /// Get user wise chat history
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>Status 200 OK</returns>
        //Get user wise chat history
        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetChatHistoryAsync(string contactId)
        {
            return Ok(await _chatService.GetChatHistoryAsync(_currentUserService.UserId, contactId));
        }
        /// <summary>
        /// get available users
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //get available users - sorted by date of last message if exists
        [HttpGet("users")]
        public async Task<IActionResult> GetChatUsersAsync()
        {
            return Ok(await _chatService.GetChatUsersAsync(_currentUserService.UserId));
        }

        /// <summary>
        /// Save Chat Message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Status 200 OK</returns>
        //save chat message
        //[HttpPost("Message")]
        //public async Task<IActionResult> SaveMessageAsync(ChatHistory<IChatUser> message)
        //{
        //    message.FromUserId = message.FromUserId;
        //    message.ToUserId = message.ToUserId;
        //    message.CreatedDate = DateTime.Now;
        //    string hostDomain = $"{Request.Scheme}://{Request.Host.Value}";


        //    HubConnection = new HubConnectionBuilder()
        //                        .WithUrl(hostDomain + "/signalRHub")
        //                        .Build();

        //    if (HubConnection.State == HubConnectionState.Disconnected)
        //    {
        //        await HubConnection.StartAsync();
        //    }
        //    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendMessage, message, "userName");

        //    return Ok(await _chatService.SaveMessageAsync(message));
        //}



        /// <summary>
        /// Save Chat Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userId"></param>

        /// <returns>Status 200 OK</returns>
        //save chat message
        [HttpPost("SaveTest")]
        public async Task<IActionResult> SaveTestAsync([FromBody] string message,string userId)
        {

            //string hostDomain = $"{Request.Scheme}://{Request.Host.Value}";


            //HubConnection = new HubConnectionBuilder()
            //                    .WithUrl(hostDomain + "/signalRHub")
            //                    .Build();
            //if (HubConnection.State == HubConnectionState.Disconnected)
            //{
            //    await HubConnection.StartAsync();
            //}
            //await HubConnection.SendAsync(ApplicationConstants.SignalR.SendMessage, message,userId);


            ////await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
            //return Ok(new { status = "Message sent" });


            var connectionId = SignalRHub.GetConnectionId(userId);
            if (connectionId != null)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                //await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);
               
                
                return Ok("Message sent");
            }

            return NotFound("User not connected");

        }

    }
}