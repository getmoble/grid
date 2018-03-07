using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Grid.Features.Attendance.Entities;
using Grid.Features.Auth.Entities;
using Grid.Features.Common;
using Grid.Features.CRM.Entities;
using Grid.Features.Feedback.Entities;
using Grid.Features.HRMS;
using Grid.Features.HRMS.Entities;
using Grid.Features.IMS.Entities;
using Grid.Features.IT.Entities;
using Grid.Features.KBS.Entities;
using Grid.Features.LMS.Entities;
using Grid.Features.Payroll.Entities;
using Grid.Features.PMS.Entities;
using Grid.Features.Recruit.Entities;
using Grid.Features.RMS.Entities;
using Grid.Features.Settings.Entities;
using Grid.Features.Social.Entities;
using Grid.Features.TicketDesk.Entities;

namespace Grid.Data
{
    public class GridDataContext : DbContext, IHRMSDataContext
    {
        public GridDataContext()
            : base("Grid")
        {
            Configuration.ProxyCreationEnabled = false;
           
        }

        public GridDataContext(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<RequirementCategory> RequirementCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<AccessRule> AccessRules { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<RequirementActivity> RequirementActivities { get; set; }
        public DbSet<CRMAccount> CRMAccounts { get; set; }
        public DbSet<CRMContact> CRMContacts { get; set; }
        public DbSet<ProjectBilling> ProjectBillings { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectActivity> ProjectActivities { get; set; }
        public DbSet<ProjectDocument> ProjectDocuments { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ProjectMileStone> ProjectMileStones { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskEffort> TaskEfforts { get; set; }
        public DbSet<CRMLeadStatus> CRMLeadStatuses { get; set; }
        public DbSet<CRMLeadSource> CRMLeadSources { get; set; }
        public DbSet<CRMSalesStage> CRMSalesStages { get; set; }
        public DbSet<CRMLead> CRMLeads { get; set; }
        public DbSet<CRMLeadActivity> CRMLeadActivities { get; set; }
        public DbSet<CRMPotential> CRMPotentials { get; set; }
        public DbSet<CRMPotentialActivity> CRMPotentialActivities { get; set; }
        public DbSet<CRMPotentialTechnologyMap> PotentialTechnologyMaps { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleMember> RoleMembers { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
        public DbSet<SalaryComponent> SalaryComponents { get; set; }
        public DbSet<SoftwareCategory> SoftwareCategories { get; set; }
        public DbSet<Software> Softwares { get; set; }
        public DbSet<SystemSnapshot> SystemSnapshots { get; set; }
        public DbSet<AssetCategory> AssetCategories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetAllocation> AssetAllocations { get; set; }
        public DbSet<CRMLeadCategory> CRMLeadCategories { get; set; }
        public DbSet<CRMPotentialCategory> CRMPotentialCategories { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<EmployeeDependent> EmployeeDependents { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserCertification> UserCertifications { get; set; }
        public DbSet<UserDocument> UserDocuments { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<TimeSheetLineItem> TimeSheetLineItems { get; set; }
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateActivity> CandidateActivities { get; set; }
        public DbSet<InterviewRound> InterviewRounds { get; set; }
        public DbSet<Referal> Referals { get; set; }
        public DbSet<JobOpening> JobOpenings { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<LeaveTimePeriod> LeaveTimePeriods { get; set; }
        public DbSet<LeaveEntitlement> LeaveEntitlements { get; set; }
        public DbSet<LeaveEntitlementUpdate> LeaveEntitlementUpdates { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleVersion> ArticleVersions { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<CRMLeadTechnologyMap> CRMLeadTechnologyMaps { get; set; }
        public DbSet<UserTechnologyMap> UserTechnologyMaps { get; set; }
        public DbSet<ProjectTechnologyMap> ProjectTechnologyMaps { get; set; }
        public DbSet<RequirementTechnologyMap> RequirementTechnologyMaps { get; set; }
        public DbSet<CandidateTechnologyMap> CandidateTechnologyMaps { get; set; }
        public DbSet<GridUpdate> GridUpdates { get; set; }
        public DbSet<LinkedAccount> LinkedAccounts { get; set; }
        public DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public DbSet<AssetDocument> AssetDocuments { get; set; }
        public DbSet<RequirementDocument> RequirementDocuments { get; set; }
        public DbSet<UserFeedback> UserFeedbacks { get; set; }
        public DbSet<CandidateDocument> CandidateDocuments { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<UserHobby> UserHobbies { get; set; }
        public DbSet<CandidateDesignation> CandidateDesignations { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<UserAward> UserAwards { get; set; }
        public DbSet<ArticleAttachment> ArticleAttachments { get; set; }
        public DbSet<TicketCategory> TicketCategories { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketActivity> TicketActivities { get; set; }
        public DbSet<TicketSubCategory> TicketSubCategories { get; set; }
        public DbSet<MissedTimeSheet> MissedTimeSheets { get; set; }

        public DbSet<TimeSheetActivity> TimeSheetActivities { get; set; }
        public DbSet<TaskActivity> TaskActivities { get; set; }

        public DbSet<Estimate> Estimates { get; set; }
        public DbSet<EstimateLineItem> EstimateLineItems { get; set; }

        public DbSet<InterviewRoundActivity> InterviewRoundActivities { get; set; }
        public DbSet<InterviewRoundDocument> InterviewRoundDocuments { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostComment> PostComments { get; set; }

        public DbSet<ScheduledJob> ScheduledJobs { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public override int SaveChanges()
        {
            PopulateIdentityAndTimeStamps();
            return base.SaveChanges();
        }

        private void PopulateIdentityAndTimeStamps()
        {
            // Get the list of new Entities
            var newEntities = ChangeTracker.Entries().Where(x => x.Entity is EntityBase && x.State == EntityState.Added).ToList();
            foreach (var entity in newEntities)
            {
                var entityBasedEntity = (EntityBase)entity.Entity;
                if (entityBasedEntity != null)
                {
                    entityBasedEntity.CreatedOn = DateTime.UtcNow;
                }
            }

            // Get the list of updated Entities
            var updatedEntities = ChangeTracker.Entries().Where(x => x.Entity is UserCreatedEntityBase && x.State == EntityState.Modified).ToList();
            foreach (var entity in updatedEntities)
            {
                var entityBasedEntity = (UserCreatedEntityBase)entity.Entity;
                if (entityBasedEntity != null)
                {
                    entityBasedEntity.UpdatedOn = DateTime.UtcNow;
                }
            }
        }
    }
}