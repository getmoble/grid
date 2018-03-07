using System.ComponentModel;

namespace Grid.Features.Settings.Models
{
    public class PointOfContactSettingsModel
    {
        [DisplayName("IT Department Level 1")]
        public int ITDepartmentLevel1 { get; set; }

        [DisplayName("IT Department Level 2")]
        public int ITDepartmentLevel2 { get; set; }

        [DisplayName("HR Department Level 1")]
        public int HRDepartmentLevel1 { get; set; }

        [DisplayName("HR Department Level 2")]
        public int HRDepartmentLevel2 { get; set; }

        [DisplayName("Sales Department Level 1")]
        public int SalesDepartmentLevel1 { get; set; }

        [DisplayName("Sales Department Level 2")]
        public int SalesDepartmentLevel2 { get; set; }
    }
}
