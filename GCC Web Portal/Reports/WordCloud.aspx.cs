using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;

//using System.Linq;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal.Reports
{
    public partial class WordCloud : AuthenticatedPage
    {
        protected bool IsHRUser
        {
            get
            {
                return User.Group == UserGroups.HRStaff;
            }
        }

        protected bool IsCorpMarketing
        {
            get
            {
                return User.Group == UserGroups.CorporateMarketing || User.Group == UserGroups.ForumAdmin;
            }
        }

        protected Dictionary<string, int> Data = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            Master.HideDateRangeFilter = false;
            Master.HideRegionFilter = false;
            Master.HidePropertyFilter = false;
            Master.HideSurveyTypeFilter = IsHRUser; //Hide for HR, they can only see snapshot
            Master.HideBusinessUnitFilter = IsHRUser;
            Master.HideSourceFilter = IsHRUser;
            Master.HideStatusFilter = IsHRUser;
            Master.HideFeedbackAgeFilter = IsHRUser;
            Master.HideFBVenueFilter = false;
            Master.HideEncoreNumberFilter = IsHRUser;
            Master.HidePlayerEmailFilter = IsHRUser;
            Master.HideAgeRangeFilter = IsHRUser;
            Master.HideGenderFilter = IsHRUser;
            Master.HideLanguageFilter = IsHRUser;
            Master.HideVisitsFilter = IsHRUser;
            Master.HideSegmentsFilter = IsHRUser;
            Master.HideTenureFilter = IsHRUser;
            Master.HideTierFilter = IsHRUser;
            Master.HideTextSearchFilter = false;

            //Allow the corp marketing team to view snapshot results
            if (IsCorpMarketing && !IsPostBack)
            {
                ReportFilterListBox fltSurveyTypeFilter = Master.GetFilter<ReportFilterListBox>("fltSurveyType");
                if (fltSurveyTypeFilter != null)
                {
                    fltSurveyTypeFilter.Items.Add(new ListItem("2015 Snapshot", "5"));
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();
            SQLParamList sqlParams = Master.GetFilters()
                                           .Add("@IsHRStaff", IsHRUser)
                                           .Add("@IsCorpMarketing", IsCorpMarketing);
            DataTable dt = sql.ExecStoredProcedureDataTable("[spReports_Wordcloud]", sqlParams);
            if (!sql.HasError)
            {
                Data = new Dictionary<string, int>();
                foreach (DataRow dr in dt.Rows)
                {
                    Data.Add(dr["Word"].ToString(), (int)dr["Count"]);
                }
            }
        }
    }
}