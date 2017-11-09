using SharedClasses;
using System;
using WebsiteUtilities;

namespace GCC_Web_Portal.Errors
{
    public partial class GeneralError : BasePage
    {
        public string ErrorCode
        {
            get
            {
                int errorCode = RequestVars.Get("e", 0);
                if (errorCode == 0)
                {
                    return String.Empty;
                }
                else
                {
                    return errorCode.ToString();
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}