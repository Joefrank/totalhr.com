﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace totalhr.web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "CalendarEdit",
               url: "Calendar/{action}/{id}",
               defaults: new
               {
                   controller = "Calendar",
                   action = "Index",
                   year = DateTime.Now.Year,
                   month = DateTime.Now.Month,
                   day = UrlParameter.Optional
               });


            routes.MapRoute(
               name: "CalendarMonth",
               url: "Calendar/GetCalendarMonth/{year}/{month}/{calendarid}",
               defaults: new
               {
                   controller = "Calendar",
                   action = "GetCalendarMonth",
                   year = DateTime.Now.Year,
                   month = DateTime.Now.Month,
                   calendarid = UrlParameter.Optional
               });

            routes.MapRoute(
               name: "CalendarView",
               url: "Calendar/{action}/{year}/{month}/{day}",
               defaults: new
               {
                   controller = "Calendar",
                   action = "Index",
                   year = DateTime.Now.Year,
                   month = DateTime.Now.Month,
                   day = UrlParameter.Optional
               });
                       
        
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

           
        }
    }
}