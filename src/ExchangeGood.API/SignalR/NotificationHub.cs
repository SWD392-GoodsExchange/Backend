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
        public NotificationHub(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string recipientId) {
            // CreateNewNotification createNewNotification
            var feId = Context.User.GetFeID();
            await Clients.User(recipientId).SendAsync($"{feId} send noti","Hello");
            // if(feId == createNewNotification.RecipientId) {
            //     throw new HubException("User cannot send notification to yourself");
            // }
            
            // var sender = await _memberService.GetMemberByFeId(feId);
            // var recipient = await _memberService.GetMemberByFeId(createNewNotification.RecipientId);
            // var notification = new Notification {
            //     Sender = sender,
            //     Recipient = recipient,
            //     SenderUsername = sender.UserName,
            //     RecipientUsername = recipient.UserName,
            //     OnwerProductId = createNewNotification.OnwerProductId,
            //     ExchangerProductIds = createNewNotification.ExchangerProductIds,
            //     Content = createNewNotification.Content,
            // };

            // if(await _memberService.AddNotification(notification)) {
            //     await Clients.User(createNewNotification.RecipientId).SendAsync("NewNotification", _mapper.Map<NotificationDto>(notification));
            // }
        }
    }
}
