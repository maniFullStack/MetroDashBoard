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

namespace GCC_Web_Portal.Reports.Hotel {
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
                



                //DateTime bom = DateTime.Now.Date.AddDays(-DateTime.Now.Day + 1);

                // For archived portal setting up date time till end of 2016

                DateTime bom = new DateTime(2017, 01, 01);


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
            DataTable dt = sql.ExecStoredProcedureDataTable( "spReports_Monthly_Hotel",
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
				worksheet.Column( 6 ).Width = 19.28f;
				worksheet.Column( 7 ).Width = 19.28f;
				worksheet.Column(8).Width = 4.14f;
                worksheet.Column(9).Width = 10.71f;
                worksheet.Column(10).Width = 10.71f;
                worksheet.Column(11).Width = 10.71f;
                worksheet.Column(12).Width = 10.71f; ;

                //Sheet title
                worksheet.Cells["A1"].Value = PropertyTools.GetCasinoName((int)sc).ToUpper() + " HOTEL GUEST EXPERIENCE REPORT";
                using (ExcelRange r = worksheet.Cells["A1:C2"])
                {
                    r.Merge = true;
                    r.Style.Font.Size = 14;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                }
                worksheet.Cells["A3"].Value = String.Format("REPORTING PERIOD: {0}", mon.ToString("MMMM, yyyy"));
                using (ExcelRange r = worksheet.Cells["A3:B8"])
                {
                    r.Merge = true;
                    r.Style.Font.Size = 14;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                }

                AddMergedCell(worksheet, "C7", "D7", "Green: +10% change or more", r => r.Style.Font.Bold = true);
                AddMergedCell(worksheet, "E7", "F7", "Red: -10% change or more", r => r.Style.Font.Bold = true);

                float titleFontSize = 10f;

                AddMergedCell(worksheet, "C9", "C10", "CURRENT PERIOD", r =>
                {
                    r.Style.WrapText = true;
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                });

                AddMergedCell(worksheet, "D9", "G9", "VARIANCE FROM", r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                });

