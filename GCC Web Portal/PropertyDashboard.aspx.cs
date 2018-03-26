using GCC_Web_Portal.Controls;
using SharedClasses;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class PropertyDashboard : AuthenticatedPage
    {
        protected const int DATA_CURRENT_STATS = 0;
        protected const int DATA_MONTHLY_STATS = 1;
        protected const int DATA_FEEDBACK = 2;
        protected const int DATA_HOTEL = 3;
        protected const int DATA_TOTAL = 4;

        protected DataSet Data = null;
        protected DataRow StatRow = null;
        
        protected string sDataGCC = null;
        protected string sDataProp = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Property Dashboard &raquo; " + PropertyTools.GetCasinoName((int)PropertyShortCode);
            Master.HidePropertyFilter = true;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            DateRangeFilterControl dr = Master.Filters[0] as DateRangeFilterControl;
            if (dr != null)
            {
                bool change = false;
                if (dr.BeginDate.HasValue && dr.EndDate.HasValue)
                {
                    TimeSpan ts = dr.EndDate.Value - dr.BeginDate.Value;
                    if (ts.TotalDays < 90 || ts.TotalDays > 90)
                    {
                        change = true;
                    }
                }
                else
                {
                    change = true;
                }
                if (change)
                {
                    dr.BeginDate = DateTime.Now.AddMonths(-5).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
                    dr.EndDate = DateTime.Now.Date;
                    dr.Save();
                    TopMessage.WarningMessage = "In order to show more meaningful data, the date filter has been updated to show the past six months.";
                }
            }

          

            SQLDatabase sql = new SQLDatabase();
            SQLParamList sqlParams = Master.GetFilters();
            if (Master.IsPropertyUser)
            {
                SQLParamList sp2 = new SQLParamList();
                foreach (SqlParameter sp in sqlParams.ToArray())
                {
                    if (sp.ParameterName.Equals("@Property"))
                    {
                        sp2.Add("@PropertyID", (int)User.PropertyShortCode);
                    }
                    else
                    {
                        sp2.Add(sp);
                    }
                }
                sqlParams = sp2;
            }
            else
            {
                sqlParams.Add("@PropertyID", (int)PropertyShortCode);
            }
            DataSet ds = sql.ExecStoredProcedureDataSet("[spReports_PropertyDashboard]", sqlParams);
            if (!sql.HasError)
            {
                Data = ds;
                if (Data.Tables.Count > DATA_CURRENT_STATS)
                {
                    StatRow = Data.Tables[DATA_CURRENT_STATS].Rows[0];
                }
                if (Data.Tables[DATA_TOTAL].Rows.Count > 0)
                {
                    Master.RecordCount = ds.Tables[DATA_TOTAL].Rows[0]["TotalRecordsForFilter"].ToString();
                }
            }           
        }              

        //WORK IN PROGRESS (To move C# to server side away from client)
        protected string LoadData()
        {
            DataTable mnthDT = Data.Tables[DATA_MONTHLY_STATS];

            //Grab the last 5 months and place them in a string array
            DateTime startDate = DateTime.Now.AddMonths(-4).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
            DateTime endDate = DateTime.Now.Date;





            //DateTime startDate = Master.GetFilters();
            //DateTime endDate = DateTime.Now.Date;
            var months = Enumerable.Range(0, 5).Select(startDate.AddMonths).Select(m => m.ToString("yyyy-MM")).ToList();

            ArrayList aDataGCC = new ArrayList();
            ArrayList aDataProp = new ArrayList();

            //Placeholder variable to temporarily store the selected index month for comparison
            var selectedMonth = months[0];

            if (mnthDT.Rows.Count > 0)
            {               
                for (int i = 4; i < mnthDT.Columns.Count; i++)
                {
                    aDataGCC = new ArrayList();
                    aDataProp = new ArrayList();

                    //Initiate Case Switch and restart at 1 for each graph
                    int caseSwitch = 1;

                    for (int j = 0; j < mnthDT.Rows.Count; j++)
                    {
                        DataRow dr = mnthDT.Rows[j];
                        double dblVal = dr[i].ToString().StringToDbl(-100000);
                        string val = dblVal == -100000 ? "null" : String.Format("{0:0.0}", dblVal);

                        //Initiate the while loop condition and restart for each row in the Data Table
                        var retry = true;

                        //Store the month from the Database row in a temp string
                        string datemonth = mnthDT.Rows[j]["DateMonth"].ToString();

                        //This loop will cover two sets of data: first, the overall GCC scores, and second, the property scores
                        if (dr["PropertyID"].Equals(1))
                        {
                            aDataGCC.Add(val);
                        }
                        else
                        {
                            while (retry)
                            {
                                retry = false;
                                switch (caseSwitch)
                                {
                                    case 1:
                                        selectedMonth = months[0];
                                        break;
                                    case 2:
                                        selectedMonth = months[1];
                                        break;
                                    case 3:
                                        selectedMonth = months[2];
                                        break;
                                    case 4:
                                        selectedMonth = months[3];
                                        break;
                                    case 5:
                                        selectedMonth = months[4];
                                        break;
                                }
                                if (selectedMonth == datemonth)
                                {
                                    aDataProp.Add(val);                                    
                                    caseSwitch++;
                                    retry = false;
                                }
                                else
                                {
                                    aDataProp.Add(",0.0");
                                    //aDataProp.AppendFormat(",{0}", "0.0");
                                    caseSwitch++;
                                    retry = true;
                                }
                            } 
                        }
                  
                        
                    }
                   
                    
                }
            } //END of if count > 0 
            sDataGCC = aDataGCC.ToString().TrimStart(',');
            sDataProp = aDataProp.ToString().TrimStart(',');
            return sDataGCC;
            //return sDataProp;
            
        } //End of LOAD DATA
    }
}