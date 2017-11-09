using SharedClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.UI;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin
{
    public partial class EmailPINGenerator : AuthenticatedPage
    {
        protected DataTable Data = null;

        private string GCCPortalUrl = ConfigurationManager.AppSettings["GCCPortalURL"].ToString();

        /// <summary>
        /// The currently selected batch ID. Returns -1 if none selected.
        /// </summary>
        protected int BatchID
        {
            get
            {
                object batchID = Page.RouteData.Values["batchid"];
                if (batchID != null)
                {
                    return batchID.ToString().StringToInt(-1);
                }
                else
                {
                    return -1;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "Email PIN Generator";
            Master.HideAllFilters = true;
            SQLDatabase sql = new SQLDatabase();
            if (BatchID == -1 && !IsPostBack)
            {
                //Load the list
                DataTable dt = sql.QueryDataTable(@"
                    SELECT eb.[EmailBatchID]
                          ,eb.[BatchName]
                          ,CONVERT(varchar(24), eb.[DateCreated], 126) AS [DateCreated]
	                      ,u.[FirstName]
	                      ,u.[LastName]
                    FROM [tblSurveyGEI_EmailBatches] eb
	                LEFT JOIN [tblCOM_Users] u ON eb.[CreateUserID] = u.UserID
                    ORDER BY [DateCreated] DESC");
                if (!sql.HasError)
                {
                    Data = dt;
                }
            }
            else if (BatchID != -1)
            {
                //Load the batch details
                DataTable dt = sql.ExecStoredProcedureDataTable("spAdmin_EmailBatchDetails", new SqlParameter("@BatchID", BatchID));
                if (sql.HasError || dt.Rows.Count == 0)
                {
                    TopMessage.ErrorMessage = "Unable to load batch details. Please go back and try again.";
                }
                else
                {
                    Data = dt;
                    if (!IsPostBack)
                    {
                        txtBatchNotes.Text = Data.Rows[0]["Notes"].ToString();
                    }
                }
            }
        }

        protected void btnCreateBatch_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtNewBatch.Text.Trim()))
            {
                TopMessage.ErrorMessage = "Please enter a name for the new batch.";
                return;
            }
            SQLDatabase sql = new SQLDatabase();
            sql.ExecStoredProcedureDataTable("spAdmin_CreateEmailBatch", new SqlParameter("@BatchName", txtNewBatch.Text), new SqlParameter("@CreateUserID", User.UserID));
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Unable to create the email batch. Please try agian.";
            }
            else
            {
                TopMessage.SuccessMessage = "Succesfully added the new email batch.";
            }
            DataTable dt = sql.QueryDataTable(@"
                SELECT eb.[EmailBatchID]
                    ,eb.[BatchName]
                    ,CONVERT(varchar(24), eb.[DateCreated], 126) AS [DateCreated]
	                ,u.[FirstName]
	                ,u.[LastName]
                FROM [tblSurveyGEI_EmailBatches] eb
	            LEFT JOIN [tblCOM_Users] u
		        ON eb.[CreateUserID] = u.UserID
                ORDER BY [DateCreated] DESC");
            if (!sql.HasError)
            {
                Data = dt;
            }
        }

        protected void btnSaveBatchNotes_Click(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();
            sql.NonQuery("UPDATE [tblSurveyGEI_EmailBatches] SET [Notes] = @Notes WHERE [EmailBatchID] = @BatchID;", new SqlParameter("@Notes", txtBatchNotes.Text), new SqlParameter("@BatchID", BatchID));
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Unable to save batch notes. Please try again.";
            }
            else
            {
                TopMessage.SuccessMessage = "Notes saved successfully.";
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (Data == null || Data.Rows.Count == 0)
            {
                TopMessage.ErrorMessage = "Invalid batch. Please go back and try again.";
                return;
            }

            if (!Data.Rows[0]["PINCount"].Equals(0))
            {
                TopMessage.ErrorMessage = "You cannot upload a file for a batch that has already generated a PIN file.";
                return;
            }

            if (fuEmailFile.HasFile)
            {
                try
                {
                    string fileExt = Path.GetExtension(fuEmailFile.FileName.ToLower());
                    if ((new string[] { ".xls", ".xlsx" }).Contains(fileExt))
                    {
                        if (fuEmailFile.PostedFile.ContentLength <= 5048576)
                        {
                            string tempPath = Path.GetTempFileName();
                            fuEmailFile.SaveAs(tempPath);

                            string connString = String.Format(fileExt.Equals(".xlsx") ? WebConfigurationManager.ConnectionStrings["Excel"].ConnectionString : WebConfigurationManager.ConnectionStrings["OldExcel"].ConnectionString, tempPath);

                            using (OleDbConnection fileConnection = new OleDbConnection(connString))
                            {
                                fileConnection.Open();

                                DataTable dt = GetFirstSheetDataTable(fileConnection);
                                //Make sure we have a worksheet
                                if (dt == null)
                                {
                                    TopMessage.ErrorMessage = "Unable to find the first worksheet. Please ensure the name of the first worksheet contains only letters and numbers. For example, \"Email\".";
                                }

                                //Make sure the columns exist in the sheet
                                if (String.IsNullOrWhiteSpace(TopMessage.ErrorMessage) && (!dt.Columns.Contains("Email") || !dt.Columns.Contains("Location") || !dt.Columns.Contains("Encore")))
                                {
                                    TopMessage.ErrorMessage = "The first sheet of the file must at least contain the columns \"Email\", \"Location\", and \"Encore\".";
                                }

                                //Make sure the columns exist in the sheet
                                if (String.IsNullOrWhiteSpace(TopMessage.ErrorMessage) && (dt.Rows.Count == 0))
                                {
                                    TopMessage.ErrorMessage = "No rows were found in the first sheet of this file.";
                                }

                                //HashSet<Tuple<string, GCCPropertyShortCode>> uniqueEmailProperties = new HashSet<Tuple<string, GCCPropertyShortCode>>();
                                if (String.IsNullOrWhiteSpace(TopMessage.ErrorMessage))
                                {
                                    List<string> errorMessages = new List<string>();

                                    dt.Columns.Add("Link", typeof(String));
                                    int rowNum = 2; //Start at row 2 because there's a header row in the Excel file
                                    int batchRowNum = 2;
                                    bool errsFound = false;
                                    StringBuilder query = new StringBuilder();
                                    SQLParamList sqlParams = new SQLParamList();
                                    sqlParams.Add("@BatchID", BatchID);
                                    List<Tuple<string, SQLParamList>> sqlBatches = new List<Tuple<string, SQLParamList>>();
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        string errs = "";

                                        //Verify the email address
                                        string email = dr["Email"].ToString().Trim();
                                        if (!Validation.RegExCheck(email, ValidationType.Email))
                                        {
                                            errs += String.Format(", invalid email address \"{0}\"", email);
                                        }

                                        //Verify the property
                                        string location = dr["Location"].ToString();
                                        GCCPropertyShortCode sc = GCCPropertyShortCode.None;
                                        if (!Enum.IsDefined(typeof(GCCPropertyShortCode), location)
                                            || !Enum.TryParse(dr["Location"].ToString(), out sc)
                                            || sc == GCCPropertyShortCode.None
                                            || sc == GCCPropertyShortCode.GCC)
                                        {
                                            errs += String.Format(", invalid property short code \"{0}\"", location);
                                        }

                                        ////Check if this is a valid email / property combo (we can't have duplicate emails at the same property)
                                        //if ( errs.Length == 0 ) {
                                        //    Tuple<string, GCCPropertyShortCode> cur = new Tuple<string, GCCPropertyShortCode>( email, sc );
                                        //    if ( uniqueEmailProperties.Contains( cur ) ) {
                                        //        errs += String.Format( ", duplicate email / property combination found for \"{0}\" and \"{1}\"", email, sc.ToString() );
                                        //    } else {
                                        //        uniqueEmailProperties.Add( cur );
                                        //    }
                                        //}

                                        //Verify the encore number
                                        string encoreString = dr["Encore"].ToString();
                                        int? encoreNumber = Conversion.StringToInt(encoreString, -1000);
                                        if (encoreNumber == -1000 && !String.IsNullOrWhiteSpace(encoreString))
                                        {
                                            errs += String.Format(", invalid encore number \"{0}\"", encoreString);
                                        }
                                        else if (encoreNumber == -1000)
                                        {
                                            encoreNumber = null;
                                        }

                                        bool isGSEIList = false;
                                        string gseiVal = null;
                                        string gseiURLStud = String.Empty;
                                        if (dt.Columns.Contains("GSEISurvey"))
                                        {
                                            isGSEIList = true;
                                            //If we have the GSEI survey column, validate it
                                            gseiVal = dr["GSEISurvey"].ToString();
                                            if (String.IsNullOrWhiteSpace(gseiVal))
                                            {
                                                errs += ", you must specify the GSEI survey type";
                                            }
                                            else if (!(new string[] { "BC", "HPI", "Hotel", "TicketMaster", "Great American" }).Contains(gseiVal))
                                            {
                                                errs += String.Format(", \"{0}\" is not a valid value for the GSEISurvey column (Note: this value is case sensitive)", gseiVal);
                                                //} else if ( gseiVal == "BC" && PropertyTools.GetCasinoRegion( sc ) != "BC" ) {
                                                //	errs += ", you can only use GSEISurvey value of \"BC\" for locations in BC";
                                                //} else if ( gseiVal == "HPI" && sc != GCCPropertyShortCode.FD && sc != GCCPropertyShortCode.EC && sc != GCCPropertyShortCode.HA ) {
                                            }
                                            else if (gseiVal == "HPI" && sc != GCCPropertyShortCode.EC && sc != GCCPropertyShortCode.HA)
                                            {
                                                errs += ", you can only use GSEISurvey value of \"HPI\" for locations FD, EC, or HA";
                                            }
                                            else if (gseiVal == "Hotel" && sc != GCCPropertyShortCode.RR)
                                            {
                                                errs += ", you can only use GSEISurvey value of \"Hotel\" for RR";
                                            }
                                            else if (gseiVal == "TicketMaster" && sc != GCCPropertyShortCode.RR && sc != GCCPropertyShortCode.HRCV)
                                            {
                                                errs += ", you can only use GSEISurvey value of \"TicketMaster\" for locations RR, or HRCV";
                                            }
                                            else if (gseiVal == "Great American" && sc != GCCPropertyShortCode.GAG)
                                            {
                                                errs += ", you can only use GSEISurvey value of \"Great American\" for GAG";
                                            }
                                            else
                                            {
                                                switch (gseiVal)
                                                {
                                                    case "BC":
                                                        gseiURLStud = "GSEIBC";
                                                        break;

                                                    case "HPI":
                                                        gseiURLStud = "GSEIHP";
                                                        break;

                                                    case "Hotel":
                                                        gseiURLStud = "GSEIHO";
                                                        break;

                                                    case "TicketMaster":
                                                        gseiURLStud = "GSEITM";
                                                        break;

                                                    case "Great American":
                                                        gseiURLStud = "GSEIGA";
                                                        break;
                                                }
                                            }
                                        }

                                        if (errs.Length == 0 && !errsFound)
                                        {
                                            //No errors with row or overall, generate GUID and update query
                                            Guid uid = Guid.NewGuid();
                                            query.AppendFormat("INSERT INTO [tblSurveyGEI_EmailPINs] ( [BatchID], [EmailAddress], [PropertyID], [Encore], [PIN]{2} ) VALUES (@BatchID, @Email{0}, {1}, @Encore{0}, @PIN{0}{3});\n", rowNum, (int)sc, (isGSEIList ? ", [GSEISurvey]" : String.Empty), (isGSEIList ? ", @GSEISurvey" + rowNum : String.Empty));
                                            sqlParams.Add("@Email" + rowNum, email)
                                                     .Add("@PIN" + rowNum, uid);
                                            if (encoreNumber != null)
                                            {
                                                sqlParams.Add("@Encore" + rowNum, encoreNumber);
                                            }
                                            else
                                            {
                                                sqlParams.Add("@Encore" + rowNum, DBNull.Value);
                                            }
                                            if (isGSEIList)
                                            {
                                                sqlParams.Add("@GSEISurvey" + rowNum, gseiVal);
                                                //Set the GSEI survey link

                                                dr["Link"] = String.Format(GCCPortalUrl + "{0}/{1}/{2}", gseiURLStud, sc.ToString(), uid.ToString());
                                            }
                                            else
                                            {
                                                //Set the GEI survey link
                                                dr["Link"] = String.Format(GCCPortalUrl + "SE/{0}/{1}", sc.ToString(), uid.ToString());
                                            }

                                            //Only ~2100 parameters are allowed so batch them
                                            if (batchRowNum * (isGSEIList ? 4 : 3) >= 2096)
                                            {
                                                sqlBatches.Add(new Tuple<string, SQLParamList>(query.ToString(), sqlParams));
                                                query = new StringBuilder();
                                                sqlParams = new SQLParamList().Add("@BatchID", BatchID);
                                                batchRowNum = 2;
                                            }
                                        }
                                        else if (errs.Length > 0)
                                        {
                                            //Errors found, add to list
                                            errsFound = true;
                                            errs = errs.Substring(2, errs.Length - 2);
                                            errorMessages.Add(String.Format("Error(s) found on row {0}: {1}", rowNum, errs));
                                        }

                                        rowNum++;
                                        batchRowNum++;
                                    }

                                    //If no errors found, save it all
                                    if (errorMessages.Count == 0)
                                    {
                                        //Add last set to the batch list
                                        if (!String.IsNullOrWhiteSpace(query.ToString()))
                                        {
                                            sqlBatches.Add(new Tuple<string, SQLParamList>(query.ToString(), sqlParams));
                                            query = new StringBuilder();
                                            sqlParams = new SQLParamList().Add("@BatchID", BatchID);
                                        }

                                        bool errorsFound = false;
                                        bool firstRound = true;

                                        //Insert everything into the db
                                        SQLDatabase sql = new SQLDatabase();
                                        foreach (var sqlBatch in sqlBatches)
                                        {
                                            sql.NonQuery(sqlBatch.Item1, sqlBatch.Item2);
                                            if (sql.HasError)
                                            {
                                                errorsFound = true;
                                                if (!firstRound)
                                                {
                                                    sql.NonQuery("DELETE FROM [tblSurveyGEI_EmailPINs] WHERE [BatchID] = @BatchID", new SqlParameter("@BatchID", BatchID));
                                                }
                                                TopMessage.ErrorMessage = "There was a database level error trying to generate and save the PINs. Please try again and if the problem persists, contact the administrator.";
                                                return;
                                            }
                                            firstRound = false;
                                        }

                                        if (errorsFound)
                                        {
                                            TopMessage.ErrorMessage = "There was an error trying to generate and save the PINs. Please try again and if the problem persists, contact the administrator.";
                                        }
                                        else
                                        {
                                            //Generate a CSV from the data table
                                            string outfile = GetFileName();
                                            outfile = Path.Combine(Server.MapPath(Config.PINFileDirectory), outfile);
                                            if (!Conversion.DataTableToCSV(dt, outfile, true))
                                            {
                                                TopMessage.ErrorMessage = "There was an error generating the output file. If you cannot download the file, please contact the administrator.";
                                            }

                                            //Set the PIN count so the page will update
                                            Data.Rows[0]["PINCount"] = rowNum - 2; //Subtract 2 because we started at 2
                                        }
                                    }
                                    else
                                    {
                                        //If errors were found, show them
                                        TopMessage.ErrorMessage = "The following errors were found when attempting to submit the file:<ul><li>";
                                        TopMessage.ErrorMessage += String.Join("</li><li>", errorMessages);
                                        TopMessage.ErrorMessage += "</li></ul>";
                                    }
                                }

                                fileConnection.Close();
                            }
                        }
                        else
                        {
                            TopMessage.ErrorMessage = "This file exceeds the maximum file size of 5MB.";
                        }
                    }
                    else
                    {
                        TopMessage.ErrorMessage = "Invalid file type. Only Excel files of type xls or xlsx are accepted.";
                    }
                }
                catch (Exception ex)
                {
                    TopMessage.ErrorMessage = "Something went wrong attempting to upload the file. Please try again.";
                    ErrorHandler.WriteLog("GCC_Web_Portal.Admin.EmailPINGenerator", "Unable to parse PIN file.", ErrorHandler.ErrorEventID.General, ex);
                }
            }
            else
            {
                TopMessage.ErrorMessage = "Please choose a file to upload.";
            }
        }

        protected string GetFileName()
        {
            return String.Format("GCC-EmailPINs-{0}-{1}.csv", BatchID, GCC_Web_Portal.SnapshotExport.MakeValidFileName(Data.Rows[0]["BatchName"].ToString().Replace(" ", "_")));
        }

        private DataTable GetFirstSheetDataTable(OleDbConnection fileConnection)
        {
            DataTable dt = new DataTable();
            string sheetName = GetFirstSheetName(fileConnection);
            if (sheetName == null)
            {
                return null;
            }
            else
            {
                OleDbCommand command = new OleDbCommand(String.Format("SELECT * FROM [{0}]", sheetName), fileConnection);
                OleDbDataAdapter da = new OleDbDataAdapter(command);
                da.Fill(dt);
                return dt;
            }
        }

        private string GetFirstSheetName(OleDbConnection fileConnection)
        {
            string sheetName = null;

            using (DataTable excelTables = fileConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null))
            {
                foreach (DataRow dr in excelTables.Rows)
                {
                    //skip invalid or empty sheets.
                    if (!dr["TABLE_NAME"].ToString().EndsWith("$"))
                        continue;
                    sheetName = dr["TABLE_NAME"].ToString();
                    break;
                }
                excelTables.Dispose();
            }
            return sheetName;
        }
    }
}