using Newtonsoft.Json.Linq;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using WebsiteUtilities;

namespace GCC_Web_Portal.Reports
{
    public partial class SocialMediaDashboard : AuthenticatedPage
    {
        protected const int DATA_CURRENT_STATS = 0;
        protected const int DATA_MONTHLY_STATS = 1;

        protected DataSet Data = null;
        protected DataRow StatRow = null;

        public string json;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Social Media Dashboard";
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

        protected void LoadData()
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            SQLParamList sqlParams = Master.GetFilters();
            Data = sql.ExecStoredProcedureDataSet("spGetSocialMediaCharts", sqlParams);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnReloadData_Click(object sender, EventArgs e)
        {
            SQLDatabase sql_keywords = new SQLDatabase();
            DataTable dt = sql_keywords.QueryDataTable("SELECT PropertyID, Keyword, Includes FROM tblGCC_SocialMediaKeywords");
            if (sql_keywords.HasError)
            {
                mmMessage.ErrorMessage = "There was a database error loading property keywords.  Please try again later.";
                return;
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string keyword = dr["Keyword"].ToString();
                    string includes = dr["Includes"].ToString();
                    int propertyID = Conversion.StringToInt(dr["PropertyID"].ToString());
                    json = new WebClient().DownloadString(String.Format("http://api.trackur.com/api/v2/?api_key=65c0c2c7-2ea2-4f7c-b50f-c9bec7b6ad7b&keyword={0}&includes={1}&limit=100", keyword, includes));
                    json = json.Trim();
                    JObject o = JObject.Parse(json);
                    if (o["results"] != null)
                    {
                        List<object> results = o["results"].ToList<object>();
                        foreach (object result in results)
                        {
                            JObject parsed_result = JObject.Parse(result.ToString());
                            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
                            SQLParamList sqlParams = new SQLParamList();
                            sqlParams.Add("@keyword", keyword);
                            sqlParams.Add("@propertyID", propertyID);
                            sqlParams.Add("@id", parsed_result["id"].ToString());
                            sqlParams.Add("@title", parsed_result["title"].ToString());
                            sqlParams.Add("@source", parsed_result["source"].ToString());

                            string published = parsed_result["published"].ToString();
                            DateTime published_date;
                            if (!DateTime.TryParse(published, null,
                                               DateTimeStyles.None, out published_date))
                            {
                                published_date = DateTime.MinValue;
                            }

                            sqlParams.Add("@published", published_date);
                            sqlParams.Add("@url", parsed_result["url"].ToString());
                            sqlParams.Add("@influence", DBNull.Value);
                            sqlParams.Add("@content", parsed_result["content"].ToString());
                            sqlParams.Add("@sentiment", parsed_result["sentiment"].ToString());
                            sqlParams.Add("@location", parsed_result["location"].ToString());
                            sql.ExecStoredProcedureDataSet("[spMergeSocialMedia]", sqlParams);
                            if (sql.HasError)
                            {
                                mmMessage.ErrorMessage = "There was a database error.  Please try again later.";
                                return;
                            }
                        }
                    }
                }
            }
            mmMessage.InfoMessage = "Data loaded successfully.";
        }
    }
}