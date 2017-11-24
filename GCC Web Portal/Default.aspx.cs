﻿using SharedClasses;
using System;
using System.Data;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class Default : AuthenticatedPage
    {
        protected const int DATA_SCORES = 0;
        protected const int DATA_FEEDBACK = 1;
        protected const int DATA_SNAPSHOT = 2;
        protected const int DATA_FOLLOWUP = 0;

        protected DataSet Data = null;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            UserInfo user = WebsiteUtilities.SessionWrapper.Get<UserInfo>("UserInfo", null);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();
            SQLParamList sqlParams = Master.GetFilters();
            DataSet ds = sql.ExecStoredProcedureDataSet("[spReports_Main]", sqlParams);
            if (!sql.HasError)
            {
                Data = ds;
                if (ds.Tables[DATA_SCORES].Rows.Count > 0)
                {
                    Master.RecordCount = ds.Tables[DATA_SCORES].Rows[0]["TotalRecords"].ToString();
                }
            }
        }
    }
}