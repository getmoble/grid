using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Multitenant;
using Grid.Api;
using Grid.BLL;
using Grid.BLL.CRM;
using Grid.BLL.CRM.Interfaces;
using Grid.BLL.HRMS;
using Grid.BLL.HRMS.Interfaces;
using Grid.BLL.IMS;
using Grid.BLL.IMS.Interfaces;
using Grid.BLL.LMS;
using Grid.BLL.LMS.Interfaces;
using Grid.BLL.PMS;
using Grid.BLL.PMS.Interfaces;
using Grid.BLL.Recruit;
using Grid.BLL.Recruit.Interfaces;
using Grid.BLL.RMS;
using Grid.BLL.RMS.Interfaces;
using Grid.BLL.Settings;
using Grid.BLL.Settings.Interfaces;
using Grid.BLL.TicketDesk;
using Grid.BLL.TicketDesk.Interfaces;
using Grid.BLL.TMS;
using Grid.BLL.TMS.Interfaces;
using Grid.Data;
using Grid.DAL;
using Grid.DAL.Attendance;
using Grid.DAL.Attendance.Interfaces;
using Grid.DAL.Auth;
using Grid.DAL.Auth.Interfaces;
using Grid.DAL.Company;
using Grid.DAL.Company.Interfaces;
using Grid.DAL.CRM;
using Grid.DAL.CRM.Interfaces;
using Grid.DAL.HRMS;
using Grid.DAL.HRMS.Interfaces;
using Grid.DAL.IMS;
using Grid.DAL.IMS.Interfaces;
using Grid.DAL.Interfaces;
using Grid.DAL.IT;
using Grid.DAL.IT.Interfaces;
using Grid.DAL.KBS;
using Grid.DAL.KBS.Interfaces;
using Grid.DAL.LMS;
using Grid.DAL.LMS.Interfaces;
using Grid.DAL.Payroll;
using Grid.DAL.Payroll.Interfaces;
using Grid.DAL.PMS;
using Grid.DAL.PMS.Interfaces;
using Grid.DAL.Recruit;
using Grid.DAL.Recruit.Interfaces;
using Grid.DAL.RMS;
using Grid.DAL.RMS.Interfaces;
using Grid.DAL.Settings;
using Grid.DAL.Settings.Interfaces;
using Grid.DAL.Social;
using Grid.DAL.Social.Interfaces;
using Grid.DAL.TicketDesk;
using Grid.DAL.TicketDesk.Interfaces;
using Grid.DAL.TMS;
using Grid.DAL.TMS.Interfaces;
using Grid.Infrastructure;
using Grid.UI.WebApp;
using Grid.UI.WebApp.Hubs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Grid.UI.WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            // DataContext
            builder.Register(MultiTenantDataContextFactory.Create).As<GridDataContext>().As<IDbContext>().InstancePerRequest();
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

            // Register Injection for Filters
            builder.Register(c => new GlobalIdentityInjector(c.Resolve<IUserRepository>(), c.Resolve<IRoleMemberRepository>(), c.Resolve<IRolePermissionRepository>())).AsActionFilterFor<Controller>().InstancePerRequest();
            builder.RegisterFilterProvider();

            builder.RegisterType<SubDomainTenantIdentificationStrategy>().As<ITenantIdentificationStrategy>();

            var appContainer = builder.Build();
            var tenantIdentifier = new SubDomainTenantIdentificationStrategy();
            var mtc = new MultitenantContainer(tenantIdentifier, appContainer);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(mtc));

            // Register the Autofac middleware FIRST, then the Autofac MVC middleware.
            app.UseAutofacMiddleware(mtc);
            app.UseAutofacMvc();
        }
    }
}