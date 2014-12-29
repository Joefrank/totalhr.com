using System.Web.Mvc;

namespace totalhr.web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default_Short",
                "Admin/",
                new { controller= "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "totalhr.web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "totalhr.web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                 "AdminSimpleRouteGuid",
                 "Admin/{controller}/{action}/Guid/{uniqueid}",
                 new
                 {
                     controller = "Home",
                     action = "Index",
                     uniqueid = System.Guid.Empty
                 },
                namespaces: new[] { "totalhr.web.Areas.Admin.Controllers" }
             );
        }
    }
}
