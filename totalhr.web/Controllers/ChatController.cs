using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authentication.Infrastructure;
using totalhr.Shared.Models;
using totalhr.Resources;

namespace totalhr.web.Controllers
{
    public class ChatController : BaseController
    {
        private static ChatModel _chatModel;
        private IOAuthService _authService;

        public ChatController(IOAuthService authservice)
            : base(authservice)
        {
            _authService = authservice;
        }

        /// <summary>
        /// When the method is called with no arguments, just return the view
        /// When argument logOn is true, a user logged on
        /// When argument logOff is true, a user closed their browser or navigated away (log off)
        /// When argument chatMessage is specified, the user typed something in the chat
        /// </summary>
        public ActionResult Index(string user, bool? logOn, bool? logOff, string chatMessage)
        {
            try
            {
                if (_chatModel == null) _chatModel = new ChatModel();

                //trim chat history if needed
                if (_chatModel.ChatHistory.Count > 100)
                    _chatModel.ChatHistory.RemoveRange(0, 90);

                if (!Request.IsAjaxRequest())
                {
                    //first time loading
                    return View(_chatModel);
                }
                else if (logOn != null && (bool)logOn)
                {
                    //check if nickname already exists
                    if (_chatModel.Users.FirstOrDefault(u => u.NickName == user) != null)
                    {
                        throw new Exception(Error.Error_Nickname_Exists);
                    }
                    //else if (_chatModel.Users.Count > 10)
                    //{
                    //    throw new Exception("The room is full!");
                    //}
                    else
                    {
                        #region create new user and add to lobby
                        _chatModel.Users.Add(new ChatModel.ChatUser()
                        {
                            NickName = user,
                            LoggedOnTime = DateTime.Now,
                            LastPing = DateTime.Now
                        });

                        //inform lobby of new user
                        _chatModel.ChatHistory.Add(new ChatModel.ChatMessage()
                        {
                            Message = string.Format(Common.V_Chat_User_X_LoggedOn, user),
                            When = DateTime.Now
                        });
                        #endregion

                    }

                    return PartialView("Lobby", _chatModel);
                }
                else if (logOff != null && (bool)logOff)
                {
                    LogOffUser(_chatModel.Users.FirstOrDefault(u => u.NickName == user));
                    return PartialView("Lobby", _chatModel);
                }
                else
                {

                    var currentUser = _chatModel.Users.FirstOrDefault(u => u.NickName == user);

                    //remember each user's last ping time
                    if (currentUser != null) currentUser.LastPing = DateTime.Now;

                    #region remove inactive users
                    var removeThese = (from usr in _chatModel.Users let span = DateTime.Now - usr.LastPing where span.TotalSeconds > 15 select usr).ToList();

                    foreach (var usr in removeThese)
                    {
                        LogOffUser(usr);
                    }

                    #endregion

                    #region if there is a new message, append it to the chat
                    if (!string.IsNullOrEmpty(chatMessage))
                    {
                        _chatModel.ChatHistory.Add(new ChatModel.ChatMessage()
                        {
                            ByUser = currentUser,
                            Message = chatMessage,
                            When = DateTime.Now
                        });
                    }
                    #endregion

                    return PartialView("ChatHistory", _chatModel);
                }
            }
            catch (Exception ex)
            {
                //return error to AJAX function
                Response.StatusCode = 500;
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// Remove this user from the lobby and inform others that he logged off
        /// </summary>
        /// <param name="user"></param>
        public void LogOffUser(ChatModel.ChatUser user)
        {
            _chatModel.Users.Remove(user);
            _chatModel.ChatHistory.Add(new ChatModel.ChatMessage()
            {
                Message = string.Format(Common.V_Chat_User_X_LoggedOff, user.NickName),
                When = DateTime.Now
            });
        }
    }
}
