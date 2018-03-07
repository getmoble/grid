namespace Grid.TenantManagement.Models
{
    public class PrincipalModel
    {
        public string Key { get; set; }

        public PrincipalModel(string key)
        {
            Key = key;
        }
    }
}
