namespace Grid.Data.MultiTenancy.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<TenantDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
            ContextKey = "Grid.Data.MultiTenancy.TenantDataContext";
        }

        protected override void Seed(TenantDataContext context)
        {
            
        }
    }
}
