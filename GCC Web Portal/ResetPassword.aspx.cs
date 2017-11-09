using SharedClasses;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class ResetPassword : BasePage
    {
        public string Email
        {
            get { return RequestVars.Get("email", string.Empty); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            form.Action = Request.Url.PathAndQuery;
            if (!string.IsNullOrEmpty(Email) && Email.RegExCheck(ValidationType.Email))
            {
                txtEmail.Text = Email;
            }

            if (Request.RequestType.Equals("POST"))
            {
                //string email = Request.Form["logon"];
                switch (SendResetEmail(txtEmail.Text))
                {
                    case PasswordResetCode.Success: //Success
                        mmMessages.SuccessMessage = "<strong>Success!</strong> A message was sent to the address containing a link to reset your password.";
                        break;

                    case PasswordResetCode.InvalidEmail: //Invalid email
                        mmMessages.ErrorMessage = "<strong>Whoops!</strong> Invalid email address.";
                        break;

                    case PasswordResetCode.GuidUpdateFailure: //Failed updating GUID
                        mmMessages.ErrorMessage = "<strong>Whoops!</strong> Invalid user information.";
                        break;

                    case PasswordResetCode.CriticalError: //Critical error
                    default:
                        mmMessages.ErrorMessage = "<strong>Whoops!</strong> It looks like there was an error sending the recovery email. Please try again.";
                        break;
                }
            }
            else if (Request.QueryString["guidexpired"] == "1")
            {
                mmMessages.ErrorMessage = "<strong>Error:</strong> This reset password link has expired, please reset your password again.";
            }
        }

        /// <summary>
        /// <para>Generates a new GUID in the database and sends a reset password email to the user.</para>
        /// <para>Return codes are as follows:</para>
        /// <para>0 - Success.</para>
        /// <para>1 - Invalid email address.</para>
        /// <para>2 - Failed updating GUID. Either there was a database error or a user doesn't exist with that email.</para>
        /// <para>3 - Critical error when attempting to send email.</para>
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <returns></returns>
        public static PasswordResetCode SendResetEmail(string email)
        {
            //Ensure we have a valid email.
            string link;
            string GCCPortalUrl = ConfigurationManager.AppSettings["GCCPortalURL"].ToString();
            bool isValid = Validation.RegExCheck(email, ValidationType.Email);
            if (!isValid)
            {
                return PasswordResetCode.InvalidEmail;
            }
            //Get new GUID for password reset
            string newGuid = UserInfo.ResetPassword(email);
            if (string.IsNullOrEmpty(newGuid))
            {
                return PasswordResetCode.InvalidEmail;
            }

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Uri uri = HttpContext.Current.Request.Url;
                link = uri.Scheme + "://" + uri.Host + ":" + uri.Port + "/PasswordChange?id=" + newGuid;
            }
            else
            {
                link = GCCPortalUrl + "PasswordChange?id=" + newGuid;
            }
            //Send email here.

            #region Create and send email

            MailMessage msg = new MailMessage { Subject = "Reporting Portal Password Recovery" };
            //Set subject
            //To address
            msg.To.Add(new MailAddress(email, email));
            //From noreply@forumresearch.com
            msg.From = new MailAddress("noreply@gcgamingsurvey.com", "Forum Research");
            //Set the reply-to email so it goes back to the help desk
            msg.ReplyToList.Add(new MailAddress("noreply@gcgamingsurvey.com", "Forum Research"));

            AlternateView plainView = AlternateView.CreateAlternateViewFromString(String.Format(
@"We have received your request to reset your password for the GCGC Reporting Portal website. To complete this request, simply click on the following link or paste it into your browser to reset your password.
{0}
Note: This link will only be active for 12 hours.

Regards,

Great Canadian Gaming Corporation", link), null, "text/plain");
            msg.AlternateViews.Add(plainView);
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(String.Format(
@"<p>We have received your request to reset your password for the GCGC Reporting Portal website. To complete this request, simply click on the following link or paste it into your browser to reset your password.<br />
{0}<br />
Note: This link will only be active for 12 hours.</p>
<p>Regards,</p>
<p>Great Canadian Gaming Corporation</p>",
"<a href='" + link + "'>" + link + "</a>"), null, "text/html");
            msg.AlternateViews.Add(htmlView);

            #endregion Create and send email

            // ReSharper disable RedundantAssignment
            PasswordResetCode response = PasswordResetCode.Success;
            // ReSharper restore RedundantAssignment

            try
            {
                //Settings in web.config
                SmtpClient smtp = new SmtpClient();
                smtp.Send(msg);
                response = PasswordResetCode.Success; //Success
            }
            catch (Exception ex)
            {
                ErrorHandler.WriteLog("GCGCReportingPortal.ResetPassword", String.Format("There was an error sending the password recovery email to: {0}", email), ErrorHandler.ErrorEventID.General, ex);
                response = PasswordResetCode.CriticalError; //Critical error!
            }
            finally
            {
                msg.Dispose();
            }
            return response;
        }
    }
}