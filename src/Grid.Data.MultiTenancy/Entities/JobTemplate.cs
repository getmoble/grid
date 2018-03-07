namespace Grid.Data.MultiTenancy.Entities
{
    public class JobTemplate: TenantEntityBase
    {
        public string Name { get; set; }

        public string JobInfo { get; set; }
    }
}
