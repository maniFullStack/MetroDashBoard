using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.Configuration;

namespace WebsiteUtilities {
    /// <summary>
    /// The basic AuthenticatedPage for a web page.
    /// </summary>
    /// <typeparam name="T">The UserInformation (derived) class which while define the type for the User variable in the child pages.</typeparam>
    public class AuthenticatedPage<T> : Page where T : UserInformation, new() {

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
        public new T User { get; set; }

        protected override void OnPreInit(EventArgs e) {
            base.OnPreInit(e);
            User = SessionWrapper.Get<T>("UserInfo", null);
            if (User == null) {
                string url = Server.UrlEncode(Request.Url.PathAndQuery);
                if (String.IsNullOrEmpty(url)) {
                    Response.Redirect(GetConfigString("LoginPage", RedirectPage));
                } else {
                    Response.Redirect(GetConfigString("LoginPageWithRedirect", String.Concat(RedirectPage, "?", RedirectVariable, "=", url)));
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

    }
}
