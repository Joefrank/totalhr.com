using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Authentication.Infrastructure;
using ChatService.Infrastructure;
using totalhr.Shared.Models;
using totalhr.Resources;
using totalhr.web.Models;
using totalhr.Shared;

namespace totalhr.web.Controllers
{
    public class ChatController : BaseController
    {
        private static ChatRoom _chatModel;
        private IOAuthService _authService;
        private readonly IChatManagerService _chatService;

        public ChatController(IChatManagerService chatService, IOAuthService authservice)
            : base(authservice)
        {
            _authService = authservice;
            _chatService = chatService;
        }

        public ActionResult LogUserIntoRoom(int roomId, string nickname)
        {            
            return View("Index");
        }

        public ActionResult LogOff()
        {
            return View("Index");
        }

        public ActionResult AddMessage(string chatMessage)
        {
            //get nickname from current chatmodel and userid
            return View("Index");

        }

        public ActionResult Index()
        {
            ViewBag.CurrentUserId = CurrentUser.UserId;
            return View(_chatService.ListChatRooms());
        }

        public ActionResult CreateRoom()
        {
            return View(new ChatRoomInfo { Target = (int)Variables.ChatRoomTarget.Public });
        }

        [HttpPost]
        public ActionResult CreateRoom(ChatRoomInfo room)
        {
            if (ModelState.IsValid)
            {
                //check if user has correct profile for this

                //check if name doesn't exist as we can't have 2 rooms with same name
                room.UserId = CurrentUser.UserId;

                _chatService.CreateRoom(room);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public ActionResult EnterRoom(int id)
        {
            var chatRoom = _chatService.LoadChatRoom(id);
            return View("ChatRegistration", chatRoom);
        }

        public ActionResult Register(int roomId, string nickName)
        {
            //register user in chat model
            if (string.IsNullOrEmpty(nickName))
                nickName = CurrentUser.FullName;

            var result = _chatService.LogUserIntoRoom(roomId, CurrentUser.UserId, nickName);
            var room = result.ItemObject as ChatRoom;

            if (result.Itemid > 0)
            {                
                return View("Lobby", room);
            }
            else
            {
                ViewBag.Error = result.ErrorMessage;
                return View("ChatRegistration", room);
            } 
        }

        public ActionResult RefreshMessages(int id)
        {
            var chatRoom = _chatService.LoadChatRoom(id);
            return View("ChatHistory", chatRoom);
        }

        [HttpPost]
        public JsonResult PostMessage(ChatRoom.ClientMessageInfo minfo)
        {
            var result = _chatService.AddMessage(minfo);
            return Json(result);
        } 

        /* <summary>
        /// When the method is called with no arguments, just return the view
        /// When argument logOn is true, a user logged on
        /// When argument logOff is true, a user closed their browser or navigated away (log off)
        /// When argument chatMessage is specified, the user typed something in the chat
        /// </summary>*/
        //public ActionResult PostAction(int roomId, string user, bool? logOn, bool? logOff, string chatMessage)
        //{
        //    try
        //    {
        //        _chatModel = GetChatModel(roomId);

        //        //trim chat history if needed
        //        if (_chatModel.ChatHistory.Count > 100)
        //            _chatModel.ChatHistory.RemoveRange(0, 90);

        //        if (!Request.IsAjaxRequest())
        //        {
        //            //first time loading
        //            return View(_chatModel);
        //        }
        //        else if (logOn != null && (bool)logOn)
        //        {
        //            //check if nickname already exists
        //            if (_chatModel.Users.FirstOrDefault(u => u.NickName == user) != null)
        //            {
        //                throw new Exception(Error.Error_Nickname_Exists);
        //            }
        //            //else if (_chatModel.Users.Count > 10)
        //            //{
        //            //    throw new Exception("The room is full!");
        //            //}
        //            else
        //            {
        //                #region create new user and add to lobby
        //                _chatModel.Users.Add(new ChatModel.ChatUser()
        //                {
        //                    NickName = user,
        //                    LoggedOnTime = DateTime.Now,
        //                    LastPing = DateTime.Now
        //                });

        //                //inform lobby of new user
        //                _chatModel.ChatHistory.Add(new ChatModel.ChatMessage()
        //                {
        //                    Message = string.Format(Common.V_Chat_User_X_LoggedOn, user),
        //                    When = DateTime.Now
        //                });
        //                #endregion

        //            }

        //            return PartialView("Lobby", _chatModel);
        //        }
        //        else if (logOff != null && (bool)logOff)
        //        {
        //            LogOffUser(_chatModel.Users.FirstOrDefault(u => u.NickName == user));
        //            return PartialView("Lobby", _chatModel);
        //        }
        //        else
        //        {

        //            var currentUser = _chatModel.Users.FirstOrDefault(u => u.NickName == user);

        //            //remember each user's last ping time
        //            if (currentUser != null) currentUser.LastPing = DateTime.Now;

        //            #region remove inactive users
        //            var removeThese = (from usr in _chatModel.Users let span = DateTime.Now - usr.LastPing where span.TotalSeconds > 15 select usr).ToList();

        //            foreach (var usr in removeThese)
        //            {
        //                LogOffUser(usr);
        //            }

        //            #endregion

        //            #region if there is a new message, append it to the chat
        //            if (!string.IsNullOrEmpty(chatMessage))
        //            {
        //                _chatModel.ChatHistory.Add(new ChatModel.ChatMessage()
        //                {
        //                    ByUser = currentUser,
        //                    Message = chatMessage,
        //                    When = DateTime.Now
        //                });
        //            }
        //            #endregion

        //            return PartialView("ChatHistory", _chatModel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //return error to AJAX function
        //        Response.StatusCode = 500;
        //        return Content(ex.Message);
        //    }
        //}

        /* <summary>
         Remove this user from the lobby and inform others that he logged off
         </summary>
         <param name="user"></param>*/
        //public void LogOffUser(ChatModel.ChatUser user)
        //{
        //    _chatModel.Users.Remove(user);
        //    _chatModel.ChatHistory.Add(new ChatModel.ChatMessage()
        //    {
        //        Message = string.Format(Common.V_Chat_User_X_LoggedOff, user.NickName),
        //        When = DateTime.Now
        //    });
        //}
    }
}
