using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Multitenant;
using Grid;
using Grid.Api;
using Grid.Hubs;
using Grid.Infrastructure;
using Hangfire;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Autofac.Integration.WebApi;
using Grid.Features.Attendance.DAL;
using Grid.Features.Attendance.DAL.Interfaces;
using Grid.Features.Auth.DAL;
using Grid.Features.Auth.DAL.Interfaces;
using Grid.Features.Common;
using Grid.Generic.Api;
using Grid.Features.HRMS.DAL;
using Grid.Features.CRM.DAL;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Services;
using Grid.Features.CRM.Services.Interfaces;
using Grid.Features.EmailService;
using Grid.Features.Feedback.DAL;
using Grid.Features.Feedback.DAL.Interfaces;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Services;
using Grid.Features.HRMS.Services.Interfaces;
using Grid.Features.IMS.DAL;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Services;
using Grid.Features.IMS.Services.Interfaces;
using Grid.Features.IT.DAL;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.KBS.DAL;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.LMS.DAL;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Features.LMS.Services;
using Grid.Features.LMS.Services.Interfaces;
using Grid.Features.Payroll.DAL;
using Grid.Features.Payroll.DAL.Interfaces;
using Grid.Features.PMS.DAL;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Services;
using Grid.Features.PMS.Services.Interfaces;
using Grid.Features.Recruit.DAL;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Services;
using Grid.Features.Recruit.Services.Interfaces;
using Grid.Features.RMS.DAL;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Services;
using Grid.Features.RMS.Services.Interfaces;
using Grid.Features.Settings.DAL;
using Grid.Features.Settings.DAL.Interfaces;
using Grid.Features.Settings.Services;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Features.Social.DAL;
using Grid.Features.Social.DAL.Interfaces;
using Grid.Features.TicketDesk.DAL;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Services;
using Grid.Features.TicketDesk.Services.Interfaces;
using Grid.Data;
using Grid.Generic.Api.Filters;
using GlobalConfiguration = Hangfire.GlobalConfiguration;

