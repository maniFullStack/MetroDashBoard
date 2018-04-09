using OfficeOpenXml;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class QuarterlyReport : AuthenticatedPage
    {
        protected DataTable Data = null;
        private int _startYear = -1;
        private int _colsPerProperty = -1;
        private int _yearsInclusive = -1;
        private const int START_COL = 3;

        //This is used to determine the sort order of the properties in the output file. It should be alphabetical.
        //int[] _propertySortOrder = { 1, 7, 11, 12, 8, 10, 9, 4, 13, 3, 5, 2, 6 };
        private int[] _propertySortOrder = { 1, 7, 19, 11, 12, 8, 10, 9, 14, 13, 3, 5, 2, 17, 20 , 18, 6, 22,23,24,25,26,27,28 };

        private Dictionary<int, Dictionary<int, double>> _varianceLookup = new Dictionary<int, Dictionary<int, double>>();

        protected enum CellFormat
        {
            Index,
            Percent,
            Number
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.HideAllFilters = true;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            sql.CommandTimeout = 90;
            DataTable dt = sql.ExecStoredProcedureDataTable("spReports_Quarterly");
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Unable to query report data from the database.";
                return;
            }

            using (ExcelPackage p = new ExcelPackage())
            {
                p.Workbook.Worksheets.Add("Annual Results");
                ExcelWorksheet worksheet = p.Workbook.Worksheets[1];
                worksheet.Cells.Style.Font.Size = 10; //Default font size for whole sheet
                worksheet.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                worksheet.Cells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);

                worksheet.Column(1).Width = 12.71f;
                worksheet.Column(2).Width = 54.85f;

                //Sheet title
                worksheet.Cells["A1"].Value = "GCGC Gaming Report";
                using (ExcelRange r = worksheet.Cells["A1:B7"])
                {
                    r.Merge = true;
                    r.Style.Font.Size = 14;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                }

                DataTable dtTransposed = dt.TransposeDataTable();
                Dictionary<string, DataRow> lookup = new Dictionary<string, DataRow>();
                foreach (DataRow dr in dtTransposed.Rows)
                {
                    lookup.Add(dr["Quarter"].ToString(), dr);
                }

                int rowNum = 8;

                worksheet.View.FreezePanes(rowNum, 3);

                AddDataRow(worksheet, lookup, rowNum++, true, "SAMPLE SIZE", "SampleCount", CellFormat.Number);

                AddDataRow(worksheet, lookup, rowNum++, true, "GUEST EXPERIENCE INDEX", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "GEI", "GEI", CellFormat.Index);

                AddDataRow(worksheet, lookup, rowNum++, true, "GUEST SERVICE EXPERIENCE INDEX", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "GSEI", "GSEI");

                AddDataRow(worksheet, lookup, rowNum++, true, "NET PROMOTER SCORE", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "NPS", "NPS");

                AddDataRow(worksheet, lookup, rowNum++, true, "PROBLEM RESOLUTION SCORE", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "PRS", "PRS");

                AddDataRow(worksheet, lookup, rowNum++, true, "GUEST LOYALTY", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Likely to recommend the casino", "Q6A");
                AddDataRow(worksheet, lookup, rowNum++, false, "Likely to mostly visit this casino", "Q6B");
                AddDataRow(worksheet, lookup, rowNum++, false, "Likely to visit this casino for next gaming entertainment opportunity", "Q6C");
                AddDataRow(worksheet, lookup, rowNum++, false, "Likely to provide personal preferences so casino can serve me better", "Q6D");

                AddDataRow(worksheet, lookup, rowNum++, true, "CASINO STAFF", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Ensuring all of your needs were met", "Q7A");
                AddDataRow(worksheet, lookup, rowNum++, false, "Making you feel welcome", "Q7B");
                AddDataRow(worksheet, lookup, rowNum++, false, "Going above & beyond normal service", "Q7C");
                AddDataRow(worksheet, lookup, rowNum++, false, "Speed of service", "Q7D");
                AddDataRow(worksheet, lookup, rowNum++, false, "Encouraging you to visit again", "Q7E");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall staff availability", "Q7F");

                AddDataRow(worksheet, lookup, rowNum++, false, "Overall staff", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Cashiers", "Q9A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Guest Services", "Q9B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Slot Attendants", "Q9C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Dealers", "Q9D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Restaurant Servers", "Q9E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Cocktail Servers", "Q9F", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Coffee Servers", "Q9G", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Security", "Q9H", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Managers/Supervisors", "Q9I", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Hotel Staff", "Q9J", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                AddDataRow(worksheet, lookup, rowNum++, false, "Attribute ratings:", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Encouraging you to take part in events or promotions", "Q10A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Answering questions you had about the property or promotions", "Q10B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Being friendly and welcoming", "Q10C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                AddDataRow(worksheet, lookup, rowNum++, true, "CASINO FACILITIES", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall facilities", "Q12");
                AddDataRow(worksheet, lookup, rowNum++, false, "Attribute ratings:", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Ambiance, mood, atmosphere of the environment", "Q13A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Cleanliness of general areas", "Q13B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Clear signage", "Q13C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Washroom cleanliness", "Q13D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Adequate  lighting - it is bright enough", "Q13E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Safe environment", "Q13F", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Parking availability", "Q13G", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                AddDataRow(worksheet, lookup, rowNum++, true, "GAMING EXPERIENCE", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Primary gaming:", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Playing Slots", "Count_Slots", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Playing Tables", "Count_Tables", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Playing Poker", "Count_Poker", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Enjoying Food or Beverages", "Count_Food", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Watching Live Entertainment at a show lounge or theatre", "Count_Entertainment", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Staying at our Hotel", "Count_Hotel", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Watching Live Racing", "Count_LiveRacing", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Watching Racing at our Racebook", "Count_Racebook", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Playing Bingo", "Count_Bingo", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Lottery / Pull Tabs", "Count_Lottery", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "None", "Count_None", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Primary gaming:", "Q14");

                AddDataRow(worksheet, lookup, rowNum++, false, "Attribute ratings:", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Variety of games available", "Q15A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Waiting time to play", "Q15B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Availability of specific game at your desired denomination", "Q15C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Contests & monthly promotions", "Q15D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Courtesy & respectfulness of staff", "Q15E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Game Knowledge of Staff", "Q15F", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Rate of earning", "Q16A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Redemption value", "Q16B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Choice of rewards", "Q16C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Slot Free Play", "Q16D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                AddDataRow(worksheet, lookup, rowNum++, true, "FOOD & BEVERAGE", null);
                rowNum = AddYesNoRow(worksheet, lookup, rowNum++, false, "Purchase food or beverages?", "Q17");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall dining experience", "Q19");

                AddDataRow(worksheet, lookup, rowNum++, false, "Attribute ratings:", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Variety of food choices", "Q20A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Cleanliness of outlet", "Q20B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Courtesy of staff", "Q20C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Timely delivery of order", "Q20D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Value for the money", "Q20E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Pleasant atmosphere", "Q20F", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Quality of food", "Q20G", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                AddDataRow(worksheet, lookup, rowNum++, true, "LOUNGE ENTERTAINMENT", null);
                rowNum = AddYesNoRow(worksheet, lookup, rowNum++, false, "Attend Lounge entertainment?", "Q21");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall entertainment experience", "Q22");

                AddDataRow(worksheet, lookup, rowNum++, false, "Attribute ratings:", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Sound / quality", "Q23A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Seating availability", "Q23B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Dance floor", "Q23C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Fun and enjoyable atmosphere", "Q23D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                AddDataRow(worksheet, lookup, rowNum++, true, "THEATRE", null);
                rowNum = AddYesNoRow(worksheet, lookup, rowNum++, false, "Attend Theatre?", "Q24");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Theatre experience", "Q25");

                AddDataRow(worksheet, lookup, rowNum++, false, "Attribute ratings:", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "The quality of the show", "Q26A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "The value of the show", "Q26B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Seating choices", "Q26C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Sound quality", "Q26D", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                //AddDataRow( worksheet, lookup, rowNum++, false, "Overall customer service of Theatre staff", "Q26E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null );

                AddDataRow(worksheet, lookup, rowNum++, true, "SERVICE RECOVERY", null);
                rowNum = AddYesNoRow(worksheet, lookup, rowNum++, false, "Experience problem?", "Q27");
                AddDataRow(worksheet, lookup, rowNum++, false, "Where experienced problem?", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Arrival and parking", "Q27A_ArrivalAndParking", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Guest Services", "Q27A_GuestServices", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Cashiers", "Q27A_Cashiers", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Manager/Supervisor", "Q27A_ManagerSupervisor", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Security", "Q27A_Security", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Slots", "Q27A_Slots", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Tables", "Q27A_Tables", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Food & Beverage", "Q27A_FoodAndBeverage", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Hotel", "Q27A_Hotel", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Other", "Q27A_Other", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                rowNum = AddYesNoRow(worksheet, lookup, rowNum++, false, "Resolve problem?", "Q28");
                rowNum = AddYesNoRow(worksheet, lookup, rowNum++, false, "Report problem?", "Q29");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall ability to fix problem", "Q30");
                AddDataRow(worksheet, lookup, rowNum++, false, "Attribute ratings:", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "The length of time taken to resolve your problem", "Q31A", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "The effort of employees in resolving your problem", "Q31B", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "The courteousness of employees while resolving your problem", "Q31C", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                worksheet.Row(rowNum).Height *= 2;
                AddDataRow(worksheet, lookup, rowNum++, false, "The amount of communication with you from employees while resolving your problem", "Q31D", CellFormat.Percent, r => { r.Style.WrapText = true; r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; r.Style.Indent = 3; }, r => { r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; });
                AddDataRow(worksheet, lookup, rowNum++, false, "The fairness of the outcome in resolving your problem", "Q31E", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);

                AddDataRow(worksheet, lookup, rowNum++, true, "DEMOGRAPHICS", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Male", "Q36_Male");
                AddDataRow(worksheet, lookup, rowNum++, false, "Female", "Q36_Female");
                AddDataRow(worksheet, lookup, rowNum++, false, String.Empty, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "19-24", "Q37_19-24");
                AddDataRow(worksheet, lookup, rowNum++, false, "25-34", "Q37_25-34");
                AddDataRow(worksheet, lookup, rowNum++, false, "35-44", "Q37_35-44");
                AddDataRow(worksheet, lookup, rowNum++, false, "45-54", "Q37_45-54");
                AddDataRow(worksheet, lookup, rowNum++, false, "55-64", "Q37_55-64");
                AddDataRow(worksheet, lookup, rowNum++, false, "65 or older", "Q37_65 or older");
                AddDataRow(worksheet, lookup, rowNum++, false, String.Empty, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "First visit", "Q38_This was my first visit");
                AddDataRow(worksheet, lookup, rowNum++, false, "2-7 times per week", "Q38_2-7 times per week");
                AddDataRow(worksheet, lookup, rowNum++, false, "Once per week", "Q38_Once per week");
                AddDataRow(worksheet, lookup, rowNum++, false, "2-3 times per month", "Q38_2-3 times per month");
                AddDataRow(worksheet, lookup, rowNum++, false, "Once per month", "Q38_Once per month");
                AddDataRow(worksheet, lookup, rowNum++, false, "Several times a year", "Q38_Several times a year");

                AddDataRow(worksheet, lookup, rowNum++, false, "Languages spoken at home (other than English)", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Korean", "Q39_1", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Punjabi", "Q39_2", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Chinese Mandarin", "Q39_3", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Other Western European languages", "Q39_4", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Eastern European languages", "Q39_5", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Spanish", "Q39_6", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "French", "Q39_7", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Hindi", "Q39_8", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Tagalog", "Q39_9", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Vietnamese", "Q39_10", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Pakistani", "Q39_11", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Farsi", "Q39_12", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Japanese", "Q39_13", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Arabic / Middle Eastern", "Q39_14", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Chinese – Cantonese", "Q39_15", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Other", "Q39_16", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
                AddDataRow(worksheet, lookup, rowNum++, false, String.Empty, null);
                rowNum = AddYesNoRow(worksheet, lookup, rowNum++, false, "Players Club?", "Q4");

                rowNum++; //Skip one row

                //Handle variance rating
                foreach (int varCol in _varianceLookup.Keys)
                {
                    double rows = _varianceLookup[varCol].Count;
                    double i = 1;
                    //Order each variance value by the amt in descending order
                    foreach (KeyValuePair<int, double> v in _varianceLookup[varCol].OrderByDescending(x => x.Value))
                    {
                        //varCol = column number
                        //v.Key = row number
                        //v.Value = variance value
                        if (i / rows <= 0.25)
                        {
                            //Pick the top 25 percent for "High"
                            using (ExcelRange r = worksheet.Cells[v.Key, varCol])
                            {
                                r.Value = "High";
                                r.Style.Font.Color.SetColor(Color.FromArgb(190, 0, 6));
                                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 199, 206));
                            }
                        }
                        else if (i / rows > 0.25 && i / rows <= 0.55)
                        {
                            //Pick the mid 30 percent for "Medium"
                            using (ExcelRange r = worksheet.Cells[v.Key, varCol])
                            {
                                r.Value = "Medium";
                                r.Style.Font.Color.SetColor(Color.FromArgb(156, 101, 0));
                                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 235, 156));
                            }
                        }
                        else
                        {
                            //Pick the last 45 percent for "Low"
                            using (ExcelRange r = worksheet.Cells[v.Key, varCol])
                            {
                                r.Value = "Low";
                            }
                        }
                        i++;
                    }
                }

                //Add notes/legend info
                int noteStartRow = rowNum;
                AddMergedCell(worksheet, rowNum, 1, 6, "NOTES:", r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                });
                AddMergedCell(worksheet, rowNum++, 7, 22, "LEGEND", r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                });

                AddMergedCell(worksheet, rowNum, 1, 6, "SELECTION PERIOD", r => { r.Style.Font.Bold = true; });
                AddMergedCell(worksheet, rowNum++, 7, 22, " - Net Promoter Score (NPS) = difference in percentage of top two box and bottom two box to the 'likelihood to recommend' question.");

                AddMergedCell(worksheet, rowNum, 1, 6, "Guests are selected based on their last visit at each site in the past week prior to selection. The interval between surveys must be at least 90 days.", r => { r.Style.WrapText = true; });
                AddMergedCell(worksheet, rowNum++, 7, 22, " - Guest Experience Index (GEI) = weighted average of responses to Guest Loyalty questions converted to a 100 pt equal interval scale.");

                AddMergedCell(worksheet, rowNum, 1, 6, "SAMPLE SIZE", r => { r.Style.Font.Bold = true; });
                AddMergedCell(worksheet, rowNum++, 7, 22, " - Problem Resolution Score (PRS) =  of those who had a problem, top two box percentage of \"overall ability to fix problem\" question.");

                AddMergedCell(worksheet, rowNum, 1, 6, "Sample Size only takes into account completed surveys.  For a statistically significant survey report and margin of error within reasonable confidence, we recommend a minimum sample size of 300 respondents per reporting period.", r => { r.Style.WrapText = true; });
                AddMergedCell(worksheet, rowNum++, 7, 22, " - 5 point scales used:");

                AddMergedCell(worksheet, rowNum, 1, 6, "REPORTING PERIOD", r => { r.Style.Font.Bold = true; });
                AddMergedCell(worksheet, rowNum++, 7, 22, "     RATIONAL CONNECTIONS, TOUCHPOINTS, ATTRIBUTES: excellent, very good, good, fair, poor");

                AddMergedCell(worksheet, rowNum, 1, 6, "Given the latest results, we will continue to collect and tabulate results on a weekly/monthly basis, but reporting will be grouped quarterly.", r => { r.Style.WrapText = true; });
                AddMergedCell(worksheet, rowNum++, 7, 22, "     EMOTIONAL CONNECTIONS, BRAND CONNECTIONS: strongly agree,moderately agree, slightly agree,disagree, strongly disagree");

                AddMergedCell(worksheet, rowNum, 1, 6, "BENCHMARK", r => { r.Style.Font.Bold = true; });
                AddMergedCell(worksheet, rowNum++, 7, 22, "     GUEST LOYALTY: definitely would, probably would, possibly would, probably would not, definitely would not");

                AddMergedCell(worksheet, rowNum, 1, 6, "The program started on March 19, 2012. Q2 Results will provide the basis for a comparative benchmark, representing the first full quarter of data collection (Apr/September/September).", r => { r.Style.WrapText = true; });
                AddMergedCell(worksheet, rowNum++, 7, 22, " - Performance Score %: Top Two Box % = excellent+very good", r => { r.Style.Font.Bold = true; });

                AddMergedCell(worksheet, rowNum, 1, 6, String.Empty, r => { r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin; });
                AddMergedCell(worksheet, rowNum++, 7, 22, " - GCGC numbers are total for  all properties ", r => { r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin; });

                using (ExcelRange r = worksheet.Cells[noteStartRow + 1, 1, rowNum - 1, 22])
                {
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                string fileName = "GCGC-QuarterlyReport-" + ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss", User) + ".xlsx";
                const string lPath = "~/Files/Cache/";

                string lOutput = string.Concat(MapPath(lPath), fileName);

                FileInfo fi = new FileInfo(lOutput);
                p.SaveAs(fi);

                hlDownload.Text = "Download File - " + fileName;
                hlDownload.NavigateUrl = String.Format("{0}{1}", lPath, fileName);
            }
        }

        private int AddYesNoRow(ExcelWorksheet worksheet, Dictionary<string, DataRow> rowLookup, int rowNum, bool isTitleCell, string mainLabel, string dataColumn)
        {
            AddDataRow(worksheet, rowLookup, rowNum++, false, mainLabel, null);
            AddDataRow(worksheet, rowLookup, rowNum++, false, "Yes", dataColumn + "_Yes", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
            AddDataRow(worksheet, rowLookup, rowNum++, false, "No", dataColumn + "_No", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
            return rowNum;
        }

        private void AddDataRow(ExcelWorksheet worksheet, Dictionary<string, DataRow> rowLookup, int rowNum, bool isTitleCell, string label, string dataColumn)
        {
            AddDataRow(worksheet, rowLookup, rowNum, isTitleCell, label, dataColumn, CellFormat.Percent);
        }

        private void AddDataRow(ExcelWorksheet worksheet, Dictionary<string, DataRow> rowLookup, int rowNum, bool isTitleCell, string label, string dataColumn, CellFormat cell)
        {
            AddDataRow(worksheet, rowLookup, rowNum, isTitleCell, label, dataColumn, cell, null, null);
        }

        private void AddDataRow(ExcelWorksheet worksheet, Dictionary<string, DataRow> rowLookup, int rowNum, bool isTitleCell, string label, string dataColumn, CellFormat cell, Action<ExcelRange> titleAction, Action<ExcelRange> dataAction)
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
            //Generate the headings if we haven't done so already.
            if (_startYear == -1)
            {
                GenerateHeadings(worksheet, rowLookup);
            }
            //Output the data
            for (int i = 0; i < _propertySortOrder.Length; i++)
            {
                int propertyID = _propertySortOrder[i];
                //Property name
                int propStartCol = START_COL + i * _colsPerProperty;
                //Loop through each year for this property
                for (int yr = 0; yr < _yearsInclusive; yr++)
                {
                    int yearStartCol = propStartCol + (yr * 5);
                    bool isLastYear = (yr == _yearsInclusive - 1);
                    //Get the number of columns (4 quarters + total col). If this is the last year, we also want to get the variance (stddev) column.
                    int colCount = (isLastYear ? 6 : 5);
                    //Loop through the quarter data
                    for (int q = 1; q <= colCount; q++)
                    {
                        int curCol = yearStartCol + q - 1;
                        string colname = propertyID + "-" + (_startYear + yr) + (q == 5 ? " Total" : (q == 6 ? " StdDev" : " Q" + q));
                        string value = null;
                        if (dataColumn != null && rowLookup[dataColumn].Table.Columns.Contains(colname))
                        {
                            value = rowLookup[dataColumn][colname].ToString();
                        }
                        using (ExcelRange r = worksheet.Cells[rowNum, curCol])
                        {
                            if (!String.IsNullOrWhiteSpace(value))
                            {
                                r.Value = value.StringToDbl();
                                if (q != 6)
                                {
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
                                }
                            }
                            r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            if (q == 5)
                            {
                                r.Style.Font.Bold = true;
                            }
                            if (q == 6)
                            {
                                r.Style.Font.Bold = true;
                                r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                                //If we're on the variance (stddev) column and the value is not null, save the value into a dictionary for use later
                                if (!String.IsNullOrWhiteSpace(value))
                                {
                                    if (!_varianceLookup.ContainsKey(curCol))
                                    {
                                        _varianceLookup.Add(curCol, new Dictionary<int, double>());
                                    }
                                    _varianceLookup[curCol].Add(rowNum, value.StringToDbl());
                                }
                            }
                            if (q == 1 && (yr == 0 || isLastYear))
                            {
                                r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            }
                            if (dataAction != null)
                            {
                                dataAction(r);
                            }
                        }
                    }
                }
            }
        }

        private void GenerateHeadings(ExcelWorksheet worksheet, Dictionary<string, DataRow> rowLookup)
        {
            _startYear = rowLookup["DateYear"][1].ToString().StringToInt();
            _yearsInclusive = (DateTime.Now.Year - _startYear) + 1;
            _colsPerProperty = (_yearsInclusive * 5) + 1; //+ 1 for variance column
            //Loop through and add property headers
            for (int i = 0; i < _propertySortOrder.Length; i++)
            {
                //Property name
                int propStartCol = START_COL + i * _colsPerProperty;
                worksheet.Cells[5, propStartCol].Value = PropertyTools.GetCasinoName(_propertySortOrder[i]);
                using (ExcelRange r = worksheet.Cells[5, propStartCol, 5, propStartCol + _colsPerProperty - 1])
                {
                    r.Merge = true;
                    r.Style.Font.Bold = true;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }
                //Years
                for (int yr = 0; yr < _yearsInclusive; yr++)
                {
                    bool isLastYear = (yr == _yearsInclusive - 1);
                    int yearStartCol = propStartCol + (yr * 5);
                    worksheet.Cells[6, yearStartCol].Value = _startYear + yr;
                    using (ExcelRange r = worksheet.Cells[6, yearStartCol, 6, yearStartCol + (isLastYear ? 5 : 4)])
                    {
                        r.Merge = true;
                        r.Style.Font.Bold = true;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        if (yr == 0)
                        {
                            r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        }
                        if (isLastYear)
                        {
                            r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        }
                    }
                    //Quarters
                    AddQuarterTitleCell(worksheet, rowLookup, yearStartCol, 1, yr == 0, false);
                    AddQuarterTitleCell(worksheet, rowLookup, yearStartCol, 2, false, false);
                    AddQuarterTitleCell(worksheet, rowLookup, yearStartCol, 3, false, false);
                    AddQuarterTitleCell(worksheet, rowLookup, yearStartCol, 4, false, false);
                    AddQuarterTitleCell(worksheet, rowLookup, yearStartCol, 5, false, false);
                    //If last year for this property, add the variance column
                    if (isLastYear)
                    {
                        AddQuarterTitleCell(worksheet, rowLookup, yearStartCol, 6, false, true);
                    }
                }
            }
        }

        private void AddQuarterTitleCell(ExcelWorksheet worksheet, Dictionary<string, DataRow> rowLookup, int yearStartCol, int quarter, bool isFirst, bool isLast)
        {
            using (ExcelRange r = worksheet.Cells[7, yearStartCol + quarter - 1])
            {
                if (quarter == 5)
                {
                    r.Value = "Total";
                }
                else if (quarter == 6)
                {
                    r.Value = "Variance";
                }
                else
                {
                    r.Value = "Q" + quarter;
                }
                r.Style.Font.Bold = true;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                if (yearStartCol + quarter - 1 == 0)
                {
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                }
                if (isFirst)
                {
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                }
                if (isLast)
                {
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                }
            }
        }

        private void AddQuarterCell(ExcelWorksheet worksheet, DataRow row, int yearStartCol, int quarter, bool isFirst, bool isLast)
        {
            using (ExcelRange r = worksheet.Cells[7, yearStartCol + quarter - 1])
            {
                if (quarter == 5)
                {
                    r.Value = "Total";
                }
                else
                {
                    r.Value = "Q" + quarter;
                }
                r.Style.Font.Bold = true;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                if (yearStartCol + quarter - 1 == 0)
                {
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                }
                if (isFirst)
                {
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                }
                if (isLast)
                {
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                }
            }
        }

        private void AddMergedCell(ExcelWorksheet worksheet, int row, int firstCol, int lastCol, string text)
        {
            AddMergedCell(worksheet, row, firstCol, lastCol, text, null);
        }

        private void AddMergedCell(ExcelWorksheet worksheet, int row, int firstCol, int lastCol, string text, Action<ExcelRange> action)
        {
            worksheet.Cells[row, firstCol].Value = text;
            using (ExcelRange r = worksheet.Cells[row, firstCol, row, lastCol])
            {
                r.Merge = true;
                if (action != null)
                {
                    action(r);
                }
            }
        }
    }
}