using Authentication.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.services.Infrastructure;

namespace totalhr.web.Areas.Admin.Controllers
{
    public class TimeRecordingController : totalhr.web.Controllers.TimeRecordingController
    {
        public TimeRecordingController(ITimeRecordingServices timeRecordingService, IAccountService accountService, IOAuthService authService)
            : base(timeRecordingService,accountService,authService)
        {
           
        }

    }
}
