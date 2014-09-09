using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Authentication.Implementation;
using Authentication.Infrastructure;
using Ninject;
using log4net;
using totalhr.Shared;

namespace Authentication.Models
{
    public class ProfileCheck : AuthorizeAttribute
    {
        public IOAuthService AuthService;
        private readonly IKernel _ninjectKernel;
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomAuthorizeAttribute));
        private ClientUser _user;

        public Variables.Profiles[] RequiredProfiles { get; set; }
        public string AccessDeniedMessage { get; set; }

        public ProfileCheck(Variables.Profiles profile)
            : base()
        {
            _ninjectKernel = new StandardKernel();
            _ninjectKernel.Bind<IOAuthService>().To<OckAuthService>();
            AuthService = _ninjectKernel.Get<IOAuthService>();
            RequiredProfiles = new Variables.Profiles[] { profile };
        }

        public ProfileCheck(params Variables.Profiles[] profiles)
            : base()
        {
            _ninjectKernel = new StandardKernel();
            _ninjectKernel.Bind<IOAuthService>().To<OckAuthService>();
            AuthService = _ninjectKernel.Get<IOAuthService>();
            RequiredProfiles = profiles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                _user = AuthService.GetClientUser();

                if (_user == null || _user.UserId < 1 || _user.Profiles.Count < 1)//user must be authenticated
                    return false;
                
                return _user.IsInProfile(RequiredProfiles);
            }
            catch (Exception ex)
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
                    var urlHelper = new UrlHelper(filterContext.RequestContext);
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
