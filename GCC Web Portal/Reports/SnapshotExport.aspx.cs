using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class SnapshotExport : AuthenticatedPage
    {
        private const int DATA_START_COL = 8;
        //private const int REPORT_YEAR = 2017;
        private const int REPORT_YEAR = 2017;

        protected DataTable Data = null;

        private bool OverrideMinimumCountCheck
        {
            get
            {
                return (new UserGroups[] { UserGroups.ForumAdmin, UserGroups.CorporateMarketing, UserGroups.HRStaff }).Contains(User.Group);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.HideAllFilters = true;
            if (!IsPostBack)
            {
                if (Master.IsPropertyUser)
                {
                    ddlRegion.SelectedValue = PropertyTools.GetCasinoRegion(User.PropertyShortCode);
                    // if this is a property user and they are from one of the GAG locations, let them select the property only
                    if (User.PropertyShortCode == GCCPropertyShortCode.GAG)
                    {
                        bool foundFirst = false;
                        foreach (ListItem li in ddlProperty.Items)
                        {
                            li.Enabled = li.Value.StartsWith("13-");
                            if (li.Enabled && !foundFirst)
                            {
                                li.Selected = true;
                                foundFirst = true;
                            }
                        }
                        ddlProperty_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        //If it's not GAG, set the property dropdown to match the user's property
                        ddlProperty.SelectedValue = ((int)User.PropertyShortCode).ToString();
                        ddlProperty_SelectedIndexChanged(null, null);
                    }
                }
                else
                {
                    ddlProperty.Visible = false;
                    ddlDepartment.Visible = false;
                }
            }
        }

        protected enum CellFormat
        {
            Percent,
            Number
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            GenerateReport(false);
        }

        protected void btnGenerateSurveillanceReport_Click(object sender, EventArgs e)
        {
            GenerateReport(true);
        }

        protected void GenerateReport(bool isSurveillanceReport)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            SQLParamList sqlParams = new SQLParamList();

            string labelColumn = "Region";
            bool isOverall = true;
            if (!String.IsNullOrWhiteSpace(ddlRegion.SelectedValue))
            {
                isOverall = false;
                //Add the region param
                sqlParams.Add("@Region", ddlRegion.SelectedValue);
                labelColumn = "Location";
                if (!String.IsNullOrWhiteSpace(ddlProperty.SelectedValue))
                {
                    //Add the property param
                    int propID = ddlProperty.SelectedValue.Split('-')[0].ToString().StringToInt();
                    sqlParams.Add("@PropertyID", propID);
                    labelColumn = "Department";
                    if (propID == 13)
                    {
                        //Add the GAG location param if this is a GAG location
                        sqlParams.Add("@GAGLocation", ddlProperty.SelectedItem.Text);
                    }
                    if (!String.IsNullOrWhiteSpace(ddlDepartment.SelectedValue))
                    {
                        //Add the department param
                        sqlParams.Add("@Department", ddlDepartment.SelectedValue);
                    }
                }
            }

            DataSet ds;
            if (isSurveillanceReport)
            {
                ds = sql.ExecStoredProcedureDataSet("spReports_Snapshot_Details_Department");
            }
            else
            {
                ds = sql.ExecStoredProcedureDataSet("spReports_Snapshot_Details", sqlParams);
            }
            if (sql.HasError || ds.Tables.Count != 4)
            {
                TopMessage.ErrorMessage = "Unable to query report data from the database.";
                return;
            }
            if (ds.Tables[0].Rows.Count == 0)
            {
                TopMessage.WarningMessage = "No data was found matching these filters.";
                return;
            }
            DataRow currentRow = ds.Tables[0].Rows[0];
            int completeSurveys = currentRow["SurveysCompleted"].ToString().StringToInt();
            if (completeSurveys < 10 && !OverrideMinimumCountCheck)
            {
                TopMessage.WarningMessage = String.Format("We cannot generate the report for these filters at this time because the minimum number of responses (10) has not yet been met. There {0} currently {1} response{2} for this filter combination.", completeSurveys == 1 ? "is" : "are", completeSurveys, completeSurveys == 1 ? String.Empty : "s");
                return;
            }

            DataRow hourlyRow;
            DataRow salaryRow;
            if (ds.Tables[0].Rows.Count == 3)
            {
                hourlyRow = ds.Tables[0].Rows[1];
                salaryRow = ds.Tables[0].Rows[2];
            }
            else if (ds.Tables[0].Rows.Count == 2 && ds.Tables[0].Rows[1]["PayType"].Equals("Salary"))
            {
                hourlyRow = ds.Tables[0].NewRow();
                salaryRow = ds.Tables[0].Rows[1];
            }
            else if (ds.Tables[0].Rows.Count == 2 && ds.Tables[0].Rows[1]["PayType"].Equals("Hourly"))
            {
                hourlyRow = ds.Tables[0].Rows[1];
                salaryRow = ds.Tables[0].NewRow();
            }
            else
            {
                hourlyRow = ds.Tables[0].NewRow();
                salaryRow = ds.Tables[0].NewRow();
            }

            Dictionary<int, DataRow> historicalRows = new Dictionary<int, DataRow>();
            int cn = 0;
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                int year = dr["Year"].ToString().StringToInt();
                historicalRows.Add(year, dr);
                cn++;
            }

            using (ExcelPackage p = new ExcelPackage())
            {
                string locationName = "BC - Surveillance";
                if (!isSurveillanceReport)
                {
                    locationName = ddlRegion.SelectedItem.Text;
                    if (!String.IsNullOrWhiteSpace(ddlProperty.SelectedValue))
                    {
                        if (!String.IsNullOrWhiteSpace(ddlDepartment.SelectedValue))
                        {
                            locationName = ddlProperty.SelectedItem.Text + " - " + ddlDepartment.SelectedItem.Text;
                        }
                        else
                        {
                            locationName = ddlProperty.SelectedItem.Text + " - All Departments";
                        }
                    }
                }

                string fileName = String.Format("{0}-SnapshotReport-{1}", MakeValidFileName(locationName.Replace(" ", String.Empty)), ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss", User));

                #region Report Worksheet

                p.Workbook.Worksheets.Add("Report");
                ExcelWorksheet worksheet = p.Workbook.Worksheets[1];
                worksheet.Cells.Style.Font.Size = 10; //Default font size for whole sheet
                worksheet.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                //Set cell backgrounds to white
                worksheet.Cells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);

                //Set the column widths
                for (int i = 1; i <= 11; i++)
                {
                    if (i < 4)
                    {
                        worksheet.Column(i).Width = 7.14f;
                    }
                    else if (i == 4 || i == 5)
                    {
                        worksheet.Column(i).Width = 12.33f;
                    }
                    else
                    {
                        worksheet.Column(i).Width = 6.57f;
                    }
                }

                //Set print view and header
                //Page Layout allows about 9 columns
                worksheet.View.PageLayoutView = true;
                worksheet.PrinterSettings.Orientation = eOrientation.Portrait;
                worksheet.HeaderFooter.FirstHeader.CenteredText = REPORT_YEAR + " Snapshot Survey Results\n" + locationName.Replace("&", "&&");

                //Show page numbers
                worksheet.HeaderFooter.FirstFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                worksheet.HeaderFooter.EvenFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                worksheet.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);

                int rowNum = 2;

                using (ExcelRange r = worksheet.Cells[rowNum, 1, rowNum, DATA_START_COL + 3])
                {
                    r.Merge = true;

               
                        r.Value = "Survey Administration Dates: September 15-29, 2017 & October 13-26, 2017 (RR Only)";
                  
                   
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                rowNum += 2;

                //Add the response rate details
                for (int i = 0; i < 4; i++)
                {
                    using (ExcelRange r = worksheet.Cells[rowNum, DATA_START_COL + i])
                    {
                        r.Value = REPORT_YEAR - i;
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                }

                rowNum++;

                Action<ExcelRange> yearHeadStyle = r =>
                {
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                };

                AddMergedCell(worksheet, "E" + rowNum, "G" + rowNum, "Response Rate\u00B9\u00B9", yearHeadStyle);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "ResponseRate", CellFormat.Percent, null);

                rowNum++;

                AddMergedCell(worksheet, "E" + rowNum, "G" + rowNum, "Total Surveys Completed", yearHeadStyle);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "SurveysCompleted", CellFormat.Number, null);

                rowNum++;

                AddMergedCell(worksheet, "E" + rowNum, "G" + rowNum, "Total Employees", yearHeadStyle);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "EmployeeCount", CellFormat.Number, null);

                rowNum += 2;

                //Response rate and goals section
                AddMergedCell(worksheet, "A" + rowNum, "E" + rowNum, "Response Rates and Goals:       ", r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size += 1;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                });

                Action<ExcelRange> subHeadStyle = r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size -= 1;
                    r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    r.Style.Border.Top.Color.SetColor(Color.FromArgb(32, 121, 64));
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                };
                AddMergedCell(worksheet, "F" + rowNum, "G" + rowNum, "Resp Rate Goal", subHeadStyle);
                worksheet.Cells["F" + rowNum].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                AddMergedCell(worksheet, "H" + rowNum, "I" + rowNum, "Resp Rate Actual", subHeadStyle);
                AddMergedCell(worksheet, "J" + rowNum, "K" + rowNum, "Difference", subHeadStyle);
                worksheet.Cells["K" + rowNum].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                rowNum++;

                Action<ExcelRange> style = r =>
                {
                    r.Style.Font.Size -= 1;
                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Numberformat.Format = "0.0%";
                };

                double targetResponseRate = currentRow["TargetResponseRate"].ToString().StringToDbl();
                AddMergedCell(worksheet, "F" + rowNum, "G" + rowNum, targetResponseRate, style);
                worksheet.Cells["F" + rowNum].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                double responseRate = currentRow["ResponseRate"].ToString().StringToDbl();
                AddMergedCell(worksheet, "H" + rowNum, "I" + rowNum, responseRate, style);
                AddMergedCell(worksheet, "J" + rowNum, "K" + rowNum, responseRate - targetResponseRate, style);
                worksheet.Cells["K" + rowNum].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                rowNum += 2;

                //Part 1 - Hewitt's Engagement Framework - Details
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "PART 1: Hewitt's Engagement Framework - Why Measure Engagement? \u00B9", r =>
                {
                    r.Style.Font.Size += 2;
                    r.Style.Font.Bold = true;
                });
                rowNum++;
                worksheet.Row(rowNum).Height *= 1.2f;
                AddMergedCell(worksheet, "A" + rowNum, "K" + (rowNum + 3), "Hewitt measures employee engagement because there is clear evidence that increasing employee engagement leads to improved organizational results. Ongoing research in academia and in Best Employers studies show that organizations with high employee engagement scores: better financial returns; more revenue growth; better prospective employee attraction; lower employee turnover; lower absenteeism; and better customer retention.", r =>
                {
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    r.Style.WrapText = true;
                });
                rowNum += 5;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "To measure engagement, Hewitt looks at three elements: say, stay and strive. Employees are engaged when they:");
                rowNum++;
                AddMergedCell(worksheet, "B" + rowNum, "K" + rowNum, "- Speak positively about the organization to co-workers, potential employees and customers;");
                rowNum++;
                AddMergedCell(worksheet, "B" + rowNum, "K" + rowNum, "- Have an intense desire to be a memeber of the organization; and");
                rowNum++;
                AddMergedCell(worksheet, "B" + rowNum, "K" + rowNum, "- Exert extra effort and are dedicated to doing the very best job possible to contribute to the organization's business success.", r =>
                {
                    r.Style.WrapText = true;
                });
                worksheet.Row(rowNum).Height = 26.21f;

                rowNum += 2;

                //Part 1 - Hewitt's Engagement Framework - Data
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "PART 1: Hewitt's Engagement Framework - " + locationName, r =>
                {
                    r.Style.Font.Size += 2;
                    r.Style.Font.Bold = true;
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                });
                worksheet.Row(rowNum).Height = 32.21f;

                rowNum += 2;

                //Header row
                style = r =>
                {
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                };

                worksheet.Row(rowNum).Height = 26.96f;

                style(worksheet.Cells[rowNum, 1]);
                using (ExcelRange r = worksheet.Cells[rowNum, 2])
                {
                    r.Value = "GC Snapshot";
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size -= 1;
                    r.Style.WrapText = true;
                    style(r);
                }
                using (ExcelRange r = worksheet.Cells[rowNum, 3])
                {
                    r.Value = "Hewitt";
                    r.Style.Font.Bold = true;
                    style(r);
                }
                style(worksheet.Cells[rowNum, 4, rowNum, DATA_START_COL - 1]);
                for (int i = 0; i < 4; i++)
                {
                    using (ExcelRange r = worksheet.Cells[rowNum, DATA_START_COL + i])
                    {
                        r.Value = REPORT_YEAR - i;
                        r.Style.Font.Bold = true;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                        style(r);
                    }
                }

                rowNum++;

                AddHewitt(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(255, 242, 0), "SAY",
                            "Q16", "Q1", "I would, without hesitation, recommend my workplace to a friend seeking employment.", "Q16",
                            "Q17", "Q2", "Given the opportunity, I tell others great things about working here.", "Q17");

                AddHewitt(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(245, 157, 86), "STAY",
                            "Q18", "Q3", "It would take a lot to get me to leave my job.", "Q18",
                            "Q19", "Q4", "I rarely think about leaving my workplace to work somewhere else.", "Q19");

                AddHewitt(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(237, 28, 36), "STRIVE",
                            "Q20", "Q5 \u00B2", "My workplace inspires me to do my best work every day.", "Q20",
                            "Q21", "Q6", "My workplace motivates me to contribute more than is normally required to complete my work.", "Q21");

                //Average row
                worksheet.Row(rowNum).Height = 26.96f;
                AddMergedCell(worksheet, "A" + rowNum, "G" + rowNum, "Engagement Score (Averaged)      ", r =>
                {
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    r.Style.Font.Bold = true;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                });
                string[] hewittCols = new string[] { "Q16", "Q17", "Q18", "Q19", "Q20", "Q21" };
                double hewittEngagementRate = -100;
                for (int c = 0; c < 4; c++)
                {
                    double total = 0;
                    double cnt = 0;
                    foreach (string colName in hewittCols)
                    {
                        string value = null;
                        if (c == 0)
                        {
                            value = currentRow[colName].ToString();
                        }
                        else if (historicalRows.ContainsKey(REPORT_YEAR - c))
                        {
                            value = historicalRows[REPORT_YEAR - c][colName].ToString();
                        }
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            total += value.StringToDbl();
                            cnt++;
                        }
                    }
                    double val = -100;
                    if (cnt > 0)
                    {
                        val = total / cnt;
                    }
                    if (val >= 0)
                    {
                        if (c == 0)
                        {
                            hewittEngagementRate = val;
                        }
                        worksheet.Cells[rowNum, DATA_START_COL + c].Value = val;
                    }
                    else
                    {
                        worksheet.Cells[rowNum, DATA_START_COL + c].Value = "N/A";
                    }
                    using (ExcelRange r = worksheet.Cells[rowNum, DATA_START_COL + c])
                    {
                        r.Style.Font.Bold = true;
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                        r.Style.Numberformat.Format = "0.0%";
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                }

                rowNum += 2;

                //Engagement rate and goals section
                AddMergedCell(worksheet, "A" + rowNum, "E" + rowNum, "Engagement Rates and Goals:       ", r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size += 1;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                });

                AddMergedCell(worksheet, "F" + rowNum, "G" + rowNum, "Engagement Goal", subHeadStyle);
                worksheet.Cells["F" + rowNum].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                AddMergedCell(worksheet, "H" + rowNum, "I" + rowNum, "Engagement Actual", subHeadStyle);
                AddMergedCell(worksheet, "J" + rowNum, "K" + rowNum, "Difference", subHeadStyle);
                worksheet.Cells["K" + rowNum].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                rowNum++;

                style = r =>
                {
                    r.Style.Font.Size -= 1;
                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Numberformat.Format = "0.0%";
                };

                double hewittTarget = currentRow["HewittTarget"].ToString().StringToDbl();
                AddMergedCell(worksheet, "F" + rowNum, "G" + rowNum, hewittTarget, style);
                worksheet.Cells["F" + rowNum].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                if (hewittEngagementRate == -100)
                {
                    AddMergedCell(worksheet, "H" + rowNum, "I" + rowNum, String.Empty, style);
                    AddMergedCell(worksheet, "J" + rowNum, "K" + rowNum, String.Empty, style);
                }
                else
                {
                    AddMergedCell(worksheet, "H" + rowNum, "I" + rowNum, hewittEngagementRate, style);
                    AddMergedCell(worksheet, "J" + rowNum, "K" + rowNum, hewittEngagementRate - hewittTarget, style);
                }
                worksheet.Cells["K" + rowNum].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                rowNum += 2;

                //Part 2 - Great Candian Temperature Check
                worksheet.Row(rowNum - 1).PageBreak = true;
                worksheet.Row(rowNum).Height = 32.21f;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "PART 2: Great Candian Temperature Check - " + locationName, r =>
                {
                    r.Style.Font.Size += 2;
                    r.Style.Font.Bold = true;
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                });

                rowNum += 2;

                //Header row
                style = r =>
                {
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                };
                worksheet.Row(rowNum).Height = 26.96f;

                using (ExcelRange r = worksheet.Cells[rowNum, 1])
                {
                    r.Value = "GC Snapshot";
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size -= 1;
                    r.Style.WrapText = true;
                    style(r);
                }
                using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum, DATA_START_COL - 1])
                {
                    style(r);
                }
                //style( worksheet.Cells[rowNum, 4, rowNum, DATA_START_COL - 1] );
                for (int i = 0; i < 4; i++)
                {
                    using (ExcelRange r = worksheet.Cells[rowNum, DATA_START_COL + i])
                    {
                        r.Value = REPORT_YEAR - i;
                        r.Style.Font.Bold = true;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                        style(r);
                    }
                }

                rowNum++;

                //Data rows

                AddTempCheck(worksheet, ref rowNum, currentRow, historicalRows,
                            "Q12", "My direct supervisor keeps me informed about matters that affect me.\u2078", "Q12");
                AddTempCheck(worksheet, ref rowNum, currentRow, historicalRows,
                            "Q13", "I know who to speak with to have my questions answered.", "Q13");
                AddTempCheck(worksheet, ref rowNum, currentRow, historicalRows,
                            "Q14", "My requests for information or assistance are addressed promptly.", "Q14");
                AddTempCheck(worksheet, ref rowNum, currentRow, historicalRows,
                            "Q15", String.Format("I am happy to be working here at {0}.", String.IsNullOrWhiteSpace(ddlProperty.SelectedValue) ? "my Great Canadian location" : ddlProperty.SelectedItem.Text), "Q15");
                AddTempCheck(worksheet, ref rowNum, currentRow, historicalRows,
                            "Q22", "My management has acted on results from previous surveys.", "Q22");
                AddTempCheck(worksheet, ref rowNum, currentRow, historicalRows,
                            "Q23", "I received the training I need to do my job well.", "Q23");
                AddTempCheck(worksheet, ref rowNum, currentRow, historicalRows,
                            "Q24", "Our GEM Recognition Program acknowledges me and my colleagues in a positive way.", "Q24");
                AddTempCheck(worksheet, ref rowNum, currentRow, historicalRows,
                            "Q25", "I believe in the Company's Values and practice them daily at work.", "Q25");
                AddTempCheck(worksheet, ref rowNum, currentRow, historicalRows,
                            "Q26", "I appreciate the opportunity to have one-on-one discussions with my manager.", "Q26");

                rowNum++; //rowNum++ is called in AddTempCheck, so this gives us an extra blank row

                //New questions from 2014
                worksheet.Row(rowNum).Height = 32.21f;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "New Questions from 2014 Onwards: " + locationName, r =>
                {
                    r.Style.Font.Size += 2;
                    r.Style.Font.Bold = true;
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                });
                rowNum++;

                //Header row
                worksheet.Row(rowNum).Height = 26.96f;

                using (ExcelRange r = worksheet.Cells[rowNum, 1])
                {
                    r.Value = "GC Snapshot";
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size -= 1;
                    r.Style.WrapText = true;
                    style(r);
                }
                using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum, DATA_START_COL + 2 - 1])
                {
                    style(r);
                }
                for (int i = 0; i < 2; i++)
                {
                    using (ExcelRange r = worksheet.Cells[rowNum, DATA_START_COL + 2 + i])
                    {
                        int year = REPORT_YEAR - i;
                        r.Value = (year == 2014 ? year + "\u2076" : year.ToString());
                        r.Style.Font.Bold = true;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                        style(r);
                    }
                }
                rowNum++;

                //Data rows
                style = r =>
                {
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                };
                Action<ExcelRange> questionStyle = r =>
                {
                    r.Style.Font.Size -= 1;
                    r.Style.WrapText = true;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                };

                worksheet.Row(rowNum).Height = 26.96f;
                AddValue(worksheet, rowNum, 1, "Q27", style);
                AddMergedCell(worksheet, "B" + rowNum, "I" + rowNum, "My manager is effective in providing performance feedback and coaching.", questionStyle);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "Q27", CellFormat.Percent, style, 2);
                rowNum++;

                worksheet.Row(rowNum).Height = 26.96f;
                AddValue(worksheet, rowNum, 1, "Q28", style);
                AddMergedCell(worksheet, "B" + rowNum, "I" + rowNum, "We have a respectful workplace that is open, values diversity, and accepts individual differences (e.g. gender, race, ethnicity, sexual orientation, religion, age).", questionStyle);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "Q28", CellFormat.Percent, style, 2);
                rowNum++;

                worksheet.Row(rowNum).Height = 26.96f;
                AddValue(worksheet, rowNum, 1, "Q29A", style);
                AddMergedCell(worksheet, "B" + rowNum, "I" + rowNum, "In our organization, we are: Hiring the people we need to be successful today and in the future.", questionStyle);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "Q29A", CellFormat.Percent, style, 2);
                rowNum++;

                worksheet.Row(rowNum).Height = 26.96f;
                AddValue(worksheet, rowNum, 1, "Q29B", style);
                AddMergedCell(worksheet, "B" + rowNum, "I" + rowNum, "In our organization, we are: Keeping the people we need to be successful today and in the future.", questionStyle);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "Q29B", CellFormat.Percent, style, 2);
                rowNum++;

                worksheet.Row(rowNum).Height = 26.96f;
                AddValue(worksheet, rowNum, 1, "Q29C", style);
                AddMergedCell(worksheet, "B" + rowNum, "I" + rowNum, "In our organization, we are: Promoting the people who are best equipped to help us be successful today and in the future.", questionStyle);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "Q29C", CellFormat.Percent, style, 2);
                rowNum++;

                worksheet.Row(rowNum).Height = 26.96f;
                AddValue(worksheet, rowNum, 1, "Q30", style);
                AddMergedCell(worksheet, "B" + rowNum, "I" + rowNum, "I know what Great Canadian / American stands for and what makes our company different and better than the rest.", questionStyle);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "Q30", CellFormat.Percent, style, 2);

                rowNum += 2;

                //Part 3: Gallup Engagement Model
                worksheet.Row(rowNum - 1).PageBreak = true;
                worksheet.Row(rowNum).Height = 32.21f;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "PART 3: Gallup Engagement Model - The \"Q12\" \u00B3", r =>
                {
                    r.Style.Font.Size += 2;
                    r.Style.Font.Bold = true;
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                });
                rowNum++;

                worksheet.Row(rowNum).Height = 52.46f;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Research by Gallup and others shows that engaged employees are more productive. They are more profitable, more customer-focused, safer, and more likely to withstand temptations to leave. The best-performing companies know that an employee engagement improvement strategy linked to the achievement of corporate goals will help them win in the marketplace.", r =>
                {
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                });
                rowNum++;

                //Camp scores
                //Header
                worksheet.Row(rowNum).Height *= 2;
                AddMergedCell(worksheet, "E" + rowNum, "G" + rowNum, String.Empty, style);
                for (int i = 0; i < 4; i++)
                {
                    using (ExcelRange r = worksheet.Cells[rowNum, DATA_START_COL + i])
                    {
                        int year = REPORT_YEAR - i;
                        r.Value = year + "\u2076";
                        r.Style.Font.Bold = true;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                        style(r);
                    }
                }

                //Camp Rows
                worksheet.Row(++rowNum).Height *= 2;
                AddMergedCell(worksheet, "E" + rowNum, "G" + rowNum, "Base Camp (GC Snapshot Q1/Q2)", style);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "BaseCamp", CellFormat.Percent, style);

                worksheet.Row(++rowNum).Height *= 2;
                AddMergedCell(worksheet, "E" + rowNum, "G" + rowNum, "Camp 1 (GC Snapshot Q20/Q3/Q4/Q5)", r =>
                {
                    style(r);
                    r.Style.Font.Size = 8;
                });
                AddValues(worksheet, rowNum, currentRow, historicalRows, "Camp1", CellFormat.Percent, style);

                worksheet.Row(++rowNum).Height *= 2;
                AddMergedCell(worksheet, "E" + rowNum, "G" + rowNum, "Camp 2 (GC Snapshot Q6/Q7/Q8/Q9)", r =>
                {
                    style(r);
                    r.Style.Font.Size = 8;
                });
                AddValues(worksheet, rowNum, currentRow, historicalRows, "Camp2", CellFormat.Percent, style);

                worksheet.Row(++rowNum).Height *= 2;
                AddMergedCell(worksheet, "E" + rowNum, "G" + rowNum, "Camp 3 (GC Snapshot Q10, Q11)", style);
                AddValues(worksheet, rowNum, currentRow, historicalRows, "Camp3", CellFormat.Percent, style);

                worksheet.Row(++rowNum).Height *= 2;
                AddMergedCell(worksheet, "E" + rowNum, "G" + rowNum, "\"Q12\" Engagement Score", style);
                string[] gallupCols = new string[] { "BaseCamp", "Camp1", "Camp2", "Camp3" };
                double gallupEngagementRate = -100;
                for (int c = 0; c < 4; c++)
                {
                    double total = 0;
                    double cnt = 0;
                    foreach (string colName in gallupCols)
                    {
                        string value = null;
                        if (c == 0)
                        {
                            value = currentRow[colName].ToString();
                        }
                        else if (historicalRows.ContainsKey(REPORT_YEAR - c))
                        {
                            value = historicalRows[REPORT_YEAR - c][colName].ToString();
                        }
                        if (!String.IsNullOrWhiteSpace(value))
                        {
                            total += value.StringToDbl();
                            cnt++;
                        }
                    }
                    double val = -100;
                    if (cnt > 0)
                    {
                        val = total / cnt;
                    }
                    if (val >= 0)
                    {
                        if (c == 0)
                        {
                            gallupEngagementRate = val;
                        }
                        worksheet.Cells[rowNum, DATA_START_COL + c].Value = val;
                    }
                    else
                    {
                        worksheet.Cells[rowNum, DATA_START_COL + c].Value = "N/A";
                    }
                    using (ExcelRange r = worksheet.Cells[rowNum, DATA_START_COL + c])
                    {
                        r.Style.Font.Bold = true;
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        r.Style.Numberformat.Format = "0.0%";
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                }

                //Gallup pyramid image
                ExcelPicture gallupPic = worksheet.Drawings.AddPicture("GallupPyramid", new FileInfo(Server.MapPath(@"~\Images\Snapshot-GallupPyramid.jpg")));
                gallupPic.From.Row = rowNum - 6;
                gallupPic.From.Column = 0;
                gallupPic.To.Row = rowNum - 2;
                gallupPic.To.Column = 3;
                gallupPic.SetSize(36);

                rowNum += 2;

                //Gallup engagement rate and goals section
                AddMergedCell(worksheet, "A" + rowNum, "E" + rowNum, "Engagement Rates and Goals:       ", r =>
                {
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size += 1;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                });

                AddMergedCell(worksheet, "F" + rowNum, "G" + rowNum, "Engagement Goal", subHeadStyle);
                worksheet.Cells["F" + rowNum].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                AddMergedCell(worksheet, "H" + rowNum, "I" + rowNum, "Engagement Actual", subHeadStyle);
                AddMergedCell(worksheet, "J" + rowNum, "K" + rowNum, "Difference", subHeadStyle);
                worksheet.Cells["K" + rowNum].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                rowNum++;

                style = r =>
                {
                    r.Style.Font.Size -= 1;
                    r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Numberformat.Format = "0.0%";
                };

                double gallupTarget = currentRow["GallupTarget"].ToString().StringToDbl();
                AddMergedCell(worksheet, "F" + rowNum, "G" + rowNum, gallupTarget, style);
                worksheet.Cells["F" + rowNum].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                if (gallupEngagementRate == -100)
                {
                    AddMergedCell(worksheet, "H" + rowNum, "I" + rowNum, String.Empty, style);
                    AddMergedCell(worksheet, "J" + rowNum, "K" + rowNum, String.Empty, style);
                }
                else
                {
                    AddMergedCell(worksheet, "H" + rowNum, "I" + rowNum, gallupEngagementRate, style);
                    AddMergedCell(worksheet, "J" + rowNum, "K" + rowNum, gallupEngagementRate - gallupTarget, style);
                }
                worksheet.Cells["K" + rowNum].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                rowNum += 2;

                //Gallup scores

                //Header row
                style = r =>
                {
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                };
                worksheet.Row(rowNum).Height = 26.96f;

                style(worksheet.Cells[rowNum, 1]);
                using (ExcelRange r = worksheet.Cells[rowNum, 2])
                {
                    r.Value = "GC Snapshot";
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size -= 1;
                    r.Style.WrapText = true;
                    style(r);
                }
                using (ExcelRange r = worksheet.Cells[rowNum, 3])
                {
                    r.Value = "Gallup";
                    r.Style.Font.Bold = true;
                    r.Style.Font.Size -= 1;
                    r.Style.WrapText = true;
                    style(r);
                }
                style(worksheet.Cells[rowNum, 4, rowNum, DATA_START_COL - 1]);

                for (int i = 0; i < 4; i++)
                {
                    using (ExcelRange r = worksheet.Cells[rowNum, DATA_START_COL + i])
                    {
                        r.Value = REPORT_YEAR - i;
                        r.Style.Font.Bold = true;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                        style(r);
                    }
                }

                rowNum++;

                //Data rows

                //Base camp
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(249, 205, 170), "Base Camp", "Q1", "Q1", "I know what is expected of me at work.", "Q1");
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(249, 205, 170), "Base Camp", "Q2", "Q2", "I have the materials and equipment to do my job right.", "Q2");
                //Camp 1
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(119, 192, 212), "Camp 1", "Q20", "Q3 \u00B2", "My workplace inspires me to do my best work every day.", "Q20");
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(119, 192, 212), "Camp 1", "Q3", "Q4", "In the last 7 days, I have received recognition or praise for doing good work.", "Q3");
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(119, 192, 212), "Camp 1", "Q4", "Q5 \u2079", "My supervisor or someone at work seems to care about me as a person.", "Q4");
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(119, 192, 212), "Camp 1", "Q5", "Q6", "Someone at work encourages my development.", "Q5");
                //Camp 2
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(191, 178, 207), "Camp 2", "Q6", "Q7", "At work, my opinions seem to count.", "Q6");
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(191, 178, 207), "Camp 2", "Q7", "Q8", "Our Vision and Mission makes me feel that my job is important.", "Q7");
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(191, 178, 207), "Camp 2", "Q8", "Q9", "My co-workers are committed to doing quality work.", "Q8");
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(191, 178, 207), "Camp 2", "Q9", "Q10 \u00B9\u2070", "I have a trusted [best] friend at work.", "Q9");
                //Camp 3
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(181, 203, 136), "Camp 3", "Q10", "Q11", "In the last 12 months, I have received a written Performance Review.", "Q10");
                AddGallup(worksheet, ref rowNum, currentRow, historicalRows, Color.FromArgb(181, 203, 136), "Camp 3", "Q11", "Q12", "This last year, I had opportunities at work to learn and grow.", "Q11");

                //Part 4: The Likert Scale
                worksheet.Row(rowNum - 1).PageBreak = true;
                worksheet.Row(rowNum).Height = 32.21f;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "PART 4: The Likert Scale", r =>
                {
                    r.Style.Font.Size += 2;
                    r.Style.Font.Bold = true;
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                });

                rowNum++;

                worksheet.Row(rowNum).Height *= 3;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "A Likert scale is a psychometric scale (or rating scale) that is commonly used in questionnaires.  When responding to a Likert questionnaire item, respondents specify their level of agreement or disagreement on a symmetric agree-disagree scale for a series of statements. Thus the range captures the intensity of their feelings for a given item.", r =>
                {
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                });

                rowNum++;

                worksheet.Row(rowNum).Height *= 2;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "As of 2010, the Snapshot Survey used by Great Canadian switched from a yes/no model to a six level Likert scale of Strongly Disagree to Strongly Agree with no middle/neutral \"Neither Agree nor Disagree\" level.", r =>
                {
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                });

                rowNum++;

                worksheet.Row(rowNum).Height *= 3;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Great Canadian's methodology is to group the 1 and 2 answers for Strongly Disagree and Disagree as \"Unengaged\"; group 3 and 4 answers of Slightly Disagree and Slightly Agree as \"Somewhat Engaged\"; and group 5 and 6 answers of Agree and Strongly Agree as \"Engaged\".", r =>
                {
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                });

                rowNum += 2;

                //Notes and Sources
                worksheet.Row(rowNum).Height = 32.21f;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Notes and Sources", r =>
                {
                    r.Style.Font.Size += 2;
                    r.Style.Font.Bold = true;
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                });

                rowNum++;

                style = r =>
                {
                    r.Style.Font.Size = 8;
                };

                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Note 1: Source: http://was2.hewitt.com/bestemployers/canada/pages/emp_engagement.htm", style);
                rowNum++;

                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Note 2: Denotes that GC Snapshot Question #20 is reflected in BOTH the Gallup Q3 and Hewitt Q5 engagement results.", style);
                rowNum++;

                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Note 3: Source: http://www.gallup.com/consulting/52/employee-engagement.aspx", style);
                rowNum++;

                worksheet.Row(rowNum).Height = 26.21f;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Note 4: Source: Buckingham, Marcus Curt Coffman: First, Break All the Rules: What the World's Greatest Managers do Differently; Simon Schuster 1999", r =>
                {
                    style(r);
                    r.Style.WrapText = true;
                    r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                });
                rowNum++;

                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Note 5: 2009 survey responses were measured using a closed scale (\"yes\" or \"no\")", style);
                rowNum++;

                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Note 6: 2011/2010 survey responses were measured using a 6 step Likert scale (from \"[1] strongly disagree\" to \"[6] strongly agree\")", style);
                rowNum++;

                worksheet.Row(rowNum).Height = 38.96f;
                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum,
                    "Note 7: In June/July 2008, GCGC participated in the Hewitt Associates' 2009 Best Employers in Canada Study (\"2008 Hewitt Engagement Survey\")" +
                    "\nHewitt uses a proprietary weighted calculation for engagement that differs slightly from an average of the scores.", r =>
                    {
                        style(r);
                        r.Style.WrapText = true;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    });
                rowNum++;

                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Note 8: Previous surveys did not use the word \"direct\"", style);
                rowNum++;

                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Note 9: Gallup does not employ the term \"or someone\". Gallup measures only how \"the supervisor\" cares about their employee.", style);
                rowNum++;

                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Note 10: Gallup uses \"best\" friend rather than \"trusted\" friend.", style);
                rowNum++;

                AddMergedCell(worksheet, "A" + rowNum, "K" + rowNum, "Note 11: Participating locations for Snapshot Surveys:", style);
                rowNum++;

                //Set up the values to build our table
                Dictionary<GCCPropertyShortCode, HashSet<int>> locationSurveyYears = new Dictionary<GCCPropertyShortCode, HashSet<int>>();

               
                /// GCGC
                locationSurveyYears.Add(GCCPropertyShortCode.GCC, new HashSet<int>() {2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010 });
                /// River Rock Casino and Hotel
                locationSurveyYears.Add(GCCPropertyShortCode.RR, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010, 2009, 2008 });
                /// Hard Rock Casino Vancouver
                locationSurveyYears.Add(GCCPropertyShortCode.HRCV, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010, 2009, 2008 });
                /// Fraser Downs
                //locationSurveyYears.Add( GCCPropertyShortCode.FD, new HashSet<int>() { 2015, 2014, 2013, 2012, 2011, 2010, 2009 } );
                /// Hastings Racetrack and Casino
                locationSurveyYears.Add(GCCPropertyShortCode.HA, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010, 2009, 2008 });
                /// Elements Casino Victoria
                locationSurveyYears.Add(GCCPropertyShortCode.ECV, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010, 2009, 2008 });
                /// Casino Nanaimo
                locationSurveyYears.Add(GCCPropertyShortCode.NAN, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010, 2009, 2008 });
                /// Chances - Chilliwack
                locationSurveyYears.Add(GCCPropertyShortCode.CCH, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2012, 2011 });
                /// Chances - Maple Ridge
                locationSurveyYears.Add(GCCPropertyShortCode.CMR, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010, 2009, 2008 });
                /// Chances - Dawson Creek
                locationSurveyYears.Add(GCCPropertyShortCode.CDC, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010, 2009, 2008 });
                /// Casino Nova Scotia - Halifax
                locationSurveyYears.Add(GCCPropertyShortCode.CNSH, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010, 2009, 2008 });
                /// Casino Nova Scotia - Sydney
                locationSurveyYears.Add(GCCPropertyShortCode.CNSS, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2012, 2011, 2010, 2009, 2008 });
                /// Great American Casino
                locationSurveyYears.Add(GCCPropertyShortCode.GAG, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013 });
                /// Flamboro Downs
                locationSurveyYears.Add(GCCPropertyShortCode.FL, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2011 });
                /// Georgian Downs
                locationSurveyYears.Add(GCCPropertyShortCode.GD, new HashSet<int>() { 2017, 2016, 2015, 2014, 2013, 2011 });
                /// Casino New Brunswick
                locationSurveyYears.Add(GCCPropertyShortCode.CNB, new HashSet<int>() { 2017, 2016 });
                /// Thousand Islands
                locationSurveyYears.Add(GCCPropertyShortCode.SCTI, new HashSet<int>() { 2017, 2016 });
                /// Kawartha Downs
                locationSurveyYears.Add(GCCPropertyShortCode.SSKD, new HashSet<int>() { 2017, 2016 });
                /// Shorelines Casino Belleville
                locationSurveyYears.Add(GCCPropertyShortCode.SCBE, new HashSet<int>() { 2017});
                ///BSQ
                locationSurveyYears.Add(GCCPropertyShortCode.BSQ, new HashSet<int>() { 2017 });


                


                //Add the data
                Action<ExcelRange> headerStyle = r =>
                {
                    r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(118, 147, 60));
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.Font.Bold = true;
                };
                //Horizontal Header
                using (ExcelRange r = worksheet.Cells[rowNum, 1, rowNum, REPORT_YEAR - 2008 + 2])
                { //+2 because we're skipping the first row, and because the date range is technically zero to REPORT_YEAR - 2008, so we have to add an extra 1 for the zero.
                    headerStyle(r);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }
                headerStyle(worksheet.Cells[rowNum, 1, rowNum + locationSurveyYears.Keys.Count, 1]);

                Action<ExcelRange, Color> colStyle = (r, c) =>
                {
                    r.Style.Fill.BackgroundColor.SetColor(c);
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                };

                for (int i = 0; i <= REPORT_YEAR - 2008; i++)
                {
                    AddValue(worksheet, rowNum, 2 + i, REPORT_YEAR - i, null);
                }
                rowNum++;

                bool firstRow = true;
                //Loop through the locations for the rows
                foreach (GCCPropertyShortCode sc in locationSurveyYears.Keys.OrderBy(x => { return x.ToString(); }))
                {
                    AddValue(worksheet, rowNum, 1, sc.ToString(), null);
                    //Loop through the
                    for (int i = 0; i <= REPORT_YEAR - 2008; i++)
                    {
                        if (locationSurveyYears[sc].Contains(REPORT_YEAR - i))
                        {
                            AddValue(worksheet, rowNum, 2 + i, "o", null);
                        }
                        //Highlight the columns if this is the first row
                        if (firstRow)
                        {
                            colStyle(worksheet.Cells[rowNum, 2 + i, rowNum + locationSurveyYears.Keys.Count - 1, 2 + i], (i % 2 == 0 ? Color.FromArgb(196, 215, 155) : Color.FromArgb(235, 241, 222)));
                        }
                    }
                    firstRow = false;
                    rowNum++;
                }
                //================================================================================
                // END OF FIRST WORKSHEET
                //================================================================================

                #endregion Report Worksheet

                #region Yearly Comparison

                if (currentRow["SurveysCompleted"].ToString().StringToInt() > 10 || OverrideMinimumCountCheck)
                {
                    p.Workbook.Worksheets.Add("History and Details");
                    worksheet = p.Workbook.Worksheets[2];

                    worksheet.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                    worksheet.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                    //Show page numbers
                    worksheet.HeaderFooter.FirstFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                    worksheet.HeaderFooter.EvenFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                    worksheet.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);

                    //Set the column widths
                    int years = historicalRows.Keys.Count;

                    for (int i = 1; i <= 6 + years; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                worksheet.Column(i).Width = 25.85f;
                                break;

                            case 4:
                                worksheet.Column(i).Width = 2.28f;
                                break;

                            default:
                                worksheet.Column(i).Width = 10f;
                                break;
                        }
                    }

                    rowNum = 3;

                    //Response details
                    using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum, 5 + historicalRows.Count])
                    {
                        r.Value = locationName;
                        r.Merge = true;
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Font.Size = 14;
                        r.Style.WrapText = true;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    worksheet.Row(rowNum).Height *= 1.2f;
                    rowNum++;

                    using (ExcelRange r = worksheet.Cells[rowNum, 1, rowNum + 4, 1])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }
                    style = r =>
                    {
                        r.Style.Font.Bold = true;
                    };

                    AddValue(worksheet, rowNum, 1, "RESPONSE", style);
                    AddValue(worksheet, rowNum + 1, 1, "Response Rate", style);
                    AddValue(worksheet, rowNum + 2, 1, "Total Surveys Completed", style);
                    AddValue(worksheet, rowNum + 3, 1, "Total Employees", style);

                    int[] histKeys = historicalRows.Keys.ToArray();
                    //Sort descending
                    Array.Sort(histKeys,
                        new Comparison<int>(
                                (i1, i2) => i2.CompareTo(i1)
                        ));
                    for (int i = 0; i <= histKeys.Length; i++)
                    {
                        int year = (i == 0 ? REPORT_YEAR : histKeys[i - 1]);
                        AddValue(worksheet, rowNum, 5 + i, year, r =>
                        {
                            r.Style.Font.Bold = true;
                        });
                        double dbl = GetValue(i, currentRow, historicalRows, histKeys, "ResponseRate");
                        if (dbl != -1000)
                        {
                            AddValue(worksheet, rowNum + 1, 5 + i, dbl, r =>
                            {
                                r.Style.Numberformat.Format = "0.0%";
                            });
                        }
                        else
                        {
                            AddValue(worksheet, rowNum + 1, 5 + i, "N/A", null);
                        }
                        dbl = GetValue(i, currentRow, historicalRows, histKeys, "SurveysCompleted");
                        if (dbl != -1000)
                        {
                            AddValue(worksheet, rowNum + 2, 5 + i, dbl, r =>
                            {
                                r.Style.Numberformat.Format = "#,###;-#,###;0";
                            });
                        }
                        else
                        {
                            AddValue(worksheet, rowNum + 2, 5 + i, "N/A", null);
                        }
                        dbl = GetValue(i, currentRow, historicalRows, histKeys, "EmployeeCount");
                        if (dbl != -1000)
                        {
                            AddValue(worksheet, rowNum + 3, 5 + i, dbl, r =>
                            {
                                r.Style.Numberformat.Format = "#,###;-#,###;0";
                            });
                        }
                        else
                        {
                            AddValue(worksheet, rowNum + 3, 5 + i, "N/A", null);
                        }
                    }
                    rowNum += 4;

                    using (ExcelRange r = worksheet.Cells[rowNum - 5, 5 + historicalRows.Count, rowNum - 1, 5 + historicalRows.Count])
                    {
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    }

                    AddValue(worksheet, rowNum, 1, "Response Rate and Goals", r =>
                    {
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    });

                    using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum, 5 + historicalRows.Count])
                    {
                        double rr = currentRow["ResponseRate"].ToString().StringToDbl();
                        double tr = currentRow["TargetResponseRate"].ToString().StringToDbl();
                        double diff = rr - tr;
                        if (diff < 0)
                        {
                            r.IsRichText = true;
                            r.RichText.Add(String.Format("Resp Rate Goal: {0:0%}    Resp Rate Actual: {1:0%}    Difference: ", tr, rr));
                            var rt = r.RichText.Add(String.Format("{0:0%}", diff));
                            rt.Color = Color.Red;
                        }
                        else
                        {
                            r.Value = String.Format("Resp Rate Goal: {0:0%}    Resp Rate Actual: {1:0%}    Difference: {2:0%}", tr, rr, diff);
                        }
                        r.Merge = true;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.Font.Size = 9;
                    }
                    rowNum += 2;

                    style = r =>
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        r.Style.Font.Bold = true;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.WrapText = true;
                        r.Style.Font.Size = 9;
                    };
                    worksheet.Row(rowNum).Height *= 2;
                    AddValue(worksheet, rowNum, 1, String.Empty, style);
                    AddValue(worksheet, rowNum, 2, "Vs. " + (REPORT_YEAR - 1), style);
                    AddValue(worksheet, rowNum, 3, String.Format("Total {0}", REPORT_YEAR), style);
                    AddValue(worksheet, rowNum, 4, String.Empty, style);
                    AddValue(worksheet, rowNum, 5, String.Format("Salary {0}", REPORT_YEAR), style);
                    AddValue(worksheet, rowNum, 6, String.Format("Hourly {0}", REPORT_YEAR), style);

                    int index = 0;
                    foreach (int key in historicalRows.Keys)
                    {
                        index++;
                        AddValue(worksheet, rowNum, 6 + index, key, style);
                    }

                    rowNum++;

                    //Hewitt's questions

                    worksheet.Row(rowNum).Height *= 2f;

                    AddValue(worksheet, rowNum, 1, "Hewitt's Engagement Rates and Goals", r =>
                    {
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        r.Style.WrapText = true;
                        r.Style.Font.Size = 9;
                    });

                    using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum, 6 + years])
                    {
                        double diff = hewittEngagementRate - hewittTarget;
                        if (diff < 0)
                        {
                            r.Style.Font.Color.SetColor(Color.Red);
                            r.IsRichText = true;
                            r.RichText.Add(String.Format("Engagement Goal: {0:0%}    Engagement Actual: {1:0%}    Difference: ", hewittTarget, hewittEngagementRate));
                            var rt = r.RichText.Add(String.Format("{0:0%}", diff));
                            rt.Color = Color.Red;
                        }
                        else
                        {
                            r.Value = String.Format("Engagement Goal: {0:0%}    Engagement Actual: {1:0%}    Difference: {2:0%}", hewittTarget, hewittEngagementRate, diff);
                        }
                        r.Merge = true;
                        r.Style.WrapText = true;
                        r.Style.Font.Size = 9;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    }
                    rowNum++;

                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q16", "I would, without hesitation, recommend my workplace to a friend seeking employment.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q17", "Given the opportunity, I tell others great things about working here.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q18", "It would take a lot to get me to leave my job.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q19", "I rarely think about leaving my workplace to work somewhere else.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q20", "My workplace inspires me to do my best work every day.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q21", "My workplace motivates me to contribute more than is normally required to complete my work.", 4f);

                    using (ExcelRange r = worksheet.Cells[rowNum - 7, 1, rowNum - 1, 6 + years])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    worksheet.Row(rowNum).Height *= 0.5f;

                    rowNum++;

                    //Gallup questions

                    worksheet.Row(rowNum).Height *= 2f;

                    AddValue(worksheet, rowNum, 1, "Gallup's Engagement Rates and Goals", r =>
                    {
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        r.Style.WrapText = true;
                        r.Style.Font.Size = 9;
                    });

                    using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum, 6 + years])
                    {
                        double diff = gallupEngagementRate - gallupTarget;
                        if (diff < 0)
                        {
                            r.Style.Font.Color.SetColor(Color.Red);
                            r.IsRichText = true;
                            r.RichText.Add(String.Format("Engagement Goal: {0:0%}    Engagement Actual: {1:0%}    Difference: ", gallupTarget, gallupEngagementRate));
                            var rt = r.RichText.Add(String.Format("{0:0%}", diff));
                            rt.Color = Color.Red;
                        }
                        else
                        {
                            r.Value = String.Format("Engagement Goal: {0:0%}    Engagement Actual: {1:0%}    Difference: {2:0%}", gallupTarget, gallupEngagementRate, diff);
                        }
                        r.Merge = true;
                        r.Style.WrapText = true;
                        r.Style.Font.Size = 9;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    }
                    rowNum++;

                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q1", "I know what is expected of me at work.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q2", "I have the materials and equipment to do my job right.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q3", "In the last 7 days, I have received recognition or praise for doing good work.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q4", "My supervisor or someone at work seems to care about me as a person.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q5", "Someone at work encourages my development.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q6", "At work, my opinions seem to count.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q7", "Our Vision and Mission makes me feel that my job is important.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q8", "My co-workers are committed to doing quality work.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q9", "I have a trusted [best] friend at work.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q10", "In the last 12 months, I have received a written Performance Review.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q11", "This last year, I had opportunities at work to learn and grow.", 3f);

                    using (ExcelRange r = worksheet.Cells[rowNum - 12, 1, rowNum - 1, 6 + years])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    worksheet.Row(rowNum).Height *= 0.5f;

                    rowNum++;

                    //Temperature Check

                    AddValue(worksheet, rowNum, 1, "GC Temperature Check", r =>
                    {
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        r.Style.WrapText = true;
                        r.Style.Font.Size = 9;
                    });

                    using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum, 6 + years])
                    {
                        r.Merge = true;
                        r.Style.Font.Size = 9;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    rowNum++;

                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q12", "My direct supervisor keeps me informed about matters that affect me.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q13", "I know who to speak with to have my questions answered.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q14", "My requests for information or assistance are addressed promptly.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q15", String.Format("I am happy to be working here at {0}.", String.IsNullOrWhiteSpace(ddlProperty.SelectedValue) ? "my Great Canadian location" : ddlProperty.SelectedItem.Text), 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q22", "My management has acted on results from previous surveys.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q23", "I received the training I need to do my job well.", 2f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q24", "Our GEM Recognition Program acknowledges me and my colleagues in a positive way.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q25", "I believe in the Company's Values and practice them daily at work.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q26", "I appreciate the opportunity to have one-on-one discussions with my manager.", 3f);

                    using (ExcelRange r = worksheet.Cells[rowNum - 10, 1, rowNum - 1, 6 + years])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    worksheet.Row(rowNum).Height *= 0.5f;

                    rowNum++;

                    //New questions

                    AddValue(worksheet, rowNum, 1, "New Questions from 2014 Onwards", r =>
                    {
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        r.Style.WrapText = true;
                        r.Style.Font.Size = 9;
                    });

                    using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum, 6 + years])
                    {
                        r.Merge = true;
                        r.Style.Font.Size = 9;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    rowNum++;

                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q27", "My manager is effective in providing performance feedback and coaching.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q28", "We have a respectful workplace that is open, values diversity, and accepts individual differences (e.g. gender, race, ethnicity, sexual orientation, religion, age).", 6f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q29A", "In our organization, we are: Hiring the people we need to be successful today and in the future.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q29B", "In our organization, we are: Keeping the people we need to be successful today and in the future.", 3f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q29C", "In our organization, we are: Promoting the people who are best equipped to help us be successful today and in the future.", 4f);
                    AddYearlyComparisonRow(worksheet, ref rowNum, currentRow, salaryRow, hourlyRow, historicalRows, "Q30", "I know what Great Canadian [American] stands for and what makes our company different and better than the rest.", 4f);

                    using (ExcelRange r = worksheet.Cells[rowNum - 7, 1, rowNum - 1, 6 + years])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    worksheet.Row(rowNum).Height *= 0.5f;
                }
                //================================================================================
                // END OF SECOND WORKSHEET
                //================================================================================

                #endregion Yearly Comparison

                #region Current Comparison

                if (true)
                {
                    if (!String.IsNullOrWhiteSpace(ddlProperty.SelectedValue))
                    {
                        p.Workbook.Worksheets.Add("Property & Dept. Comparison");
                    }
                    else if (!String.IsNullOrWhiteSpace(ddlRegion.SelectedValue))
                    {
                        p.Workbook.Worksheets.Add("Region & Location Comparison");
                    }
                    else
                    {
                        p.Workbook.Worksheets.Add("GCC, Region & Prop. Comparison");
                    }
                    worksheet = p.Workbook.Worksheets[p.Workbook.Worksheets.Count];

                    worksheet.Cells.Style.Font.Size = 10; //Default font size for whole sheet
                    worksheet.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                    //Show page numbers
                    worksheet.HeaderFooter.FirstFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                    worksheet.HeaderFooter.EvenFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                    worksheet.HeaderFooter.OddFooter.RightAlignedText = String.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);

                    DataTable comparisonTable = ds.Tables[2];

                    //Create a list of separator row numbers so we can get rid of the extra borders later
                    List<int> separatorRows = new List<int>();

                    //Set the column widths
                    int sectionCount = comparisonTable.Rows.Count;

                    rowNum = 1;

                    AddMergedCell(worksheet, rowNum, 1, rowNum, 1 + sectionCount, String.Format("{0} Snapshot Survey - {1} {2}", REPORT_YEAR, locationName, worksheet.Name), r =>
                    {
                        r.Style.Font.Size = 16;
                        r.Style.Font.Bold = true;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    });

                    rowNum++;

                    int firstPropCol = -1;
                    int firstRegCol = -1;
                    worksheet.Column(1).Width = 25.85;

                    worksheet.Cells[rowNum, 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    for (int i = 0; i < comparisonTable.Rows.Count; i++)
                    {
                        int propertyID = comparisonTable.Rows[i]["PropertyID"].ToString().StringToInt(-1);
                        if (propertyID != -1 && firstPropCol == -1)
                        {
                            firstPropCol = 2 + i;
                        }
                        string region = comparisonTable.Rows[i]["Region"].ToString();
                        if (!region.Equals("Division") && firstRegCol == -1)
                        {
                            firstRegCol = 2 + i;
                        }
                        string tmpLabelCol = labelColumn;
                        if (isOverall)
                        {
                            if (isSurveillanceReport)
                            {
                                tmpLabelCol = "Location";
                            }
                            else
                            {
                                if (region.Equals("Division"))
                                {
                                    tmpLabelCol = "Division";
                                }
                                else if (propertyID == -1)
                                {
                                    tmpLabelCol = "Region";
                                }
                                else
                                {
                                    tmpLabelCol = "Location";
                                }
                            }
                        }
                        AddValue(worksheet, rowNum, 2 + i, comparisonTable.Rows[i][tmpLabelCol].ToString(), r =>
                        {
                            r.Style.Font.Bold = true;
                            r.Style.Font.Size = 11;
                            r.Style.WrapText = true;
                            r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Bottom;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            //If it's the last column, add the border
                            if (2 + i == 1 + sectionCount)
                            {
                                r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            }
                        });
                        worksheet.Column(2 + i).Width = 12.85;
                    }
                    worksheet.Row(rowNum).Height *= 3;

                    worksheet.View.FreezePanes(3, 3);

                    rowNum++;

                    Action<ExcelRange> grayHeaders = r =>
                    {
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        r.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.WrapText = true;
                    };
                    AddValue(worksheet, rowNum, 1, "Response Rates and Goals: Target/Actual = Difference", r =>
                    {
                        grayHeaders(r);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    });
                    for (int i = 0; i < comparisonTable.Rows.Count; i++)
                    {
                        DataRow dr = comparisonTable.Rows[i];
                        double rr = dr["ResponseRate"].ToString().StringToDbl();
                        double tr = dr["TargetResponseRate"].ToString().StringToDbl();
                        double diff = rr - tr;

                        AddValue(worksheet, rowNum, 2 + i, String.Empty, r =>
                        {
                            grayHeaders(r);
                            if (diff < 0)
                            {
                                r.IsRichText = true;
                                r.RichText.Add(String.Format("{0:0%}/{1:0%} ={2}", tr, rr, Environment.NewLine));
                                var rt = r.RichText.Add(String.Format("{0:0%}", diff));
                                rt.Color = Color.Red;
                            }
                            else
                            {
                                r.Value = String.Format("{0:0%}/{1:0%} ={3}{2:0%}", tr, rr, diff, Environment.NewLine);
                            }
                            //If it's the last column, add the border
                            if (2 + i == 1 + sectionCount)
                            {
                                r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                            }
                        });
                        AddValue(worksheet, rowNum + 1, 2 + i, "Total " + REPORT_YEAR, r =>
                        {
                            r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(204, 255, 204));
                            r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            r.Style.Font.Bold = true;
                        });
                        worksheet.Column(2 + i).Width = 11.85;
                    }
                    worksheet.Row(rowNum).Height *= 2;
                    worksheet.Row(rowNum + 1).Height *= 2;
                    rowNum += 2;

                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1, CellFormat.Percent, "ResponseRate", "Response Rates", true);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1, CellFormat.Number, "SurveysCompleted", "Total Surveys Completed", true);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1, CellFormat.Number, "EmployeeCount", "Total Employees", true);

                    //Section border
                    using (ExcelRange r = worksheet.Cells[rowNum - 4, 1, rowNum - 1, 1 + sectionCount])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    //Break
                    separatorRows.Add(rowNum);
                    rowNum++;

                    //Hewitt scores

                    AddValue(worksheet, rowNum, 1, "Hewitt's Engagement Rates and Goals: Target/Actual = Difference", r =>
                    {
                        grayHeaders(r);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    });
                    for (int i = 0; i < comparisonTable.Rows.Count; i++)
                    {
                        if (!OverrideMinimumCountCheck)
                        {
                            int count = comparisonTable.Rows[i]["SurveysCompleted"].ToString().StringToInt();
                            if (count < 10)
                            {
                                AddValue<string>(worksheet, rowNum, 2 + i, null, grayHeaders);
                                continue;
                            }
                        }
                        DataRow dr = comparisonTable.Rows[i];
                        double hs = dr["HewittScore"].ToString().StringToDbl();
                        double tr = dr["HewittTarget"].ToString().StringToDbl();
                        double diff = hs - tr;

                        AddValue(worksheet, rowNum, 2 + i, String.Empty, r =>
                        {
                            grayHeaders(r);
                            if (diff < 0)
                            {
                                r.IsRichText = true;
                                r.RichText.Add(String.Format("{0:0%}/{1:0%} ={2}", tr, hs, Environment.NewLine));
                                var rt = r.RichText.Add(String.Format("{0:0%}", diff));
                                rt.Color = Color.Red;
                            }
                            else
                            {
                                r.Value = String.Format("{0:0%}/{1:0%} ={3}{2:0%}", tr, hs, diff, Environment.NewLine);
                            }
                        });
                    }
                    worksheet.Row(rowNum).Height *= 3;
                    rowNum++;

                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q16", "I would, without hesitation, recommend my workplace to a friend seeking employment.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q17", "Given the opportunity, I tell others great things about working here.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q18", "It would take a lot to get me to leave my job.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q19", "I rarely think about leaving my workplace to work somewhere else.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q20", "My workplace inspires me to do my best work every day.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 4f, CellFormat.Percent, "Q21", "My workplace motivates me to contribute more than is normally required to complete my work.", false);

                    //Section border
                    using (ExcelRange r = worksheet.Cells[rowNum - 7, 1, rowNum - 1, 1 + sectionCount])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    //Break
                    separatorRows.Add(rowNum);
                    rowNum++;

                    //Gallup scores

                    AddValue(worksheet, rowNum, 1, "Gallup's Engagement Rates and Goals: Target/Actual = Difference", r =>
                    {
                        grayHeaders(r);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    });
                    for (int i = 0; i < comparisonTable.Rows.Count; i++)
                    {
                        if (!OverrideMinimumCountCheck)
                        {
                            int count = comparisonTable.Rows[i]["SurveysCompleted"].ToString().StringToInt();
                            if (count < 10)
                            {
                                AddValue<string>(worksheet, rowNum, 2 + i, null, grayHeaders);
                                continue;
                            }
                        }
                        DataRow dr = comparisonTable.Rows[i];
                        double gs = dr["GallupScore"].ToString().StringToDbl();
                        double tr = dr["GallupTarget"].ToString().StringToDbl();
                        double diff = gs - tr;

                        AddValue(worksheet, rowNum, 2 + i, String.Empty, r =>
                        {
                            grayHeaders(r);
                            if (diff < 0)
                            {
                                r.IsRichText = true;
                                r.RichText.Add(String.Format("{0:0%}/{1:0%} ={2}", tr, gs, Environment.NewLine));
                                var rt = r.RichText.Add(String.Format("{0:0%}", diff));
                                rt.Color = Color.Red;
                            }
                            else
                            {
                                r.Value = String.Format("{0:0%}/{1:0%} ={3}{2:0%}", tr, gs, diff, Environment.NewLine);
                            }
                        });
                    }
                    worksheet.Row(rowNum).Height *= 3;
                    rowNum++;

                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q1", "I know what is expected of me at work.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q2", "I have the materials and equipment to do my job right.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q3", "In the last 7 days, I have received recognition or praise for doing good work.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q4", "My supervisor or someone at work seems to care about me as a person.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q5", "Someone at work encourages my development.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q6", "At work, my opinions seem to count.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q7", "Our Vision and Mission makes me feel that my job is important.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q8", "My co-workers are committed to doing quality work.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q9", "I have a trusted [best] friend at work.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q10", "In the last 12 months, I have received a written Performance Review.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q11", "This last year, I had opportunities at work to learn and grow.", false);

                    //Section border
                    using (ExcelRange r = worksheet.Cells[rowNum - 12, 1, rowNum - 1, 1 + sectionCount])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    //Break
                    separatorRows.Add(rowNum);
                    rowNum++;

                    //GC Temperature Check

                    AddMergedCell(worksheet, rowNum, 1, rowNum, 1 + sectionCount, "GC Temperature Check", r =>
                    {
                        grayHeaders(r);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    });
                    rowNum++;
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q12", "My direct supervisor keeps me informed about matters that affect me.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q13", "I know who to speak with to have my questions answered.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q14", "My requests for information or assistance are addressed promptly.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q15", String.Format("I am happy to be working here at {0}.", String.IsNullOrWhiteSpace(ddlProperty.SelectedValue) ? "my Great Canadian location" : ddlProperty.SelectedItem.Text), false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q22", "My management has acted on results from previous surveys.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "Q23", "I received the training I need to do my job well.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q24", "Our GEM Recognition Program acknowledges me and my colleagues in a positive way.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q25", "I believe in the Company's Values and practice them daily at work.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q26", "I appreciate the opportunity to have one-on-one discussions with my manager.", false);

                    //Section border
                    using (ExcelRange r = worksheet.Cells[rowNum - 10, 1, rowNum - 1, 1 + sectionCount])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    //Break
                    separatorRows.Add(rowNum);
                    rowNum++;

                    //New questions post 2014

                    AddMergedCell(worksheet, rowNum, 1, rowNum, 1 + sectionCount, "New Questions from 2014 Onwards", r =>
                    {
                        grayHeaders(r);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    });
                    rowNum++;
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q27", "My manager is effective in providing performance feedback and coaching.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 6f, CellFormat.Percent, "Q28", "We have a respectful workplace that is open, values diversity, and accepts individual differences (e.g. gender, race, ethnicity, sexual orientation, religion, age).", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q29A", "In our organization, we are: Hiring the people we need to be successful today and in the future.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "Q29B", "In our organization, we are: Keeping the people we need to be successful today and in the future.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 4f, CellFormat.Percent, "Q29C", "In our organization, we are: Promoting the people who are best equipped to help us be successful today and in the future.", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 4f, CellFormat.Percent, "Q30", "I know what Great Canadian [American] stands for and what makes our company different and better than the rest.", false);

                    //Section border
                    using (ExcelRange r = worksheet.Cells[rowNum - 7, 1, rowNum - 1, 1 + sectionCount])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    //Break
                    separatorRows.Add(rowNum);
                    rowNum++;

                    //CSR questions

                    AddMergedCell(worksheet, rowNum, 1, rowNum, 1 + sectionCount, "CSR Questions", r =>
                    {
                        grayHeaders(r);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    });
                    rowNum++;
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "CSR_Q3", "I would describe our Company's commitment to community outreach and support as", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 3f, CellFormat.Percent, "CSR_Q6", "I would describe Great Canadian's commitment to Responsible Gaming as", false);

                    //Section border
                    using (ExcelRange r = worksheet.Cells[rowNum - 3, 1, rowNum - 1, 1 + sectionCount])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    //Break
                    separatorRows.Add(rowNum);
                    rowNum++;

                    //CSR questions

                    AddMergedCell(worksheet, rowNum, 1, rowNum, 1 + sectionCount, "On a scale of Very Good to Very Poor, how would you rate your understanding of:", r =>
                    {
                        grayHeaders(r);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    });
                    rowNum++;

                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1f, CellFormat.Percent, "CSR_Q9A", "Risks of gaming", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1f, CellFormat.Percent, "CSR_Q9B", "Signs of gaming problem", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1f, CellFormat.Percent, "CSR_Q9C", "Chances of winning and losing", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1f, CellFormat.Percent, "CSR_Q9D", "Tips for safer gaming", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "CSR_Q9E", "Randomness and house advantage", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1f, CellFormat.Percent, "CSR_Q9F", "Gaming myths", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1f, CellFormat.Percent, "CSR_Q9G", "Responsible gaming tools", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1f, CellFormat.Percent, "CSR_Q9H", "Available help resources", false);

                    //Section border
                    using (ExcelRange r = worksheet.Cells[rowNum - 9, 1, rowNum - 1, 1 + sectionCount])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    //Break
                    separatorRows.Add(rowNum);
                    rowNum++;

                    //CSR questions

                    AddMergedCell(worksheet, rowNum, 1, rowNum, 1 + sectionCount, "How comfortable are you in responding to a guest/player who:", r =>
                    {
                        grayHeaders(r);
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    });
                    rowNum++;

                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "CSR_Q10A", "Expresses concerns about their gambling", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 1f, CellFormat.Percent, "CSR_Q10B", "Wants to self-exclude", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "CSR_Q10C", "Is angry or upset about how much they lost", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "CSR_Q10D", "Is concerned about a friend or a family member's gambling", false);
                    AddComparisonRow(worksheet, ref rowNum, comparisonTable, 2f, CellFormat.Percent, "CSR_Q10E", "Expresses their belief in a common gambling myth", false);

                    //Section border
                    using (ExcelRange r = worksheet.Cells[rowNum - 6, 1, rowNum - 1, 1 + sectionCount])
                    {
                        r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                    }

                    //Add overall border
                    using (ExcelRange r = worksheet.Cells[1, 2, rowNum - 1, 2])
                    {
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    }

                    //Add division border
                    if (isOverall && firstRegCol != -1)
                    {
                        using (ExcelRange r = worksheet.Cells[1, firstRegCol, rowNum - 1, firstRegCol])
                        {
                            r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                        }
                    }

                    //Add region border
                    using (ExcelRange r = worksheet.Cells[1, firstPropCol, rowNum - 1, firstPropCol])
                    {
                        r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    }

                    //Remove right & left borders from separator rows
                    foreach (int rw in separatorRows)
                    {
                        using (ExcelRange r = worksheet.Cells[rw, 1, rw, 1 + sectionCount])
                        {
                            r.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                            r.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.None;
                        }
                    }
                }

                #endregion Current Comparison

                #region Raw Data

                if (OverrideMinimumCountCheck)
                {
                    #region Raw Data Sheet

                    p.Workbook.Worksheets.Add("Raw Data");
                    worksheet = p.Workbook.Worksheets[p.Workbook.Worksheets.Count];

                    worksheet.Cells.Style.Font.Size = 10; //Default font size for whole sheet
                    worksheet.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                    DataTable rawDataTable = ds.Tables[3];

                    int sectionCount = rawDataTable.Rows.Count;

                    int colNum = 1;
                    rowNum = 1;
                    foreach (DataColumn col in rawDataTable.Columns)
                    {
                        worksheet.Cells[rowNum, colNum].Value = col.ColumnName;
                        colNum++;
                    }
                    rowNum++;

                    foreach (DataRow dr in rawDataTable.Rows)
                    {
                        colNum = 1;
                        foreach (DataColumn col in rawDataTable.Columns)
                        {
                            worksheet.Cells[rowNum, colNum].Value = dr[col.ColumnName];
                            colNum++;
                        }
                        rowNum++;
                    }

                    


                    #endregion Raw Data Sheet

                    #region Column Detail Sheet

                    p.Workbook.Worksheets.Add("Column Specs");
                    worksheet = p.Workbook.Worksheets[p.Workbook.Worksheets.Count];

                    worksheet.Cells.Style.Font.Size = 10; //Default font size for whole sheet
                    worksheet.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                    style = r =>
                    {
                        r.Style.Font.Bold = true;
                    };

                    rowNum = 1;

                    AddValue(worksheet, rowNum, 1, "Column", style);
                    AddValue(worksheet, rowNum, 2, "Question Text / Details", style);
                    AddValue(worksheet, rowNum++, 3, "Value Details", style);

                    AddInfoRow(worksheet, rowNum++, "Q1", "I know what is expected of me at work.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q2", "I have the materials and equipment to do my job right.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q3", "In the last 7 days, I have received recognition or praise for doing good work.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q4", "My supervisor or someone at work seems to care about me as a person.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q5", "Someone at work encourages my development.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q6", "At work, my opinions seem to count.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q7", "Our Vision and Mission makes me feel that my job is important.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q8", "My co-workers are committed to doing quality work.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q9", "I have a trusted [best] friend at work.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q10", "In the last 12 months, I have received a written Performance Review.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q11", "This last year, I had opportunities at work to learn and grow.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q12", "My direct supervisor keeps me informed about matters that affect me.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q13", "I know who to speak with to have my questions answered.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q14", "My requests for information or assistance are addressed promptly.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q15", String.Format("I am happy to be working here at {0}.", String.IsNullOrWhiteSpace(ddlProperty.SelectedValue) ? "my Great Canadian location" : ddlProperty.SelectedItem.Text), "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q16", "I would, without hesitation, recommend my workplace to a friend seeking employment.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q17", "Given the opportunity, I tell others great things about working here.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q18", "It would take a lot to get me to leave my job.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q19", "I rarely think about leaving my workplace to work somewhere else.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q20", "My workplace inspires me to do my best work every day.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q21", "My workplace motivates me to contribute more than is normally required to complete my work.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q22", "My management has acted on results from previous surveys.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q23", "I received the training I need to do my job well.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q24", "Our GEM Recognition Program acknowledges me and my colleagues in a positive way.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q25", "I believe in the Company's Values and practice them daily at work.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q26", "I appreciate the opportunity to have one-on-one discussions with my manager.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q27", "My manager is effective in providing performance feedback and coaching.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q28", "We have a respectful workplace that is open, values diversity, and accepts individual differences (e.g. gender, race, ethnicity, sexual orientation, religion, age).", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "", "In our organization, we are: ", "");
                    AddInfoRow(worksheet, rowNum++, "Q29A", "Hiring the people we need to be successful today and in the future.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q29B", "Keeping the people we need to be successful today and in the future.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q29C", "Promoting the people who are best equipped to help us be successful today and in the future.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q30", "I know what Great Canadian / American stands for and what makes our company different and better than the rest.", "Scale value 0-6. 0 = N/A, 1 = Lowest / Negative, 6 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "Q31", "I am:", "Hourly, Salary");
                    AddInfoRow(worksheet, rowNum++, "Q32", "My length of service is:", "0-1 year, 1-3 years, 3-5 years, 5-9 years, 10 years+");
                    AddInfoRow(worksheet, rowNum++, "Q33", "In your own words... please take this opportunity to tell us a bit more about your experience at work and provide your comments (either positive or not so positive) that will help to improve your experience at work.", "Open-ended text field.");
                    AddInfoRow(worksheet, rowNum++, "", "To help us group similar comments together, please select the topic area(s) that best fit the comments you made: (Please select all that apply)", "");
                    AddInfoRow(worksheet, rowNum++, "Q34_WorkEnvironment", "Your Work Environment", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_PeopleYouWorkWith", "The People You Work With", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_YourManager", "Your Manager", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_Leadership", "Leadership", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_WorkProcessesResources", "Work Processes/Resources", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_CorporateSocialResponsibility", "Corporate Social Responsibility", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_ManagingPerformance", "Managing Performance", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_Benefits", "Benefits", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_WorkLifeBalance", "Work/Life Balance", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_CareerDevelopmentOpportunities", "Career and Development Opportunities", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_PayRecognition", "Pay/Recognition", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "Q34_EmployerInGeneral", "Your Employer in General", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q1", "I am aware of the PROUD program?", "1 = Yes, 0 = No, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q2", "I am aware that I can apply for financial support for my charity through the \"We're Proud\" Volunteer Recognition Program and for my volunteering efforts?", "1 = Yes, 0 = No, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q3", "I would describe our Company's commitment to community outreach and support as:", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q4", "The \"We're Proud\" Volunteer Recognition Program is:", "Scale value 0-3. 0 = N/A, 1 = Lowest / Negative, 3 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "", "I am aware of the following PROUD programs: (Please check all that apply)", "");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q5_Challenge", "PROUD Challenge", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q5_Champion", "PROUD Champion", "2 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q5_DayOfCaring", "PROUD Day of Caring", "3 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q5_Scholarship", "PROUD Scholarship", "4 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q6", "I would describe Great Canadian's commitment to Responsible Gaming as:", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q7", "How relevant /important is Responsible Gaming to your particular role/position in the organization?", "Scale value 0-3. 0 = N/A, 1 = Lowest / Negative, 3 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "", "I have received information about Responsible Gaming at Great Canadian through: (Please check all that apply)", "");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q8_Training", "Training programs", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q8_StaffMeetings", "Staff meetings", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q8_Newsletters", "Newsletters and memos", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q8_Email", "Email", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q8_Intranet", "Intranet", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q8_PostersAndBrochures", "Posters and/or brochures", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q8_ResponsibleGamingKiosk", "On-site responsible gaming kiosk", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q8_PublicAdvertising", "Public advertising materials (TV commercials, newspaper ads, etc.)", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q8_Other", "Other – Please specify", "1 = Checked, 0 = Unchecked, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q8_OtherExplanation", "", "Open-ended text field.");
                    AddInfoRow(worksheet, rowNum++, "", "On a scale of Very Good to Very Poor, how would you rate your understanding of:", "");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q9A", "Risks of gaming", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q9B", "Signs of gaming problem", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q9C", "Chances of winning and losing", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q9D", "Tips for safer gaming", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q9E", "Randomness and house advantage", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q9F", "Gaming myths", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q9G", "Responsible gaming tools", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q9H", "Available help resources", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "", "How comfortable are you in responding to a guest/player who:", "");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q10A", "Expresses concerns about their gambling", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q10B", "Wants to self-exclude", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q10C", "Is angry or upset about how much they lost", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q10D", "Is concerned about a friend or a family member's gambling", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q10E", "Expresses their belief in a common gambling myth", "Scale value 0-5. 0 = N/A, 1 = Lowest / Negative, 5 = Highest / Positive");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q11", "How informed do you feel about responsible gaming at GCGC?", "Very, Moderately, Somewhat, Not at all");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q12", "Are you aware of the RG Check Accreditation process?", "1 = Yes, 0 = No, -1 = N/A, Blank if unanswered");
                    AddInfoRow(worksheet, rowNum++, "CSR_Q13", "What would be helpful to enhance your knowledge of responsible gaming?   What areas of responsible gaming would you like to learn more about?", "Open-ended text field.");

                    worksheet.Column(1).AutoFit();
                    worksheet.Column(2).Width = 93.5f;
                    worksheet.Column(2).Style.WrapText = true;
                    worksheet.Column(3).AutoFit();

                    #endregion Column Detail Sheet
                }

                #endregion Raw Data

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", String.Format("attachment;    filename={0}.xlsx", fileName));
                p.SaveAs(Response.OutputStream);
                worksheet.Dispose();
                Response.End();
            }
        }

        #region Data Load Helpers

        private void AddInfoRow(ExcelWorksheet worksheet, int rowNum, string colName, string description, string dataType)
        {
            AddValue(worksheet, rowNum, 1, colName, null);
            AddValue(worksheet, rowNum, 2, description, null);
            AddValue(worksheet, rowNum, 3, dataType, null);
        }

        private void AddComparisonRow(ExcelWorksheet worksheet, ref int rowNum, DataTable dataTable, float rowHeightMultiplier, CellFormat cellFormat, string colName, string label, bool ignoreCountCheck)
        {
            AddValue(worksheet, rowNum, 1, label, r =>
            {
                r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                r.Style.Font.Size = 10;
                r.Style.WrapText = true;
            });
            Action<ExcelRange> cellStyle = r =>
            {
                switch (cellFormat)
                {
                    case CellFormat.Number:
                        r.Style.Numberformat.Format = "#,###;-#,###;0";
                        break;

                    case CellFormat.Percent:
                        r.Style.Numberformat.Format = "0.0%";
                        break;
                }
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(204, 255, 204));
                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            };
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (!ignoreCountCheck && !OverrideMinimumCountCheck)
                {
                    int count = dataTable.Rows[i]["SurveysCompleted"].ToString().StringToInt();
                    if (count < 10)
                    {
                        AddValue<string>(worksheet, rowNum, 2 + i, null, cellStyle);
                        continue;
                    }
                }
                double val = dataTable.Rows[i][colName].ToString().StringToDbl(-1000);
                if (val != -1000)
                {
                    AddValue(worksheet, rowNum, 2 + i, val, cellStyle);
                }
                else
                {
                    AddValue<string>(worksheet, rowNum, 2 + i, "N/A", cellStyle);
                }
            }
            worksheet.Row(rowNum).Height *= rowHeightMultiplier;
            rowNum++;
        }

        private void AddYearlyComparisonRow(ExcelWorksheet worksheet, ref int rowNum, DataRow currentRow, DataRow salaryRow, DataRow hourlyRow, Dictionary<int, DataRow> historicalRows, string colName, string label, float rowHeightMultiplier)
        {
            Action<ExcelRange> style = r =>
            {
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                r.Style.Font.Size = 10;
                r.Style.Numberformat.Format = "0.0%";
                r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                r.Style.WrapText = true;
            };

            //First Col
            using (ExcelRange r = worksheet.Cells[rowNum, 1])
            {
                r.Value = label;
                style(r);
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            }

            //Comparison Col
            double compareVal = -1000;
            int compareYear = REPORT_YEAR - 1;
            double curVal = currentRow[colName].ToString().StringToDbl();
            if (historicalRows.ContainsKey(compareYear))
            {
                double histVal = historicalRows[compareYear][colName].ToString().StringToDbl();
                compareVal = curVal - histVal;
            }
            if (compareVal != -1000)
            {
                AddValue(worksheet, rowNum, 2, compareVal, style);
            }
            else
            {
                AddValue<string>(worksheet, rowNum, 2, "N/A", style);
            }

            //Current Value Overall
            AddValue(worksheet, rowNum, 3, curVal, r =>
            {
                style(r);
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(204, 255, 204));
            });

            //Empty col
            AddValue<string>(worksheet, rowNum, 4, null, r =>
            {
                style(r);
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 204));
            });

            //Salary
            Action<ExcelRange> salHrStyle = r =>
            {
                style(r);
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(228, 223, 236));
            };
            double salVal = salaryRow[colName].ToString().StringToDbl(-1000);
            if (salVal != -1000)
            {
                AddValue(worksheet, rowNum, 5, salVal, salHrStyle);
            }
            else
            {
                AddValue<string>(worksheet, rowNum, 5, "N/A", salHrStyle);
            }

            //Hourly
            double hrlyVal = hourlyRow[colName].ToString().StringToDbl(-1000);
            if (hrlyVal != -1000)
            {
                AddValue(worksheet, rowNum, 6, hrlyVal, salHrStyle);
            }
            else
            {
                AddValue<string>(worksheet, rowNum, 6, "N/A", salHrStyle);
            }

            //Historical cols
            int index = 0;
            foreach (int key2 in historicalRows.Keys.OrderBy(x => -x))
            {
                index++;
                double val = historicalRows[key2][colName].ToString().StringToDbl(-1000);
                if (val != -1000)
                {
                    AddValue(worksheet, rowNum, 6 + index, val, style);
                }
                else
                {
                    AddValue<string>(worksheet, rowNum, 6 + index, "N/A", style);
                }
            }

            worksheet.Row(rowNum).Height *= rowHeightMultiplier;
            rowNum++;
        }

        /// <summary>
        /// Gets values used on the second report sheet.
        /// </summary>
        private double GetValue(int index, DataRow currentRow, Dictionary<int, DataRow> historicalRows, int[] histKeys, string columnName)
        {
            double value = -1000;
            if (index == 0)
            {
                value = currentRow[columnName].ToString().StringToDbl(-1000);
            }
            else
            {
                if (index <= histKeys.Length)
                {
                    value = historicalRows[histKeys[index - 1]][columnName].ToString().StringToDbl(-1000);
                }
            }
            return value;
        }

        /// <summary>
        /// Adds score values for the first report sheet.
        /// </summary>
        private void AddValues(ExcelWorksheet worksheet, int rowNum, DataRow dataRow, Dictionary<int, DataRow> historicalRows, string colName, CellFormat cellFormat, Action<ExcelRange> action)
        {
            AddValues(worksheet, rowNum, dataRow, historicalRows, colName, cellFormat, action, 4);
        }

        /// <summary>
        /// Adds score values for the first report sheet.
        /// </summary>
        private void AddValues(ExcelWorksheet worksheet, int rowNum, DataRow dataRow, Dictionary<int, DataRow> historicalRows, string colName, CellFormat cellFormat, Action<ExcelRange> action, int dataColCount)
        {
            for (int c = 0; c < dataColCount; c++)
            {
                using (ExcelRange r = worksheet.Cells[rowNum, DATA_START_COL + c + (4 - dataColCount)])
                {
                    string value = null;
                    if (c == 0)
                    {
                        value = dataRow[colName].ToString();
                    }
                    else if (historicalRows.ContainsKey(REPORT_YEAR - c))
                    {
                        value = historicalRows[REPORT_YEAR - c][colName].ToString();
                    }
                    if (!String.IsNullOrWhiteSpace(value))
                    {
                        r.Value = value.StringToDbl();
                        switch (cellFormat)
                        {
                            case CellFormat.Number:
                                r.Style.Numberformat.Format = "#,###;-#,###;0";
                                break;

                            case CellFormat.Percent:
                                r.Style.Numberformat.Format = "0.0%";
                                break;
                        }
                    }
                    else
                    {
                        r.Value = "N/A";
                    }
                    r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    if (action != null)
                    {
                        action(r);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a value to a worksheet based on the row and col.
        /// </summary>
        private void AddValue<T>(ExcelWorksheet worksheet, int rowNum, int colNum, T value, Action<ExcelRange> action)
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

        /// <summary>
        /// Adds a value to a worksheet and merges the cells together.
        /// </summary>
        private void AddMergedCell<T>(ExcelWorksheet worksheet, string startCell, string endCell, T value)
        {
            AddMergedCell(worksheet, startCell, endCell, value, null);
        }

        /// <summary>
        /// Adds a value to a worksheet and merges the cells together.
        /// </summary>
        private void AddMergedCell<T>(ExcelWorksheet worksheet, string startCell, string endCell, T value, Action<ExcelRange> action)
        {
            worksheet.Cells[startCell].Value = value;
            using (ExcelRange r = worksheet.Cells[startCell + ":" + endCell])
            {
                r.Merge = true;
                if (action != null)
                {
                    action(r);
                }
            }
        }

        /// <summary>
        /// Adds a value to a worksheet and merges the cells together.
        /// </summary>
        private void AddMergedCell<T>(ExcelWorksheet worksheet, int rowNumStart, int colNumStart, int rowNumEnd, int colNumEnd, T value, Action<ExcelRange> action)
        {
            worksheet.Cells[rowNumStart, colNumStart].Value = value;
            using (ExcelRange r = worksheet.Cells[rowNumStart, colNumStart, rowNumEnd, colNumEnd])
            {
                r.Merge = true;
                if (action != null)
                {
                    action(r);
                }
            }
        }

        /// <summary>
        /// Adds Hewitt scores for the first worksheet.
        /// </summary>
        private void AddHewitt(ExcelWorksheet worksheet, ref int rowNum, DataRow currentRow, Dictionary<int, DataRow> historicalRows, Color color, string typeLabel,
                                string questionNumber1, string hewittQuestion1, string questionText1, string colName1,
                                string questionNumber2, string hewittQuestion2, string questionText2, string colName2)
        {
            Action<ExcelRange> style = r =>
            {
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            };
            //First row
            worksheet.Row(rowNum).Height = 26.96f;
            AddValue(worksheet, rowNum, 1, typeLabel, r =>
            {
                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            });
            AddValue(worksheet, rowNum, 2, questionNumber1, style);
            AddValue(worksheet, rowNum, 3, hewittQuestion1, style);
            AddMergedCell(worksheet, "D" + rowNum, "G" + rowNum, questionText1, r =>
            {
                style(r);
                r.Style.WrapText = true;
                r.Style.Font.Size = 8;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            });
            AddValues(worksheet, rowNum, currentRow, historicalRows, colName1, CellFormat.Percent, style);
            rowNum++;

            //Second row
            worksheet.Row(rowNum).Height = 26.96f;
            AddValue(worksheet, rowNum, 1, typeLabel, r =>
            {
                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            });
            AddValue(worksheet, rowNum, 2, questionNumber2, style);
            AddValue(worksheet, rowNum, 3, hewittQuestion2, style);
            AddMergedCell(worksheet, "D" + rowNum, "G" + rowNum, questionText2, r =>
            {
                style(r);
                r.Style.WrapText = true;
                r.Style.Font.Size = 8;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            });
            AddValues(worksheet, rowNum, currentRow, historicalRows, colName2, CellFormat.Percent, style);

            //Set background color:
            using (ExcelRange r = worksheet.Cells[rowNum - 1, 1, rowNum, 3])
            {
                r.Style.Fill.BackgroundColor.SetColor(color);
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            }
            rowNum++;
        }

        /// <summary>
        /// Adds Gallup scores for the first worksheet.
        /// </summary>
        private void AddGallup(ExcelWorksheet worksheet, ref int rowNum, DataRow currentRow, Dictionary<int, DataRow> historicalRows, Color color, string campLabel,
                                string gcQuestionNumber, string gallupQuestionNumber, string questionText, string colName)
        {
            Action<ExcelRange> style = r =>
            {
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            };
            //First row
            worksheet.Row(rowNum).Height *= 2;
            AddValue(worksheet, rowNum, 1, campLabel, r =>
            {
                r.Style.WrapText = true;
                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            });
            AddValue(worksheet, rowNum, 2, gcQuestionNumber, style);
            AddValue(worksheet, rowNum, 3, gallupQuestionNumber, style);
            AddMergedCell(worksheet, "D" + rowNum, "G" + rowNum, questionText, r =>
            {
                style(r);
                r.Style.WrapText = true;
                r.Style.Font.Size = 8;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            });
            AddValues(worksheet, rowNum, currentRow, historicalRows, colName, CellFormat.Percent, style);

            //Set background color:
            using (ExcelRange r = worksheet.Cells[rowNum, 1, rowNum, 3])
            {
                r.Style.Fill.BackgroundColor.SetColor(color);
                r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            }
            rowNum++;
        }

        /// <summary>
        /// Adds temp check scores for the first worksheet.
        /// </summary>
        private void AddTempCheck(ExcelWorksheet worksheet, ref int rowNum, DataRow currentRow, Dictionary<int, DataRow> historicalRows, string questionNumber, string questionText, string colName)
        {
            Action<ExcelRange> style = r =>
            {
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                r.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            };
            //First row
            worksheet.Row(rowNum).Height = 26.96f;
            AddValue(worksheet, rowNum, 1, questionNumber, style);
            AddMergedCell(worksheet, "B" + rowNum, "G" + rowNum, questionText, r =>
            {
                style(r);
                r.Style.WrapText = true;
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            });
            AddValues(worksheet, rowNum, currentRow, historicalRows, colName, CellFormat.Percent, style);
            rowNum++;
        }

        /// <summary>
        /// Cleans up the filename for sending to the user.
        /// </summary>
        public static string MakeValidFileName(string name)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(name, invalidRegStr, "_").Replace(",", String.Empty).Replace("\"", String.Empty);
        }

        #endregion Data Load Helpers

        #region Dropdown Change Events

        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDepartment.Visible = false;
            if (String.IsNullOrWhiteSpace(ddlRegion.SelectedValue))
            {
                ddlProperty.Visible = false;
                ddlProperty.SelectedIndex = 0;
                ddlDepartment.SelectedIndex = 0;
            }
            else
            {
                foreach (ListItem li in ddlProperty.Items)
                {
                    if (String.IsNullOrWhiteSpace(li.Value)) { continue; }
                    int propID = li.Value.Split('-')[0].ToString().StringToInt();
                    switch (ddlRegion.SelectedValue)
                    {
                        case "BC":
                            li.Enabled = (propID <= 10 || propID == 14 || propID == 21);
                            break;

                        case "NS":
                            li.Enabled = (propID == 11 || propID == 12);
                            break;

                        case "ON":
                            li.Enabled = (propID == 15 || propID == 16 || propID == 17 || propID == 18 || propID == 20);
                            break;

                        case "WA":
                            li.Enabled = (propID == 13);
                            break;
                        case "NB":
                            li.Enabled = (propID == 19);
                            break;
                    }
                }
                ddlProperty.Visible = true;
            }
        }

        protected void ddlProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(ddlProperty.SelectedValue))
            {
                ddlDepartment.Visible = false;
                ddlDepartment.SelectedIndex = 0;
            }
            else
            {
                //int ind = 1;
                //int propID = ddlProperty.SelectedValue.Split('-')[0].ToString().StringToInt();
                ////Be sure to also update SurveySnapshot2015.aspx.cs > Page_LoadComplete
                ////Accounting / Receiving / Human Resources
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Administration & Miscellaneous
                //ddlDepartment.Items[ind++].Enabled = (propID == 12);
                ////Banquets
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////BC Operations & Development Management
                //ddlDepartment.Items[ind++].Enabled = (propID == 1);
                ////Bingo, Cage & Slots
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 8, 10 }.Contains(propID));
                ////Cage & Countroom
                //ddlDepartment.Items[ind++].Enabled = (propID == 12);
                ////Cage / Count
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 6, 7, 13, 19, 18, 17 }.Contains(propID));
                ////Cage / Countroom / Guest Services
                //ddlDepartment.Items[ind++].Enabled = (propID == 11);
                ////Casino: Guest Services
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Casino Guest Services & Entertainment & Spa
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Casino Housekeeping
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Casino Operations
                //ddlDepartment.Items[ind++].Enabled = (propID == 12);
                ////Corporate Support Services
                //ddlDepartment.Items[ind++].Enabled = (propID == 1);
                ////Culinary
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 12, 19 }.Contains(propID));
                ////Culinary / Food & Beverage Management & Admin
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Culinary / Stewarding
                //ddlDepartment.Items[ind++].Enabled = (propID == 11);
                ////Executive & Senior Management
                //ddlDepartment.Items[ind++].Enabled = (propID == 1);
                ////Facilities
                //ddlDepartment.Items[ind++].Enabled = (propID == 17);
                ////Facilities / Maintenance
                //ddlDepartment.Items[ind++].Enabled = (propID == 18);
                ////Finance & Accounting
                //ddlDepartment.Items[ind++].Enabled = (propID == 1);
                ////Finance / Receiving / Human Resources / IT(TSG)
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Food & Beverage
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 3, 14, 5, 6, 7, 8, 9, 10, 12, 13, 15, 18 }.Contains(propID));
                ////Food & Beverage Management (Including Culinary)
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Food & Beverage: Outlets
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 19 }.Contains(propID));
                ////Food & Beverage: Banquets
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 11 }.Contains(propID));
                ////Food & Beverage: Beverage
                //ddlDepartment.Items[ind++].Enabled = (propID == 11);
                ////Food & Beverage: Restaurants
                //ddlDepartment.Items[ind++].Enabled = (propID == 11);
                ////Gaming Operations: Bingo, Slots and Cage
                //ddlDepartment.Items[ind++].Enabled = (propID == 9);
                ////Guest Services
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 18, 17 }.Contains(propID));
                ////Guest Services & Retail
                //ddlDepartment.Items[ind++].Enabled = (propID == 3);

                ////ddlDepartment.Items[ind++].Enabled = (propID == 4);

                ////Guest Services & Slots
                //ddlDepartment.Items[ind++].Enabled = (propID == 14);
                ////Guest Services/Slots
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 6, 7 }.Contains(propID));
                ////Hotel Ops Mrgs & Sups, Sales & Catering, Fac. Lead, Hotel Hskping Lead
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Hotel: Reservations, Front Office, Concierge and Guest Services
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 19 }.Contains(propID));
                ////Housekeeping
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Hotel Housekeeping
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Housekeeping/Maintenance/Facilities
                //ddlDepartment.Items[ind++].Enabled = (propID == 11);
                ////HPI & Customer Service
                //ddlDepartment.Items[ind++].Enabled = (propID == 5);
                ////Human Resources & Payroll
                //ddlDepartment.Items[ind++].Enabled = (propID == 1);
                ////Leadership / Administrative Assistant
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Mktg & Plyr Dev/Spa Mgr & Sup/CSG Mgr/Cage & Count Mgr & Sups/DB Mgr
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Mutuels
                //ddlDepartment.Items[ind++].Enabled = (propID == 5);
                ////Mutuels & Facilities
                //ddlDepartment.Items[ind++].Enabled = (propID == 15);
                ////Mutuels & Racing
                //ddlDepartment.Items[ind++].Enabled = (propID == 14);
                ////Operations Management
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 3, 14, 6, 7, 8, 9, 10, 12, 13, 15, 16, 18 }.Contains(propID));
                ////Operations Management (HR, Audit, IT, Managers)
                //ddlDepartment.Items[ind++].Enabled = (propID == 17);
                ////Operations Management & Marketing
                //ddlDepartment.Items[ind++].Enabled = (propID == 5);
                ////Operations Support
                //ddlDepartment.Items[ind++].Enabled = (propID == 18);
                ////Operations Support & Specialists
                //ddlDepartment.Items[ind++].Enabled = (propID == 11);
                ////Ops Management & Department Heads
                //ddlDepartment.Items[ind++].Enabled = (propID == 11);

                ////ddlDepartment.Items[ind++].Enabled = (propID == 4);

                ////Property / Janitorial
                ////ddlDepartment.Items[ind++].Enabled = (propID == 14);

                ////Property Services
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 5, 12 }.Contains(propID));
                ////Racing and First Aid
                //ddlDepartment.Items[ind++].Enabled = (propID == 5);

                ////ddlDepartment.Items[ind++].Enabled = (propID == 4);

                ////Racing/Race Office
                ////ddlDepartment.Items[ind++].Enabled = (propID == 14);
                ////Resort Management
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Sales, Marketing & Player Relations
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Security
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 6, 7, 8, 9, 10, 11, 12, 15, 16, 18, 17 }.Contains(propID));
                ////Security / Surveillance
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 13, 19 }.Contains(propID));
                ////Senior Management
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Slot Operations
                //ddlDepartment.Items[ind++].Enabled = (propID == 17);
                ////Slots
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 11, 18 }.Contains(propID));
                ////Slots (Includes Slot Techs)
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Slots & Guest Services
                //ddlDepartment.Items[ind++].Enabled = (propID == 5);
                ////Stewarding
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Surveillance
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 6, 7, 11, 12, 18, 17 }.Contains(propID));
                ////Table Games
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 14, 6, 7, 11, 13, 18 }.Contains(propID));
                ////Table Games & Customer Loyalty
                //ddlDepartment.Items[ind++].Enabled = (propID == 3);
                ////Table Games: Dealers
                //ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 19 }.Contains(propID));
                ////Table Games: Dealer Supervisors
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Table Games Management
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Table Games Management & Slot Supervisors
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Technology Services Group
                //ddlDepartment.Items[ind++].Enabled = (propID == 1);
                ////Theatre
                //ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Theatre: Box Office, Ushers
                //ddlDepartment.Items[ind++].Enabled = (propID == 3);



                int ind = 1;
                int propID = ddlProperty.SelectedValue.Split('-')[0].ToString().StringToInt();
                //Be sure to also update Reports/SnapshotExport.aspx.cs > ddlProperty_SelectedIndexChanged

                //All
                ddlDepartment.Items[ind++].Enabled = (new int[] { 16 }.Contains(propID));




                //Accounting / Receiving / Human Resources
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Administration & Miscellaneous
                //ddlDepartment.Items[ind++].Enabled = (propID == 12);
                //Banquets
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //BC Operations & Development Management
                ddlDepartment.Items[ind++].Enabled = (propID == 1);
                //Bingo, Cage & Slots
                ddlDepartment.Items[ind++].Enabled = (new int[] { 8, 10 }.Contains(propID));
                //Cage & Countroom
                ddlDepartment.Items[ind++].Enabled = (propID == 12);
                //Cage / Count
                ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 6, 7, 19, 18, 17, 20 }.Contains(propID));
                //Cage / Count, Surveillance & Security
                ddlDepartment.Items[ind++].Enabled = (propID == 13);

                //Cage / Countroom / Guest Services
                ddlDepartment.Items[ind++].Enabled = (propID == 11);
                //Casino: Guest Services
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Casino Guest Services & Entertainment & Spa
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);

                //Casino Guest Services including GS Manager and Marketing Coordintor
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                ////Casino Housekeeping
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Casino Housekeeping including Leads
                ddlDepartment.Items[ind++].Enabled = (propID == 19);

                //Casino Operations
                ddlDepartment.Items[ind++].Enabled = (propID == 12);
                //Corporate Support Services
                ddlDepartment.Items[ind++].Enabled = (propID == 1);
                //Culinary
                ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 12, 19 }.Contains(propID));
                //Culinary / Food & Beverage Management & Admin
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                //Culinary / Stewarding
                ddlDepartment.Items[ind++].Enabled = (propID == 11);
                //Executive & Senior Management
                ddlDepartment.Items[ind++].Enabled = (propID == 1);
                //Exec. Hskper, HGS Mgr, Exec Chef, Fac. Mgr, Buf. Mgr, Pub Mgr, Banq. Mgr, S&C Mgr, S&C Coord, Hot Hskping 
                ddlDepartment.Items[ind++].Enabled = (propID == 19);

                //Facilities
                ddlDepartment.Items[ind++].Enabled = (new int[] { 17, 20 }.Contains(propID));
                //Facilities / Maintenance
                ddlDepartment.Items[ind++].Enabled = (propID == 18);
                //Finance & Accounting
                ddlDepartment.Items[ind++].Enabled = (propID == 1);
                //Finance /Database Analyst / Human Resources/IT(TSG)
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Food & Beverage
                ddlDepartment.Items[ind++].Enabled = (new int[] { 3, 14, 5, 6, 7, 8, 9, 10, 12, 13, 18, 20 }.Contains(propID));
                //Food & Beverage Culinary
                ddlDepartment.Items[ind++].Enabled = (propID == 15);
                //Food & Beverage Buffet including Buffet Sups
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Food & Beverage Pub & Beverage including Pub Sups and Bev Hostesses
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Food & Beverage: Outlets
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                //Food & Beverage: Banquets
                ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 11 }.Contains(propID));
                //Food & Beverage: Beverage
                ddlDepartment.Items[ind++].Enabled = (propID == 11);
                //Food & Beverage: Restaurants
                ddlDepartment.Items[ind++].Enabled = (propID == 11);
                //Food & Beverage: Gaming
                ddlDepartment.Items[ind++].Enabled = (propID == 15);

                //Food & Beverage: Non gaming
                ddlDepartment.Items[ind++].Enabled = (propID == 15);
                //Gaming Operations: Bingo, Slots and Cage
                ddlDepartment.Items[ind++].Enabled = (propID == 9);
                //Gaming Operations: Bingo, F&B
                ddlDepartment.Items[ind++].Enabled = (propID == 21);
                //Guest Services
                ddlDepartment.Items[ind++].Enabled = (new int[] { 18, 20 }.Contains(propID));
                //Guest Services & Retail
                ddlDepartment.Items[ind++].Enabled = (propID == 3);

                //ddlDepartment.Items[ind++].Enabled = (propID == 4);

                //Guest Services & Slots
                ddlDepartment.Items[ind++].Enabled = (propID == 14);
                //Guest Services/Slots
                ddlDepartment.Items[ind++].Enabled = (new int[] { 6, 7 }.Contains(propID));
                ////Hotel Ops Mrgs & Sups, Sales & Catering, Fac. Lead, Hotel Hskping Lead
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Hotel: Reservations, Front Office, Concierge and Guest Services
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                //Hotel: Res., Concierge and GS, Night Audit, GS Supervisor 
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Housekeeping
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Hotel Housekeeping
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Hotel Housekeeping including Lead and supervisor
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Housekeeping/Maintenance/Facilities
                ddlDepartment.Items[ind++].Enabled = (propID == 11);
                //HPI & Customer Service
                ddlDepartment.Items[ind++].Enabled = (propID == 5);
                //Human Resources & Payroll
                ddlDepartment.Items[ind++].Enabled = (propID == 1);

                //Human Resources,Accounting, Maintenance (Prperty)
                ddlDepartment.Items[ind++].Enabled = (propID == 3);


                //Janitorial & Maintenance
                ddlDepartment.Items[ind++].Enabled = (propID == 15);


                ////Leadership / Administrative Assistant
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Leadership, Admin Assist Mktg Mgr, Cage Mgr, Spa Mgr, Asian Host, Receiving, IT Mgr, Sec. Mgr, db Mgr, Ent.
                ddlDepartment.Items[ind++].Enabled = (propID == 19);

                //Marketing & Player Development
                ddlDepartment.Items[ind++].Enabled = (propID == 17);
                //Mutuels
                ddlDepartment.Items[ind++].Enabled = (propID == 5);
                //Mutuels, Security & Facilities
                ddlDepartment.Items[ind++].Enabled = (propID == 15);
                //Mutuels & Racing
                ddlDepartment.Items[ind++].Enabled = (propID == 14);
                //Operations Management
                ddlDepartment.Items[ind++].Enabled = (new int[] { 14, 6, 7, 8, 9, 10, 13, 18, 20 }.Contains(propID));
                //Operations Management/Admin
                ddlDepartment.Items[ind++].Enabled = (new int[] { 12, 15 }.Contains(propID));

                //Operations Management (HR, Audit, IT, Managers)
                ddlDepartment.Items[ind++].Enabled = (propID == 17);
                //Operations Management & Marketing
                ddlDepartment.Items[ind++].Enabled = (propID == 5);


                //Operations Support
                ddlDepartment.Items[ind++].Enabled = (propID == 18);
                //Operations Support (HR/Audit/IT)
                ddlDepartment.Items[ind++].Enabled = (propID == 20);
                //Operations Support & Specialists
                ddlDepartment.Items[ind++].Enabled = (propID == 11);
                //Ops Management & Department Heads
                ddlDepartment.Items[ind++].Enabled = (propID == 11);

                //ddlDepartment.Items[ind++].Enabled = (propID == 4);

                ////Property / Janitorial
                //ddlDepartment.Items[ind++].Enabled = (propID == 15);

                //Property Services
                ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 5, 12 }.Contains(propID));
                //Racing and First Aid
                ddlDepartment.Items[ind++].Enabled = (propID == 5);

                //ddlDepartment.Items[ind++].Enabled = (propID == 4);

                //Racing/Race Office
                //ddlDepartment.Items[ind++].Enabled = (propID == 14);
                //Resort Management
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                //Sales, Marketing & Player Relations
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                //Sales, Marketing, Buyer
                ddlDepartment.Items[ind++].Enabled = (propID == 3);

                //Security
                ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 6, 7, 8, 9, 10, 11, 12, 18, 17, 20 }.Contains(propID));
                ////Security / Surveillance
                //ddlDepartment.Items[ind++].Enabled = (propID == 13);
                //Security (Incl. Event Safety Officers) 
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Senior Management
                ddlDepartment.Items[ind++].Enabled = (new int[] { 2 }.Contains(propID));

                //Senior Management (includes DHs)
                ddlDepartment.Items[ind++].Enabled = (new int[] { 3 }.Contains(propID));

                //Slot Operations
                ddlDepartment.Items[ind++].Enabled = (propID == 17);
                //Slots
                ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 11, 18, 20 }.Contains(propID));
                //Slots (Includes Slot Techs)
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Slots & Guest Services
                ddlDepartment.Items[ind++].Enabled = (propID == 5);
                //Spa Aesticians & Lead
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Stewarding
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                //Surveillance
                ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 6, 7, 11, 12, 18, 17, 20, 19 }.Contains(propID));
                //Table Games
                ddlDepartment.Items[ind++].Enabled = (new int[] { 14, 6, 7, 11, 13, 18 }.Contains(propID));
                //Table Games & Customer Loyalty
                ddlDepartment.Items[ind++].Enabled = (propID == 3);
                //Table Games: Dealers
                ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 20 }.Contains(propID));
                //Table Games: Dealers & Dual Supervisors
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Table Games: Dealer Supervisors
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                //Table Games Management
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                ////Table Games Management & Slot Supervisors
                //ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Table Games Management including CSMs and Pit/Dual Pit Managers, Full Table Sups
                ddlDepartment.Items[ind++].Enabled = (propID == 19);
                //Table GamesSups/PitMan/CSM
                ddlDepartment.Items[ind++].Enabled = (propID == 20);
                //Technology Services Group
                ddlDepartment.Items[ind++].Enabled = (propID == 1);
                //Theatre
                ddlDepartment.Items[ind++].Enabled = (propID == 2);
                //Theatre: Box Office, Ushers
                ddlDepartment.Items[ind++].Enabled = (propID == 3);

          

                ddlDepartment.Visible = true;
            }
        }

        #endregion Dropdown Change Events
    }
}