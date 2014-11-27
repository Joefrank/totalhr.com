using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using totalhr.services.Implementation;
using totalhr.services.Infrastructure;
using totalhr.data.Repositories.Implementation;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.EF;
using totalhr.dummydata;
using totalhr.Shared.Models;
using totalhr.Shared.Infrastructure;
using totalhr.Shared;
using totalhr.services.messaging.Infrastructure;
using totalhr.services.messaging.Implementation;

namespace totalhr.tests.libs
{
    public class BaseTest
    {
        protected IKernel ninjectKernel;

       
        protected AdminStruct _dummystruct = new AdminStruct
        {
            AdminEmail = "admin@test.com",
            AdminName = "Dummy Admin",
            WebsiteName = "TotalHR",
            SiteRootURL = "http://totalhr.local/test/",
            SMTPServer = "127.0.0.1",
            SMTPUser = "administrator",
            SMTPPassword = "password"

        };

        public BaseTest()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        public void AddBindings()
        {
            ninjectKernel.Bind<IGlossaryRepository>().To<GlossaryRepository>();
            ninjectKernel.Bind<ILanguageRepository>().To<LanguageRepository>();
            ninjectKernel.Bind<IUserRepository>().To<UserRepository>();
            ninjectKernel.Bind<ICompanyRepository>().To<CompanyRepository>();
            ninjectKernel.Bind<IAccountService>().To<AccountService>();
            ninjectKernel.Bind<IGlossaryService>().To<GlossaryService>();
            ninjectKernel.Bind<IMessagingService>().To<MessagingService>();
            ninjectKernel.Bind<IEmailRepository>().To<EmailTemplateRepository>();
            ninjectKernel.Bind<IEmailService>().To<EmailService>();
            ninjectKernel.Bind<ICacheHelper>().To<HttpCacheHelper>();
        }

    }
}