[assembly: OwinStartup(typeof(Startup))]
namespace Grid
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // DataContext
            builder.RegisterType<GridDataContext>().As<GridDataContext>().As<IDbContext>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            // Repositories

            // Attendance
            builder.RegisterType<AttendanceRepository>().As<IAttendanceRepository>().InstancePerRequest();

            // Auth
            builder.RegisterType<TokenRepository>().As<ITokenRepository>().InstancePerRequest();

            //Company
            builder.RegisterType<AwardRepository>().As<IAwardRepository>().InstancePerRequest();
            builder.RegisterType<EmailTemplateRepository>().As<IEmailTemplateRepository>().InstancePerRequest();
            builder.RegisterType<LocationRepository>().As<ILocationRepository>().InstancePerRequest();
            builder.RegisterType<PermissionRepository>().As<IPermissionRepository>().InstancePerRequest();
            builder.RegisterType<RoleMemberRepository>().As<IRoleMemberRepository>().InstancePerRequest();
            builder.RegisterType<RolePermissionRepository>().As<IRolePermissionRepository>().InstancePerRequest();
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerRequest();
            builder.RegisterType<TechnologyRepository>().As<ITechnologyRepository>().InstancePerRequest();

            // CRM
            builder.RegisterType<CRMAccountRepository>().As<ICRMAccountRepository>().InstancePerRequest();
            builder.RegisterType<CRMContactRepository>().As<ICRMContactRepository>().InstancePerRequest();
            builder.RegisterType<CRMLeadActivityRepository>().As<ICRMLeadActivityRepository>().InstancePerRequest();
            builder.RegisterType<CRMLeadCategoryRepository>().As<ICRMLeadCategoryRepository>().InstancePerRequest();
            builder.RegisterType<CRMLeadRepository>().As<ICRMLeadRepository>().InstancePerRequest();
            builder.RegisterType<CRMLeadSourceRepository>().As<ICRMLeadSourceRepository>().InstancePerRequest();
            builder.RegisterType<CRMLeadStatusRepository>().As<ICRMLeadStatusRepository>().InstancePerRequest();
            builder.RegisterType<CRMLeadTechnologyMapRepository>().As<ICRMLeadTechnologyMapRepository>().InstancePerRequest();
            builder.RegisterType<CRMPotentialActivityRepository>().As<ICRMPotentialActivityRepository>().InstancePerRequest();
            builder.RegisterType<CRMPotentialCategoryRepository>().As<ICRMPotentialCategoryRepository>().InstancePerRequest();
            builder.RegisterType<CRMPotentialRepository>().As<ICRMPotentialRepository>().InstancePerRequest();
            builder.RegisterType<CRMPotentialTechnologyMapRepository>().As<ICRMPotentialTechnologyMapRepository>().InstancePerRequest();
            builder.RegisterType<CRMSalesStageRepository>().As<ICRMSalesStageRepository>().InstancePerRequest();

            //HRMS 
            builder.RegisterType<AccessRuleRepository>().As<IAccessRuleRepository>().InstancePerRequest();
            builder.RegisterType<CertificationRepository>().As<ICertificationRepository>().InstancePerRequest();
            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>().InstancePerRequest();
            builder.RegisterType<DesignationRepository>().As<IDesignationRepository>().InstancePerRequest();
            builder.RegisterType<EmergencyContactRepository>().As<IEmergencyContactRepository>().InstancePerRequest();
            builder.RegisterType<EmployeeDependentRepository>().As<IEmployeeDependentRepository>().InstancePerRequest();
            builder.RegisterType<HobbyRepository>().As<IHobbyRepository>().InstancePerRequest();
            builder.RegisterType<LinkedAccountRepository>().As<ILinkedAccountRepository>().InstancePerRequest();
            builder.RegisterType<PersonRepository>().As<IPersonRepository>().InstancePerRequest();
            builder.RegisterType<ShiftRepository>().As<IShiftRepository>().InstancePerRequest();
            builder.RegisterType<SkillRepository>().As<ISkillRepository>().InstancePerRequest();
            builder.RegisterType<UserActivityRepository>().As<IUserActivityRepository>().InstancePerRequest();
            builder.RegisterType<UserAwardRepository>().As<IUserAwardRepository>().InstancePerRequest();
            builder.RegisterType<UserCertificationRepository>().As<IUserCertificationRepository>().InstancePerRequest();
            builder.RegisterType<UserDocumentRepository>().As<IUserDocumentRepository>().InstancePerRequest();
            builder.RegisterType<UserHobbyRepository>().As<IUserHobbyRepository>().InstancePerRequest();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<UserSkillRepository>().As<IUserSkillRepository>().InstancePerRequest();
            builder.RegisterType<UserTechnologyMapRepository>().As<IUserTechnologyMapRepository>().InstancePerRequest();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>().InstancePerRequest();
            

            // IMS
            builder.RegisterType<AssetAllocationRepository>().As<IAssetAllocationRepository>().InstancePerRequest();
            builder.RegisterType<AssetCategoryRepository>().As<IAssetCategoryRepository>().InstancePerRequest();
            builder.RegisterType<AssetDocumentRepository>().As<IAssetDocumentRepository>().InstancePerRequest();
            builder.RegisterType<AssetRepository>().As<IAssetRepository>().InstancePerRequest();
            builder.RegisterType<VendorRepository>().As<IVendorRepository>().InstancePerRequest();

            // IT
            builder.RegisterType<SoftwareCategoryRepository>().As<ISoftwareCategoryRepository>().InstancePerRequest();
            builder.RegisterType<SoftwareRepository>().As<ISoftwareRepository>().InstancePerRequest();
            builder.RegisterType<SystemSnapshotRepository>().As<ISystemSnapshotRepository>().InstancePerRequest();

            // KBS
            builder.RegisterType<ArticleAttachmentRepository>().As<IArticleAttachmentRepository>().InstancePerRequest();
            builder.RegisterType<ArticleRepository>().As<IArticleRepository>().InstancePerRequest();
            builder.RegisterType<ArticleVersionRepository>().As<IArticleVersionRepository>().InstancePerRequest();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerRequest();
            builder.RegisterType<KeywordRepository>().As<IKeywordRepository>().InstancePerRequest();

            // LMS
            builder.RegisterType<HolidayRepository>().As<IHolidayRepository>().InstancePerRequest();
            builder.RegisterType<LeaveEntitlementRepository>().As<ILeaveEntitlementRepository>().InstancePerRequest();
            builder.RegisterType<LeaveEntitlementUpdateRepository>().As<ILeaveEntitlementUpdateRepository>().InstancePerRequest();
            builder.RegisterType<LeaveRepository>().As<ILeaveRepository>().InstancePerRequest();
            builder.RegisterType<LeaveTimePeriodRepository>().As<ILeaveTimePeriodRepository>().InstancePerRequest();
            builder.RegisterType<LeaveTypeRepository>().As<ILeaveTypeRepository>().InstancePerRequest();

            // Payroll
            builder.RegisterType<EmployeeSalaryRepository>().As<IEmployeeSalaryRepository>().InstancePerRequest();
            builder.RegisterType<SalaryComponentRepository>().As<ISalaryComponentRepository>().InstancePerRequest();

            //PMS 
            builder.RegisterType<ProjectBillingRepository>().As<IProjectBillingRepository>().InstancePerRequest();
            builder.RegisterType<ProjectDocumentRepository>().As<IProjectDocumentRepository>().InstancePerRequest();
            builder.RegisterType<ProjectMemberRepository>().As<IProjectMemberRepository>().InstancePerRequest();
            builder.RegisterType<ProjectMileStoneRepository>().As<IProjectMileStoneRepository>().InstancePerRequest();
            builder.RegisterType<ProjectRepository>().As<IProjectRepository>().InstancePerRequest();
            builder.RegisterType<ProjectActivityRepository>().As<IProjectActivityRepository>().InstancePerRequest();
            builder.RegisterType<ProjectTechnologyMapRepository>().As<IProjectTechnologyMapRepository>().InstancePerRequest();
            builder.RegisterType<TaskEffortRepository>().As<ITaskEffortRepository>().InstancePerRequest();
            builder.RegisterType<TaskRepository>().As<ITaskRepository>().InstancePerRequest();
            builder.RegisterType<TimeSheetLineItemRepository>().As<ITimeSheetLineItemRepository>().InstancePerRequest();
            builder.RegisterType<TimeSheetRepository>().As<ITimeSheetRepository>().InstancePerRequest();
            builder.RegisterType<EstimateRepository>().As<IEstimateRepository>().InstancePerRequest();
            builder.RegisterType<EstimateLineItemRepository>().As<IEstimateLineItemRepository>().InstancePerRequest();
            builder.RegisterType<TimeSheetActivityRepository>().As<ITimeSheetActivityRepository>().InstancePerRequest();
            builder.RegisterType<TaskActivityRepository>().As<ITaskActivityRepository>().InstancePerRequest();

            //Recruit
            builder.RegisterType<CandidateActivityRepository>().As<ICandidateActivityRepository>().InstancePerRequest();
            builder.RegisterType<CandidateDesignationRepository>().As<ICandidateDesignationRepository>().InstancePerRequest();
            builder.RegisterType<CandidateDocumentRepository>().As<ICandidateDocumentRepository>().InstancePerRequest();
            builder.RegisterType<CandidateRepository>().As<ICandidateRepository>().InstancePerRequest();
            builder.RegisterType<CandidateTechnologyMapRepository>().As<ICandidateTechnologyMapRepository>().InstancePerRequest();
            builder.RegisterType<InterviewRoundRepository>().As<IInterviewRoundRepository>().InstancePerRequest();
            builder.RegisterType<InterviewRoundActivityRepository>().As<IInterviewRoundActivityRepository>().InstancePerRequest();
            builder.RegisterType<InterviewRoundDocumentRepository>().As<IInterviewRoundDocumentRepository>().InstancePerRequest();
            builder.RegisterType<JobOfferRepository>().As<IJobOfferRepository>().InstancePerRequest();
            builder.RegisterType<JobOpeningRepository>().As<IJobOpeningRepository>().InstancePerRequest();
            builder.RegisterType<ReferalRepository>().As<IReferalRepository>().InstancePerRequest();
            builder.RegisterType<RoundRepository>().As<IRoundRepository>().InstancePerRequest();

            // RMS
            builder.RegisterType<RequirementActivityRepository>().As<IRequirementActivityRepository>().InstancePerRequest();
            builder.RegisterType<RequirementCategoryRepository>().As<IRequirementCategoryRepository>().InstancePerRequest();
            builder.RegisterType<RequirementDocumentRepository>().As<IRequirementDocumentRepository>().InstancePerRequest();
            builder.RegisterType<RequirementRepository>().As<IRequirementRepository>().InstancePerRequest();
            builder.RegisterType<RequirementTechnologyMapRepository>().As<IRequirementTechnologyMapRepository>().InstancePerRequest();

            // Settings 
            builder.RegisterType<GridUpdateRepository>().As<IGridUpdateRepository>().InstancePerRequest();
            builder.RegisterType<SettingRepository>().As<ISettingRepository>().InstancePerRequest();
            builder.RegisterType<UserFeedbackRepository>().As<IUserFeedbackRepository>().InstancePerRequest();
            builder.RegisterType<ApplicationRepository>().As<IApplicationRepository>().InstancePerRequest();

            // TicketDesk
            builder.RegisterType<TicketCategoryRepository>().As<ITicketCategoryRepository>().InstancePerRequest();
            builder.RegisterType<TicketSubCategoryRepository>().As<ITicketSubCategoryRepository>().InstancePerRequest();
            builder.RegisterType<TicketRepository>().As<ITicketRepository>().InstancePerRequest();
            builder.RegisterType<TicketActivityRepository>().As<ITicketActivityRepository>().InstancePerRequest();

            // TMS
            builder.RegisterType<MissedTimeSheetRepository>().As<IMissedTimeSheetRepository>().InstancePerRequest();

            //Social 
            builder.RegisterType<PostRepository>().As<IPostRepository>().InstancePerRequest();
            builder.RegisterType<PostCommentRepository>().As<IPostCommentRepository>().InstancePerRequest();
            builder.RegisterType<PostLikeRepository>().As<IPostLikeRepository>().InstancePerRequest();

            // Business Services
            builder.RegisterType<CRMLeadService>().As<ICRMLeadService>().InstancePerRequest();
            builder.RegisterType<CRMPotentialService>().As<ICRMPotentialService>().InstancePerRequest();
            builder.RegisterType<CRMAccountService>().As<ICRMAccountService>().InstancePerRequest();
            builder.RegisterType<CRMContactService>().As<ICRMContactService>().InstancePerRequest();
           
            builder.RegisterType<RequirementService>().As<IRequirementService>().InstancePerRequest();
            builder.RegisterType<SettingsService>().As<ISettingsService>().InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<AssetService>().As<IAssetService>().InstancePerRequest();
            builder.RegisterType<LeaveService>().As<ILeaveService>().InstancePerRequest();
            builder.RegisterType<CandidateService>().As<ICandidateService>().InstancePerRequest();
            builder.RegisterType<RequirementService>().As<IRequirementService>().InstancePerRequest();
            builder.RegisterType<SettingsService>().As<ISettingsService>().InstancePerRequest();
            builder.RegisterType<TicketService>().As<ITicketService>().InstancePerRequest();
            builder.RegisterType<TimeSheetService>().As<ITimeSheetService>().InstancePerRequest();
            builder.RegisterType<TaskService>().As<ITaskService>().InstancePerRequest();
            builder.RegisterType<InterviewRoundService>().As<IInterviewRoundService>().InstancePerRequest();

            // Common Services
            builder.RegisterType<NotificationService>().As<INotificationService>().InstancePerRequest();
            builder.RegisterType<EmailComposerService>().As<EmailComposerService>().InstancePerRequest();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterControllers(typeof(ApiAreaRegistration).Assembly);
            builder.RegisterApiControllers(typeof(GenericApiAreaRegistration).Assembly);

            // Register Injection for Filters
            builder.Register(c => new GlobalIdentityInjector(c.Resolve<IUserRepository>(), c.Resolve<IRoleMemberRepository>(), c.Resolve<IRolePermissionRepository>())).AsActionFilterFor<Controller>().InstancePerRequest();
            builder.RegisterFilterProvider();


            builder.Register(c => new GridApiAuthenticationFilter(c.Resolve<IUserRepository>(), c.Resolve<IRoleMemberRepository>(), c.Resolve<IRolePermissionRepository>())).AsWebApiAuthorizationFilterFor<ApiController>();
            builder.RegisterWebApiFilterProvider(System.Web.Http.GlobalConfiguration.Configuration);

            builder.RegisterType<SubDomainTenantIdentificationStrategy>().As<ITenantIdentificationStrategy>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);


            // Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            GlobalConfiguration.Configuration.UseSqlServerStorage("Grid");
            app.UseHangfireDashboard("/jobqueue", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
            app.UseHangfireServer();


            // SignalR
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new GridSignalRUserIdProvider());
            app.MapSignalR();
        }
    }
}