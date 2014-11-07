using Authentication.Infrastructure;
using CompanyDocumentService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using totalhr.services.Infrastructure;

namespace totalhr.web.Controllers
{
    public class DocumentController : BaseController
    {
        private IOAuthService _authService;
        private IDocumentManager _docService;

        public DocumentController(IOAuthService authservice, IDocumentManager docManager)
            : base(authservice)
        {
            _authService = authservice;
            _docService = docManager;
        }

        public ActionResult Index()
        {
            return View(_docService.ListFoldersByUser(CurrentUser.UserId, CurrentUser.CompanyId));
        }

    }
}
