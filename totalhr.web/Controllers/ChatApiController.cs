using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Http;
using System.Web.Http;
using totalhr.Shared.Models;

namespace totalhr.web.Controllers
{
    public class ChatApiController : ApiController
    {
        private static ChatRoom _chatModel;

        [HttpGet]
        public ChatRoom.ChatUser FindUser()
        {
            return new ChatRoom.ChatUser
                {
                    NickName = "Joe", 
                    Userid = 59, 
                    LoggedOnTime = DateTime.Now
                };
        }

        [HttpPost]
        public string LogUser(string nickname)
        {
            return "successfull login " + nickname;
        }

    }
}
