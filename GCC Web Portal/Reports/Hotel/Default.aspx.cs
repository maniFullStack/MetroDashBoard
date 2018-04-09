using SharedClasses;
using System;
using System.Data;
using System.Text;
using WebsiteUtilities;

namespace GCC_Web_Portal.Reports.Hotel
{
    public partial class Default : PropertyDashboardPage
    {
        protected DataTable ChartData = null;
        protected DataTable TableData = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            ForceSpecificProperty = GCCPropertyShortCode.RR;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Hotel Dashboard &raquo; " + PropertyTools.GetCasinoName((int)PropertyShortCode);
            Master.HideDateRangeFilter = false;
            Master.HideRegionFilter = true;
            Master.HidePropertyFilter = true;
            Master.HideSurveyTypeFilter = true;
            Master.HideBusinessUnitFilter = true;
            Master.HideSourceFilter = true;
            Master.HideStatusFilter = false;
            Master.HideFeedbackAgeFilter = true;
            Master.HideFBVenueFilter = true;
            Master.HideEncoreNumberFilter = false;
            Master.HidePlayerEmailFilter = false;
            Master.HideAgeRangeFilter = true;
            Master.HideGenderFilter = true;
            Master.HideLanguageFilter = true;
            Master.HideVisitsFilter = true;
            Master.HideSegmentsFilter = false;
            Master.HideTenureFilter = false;
            Master.HideTierFilter = false;
            Master.HideTextSearchFilter = false;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            SQLParamList sqlParams = GetFilters();
            DataSet ds = sql.ExecStoredProcedureDataSet("[spReports_Hotel_Main]", sqlParams);
            if (!sql.HasError)
            {
                ChartData = ds.Tables[0];
                TableData = ds.Tables[1];
                if (ChartData.Rows.Count > 1)
                {
                    Master.RecordCount = ChartData.Rows[0]["TotalRecords"].ToString();
                }
            }
        }

        protected string GetDataRow(string label, string colPrefix, DataRow currentRow, DataRow lastMonthRow, DataRow last12MonthsRow)
        {
            StringBuilder sb = new StringBuilder("<tr><th>");
            sb.Append(label)
              .Append("</th>");
            double currentVal = currentRow[colPrefix + "Score"].ToString().StringToDbl(-1000);
            double lastMonthVal = lastMonthRow[colPrefix + "Score"].ToString().StringToDbl(-1000);
            double last12MonthsVal = last12MonthsRow[colPrefix + "Score"].ToString().StringToDbl(-1000);

            sb.AppendFormat("<td>{0}</td>", ReportingTools.FormatPercent(currentRow[colPrefix + "Score"].ToString()));
            sb.AppendFormat("<td>{0}</td>", ReportingTools.FormatPercent(lastMonthRow[colPrefix + "Score"].ToString()));

            if (currentVal == -1000 || lastMonthVal == -1000)
            {
                sb.Append("<td>-</td>");
            }
            else
            {
                sb.AppendFormat("<td>{0}</td>", ReportingTools.FormatPercent((currentVal - lastMonthVal).ToString()));
            }

            sb.AppendFormat("<td>{0}</td>", ReportingTools.FormatPercent(last12MonthsRow[colPrefix + "Score"].ToString()));

            if (currentVal == -1000 || last12MonthsVal == -1000)
            {
                sb.Append("<td>-</td>");
            }
            else
            {
                sb.AppendFormat("<td>{0}</td>", ReportingTools.FormatPercent((currentVal - last12MonthsVal).ToString()));
            }
            sb.Append("</tr>");
            return sb.ToString();
        }
    }
}