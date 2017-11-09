using SharedClasses;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class RespondentDetails : AuthenticatedPage
    {
        protected const int DATA_PRIMARY = 0;
        protected const int DATA_ALTERNATES = 1;
        protected const int DATA_SURVEYS = 2;
        protected const int DATA_SCORES = 2;

        protected DataSet Data = null;

        public int? RespondentEncoreNumber
        {
            get
            {
                object data = Page.RouteData.Values["respid"];
                if (data != null)
                {
                    string num = data.ToString();
                    //Check if it's all digits
                    if (Regex.IsMatch(num, @"^\d+$"))
                    {
                        return num.StringToInt(-1000);
                    }
                }
                return null;
            }
        }

        public string RespondentEmail
        {
            get
            {
                return RequestVars.Get<string>("e", null);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Respondent Details";
            Master.HideAllFilters = true;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (RespondentEmail != null || RespondentEncoreNumber != null)
            {
                SQLDatabase sql = new SQLDatabase();
                SQLParamList sqlParams = new SQLParamList()
                                            .Add("@EncoreNum", RespondentEncoreNumber)
                                            .Add("@Email", RespondentEmail);
                Data = sql.ExecStoredProcedureDataSet("[spReports_RespondentDetails]", sqlParams);
            }
        }

        public static string GenerateRespondentDetailsLink(string encoreNumberOrEmail)
        {
            if (!String.IsNullOrWhiteSpace(encoreNumberOrEmail))
            {
                if (Regex.IsMatch(encoreNumberOrEmail, @"^\d+$"))
                {
                    return "<a href=\"/GuestDetails/" + encoreNumberOrEmail + "\" title=\"View this Player's Information\">" + ReportingTools.CleanData(encoreNumberOrEmail) + "</a>";
                }
                else if (Regex.IsMatch(encoreNumberOrEmail, "^.+@.+$"))
                {
                    return "<a href=\"/GuestDetails/?e=" + encoreNumberOrEmail + "\" title=\"View this Player's Information\">" + ReportingTools.CleanData(encoreNumberOrEmail) + "</a>";
                }
            }
            return String.Empty;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.All(Char.IsDigit))
            {
                //Encore #
                Response.Redirect("/GuestDetails/" + txtSearch.Text);
            }
            else
            {
                //Email address
                Response.Redirect("/GuestDetails/?e=" + Server.UrlEncode(txtSearch.Text));
            }
        }
    }
}