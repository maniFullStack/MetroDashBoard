using SharedClasses;
using System;
using System.Web;
using System.Web.SessionState;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    /// <summary>
    /// Summary description for Director
    /// </summary>
    public class Director : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            UserInfo ui = SessionWrapper.Get<UserInfo>("UserInfo", null);
            if (ui != null)
            {
                string redir = RequestVars.Get("rd", String.Empty);
                string redirSuffix = String.Empty;
                if (!String.IsNullOrEmpty(redir))
                {
                    redirSuffix = "?rd=" + HttpContext.Current.Server.UrlEncode(redir);
                }

                if (ui.PasswordExpireDate <= DateTime.Now)
                {
                    //Password update required
                    context.Response.Redirect("/PasswordChange" + redirSuffix, true);
                }
                else
                {
                    //Carry on
                    if (!String.IsNullOrEmpty(redir))
                    {
                        context.Response.Redirect(redir);
                    }
                    else
                    {
                        context.Response.Redirect("/");
                    }
                }
            }
            else
            {
                //Not logged in
                context.Response.Redirect("/Login", true);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}