using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using ChatService.Implementation;
using ChatService.Infrastructure;
using Ninject;
using totalhr.data.EF;
using totalhr.data.Repositories.Infrastructure;
using totalhr.data.Repositories.Implementation;
using totalhr.services.Infrastructure;
using totalhr.services.Implementation;
using totalhr.services.messaging.Infrastructure;
using totalhr.services.messaging.Implementation;
using totalhr.Shared;
using CM;
using totalhr.Shared.Infrastructure;
using Authentication;
using Authentication.Infrastructure;
using Authentication.Implementation;
using Calendar.Infrastructure;
using Calendar.Implementation;
using CompanyDocumentService.Implementation;
using CompanyDocumentService.Infrastructure;
using FileManagementService.Infrastructure;
using FileManagementService.Implementation;
using totalhr.data.TimeRecordingSystem.Repositories.Infrastructure;
using totalhr.data.TimeRecordingSystem.Repositories.Implementation;
using FormService.Infrastructure;
using FormService.Implementation;
using AbsencesService.Infrastructure;
using Absences.Implementation;
using ImageGallery;
using ImageGallery.Implementation;
using ImageGallery.Infrastructure;

namespace totalhr.web.Controllers.Dependencies
{
    public class Container : DefaultControllerFactory
    {
        private readonly IKernel ninjectKernel;

        public Container()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)ninjectKernel.Get(controllerType);
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
            ninjectKernel.Bind<IAuthService>().To<AuthService>();
            ninjectKernel.Bind<IOAuthService>().To<OckAuthService>();
            ninjectKernel.Bind<ICalendarDisplayService>().To<CalendarDisplayService>();
            ninjectKernel.Bind<ICalendarManagementService>().To<CalendarManagementService>();
            ninjectKernel.Bind<ICalendarRepository>().To<CalendarRepository>();
            ninjectKernel.Bind<ICalendarEventRepository>().To<CalendarEventRepository>();
            ninjectKernel.Bind<IDocumentManager>().To<DocumentManager>();
            ninjectKernel.Bind<ICompanyDocumentRepository>().To<CompanyDocumentRepository>();
            ninjectKernel.Bind<IFileRepository>().To<FileRepository>();
            ninjectKernel.Bind<IFileService>().To<FileService>();
            ninjectKernel.Bind<ICompanyService>().To<CompanyService>();
            ninjectKernel.Bind<ITimeRecordingServices>().To<TimeRecordingServices>();
            ninjectKernel.Bind<ITimeRecordingRepository>().To<TimeRecordingRepository>();
            ninjectKernel.Bind<IProfileService>().To<ProfileService>();
            ninjectKernel.Bind<IProfileRepository>().To<ProfileRepository>();
            ninjectKernel.Bind<IRoleService>().To<RoleService>();
            ninjectKernel.Bind<IRoleRepository>().To<RoleRepository>();
            ninjectKernel.Bind<ITaskSchedulerRepository>().To<TaskSchedulerRepository>();
            ninjectKernel.Bind<ITaskSchedulerService>().To<TaskSchedulerService>();
            ninjectKernel.Bind<IContractRepository>().To<ContractRepository>();
            ninjectKernel.Bind<IContractTemplateRepository>().To<ContractTemplateRepository>();
            ninjectKernel.Bind<IContractService>().To<ContractService>();
            ninjectKernel.Bind<IFormEditorService>().To<FormEditorService>();
            ninjectKernel.Bind<IFormRepository>().To<FormRepository>();
            ninjectKernel.Bind<IChatRepository>().To<ChatRepository>();
            ninjectKernel.Bind<IChatManagerService>().To<ChatManagerService>();
            ninjectKernel.Bind<IAbsencesManager>().To<AbsencesManager>();
            ninjectKernel.Bind<IAbsencesRepository>().To<AbsencesRepository>();
            ninjectKernel.Bind<IGalleryRepository>().To<GalleryRepository>();
            ninjectKernel.Bind<IGalleryService>().To<GalleryService>();
        }

    }

}

