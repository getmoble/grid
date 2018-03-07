using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Grid.Data.MultiTenancy.Entities;

namespace Grid.Data.MultiTenancy
{
    public class TenantDataContext : DbContext
    {
        public TenantDataContext()
            : base("Tenants")
        {
        }

        public TenantDataContext(string connectionString)
            : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantScheduledJob> TenantScheduledJobs { get; set; }

        // Reference Tables
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<JobTemplate> JobTemplates { get; set; }

        public override int SaveChanges()
        {
            PopulateIdentityAndTimeStamps();
            return base.SaveChanges();
        }

        private void PopulateIdentityAndTimeStamps()
        {
            // Get the list of new Entities
            var newEntities = ChangeTracker.Entries().Where(x => x.Entity is TenantEntityBase && (x.State == EntityState.Added)).ToList();
            foreach (var entity in newEntities)
            {
                var entityBasedEntity = (TenantEntityBase)entity.Entity;
                if (entityBasedEntity != null)
                {
                    entityBasedEntity.CreatedOn = DateTime.UtcNow;
                }
            }
        }
    }
}
