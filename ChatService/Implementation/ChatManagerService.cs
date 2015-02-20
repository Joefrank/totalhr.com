using ChatService.Infrastructure;
using totalhr.Shared.Models;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace ChatService.Implementation
{
    public class ChatManagerService : IChatManagerService
    {
        private readonly IChatRepository _chatRepository;
        
        public ChatManagerService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public IEnumerable<ChatRoom> ListChatRooms()
        {
            return _chatRepository.GetAll();
        }

        public int CreateRoom(ChatRoomInfo room)
        {
           return _chatRepository.CreateRoom(room);
        }

        public ChatRoom GetRoom(int id)
        {
            return _chatRepository.FindBy(x => x.Id == id).FirstOrDefault();
        }
    }
}
