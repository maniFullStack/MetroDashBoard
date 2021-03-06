﻿using SharedClasses;
using System;
using System.Data;
using WebsiteUtilities;

namespace GCC_Web_Portal.Reports.Hotel
{
    public partial class PRS : PropertyDashboardPage
    {
        protected DataTable ChartData = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            ForceSpecificProperty = GCCPropertyShortCode.RR;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Hotel PRS Dashboard &raquo; " + PropertyTools.GetCasinoName((int)PropertyShortCode);
            Master.HideDateRangeFilter = false;
            Master.HideRegionFilter = true;
            Master.HidePropertyFilter = true;
            Master.HideSurveyTypeFilter = true;
            Master.HideBusinessUnitFilter = true;
            Master.HideSourceFilter = true;
            Master.HideStatusFilter = false;
            Master.HideFeedbackAgeFilter = true;
            Master.HideFBVenueFilter = true;
            Master.HideEncoreNumberFilter = false;
            Master.HidePlayerEmailFilter = false;
            Master.HideAgeRangeFilter = true;
            Master.HideGenderFilter = true;
            Master.HideLanguageFilter = true;
            Master.HideVisitsFilter = true;
            Master.HideSegmentsFilter = false;
            Master.HideTenureFilter = false;
            Master.HideTierFilter = false;
            Master.HideTextSearchFilter = false;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            SQLParamList sqlParams = GetFilters();
            DataTable dt = sql.ExecStoredProcedureDataTable("[spReports_Hotel_PRS]", sqlParams);
            if (!sql.HasError)
            {
                ChartData = dt;
                if (ChartData.Rows.Count > 1)
                {
                    Master.RecordCount = ChartData.Rows[0]["TotalRecords"].ToString();
                }
            }
        }
    }
}