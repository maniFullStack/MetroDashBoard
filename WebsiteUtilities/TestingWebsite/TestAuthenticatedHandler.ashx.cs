using System.Web;
using System.Web.Services;
using WebsiteUtilities;

namespace TestingWebsite {
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TestAuthenticatedHandler : AuthenticatedHandler<UserInfoDerived> {

        public override void ProcessRequest(HttpContext context) {
            base.ProcessRequest(context);
            context.Response.Write("Authenticated");
            context.Response.End();
        }

    }
}
