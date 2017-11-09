using SharedClasses;
using System;
using System.Web.UI;


namespace GCC_Web_Portal
{
    public partial class TAndCs_French : System.Web.UI.Page
    {

        /// <summary>
        /// Gets the property short code for the current request.
        /// </summary>
        public GCCPropertyShortCode PropertyShortCode
        {
            get
            {
                object property = Page.RouteData.Values["propertyshortcode"];
                if (property != null)
                {
                    GCCPropertyShortCode sc;
                    if (Enum.TryParse<GCCPropertyShortCode>(property.ToString().ToUpper(), out sc))
                    {
                        return sc;
                    }
                    return GCCPropertyShortCode.GCC;
                }
                else
                {
                    return GCCPropertyShortCode.GCC;
                }
            }
        }

        /// <summary>
        /// Gets the property ID to load the theme CSS file for.
        /// </summary>
        public int PropertyID
        {
            get
            {
                return (int)PropertyShortCode;
            }
        }

        /// <summary>
        /// Gets the casino's name. Short for calling SharedClasses.Config.GetCasinoName(PropertyID).
        /// </summary>
        protected string CasinoName
        {
            get
            {
                return SharedClasses.PropertyTools.GetCasinoName(PropertyID);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}