using SharedClasses;
using System;
using System.Data;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin
{
    public partial class AbandonmentReport : AuthenticatedPage
    {
        protected DataTable Data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.HideDateRangeFilter = false;
            Master.HideRegionFilter = true;
            Master.HidePropertyFilter = true;
            Master.HideSurveyTypeFilter = true;
            Master.HideBusinessUnitFilter = true;
            Master.HideSourceFilter = true;
            Master.HideStatusFilter = true;
            Master.HideFeedbackAgeFilter = true;
            Master.HideFBVenueFilter = true;
            Master.HideEncoreNumberFilter = true;
            Master.HidePlayerEmailFilter = true;
            Master.HideAgeRangeFilter = true;
            Master.HideGenderFilter = true;
            Master.HideLanguageFilter = true;
            Master.HideVisitsFilter = true;
            Master.HideSegmentsFilter = true;
            Master.HideTenureFilter = true;
            Master.HideTierFilter = true;
            Master.HideTextSearchFilter = true;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            SQLParamList sqlParams = Master.GetFilters();
            DataTable dt = sql.ExecStoredProcedureDataTable("spAdmin_Abandonment", sqlParams);
            if (!sql.HasError)
            {
                Data = dt;
            }
            else
            {
                TopMessage.ErrorMessage = "We were unable to pull this information from the database.";
            }
        }
    }
}