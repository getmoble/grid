namespace Grid.Models
{
    public class JobInfoModel
    {
        public string JobType { get; set; }
        public string CronExpression { get; set; }
        public string Url { get; set; }
    }
}