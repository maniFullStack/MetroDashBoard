using SharedClasses;
using System;
using System.Data;
using System.Web.UI;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin
{
    public partial class SurveyList : AuthenticatedPage
    {
        protected const int ROWS_PER_PAGE = 20;
        protected DataTable Data = null;

        /// <summary>
        /// Gets the current listing page.
        /// </summary>
        public int CurrentPage
        {
            get
            {
                object page = Page.RouteData.Values["page"];
                if (page != null)
                {
                    return Conversion.StringToInt(page.ToString(), 1);
                }
                else
                {
                    return 1;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Survey Listing";
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            string currentSort = RequestVars.Get("s", "D"); ; //Date
            string currentSortDir = RequestVars.Get("d", "D"); ; //Desc

            SQLDatabase sql = new SQLDatabase();
            SQLParamList sqlParams = Master.GetFilters()
                                            .Add("@Sort", currentSort)
                                            .Add("@SortDir", currentSortDir);
            if (CurrentPage == -1)
            {
                sqlParams.Add("@ShowAllRows", true);
            }
            else
            {
                sqlParams.Add("@Page", CurrentPage)
                         .Add("@RowsPerPage", ROWS_PER_PAGE);
            }
            DataTable dt = sql.ExecStoredProcedureDataTable("spReports_Survey_List", sqlParams);
            if (!sql.HasError)
            {
                Data = dt;
            }
        }

        protected string GetPaginationURL(int pageNumber)
        {
            string currentSort = RequestVars.Get("s", "F"); //Feedback status
            string sortDir = RequestVars.Get("d", "D"); ; //Desc

            return String.Format("/Admin/Surveys/{0}?s={1}&d={2}", pageNumber, currentSort, sortDir);
        }

        protected string GetSort(string sortCol, string label)
        {
            string currentSort = RequestVars.Get("s", "D"); ; //Date
            string sortDir = RequestVars.Get("d", "D"); ; //Desc

            sortDir = sortDir.Equals("A") && currentSort.Equals(sortCol) ? "D" : "A";

            return String.Format("<a href=\"/Admin/Surveys/{0}?s={1}&d={2}\">{3}</a>", CurrentPage, sortCol, sortDir, label);
        }
    }
}