using System;
using System.Web;
using System.Web.SessionState;
using System.Web.Configuration;

namespace WebsiteUtilities {
    /// <summary>
    /// The basic AuthenticatedPage for a handler.
    /// </summary>
    /// <typeparam name="T">The UserInformation (derived) class which while define the type for the User variable in the child pages.</typeparam>
    public class AuthenticatedHandler<T> : IHttpHandler, IRequiresSessionState where T : UserInformation, new() {

        private string _redirectPage = "~/Default.aspx";
        private string _redirectVariable = "rd";

        /// <summary>
        /// The page to redirect to if the user is not authenticated. Defaults to "~/Default.aspx";
        /// </summary>
        public string RedirectPage {
            get { return _redirectPage; }
            set { _redirectPage = value; }
        }

        /// <summary>
        /// The name of the query string variable used to pass the address to redirect back to. Defaults to "rd".
        /// </summary>
        public string RedirectVariable {
            get { return _redirectVariable; }
            set { _redirectVariable = value; }
        }


        /// <summary>
        /// Information about the currently logged in user.
        /// </summary>
        public T User { get; set; }

        public virtual void ProcessRequest(HttpContext context) {
            User = SessionWrapper.Get<T>("UserInfo", null);
            if (User == null) {
                string url = context.Server.UrlEncode(context.Request.Url.PathAndQuery);
                if (String.IsNullOrEmpty(url)) {
                    context.Response.Redirect(GetConfigString("LoginPage", RedirectPage), true);
                } else {
                    context.Response.Redirect(GetConfigString("LoginPageWithRedirect", String.Concat(RedirectPage, "?", RedirectVariable, "=", url)), true);
                }
            }
        }


        private string GetConfigString(string appSettingValue, string defaultVal) {
            var value = WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings[appSettingValue];
            if (value == null) {
                return defaultVal;
            } else {
                return value.Value;
            }
        }

        #region IHttpHandler Members

        public bool IsReusable {
            get { return false; }
        }

        #endregion
    }
}
