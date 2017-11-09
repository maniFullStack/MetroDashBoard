using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;

namespace SharedClasses {
    public class BasePage : Page {

        protected override void OnPreInit(EventArgs e) {
            VerifyHTTPS(Request, Response);
            base.OnPreInit(e);
        }

        /// <summary>
        /// Checks to ensure that the site is running under HTTPS based on the EnforceSSL app setting in web.config.
        /// </summary>
        /// <param name="Request">The HttpRequest object associated with the current connection.</param>
        /// <param name="Response">The HttpResponse object associated with the current connection.</param>
        public static void VerifyHTTPS(HttpRequest Request, HttpResponse Response) {
            if (!Request.IsSecureConnection
                && WebConfigurationManager.AppSettings["EnforceSSL"] != null
                && WebConfigurationManager.AppSettings["EnforceSSL"].ToLower().Equals("true")) {
                Response.Redirect(String.Format("https://{0}{1}", Request.ServerVariables["HTTP_HOST"], Request.RawUrl));
            }
        }

        /// <summary>
        /// This method returns a different value depending on which mode the project has been built in. The intent is to use it to load things like regular vs minified scripts and stylesheets.
        /// </summary>
        /// <param name="debugValue">The value to output when running in debug mode.</param>
        /// <param name="releaseValue">The value to output when running in release mode.</param>
        /// <returns></returns>
        public static string GetBuildVersion(string debugValue, string releaseValue) {
#if DEBUG
            return debugValue;
#else
            return releaseValue;
#endif
        }
    }
}
