using SharedClasses;
using System;
using System.Configuration;
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class EmailLog : AuthenticatedPage
    {
        public static string PickupDirectory
        {
            get
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~/Web.config");
                MailSettingsSectionGroup mail = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
                if (mail.Smtp.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
                {
                    return mail.Smtp.SpecifiedPickupDirectory.PickupDirectoryLocation;
                }
                return Path.Combine(HttpRuntime.AppDomainAppPath, @"\Files\mail-drop\");
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            string filename = RequestVars.Get("f", String.Empty);
            if (!String.IsNullOrWhiteSpace(filename))
            {
                Response.Clear();
                Response.ContentType = "message/rfc822";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.WriteFile(Path.Combine(PickupDirectory, filename));
                Response.End();
                return;
            }
        }
    }
}