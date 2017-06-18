using System.Collections.Generic;

namespace ServerSite.Ids4.Account
{
    public class LoginViewModel : LoginFormModel
    {
        public LoginViewModel(LoginFormModel form)
        {
            if (form != null)
            {
                base.UserName = form.UserName;
                base.ResumeUrl = form.ResumeUrl;
            }
        }

        public List<ExternalLoginViewModel> ExternalLoginList { get; set; }
    }
}
