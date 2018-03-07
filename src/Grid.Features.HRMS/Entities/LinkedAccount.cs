using Grid.Features.Common;
using Grid.Features.HRMS.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;


namespace Grid.Features.HRMS.Entities
{
    public class LinkedAccount: EntityBase
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public LinkedAccountType AccountType { get; set; }

        public string AccessToken { get; set; }

        public string TokenSecret { get; set; }

        // What user has allowed us
        public string Scope { get; set; }
    }
}