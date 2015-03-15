using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using totalhr.Resources;

namespace totalhr.Shared.Models
{
    public class ChatRoom
    {
        /// <summary>
        /// Users that have connected to the chat
        /// </summary>
        public List<ChatUser> Users;

        /// <summary>
        /// Messages by the users
        /// </summary>
        public List<ChatMessage> ChatHistory;

        public int DBid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Creator { get; set; }

        public int CreatorId { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool Private { get { return InvitedUserIds != null && InvitedUserIds.Count > 0; } }

        public List<int> InvitedUserIds { get; set; } 

        public ChatRoom()
        {
            Users = new List<ChatUser>();
            ChatHistory = new List<ChatMessage>
                {
                    new ChatMessage()
                        {
                            Message = string.Format(Common.V_Chat_Server_Started_At, DateTime.Now)
                        }
                };
        }

        public static string GetCacheKey(int id)
        {
            return string.Format("ChatRoom_{0}", id);
        }

        public class ChatUser
        {
            public string NickName;
            public int Userid;
            public DateTime LoggedOnTime;
            public DateTime LastPing;
        }

        public class ChatMessage
        {
            /// <summary>
            /// If null, the message is from the server
            /// </summary>
            public ChatUser ByUser;

            public DateTime When = DateTime.Now;

            public string Message ="";

        }

        public class AjaxPostResult
        {
            public int MessageId { get; set; }

            public string ResultMessage { get; set; }
        }

        public class ClientMessageInfo
        {
            public int RoomId { get; set; }

            public int UserId { get; set; }

            public string Nickname { get; set; }

            public string UserName { get; set; }

            public string Message { get; set; }
        }
    }

}
