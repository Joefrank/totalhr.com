using ChatService.Infrastructure;
using totalhr.Shared.Models;
using TEF = totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using totalhr.Shared.Infrastructure;
using System;
using totalhr.Resources;
using Microsoft.JScript;

namespace ChatService.Implementation
{
    public class ChatManagerService : IChatManagerService
    {
        private readonly IChatRepository _chatRepository;
        private readonly ICacheHelper _cacheHelper;
        
        public ChatManagerService(IChatRepository chatRepository, ICacheHelper cacheHelper)
        {
            _chatRepository = chatRepository;
            _cacheHelper = cacheHelper;
        }

        public IEnumerable<TEF.ChatRoom> ListChatRooms()
        {
            return _chatRepository.GetAll();
        }

        public int CreateRoom(ChatRoomInfo room)
        {
           return _chatRepository.CreateRoom(room);
        }

        public TEF.ChatRoom GetRoom(int id)
        {
            return _chatRepository.FindBy(x => x.Id == id).FirstOrDefault();
        }

        public ChatRoom LoadChatRoom(int roomId)
        {
            var room = _cacheHelper.Get<ChatRoom>(ChatRoom.GetCacheKey(roomId));

            if (room == null)
            {
                var chatRoom = this.GetRoom(roomId);

                room = new ChatRoom
                {
                    DBid = chatRoom.Id,
                    Name = chatRoom.Name,
                    Description = chatRoom.Description,
                    CreatorId = chatRoom.CreatedBy,
                    CreatedOn = chatRoom.Created,
                    InvitedUserIds = chatRoom.Private? chatRoom.InvitedUsers.Split(',').Select(Int32.Parse).ToList(): null
                };

                _cacheHelper.Add(room, ChatRoom.GetCacheKey(roomId));
            }

            return room;
        }

        public ResultInfo LogUserIntoRoom(int roomId, int userid, string nickname)
        {
            var chatRoom = LoadChatRoom(roomId);

            var searchedForUser = chatRoom.Users.FirstOrDefault(u => u.NickName.ToLower().Trim() == nickname.ToLower().Trim());

            if (searchedForUser != null && searchedForUser.Userid != userid)
            {
                return new ResultInfo { Itemid = -1, ErrorMessage = Error.Error_Nickname_Exists, ItemObject = chatRoom };
            }

            if (searchedForUser == null)
            {
                var user = new ChatRoom.ChatUser()
                {
                    NickName = nickname,
                    Userid = userid,
                    LoggedOnTime = DateTime.Now,
                    LastPing = DateTime.Now
                };

                chatRoom.Users.Add(user);

                chatRoom.ChatHistory.Add(new ChatRoom.ChatMessage()
                {
                    Message = string.Format(Common.V_Chat_User_X_LoggedOn, nickname),
                    When = DateTime.Now,
                    ByUser = user
                });
                
                _cacheHelper.Update(chatRoom, ChatRoom.GetCacheKey(roomId));
            }

            return new ResultInfo { Itemid = chatRoom.DBid, ItemObject = chatRoom };
        }

        public ChatRoom.AjaxPostResult AddMessage(ChatRoom.ClientMessageInfo minfo)
        {
            var chatRoom = LoadChatRoom(minfo.RoomId);

            var searchedForUser = chatRoom.Users.FirstOrDefault(u => u.Userid == minfo.UserId);

            if (searchedForUser == null)            
            {
                searchedForUser = new ChatRoom.ChatUser()
                {
                    NickName = GlobalObject.decodeURIComponent(minfo.UserName),//get this from database 
                    Userid = minfo.UserId,
                    LoggedOnTime = DateTime.Now,
                    LastPing = DateTime.Now
                };

                chatRoom.Users.Add(searchedForUser);               
            }

            chatRoom.ChatHistory.Add(new ChatRoom.ChatMessage()
            {
                Message =  minfo.Message,
                When = DateTime.Now,
                ByUser = searchedForUser
            });

            return new ChatRoom.AjaxPostResult { MessageId = 1, ResultMessage = "" };

        }

        public void LogUserOutOfRoom(ChatRoom.ClientMessageInfo minfo)
        {
            var chatRoom = LoadChatRoom(minfo.RoomId);
            
            var userToSearch = chatRoom.Users.FirstOrDefault(x => x.Userid == minfo.UserId);

            if (userToSearch != null)
            {
                chatRoom.ChatHistory.Add(new ChatRoom.ChatMessage()
                    {
                        Message = string.Format(Common.V_Chat_User_X_LoggedOff, userToSearch.NickName),
                        When = DateTime.Now
                    });
                
                chatRoom.Users.Remove(userToSearch);
            }
        }
    }
}
