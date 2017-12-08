using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SharedClasses;

namespace GCC_Web_Portal
{
    public partial class TandCGTA : System.Web.UI.Page
    {
        public GCCPropertyShortCode ForceSpecificProperty { get; set; }
       
        /// <summary>
        /// Gets the property short code for the current request.
        /// </summary>
        public GCCPropertyShortCode PropertyShortCode
        {
            get
            {
                if (ForceSpecificProperty != GCCPropertyShortCode.None)
                {
                    return ForceSpecificProperty;
                }
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
    }
}