                AddMergedCell(worksheet, "I9", "L9", "PERFORMANCE " + ( mon.AddYears( -1 ) ).ToString( "yyyy" ), r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                });

                AddMergedCell(worksheet, "A10", "B10", "Total Sample:", r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                });

                using (ExcelRange r = worksheet.Cells["D10"])
                {
                    r.Value = "PREVIOUS PERIOD";
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                using (ExcelRange r = worksheet.Cells["E10"])
                {
                    r.Value = "M/M Change";
                    r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
				}

				using ( ExcelRange r = worksheet.Cells["F10"] ) {
					r.Value = "Last 12 Mo";
					r.Style.Font.Size = titleFontSize;
					r.Style.Font.Bold = true;
					r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
				}

				using ( ExcelRange r = worksheet.Cells["G10"] ) {
					r.Value = "Change from L12M";
					r.Style.Font.Size = titleFontSize;
					r.Style.Font.Bold = true;
					r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
					r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
				}

				using (ExcelRange r = worksheet.Cells["I10"])
                {
					r.Value = "Q1/FY" + ( mon.AddYears( -1 ) ).ToString( "yy" );
					r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                using (ExcelRange r = worksheet.Cells["J10"])
                {
					r.Value = "Q2/FY" + ( mon.AddYears( -1 ) ).ToString( "yy" );
					r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                using (ExcelRange r = worksheet.Cells["K10"])
                {
					r.Value = "Q3/FY" + ( mon.AddYears( -1 ) ).ToString( "yy" );
					r.Style.Font.Size = titleFontSize;
                    r.Style.Font.Bold = true;
                    r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                using (ExcelRange r = worksheet.Cells["L10"])
                {
					r.Value = "Q4/FY" + ( mon.AddYears( -1 ) ).ToString( "yy" );
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

                int rowNum = 11;

                AddDataRow(worksheet, lookup, rowNum++, true, "SAMPLE SIZE", "SampleCount", CellFormat.Number);
				// Overall Stay
                AddDataRow(worksheet, lookup, rowNum++, true, "OVERALL STAY", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Satisfaction score", "Q1Overall");
				// GSEI
                AddDataRow(worksheet, lookup, rowNum++, true, "GUEST SERVICE EXPERIENCE INDEX (GSEI)", null);
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall GSEI", "Q2" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Ensuring all of your needs were met", "Q1A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Making you feel welcome", "Q1B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Going above & beyond normal service", "Q1C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Speed of service", "Q1D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Encouraging you to visit again", "Q1E" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Overall staff availability", "Q1F" );
				//ROOMS
				AddDataRow( worksheet, lookup, rowNum++, true, "ROOMS", null );
				AddDataRow( worksheet, lookup, rowNum++, false, "Overall Rooms Score", "RoomScore" );
				//Reservation, Front Desk
				AddDataRow( worksheet, lookup, rowNum++, true, "Reservation, Front Desk", null );
				AddDataRow( worksheet, lookup, rowNum++, false, "Overall Score", "ReservationScore");
				AddDataRow( worksheet, lookup, rowNum++, false, "Friendliness of Reservation Agent", "Q3A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Helpfulness of Reservation Agent", "Q3B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Accuracy of reservation information upon check-in", "Q3C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Employee knowledge of the River Rock Casino Resort & Facilities", "Q3D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Efficiency of check-in", "Q3E" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Friendliness of Front Desk staff", "Q3F" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Helpfulness of Front Desk staff", "Q3G" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Employees' 'can-do' attitude", "Q3H" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Efficiency of check-out", "Q3I" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Accuracy of bill at check-out", "Q3J" );
				// Housekeeping
				AddDataRow( worksheet, lookup, rowNum++, true, "Housekeeping", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "HousekeepingScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Friendliness of Housekeeping staff", "Q4A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Room cleanliness", "Q4B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Bathroom cleanliness", "Q4C" );
				// Hotel Room
				AddDataRow( worksheet, lookup, rowNum++, true, "Hotel Room", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "HotelRoomScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Towels & Linens", "Q5A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Proper functioning of lights, TV, etc.", "Q5B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Overall condition of the room", "Q5C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Adequate amenities", "Q5D" );
				// Fitness Centre
				AddDataRow( worksheet, lookup, rowNum++, true, "Fitness Centre", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "% Used", "Q6FitnessCenter");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "FitnessScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Cleanliness of Fitness Center", "Q11A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Quality/ condition of fitness equipment", "Q11B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Availability of Fitness Center equipment", "Q11C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Variety of equipment", "Q11D" );
				// Pool / Hot Tub
				AddDataRow( worksheet, lookup, rowNum++, true, "Pool / Hot Tub", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "% Used", "Q6PoolHotTub");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "PoolScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Cleanliness of pool area", "Q12A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Temperature of pool", "Q12B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Cleanliness of hot tub area", "Q12C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Temperature of hot tub", "Q12D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Cleanliness of changing rooms", "Q12E" );
				// Valet Parking
				AddDataRow( worksheet, lookup, rowNum++, true, "Valet Parking", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "% Used", "Q6ValetParking");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "ValetScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Greeting upon arrival", "Q14A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Car returned in timely manner", "Q14B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Original mirror position", "Q14C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Original radio station", "Q14D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Original seat position", "Q14E" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Valet driver drove care in respectful manner", "Q14F" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Pleasant departure greeting", "Q14G" );
				// Concierge
				AddDataRow( worksheet, lookup, rowNum++, true, "Concierge", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "% Used", "Q6Concierge");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "ConciergeScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Availability of Concierge", "Q15A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Friendliness of Concierge", "Q15B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Employee knowledge of the River Rock Casino Resort & Facilities", "Q15C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Staff member went out of way to provide excellent service", "Q15D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Pleasant departure greeting", "Q15E" );
				// Bell /Door Service
				AddDataRow( worksheet, lookup, rowNum++, true, "Bell /Door Service", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "% Used", "Q6BellDoorService");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "BellDoorScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Greeting upon arrival", "Q16A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Acknowledgement throughout stay", "Q16B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Friendliness of bell/ door staff", "Q16C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Employee knowledge of the River Rock Casino Resort & Facilities", "Q16D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Staff member went out of way to provide excellent service", "Q16E" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Pleasant departure greeting", "Q16F" );
				// FOOD & BEVERAGE, CATERING
				AddDataRow( worksheet, lookup, rowNum++, true, "FOOD & BEVERAGE, CATERING", null );
				AddDataRow( worksheet, lookup, rowNum++, false, "Overall F & B, Catering Score", "RoomScore" );
				// Tramonto
				AddDataRow( worksheet, lookup, rowNum++, true, "Tramonto", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "% Used", "Q6Tramonto");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "TramontoScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Greeting upon arrival", "Q7A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Timeliness of seating", "Q7B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Attentiveness of server", "Q7C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Server's knowledge of menu selections", "Q7D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Timeliness of meal delivery", "Q7E" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Quality and taste of food", "Q7F" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Presentation of food", "Q7G" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Quality of beverage", "Q7H" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Accuracy of bill", "Q7I" );
				// Buffet
				AddDataRow( worksheet, lookup, rowNum++, true, "Buffet", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "% Used", "Q6TheBuffet");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "BuffetScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Greeting upon arrival", "Q8A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Attentiveness of server", "Q8B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Server's knowledge of menu selections", "Q8C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Quality and taste of food", "Q8D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Quality of beverage", "Q8E" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Accuracy of bill", "Q8F" );
				// Curve
				AddDataRow( worksheet, lookup, rowNum++, true, "Curve", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "% Used", "Q6Curve");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "CurveScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Greeting upon arrival", "Q9A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Timeliness of seating", "Q9B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Attentiveness of server", "Q9C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Server's knowledge of menu selections", "Q9D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Timeliness of meal delivery", "Q9E" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Quality and taste of food", "Q9F" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Presentation of food", "Q9G" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Quality of beverage", "Q9H" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Accuracy of bill", "Q9I" );
				// In-Room Dining
				AddDataRow( worksheet, lookup, rowNum++, true, "In-Room Dining", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "% Used", "Q6InRoomDining");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "InRoomScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Phone answered promptly", "Q10A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Friendliness of order taker", "Q10B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Friendliness of server", "Q10C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Order delivered within time period advised", "Q10D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Accuracy of order", "Q10E" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Presentation of food", "Q10F" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Quality of in-room dining food", "Q10G" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Delivery staff offered pick-up of empty tray", "Q10H" );
				// Meeting & Events
				AddDataRow( worksheet, lookup, rowNum++, true, "Meeting & Events", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "% Used", "Q6Meeting");
                AddDataRow(worksheet, lookup, rowNum++, false, "Overall Score", "MeetingScore");
                AddDataRow( worksheet, lookup, rowNum++, false, "Condition and cleanliness of meeting/event room", "Q13A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Proper meeting/event room temperature", "Q13B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Quality of meeting/event food and beverage", "Q13C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Friendliness and efficiency of meeting/event staff", "Q13D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Quality/condition/support of technical equipment", "Q13E" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Meeting/event facilities (size, design, amenities)", "Q13F" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Accuracy of meeting/ event signage", "Q13G" );
				// SERVICE RECOVERY
				AddDataRow( worksheet, lookup, rowNum++, true, "SERVICE RECOVERY", null );
				AddDataRow( worksheet, lookup, rowNum++, false, "Experience problem?", "Q23" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Report problem?", "Q24C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Where experienced problem? (% of all problems)", null );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Arrival", "Q24A_Arrival" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Staff", "Q24A_Staff" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Guest Room", "Q24A_GuestRoom" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Food & Beverage", "Q24A_FoodBeverage" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Facilities & Service", "Q24A_FacilitiesService" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Billing/Departure", "Q24A_BillingDeparture" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Meetings & Events", "Q24A_MeetingsEvents" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Other", "Q24A_Other" );
                AddDataRow(worksheet, lookup, rowNum++, false, "", null);
                AddDataRow( worksheet, lookup, rowNum++, false, "Overall ability to fix problem (PRS Score)", "Q24D" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Attribute ratings:", null );
				AddDataRow( worksheet, lookup, rowNum++, false, "  The length of time taken to resolve your problem", "Q24E_1" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  The effort of employees in resolving your problem", "Q24E_2" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  The courteousness of employees while resolving your problem", "Q24E_3" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  The amount of communication with you from employees while resolving your problem", "Q24E_4" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  The fairness of the outcome in resolving your problem", "Q24E_5" );
				// Satisfaction Attributes
				AddDataRow( worksheet, lookup, rowNum++, true, "Satisfaction Attributes", null );
				AddDataRow( worksheet, lookup, rowNum++, false, "Satisfaction with how we made you feel", null );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Welcome", "Q17A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Comfortable", "Q17B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Important", "Q17C" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Satisfaction with Overall Stay", null );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Overall condition of the River Rock Casino Resort", "Q18A" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Value for Price", "Q18B" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Likelihood to Return", "Q19" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Likelihood to Recommend", "Q20" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Received \"Exceptional\" Service", "Q21" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Importance of \"Green\" Initiatives", "Q22" );
				AddDataRow( worksheet, lookup, rowNum++, false, "Visited River Rock before?", "Q27" );
                AddDataRow(worksheet, lookup, rowNum++, false, "", null);
                AddDataRow( worksheet, lookup, rowNum++, false, "Primary Reason for Choosing (% of all Responses)", null );
                AddDataRow(worksheet, lookup, rowNum++, false, "  Articles/Advertisements", "Q25_Articles");
                AddDataRow(worksheet, lookup, rowNum++, false, "  Business meeting/Conference venue", "Q25_Business");
                AddDataRow(worksheet, lookup, rowNum++, false, "  Facilities/Amenities", "Q25_Facilities");
                AddDataRow(worksheet, lookup, rowNum++, false, "  Location", "Q25_Location");
                AddDataRow(worksheet, lookup, rowNum++, false, "  Other, please specify", "Q25_Other");
                AddDataRow(worksheet, lookup, rowNum++, false, "  Personal recommendation", "Q25_Personal");
                AddDataRow(worksheet, lookup, rowNum++, false, "  Previous visit", "Q25_Previous");
                AddDataRow(worksheet, lookup, rowNum++, false, "  Special package/rate", "Q25_Special");
                AddDataRow(worksheet, lookup, rowNum++, false, "  Travel Agent", "Q25_Travel");
                AddDataRow(worksheet, lookup, rowNum++, false, "  Website", "Q25_Website");
                AddDataRow(worksheet, lookup, rowNum++, false, "", null);
                AddDataRow( worksheet, lookup, rowNum++, false, "Reason to Visit (% of all Responses)", null );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Business", "Q26Business" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Pleasure", "Q26Pleasure" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Meeting / Event", "Q26MeetingEvent" );
				AddDataRow( worksheet, lookup, rowNum++, false, "  Other", "Q26Other" );
                AddDataRow(worksheet, lookup, rowNum++, false, "", null);
                AddDataRow( worksheet, lookup, rowNum++, false, "Follow-up on comments requested", "Q29" );

                // Bottom Line
                worksheet.Cells["A" + rowNum].Value = "";
                using (ExcelRange r = worksheet.Cells["A" + rowNum + ":L" + rowNum])
                {
                    r.Merge = true;
                    r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }
                rowNum++;
                // Bottom Message
                worksheet.Cells["A" + rowNum].Value = "All scores are top-2 box scores on a scale of 5, unless otherwise indicated.";
                using (ExcelRange r = worksheet.Cells["A" + rowNum + ":L" + rowNum])
                {
                    r.Merge = true;
                    r.Style.Font.Size = 10;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                string fileName = sc.ToString() + "-Hotel-Monthly-" + ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss", User) + ".xlsx";
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
            for (int i = 1; i <= 9; i++)
            {
                int curCol = 2 + i;
                if (i > 5)
                {
                    curCol++;
                }
                string value = null;
				int rowKey = i;
				if (rowKey > 5 ) {
					rowKey--;
				}
                if (dataColumn != null && rowLookup.ContainsKey( rowKey ) )
                {
					if ( i == 4 ) {
						value = rowLookup[3][dataColumn].ToString();
					} else if ( i > 4) {
						value = rowLookup[i - 1][dataColumn].ToString();
					} else {
						value = rowLookup[i][dataColumn].ToString();
					}
                    //Columns 3 and 5 are diff columns, so we subtract them.
                    if (!String.IsNullOrWhiteSpace(value) && ( i == 3 || i == 5 ))
                    {
						int rowToUse = 1;
						if ( i == 3 ) {
							rowToUse = 2;
						} else if ( i == 5 ) {
							rowToUse = 3;
						}
						if ( rowLookup.ContainsKey( rowToUse ) ) {
							double otherVal = rowLookup[rowToUse][dataColumn].ToString().StringToDbl();
							double curVal = 0;
							if ( rowLookup.ContainsKey( 1 ) ) {
								curVal = rowLookup[1][dataColumn].ToString().StringToDbl();
							}
							value = ( curVal - otherVal ).ToString();
						}
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
								if ( i == 3 || i == 5 ) {
									if ( value.StringToDbl() <= -0.1 ) {
										r.Style.Numberformat.Format = "0.0%";
										r.Style.Font.Color.SetColor( Color.Red );
									} else if ( value.StringToDbl() >= 0.1 ) {
										r.Style.Numberformat.Format = "0.0%";
										r.Style.Font.Color.SetColor( Color.Green );
									} else {
										r.Style.Numberformat.Format = "0.0%";
									}
								} else {
									r.Style.Numberformat.Format = "0.0%";
								}
                                break;
                        }
                    }
                    if (i == 1 || i == 9)
                    {
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    else if (i == 5)
                    {
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    else if (i >= 5 && i < 9)
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