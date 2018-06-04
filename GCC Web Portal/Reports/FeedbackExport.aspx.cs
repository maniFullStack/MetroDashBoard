using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal.Reports
{
    public partial class FeedbackExport : AuthenticatedPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Feedback Detail Export";
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

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        protected void GenerateReport()
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            sql.CommandTimeout = 90;
            DataSet ds = sql.ExecStoredProcedureDataSet("spReports_FeedbackReport_2",
                                                                new SqlParameter("@MonthStart", ddlMonth.SelectedValue + "-01"),
                                                                new SqlParameter("@PropertyID", ddlProperty.SelectedValue));


            //DataSet ds = sql.ExecStoredProcedureDataSet("spReports_FeedbackReport",
            //                                                   new SqlParameter("@MonthStart", ddlMonth.SelectedValue + "-01"),
            //                                                   new SqlParameter("@PropertyID", ddlProperty.SelectedValue));


            using (ExcelPackage package = new ExcelPackage())
            {
                GCCPropertyShortCode sc = (GCCPropertyShortCode)ddlProperty.SelectedValue.StringToInt(0);
                string fileName = sc.ToString() + "-FeedbackExport-" + ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss", User);

                package.Workbook.Worksheets.Add("GEI Survey");
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                worksheet.Cells.Style.Font.Size = 10;
                worksheet.Cells.Style.Font.Name = "Calibri";

                //set column widths
                worksheet.Column(1).Width = 23f;                    
                                   
                worksheet.Column(2).Width = 11.3f;
                worksheet.Column(3).Width = 11.3f;
                worksheet.Column(4).Width = 11.3f;
                worksheet.Column(5).Width = 11.3f;                   
                
                worksheet.Column(6).Width = 21.86f;
                worksheet.Column(7).Width = 21.86f;              
                worksheet.Column(8).Width = 9.2f;               
                worksheet.Column(9).Width = 31.86f;                    
                worksheet.Column(10).Width = 10.5f;                   
                worksheet.Column(11).Width = 21.7f;                   
                worksheet.Column(12).Width = 25f;
                worksheet.Column(13).Width = 10.5f;                   
                worksheet.Column(14).Width = 30f;                 
                worksheet.Column(15).Width = 15f;
                                
                DataTable DT = ds.Tables[0];
                int colNum = 1;
                int rowNum = 1;

                foreach (DataColumn col in DT.Columns)
                {
                    worksheet.Cells[rowNum, colNum].Value = col.ColumnName;
                    colNum++;
                }
                rowNum++;

                foreach (DataRow DR in DT.Rows)
                {
                    colNum = 1;
                    foreach (DataColumn col in DT.Columns)
                    {
                        worksheet.Cells[rowNum, colNum].Value = DR[col.ColumnName];
                        colNum++;
                    }
                    rowNum++;
                    
                }

                #region Feedback Survey Tab
                package.Workbook.Worksheets.Add("Feedback Survey");
                worksheet = package.Workbook.Worksheets[2];

                worksheet.Cells.Style.Font.Size = 10;
                worksheet.Cells.Style.Font.Name = "Calibri";

                DT = ds.Tables[1];
                colNum = 1;
                rowNum = 1;

                foreach (DataColumn col in DT.Columns)
                {
                    worksheet.Cells[rowNum, colNum].Value = col.ColumnName;
                    colNum++;
                }
                rowNum++;

                foreach (DataRow DR in DT.Rows)
                {
                    colNum = 1;
                    foreach (DataColumn col in DT.Columns)
                    {
                        worksheet.Cells[rowNum, colNum].Value = DR[col.ColumnName];
                        colNum++;
                    }
                    rowNum++;

                }
                #endregion Feedback Survey Tab

                #region Hotel Survey Tab
                package.Workbook.Worksheets.Add("Hotel Survey");
                worksheet = package.Workbook.Worksheets[3];

                worksheet.Cells.Style.Font.Size = 10;
                worksheet.Cells.Style.Font.Name = "Calibri";

                DT = ds.Tables[2];
                colNum = 1;
                rowNum = 1;

                foreach (DataColumn col in DT.Columns)
                {
                    worksheet.Cells[rowNum, colNum].Value = col.ColumnName;
                    colNum++;
                }
                rowNum++;

                foreach (DataRow DR in DT.Rows)
                {
                    colNum = 1;
                    foreach (DataColumn col in DT.Columns)
                    {
                        worksheet.Cells[rowNum, colNum].Value = DR[col.ColumnName];
                        colNum++;
                    }
                    rowNum++;

                }
                #endregion Hotel Survey Tab

                #region Donation Survey Tab
                package.Workbook.Worksheets.Add("Donation Survey");
                worksheet = package.Workbook.Worksheets[4];

                worksheet.Cells.Style.Font.Size = 10;
                worksheet.Cells.Style.Font.Name = "Calibri";

                DT = ds.Tables[3];
                colNum = 1;
                rowNum = 1;

                foreach (DataColumn col in DT.Columns)
                {
                    worksheet.Cells[rowNum, colNum].Value = col.ColumnName;
                    colNum++;
                }
                rowNum++;

                foreach (DataRow DR in DT.Rows)
                {
                    colNum = 1;
                    foreach (DataColumn col in DT.Columns)
                    {
                        worksheet.Cells[rowNum, colNum].Value = DR[col.ColumnName];
                        colNum++;
                    }
                    rowNum++;

                }
                #endregion Donation Survey Tab

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", String.Format("attachment;    filename={0}.xlsx", fileName));
                package.SaveAs(Response.OutputStream);
                worksheet.Dispose();
                Response.End();
            }
        }
            
    }
}