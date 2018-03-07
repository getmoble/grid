using System.ComponentModel.DataAnnotations;

namespace Grid.Features.Common.Models
{
    public class SignInViewModel
    {
        public string ReturnUrl { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}