using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grid.Features.Common
{
    public class ActivityModel
    {
        public string Title { get; set; }

        [DisplayName("New Activity")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
