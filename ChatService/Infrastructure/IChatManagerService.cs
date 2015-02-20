using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared.Models;
using totalhr.data.EF;

namespace ChatService.Infrastructure
{
    public interface IChatManagerService
    {
        IEnumerable<ChatRoom> ListChatRooms();

        int CreateRoom(ChatRoomInfo room);
    }
}
