using SharedClasses;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin
{
    public partial class WinnerSelection : AuthenticatedPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime firstMonth = new DateTime(2015, 6, 1);
                int months = ((DateTime.Now.Year - firstMonth.Year) * 12) + DateTime.Now.Month - firstMonth.Month;
                ddlMonthYear.Items.Clear();
                for (int i = months - 1; i >= 0; i--)
                {
                    ddlMonthYear.Items.Add(new ListItem(firstMonth.AddMonths(i).ToString("MMMM, yyyy"), firstMonth.AddMonths(i).ToString("yyyy-MM")));
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.HideAllFilters = true;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string[] dateDetails = ddlMonthYear.SelectedValue.Split('-');
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            SQLParamList sqlParams = new SQLParamList()
                                            .Add("@Year", dateDetails[0].StringToInt())
                                            .Add("@Month", dateDetails[1].StringToInt());
            SqlParameter isNewParam = new SqlParameter("@IsNew", SqlDbType.Bit);
            isNewParam.Direction = ParameterDirection.Output;
            sqlParams.Add(isNewParam);
            DataTable dt = sql.ExecStoredProcedureDataTable("spAdmin_GetGEIWinners", sqlParams);
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Unable to query the details from the database.";
            }
            else
            {
                string fileName = String.Format("WinnerSelection-{0}.csv", ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss", User));
                string fullPath = Server.MapPath(Path.Combine(Config.CacheFileDirectory, fileName));
                if (dt.DataTableToCSV(fullPath, true))
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
}