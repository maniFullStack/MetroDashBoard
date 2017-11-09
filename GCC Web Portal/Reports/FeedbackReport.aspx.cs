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

namespace GCC_Web_Portal.Reports
{
    public partial class FeedbackReport : AuthenticatedPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Feedback Export";
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
            SQLDatabase sql = new SQLDatabase();
            SQLParamList sqlParams = Master.GetFilters();

            DataSet ds;
            ds = sql.ExecStoredProcedureDataSet("spReports_FeedbackReport", sqlParams);

            using (ExcelPackage package = new ExcelPackage())
            {
                string fileName = "FeedBackExport";

                package.Workbook.Worksheets.Add("Feedback Items");
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