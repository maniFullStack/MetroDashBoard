using SharedClasses;
using System;
using System.Data;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class RespondentProfile : AuthenticatedPage
    {
        protected DataTable Data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Respondent Profile Report";
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();
            SQLParamList sqlParams = Master.GetFilters();
            DataSet ds = sql.ExecStoredProcedureDataSet("[spReports_RespondentProfile]", sqlParams);
            if (!sql.HasError)
            {
                Data = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 1)
                {
                    Master.RecordCount = ds.Tables[0].Rows[1]["TotalRecords"].ToString();
                }
            }
        }
    }
}