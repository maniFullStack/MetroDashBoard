using OfficeOpenXml;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin
{
    public partial class CrossTabReport : AuthenticatedPage
    {
        protected DataTable Data = null;
        protected DataRow HeaderRow = null;
        protected List<Answer> HeaderAnswers = null;

        protected DataRow RowRow = null;
        protected List<Answer> RowAnswers = null;

        protected struct Answer
        {
            public string Label { get; set; }
            public string DBValue { get; set; }
            public int Sort { get; set; }

            public Answer(string label, string dbValue, int sort)
                : this()
            {
                Label = label;
                DBValue = dbValue;
                Sort = sort;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.HideAllFilters = true;
            drDateRange.User = User;
            if (!IsPostBack)
            {
                SQLDatabase sql = new SQLDatabase();
                DataTable dt = sql.QueryDataTable(@"
SELECT [ColumnName],[Longlabel],[ShortLabel]
FROM [GCC].[dbo].[tblSurveyQuestions]
WHERE [AvailableOnCrossTab] = 1
    AND [AnswerTypeID] != 0
ORDER BY SortOrder
");
                if (sql.HasError)
                {
                    TopMessage.ErrorMessage = "Unable to load questions from the database. Please reload the page and try again. (ECT101)";
                }
                else
                {
                    ddlQuestion1.Items.Add(new ListItem());
                    ddlQuestion2.Items.Add(new ListItem());
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListItem li = new ListItem();
                        li.Value = dr["ColumnName"].ToString();
                        if (dr["ShortLabel"].Equals(DBNull.Value))
                        {
                            li.Text = dr["ColumnName"].ToString() + " - " + dr["LongLabel"].ToString();
                        }
                        else
                        {
                            li.Text = dr["ColumnName"].ToString() + " - " + dr["ShortLabel"].ToString();
                        }
                        ddlQuestion1.Items.Add(li);
                        ddlQuestion2.Items.Add(li);
                    }
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();

            if (String.IsNullOrWhiteSpace(ddlQuestion1.SelectedValue)
                || String.IsNullOrWhiteSpace(ddlQuestion1.SelectedValue))
            {
                TopMessage.ErrorMessage = "Please select two questions to generate cross-tabs for.";
                return;
            }
            else if (ddlQuestion1.SelectedValue == ddlQuestion2.SelectedValue)
            {
                TopMessage.ErrorMessage = "Please select two <b>separate</b> questions to generate cross-tabs for.";
                return;
            }

            SQLParamList sqlParams = new SQLParamList()
                                            .Add("@Question1", ddlQuestion1.SelectedValue)
                                            .Add("@Question2", ddlQuestion2.SelectedValue);
            DataSet ds = sql.QueryDataSet(@"
SELECT [SurveyTypeID]
      ,[ColumnName]
      ,[LongLabel]
      ,[ShortLabel]
      ,[AnswerTypeID]
      ,[AvailableOnCrossTab]
      ,[HasNA]
      ,[SortOrder]
FROM [tblSurveyQuestions]
WHERE [SurveyTypeID] = 1
    AND [ColumnName] IN (@Question1, @Question2);

SELECT [ColumnName]
      ,[DBValue]
      ,[Label]
	  ,[SortOrder]
FROM [GCC].[dbo].[tblSurveyQuestions_CustomAnswers]
WHERE [SurveyTypeID] = 1
	AND [ColumnName] IN (@Question1, @Question2)
ORDER BY [ColumnName], [SortOrder];", sqlParams);

            if (sql.HasError || ds.Tables.Count != 2 || ds.Tables[0].Rows.Count != 2)
            {
                TopMessage.ErrorMessage = "Unable to load question information from the database.";
                return;
            }

            DataRow drQ1, drQ2;

            if (ds.Tables[0].Rows[0]["ColumnName"].Equals(ddlQuestion1.SelectedValue))
            {
                drQ1 = ds.Tables[0].Rows[0];
                drQ2 = ds.Tables[0].Rows[1];
            }
            else
            {
                drQ1 = ds.Tables[0].Rows[1];
                drQ2 = ds.Tables[0].Rows[0];
            }

            List<Answer> q1Answers = GetAnswers(drQ1, ds.Tables[1]),
                         q2Answers = GetAnswers(drQ2, ds.Tables[1]);

            StringBuilder sbRowBase = new StringBuilder();
            StringBuilder sbGroupCols = new StringBuilder();

            bool headerNull = false;
            foreach (Answer a in q1Answers)
            {
                if (a.DBValue != "NULL")
                {
                    sbRowBase.AppendFormat("'{0}',", a.DBValue.Replace("'", "''"));
                    sbGroupCols.AppendFormat("SUM( CASE WHEN {{0}} = '{0}' THEN 1 ELSE 0 END ) AS '{1}',\n", a.DBValue.Replace("'", "''"), a.Label.Replace("'", "''"));
                }
                else
                {
                    headerNull = true;
                    sbGroupCols.AppendFormat("SUM( CASE WHEN {{0}} IS NULL THEN 1 ELSE 0 END ) AS '{1}',\n", a.DBValue.Replace("'", "''"), a.Label.Replace("'", "''"));
                }
            }
            if (sbRowBase.Length > 0)
            {
                sbRowBase.Remove(sbRowBase.Length - 1, 1);
            }

            StringBuilder sbRows = new StringBuilder();
            bool rowNull = false;
            foreach (Answer a in q2Answers)
            {
                if (a.DBValue != "NULL")
                {
                    sbRows.AppendFormat("UNION ALL SELECT '{0}' [Val], '{1}' [Label], {2} [Sort]\n",
                        a.DBValue.ToString().Replace("'", "''"),
                        a.Label.ToString().Replace("'", "''"),
                        a.Sort);
                }
                else
                {
                    rowNull = true;
                    sbRows.AppendFormat("UNION ALL SELECT NULL [Val], '{0}' [Label], {1} [Sort]\n",
                        a.Label.ToString().Replace("'", "''"),
                        a.Sort);
                }
            }

            string query = String.Format(@"
;WITH grp AS (
	SELECT CONVERT(varchar(1000), {0}) AS [ColVal],
		SUM( CASE WHEN {1} IN ( {2} ){5} THEN 1 ELSE 0 END ) AS [RowBase],
		{3}
		GROUPING( {0} ) AS [IsBase]
	FROM [tblSurveyGEI] d
	WHERE [DateEntered] BETWEEN @BeginDate AND @EndDate
		{6}
	GROUP BY {0}
	WITH ROLLUP
)
SELECT x.Label, grp.*
FROM (

SELECT 'Base' [Val], 'Base' [Label], -10 [Sort]
{4}

	) x
LEFT JOIN grp
	ON x.Val = grp.ColVal
		OR ( x.Sort = -10 AND grp.IsBase = 1 )
        OR ( grp.IsBase = 0 AND x.Val IS NULL AND grp.ColVal IS NULL )
ORDER BY x.Sort
",
                "[" + drQ2["ColumnName"].ToString() + "]",
                "[" + drQ1["ColumnName"].ToString() + "]",
                sbRowBase.ToString(),
                String.Format(sbGroupCols.ToString(), "[" + drQ1["ColumnName"].ToString() + "]"),
                sbRows.ToString(),
                (headerNull ? " OR [" + drQ1["ColumnName"].ToString() + "] IS NULL " : String.Empty),
                (!headerNull && !rowNull ? "AND [" + drQ2["ColumnName"].ToString() + "] IS NOT NULL" : String.Empty)
            );

            sqlParams = new SQLParamList()
                                .Add("@BeginDate", drDateRange.BeginDate)
                                .Add("@EndDate", drDateRange.EndDate);

            DataTable crossTab = sql.QueryDataTable(query, sqlParams);
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "There was an error querying the cross tab data from the database.";
                return;
            }
            Data = crossTab;
            HeaderRow = drQ1;
            HeaderAnswers = q1Answers;
            RowRow = drQ2;
            RowAnswers = q2Answers;

            if (ddlExport.SelectedIndex == 1)
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    p.Workbook.Worksheets.Add("CrossTabs");
                    ExcelWorksheet worksheet = p.Workbook.Worksheets[1];
                    worksheet.Cells.Style.Font.Size = 11; //Default font size for whole sheet
                    worksheet.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet
                    worksheet.Column(1).Width *= 5;

                    worksheet.Cells[1, 1].Value = "Start Date: " + drDateRange.BeginDate.Value.ToString("yyyy-MM-dd");
                    worksheet.Cells[2, 1].Value = "End Date: " + drDateRange.EndDate.Value.ToString("yyyy-MM-dd");
                    //worksheet.Cells[3, 1].Value = "Header Question: " + HeaderRow["LongLabel"].ToString() + ( !HeaderRow["ShortLabel"].Equals( DBNull.Value ) ? " - " + HeaderRow["ShortLabel"].ToString() : String.Empty );
                    //worksheet.Cells[4, 1].Value = "Row Question: " + RowRow["LongLabel"].ToString() + ( !RowRow["ShortLabel"].Equals( DBNull.Value ) ? " - " + RowRow["ShortLabel"].ToString() : String.Empty );

                    int rowNum = 4;
                    using (ExcelRange r = worksheet.Cells[rowNum, 1, rowNum + 1, 1])
                    {
                        r.Merge = true;
                    }
                    using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum + 1, 2])
                    {
                        r.Value = "Base";
                        r.Merge = true;
                        r.Style.Font.Bold = true;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    using (ExcelRange r = worksheet.Cells[rowNum, 3, rowNum, 2 + HeaderAnswers.Count])
                    {
                        r.Value = HeaderRow["LongLabel"].ToString() + (!HeaderRow["ShortLabel"].Equals(DBNull.Value) ? " - " + HeaderRow["ShortLabel"].ToString() : String.Empty);
                        r.Merge = true;
                        r.Style.WrapText = true;
                        r.Style.Font.Bold = true;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    worksheet.Row(rowNum).Height *= 2;
                    rowNum++;
                    for (int i = 0; i < HeaderAnswers.Count; i++)
                    {
                        using (ExcelRange r = worksheet.Cells[rowNum, 3 + i])
                        {
                            r.Value = HeaderAnswers[i].Label;
                            r.Style.Font.Bold = true;
                            r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                    }
                    rowNum++;
                    using (ExcelRange r = worksheet.Cells[rowNum, 1, rowNum + 1, 1])
                    {
                        r.Value = "Base";
                        r.Merge = true;
                        r.Style.Font.Bold = true;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                    }
                    double totalCount = Data.Rows[0]["RowBase"].ToString().StringToDbl();

                    using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum + 1, 2])
                    {
                        r.Value = totalCount;
                        r.Merge = true;
                        r.Style.Numberformat.Format = "#,###;-#,###;0";
                        r.Style.Font.Bold = true;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;
                        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    DataRow baseRow = Data.Rows[0];
                    int c = 3;
                    foreach (Answer a in HeaderAnswers)
                    {
                        AddExcelCellValue(worksheet, rowNum, c, totalCount, baseRow[a.Label]);
                        c++;
                    }
                    rowNum += 2;
                    worksheet.Row(rowNum).Height *= 3;
                    using (ExcelRange r = worksheet.Cells[rowNum, 1])
                    {
                        r.Value = RowRow["LongLabel"].ToString() + (!RowRow["ShortLabel"].Equals(DBNull.Value) ? " - " + RowRow["ShortLabel"].ToString() : String.Empty);
                        r.Style.Font.Bold = true;
                        r.Style.WrapText = true;
                        r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    }
                    using (ExcelRange r = worksheet.Cells[rowNum, 2, rowNum, 2 + HeaderAnswers.Count])
                    {
                        r.Merge = true;
                    }
                    rowNum++;
                    for (int i = 1; i < Data.Rows.Count; i++)
                    {
                        DataRow dr = Data.Rows[i];
                        using (ExcelRange r = worksheet.Cells[rowNum, 1])
                        {
                            r.Value = dr["Label"];
                            r.Style.Font.Bold = true;
                            r.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        }
                        AddExcelCellValue(worksheet, rowNum, 2, totalCount, dr["RowBase"]);
                        c = 3;
                        foreach (Answer a in HeaderAnswers)
                        {
                            AddExcelCellValue(worksheet, rowNum, c, totalCount, dr[a.Label]);
                            c++;
                        }
                        rowNum += 2;
                    }

                    worksheet.Cells[5, 3, 5, 2 + HeaderAnswers.Count].AutoFitColumns(9.14f);

                    string lFileName = string.Format("CrossTabs-{0}.xlsx", ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss-fff", User));
                    const string lPath = "~/Files/Cache/";

                    string lOutput = string.Concat(MapPath(lPath), lFileName);

                    FileInfo fi = new FileInfo(lOutput);
                    p.SaveAs(fi);
                    hlDownload.Text = "Download File - " + lFileName;
                    hlDownload.NavigateUrl = String.Format("/Files/Cache/{0}", lFileName);
                }
            }
            else
            {
                hlDownload.Text = String.Empty;
            }
        }

        private List<Answer> GetAnswers(DataRow questionRow, DataTable customVals)
        {
            List<Answer> answers = new List<Answer>();
            int answerType = questionRow["AnswerTypeID"].ToString().StringToInt();
            bool hasNA = (bool)questionRow["HasNA"];
            switch (answerType)
            {
                case -1: //Custom Answer
                    string colName = questionRow["ColumnName"].ToString().ToLower();
                    foreach (DataRow dr in customVals.Rows)
                    {
                        if (dr["ColumnName"].ToString().ToLower().Equals(colName))
                        {
                            answers.Add(new Answer(dr["Label"].ToString(), dr["DBValue"].ToString(), dr["SortOrder"].ToString().StringToInt()));
                        }
                    }
                    break;

                case 0: //Open End
                    //Can't do anything with this
                    break;

                case 1: //Scale 1-5 - Poor to Excellent
                    if (hasNA)
                    {
                        answers.Add(new Answer("N/A", "0", 0));
                    }
                    answers.Add(new Answer("Poor", "1", 1));
                    answers.Add(new Answer("Fair", "2", 2));
                    answers.Add(new Answer("Good", "3", 3));
                    answers.Add(new Answer("Very Good", "4", 4));
                    answers.Add(new Answer("Excellent", "5", 5));
                    break;

                case 2: //Scale 1-5 - Definitely Would Not to Definitely Would
                    if (hasNA)
                    {
                        answers.Add(new Answer("N/A", "0", 0));
                    }
                    answers.Add(new Answer("Definitely Would Not", "1", 1));
                    answers.Add(new Answer("Probably Would Not", "2", 2));
                    answers.Add(new Answer("Might or Might Not", "3", 3));
                    answers.Add(new Answer("Probably Would", "4", 4));
                    answers.Add(new Answer("Definitely Would", "5", 5));
                    break;

                case 3: //Scale 1-5 - Very Dissatisfied to Very Satisfied
                    if (hasNA)
                    {
                        answers.Add(new Answer("N/A", "0", 0));
                    }
                    answers.Add(new Answer("Very Dissatisfied", "1", 1));
                    answers.Add(new Answer("Dissatisfied", "2", 2));
                    answers.Add(new Answer("Satisfied", "3", 3));
                    answers.Add(new Answer("Very Satisfied", "4", 4));
                    answers.Add(new Answer("Extremely Satisfied", "5", 5));
                    break;

                case 4: //Yes / No
                    if (hasNA)
                    {
                        answers.Add(new Answer("N/A", "-1", 0));
                    }
                    answers.Add(new Answer("Yes", "1", 1));
                    answers.Add(new Answer("No", "0", 2));
                    break;

                case 5: //Checkbox
                    answers.Add(new Answer("Selected", "1", 1));
                    answers.Add(new Answer("Unselected", "NULL", 2));
                    break;
            }
            return answers;
        }

        protected string GetCellValue(double total, object value)
        {
            double val = value.ToString().StringToDbl();
            if (total == 0 || value.Equals(DBNull.Value) || val == 0)
            {
                return "<td>-<br />-</td>";
            }
            return String.Format("<td>{0:#,###}<br />{1:0.0%}</td>", val, val / total);
        }

        protected void AddExcelCellValue(ExcelWorksheet worksheet, int rowNum, int colNum, double total, object value)
        {
            double val = value.ToString().StringToDbl();
            object outputNum = String.Empty;
            object outputPercent = String.Empty;
            if (total == 0 || value.Equals(DBNull.Value) || val == 0)
            {
                outputNum = null;
                outputPercent = null;
            }
            else
            {
                outputNum = val;
                outputPercent = val / total;
            }

            using (ExcelRange r = worksheet.Cells[rowNum, colNum])
            {
                r.Value = outputNum;
                if (outputNum != null)
                {
                    r.Style.Numberformat.Format = "#,###;-#,###;0";
                }
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
            using (ExcelRange r = worksheet.Cells[rowNum + 1, colNum])
            {
                r.Value = outputPercent;
                if (outputPercent != null)
                {
                    r.Style.Numberformat.Format = "0.0%";
                }
                r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            }
        }
    }
}