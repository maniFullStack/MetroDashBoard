using GCC_Web_Portal.Controls;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class Dashboard : AuthenticatedMasterPage
    {
        private bool _hideDateRange = false;
        private bool _hideRegion = false;
        private bool _hideProperty = false;
        private bool _hideSurveyType = false;
        private bool _hideBusinessUnit = false;
        private bool _hideSource = false;
        private bool _hideStatus = false;
        private bool _hideFeedbackAge = true;
        private bool _hideFeedbackTier = true;
        private bool _hideFBVenue = false;
        private bool _hideEncoreNumber = false;
        private bool _hidePlayerEmail = false;
        private bool _hideAgeRange = false;
        private bool _hideGender = false;
        private bool _hideLanguage = false;
        private bool _hideVisits = false;
        private bool _hideSegments = false;
        private bool _hideTenure = false;
        private bool _hideTier = false;
        private bool _hideTextSearch = false;

        public int ActiveFilters
        {
            get
            {
                int cnt = 0;
                foreach (IReportFilter flt in Filters)
                {
                    if (flt.IsActive)
                    {
                        cnt++;
                    }
                }

                
                return cnt;
            }
        }

        public string RecordCount
        {
            get
            {
                return lblRecordCount.Text;
            }
            set
            {
                lblRecordCount.Text = value;
            }
        }

        public bool HideAllFilters { get; set; }

        public bool HideDateRangeFilter
        {
            get
            {
                return HideAllFilters || _hideDateRange;
            }
            set
            {
                _hideDateRange = value;
            }
        }

        public bool HideRegionFilter
        {
            get
            {
                return HideAllFilters || _hideRegion;
            }
            set
            {
                _hideRegion = value;
            }
        }

        public bool HidePropertyFilter
        {
            get
            {
                return HideAllFilters || _hideProperty;
            }
            set
            {
                _hideProperty = value;
            }
        }

        public bool HideSurveyTypeFilter
        {
            get
            {
                return HideAllFilters || _hideSurveyType;
            }
            set
            {
                _hideSurveyType = value;
            }
        }

        public bool HideBusinessUnitFilter
        {
            get
            {
                return HideAllFilters || _hideBusinessUnit;
            }
            set
            {
                _hideBusinessUnit = value;
            }
        }

        public bool HideSourceFilter
        {
            get
            {
                return HideAllFilters || _hideSource;
            }
            set
            {
                _hideSource = value;
            }
        }

        public bool HideStatusFilter
        {
            get
            {
                return HideAllFilters || _hideStatus;
            }
            set
            {
                _hideStatus = value;
            }
        }

        public bool HideFeedbackAgeFilter
        {
            get
            {
                return HideAllFilters || _hideFeedbackAge;
            }
            set
            {
                _hideFeedbackAge = value;
            }
        }

        public bool HideFeedbackTierFilter
        {
            get
            {
                return HideAllFilters || _hideFeedbackTier;
            }
            set
            {
                _hideFeedbackTier = value;
            }
        }

        public bool HideFBVenueFilter
        {
            get
            {
                return HideAllFilters || _hideFBVenue;
            }
            set
            {
                _hideFBVenue = value;
            }
        }

        public bool HideEncoreNumberFilter
        {
            get
            {
                return HideAllFilters || _hideEncoreNumber;
            }
            set
            {
                _hideEncoreNumber = value;
            }
        }

        public bool HidePlayerEmailFilter
        {
            get
            {
                return HideAllFilters || _hidePlayerEmail;
            }
            set
            {
                _hidePlayerEmail = value;
            }
        }

        public bool HideAgeRangeFilter
        {
            get
            {
                return HideAllFilters || _hideAgeRange;
            }
            set
            {
                _hideAgeRange = value;
            }
        }

        public bool HideGenderFilter
        {
            get
            {
                return HideAllFilters || _hideGender;
            }
            set
            {
                _hideGender = value;
            }
        }

        public bool HideLanguageFilter
        {
            get
            {
                return HideAllFilters || _hideLanguage;
            }
            set
            {
                _hideLanguage = value;
            }
        }

        public bool HideVisitsFilter
        {
            get
            {
                return HideAllFilters || _hideVisits;
            }
            set
            {
                _hideVisits = value;
            }
        }

        public bool HideSegmentsFilter
        {
            get
            {
                return HideAllFilters || _hideSegments;
            }
            set
            {
                _hideSegments = value;
            }
        }

        public bool HideTenureFilter
        {
            get
            {
                return HideAllFilters || _hideTenure;
            }
            set
            {
                _hideTenure = value;
            }
        }

        public bool HideTierFilter
        {
            get
            {
                return HideAllFilters || _hideTier;
            }
            set
            {
                _hideTier = value;
            }
        }

        public bool HideTextSearchFilter
        {
            get
            {
                return HideAllFilters || _hideTextSearch;
            }
            set
            {
                _hideTextSearch = value;
            }
        }

        public ReportFilterListBox StatusFilter
        {
            get
            {
                return fltStatus;
            }
        }

        public DateRangeFilterControl DateFilter
        {
            get
            {
                return fltDateRange;
            }
        }

        public IReportFilter[] Filters { get; set; }

        public bool IsPropertyUser
        {
            get
            {
                return User.Property != GCCProperty.GCCMain && User.Property != GCCProperty.None;
            }
        }






        public bool IsMultiPropertyUser
        {
            get
            {
                return true;
            }
        }



        public class PropData
{
    public int Value { get; set; }
    public string Text { get; set; }
}


        protected void Page_Init(object sender, EventArgs e)
        {
            fltDateRange.User = User;
            Filters = new IReportFilter[] {
                fltDateRange,
                fltRegion,
                fltProperty,
                fltSurveyType,
                fltBusinessUnit,
                fltSource,
                fltStatus,
                fltFeedbackAge,
                fltFeedbackTier,
                fltFBVenue,
                fltEncoreNumber,
                fltPlayerEmail,
                fltAgeRange,
                fltGender,
                fltLanguage,
                fltVisits,
                fltSegments,
                fltTenure,
                fltTier,
                fltTextSearch
            };

            if (IsPropertyUser)
            {
                fltProperty.OnClear = fltProperty_OnClear;
                fltProperty.SelectedIndexChanged += fltProperty_SelectedIndexChanged;
                fltProperty.SelectedValue = ((int)User.Property).ToString();
            }


            //if (IsMultiPropertyUser)
            //{
            //    List<GCCProperty> myprop = new List<GCCProperty>();
            //    myprop.Add(GCCProperty.CasinoNewBrunswick);
            //    fltProperty.Items.Clear();
            //    fltProperty.DataSource = myprop;
                
            //    fltProperty.DataBind();
               
            //    ////fltProperty.OnClear = fltProperty_OnClear;
            //    //List<PropData> data = new List<PropData>();
            //    //data.Add(new PropData() { Value = 2, Text = "River Rock Casino Resort" });
            //    //data.Add(new PropData() { Value = 3, Text = "Hard Rock Casino Vancouver" });

            //    //fltProperty.DataSource = data;
            //    //fltProperty.DataMember = "Text";
            //    //fltProperty.DataBind();
               

            //    //fltProperty.SelectedIndexChanged += fltProperty_SelectedIndexChanged;
            //    //fltProperty.SelectedValue = ((int)User.Property).ToString();
            //}
            //Set filters from query string
            int sf = RequestVars.Get("sf", 0);
            if (sf == 1)
            {
                foreach (IReportFilter flt in Filters)
                {
                    string getVal = RequestVars.Get<string>(flt.SessionKey, null);
                    if (!String.IsNullOrWhiteSpace(getVal))
                    {
                        ReportFilterListBox rflb = flt as ReportFilterListBox;
                        if (rflb != null)
                        {
                            string[] vals = getVal.Split(',');
                            foreach (ListItem li in rflb.Items)
                            {
                                if (vals.Contains(li.Value))
                                {
                                    li.Selected = true;
                                }
                            }
                            flt.Save();
                            continue;
                        }
                        ReportFilterTextBox rftb = flt as ReportFilterTextBox;
                        if (rftb != null)
                        {
                            rftb.Text = getVal;
                            flt.Save();
                            continue;
                        }
                        DateRangeFilterControl drfc = flt as DateRangeFilterControl;
                        if (drfc != null)
                        {
                            string[] vals = getVal.Split(',');
                            if (vals.Length == 2)
                            {
                                drfc.SetValues(vals[0], vals[1]);
                                flt.Save();
                                continue;
                            }
                        }
                        ReportFilterDropDownList rfddl = flt as ReportFilterDropDownList;
                        if (rfddl != null)
                        {
                            rfddl.SelectedValue = getVal;
                            flt.Save();
                            continue;
                        }
                    }
                }
                //Remove the query string and refresh
                Response.Redirect(Request.Path);
            }

            string rm = RequestVars.Get("rm", String.Empty);
            if (!String.IsNullOrEmpty(rm))
            {
                foreach (IReportFilter flt in Filters)
                {
                    if (flt.ID.Equals(rm))
                    {
                        flt.Clear();
                        break;
                    }
                }
                Response.Redirect(Request.Path);
            }



            // Jan 2017 Date range was default from 2016-12-01 to Jan 2017 but data was too much to load so resetting it to Jan 01,2017 to current date
            //if (DateTime.Today.Month == 1)
            //{
            //    fltDateRange.DefaultBeginDate = DateTime.Now.AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
            //}
            //else
            //{

            // connection timeout set to = 120 in Feddbacklist Page load complete
                //fltDateRange.DefaultBeginDate = DateTime.Now.AddMonths(-1).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
           // }







            fltDateRange.DefaultBeginDate = DateTime.Now.AddMonths(-1).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;




            // For Archived Portal setting up from Jan to Dec 2016
            //fltDateRange.DefaultBeginDate = new DateTime(2016, 01, 01, 00, 00, 01); ;

            fltDateRange.DefaultEndDate = DateTime.Now;
        }

        private void fltProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsPropertyUser && fltProperty.SelectedValue != ((int)User.Property).ToString())
            {
                fltProperty.SelectedValue = ((int)User.Property).ToString();
            }
        }

        private bool fltProperty_OnClear()
        {
            if (IsPropertyUser)
            {
                fltProperty.SelectedValue = ((int)User.Property).ToString();
                return true;
            }
            return false;
        }

        protected void ApplyFilters_Click(object sender, EventArgs e)
        {
            foreach (IReportFilter flt in Filters)
            {
                flt.Save();
            }


            //if (fltDateRange.BeginDate.Value.Year > 2016)
            //{

            //    Response.Write(@"<script language='javascript'>alert('Please Check archive reports')</script>");
            //}
          
        }

        public SQLParamList GetFilters()
        {
            SQLParamList sqlParams = new SQLParamList();
            foreach (IReportFilter flt in Filters)
            {
                if (!IsFilterHidden(flt)
                    || (IsPropertyUser && flt.ID.Equals("fltProperty")))
                { //Make sure the property filter is always added for the property users
                    flt.AddToQuery(sqlParams);
                }
            }
            return sqlParams;
        }

        protected bool IsFilterHidden(IReportFilter flt)
        {
            string id = flt.ID.Replace("flt", "Hide") + "Filter";
            var prop = this.GetType().GetProperty(id);
            if (prop != null)
            {
                return (bool)prop.GetValue(this, null);
            }
            else
            {
                return false;
            }
        }

        public T GetFilter<T>(string filterID)
            where T : class, IReportFilter
        {
            return FindControl(filterID) as T;
        }

        #region Filter Query Override Methods

        protected void fltStatus_AddToQuery(ReportFilterListBox control, SQLParamList sqlParams)
        {
            if (control.IsActive)
            {
                /* No Response Required = 1
                 * Response required - Active = 2
                 * Response required - Inactive = 3
                 */
                StringBuilder sb = new StringBuilder();
                foreach (ListItem li in control.Items)
                {
                    if (li.Selected)
                    {
                        if (li.Value.Equals("1"))
                        {
                            sb.Append(@"OR NOT (  Q29 = 0
                                                OR Q30 = 1 OR Q30 = 2
                                                OR Q31A = 1 OR Q31A = 2
                                                OR Q31B = 1 OR Q31B = 2
                                                OR Q31C = 1 OR Q31C = 2
                                                OR Q31D = 1 OR Q31D = 2
                                                OR Q31E = 1 OR Q31E = 2) ");
                        }
                        else if (control.Equals("2"))
                        {
                            sb.Append(@" OR ( (Q29 = 0
                                                OR Q30 = 1 OR Q30 = 2
                                                OR Q31A = 1 OR Q31A = 2
                                                OR Q31B = 1 OR Q31B = 2
                                                OR Q31C = 1 OR Q31C = 2
                                                OR Q31D = 1 OR Q31D = 2
                                                OR Q31E = 1 OR Q31E = 2) AND ResponseComplete = 0 ) ");
                        }
                        else if (control.Equals("3"))
                        {
                            sb.Append(@" OR ( (Q29 = 0
                                                OR Q30 = 1 OR Q30 = 2
                                                OR Q31A = 1 OR Q31A = 2
                                                OR Q31B = 1 OR Q31B = 2
                                                OR Q31C = 1 OR Q31C = 2
                                                OR Q31D = 1 OR Q31D = 2
                                                OR Q31E = 1 OR Q31E = 2) AND ResponseComplete = 1 ) ");
                        }
                    }
                    if (sb.Length > 4)
                    {
                        sb.Remove(0, 4);
                    }
                }
            }
        }

        private void fltFBVenue_AddToQuery(ReportFilterListBox control, SQLParamList sqlParams)
        {
            if (control.IsActive)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ListItem li in control.Items)
                {
                    if (li.Selected)
                    {
                        sb.AppendFormat(" OR [Q18_{0}] = 1 ", li.Value);
                    }
                }
                if (sb.Length > 4)
                {
                    sb.Remove(0, 4);
                }
            }
        }

        private void fltLanguage_AddToQuery(ReportFilterListBox control, SQLParamList sqlParams)
        {
            if (control.IsActive)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ListItem li in control.Items)
                {
                    if (li.Selected)
                    {
                        sb.AppendFormat(" OR [Q39_{0}] = 1 ", li.Value);
                    }
                }
                if (sb.Length > 4)
                {
                    sb.Remove(0, 4);
                }
            }
        }

        #endregion Filter Query Override Methods
    }
}