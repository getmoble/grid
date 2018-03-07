using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;

namespace Grid.Features.HRMS.Entities
{
    public class EmployeeDependent : EntityBase
    {
        public DependentType DependentType { get; set; }

        public string Name { get; set; }

        [DisplayName("Date of Birth")]
        [UIHint("Date")]
        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; }
     
        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
