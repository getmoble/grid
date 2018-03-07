using Grid.Features.IT.Entities;
using Grid.Features.IT.Entities.Enums;

namespace Grid.Api.Models.IT
{
    public class SoftwareModel:ApiModelBase
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Version { get; set; }
       
        public decimal LatestVersion { get; set; }
       
        public decimal RecommendedVersion { get; set; }
        public string StatusName { get; set; }
        public SoftwareStatus Status { get; set; }
        public LicenseType LicenseType { get; set; }

        public int LicensesAllowed { get; set; }
        public int SoftwareCategoryId { get; set; }       
        public string Category { get; set; }

        public SoftwareModel()
        {

        }
        public SoftwareModel(Software software)
        {
            Id = software.Id;
            Title = software.Title;
            Version = software.Version;
            StatusName = GetEnumDescription(software.Status);           
            CreatedOn = software.CreatedOn;
        }
    }
}

