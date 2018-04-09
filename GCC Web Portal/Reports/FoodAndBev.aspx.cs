using SharedClasses;
using System;
using System.Data;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class FoodAndBev : PropertyDashboardPage
    {
        protected DataTable Data = null;
        protected DataSet DataFull = null;

        /// <summary>
        /// Gets the currently selected mention.
        /// </summary>
        protected int SelectedMention
        {
            get
            {
                return ddlSelectedLocation.SelectedIndex + 1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Food & Beverage Dashboard &raquo; " + PropertyTools.GetCasinoName((int)PropertyShortCode);
            Master.HidePropertyFilter = true;
            if (!IsPostBack && PropertyShortCode == GCCPropertyShortCode.GCC)
            {
                //Set up the restaurant list if we're looking at all locations
                SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
                SQLParamList sqlParams = GetFilters();
                DataTable dt = sql.QueryDataTable(
                    @"SELECT [RestaurantID]
                            ,[Name]
                    FROM [GCC].[dbo].[tblRestaurants]
                    WHERE RestaurantID IN (SELECT RestaurantID FROM tblPropertyRestaurants WHERE PropertyID = @PropertyID)
	                OR @PropertyID = 1
                    ORDER BY Name", sqlParams);
                if (!sql.HasError)
                {
                    ddlSelectedLocation.Items.Clear();
                    foreach (DataRow dr in dt.Rows)
                    {
                        ddlSelectedLocation.Items.Add(new ListItem(dr["Name"].ToString(), dr["RestaurantID"].ToString()));
                    }
                }
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            sql.CommandTimeout = 120;
            SQLParamList sqlParams = GetFilters();
            if (PropertyShortCode == GCCPropertyShortCode.GCC)
            {
                sqlParams.Add("@RestaurantID", ddlSelectedLocation.SelectedValue);
            }
            DataSet ds = sql.ExecStoredProcedureDataSet("[spReports_FoodAndBev]", sqlParams);
            if (!sql.HasError)
            {
                DataFull = ds;
                //Set the dashboard counts
                if (PropertyShortCode == GCCPropertyShortCode.GCC)
                {
                    //Set it up if we're looking at all locations
                    Data = ds.Tables[1];
                }
                else
                {
                    Data = ds.Tables[0];
                    //Set it up if we're looking at a particular location
                    if (!IsPostBack)
                    {
                        for (int i = 0; i < 13; i++)
                        {
                            string name;
                            if (PropertyTools.HasFoodAndBev(PropertyShortCode, i + 1, out name))
                            {
                                ddlSelectedLocation.Items[i].Enabled = true;
                                ddlSelectedLocation.Items[i].Text = name;
                            }
                            else
                            {
                                ddlSelectedLocation.Items[i].Enabled = false;
                            }
                        }
                    }
                }
                if (Data.Rows.Count > 0)
                {
                    string suffix = PropertyShortCode == GCCPropertyShortCode.GCC ? String.Empty : "_M" + SelectedMention;
                    Master.RecordCount = Data.Rows[0]["Count" + suffix].ToString();
                }
            }
        }
    }
}