using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using totalhr.web.Controllers;

namespace totalhr.Unit.Test.ControllerTests
{
    [TestClass]
    public class CompanyDocumentControllerTest
    {
        [TestMethod]
        public void IndexTest()
        {
            var controller = new DocumentController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }
    }
}
