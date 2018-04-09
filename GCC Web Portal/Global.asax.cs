using GCGC_Web_Portal;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //If the session doesn't exist, culture probably doesn't apply to this request. For example, this will happen when favicon.ico is requested.
            if (HttpContext.Current.Session == null)
            {
                return;
            }

            //Figure out the culture of the current request.
            string cultureName;
            //See if we have a session variable. If not, check for a cookie from last time.
            if (!SessionWrapper.Exists("DisplayLanguage"))
            {
                //No session, let's check the cookie
                HttpCookie cook = HttpContext.Current.Request.Cookies["DisplayLanguage"];
                if (cook != null && cook.Value.Equals("fr-CA"))
                {
                    //Found French cookie
                    cultureName = "fr-CA";
                }
                else
                {
                    //No cookie or cookie not French, default to English-Canada
                    cultureName = "en-CA";
                }
                //Save in session for future requests
                SessionWrapper.Add("DisplayLanguage", cultureName);
            }
            else
            {
                //Session value exists, load it
                cultureName = SessionWrapper.Get("DisplayLanguage", "en-CA");
            }

            //Don't do anything if it's the same culture
            if (Thread.CurrentThread.CurrentUICulture.Name == cultureName)
                return;

            //Set the thread culture
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
        }

        private void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
        }

        private void Application_Error(object sender, EventArgs e)
        {
            //Log ALL uncaught exceptions
            Exception exc = Server.GetLastError();
            if (exc is HttpUnhandledException)
            {
                exc = Context.Error.InnerException;
            }

            int dberrid = ErrorHandler.WriteLog("Uncaught", "Uncaught exception was unhandled.",
                ErrorHandler.ErrorEventID.General, exc);
            if (dberrid > -1)
            {
                SessionWrapper.Add("CaughtExceptionID", dberrid);
            }
        }

        private void Session_End(object sender, EventArgs e)
        {
            object geiPageNumber = Session["GEIPageNumber"];
            if (geiPageNumber != null)
            {
                int pageNumber = (int)geiPageNumber;
                SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
                sql.NonQuery(@"INSERT INTO [tblSurveyGEI_Abandonment] ([DateCreated], [PageNumber]) VALUES (GETDATE(), @PageNumber)", new SQLParamList().Add("@PageNumber", pageNumber));
            }
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }

        private void Application_BeginRequest()
        {
#if !DEBUG
            switch ( Request.Url.Scheme ) {
                case "https":
                    Response.AddHeader( "Strict-Transport-Security", "max-age=31536000" );
                    break;

                case "http":
                    var path = "https://" + Request.Url.Host + Request.Url.PathAndQuery;
                    Response.Status = "301 Moved Permanently";
                    Response.AddHeader( "Location", path );
                    break;
            }
#endif
        }
    }
}