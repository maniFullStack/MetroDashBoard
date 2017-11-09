using SharedClasses;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Configuration;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class PasswordChange : BasePage
    {
        protected bool HideForm = false; //Whether or not to hide the entire form.
        protected bool ShowOldPassword = true; //Whether or not to show the previous password field

        protected UserInfo UserData; //The current user's information.

        protected void Page_Load(object sender, EventArgs e)
        {
            form.Action = Request.Url.PathAndQuery;
            bool isGuidReset = false;
            //Load user variable
            if (RequestVars.Get<string>("id", null) != null)
            {
                UserData = UserInformation.GetUser<UserInfo>(RequestVars.Get<string>("id", null));
                ShowOldPassword = false;
                isGuidReset = true;
            }
            else
            {
                UserData = SessionWrapper.Get<UserInfo>("UserInfo");
            }
            //If no user was loaded, output error message
            if (UserData == null)
            {
                mmMessages.ErrorMessage = "We're sorry. We couldn't find the user associated with this account or the link is invalid. Please try again.";
                HideForm = true;
                return;
            }

            //Check if the GUID reset time has expired
            if (isGuidReset && !IsResetPasswordTimeValid(UserData.PasswordResetDate))
            {
                Response.Redirect("/ResetPassword?guidexpired=1");
                return;
            }

            if (Request.RequestType.Equals("POST"))
                ResetPassword(isGuidReset);
        }

        /// <summary>
        ///     Check if the reset password time stamp is valid
        /// </summary>
        /// <param name="resetPasswordTime"></param>
        /// <returns></returns>
        private static bool IsResetPasswordTimeValid(DateTime? resetPasswordTime)
        {
            int hours = 12;
            if (WebConfigurationManager.AppSettings["ResetPasswordTimeHours"] != null)
                int.TryParse(WebConfigurationManager.AppSettings["ResetPasswordTimeHours"], out hours);

            return resetPasswordTime != null && resetPasswordTime.Value.AddHours(hours) > DateTime.Now;
        }

        /// <summary>
        ///     Perform password reset
        /// </summary>
        /// <param name="isGuidReset">is this a password retrieval?</param>
        public void ResetPassword(bool isGuidReset)
        {
            string pass = Request.Form["pwd"]; //Old password
            string newpass1 = Request.Form["newpwd1"]; //New passwords
            string newpass2 = Request.Form["newpwd2"];
            LoginErrorCode loginError = LoginErrorCode.UnknownError;
            UserInfo user = UserData;
            //If this isn't a GUID-based reset (we already checked those)
            if (!isGuidReset)
            {
                int outputValue;
                user = UserInformation.LogUserIn<UserInfo>(user.Email, pass, true, Config.ClientID, out outputValue);
                loginError = (LoginErrorCode)outputValue;
                if (loginError != LoginErrorCode.Success)
                {
                    user = SessionWrapper.Get<UserInfo>("UserInfo", null);
                    switch (loginError)
                    {
                        case LoginErrorCode.NoUserOrEmailSpecified:
                            mmMessages.ErrorMessage = "Please enter your old password.";
                            return;

                        case LoginErrorCode.LoginFailed:
                            mmMessages.ErrorMessage = "Invalid password.";
                            return;

                        case LoginErrorCode.UserLockedOut:
                            mmMessages.ErrorMessage = "This account has been locked out. Please try again in 30 minutes.";
                            return;

                        case LoginErrorCode.UserOrEmailNotExists:
                            mmMessages.ErrorMessage = "This user doesn't appear to exist. If you think this is an error, please contact the administrator.";
                            return;
                    }
                }
            }
            if (user == null)
            {
                //Error message!
                mmMessages.ErrorMessage = "We're sorry. We couldn't find the user associated with this account. Please try again.";
                return;
            }
            UserData = user;
            //Validate new password
            if (newpass1 != newpass2)
            {
                mmMessages.ErrorMessage = "These new passwords don't match.";
                return;
            }
            //try to update password and return error message if invalid
            string error;
            if (!UserData.UpdatePassword(newpass1, out error))
            {
                mmMessages.ErrorMessage = error;
                return;
            }

            int rows = UserData.ClearLoginAttempts();

#if DEBUG
            Debug.WriteLine("{0} Login Attempts deleted");
#endif

            //Get user with new password to make sure that everything is OK
            int outputValue2;
            UserData = UserInformation.LogUserIn<UserInfo>(UserData.Email, newpass1, true, Config.ClientID, out outputValue2);

            if (UserData == null)
            {
                if (user != null)
                {
                    UserData = user;
                }
                else
                {
                    UserData = SessionWrapper.Get<UserInfo>("UserInfo", null);
                }

                mmMessages.ErrorMessage = "There was a database level error while attempting to save this user. Please contact the administrator if this error persists.";
                return;
            }

            SessionWrapper.Add("UserInfo", UserData);

            string redir = RequestVars.Get("rd", String.Empty);
            if (!String.IsNullOrEmpty(redir))
            {
                redir = "?rd=" + HttpContext.Current.Server.UrlEncode(redir);
            }
            Response.Redirect("/Director.ashx" + redir);
        }
    }
}