using Microsoft.AspNetCore.SignalR;
using ExchangeGood.Contract.Payloads.Request.Notification;
using ExchangeGood.API.Extensions;
using ExchangeGood.Service.Interfaces;
using ExchangeGood.Data.Models;
using AutoMapper;
using ExchangeGood.Contract.DTOs;

namespace ExchangeGood.API.SignalR
{
    public class NotificationHub : Hub
    {
        private readonly IMemberService _memberService;
        private readonly IMapper _mapper;
        private readonly PresenceTracker _tracker;
        public NotificationHub(IMemberService memberService, PresenceTracker tracker, IMapper mapper)  
        {
            _memberService = memberService;
            _tracker = tracker; 
            _mapper = mapper; 
        }

        public async override Task OnConnectedAsync()
        {
            var feId = Context.User.GetFeID();
            // Add the logged user to the hub
            await _tracker.AddConnection(feId, Context.ConnectionId);
            // get notification for themself
            int unreadNotis = await _memberService.GetNumberUnreadNotificationedOfUser(feId);
            // send it when use open website -> connect to this hub
            await Clients.Caller.SendAsync("UnreadNotificationNumber", unreadNotis);
            /*await base.OnConnectedAsync();*/
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _tracker.RemoveConnection(Context.User.GetFeID(), Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(CreateNewNotification newNotification) {
            var feId = Context.User.GetFeID();
            if(feId == newNotification.RecipientId) {
                 throw new HubException("User cannot send notification to yourself");
            }

            var sender = await _memberService.GetMemberByFeId(feId);
            var recipient = await _memberService.GetMemberByFeId(newNotification.RecipientId);

            if(recipient == null) throw new HubException("Not found user");

            var notification = new Notification {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                OnwerProductId = newNotification.OnwerProductId, // product of owner
                ExchangerProductIds = newNotification.ExchangerProductIds, // products of sender
                Content = $"{sender.UserName} want to exchange goods with you",
                CreatedDate = DateTime.UtcNow,
                Type = Contract.Enum.Notification.Type.ExchangeRequest.Name
            };

            // get all connectionIds of recipientUser
            List<string> connectionIds = await _tracker.GetConnectionForUser(newNotification.RecipientId);

            if (connectionIds != null && connectionIds.Count > 0) {
                notification.DateRead = DateTime.UtcNow;   
            }
  
            if(await _memberService.AddNotification(notification)) {
                await Clients.Clients(connectionIds).SendAsync("NewNotification", _mapper.Map<NotificationDto>(notification));
            }
        }
    }
}
