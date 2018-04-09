using SharedClasses;
using System;
using System.Data;
using System.IO;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class SnapshotStatus : AuthenticatedPage
    {
        protected DataTable Data = null;

        private bool _runExport = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Snapshot Status Report";
            Master.HideRegionFilter = true;
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
            SQLParamList sqlParams = new SQLParamList();
            var fltProperty = Master.GetFilter<ReportFilterListBox>("fltProperty");
            fltProperty.AddToQuery(sqlParams);
            DataTable dt = sql.ExecStoredProcedureDataTable("[spReports_SnapshotStatus]", sqlParams);
            if (!sql.HasError)
            {
                Data = dt;
                if (_runExport)
                {
                    string fileName = String.Format("SnapshotStatus-{0}.csv", ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss", User));
                    string fullPath = Server.MapPath(Path.Combine(Config.CacheFileDirectory, fileName));
                    if (Data.DataTableToCSV(fullPath, true))
                    {
                        Response.Clear();
                        Response.ContentType = "text/csv";
                        Response.AddHeader("content-disposition", String.Format(@"attachment;filename=""{0}""", fileName));
                        Response.WriteFile(fullPath);
                        Response.End();
                    }
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            _runExport = true;
        }
    }
}