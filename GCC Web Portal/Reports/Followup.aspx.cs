using SharedClasses;
using System;
using System.Data;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class FollowupReport : PropertyDashboardPage
    {
        protected DataTable Data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Followup Dashboard &raquo; " + PropertyTools.GetCasinoName((int)PropertyShortCode);
            Master.HidePropertyFilter = true;
            if (PropertyShortCode == GCCPropertyShortCode.GCC)
            {
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            sql.CommandTimeout = 120;
            SQLParamList sqlParams = GetFilters();
            DataTable dt = sql.ExecStoredProcedureDataTable("[spReports_Followup]", sqlParams);
            if (!sql.HasError)
            {
                Data = dt;
            }
        }

        protected string GetPercentage(string value, string total)
        {
            double val = Conversion.StringToDbl(value);
            double ttl = Conversion.StringToDbl(total);
            if (ttl == 0)
            {
                return "-";
            }
            else
            {
                return ReportingTools.FormatPercent(val / ttl, 1);
            }
        }
    }
}