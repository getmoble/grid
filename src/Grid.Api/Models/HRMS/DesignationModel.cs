
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Models.HRMS
{
    public class DesignationModel: ApiModelBase
    {
        public string Title { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public string Band { get; set; }
        public string MailAlias { get; set; }
        public string Description { get; set; }

        public DesignationModel(Designation designation)
        {
            Id = designation.Id;
            Title = designation.Title;
            if (designation.Department != null)
            {
                Department = designation.Department.Title;
            }

            Band = GetEnumDescription(designation.Band);
            MailAlias = designation.MailAlias;
            Description = designation.Description;
            CreatedOn = designation.CreatedOn;
        }
    }
}
