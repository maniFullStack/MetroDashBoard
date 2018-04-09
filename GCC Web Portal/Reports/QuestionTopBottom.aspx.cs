using SharedClasses;
using System;
using System.Data;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class QuestionTopBottom : AuthenticatedPage
    {
        protected DataTable Data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Top / Bottom Question Report";
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            sql.CommandTimeout = 90;
            SQLParamList sqlParams = Master.GetFilters();
            DataTable dt = sql.ExecStoredProcedureDataTable("[spReports_TopBottomQuestions]", sqlParams);
            if (!sql.HasError)
            {
                Data = dt;
            }
        }
    }
}