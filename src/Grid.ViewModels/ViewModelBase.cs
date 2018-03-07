using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Grid.ViewModels
{
    public class ViewModelBase
    {
        public int Id { get; set; }
        
        [ScaffoldColumn(false)]
        public string Code { get; set; }
        
        [DisplayName("Created On")]
        [ScaffoldColumn(false)]
        public DateTime CreatedOn { get; set; }

        public int TrimLength { get; set; }

        public ViewModelBase()
        {
            TrimLength = 80;
            CreatedOn = DateTime.UtcNow;
        }
    }
}