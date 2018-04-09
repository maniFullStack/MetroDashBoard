using OfficeOpenXml;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class MonthlyReport : AuthenticatedPage
    {
        protected DataTable Data = null;

        protected enum CellFormat
        {
            Index,
            Percent,
            Number
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.HideAllFilters = true;
            if (!IsPostBack)
            {
                DateTime startDate = new DateTime(2015, 05, 01);
                DateTime bom = DateTime.Now.Date.AddDays(-DateTime.Now.Day + 1);
                int months = ((bom.Year - startDate.Year) * 12) + bom.Month - startDate.Month;
                for (int i = months - 1; i >= 0; i--)
                {
                    DateTime curMon = startDate.AddMonths(i);
                    ddlMonth.Items.Add(new ListItem(curMon.ToString("MMMM, yyyy"), curMon.ToString("yyyy-MM")));
                }
                if (Master.IsPropertyUser)
                {
                    //If it's not GAG, set the property dropdown to match the user's property
                    ddlProperty.SelectedValue = ((int)User.PropertyShortCode).ToString();
                    ddlProperty.Visible = false;
                }
                else
                {
                    ddlProperty.Visible = true;
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            sql.CommandTimeout = 90;
            DataTable dt = sql.ExecStoredProcedureDataTable("spReports_Monthly",
                                                                new SqlParameter("@MonthStart", ddlMonth.SelectedValue + "-01"),
                                                                new SqlParameter("@PropertyID", ddlProperty.SelectedValue));
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Unable to query report data from the database.";
                return;
            }

            using (ExcelPackage p = new ExcelPackage())
            {
                string[] date = ddlMonth.SelectedValue.Split('-');
                DateTime mon = new DateTime(date[0].StringToInt(2017), date[1].StringToInt(1), 1);

                GCCPropertyShortCode sc = (GCCPropertyShortCode)ddlProperty.SelectedValue.StringToInt(0);

                p.Workbook.Worksheets.Add(PropertyTools.GetCasinoName((int)sc));
                ExcelWorksheet worksheet = p.Workbook.Worksheets[1];
                worksheet.Cells.Style.Font.Size = 10; //Default font size for whole sheet
                worksheet.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                worksheet.Cells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);

                worksheet.Column(1).Width = 12.71f;
                worksheet.Column(2).Width = 54.85f;

                worksheet.Column(3).Width = 10f;
                worksheet.Column(4).Width = 15.42f;
                worksheet.Column(5).Width = 14.57f;
                worksheet.Column(6).Width = 19.28f;
                worksheet.Column(7).Width = 4.14f;
                worksheet.Column(8).Width = 10.71f;
                worksheet.Column(9).Width = 10.71f;
                worksheet.Column(10).Width = 10.71f;
                worksheet.Column(11).Width = 10.71f; ;

                //Sheet title
                worksheet.Cells["A1"].Value = PropertyTools.GetCasinoName((int)sc).ToUpper() + " GUEST EXPERIENCE REPORT";
                using (ExcelRange r = worksheet.Cells["A1:B8"])
                {
                    r.Merge = true;
                    r.Style.Font.Size = 14;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                }
                worksheet.Cells["A9"].Value = String.Format("REPORTING PERIOD: {0}", mon.ToString("MMMM, yyyy"));
                using (ExcelRange r = worksheet.Cells["A9:B14"])
                {
                    r.Merge = true;
                    r.Style.Font.Size = 14;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                }

                //Legend
                worksheet.Cells["C1"].Value = "LEGEND";
                using (ExcelRange r = worksheet.Cells["C1:K1"])
                {
                    r.Merge = true;
                    r.Style.Font.Size = 12;
                    r.Style.Font.Bold = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                }
                AddMergedCell(worksheet, "C2", "K2", "- Net Promoter Score (NPS) = difference in percentage of top two box and bottom two box to the 'likelihood to recommend' question.", r => { r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; });
                AddMergedCell(worksheet, "C3", "K3", "- Guest Experience Index (GEI) = weighted average of responses to Guest Loyalty questions converted to a 100 point equal interval scale.", r => { r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; });
                AddMergedCell(worksheet, "C4", "K4", "- Problem Resolution Score (PRS) =  of those who had a problem, top two box percentage of \"overall ability to fix problem\" question.", r => { r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; });
                AddMergedCell(worksheet, "C5", "K5", "- 5 point scales used:", r => { r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; });
                AddMergedCell(worksheet, "C6", "K6", "RATIONAL CONNECTIONS, TOUCHPOINTS, ATTRIBUTES: excellent, very good, good, fair, poor", r => { r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Indent = 2; });
                AddMergedCell(worksheet, "C7", "K7", "EMOTIONAL CONNECTIONS, BRAND CONNECTIONS: strongly agree,moderately agree, slightly agree,disagree, strongly disagree", r => { r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Indent = 2; });
                AddMergedCell(worksheet, "C8", "K8", "GUEST LOYALTY: definitely would, probably would, possibly would, probably would not, definitely would not", r => { r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Indent = 2; });
                AddMergedCell(worksheet, "C9", "K9", "- Performance Score %: Top Two Box % = excellent+very good; strongly agree+moderately agree; definitely would +probably would", r => { r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; });
                AddMergedCell(worksheet, "C10", "K10", "- GCGC numbers are total for  all properties ", r => { r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium; });

                AddMergedCell(worksheet, "C12", "D12", "Green: +10% change or more", r => r.Style.Font.Bold = true);
                AddMergedCell(worksheet, "E12", "F12", "Red: -10% change or more", r => r.Style.Font.Bold = true);

                float titleFontSize = 10f;

                AddMergedCell(worksheet, "C14", "C15", "CURRENT PERIOD", r =>
                {
                    r.Style.WrapText = true;
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                });

                AddMergedCell(worksheet, "D14", "F14", "VARIANCE FROM", r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                });

                AddMergedCell(worksheet, "H14", "K14", "PERFORMANCE " + "2015", r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                });

                AddMergedCell(worksheet, "A15", "B15", "Total Sample:", r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                });

                using (ExcelRange r = worksheet.Cells["D15"])
                {
                    r.Value = "PREVIOUS PERIOD";
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                using (ExcelRange r = worksheet.Cells["E15"])
                {
                    r.Value = "YEAR AGO";
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                using (ExcelRange r = worksheet.Cells["F15"])
                {
                    r.Value = "CURRENT PERIOD GCGC";
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                using (ExcelRange r = worksheet.Cells["H15"])
                {
                    r.Value = "Q1/FY15"; //+mon.ToString("yy");
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                using (ExcelRange r = worksheet.Cells["I15"])
                {
                    r.Value = "Q2/FY15"; //+mon.ToString("yy");
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                using (ExcelRange r = worksheet.Cells["J15"])
                {
                    r.Value = "Q3/FY15"; // + mon.ToString("yy");
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                using (ExcelRange r = worksheet.Cells["K15"])
                {
                    r.Value = "Q4/FY15"; //+mon.ToString("yy");
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                //Set up the lookup dictionary for getting rows when they're not all there
                Dictionary<int, DataRow> lookup = new Dictionary<int, DataRow>();
                foreach (DataRow dr in dt.Rows)
                {
                    int rowid = dr["DateRange"].ToString()[0].ToString().StringToInt();
                    lookup.Add(rowid, dr);
                }

                int rowNum = 16;

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

                string fileName = sc.ToString() + "-Monthly-" + ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss", User) + ".xlsx";
                const string lPath = "~/Files/Cache/";

                string lOutput = string.Concat(MapPath(lPath), fileName);

                FileInfo fi = new FileInfo(lOutput);
                p.SaveAs(fi);

                hlDownload.Text = "Download File - " + fileName;
                hlDownload.NavigateUrl = String.Format("{0}{1}", lPath, fileName);
            }
        }

        private int AddYesNoRow(ExcelWorksheet worksheet, Dictionary<int, DataRow> rowLookup, int rowNum, bool isTitleCell, string mainLabel, string dataColumn)
        {
            AddDataRow(worksheet, rowLookup, rowNum++, false, mainLabel, null);
            AddDataRow(worksheet, rowLookup, rowNum++, false, "Yes", dataColumn + "_Yes", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
            AddDataRow(worksheet, rowLookup, rowNum++, false, "No", dataColumn + "_No", CellFormat.Percent, r => { r.Style.Indent = 3; }, null);
            return rowNum;
        }

        private void AddDataRow(ExcelWorksheet worksheet, Dictionary<int, DataRow> rowLookup, int rowNum, bool isTitleCell, string label, string dataColumn)
        {
            AddDataRow(worksheet, rowLookup, rowNum, isTitleCell, label, dataColumn, CellFormat.Percent);
        }

        private void AddDataRow(ExcelWorksheet worksheet, Dictionary<int, DataRow> rowLookup, int rowNum, bool isTitleCell, string label, string dataColumn, CellFormat cell)
        {
            AddDataRow(worksheet, rowLookup, rowNum, isTitleCell, label, dataColumn, cell, null, null);
        }

        private void AddDataRow(ExcelWorksheet worksheet, Dictionary<int, DataRow> rowLookup, int rowNum, bool isTitleCell, string label, string dataColumn, CellFormat cell, Action<ExcelRange> titleAction, Action<ExcelRange> dataAction)
        {
            if (isTitleCell)
            {
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
                worksheet.Cells[rowNum, 2].Value = label;
                if (titleAction != null)
                {
                    titleAction(worksheet.Cells[rowNum, 2]);
                }
            }
            for (int i = 1; i <= 8; i++)
            {
                int curCol = 2 + i;
                if (i > 4)
                {
                    curCol++;
                }
                string value = null;
                if (dataColumn != null && rowLookup.ContainsKey(i))
                {
                    value = rowLookup[i][dataColumn].ToString();
                    //Columns 2-4 are diff columns, so we subtract them.
                    if (!String.IsNullOrWhiteSpace(value) && i >= 2 && i <= 4)
                    {
                        double otherVal = value.StringToDbl();
                        double curVal = 0;
                        if (rowLookup.ContainsKey(1))
                        {
                            curVal = rowLookup[1][dataColumn].ToString().StringToDbl();
                        }
                        value = (curVal - otherVal).ToString();
                    }
                }
                using (ExcelRange r = worksheet.Cells[rowNum, curCol])
                {
                    if (!String.IsNullOrWhiteSpace(value))
                    {
                        r.Value = value.StringToDbl();
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
                    if (i == 1 || i == 8)
                    {
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    else if (i == 4)
                    {
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    else if (i >= 5 && i < 8)
                    {
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    if (dataAction != null)
                    {
                        dataAction(r);
                    }
                }
            }
        }

        private void AddMergedCell(ExcelWorksheet worksheet, string startCell, string endCell, string text)
        {
            worksheet.Cells[startCell].Value = text;
            using (ExcelRange r = worksheet.Cells[startCell + ":" + endCell])
            {
                r.Merge = true;
            }
        }

        private void AddMergedCell(ExcelWorksheet worksheet, string startCell, string endCell, string text, Action<ExcelRange> action)
        {
            worksheet.Cells[startCell].Value = text;
            using (ExcelRange r = worksheet.Cells[startCell + ":" + endCell])
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