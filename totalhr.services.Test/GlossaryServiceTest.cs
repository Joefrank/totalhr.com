using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using totalhr.tests.libs;
using totalhr.services.Implementation;
using totalhr.services.Infrastructure;
using totalhr.data.EF;
using totalhr.Shared;
using totalhr.Shared.Infrastructure;
using totalhr.data.Repositories.Implementation;
using totalhr.data.Repositories.Infrastructure;

namespace totalhr.services.Test
{
    [TestClass]
    public class GlossaryServiceTest : BaseTest
    {
        private IGlossaryService _glossaryService;
        private ICacheHelper _cacheHelper;
       // private IGlossaryRepository _glossaryRepos;

        public GlossaryServiceTest()
        {
            _glossaryService = ninjectKernel.Get<GlossaryService>();
            _cacheHelper = ninjectKernel.Get<TestGlossaryCacheHelper>();
            //_glossaryRepos = ninjectKernel.Get<GlossaryRepository>();
            //List<Glossary> allGlossaries = _glossaryRepos.GetAll().ToList();
            //_cacheHelper.Add(allGlossaries);
        }

        [TestMethod]
        public void CanReadGlossaryByLanguageAndGroup()
        {
            
            //List<Glossary> allGlossaries = _glossaryService.GetGlossary(1, Variables.GlossaryGroups.Language);

            //Assert.IsTrue(allGlossaries.Count > 0);
        }
    }
}
