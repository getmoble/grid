using Grid.Features.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Grid.Features.HRMS.Entities
{
    public class UserHobby: EntityBase
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public int HobbyId { get; set; }
        [ForeignKey("HobbyId")]
        public virtual Hobby Hobby { get; set; }
    }
}