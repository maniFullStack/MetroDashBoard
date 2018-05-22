using SharedClasses;
using System;
using System.Globalization;
using WebsiteUtilities;

namespace GCC_Web_Portal.Controls
{
    public partial class DateRangeFilterControl : System.Web.UI.UserControl, IReportFilter
    {
        private const string DATE_FORMAT = "dd'/'MM'/'yyyy";
        public string Label { get; set; }
        public string SessionKey { get; set; }
        public string DBColumn { get; set; }

        public bool IsActive
        {
            get
            {
                return BeginDate.HasValue && EndDate.HasValue;
            }
        }

        public MessageManager MessageManager { get; private set; }
        public DateTime? DefaultBeginDate { get; set; }
        public DateTime? DefaultEndDate { get; set; }

        public UserInfo User { get; set; }

        public DateTime? BeginDate
        {
            get
            {
                DateTime date;
                if (DateTime.TryParseExact(hdnBegin.Value, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                    // For Archived Data setting from date to 2016 
                    //date = new DateTime(2016, 01, 01, 00, 00, 01); //20180312 - Since the End date was having 00:00:00 HH:mm:ss end of day was not selected so manually forcing to select till End of day
                    //return date;
                }
                else
                {
                    return DefaultBeginDate;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    hdnBegin.Value = value.Value.ToString(DATE_FORMAT, null);
                }
                else
                {
                    if (DefaultBeginDate.HasValue)
                    {
                        hdnBegin.Value = DefaultBeginDate.Value.ToString(DATE_FORMAT, null);
                    }
                    else
                    {
                        hdnBegin.Value = String.Empty;
                    }
                }
            }
        }

        public DateTime? EndDate
        {
            get
            {
                DateTime date;

              //  DateTime date; =  new DateTime(date.Year,date.Month,date.Day,0,0,0);

                if (DateTime.TryParseExact(hdnEnd.Value, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    //return date.AddDays(1).AddMilliseconds(-1); //Return end of day

                    date = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59); //20180312 - Since the End date was having 00:00:00 HH:mm:ss end of day was not selected so manually forcing to select till End of day



                    // For Archived Data setting from date to 2016 
                   // date = new DateTime(2016, date.Month, date.Day, 23, 59, 59); //20180312 - Since the End date was having 00:00:00 HH:mm:ss end of day was not selected so manually forcing to select till End of day
                    return date;
                }
                else
                {
                    return DefaultEndDate;
                }
            }
            set
            {
                if (value.HasValue)
                {
                    hdnEnd.Value = value.Value.ToString(DATE_FORMAT);
                }
                else
                {
                    if (DefaultEndDate.HasValue)
                    {
                        hdnEnd.Value = DefaultEndDate.Value.ToString(DATE_FORMAT, null);
                    }
                    else
                    {
                        hdnEnd.Value = String.Empty;
                    }
                }
            }
        }

        public DateRangeFilterControl()
        {
            MessageManager = new MessageManager();
        }

        public void SetValues(string beginDate, string endDate)
        {
            DateTime bDate;
            if (DateTime.TryParseExact(beginDate, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out bDate))
            {
                BeginDate = bDate;
            }
            DateTime eDate;
            if (DateTime.TryParseExact(endDate, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out eDate))
            {
                EndDate = eDate;
            }
        }

        public string GetSelectedFilterText()
        {
            if (!BeginDate.HasValue || !EndDate.HasValue)
            {
                return String.Empty;
            }
            else
            {
                return BeginDate.Value.ToString(DATE_FORMAT) + " - " + EndDate.Value.ToString(DATE_FORMAT);
            }
        }

        public void Save()
        {
            SessionWrapper.Add(SessionKey + "_BeginDate", BeginDate);
            SessionWrapper.Add(SessionKey + "_EndDate", EndDate);
        }

        public void Clear()
        {
            SessionWrapper.Remove(SessionKey + "_BeginDate");
            SessionWrapper.Remove(SessionKey + "_EndDate");
            hdnBegin.Value = String.Empty;
            hdnEnd.Value = String.Empty;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!String.IsNullOrEmpty(SessionKey + "_BeginDate") && String.IsNullOrEmpty(hdnBegin.Value))
            {
                BeginDate = SessionWrapper.Get<DateTime?>(SessionKey + "_BeginDate", null);
            }
            if (!String.IsNullOrEmpty(SessionKey + "_EndDate") && String.IsNullOrEmpty(hdnEnd.Value))
            {
                EndDate = SessionWrapper.Get<DateTime?>(SessionKey + "_EndDate", null);
            }
        }

        public void AddToQuery(SQLParamList sqlParams)
        {
            if (IsActive)
            {
                //Adjust dates from user's timezone
                if (BeginDate.HasValue)
                {
                    sqlParams.Add("@" + DBColumn + "_Begin", BeginDate.Value);
                }
                if (EndDate.HasValue)
                {
                    sqlParams.Add("@" + DBColumn + "_End", EndDate.Value);
                }
            }
        }
    }
}