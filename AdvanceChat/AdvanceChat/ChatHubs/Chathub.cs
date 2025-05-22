using AdvanceChat.Client.DTOs;
using AdvanceChat.Client.Models;
using AdvanceChat.Repositories;
using ChatModels;
using ChatModels.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace AdvanceChat.ChatHubs
{
    public class Chathub : Hub
    {
        private readonly IChatRepository _chatRepository;
        public Chathub(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }
        public async Task SendMessage(Chat chat)
        {
            var GroupChat = await _chatRepository.SaveChatAsync(chat);
            await Clients.All.SendAsync("ReceiveGroupMessage", GroupChat);
        }

        public async Task AddAvailableUser(AvaliableUser avaliableUser)
        {
            Console.WriteLine("AddAvailableUser called with UserId: " + avaliableUser.UserId);

            avaliableUser.ConnectionId = Context.ConnectionId;
            var availableusers =await _chatRepository.AddAvailableUserAsync(avaliableUser);
            
            Console.WriteLine("User Count: " + availableusers.Count); // Debug line
            await Clients.All.SendAsync("NotifyAllUsers", availableusers);
        }

        public async Task RemoveUserAsync(string UserId)
        {
            var availableusers = await _chatRepository.RemoveUserAsync(UserId);
            await Clients.All.SendAsync("NotifyAllUsers", availableusers);
        }

        public async Task AddIndividualChat(IndividualChat individualChat)
        {
            await _chatRepository.AddIndividualChatAsync(individualChat);
            var requestdto = new RequestChatDTO() { ReceiverId = individualChat.ReceiverId, SenderId = individualChat.SenderId };
            var getChats = await _chatRepository.GetIndividualChatsAsync(requestdto);
            var prepareindichat = new IndividualChatDTO()
            {
                SenderId = individualChat.SenderId,
                ReceiverId = individualChat.ReceiverId,
                message = individualChat.message,
                datetime = individualChat.datetime,
                ReceiverName = getChats.Where(_ => _.ReceiverId == individualChat.ReceiverId).FirstOrDefault().ReceiverName,
                SenderName = getChats.Where(_=>_.SenderId == individualChat.SenderId).FirstOrDefault().SenderName
            };
            await Clients.User(individualChat.ReceiverId).SendAsync("ReceiveIndividualMessage", prepareindichat);
        }
    }

}
