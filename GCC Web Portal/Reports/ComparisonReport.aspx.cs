using OfficeOpenXml;
using SharedClasses;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using WebsiteUtilities;
using System.Linq;

namespace GCC_Web_Portal.Reports
{
    public partial class ComparisonReport : AuthenticatedPage
    {
        //This is used to determine the sort order of the properties in the output file. It should be alphabetical.
        private int[] _propertySortOrder = { 1, 7, 19, 11, 12, 8, 10, 9, 14, 13, 3, 5, 2, 20, 18, 6, 22,23,24 };

        protected enum CellFormat
        {
            Index,
            Percent,
            Number
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.HideAllFilters = true;
            drDateRangeFirst.User = User;
            drDateRangeSecond.User = User;
            Title = "Comparison Report";
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();
            sql.CommandTimeout = 120;
            SQLParamList sqlParams = new SQLParamList()
                                            .Add("@DR1_Begin", drDateRangeFirst.BeginDate)
                                            .Add("@DR1_End", drDateRangeFirst.EndDate)
                                            .Add("@DR2_Begin", drDateRangeSecond.BeginDate)
                                            .Add("@DR2_End", drDateRangeSecond.EndDate);

            DataTable dt = sql.ExecStoredProcedureDataTable("spReports_Comparison", sqlParams);
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Oops. Something went wrong when exporting the data. Please try again. (ECP100)";
            }
            else
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    p.Workbook.Worksheets.Add("Comparison");
                    ExcelWorksheet worksheet = p.Workbook.Worksheets[1];

                    worksheet.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                    worksheet.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                    worksheet.Column(1).Width = 12.71f;
                    worksheet.Column(2).Width = 62f;

                    for (int i = 0; i < _propertySortOrder.Length * 3; i++)
                    {
                        worksheet.Column(3 + i).Width = 12.71f;
                    }

                    string group1 = String.Format("{0} - {1}", drDateRangeFirst.BeginDate.Value.ToString("yyyy-MM-dd"), drDateRangeFirst.EndDate.Value.ToString("yyyy-MM-dd"));
                    string group2 = String.Format("{0} - {1}", drDateRangeSecond.BeginDate.Value.ToString("yyyy-MM-dd"), drDateRangeSecond.EndDate.Value.ToString("yyyy-MM-dd"));

                    worksheet.Cells[1, 1].Value = String.Format("First Date Range: {0}", group1);
                    worksheet.Cells[2, 1].Value = String.Format("Second Date Range: {0}", group2);

                    int rowNum = 4;

                    Action<ExcelRange> titleMerge = r =>
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Font.Bold = true;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    };

