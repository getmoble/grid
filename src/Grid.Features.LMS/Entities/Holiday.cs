using System;
using System.ComponentModel;
using Grid.Features.Common;
using Grid.Features.LMS.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Grid.Features.LMS.Entities
{
    public class Holiday: EntityBase
    {
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }

        public HolidayType Type { get; set; }

        [Required]
        [UIHint("Date")]
        public DateTime Date { get; set; }

        public string Description { get; set; }
    }
}
