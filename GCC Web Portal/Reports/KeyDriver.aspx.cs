using GCC_Web_Portal.Controls;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal.Reports
{
    public partial class KeyDriver : AuthenticatedPage
    {
        private const int DATA_ROW_START = 9;

        protected DataTable Data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.HideDateRangeFilter = false;
            Master.HideRegionFilter = true;
            Master.HidePropertyFilter = false;
            Master.HideSurveyTypeFilter = true;
            Master.HideBusinessUnitFilter = true;
            Master.HideSourceFilter = true;
            Master.HideStatusFilter = true;
            Master.HideFeedbackAgeFilter = true;
            Master.HideFBVenueFilter = true;
            Master.HideEncoreNumberFilter = true;
            Master.HidePlayerEmailFilter = true;
            Master.HideAgeRangeFilter = false;
            Master.HideGenderFilter = false;
            Master.HideLanguageFilter = true;
            Master.HideVisitsFilter = true;
            Master.HideSegmentsFilter = true;
            Master.HideTenureFilter = false;
            Master.HideTierFilter = false;
            Master.HideTextSearchFilter = true;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;

            SQLParamList sqlParams = Master.GetFilters();
            sql.CommandTimeout = 120;

            DataTable dt = sql.ExecStoredProcedureDataTable("spReports_KeyDriver", sqlParams);
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Oops. Something went wrong when generating the data. Please try again. (EKD100)";
            }
            else
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    p.Workbook.Worksheets.Add("Top 2 Box");
                    p.Workbook.Worksheets.Add("Correlation");
                    p.Workbook.Worksheets.Add("Room for Improvement");
                    ExcelWorksheet wsT2B = p.Workbook.Worksheets[1];
                    ExcelWorksheet wsCorr = p.Workbook.Worksheets[2];
                    ExcelWorksheet wsR4I = p.Workbook.Worksheets[3];
                    wsT2B.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                    wsT2B.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                    wsCorr.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                    wsCorr.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                    wsR4I.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                    wsR4I.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

                    DateRangeFilterControl drDateRange = Master.GetFilter<DateRangeFilterControl>("fltDateRange");
                    ReportFilterListBox ddlProperty = Master.GetFilter<ReportFilterListBox>("fltProperty");
                    ReportFilterListBox ddlAgeRange = Master.GetFilter<ReportFilterListBox>("fltAgeRange");
                    ReportFilterListBox ddlGender = Master.GetFilter<ReportFilterListBox>("fltGender");
                    ReportFilterListBox ddlTenure = Master.GetFilter<ReportFilterListBox>("fltTenure");
                    ReportFilterListBox ddlTier = Master.GetFilter<ReportFilterListBox>("fltTier");

                    wsT2B.Cells[1, 1].Value = "Start Date: " + drDateRange.BeginDate.Value.ToString("yyyy-MM-dd");
                    wsT2B.Cells[2, 1].Value = "End Date: " + drDateRange.EndDate.Value.ToString("yyyy-MM-dd");
                    wsT2B.Cells[3, 1].Value = GetFilterDetails("Properties: ", ddlProperty.Items);
                    wsT2B.Cells[4, 1].Value = GetFilterDetails("Age Range: ", ddlAgeRange.Items);
                    wsT2B.Cells[5, 1].Value = GetFilterDetails("Gender: ", ddlGender.Items);
                    wsT2B.Cells[6, 1].Value = GetFilterDetails("Tenure: ", ddlTenure.Items);
                    wsT2B.Cells[7, 1].Value = GetFilterDetails("Tier: ", ddlTier.Items);

                    int rowNum = DATA_ROW_START;
                    int altRowOffset = 1 - DATA_ROW_START;

                    //Set the header styles
                    wsT2B.Row(rowNum).Style.Font.Bold = true;
                    wsT2B.Row(rowNum).Style.Font.Size = 12;
                    wsCorr.Row(rowNum + altRowOffset).Style.Font.Bold = true;
                    wsCorr.Row(rowNum + altRowOffset).Style.Font.Size = 12;
                    wsR4I.Row(rowNum + altRowOffset).Style.Font.Bold = true;
                    wsR4I.Row(rowNum + altRowOffset).Style.Font.Size = 12;

                    AddValue(wsT2B, rowNum, 1, "Question", null);
                    AddValue(wsT2B, rowNum, 2, "Section", null);
                    AddValue(wsT2B, rowNum, 3, "Label", null);
                    AddValue(wsT2B, rowNum, 4, "Top 2 Box", null);
                    AddValue(wsT2B, rowNum, 5, "Average", null);

                    AddValue(wsCorr, rowNum + altRowOffset, 1, "Question", null);
                    AddValue(wsCorr, rowNum + altRowOffset, 2, "Label", null);
                    AddValue(wsCorr, rowNum + altRowOffset, 3, "Correlation", null);

                    AddValue(wsR4I, rowNum + altRowOffset, 1, "Question", null);
                    AddValue(wsR4I, rowNum + altRowOffset, 2, "Label", null);
                    AddValue(wsR4I, rowNum + altRowOffset, 3, "Room for Improvement", null);

                    rowNum++;

                    DataRow dr = dt.Rows[0];
                    string lastSection = String.Empty;
                    List<Tuple<string, string, object>> corrRows = new List<Tuple<string, string, object>>();
                    List<Tuple<string, string, object>> r4iRows = new List<Tuple<string, string, object>>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        string[] part1 = dc.ColumnName.Split('_');
                        //Check and make sure we haven't done these (they're done with the correlation columns).
                        if (part1.Length < 2
                            || part1[1].Equals("T2B")
                            || part1[1].Equals("R4I"))
                        {
                            continue;
                        }
                        string[] part2 = part1[1].Split(new string[] { " - " }, 2, StringSplitOptions.None);
                        string dbColumn = part1[0];
                        string section = part2[0];
                        string label = part2[1];

                        //T2B scores
                        AddValue(wsT2B, rowNum, 1, dbColumn, null);
                        //Skip section values so the multi-level category labels will show up correctly
                        if (section != lastSection)
                        {
                            AddValue(wsT2B, rowNum, 2, section, null);
                            lastSection = section;
                        }
                        AddValue(wsT2B, rowNum, 3, label, null);
                        AddValue(wsT2B, rowNum, 4, dr[dbColumn + "_T2B"], r =>
                        {
                            r.Style.Numberformat.Format = "0.0%";
                        });
                        AddValue(wsT2B, rowNum, 5, null, r =>
                        {
                            r.Formula = "AVERAGE(D:D)";
                        });

                        //Correlation scores
                        corrRows.Add(new Tuple<string, string, object>(dbColumn, part1[1], dr[dc.ColumnName]));
                        //Room for Improvement
                        r4iRows.Add(new Tuple<string, string, object>(dbColumn, part1[1], dr[dbColumn + "_R4I"]));

                        rowNum++;
                    }

                    //Add sorted items to correlation sheet
                    rowNum = DATA_ROW_START + 1;
                    foreach (var rowDeets in corrRows.OrderByDescending(r =>
                    {
                        return r.Item3.ToString();
                    }))
                    {
                        AddValue(wsCorr, rowNum + altRowOffset, 1, rowDeets.Item1, null);
                        AddValue(wsCorr, rowNum + altRowOffset, 2, rowDeets.Item2, null);
                        AddValue(wsCorr, rowNum + altRowOffset, 3, rowDeets.Item3, null);
                        rowNum++;
                    }

                    //Add sorted items to room for improvement sheet
                    rowNum = DATA_ROW_START + 1;
                    foreach (var rowDeets in r4iRows.OrderByDescending(r =>
                    {
                        return r.Item3.ToString();
                    }))
                    {
                        AddValue(wsR4I, rowNum + altRowOffset, 1, rowDeets.Item1, null);
                        AddValue(wsR4I, rowNum + altRowOffset, 2, rowDeets.Item2, null);
                        AddValue(wsR4I, rowNum + altRowOffset, 3, rowDeets.Item3, null);
                        rowNum++;
                    }

                    rowNum--; //Set to last row

                    //Set up T2B sheet

                    //Auto fit the columns
                    wsT2B.Cells[DATA_ROW_START, 1, rowNum, 4].AutoFitColumns();

                    double rowWidth = 0;
                    for (int i = 1; i <= 5; i++)
                    {
                        rowWidth += wsT2B.Column(i).Width;
                    }
                    rowWidth /= 0.1423;
                    rowWidth += 20; //Extra padding

                    //Add charts
                    var t2bChart = wsT2B.Drawings.AddChart("T2BChart", eChartType.ColumnClustered);

                    //Set position and size
                    t2bChart.SetPosition(DATA_ROW_START * 20, (int)rowWidth);
                    t2bChart.SetSize(1200, 800);
                    t2bChart.Title.Text = "Top 2 Box Scores";
                    t2bChart.XAxis.LabelPosition = eTickLabelPosition.NextTo;
                    t2bChart.XAxis.MajorTickMark = eAxisTickMark.None;
                    t2bChart.XAxis.MinorTickMark = eAxisTickMark.Out;
                    t2bChart.YAxis.MaxValue = 1;
                    t2bChart.YAxis.MinValue = 0;
                    t2bChart.YAxis.MinorTickMark = eAxisTickMark.None;

                    //Add main series.
                    var series = t2bChart.Series.Add(wsT2B.Cells[DATA_ROW_START + 1, 4, rowNum, 4], wsT2B.Cells[DATA_ROW_START + 1, 3, rowNum, 3]);
                    series.Header = "Top 2 Box Score";

                    //Hide the average column
                    wsT2B.Column(5).Hidden = true;
                    using (ExcelRange r = wsT2B.Cells[DATA_ROW_START + 1, 5, rowNum - 1, 5])
                    {
                        r.Calculate();
                    }

                    var lineChart = t2bChart.PlotArea.ChartTypes.Add(eChartType.Line);
                    var avgT2BSeries = lineChart.Series.Add(wsT2B.Cells[DATA_ROW_START + 1, 5, rowNum, 5], wsT2B.Cells[DATA_ROW_START + 1, 3, rowNum, 3]);
                    avgT2BSeries.Header = "Average Top 2";
                    lineChart.ShowHiddenData = true;

                    //Set up Correlation sheet

                    //Auto fit the columns
                    wsCorr.Cells[1, 1, rowNum + altRowOffset, 3].AutoFitColumns();

                    rowWidth = 0;
                    for (int i = 1; i <= 3; i++)
                    {
                        rowWidth += wsCorr.Column(i).Width;
                    }
                    rowWidth /= 0.1423;
                    rowWidth += 40; //Extra padding

                    //Add charts
                    var corrChart = wsCorr.Drawings.AddChart("CorrelationChart", eChartType.ColumnClustered);

                    //Set position and size
                    corrChart.SetPosition(20, (int)rowWidth);
                    corrChart.SetSize(1200, 800);
                    corrChart.Title.Text = "GEI Correlation Rankings";
                    corrChart.XAxis.LabelPosition = eTickLabelPosition.NextTo;
                    corrChart.XAxis.MajorTickMark = eAxisTickMark.None;
                    corrChart.XAxis.MinorTickMark = eAxisTickMark.Out;
                    corrChart.YAxis.MinorTickMark = eAxisTickMark.None;

                    //Add main series.
                    series = corrChart.Series.Add(wsCorr.Cells[2, 3, rowNum + altRowOffset, 3], wsCorr.Cells[2, 2, rowNum + altRowOffset, 2]);
                    series.Header = "Correlation Value";

                    //Set up Room for Improvement sheet

                    //Auto fit the columns
                    wsR4I.Cells[1, 1, rowNum + altRowOffset, 3].AutoFitColumns();

                    rowWidth = 0;
                    for (int i = 1; i <= 3; i++)
                    {
                        rowWidth += wsR4I.Column(i).Width;
                    }
                    rowWidth /= 0.1423;
                    rowWidth += 40; //Extra padding

                    //Add charts
                    var r4iChart = wsR4I.Drawings.AddChart("CorrelationChart", eChartType.ColumnClustered);

                    //Set position and size
                    r4iChart.SetPosition(20, (int)rowWidth);
                    r4iChart.SetSize(1200, 800);
                    r4iChart.Title.Text = "Room / Need for Improvement";
                    r4iChart.XAxis.LabelPosition = eTickLabelPosition.NextTo;
                    r4iChart.XAxis.MajorTickMark = eAxisTickMark.None;
                    r4iChart.XAxis.MinorTickMark = eAxisTickMark.Out;
                    r4iChart.YAxis.MinorTickMark = eAxisTickMark.None;

                    //Add main series.
                    series = r4iChart.Series.Add(wsR4I.Cells[2, 3, rowNum + altRowOffset, 3], wsR4I.Cells[2, 2, rowNum + altRowOffset, 2]);
                    series.Header = "Room For Improvement";

                    string lFileName = string.Format("KeyDriverAnalysis-{0}.xlsx", ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss-fff", User));
                    const string lPath = "~/Files/Cache/";

                    string lOutput = string.Concat(MapPath(lPath), lFileName);

                    FileInfo fi = new FileInfo(lOutput);
                    p.SaveAs(fi);
                    hlDownload.Text = "Download File - " + lFileName;
                    hlDownload.NavigateUrl = String.Format("/Files/Cache/{0}", lFileName);
                }
            }
        }

        private string GetFilterDetails(string label, ListItemCollection listItemCollection)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ListItem li in listItemCollection)
            {
                if (li.Selected)
                {
                    sb.AppendFormat("{0}, ", li.Text);
                }
            }
            if (sb.Length == 0)
            {
                sb.Append("All");
            }
            else
            {
                sb.Remove(sb.Length - 2, 2);
            }
            return label + sb.ToString();
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
    }
}