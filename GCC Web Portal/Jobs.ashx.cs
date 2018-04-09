using SharedClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    /// <summary>
    /// Summary description for Jobs
    /// </summary>
    public class Jobs : IHttpHandler
    {
        private string GCCPortalUrl = ConfigurationManager.AppSettings["GCCPortalURL"].ToString();

        public void ProcessRequest(HttpContext context)
        {
            //Only run locally or in the network
            string ip = RequestVars.GetRequestIPv4Address();
            if (!ip.Equals("127.0.0.1") && !ip.StartsWith("172.16.") && !ip.StartsWith("192.168.0."))
            {
                ErrorHandler.WriteLog("GCC_Web_Portal.Jobs", "Job attempted to be run by invalid IP: " + ip, ErrorHandler.ErrorEventID.General);
                return;
            }
            //Get the job ID
            string jobID = RequestVars.Get("jobid", String.Empty);
            switch (jobID)
            {
                case "e07db58b-d3a6-4e01-a5ed-ff9875773b3c":

                    #region Send weekly notification email

                    DateTime startDate = DateTime.Now.Date.AddDays(-7);
                    DateTime endDate = DateTime.Now.Date.AddMilliseconds(-1);
                    //DateTime startDate = new DateTime( 2015, 7, 1 ).Date;
                    //DateTime endDate = new DateTime( 2015, 8, 1 ).Date.AddMilliseconds( -1 );
                    SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
                    DataTable dt = sql.ExecStoredProcedureDataTable("spJobs_StatusEmail",
                                                    new SQLParamList()
                                                        .Add("@DateCreated_Begin", startDate)
                                                        .Add("@DateCreated_End", endDate));
                    StringBuilder sbCurrent = new StringBuilder();
                    sbCurrent.AppendFormat(@"<h3 style='margin:20px 0'>GEI / GSEI Dashboard for {0} to {1}</h3><table style='border-collapse:collapse;border:1px solid #444;width:100%' cellspacing='0' cellpadding='0'>", startDate.ToString("MMM d, yyyy"), endDate.ToString("MMM d, yyyy"));
                    sbCurrent.AppendFormat("<thead><tr><th{0}></th><th{0}># Surveys</th><th{0}>GEI</th><th{0}>NPS</th><th{0}>PRS</th><th{0}>GSEI</th><th{0}># Followup</th><th{0}>% Followup</th><th{0}># <24h</th><th{0}>#24-48h</th><th{0}># > 48h</th><th{0}>Avg. Response</th></tr></thead>",
                                            " style='padding:5px;border:1px solid #BBB'");
                    StringBuilder sbComparison = new StringBuilder("<h3 style='margin:20px 0'>Change from Previous Week</h3><table style='border-collapse:collapse;border:1px solid #444;width:100%' cellspacing='0' cellpadding='0'>");
                    sbComparison.AppendFormat("<thead><tr><th{0}></th><th{0}># Surveys</th><th{0}>GEI</th><th{0}>NPS</th><th{0}>PRS</th><th{0}>GSEI</th><th{0}># Followup</th><th{0}>% Followup</th><th{0}># <24h</th><th{0}>#24-48h</th><th{0}># > 48h</th><th{0}>Avg. Response</th></tr></thead>",
                                               " style='padding:5px;border:1px solid #BBB'");
                    Dictionary<string, List<string>> redFlagDetails = new Dictionary<string, List<string>>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        sbCurrent.AppendFormat("<tr><th style='padding:5px;text-align:left;border:1px solid #BBB;'>{0}</th><td{12}>{1:#,###}</td><td{12}>{2}</td><td{12}>{3}</td><td{12}>{4}</td><td{12}>{5}</td><td{12}>{6:#,###}</td><td{12}>{7:#,###}</td><td{12}>{8:#,###}</td><td{12}>{9:#,###}</td><td{12}>{10:#,###}</td><td{12}>{11}</td></tr>",
                            dr["ShortCode"],
                            dr["TotalRecords"],
                            ReportingTools.FormatIndex(dr["GEI"].ToString()),
                            ReportingTools.FormatPercent(dr["NPS"].ToString()),
                            ReportingTools.FormatPercent(dr["PRS"].ToString()),
                            ReportingTools.FormatPercent(dr["GSEI"].ToString()),
                            dr["FeedbackCount"], //6
                            ReportingTools.FormatPercent(dr["FeedbackCompletePercent"].ToString()),
                            dr["FeedbackLessThan24Hrs"],
                            dr["Feedback24HrsTo48Hrs"],
                            dr["FeedbackGreaterThan48Hrs"],
                            ReportingTools.MinutesToNiceTime(dr["AverageFeedbackResponse"].ToString()),
                            " style='padding:5px;border:1px solid #BBB;text-align:center'"
                            );
                        sbComparison.AppendFormat("<tr><th style='padding:5px;text-align:left;border:1px solid #BBB;'>{0}</th>{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}</tr>",
                            dr["ShortCode"],
                            GetChangeCell(dr, "TotalRecords_Diff", String.Format("{0:#,###}", dr["TotalRecords"]), redFlagDetails, null),
                            GetChangeCell(dr, "GEI_Diff", ReportingTools.FormatIndex(dr["GEI_Diff"].ToString()), redFlagDetails, r => { return r != -1000 && r <= -10; }),
                            GetChangeCell(dr, "NPS_Diff", ReportingTools.FormatPercent(dr["NPS_Diff"].ToString()), redFlagDetails, r => { return r != -1000 && r <= -0.1; }),
                            GetChangeCell(dr, "PRS_Diff", ReportingTools.FormatPercent(dr["PRS_Diff"].ToString()), redFlagDetails, r => { return r != -1000 && r <= -0.1; }),
                            GetChangeCell(dr, "GSEI_Diff", ReportingTools.FormatPercent(dr["GSEI_Diff"].ToString()), redFlagDetails, r => { return r != -1000 && r <= -0.1; }),
                            GetChangeCell(dr, "FeedbackCount_Diff", String.Format("{0:#,###}", dr["FeedbackCount_Diff"]), redFlagDetails, null), //6
                            GetChangeCell(dr, "FeedbackCompletePercent_Diff", ReportingTools.FormatPercent(dr["FeedbackCompletePercent_Diff"].ToString()), redFlagDetails, r => { return r != -1000 && r <= -0.1; }),
                            GetChangeCell(dr, "FeedbackLessThan24Hrs_Diff", String.Format("{0:#,###}", dr["FeedbackLessThan24Hrs_Diff"]), redFlagDetails, null),
                            GetChangeCell(dr, "Feedback24HrsTo48Hrs_Diff", String.Format("{0:#,###}", dr["Feedback24HrsTo48Hrs_Diff"]), redFlagDetails, null),
                            GetChangeCell(dr, "FeedbackGreaterThan48Hrs_Diff", String.Format("{0:#,###}", dr["FeedbackGreaterThan48Hrs_Diff"]), redFlagDetails, null),
                            GetChangeCell(dr, "AverageFeedbackResponse_Diff", ReportingTools.MinutesToNiceTime(dr["AverageFeedbackResponse_Diff"].ToString()), redFlagDetails, r => { return r != -1000 && r >= 120; })
                            );
                    }
                    sbCurrent.Append("</table><br /><br /><br />");
                    sbComparison.Append("</table>");

                    StringBuilder sbRedFlags = new StringBuilder();
                    foreach (var kvp in redFlagDetails)
                    {
                        if (sbRedFlags.Length == 0)
                        {
                            sbRedFlags.Append("<h3 style='margin:20px 0'>Red Flag Summary</h3><ul>");
                        }
                        if (kvp.Key.Length <= 4)
                        {
                            //Score
                            sbRedFlags.AppendFormat("<li><b>{0} score</b> has decreased by at least 10% for the following site(s): ", kvp.Key);
                        }
                        else if (kvp.Key.Equals("FeedbackCompletePercent"))
                        {
                            //Completion percentage
                            sbRedFlags.Append("<li><b>Feedback Completion</b> has decreased by at least 10% for the following site(s): ");
                        }
                        else if (kvp.Key.Equals("AverageFeedbackResponse"))
                        {
                            //Feedback response
                            sbRedFlags.Append("<li><b>Average Feedback Response</b> has increased by at least 2 hours for the following site(s): ");
                        }
                        foreach (string shortCode in kvp.Value)
                        {
                            sbRedFlags.AppendFormat("{0}, ", shortCode);
                        }
                        sbRedFlags.Remove(sbRedFlags.Length - 2, 2)
                                  .Append("</li>");
                    }
                    if (sbRedFlags.Length > 0)
                    {
                        sbRedFlags.Append("</ul><br />");
                        sbCurrent.Insert(0, sbRedFlags.ToString());
                    }

                    MailMessage msg = null;
                    try
                    {
                        var replacementModel = new
                        {
                            DataTables = sbCurrent.ToString() + sbComparison.ToString()
                        };
                        string path = context.Server.MapPath("~/Content/notifications/");
                        msg = EmailManager.CreateEmailFromTemplate(
                                            Path.Combine(path, "WeeklyNotification.htm"),
                                            replacementModel);
                        msg.IsBodyHtml = true;
                        msg.BodyEncoding = System.Text.Encoding.UTF8;
                        msg.From = new MailAddress("no-reply@gcgamingsurvey.com");
                        msg.Subject = "GCGC Weekly Status Notification - Week Ending " + endDate.ToString("MMM d, yyyy");
                        bool hasAddress = false;

                        dt = sql.QueryDataTable(@"
SELECT [SendType], u.[FirstName], u.[LastName],  u.[Email]
FROM [tblNotificationUsers] ne
	INNER JOIN [tblNotificationPropertySurveyReason] psr
		ON ne.[PropertySurveyReasonID] = psr.[PropertySurveyReasonID]
	INNER JOIN [tblCOM_Users] u
		ON ne.UserID = u.UserID
WHERE psr.PropertyID = @PropertyID
    AND psr.SurveyTypeID = @SurveyID
    AND psr.ReasonID = @ReasonID
;",
  new SQLParamList()
    .Add("@PropertyID", (int)GCCPropertyShortCode.GCC)
    .Add("@SurveyID", (int)SurveyType.GEI)
    .Add("@ReasonID", (int)NotificationReason.WeeklyStatusNotification)
);
                        if (!sql.HasError && dt.Rows.Count > 0)
                        {
                            StringBuilder addrs = new StringBuilder();
                            foreach (DataRow dr in dt.Rows)
                            {
                                switch (dr["SendType"].ToString())
                                {
                                    case "1":
                                        msg.To.Add(dr["Email"].ToString());
                                        addrs.Append(dr["FirstName"].ToString() + " " + dr["LastName"].ToString() + " <" + dr["Email"].ToString() + ">" + "\n");
                                        hasAddress = true;
                                        break;

                                    case "2":
                                        msg.CC.Add(dr["Email"].ToString());
                                        //Colin requested that CC addresses not show on the call Aug 10,2015
                                        //addrs.Append( dr["FirstName"].ToString() + " " + dr["LastName"].ToString() + " <" + dr["Email"].ToString() + ">" + "\n" );
                                        hasAddress = true;
                                        break;

                                    case "3":
                                        msg.Bcc.Add(dr["Email"].ToString());
                                        hasAddress = true;
                                        break;
                                }
                            }
                            //using ( StreamReader sr = new StreamReader( msg.AlternateViews[0].ContentStream ) ) {
                            //    msg.AlternateViews[0] = AlternateView.CreateAlternateViewFromString( sr.ReadToEnd().Replace( "{Recipients}", context.Server.HtmlEncode( addrs.ToString() ).Replace( "\n", "<br />" ) ), null, MediaTypeNames.Text.Html );
                            //}
                        }

                        if (hasAddress)
                        {
                            msg.Send();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.WriteLog("GCC_Web_Portal.Jobs", "Error running job.", ErrorHandler.ErrorEventID.General, ex);
                    }
                    finally
                    {
                        if (msg != null)
                        {
                            msg.Dispose();
                            msg = null;
                        }
                    }
                    return;

                    #endregion Send weekly notification email

                case "506aebb3-dfa2-4b34-94bc-51e81f5f31d3":

                    #region Send Feedback Reminder Email

                    sql = new SQLDatabase();
                    dt = sql.ExecStoredProcedureDataTable("[spJobs_48HrReminder]");
                    foreach (DataRow dr in dt.Rows)
                    {
                        GCCPropertyShortCode sc;
                        SurveyType st;
                        NotificationReason nr;
                        DateTime created = Conversion.XMLDateToDateTime(dr["DateCreated"].ToString());
                        string feedbackUID = dr["UID"].ToString();
                        if (Enum.TryParse(dr["PropertyID"].ToString(), out sc)
                            && Enum.TryParse(dr["SurveyTypeID"].ToString(), out st)
                            && Enum.TryParse(dr["ReasonID"].ToString(), out nr))
                        {
                            switch (st)
                            {
                                case SurveyType.GEI:
                                    string gagLocation = dr["GEIGAGLocation"].ToString();
                                    if (gagLocation.Length > 0)
                                    {
                                        gagLocation = " - " + gagLocation;
                                    }
                                    string fbLink = GCCPortalUrl + "Admin/Feedback/" + feedbackUID;
                                    SurveyTools.SendNotifications(HttpContext.Current.Server, sc, st, nr, string.Empty,
                                        new
                                        {
                                            Date = created.ToString("yyyy-MM-dd"),
                                            CasinoName = PropertyTools.GetCasinoName((int)sc) + gagLocation,
                                            Problems = (dr["Q27"].Equals(1) ? "Yes" : "No"),
                                            Response = (dr["Q40"].Equals(1) ? "Yes" : "No"),
                                            ProblemDescription = dr["Q27B"].ToString(),
                                            StaffComment = dr["Q11"].ToString(),
                                            GeneralComments = dr["Q34"].ToString(),
                                            MemorableEmployee = dr["Q35"].ToString(),
                                            FeedbackLinkTXT = (String.IsNullOrWhiteSpace(feedbackUID) ? String.Empty : "\n\nRespond to this feedback:\n" + fbLink + "\n"),
                                            FeedbackLinkHTML = (String.IsNullOrWhiteSpace(feedbackUID) ? String.Empty : @"<br /><br />
	<p><b>Respond to this feedback:</b></p>
	<p>" + fbLink + "</p>"),
                                            SurveyLink = GCCPortalUrl + "Display/GEI/" + dr["RecordID"].ToString()
                                        },
                                        String.Empty,
                                        "Overdue: ");
                                    break;

                                case SurveyType.Hotel:
                                    SurveyTools.SendNotifications(HttpContext.Current.Server, sc, st, nr, string.Empty,
                                        new
                                        {
                                            CasinoName = PropertyTools.GetCasinoName((int)sc),
                                            FeedbackNoteHTML = "<p><b>This guest has requested a response.</b> You can view and respond to the feedback request by clicking on (or copying and pasting) the following link:</p>\n<p>" + GCCPortalUrl + "Admin/Feedback/" + feedbackUID + "</p>",
                                            FeedbackNoteTXT = "The guest requested feedback. You can view and respond to the feedback request by clicking on (or copying and pasting) the following link:\n" + GCCPortalUrl + "Admin/Feedback/" + feedbackUID + "\n\n",
                                            SurveyLink = GCCPortalUrl + "Display/Hotel/" + dr["RecordID"].ToString()
                                        },
                                        String.Empty,
                                        "Overdue: ");
                                    break;

                                case SurveyType.Feedback:
                                    gagLocation = dr["FBKGAGLocation"].ToString();
                                    if (gagLocation.Length > 0)
                                    {
                                        gagLocation = " - " + gagLocation;
                                    }
                                    SurveyTools.SendNotifications(HttpContext.Current.Server, sc, st, nr, string.Empty,
                                        new
                                        {
                                            Date = created.ToString("yyyy-MM-dd"),
                                            CasinoName = PropertyTools.GetCasinoName((int)sc) + gagLocation,
                                            FeedbackNoteHTML = "<p><b>This guest has requested a response.</b> You can view and respond to the feedback request by clicking on (or copying and pasting) the following link:</p>\n<p>" + GCCPortalUrl + "Admin/Feedback/" + feedbackUID + "</p>",
                                            FeedbackNoteTXT = "The guest requested feedback. You can view and respond to the feedback request by clicking on (or copying and pasting) the following link:\n" + GCCPortalUrl + "Admin/Feedback/" + feedbackUID + "\n\n",
                                            SurveyLink = GCCPortalUrl + "Display/Feedback/" + dr["RecordID"].ToString()
                                        },
                                        String.Empty,
                                        "Overdue: ");
                                    break;

                                case SurveyType.Donation:
                                    SurveyTools.SendNotifications(HttpContext.Current.Server, sc, st, nr, string.Empty,
                                        new
                                        {
                                            Date = created.ToString("yyyy-MM-dd"),
                                            CasinoName = PropertyTools.GetCasinoName((int)sc),
                                            FeedbackLink = GCCPortalUrl + "Admin/Feedback/" + feedbackUID,
                                            Link = GCCPortalUrl + "Display/Donation/" + dr["RecordID"].ToString()
                                        },
                                        String.Empty,
                                        "Overdue: ");
                                    break;
                            }
                        }
                    }
                    return;

                    #endregion Send Feedback Reminder Email
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("Invalid Job ID.");
        }

        #region Weekly Notification Helpers - e07db58b-d3a6-4e01-a5ed-ff9875773b3c

        private string GetChangeCell(DataRow dr, string colname, string displayValue, Dictionary<string, List<string>> redFlagDetails, Func<double, bool> redFlagCheck)
        {
            double val = dr[colname].ToString().StringToDbl(-1000);
            bool redFlag = redFlagCheck != null && redFlagCheck(val);

            if (redFlag)
            {
                string key = colname.Replace("_Diff", String.Empty);
                if (!redFlagDetails.ContainsKey(key))
                {
                    redFlagDetails.Add(key, new List<string>());
                }
                redFlagDetails[key].Add(dr["ShortCode"].ToString());
            }

            return String.Format("<td style='padding:5px;border:1px solid #BBB;text-align:center;{1}'>{0}</td>", displayValue, (redFlag ? "background:#d04437;font-weight:bold;color:#fff" : String.Empty));
        }

        #endregion Weekly Notification Helpers - e07db58b-d3a6-4e01-a5ed-ff9875773b3c

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}