using System;

namespace Grid.Features.Common
{
    public class ProjectContribution
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public double Effort { get; set; }
        public double Value { get; set; }
        public DateTime Day { get; set; }
    }
}