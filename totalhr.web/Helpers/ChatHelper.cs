using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using totalhr.data.EF;

namespace totalhr.web.Helpers
{
    public class ChatHelper
    {
        public static bool UserCanAccessChatRoom(int userId, ChatRoom room)
        {
            return !room.Private 
                || 
                (room.Private && userId == room.CreatedBy)
                ||
                (room.Private && !string.IsNullOrEmpty(room.InvitedUsers)
                && room.InvitedUsers.Split(',').Select(Int32.Parse).ToList().Contains(userId));
        }
    }
}