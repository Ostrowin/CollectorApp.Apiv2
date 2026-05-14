namespace CollectorApp.Api.Models
{
    public class LoginModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }

        public string FullName => $"{Name} {Surname}".Trim();
    }
}