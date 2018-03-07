using System.ComponentModel.DataAnnotations;

namespace Grid.Areas.Recruit.ViewModels
{
    public class CandidateSignInViewModel
    {
        public string ReturnUrl { get; set; }
        public string Email { get; set; }
        public bool RememberMe { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}