                    Action<ExcelRange> style = r =>
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    };
                    int offset = 0;
                    for (int i = 0; i < _propertySortOrder.Length; i++)
                    {
                        int prop = _propertySortOrder[i];
                        if (Master.IsPropertyUser
                            && prop != 1
                            && prop != ((int)User.PropertyShortCode))
                        {
                            offset -= 3;
                            continue;
                        }

                        AddValue(worksheet, rowNum, 3 + offset + i * 3, rowNum, 5 + offset + i * 3, PropertyTools.GetCasinoName(prop), titleMerge);
                        AddValue(worksheet, rowNum + 1, 3 + offset + i * 3, "First Range", r =>
                        {
                            style(r);
                            r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        });
                        AddValue(worksheet, rowNum + 1, 4 + offset + i * 3, "Second Range", style);
                        AddValue(worksheet, rowNum + 1, 5 + offset + i * 3, "Diff.", r =>
                        {
                            style(r);
                            r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        });
                    }

                    rowNum += 2;

                    worksheet.View.FreezePanes(rowNum, 3);

                    AddDataRow(worksheet, dt, rowNum++, true, "SAMPLE SIZE", "SampleCount", CellFormat.Number);

                    AddDataRow(worksheet, dt, rowNum++, true, "GUEST EXPERIENCE INDEX", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "GEI", "GEI", CellFormat.Index);

                    AddDataRow(worksheet, dt, rowNum++, true, "GUEST SERVICE EXPERIENCE INDEX", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "GSEI", "GSEI");

                    AddDataRow(worksheet, dt, rowNum++, true, "NET PROMOTER SCORE", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "NPS", "NPS");

                    AddDataRow(worksheet, dt, rowNum++, true, "PROBLEM RESOLUTION SCORE", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "PRS", "PRS");

                    AddDataRow(worksheet, dt, rowNum++, true, "GUEST LOYALTY", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Likely to recommend the casino", "Q6A");
                    AddDataRow(worksheet, dt, rowNum++, false, "Likely to mostly visit this casino", "Q6B");
                    AddDataRow(worksheet, dt, rowNum++, false, "Likely to visit this casino for next gaming entertainment opportunity", "Q6C");
                    AddDataRow(worksheet, dt, rowNum++, false, "Likely to provide personal preferences so casino can serve me better", "Q6D");

                    AddDataRow(worksheet, dt, rowNum++, true, "CASINO STAFF", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Ensuring all of your needs were met", "Q7A");
                    AddDataRow(worksheet, dt, rowNum++, false, "Making you feel welcome", "Q7B");
                    AddDataRow(worksheet, dt, rowNum++, false, "Going above & beyond normal service", "Q7C");
                    AddDataRow(worksheet, dt, rowNum++, false, "Speed of service", "Q7D");
                    AddDataRow(worksheet, dt, rowNum++, false, "Encouraging you to visit again", "Q7E");
                    AddDataRow(worksheet, dt, rowNum++, false, "Overall staff availability", "Q7F");

                    AddDataRow(worksheet, dt, rowNum++, false, "Overall staff", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Cashiers", "Q9A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Guest Services", "Q9B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Slot Attendants", "Q9C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Dealers", "Q9D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Restaurant Servers", "Q9E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Cocktail Servers", "Q9F", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Coffee Servers", "Q9G", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Security", "Q9H", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Managers/Supervisors", "Q9I", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Hotel Staff", "Q9J", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                    AddDataRow(worksheet, dt, rowNum++, false, "Attribute ratings:", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Encouraging you to take part in events or promotions", "Q10A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Answering questions you had about the property or promotions", "Q10B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Being friendly and welcoming", "Q10C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                    AddDataRow(worksheet, dt, rowNum++, true, "CASINO FACILITIES", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Overall facilities", "Q12");
                    AddDataRow(worksheet, dt, rowNum++, false, "Attribute ratings:", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Ambiance, mood, atmosphere of the environment", "Q13A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Cleanliness of general areas", "Q13B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Clear signage", "Q13C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Washroom cleanliness", "Q13D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Adequate  lighting - it is bright enough", "Q13E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Safe environment", "Q13F", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Parking availability", "Q13G", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                    AddDataRow(worksheet, dt, rowNum++, true, "GAMING EXPERIENCE", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Primary gaming:", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Playing Slots", "Count_Slots", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Playing Tables", "Count_Tables", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Playing Poker", "Count_Poker", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Enjoying Food or Beverages", "Count_Food", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Watching Live Entertainment at a show lounge or theatre", "Count_Entertainment", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Staying at our Hotel", "Count_Hotel", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Watching Live Racing", "Count_LiveRacing", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Watching Racing at our Racebook", "Count_Racebook", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Playing Bingo", "Count_Bingo", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Lottery / Pull Tabs", "Count_Lottery", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "None", "Count_None", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Primary gaming:", "Q14");

                    AddDataRow(worksheet, dt, rowNum++, false, "Attribute ratings:", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Variety of games available", "Q15A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Waiting time to play", "Q15B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Availability of specific game at your desired denomination", "Q15C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Contests & monthly promotions", "Q15D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Courtesy & respectfulness of staff", "Q15E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Game Knowledge of Staff", "Q15F", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Rate of earning", "Q16A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Redemption value", "Q16B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Choice of rewards", "Q16C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Slot Free Play", "Q16D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                    AddDataRow(worksheet, dt, rowNum++, true, "FOOD & BEVERAGE", null);
                    rowNum = AddYesNoRow(worksheet, dt, rowNum++, false, "Purchase food or beverages?", "Q17");
                    AddDataRow(worksheet, dt, rowNum++, false, "Overall dining experience", "Q19");

                    AddDataRow(worksheet, dt, rowNum++, false, "Attribute ratings:", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Variety of food choices", "Q20A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Cleanliness of outlet", "Q20B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Courtesy of staff", "Q20C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Timely delivery of order", "Q20D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Value for the money", "Q20E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Pleasant atmosphere", "Q20F", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Quality of food", "Q20G", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                    AddDataRow(worksheet, dt, rowNum++, true, "LOUNGE ENTERTAINMENT", null);
                    rowNum = AddYesNoRow(worksheet, dt, rowNum++, false, "Attend Lounge entertainment?", "Q21");
                    AddDataRow(worksheet, dt, rowNum++, false, "Overall entertainment experience", "Q22");

                    AddDataRow(worksheet, dt, rowNum++, false, "Attribute ratings:", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Sound / quality", "Q23A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Seating availability", "Q23B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Dance floor", "Q23C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Fun and enjoyable atmosphere", "Q23D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                    AddDataRow(worksheet, dt, rowNum++, true, "THEATRE", null);
                    rowNum = AddYesNoRow(worksheet, dt, rowNum++, false, "Attend Theatre?", "Q24");
                    AddDataRow(worksheet, dt, rowNum++, false, "Overall Theatre experience", "Q25");

                    AddDataRow(worksheet, dt, rowNum++, false, "Attribute ratings:", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "The quality of the show", "Q26A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "The value of the show", "Q26B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Seating choices", "Q26C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Sound quality", "Q26D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                    AddDataRow(worksheet, dt, rowNum++, true, "SERVICE RECOVERY", null);
                    rowNum = AddYesNoRow(worksheet, dt, rowNum++, false, "Experience problem?", "Q27");
                    AddDataRow(worksheet, dt, rowNum++, false, "Where experienced problem?", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Arrival and parking", "Q27A_ArrivalAndParking", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Guest Services", "Q27A_GuestServices", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Cashiers", "Q27A_Cashiers", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Manager/Supervisor", "Q27A_ManagerSupervisor", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Security", "Q27A_Security", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Slots", "Q27A_Slots", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Tables", "Q27A_Tables", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Food & Beverage", "Q27A_FoodAndBeverage", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Hotel", "Q27A_Hotel", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Other", "Q27A_Other", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                    rowNum = AddYesNoRow(worksheet, dt, rowNum++, false, "Resolve problem?", "Q28");
                    rowNum = AddYesNoRow(worksheet, dt, rowNum++, false, "Report problem?", "Q29");
                    AddDataRow(worksheet, dt, rowNum++, false, "Overall ability to fix problem", "Q30");
                    AddDataRow(worksheet, dt, rowNum++, false, "Attribute ratings:", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "The length of time taken to resolve your problem", "Q31A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "The effort of employees in resolving your problem", "Q31B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "The courteousness of employees while resolving your problem", "Q31C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    worksheet.Row(rowNum).Height *= 2;
                    AddDataRow(worksheet, dt, rowNum++, false, "The amount of communication with you from employees while resolving your problem", "Q31D", CellFormat.Percent, r => { r.Style.WrapText = true; r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "The fairness of the outcome in resolving your problem", "Q31E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                    AddDataRow(worksheet, dt, rowNum++, true, "DEMOGRAPHICS", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Male", "Q36_Male");
                    AddDataRow(worksheet, dt, rowNum++, false, "Female", "Q36_Female");
                    AddDataRow(worksheet, dt, rowNum++, false, String.Empty, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "19-24", "Q37_19-24");
                    AddDataRow(worksheet, dt, rowNum++, false, "25-34", "Q37_25-34");
                    AddDataRow(worksheet, dt, rowNum++, false, "35-44", "Q37_35-44");
                    AddDataRow(worksheet, dt, rowNum++, false, "45-54", "Q37_45-54");
                    AddDataRow(worksheet, dt, rowNum++, false, "55-64", "Q37_55-64");
                    AddDataRow(worksheet, dt, rowNum++, false, "65 or older", "Q37_65 or older");
                    AddDataRow(worksheet, dt, rowNum++, false, String.Empty, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "First visit", "Q38_This was my first visit");
                    AddDataRow(worksheet, dt, rowNum++, false, "2-7 times per week", "Q38_2-7 times per week");
                    AddDataRow(worksheet, dt, rowNum++, false, "Once per week", "Q38_Once per week");
                    AddDataRow(worksheet, dt, rowNum++, false, "2-3 times per month", "Q38_2-3 times per month");
                    AddDataRow(worksheet, dt, rowNum++, false, "Once per month", "Q38_Once per month");
                    AddDataRow(worksheet, dt, rowNum++, false, "Several times a year", "Q38_Several times a year");

                    AddDataRow(worksheet, dt, rowNum++, false, "Languages spoken at home (other than English)", null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Korean", "Q39_1", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Punjabi", "Q39_2", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Chinese Mandarin", "Q39_3", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Other Western European languages", "Q39_4", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Eastern European languages", "Q39_5", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Spanish", "Q39_6", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "French", "Q39_7", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Hindi", "Q39_8", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Tagalog", "Q39_9", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Vietnamese", "Q39_10", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Pakistani", "Q39_11", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Farsi", "Q39_12", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Japanese", "Q39_13", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Arabic / Middle Eastern", "Q39_14", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Chinese – Cantonese", "Q39_15", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, "Other", "Q39_16", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                    AddDataRow(worksheet, dt, rowNum++, false, String.Empty, null);
                    rowNum = AddYesNoRow(worksheet, dt, rowNum++, false, "Players Club?", "Q4");

                    rowNum++; //Skip one row

                    string lFileName = string.Format("DateRangeComparison-{0}.xlsx", ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss-fff", User));
                    const string lPath = "~/Files/Cache/";

                    string lOutput = string.Concat(MapPath(lPath), lFileName);

                    FileInfo fi = new FileInfo(lOutput);
                    p.SaveAs(fi);

                    hlDownload.Text = "Download File - " + lFileName;
                    hlDownload.NavigateUrl = String.Format("{0}{1}", lPath, lFileName);
                }
            }
        }

        private void AddValue(ExcelWorksheet worksheet, int rowNum, int colNum, object value, Action<ExcelRange> action)
        {
            using (ExcelRange r = worksheet.Cells[rowNum, colNum])
            {
                r.Value = value;
                if (action != null)
                {
                    action(r);
                }
            }
        }

        private void AddValue(ExcelWorksheet worksheet, int rowNumStart, int colNumStart, int rowNumEnd, int colNumEnd, object value, Action<ExcelRange> action)
        {
            using (ExcelRange r = worksheet.Cells[rowNumStart, colNumStart, rowNumEnd, colNumEnd])
            {
                r.Value = value;
                r.Merge = true;
                if (action != null)
                {
                    action(r);
                }
            }
        }

        private int AddYesNoRow(ExcelWorksheet worksheet, DataTable dt, int rowNum, bool isTitleCell, string mainLabel, string dataColumn)
        {
            AddDataRow(worksheet, dt, rowNum++, false, mainLabel, null);
            AddDataRow(worksheet, dt, rowNum++, false, "Yes", dataColumn + "_Yes", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
            AddDataRow(worksheet, dt, rowNum++, false, "No", dataColumn + "_No", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
            return rowNum;
        }

        private void AddDataRow(ExcelWorksheet worksheet, DataTable dt, int rowNum, bool isTitleCell, string label, string dataColumn)
        {
            AddDataRow(worksheet, dt, rowNum, isTitleCell, label, dataColumn, CellFormat.Percent);
        }

        private void AddDataRow(ExcelWorksheet worksheet, DataTable dt, int rowNum, bool isTitleCell, string label, string dataColumn, CellFormat cell)
        {
            AddDataRow(worksheet, dt, rowNum, isTitleCell, label, dataColumn, cell, null, null);
        }

        private void AddDataRow(ExcelWorksheet worksheet, DataTable dt, int rowNum, bool isTitleCell, string label, string dataColumn, CellFormat cell, Action<ExcelRange> titleAction, Action<ExcelRange> dataAction)
        {
            //Set the titles
            if (isTitleCell)
            {
                //Big title
                worksheet.Cells[rowNum, 1].Value = label;
                using (ExcelRange r = worksheet.Cells[rowNum, 1, rowNum, 2])
                {
                    r.Merge = true;
                    r.Style.Font.Bold = true;
                    if (titleAction != null)
                    {
                        titleAction(r);
                    }
                }
            }
            else
            {
                //Sub-title
                worksheet.Cells[rowNum, 2].Value = label;
                if (titleAction != null)
                {
                    titleAction(worksheet.Cells[rowNum, 2]);
                }
            }
            Action<ExcelRange> baseStyle = r =>
            {
                r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            };
            int offset = 0;
            for (int i = 0; i < _propertySortOrder.Length; i++)
            {
                int propIndex = _propertySortOrder[i] - 1;

                if (Master.IsPropertyUser
                    && propIndex != 0
                    && propIndex != ((int)User.PropertyShortCode) - 1)
                {
                    offset -= 3;
                    continue;
                }

				int rowOffset = -1;
				for ( int inner = 0; inner < dt.Rows.Count; inner++ ) {
					if ( dt.Rows[inner]["PropertyID"].Equals( _propertySortOrder[i] ) ) {
						rowOffset = inner;
						break;
					}
				}
				if ( rowOffset < 0 ) {
					continue;
				}

                double val1 = -1000;
                double val2 = -1000;
                if (dataColumn != null)
                {
                    val1 = dt.Rows[rowOffset][dataColumn].ToString().StringToDbl(-1000);
                    val2 = dt.Rows[rowOffset + 1][dataColumn].ToString().StringToDbl(-1000);
                }
                if (val1 == -1000)
                {
                    AddValue(worksheet, rowNum, 3 + offset + i * 3, null, r =>
                    {
                        baseStyle(r);
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    });
                }
                else
                {
                    AddValue(worksheet, rowNum, 3 + offset + i * 3, val1, r =>
                    {
                        baseStyle(r);
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        switch (cell)
                        {
                            case CellFormat.Index:
                                r.Style.Numberformat.Format = "0.0";
                                break;

                            case CellFormat.Number:
                                r.Style.Numberformat.Format = "#,###;[Red](#,##0);0";
                                break;

                            case CellFormat.Percent:
                                r.Style.Numberformat.Format = "0.0%";
                                break;
                        }
                    });
                }

                if (val2 == -1000)
                {
                    AddValue(worksheet, rowNum, 4 + offset + i * 3, null, baseStyle);
                }
                else
                {
                    AddValue(worksheet, rowNum, 4 + offset + i * 3, val2, r =>
                    {
                        baseStyle(r);
                        switch (cell)
                        {
                            case CellFormat.Index:
                                r.Style.Numberformat.Format = "0.0";
                                break;

                            case CellFormat.Number:
                                r.Style.Numberformat.Format = "#,###;[Red](#,##0);0";
                                break;

                            case CellFormat.Percent:
                                r.Style.Numberformat.Format = "0.0%";
                                break;
                        }
                    });
                }

                if (val2 == -1000 || val1 == -1000)
                {
                    AddValue(worksheet, rowNum, 5 + offset + i * 3, null, r =>
                    {
                        baseStyle(r);
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    });
                }
                else
                {
                    double diff = val2 - val1;
                    AddValue(worksheet, rowNum, 5 + offset + i * 3, diff, r =>
                    {
                        baseStyle(r);
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Font.Bold = true;
                        switch (cell)
                        {
                            case CellFormat.Index:
                                r.Style.Numberformat.Format = "0.0";
                                break;

                            case CellFormat.Number:
                                r.Style.Numberformat.Format = "#,###;[Red](#,##0);0";
                                break;

                            case CellFormat.Percent:
                                r.Style.Numberformat.Format = "0.0%";
                                if (diff >= 0.1)
                                {
                                    r.Style.Font.Color.SetColor(Color.FromArgb(0, 97, 0));
                                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(198, 239, 206));
                                }
                                else if (diff <= -0.1)
                                {
                                    r.Style.Font.Color.SetColor(Color.FromArgb(190, 0, 6));
                                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 199, 206));
                                }
                                break;
                        }
                    });
                }
            }
        }

        private void GenerateHeadings(ExcelWorksheet worksheet, DataTable dt)
        {
        }
    }
}