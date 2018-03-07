namespace Grid.Api.Models
{
    public class AuthResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}