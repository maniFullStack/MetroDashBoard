using SharedClasses;
using System;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class Login : BasePage
    {
        protected bool LoggedOutOnLoad = false;
        protected string LoginRedirect = String.Empty;
        protected LoginErrorCode LoginResponse = LoginErrorCode.UnknownError;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckRedirect();

            SQLDatabase sql = new SQLDatabase();
            if(sql.HasError == true)
            {
                string a;
            }

            if (Request.RequestType == "GET")
            {
                //Check for logouts
                if (RequestVars.Get("lo", 0) == 1)
                {
                    Session.Abandon();
                    LoggedOutOnLoad = true;
                }
            }

            if (Request.RequestType == "POST")
            {
                LoginResponse = TryLogUserIn();
                switch (LoginResponse)
                {
                    case LoginErrorCode.Success:
                        Response.Redirect(LoginRedirect);
                        break;

                    case LoginErrorCode.PasswordExpired:
                        Response.Redirect("/Director.ashx"); //This will redirect to password change
                        break;

                    default:
                        SetErrorSuccessMessage(LoginResponse);
                        break;
                }
            }
        }

        public void SetErrorSuccessMessage(LoginErrorCode loginCode)
        {
            switch (loginCode)
            {
                case LoginErrorCode.Success:
                    mmMessages.SuccessMessage = string.Format("You're now logged in. You will be redirected in 5 seconds or you can click <a href='{0}' title='Continue'>here</a> to continue.", "Director.ashx");
                    break;

                case LoginErrorCode.UserLockedOut:
                    mmMessages.ErrorMessage = "We're sorry, but you've been locked out. Please wait 30 minutes and try again.";
                    break;

                case LoginErrorCode.LoginFailed:
                    mmMessages.ErrorMessage = "Invalid email and password combination. Please try again.";
                    break;

                case LoginErrorCode.NoUserOrEmailSpecified:
                    mmMessages.ErrorMessage = "Please specify both an email and password.";
                    break;

                case LoginErrorCode.UserOrEmailNotExists:
                    mmMessages.ErrorMessage = "This account does not appear to exist.";
                    break;

                default:
                    mmMessages.ErrorMessage = "Oops! It looks like an unknown error occurred. Please try again and if this issue persists, contact the administrator.";
                    break;
            }
        }

        /// <summary>
        ///     Checks if page has redirect url
        /// </summary>
        public void CheckRedirect()
        {
            LoginRedirect = "/Director.ashx";
            string redir = RequestVars.Get("rd", String.Empty);

            if (!String.IsNullOrEmpty(redir))
            {
                LoginRedirect += "?rd=" + Server.UrlPathEncode(redir);
            }
        }

        /// <summary>
        ///     Checks for valid login credentials. Returns 0 if successful, 1 if the username or password is empty, 2 if there was a SQL error, 3 if the user is locked out or 4 if the username or password is invalid.
        /// </summary>
        private static LoginErrorCode TryLogUserIn()
        {
            string username = RequestVars.Post("inputUsername", String.Empty).Trim(),
                   password = RequestVars.Post("inputPassword", String.Empty).Trim();

            int outputVal;

            UserInfo ui = UserInformation.LogUserIn<UserInfo>(
                username,
                password,
                true, //email logins allowed
                Config.ClientID, out outputVal);

            LoginErrorCode loginCode = (LoginErrorCode)outputVal;

            return loginCode;
        }
    }
}