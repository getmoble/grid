using Grid.Features.HRMS.Entities;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Api.Models
{
    public class EmployeeInfo
    {
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public Gender? Gender { get; set; }
        
        public string Location { get; set; }
        public string SeatNo { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }

        public string OfficialEmail { get; set; }
        public string OfficialPhone { get; set; }

        public string Photo { get; set; }

        public EmployeeInfo(User user)
        {
            EmployeeCode = user.EmployeeCode;
            Gender = user.Person.Gender;

            if (user.Person != null)
            {
                Name = user.Person.Name;
            }

            OfficialEmail = user.OfficialEmail;
            OfficialPhone = user.OfficialPhone;

            if (user.Location != null)
            {
                Location = user.Location.Title;
            }

            SeatNo = user.SeatNo;

            if (user.Department != null)
            {
                Department = user.Department.Title;
            }

            if (user.Designation != null)
            {
                Designation = user.Designation.Title;
            }

            Photo = user.Person.PhotoPath;
        }
    }
}