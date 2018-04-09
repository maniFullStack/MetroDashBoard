using SharedClasses;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class SurveyMaster : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Gets the property short code for the current request.
        /// </summary>
        public GCCPropertyShortCode PropertyShortCode
        {
            get
            {
                if (PropertyShortCodeOverride != null)
                {
                    return PropertyShortCodeOverride();
                }
                else
                {
                    return OriginalPropertyShortCode;
                }
            }
        }

        /// <summary>
        /// Returns the master property short code ignoring the PropertyShortCodeOverride.
        /// </summary>
        public GCCPropertyShortCode OriginalPropertyShortCode
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

        public GCCPropertyShortCode ForceSpecificProperty { get; set; }

        /// <summary>
        /// If true, the page content will be hidden. Meant to be used in conjunction with the TopMessage control.
        /// </summary>
        public bool HideContent { get; set; }

        /// <summary>
        /// Message shown above the content but below the header image.
        /// </summary>
        public MessageManager TopMessage
        {
            get
            {
                return mmTopMessage;
            }
        }

        /// <summary>
        /// If set to true, the email PIN will be verified to exist in Page_Init.
        /// </summary>
        public bool IsEmailOnlySurvey { get; set; }

        /// <summary>
        /// Contains the email PIN data. Loaded on master PreInit.
        /// </summary>
        public DataRow EmailPINRow { get; set; }

        /// <summary>
        /// Gets the GUID PIN for the email surveys.
        /// </summary>
        public Guid EmailPIN
        {
            get
            {
                object pin = Page.RouteData.Values["pin"];
                if (pin != null)
                {
                    Guid gPin;
                    if (Guid.TryParse(pin.ToString(), out gPin))
                    {
                        return gPin;
                    }
                    return Guid.Empty;
                }
                else
                {
                    return Guid.Empty;
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
        /// The property associated with this survey.
        /// </summary>
        public GCCProperty ThemeProperty
        {
            get
            {
                return (GCCProperty)PropertyID;
            }
        }

        /// <summary>
        /// Gets the casino's name. Short for calling SharedClasses.Config.GetCasinoName(PropertyID).
        /// </summary>
        public string CasinoName
        {
            get
            {
                return SharedClasses.PropertyTools.GetCasinoName(PropertyID);
            }
        }


        /// <summary>
        /// Gets the casino's name. Short for calling SharedClasses.Config.GetCasinoName(PropertyID).
        /// </summary>
        public string CasinoNameFrench
        {
            get
            {
                return SharedClasses.PropertyTools.GetCasinoName_French(PropertyID);
            }
        }

        /// <summary>
        /// Gets the current survey page.
        /// </summary>
        public int CurrentPage
        {
            get
            {
                object page = Page.RouteData.Values["page"];
                if (page != null)
                {
                    return Conversion.StringToInt(page.ToString(), 1);
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// Gets or sets the flag to determine if the survey has been completed.
        /// </summary>
        public bool SurveyComplete { get; set; }

        /// <summary>
        /// Returns 1 or -1 depending on which direction the redirect is going. This is used to manage skips during presses of the Prev / Next buttons.
        /// </summary>
        public int RedirectDirection
        {
            get
            {
                object rdo = Page.RouteData.Values["redirectdirection"];
                if (rdo == null) { return 1; }
                int rd = Conversion.StringToInt(rdo.ToString(), 1);
                return (rd == 1 || rd == -1) ? rd : 1;
            }
        }

        public Func<GCCPropertyShortCode> PropertyShortCodeOverride { get; set; }

        public SurveyMaster()
        {
            ForceSpecificProperty = GCCPropertyShortCode.None;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!EmailPIN.Equals(Guid.Empty) || IsEmailOnlySurvey)
            {
                if (EmailPIN.Equals(Guid.Empty))
                {
                    TopMessage.ErrorMessage = "Invalid link. Please ensure you copied the full link into the address bar.";
                    HideContent = true;
                    return;
                }
                SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
                DataTable dt = sql.QueryDataTable(@"
                    SELECT [BatchID],[EmailAddress],[PropertyID],[Encore],[PIN],[SurveyCompleted]
                    FROM [tblSurveyGEI_EmailPINs]
                    WHERE [PIN] = @PIN", new SqlParameter("@PIN", EmailPIN));
                if (sql.HasError)
                {
                    TopMessage.ErrorMessage = "Unable to verify link. Please try again.";
                    HideContent = true;
                    return;
                }
                else if (dt.Rows.Count == 0)
                {
                    TopMessage.ErrorMessage = "Invalid link. Please ensure you copied the full link into the address bar.";
                    HideContent = true;
                    return;
                }
                else if (dt.Rows[0]["SurveyCompleted"].Equals(true))
                {
                    ForceSpecificProperty = (GCCPropertyShortCode)dt.Rows[0]["PropertyID"].ToString().StringToInt(0);
                    TopMessage.InfoMessage = "It looks like you have already completed the survey. Thank you!";
                    HideContent = true;
                    return;
                }
                else
                {
                    EmailPINRow = dt.Rows[0];
                    if (!IsPostBack)
                    {
                        ForceSpecificProperty = (GCCPropertyShortCode)EmailPINRow["PropertyID"].ToString().StringToInt(0);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}