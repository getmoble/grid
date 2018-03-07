namespace Grid.Infrastructure
{
    public class PrincipalModel
    {
        public string Key { get; set; }

        public PrincipalModel()
        {
            
        }
        public PrincipalModel(string key)
        {
            Key = key;
        }
    }
}