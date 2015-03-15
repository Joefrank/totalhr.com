using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using totalhr.Resources;

namespace totalhr.Shared.Models
{
    public class ChatRoomInfo
    {
        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Error_ChatRoomName_Rq")]
        public string RoomName { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "Error_ChatRoomTarget_Rq")]
        public int Target { get; set; }

        public string InvitedUserIds { get; set; }

        public int UserId { get; set; }
    }
}