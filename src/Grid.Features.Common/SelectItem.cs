namespace Grid.Features.Common
{
    public class SelectItem
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public SelectItem()
        {
            
        }

        public SelectItem(int id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
