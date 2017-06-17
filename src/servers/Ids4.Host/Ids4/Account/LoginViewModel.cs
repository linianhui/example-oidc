namespace Ids4.Host.Ids4.Account
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
    }
}
