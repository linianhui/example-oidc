using System.ComponentModel.DataAnnotations;

namespace ServerSite.Ids4.Account
{
    public class LoginFormModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ResumeUrl { get; set; }
    }
}