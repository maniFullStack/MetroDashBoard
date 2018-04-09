using SharedClasses;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class DisplaySurvey : AuthenticatedPage
    {
        public readonly string[] ANSWERS_SATISFACTION = new string[] { "N/A", "Very Dissatisfied", "Dissatisfied", "Satisfied", "Very Satisfied", "Extremely Satisfied" };
        public readonly string[] ANSWERS_WOULD = new string[] { "N/A", "Definitely Would Not", "Probably Would Not", "Might or Might Not", "Probably Would", "Definitely Would" };
        public readonly string[] ANSWERS_EXCELLENT = new string[] { "N/A", "Poor", "Fair", "Good", "Very Good", "Excellent" };
        public readonly string[] ANSWERS_YESNO = new string[] { "N/A", "No", "Yes" };
        public readonly string[] ANSWERS_TRI_LIKELY = new string[] { "N/A", "Not Likely", "Possibly", "Very Likely" };
        public readonly string[] ANSWERS_TRI_IMPORTANT = new string[] { "N/A", "Not Important", "Somewhat Important", "Very Important" };

        public DataRow Data = null;

        /// <summary>
        /// The current survey type for handling how it is displayed.
        /// </summary>
        public SurveyType SurveyType
        {
            get
            {
                object property = Page.RouteData.Values["surveytype"];
                if (property != null)
                {
                    SurveyType sc;
                    if (Enum.TryParse<SurveyType>(property.ToString(), out sc))
                    {
                        return sc;
                    }
                    return SurveyType.None;
                }
                else
                {
                    return SurveyType.None;
                }
            }
        }

        public int RecordID
        {
            get
            {
                object recordID = Page.RouteData.Values["recordid"];
                if (recordID != null)
                {
                    return recordID.ToString().StringToInt(-1);
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Gets the property short code after the data has been loaded.
        /// </summary>
        new public GCCPropertyShortCode PropertyShortCode
        {
            get
            {
                if (Data == null)
                {
                    return GCCPropertyShortCode.None;
                }
                else
                {
                    GCCPropertyShortCode sc = (GCCPropertyShortCode)Data["PropertyID"];
                    if (SurveyType == SharedClasses.SurveyType.Feedback && sc == GCCPropertyShortCode.GCC)
                    {
                        return (GCCPropertyShortCode)Data["SelectedPropertyID"];
                    }
                    else
                    {
                        return sc;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the property ID after the data has been loaded.
        /// </summary>
        public int PropertyID
        {
            get
            {
                if (Data == null)
                {
                    return 0;
                }
                else
                {
                    return (int)PropertyShortCode;
                }
            }
        }

        /// <summary>
        /// Gets the name of the casino after the data has been loaded.
        /// </summary>
        public string CasinoName
        {
            get
            {
                return PropertyTools.GetCasinoName((int)PropertyShortCode);
            }
        }

        public bool IsKioskOrStaffEntry
        {
            get
            {
                if (Data == null)
                {
                    return false;
                }
                else
                {
                    return GEISurveyType == GEISurveyType.Kiosk || GEISurveyType == GEISurveyType.StaffSurvey;
                }
            }
        }

        public GEISurveyType GEISurveyType
        {
            get
            {
                if (Data == null)
                {
                    return GEISurveyType.DirectAccess;
                }
                else
                {
                    return (GEISurveyType)Data["SurveyType"];
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SurveyType != SurveyType.None)
            {
                SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
                DataTable dt = sql.ExecStoredProcedureDataTable("[spReports_Survey_GetData]", new SqlParameter("@SurveyType", (int)SurveyType), new SqlParameter("@RecordID", RecordID));
                if (!sql.HasError && dt.Rows.Count == 1)
                {
                    Data = dt.Rows[0];

                    //Check permissions
                    if ((User.Group == UserGroups.PropertyManagers && User.PropertyShortCode != PropertyShortCode && User.PropertyShortCode != GCCPropertyShortCode.None)
                        || (User.Group == UserGroups.PropertyStaff && User.PropertyShortCode != PropertyShortCode)
                    )
                    {
                        Data = null;
                        TopMessage.ErrorMessage = "You do not have permission to view these survey responses.";
                        TopMessage.TitleOverride = "Unauthorized";
                        return;
                    }

                    Master.ForceSpecificProperty = PropertyShortCode;
                }
                switch (SurveyType)
                {
                    case SurveyType.GEI:
                        Title = "GEI Survey";
                        break;

                    case SurveyType.Hotel:
                        Title = "Hotel Survey";
                        break;

                    case SurveyType.Feedback:
                        Title = "Feedback Survey";
                        break;

                    case SurveyType.Donation:
                        Title = "Donation Survey";
                        break;
                }
            }
        }

        public string GetAnswerValue(object dataRowValue, string[] answers)
        {
            return GetAnswerValue(dataRowValue, answers, false);
        }

        public string GetAnswerValue(object dataRowValue, string[] answers, bool isYesNo)
        {
            int val = dataRowValue.ToString().StringToInt(-2);
            //Yes / no questions are stored as -1,0,1
            if (isYesNo && val != -2)
            {
                val++;
            }
            if (val == -2)
            {
                return String.Empty;
            }
            else
            {
                return answers[val].Replace(" ", "&nbsp;");
            }
        }

        public string GetFoodAndBevName(int mention)
        {
            return PropertyTools.GetFoodAndBevName(PropertyShortCode, mention);
        }

        public string GetShowLoungeName()
        {
            return GetShowLoungeName(false);
        }

        public string GetShowLoungeName(bool checkHRCV)
        {
            return PropertyTools.GetShowLoungeName(PropertyShortCode, ((!checkHRCV || (String.IsNullOrEmpty(Data["Q21_HRCV_Lounge"].ToString()) && String.IsNullOrEmpty(Data["Q21_EC_Lounge"].ToString()))) ? 0 : (Data["Q21_HRCV_Lounge"].Equals("Asylum") || Data["Q21_EC_Lounge"].Equals("Molson Lounge") ? 1 : 2)));
        }
    }
}