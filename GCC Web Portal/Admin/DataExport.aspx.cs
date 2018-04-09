using SharedClasses;
using System;
using System.Data;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin
{
    public partial class DataExport : AuthenticatedPage
    {
        protected DataTable Data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.HideAllFilters = true;
            drDateRange.User = User;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;

            SQLParamList sqlParams = new SQLParamList()
                                            .Add("@DateRange_Begin", drDateRange.BeginDate)
                                            .Add("@DateRange_End", Convert.ToDateTime(drDateRange.EndDate))
                                            .Add("@SurveyType", ddlSurvey.SelectedValue);
            if (ddlProperty.SelectedIndex == 0)
            {
                sqlParams.Add("@PropertyID", DBNull.Value);
            }
            else
            {
                sqlParams.Add("@PropertyID", ddlProperty.SelectedValue);
            }

            DataSet ds = sql.ExecStoredProcedureDataSet("spData_Export", sqlParams);
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Oops. Something went wrong when exporting the data. Please try again. (EDE100)";
            }
            else
            {
                string lFileName = string.Format("{0}_{1}", ddlSurvey.SelectedItem.Text, ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss", User));
                const string lPath = "~/Files/Cache/";

                string lOutput = string.Concat(MapPath(lPath), lFileName);

                using (CSVWriter csv = new CSVWriter(lOutput + ".csv"))
                {
                    csv.WriteLine("{0} Data Extract", ddlSurvey.SelectedItem.Text);
                    csv.WriteLine("Date Range: {0}", drDateRange.BeginDate.HasValue ? String.Format("{0} - {1}", drDateRange.BeginDate.Value.ToString("yyyy-MM-dd"), drDateRange.EndDate.Value.ToString("yyyy-MM-dd")) : "All");
                    csv.WriteLine("Property: {0}", ddlProperty.SelectedItem.Text);

                    //CSVRow longLabelRow = new CSVRow();
                    //CSVRow shortLabelRow = new CSVRow();
                    CSVRow headerRow = new CSVRow();
                    foreach (DataColumn dc in ds.Tables[0].Columns)
                    {
                        headerRow.Add(dc.ColumnName);
                        string kv = dc.ColumnName;
                        //if ( QuestionLookup.ContainsKey( kv ) ) {
                        //    string[] vals = QuestionLookup[kv].Split( '|' );
                        //    longLabelRow.Add( vals[0] );
                        //    if ( vals.Length > 1 ) {
                        //        shortLabelRow.Add( vals[1] );
                        //    } else {
                        //        shortLabelRow.Add( vals[1] );
                        //    }
                        //} else {
                        //    longLabelRow.Add( "" );
                        //    shortLabelRow.Add( "" );
                        //}
                    }
                    //csv.WriteRow( longLabelRow );
                    //csv.WriteRow( shortLabelRow );
                    csv.WriteRow(headerRow);

                    int colCount = ds.Tables[0].Columns.Count;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        CSVRow row = new CSVRow();
                        for (int i = 0; i < colCount; i++)
                        {
                            row.Add(dr[i].ToString());
                        }
                        csv.WriteRow(row);
                    }
                    csv.Close();
                    csv.Dispose();
                }
                //Conversion.DataTableToExcel(lResultSet, ddlQID.SelectedItem.Text, lOutput, true);
                hlDownload.Text = "Download File - " + String.Concat(lFileName, ".csv");
                hlDownload.NavigateUrl = String.Format("{0}{1}", lPath, String.Concat(lFileName, ".csv"));
            }
        }
    }
}