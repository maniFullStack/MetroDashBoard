using SharedClasses;
using System;
using System.Data;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class Staff : PropertyDashboardPage
    {
        protected DataTable Data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC GSEI Dashboard &raquo; " + PropertyTools.GetCasinoName((int)PropertyShortCode);
            Master.HidePropertyFilter = true;
        }    

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            SQLParamList sqlParams = GetFilters();
            DataTable dt = sql.ExecStoredProcedureDataTable("[spReports_Staff]", sqlParams);
            if (!sql.HasError)
            {
                Data = dt;
                if (dt.Rows.Count > 1)
                {
                    Master.RecordCount = dt.Rows[1]["TotalRecords"].ToString();
                }
            }
        }
    }
}