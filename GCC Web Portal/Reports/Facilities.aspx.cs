using SharedClasses;
using System;
using System.Data;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class Facilities : PropertyDashboardPage
    {
        protected DataTable Data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Facilities Dashboard &raquo; " + PropertyTools.GetCasinoName((int)PropertyShortCode);
            Master.HidePropertyFilter = true;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();
            SQLParamList sqlParams = GetFilters();
            DataSet ds = sql.ExecStoredProcedureDataSet("[spReports_Facilities]", sqlParams);
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