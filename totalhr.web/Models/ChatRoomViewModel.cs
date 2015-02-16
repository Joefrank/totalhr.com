using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace totalhr.web.Models
{
    public class ChatRoomViewModel
    {
        public string RoomName { get; set; }

        public string Description { get; set; }

        public int Target { get; set; }

        public string InvitedUserIds { get; set; } 
    }
}