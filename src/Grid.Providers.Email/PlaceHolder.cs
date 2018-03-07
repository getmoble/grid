namespace Grid.Providers.Email
{
    public class PlaceHolder
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public PlaceHolder(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
