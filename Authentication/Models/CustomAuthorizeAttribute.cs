using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Authentication.Infrastructure;
using Ninject;
using Authentication.Implementation;
using System;
using log4net;
using System.Web.Routing;

namespace Authentication.Models
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public IOAuthService AuthService;
        private readonly IKernel _ninjectKernel;
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomAuthorizeAttribute));
        private ClientUser _user;

        public string Profiles { get; set; }
        public string AccessDeniedMessage { get; set; }

        public CustomAuthorizeAttribute()
        {
            _ninjectKernel = new StandardKernel();
            _ninjectKernel.Bind<IOAuthService>().To<OckAuthService>();
            AuthService =_ninjectKernel.Get<IOAuthService>();            
        }

      
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                _user = AuthService.GetClientUser();

                if (_user == null)//user must be authenticated
                    return false;

                if (_user.UserId < 1)//if user is authenticated then will have id.
                    return false;

                if (_user.Roles.Count < 1)//user must be in at least 1 role
                    return false;

                 var roles = string.IsNullOrEmpty(Roles)? null : Roles.Split(',').ToList();
                 var profiles = string.IsNullOrEmpty(Profiles)? null : Profiles.Split(',').ToList();


                return (
                    (roles == null || (_user.IsInRole(roles)))
                    &&
                    (profiles == null || _user.IsInProfile(profiles))
                    );
            }
            catch(Exception ex)
            {
                Log.Debug("Authentication failed: " + ex.Message);
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (_user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Account", action = "Login" }));
            }
            else
            {

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Error = "NotAuthorized",
                            LogOnUrl = urlHelper.Action("AccessDenied", "Error")
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                } 


                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Error", action = "AccessDenied", ModelError = AccessDeniedMessage }));
            }
        }

    }
}