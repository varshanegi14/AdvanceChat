using AdvanceChat.Client.DTOs;
using AdvanceChat.Client.Models;
using AdvanceChat.Data;
using ChatModels;
using ChatModels.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;


namespace AdvanceChat.Repositories
{
    public interface IChatRepository 
    {
        public  Task<GroupChatDTO> SaveChatAsync(Chat chat);
        public Task<List<GroupChatDTO>> GetChatsAsync();
        public Task<List<AvailableUserDTO>> AddAvailableUserAsync(AvaliableUser availableUser);
        public Task<List<AvailableUserDTO>> GetAvailableUserAsync();
        public Task<List<AvailableUserDTO>> RemoveUserAsync(string UserId);
        public Task<List<IndividualChatDTO>> GetIndividualChatsAsync(RequestChatDTO requestChatDTO);
        public Task AddIndividualChatAsync(IndividualChat individualChat);
        public Task<string> findNameByID(string id);
    }

    public class ChatRepository : IChatRepository
    {
        ApplicationDbContext _dbContext { get; set; }
        UserManager<AppUser> _userManager { get; set; }
        public ChatRepository(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<GroupChatDTO> SaveChatAsync(Chat chat)
        {

            var entity = (await _dbContext.chats.AddAsync(chat)).Entity;
            await _dbContext.SaveChangesAsync();
                return new GroupChatDTO(){
                    ChatId = entity.ChatId,
                    SenderId = entity.SenderId,
                    SenderName = (await _userManager.FindByIdAsync(entity.SenderId)).FullName,
                    message = entity.message
                };
        }

        public async Task<List<GroupChatDTO>> GetChatsAsync()
        {
            var list = new List<GroupChatDTO>();
            var chats = await _dbContext.chats.ToListAsync();
             foreach(var chat in chats)
            {
                list.Add(new GroupChatDTO()
                {
                    ChatId = chat.ChatId,
                    SenderId = chat.SenderId,
                    SenderName = (await _userManager.FindByIdAsync(chat.SenderId)).FullName,
                    message = chat.message
                });
            }

            return list;
          
        }

        public async Task<List<AvailableUserDTO>> AddAvailableUserAsync(AvaliableUser availableUser)
        {
            var list = new List<AvailableUserDTO>();
            var GetUser = await _dbContext.avaliableUsers.FirstOrDefaultAsync(_ => _.UserId == availableUser.UserId);
            if (GetUser != null)
            {
                GetUser.ConnectionId = availableUser.ConnectionId;
            }
            else
            {
                await _dbContext.avaliableUsers.AddAsync(availableUser);
               
            }
            await _dbContext.SaveChangesAsync();

            var allusers = await _dbContext.avaliableUsers.ToListAsync();
            foreach(var user in allusers)
            {
                list.Add(new AvailableUserDTO(
                    UserId: user.UserId,
                    ConnectionId: user.ConnectionId,
                    Fullname: (await _userManager.FindByIdAsync(user.UserId)).FullName,
                    Email: (await _userManager.FindByIdAsync(user.UserId)).Email
                    ));
            }
            return list;
        }

        public async Task<List<AvailableUserDTO>> GetAvailableUserAsync()
        {
            var list = new List<AvailableUserDTO>();
            var users = await _dbContext.avaliableUsers.ToListAsync();
            foreach(var user in users)
            {
                list.Add(new AvailableUserDTO(
                    UserId : user.UserId,
                    ConnectionId : user.ConnectionId,
                    Fullname : (await _userManager.FindByIdAsync(user.UserId)).FullName,
                    Email: (await _userManager.FindByIdAsync(user.UserId)).Email
                    ));
            }
            return list;
        }

        public async Task<List<AvailableUserDTO>> RemoveUserAsync(string UserId)
        {
            var user = await _dbContext.avaliableUsers.FirstOrDefaultAsync(_ => _.UserId == UserId);
            if (user != null)
            {
                _dbContext.avaliableUsers.Remove(user);
               await _dbContext.SaveChangesAsync();
            }
            var list = new List<AvailableUserDTO>();
            var allusers = await _dbContext.avaliableUsers.ToListAsync();
            foreach (var u in allusers)
            {
                list.Add(new AvailableUserDTO(
                    UserId: u.UserId,
                    ConnectionId: u.ConnectionId,
                    Fullname: (await _userManager.FindByIdAsync(u.UserId)).FullName,
                    Email: (await _userManager.FindByIdAsync(u.UserId)).Email
                    ));
            }
            return list;

        }
        public async Task AddIndividualChatAsync(IndividualChat individualChat)
        {
            await _dbContext.individualChats.AddAsync(individualChat);
            await _dbContext.SaveChangesAsync();

        }

        //public async Task<List<IndividualChatDTO>> GetIndividualChatsAsync(RequestChatDTO requestChatDTO)
        //{
        //    var chatlist = new List<IndividualChatDTO>();
        //    var chats = await _dbContext.individualChats.Where(s => s.SenderId == requestChatDTO.SenderId && s.ReceiverId == requestChatDTO.ReceiverId
        //                || s.SenderId == requestChatDTO.ReceiverId && s.ReceiverId == requestChatDTO.SenderId).ToListAsync();
        //    try
        //    {
        //        if (chats != null)
        //        {
        //            foreach (var chat in chats)
        //            {
        //                chatlist.Add(new IndividualChatDTO()
        //                {
        //                    Id = chat.Id,
        //                    SenderId = chat.SenderId,
        //                    ReceiverId = chat.ReceiverId,
        //                    SenderName = (await _userManager.FindByEmailAsync(chat.SenderId)).FullName,
        //                    ReceiverName = (await _userManager.FindByEmailAsync(chat.ReceiverId)).FullName,
        //                    message = chat.message,
        //                    datetime = chat.datetime
        //                });

        //            }
        //            return chatlist;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.StackTrace);
        //    }
            
        //}
        public async Task<List<IndividualChatDTO>> GetIndividualChatsAsync(RequestChatDTO requestChatDTO)
        {
            var chatList = new List<IndividualChatDTO>();

            var chats = await _dbContext.individualChats
                .Where(s =>
                    (s.SenderId == requestChatDTO.SenderId && s.ReceiverId == requestChatDTO.ReceiverId) ||
                    (s.SenderId == requestChatDTO.ReceiverId && s.ReceiverId == requestChatDTO.SenderId))
                .OrderBy(c => c.datetime) // Optional: ensure chats are ordered by time
                .ToListAsync();

            try
            {
                if (chats.Any())
                {
                    // Get all unique user IDs in the chat
                    var userIds = chats.Select(c => c.SenderId)
                                       .Union(chats.Select(c => c.ReceiverId))
                                       .Distinct()
                                       .ToList();

                    var userDict = new Dictionary<string, string>();
                    foreach (var userId in userIds)
                    {
                        var user = await _userManager.FindByEmailAsync(userId);
                        if (user != null)
                        {
                            userDict[userId] = user.FullName;
                        }
                    }

                    foreach (var chat in chats)
                    {
                        chatList.Add(new IndividualChatDTO
                        {
                            Id = chat.Id,
                            SenderId = chat.SenderId,
                            ReceiverId = chat.ReceiverId,
                            SenderName = userDict.ContainsKey(chat.SenderId) ? userDict[chat.SenderId] : "Unknown",
                            ReceiverName = userDict.ContainsKey(chat.ReceiverId) ? userDict[chat.ReceiverId] : "Unknown",
                            message = chat.message,
                            datetime = chat.datetime
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetIndividualChatsAsync] Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            return chatList;
        }


        public async Task<string> findNameByID(string id)
        {
            return (await _userManager.FindByNameAsync(id)).FullName;
        }

      
    }
}
