using System;
using System.Web.UI;
using WebsiteUtilities;

namespace SharedClasses
{
    public class MasterPageWithUser : MasterPage {
        protected UserInfo User;
        protected override void OnInit(EventArgs e) {
            //Check HTTPS first.
            BasePage.VerifyHTTPS(Request, Response);

            //Attach the user to the master page
            try {
                User = ( (AuthenticatedPage<UserInfo>)this.Page ).User;
            } catch {
                User = SessionWrapper.Get<UserInfo>( "UserInfo", null );
            }

            base.OnInit(e);
            
        }
    }
}
