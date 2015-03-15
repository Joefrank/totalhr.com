using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Shared.Models;
using TEF = totalhr.data.EF;

namespace ChatService.Infrastructure
{
    public interface IChatManagerService
    {
        IEnumerable<TEF.ChatRoom> ListChatRooms();

        int CreateRoom(ChatRoomInfo room);

        TEF.ChatRoom GetRoom(int id);

        ChatRoom LoadChatRoom(int roomId);

        ResultInfo LogUserIntoRoom(int roomId, int userid, string nickname);

        ChatRoom.AjaxPostResult AddMessage(ChatRoom.ClientMessageInfo minfo);

        void LogUserOutOfRoom(ChatRoom.ClientMessageInfo minfo);
    }
}
