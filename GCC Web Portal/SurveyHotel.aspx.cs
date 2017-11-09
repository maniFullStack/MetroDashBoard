using SharedClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class SurveyHotel : BasePage
    {
        private const int LAST_PAGE = 10;
        private string GCCPortalUrl = ConfigurationManager.AppSettings["GCCPortalURL"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check for a reset and abandon the session and redirect to page 1.
            if (RequestVars.Get("r", 0) == 1)
            {
                Session.Abandon();
                Response.Redirect(GetURL(1, 1), true);
                return;
            }
            Master.ForceSpecificProperty = GCCPropertyShortCode.RR;

            Title = "Hotel Survey &raquo; " + Master.CasinoName;

            string[] answerLabels = new string[] { "N/A", "Very Dissatisfied", "Dissatisfied", "Satisfied", "Very Satisfied", "Extremely Satisfied" };
            Q2.SetAnswerLabels(answerLabels);
            Q3A.SetAnswerLabels(answerLabels);
            Q3B.SetAnswerLabels(answerLabels);
            Q3C.SetAnswerLabels(answerLabels);
            Q3D.SetAnswerLabels(answerLabels);
            Q3E.SetAnswerLabels(answerLabels);
            Q3F.SetAnswerLabels(answerLabels);
            Q3G.SetAnswerLabels(answerLabels);
            Q3H.SetAnswerLabels(answerLabels);
            Q3I.SetAnswerLabels(answerLabels);
            Q3J.SetAnswerLabels(answerLabels);
            Q4A.SetAnswerLabels(answerLabels);
            Q4B.SetAnswerLabels(answerLabels);
            Q4C.SetAnswerLabels(answerLabels);
            Q5A.SetAnswerLabels(answerLabels);
            Q5B.SetAnswerLabels(answerLabels);
            Q5C.SetAnswerLabels(answerLabels);
            Q5D.SetAnswerLabels(answerLabels);
            Q7A.SetAnswerLabels(answerLabels);
            Q7B.SetAnswerLabels(answerLabels);
            Q7C.SetAnswerLabels(answerLabels);
            Q7D.SetAnswerLabels(answerLabels);
            Q7E.SetAnswerLabels(answerLabels);
            Q7F.SetAnswerLabels(answerLabels);
            Q7G.SetAnswerLabels(answerLabels);
            Q7H.SetAnswerLabels(answerLabels);
            Q7I.SetAnswerLabels(answerLabels);
            Q8A.SetAnswerLabels(answerLabels);
            Q8B.SetAnswerLabels(answerLabels);
            Q8C.SetAnswerLabels(answerLabels);
            Q8D.SetAnswerLabels(answerLabels);
            Q8E.SetAnswerLabels(answerLabels);
            Q8F.SetAnswerLabels(answerLabels);
            Q9A.SetAnswerLabels(answerLabels);
            Q9B.SetAnswerLabels(answerLabels);
            Q9C.SetAnswerLabels(answerLabels);
            Q9D.SetAnswerLabels(answerLabels);
            Q9E.SetAnswerLabels(answerLabels);
            Q9F.SetAnswerLabels(answerLabels);
            Q9G.SetAnswerLabels(answerLabels);
            Q9H.SetAnswerLabels(answerLabels);
            Q9I.SetAnswerLabels(answerLabels);
            Q10A.SetAnswerLabels(answerLabels);
            Q10B.SetAnswerLabels(answerLabels);
            Q10C.SetAnswerLabels(answerLabels);
            Q10D.SetAnswerLabels(answerLabels);
            Q10E.SetAnswerLabels(answerLabels);
            Q10F.SetAnswerLabels(answerLabels);
            Q10G.SetAnswerLabels(answerLabels);
            Q10H.SetAnswerLabels(answerLabels);
            Q11A.SetAnswerLabels(answerLabels);
            Q11B.SetAnswerLabels(answerLabels);
            Q11C.SetAnswerLabels(answerLabels);
            Q11D.SetAnswerLabels(answerLabels);
            Q12A.SetAnswerLabels(answerLabels);
            Q12B.SetAnswerLabels(answerLabels);
            Q12C.SetAnswerLabels(answerLabels);
            Q12D.SetAnswerLabels(answerLabels);
            Q12E.SetAnswerLabels(answerLabels);
            Q13A.SetAnswerLabels(answerLabels);
            Q13B.SetAnswerLabels(answerLabels);
            Q13C.SetAnswerLabels(answerLabels);
            Q13D.SetAnswerLabels(answerLabels);
            Q13E.SetAnswerLabels(answerLabels);
            Q13F.SetAnswerLabels(answerLabels);
            Q13G.SetAnswerLabels(answerLabels);
            Q14A.SetAnswerLabels(answerLabels);
            Q14B.SetAnswerLabels(answerLabels);
            Q14C.SetAnswerLabels(answerLabels);
            Q14D.SetAnswerLabels(answerLabels);
            Q14E.SetAnswerLabels(answerLabels);
            Q14F.SetAnswerLabels(answerLabels);
            Q14G.SetAnswerLabels(answerLabels);
            Q15A.SetAnswerLabels(answerLabels);
            Q15B.SetAnswerLabels(answerLabels);
            Q15C.SetAnswerLabels(answerLabels);
            Q15D.SetAnswerLabels(answerLabels);
            Q15E.SetAnswerLabels(answerLabels);
            Q16A.SetAnswerLabels(answerLabels);
            Q16B.SetAnswerLabels(answerLabels);
            Q16C.SetAnswerLabels(answerLabels);
            Q16D.SetAnswerLabels(answerLabels);
            Q16E.SetAnswerLabels(answerLabels);
            Q16F.SetAnswerLabels(answerLabels);
            Q24D.SetAnswerLabels(answerLabels);

            Q22.SetAnswerLabels(new string[] { "Don't Know / N/A", "Not important", "Somewhat important", "Very important" });
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //Check all previous pages
            //Must do in LoadComplete because controls load values in Load method (Init didn't work because reasons...)

            if (Master.CurrentPage > 1 && Master.CurrentPage != 99 && !IsPostBack)
            {
                for (int i = 1; i < Master.CurrentPage; i++)
                {
                    //System.Diagnostics.Debug.WriteLine( "Checking Page: " + i );
                    if (!ValidateAndSave(i, false, false))
                    {
                        //System.Diagnostics.Debug.WriteLine( "Invalid Page: " + i );
                        Response.Redirect(GetURL(i, Master.RedirectDirection), true);
                        return;
                    }
                }
                if (PageShouldBeSkipped(Master.CurrentPage))
                {
                    int nextPage = Master.CurrentPage + Master.RedirectDirection;
                    if (Master.CurrentPage == LAST_PAGE && Master.RedirectDirection == 1)
                    {
                        nextPage = 99;
                    }
                    Response.Redirect(GetURL(nextPage, Master.RedirectDirection), true);
                    return;
                }
                //If we've made it to 97, save to database.
                if (Master.CurrentPage == 97 && !IsPostBack)
                {
                    int surveyID;
                    if (SaveData(out surveyID))
                    {
                        string feedbackUID;
                        CheckFeedback(surveyID, out feedbackUID);
                        SendNotifications(surveyID, feedbackUID);
                        Master.SurveyComplete = true;
                        Session.Abandon();
                        mmLastPage.SuccessMessage = "Your responses have been submitted successfully. Thank you for your feedback!<br /><br />Please Note: If you have requested someone to contact you, please ensure you check your \"Junk Mail\" folder or add \"@gcgamingsurvey.com\" to your email account's white list.";
                    }
                    else
                    {
                        mmLastPage.ErrorMessage = "We were unable to save your responses. Please go back and try again.";
                    }
                }
            }
        }

        public void CheckFeedback(int surveyID, out string feedbackUID)
        {
            if (Q29.SelectedValue == 1)
            {
                //Add the feedback
                SQLDatabase sql = new SQLDatabase();
                SqlParameter feedbackUIDParam = new SqlParameter("@UID", System.Data.SqlDbType.UniqueIdentifier);
                feedbackUIDParam.Direction = System.Data.ParameterDirection.Output;

                sql.ExecStoredProcedureDataSet("spFeedback_Create",
                    new SQLParamList()
                            .Add("@PropertyID", Master.PropertyID)
                            .Add("@SurveyTypeID", SharedClasses.SurveyType.Hotel)
                            .Add("@RecordID", surveyID)
                            .Add("@ReasonID", NotificationReason.Hotel)
                            .Add(feedbackUIDParam)
                );
                feedbackUID = feedbackUIDParam.Value.ToString();
            }
            else
            {
                feedbackUID = String.Empty;
            }
        }

        private void SendNotifications(int surveyID, string feedbackUID)
        {
            //Notification to guest if we have a valid email
            string email = txtEmail2.Text;
            if (String.IsNullOrEmpty(email))
            {
                email = txtEmail.Text;
            }

            // If user say yes to be contacted Q29 then notification sent
            if (Q29.SelectedValue == 1)
            {
                //Notification to staff
                SurveyTools.SendNotifications(
                    Server,
                    Master.PropertyShortCode,
                    SharedClasses.SurveyType.Hotel,
                    NotificationReason.Hotel,
                    string.Empty,
                    new
                    {
                        Date = DateTime.Now.ToString("yyyy-MM-dd"),
                        CasinoName = Master.CasinoName,
                        Problems = (Q23.SelectedValue == 1 ? "Yes" : "No"),
                        Response = (Q29.SelectedValue == 1 ? "Yes" : "No"),
                        ProblemDescription = Q24B.Text,
                        StaffComment = Q28.Text,
                        GeneralComments = txtFollowupReason.Text,
                        MemorableEmployee = txtQ21Explanation.Text,

                        FeedbackNoteHTML = String.IsNullOrEmpty(feedbackUID) ? String.Empty : "<p><b>This guest has requested a response.</b> You can view and respond to the feedback request by clicking on (or copying and pasting) the following link:</p>\n<p>" + GCCPortalUrl + "Admin/Feedback/" + feedbackUID + "</p>",
                        FeedbackNoteTXT = String.IsNullOrEmpty(feedbackUID) ? String.Empty : "The guest requested feedback. You can view and respond to the feedback request by clicking on (or copying and pasting) the following link:\n" + GCCPortalUrl + "Admin/Feedback/" + feedbackUID + "\n\n",
                        SurveyLink = GCCPortalUrl + "Display/Hotel/" + surveyID
                    });

                if (!String.IsNullOrEmpty(email))
                {
                    SurveyTools.SendNotifications(
                        Server,
                        Master.PropertyShortCode,
                        SharedClasses.SurveyType.Hotel,
                        NotificationReason.ThankYou,
                        string.Empty,
                        new
                        {
                            CasinoName = Master.CasinoName,
                            FeedbackNoteHTML = String.IsNullOrEmpty(feedbackUID) ? String.Empty : "<p>You can view and respond to your feedback request by clicking on (or copying and pasting) the following link:<br />" + GCCPortalUrl + "F/" + feedbackUID + "</p>",
                            FeedbackNoteTXT = String.IsNullOrEmpty(feedbackUID) ? String.Empty : "You can view and respond to your feedback request by clicking on (or copying and pasting) the following link:\n" + GCCPortalUrl + "F/" + feedbackUID + "\n\n",
                            Attachments = new SurveyTools.SurveyAttachmentDetails[] {
                            new SurveyTools.SurveyAttachmentDetails() { Path = "~/Images/headers/" + PropertyTools.GetCasinoHeaderImage( Master.PropertyShortCode ), ContentID = "HeaderImage" }
                            }
                        },
                        email);
                }
            }
        }

        protected void Prev_Click(object sender, EventArgs e)
        {
            if (ValidateAndSave(Master.CurrentPage, true, true))
            {
                int prevPage = Master.CurrentPage - 1;
                //Check if they're undoing their decline
                if (Master.CurrentPage == 99)
                {
                    prevPage = 2;
                }
                else if (Master.CurrentPage == 97)
                {
                    prevPage = LAST_PAGE;
                }
                Response.Redirect(GetURL(prevPage, -1), true);
            }
        }

        protected void Next_Click(object sender, EventArgs e)
        {
            if (ValidateAndSave(Master.CurrentPage, true, false))
            {
                int nextPage = Master.CurrentPage + 1;
                if (nextPage > LAST_PAGE)
                {
                    nextPage = 97;
                }
                if (Master.CurrentPage == 97)
                {
                    Response.Redirect(PropertyTools.GetCasinoURL(Master.PropertyShortCode), true);
                    return;
                }

                //Check for if they declined
                if (radDecline.Checked)
                {
                    nextPage = 99;
                }

                Response.Redirect(GetURL(nextPage, 1), true);
            }
        }

        /// <summary>
        /// Validates and saves a page's values. Returns true if the page is valid.
        /// </summary>
        /// <param name="page">The page ID to check.</param>
        /// <param name="currentPage">Whether this is the current page. If true, the values in the controls will be checked, otherwise the session will be checked.</param>
        /// <param name="saveOnly">If save only is true, the validation will be ignored and values will be saved so that they can be returned to. This is for use with the "Back" button.</param>
        protected bool ValidateAndSave(int page, bool currentPage, bool saveOnly)
        {
            bool retVal = true;
            switch (page)
            {
                case 1: // Intro

                    #region Page 1

                    string email = SurveyTools.GetValue(txtEmail, currentPage, String.Empty);
                    if (!Validation.RegExCheck(email, ValidationType.Email))
                    {
                        mmTxtEmail.ErrorMessage = "Please enter a valid email address.";
                        return false;
                    }
                    else if (currentPage)
                    {
                        SurveyTools.SaveValue<string>(txtEmail);
                    }
                    break;

                    #endregion Page 1

                case 2: // Privacy Policy

                    #region Page 2

                    if (!saveOnly)
                    {
                        bool accept = SurveyTools.GetValue(radAccept, currentPage, false);
                        bool decline = SurveyTools.GetValue(radDecline, currentPage, false);
                        if (!accept && !decline)
                        {
                            mmAcceptGroup.ErrorMessage = "Please select one of the following options.";
                            return false;
                        }
                    }
                    if (currentPage)
                    {
                        SurveyTools.SaveRadioButtons(radAccept, radDecline);
                    }
                    break;

                    #endregion Page 2

                case 3: // Q1-5

                    #region Page 3

                    if (!saveOnly)
                    {
                        if (!SurveyTools.CheckForAnswer(Q1Overall, true)
                            | !SurveyTools.CheckForAnswer(Q1A, true)
                            | !SurveyTools.CheckForAnswer(Q1B, true)
                            | !SurveyTools.CheckForAnswer(Q1C, true)
                            | !SurveyTools.CheckForAnswer(Q1D, true)
                            | !SurveyTools.CheckForAnswer(Q1E, true)
                            | !SurveyTools.CheckForAnswer(Q1F, true)
                            | !SurveyTools.CheckForAnswer(Q2, true)
                            | !SurveyTools.CheckForAnswer(Q3A, true)
                            | !SurveyTools.CheckForAnswer(Q3B, true)
                            | !SurveyTools.CheckForAnswer(Q3C, true)
                            | !SurveyTools.CheckForAnswer(Q3D, true)
                            | !SurveyTools.CheckForAnswer(Q3E, true)
                            | !SurveyTools.CheckForAnswer(Q3F, true)
                            | !SurveyTools.CheckForAnswer(Q3G, true)
                            | !SurveyTools.CheckForAnswer(Q3H, true)
                            | !SurveyTools.CheckForAnswer(Q3I, true)
                            | !SurveyTools.CheckForAnswer(Q3J, true)
                            | !SurveyTools.CheckForAnswer(Q4A, true)
                            | !SurveyTools.CheckForAnswer(Q4B, true)
                            | !SurveyTools.CheckForAnswer(Q4C, true)
                            | !SurveyTools.CheckForAnswer(Q5A, true)
                            | !SurveyTools.CheckForAnswer(Q5B, true)
                            | !SurveyTools.CheckForAnswer(Q5C, true)
                            | !SurveyTools.CheckForAnswer(Q5D, true))
                        {
                            retVal = false;
                        }
                    }
                    if (currentPage)
                    {
                        SurveyTools.SaveValue<int>(Q1Overall);
                        SurveyTools.SaveValue<int>(Q1A);
                        SurveyTools.SaveValue<int>(Q1B);
                        SurveyTools.SaveValue<int>(Q1C);
                        SurveyTools.SaveValue<int>(Q1D);
                        SurveyTools.SaveValue<int>(Q1E);
                        SurveyTools.SaveValue<int>(Q1F);
                        SurveyTools.SaveValue<int>(Q2);
                        SurveyTools.SaveValue<int>(Q3A);
                        SurveyTools.SaveValue<int>(Q3B);
                        SurveyTools.SaveValue<int>(Q3C);
                        SurveyTools.SaveValue<int>(Q3D);
                        SurveyTools.SaveValue<int>(Q3E);
                        SurveyTools.SaveValue<int>(Q3F);
                        SurveyTools.SaveValue<int>(Q3G);
                        SurveyTools.SaveValue<int>(Q3H);
                        SurveyTools.SaveValue<int>(Q3I);
                        SurveyTools.SaveValue<int>(Q3J);
                        SurveyTools.SaveValue<int>(Q4A);
                        SurveyTools.SaveValue<int>(Q4B);
                        SurveyTools.SaveValue<int>(Q4C);
                        SurveyTools.SaveValue<int>(Q5A);
                        SurveyTools.SaveValue<int>(Q5B);
                        SurveyTools.SaveValue<int>(Q5C);
                        SurveyTools.SaveValue<int>(Q5D);
                        SurveyTools.SaveValue<string>(txtQ5Amenities);
                    }
                    break;

                    #endregion Page 3

                case 4: // Q6 - Locations Visited

                    #region Page 4

                    if (!saveOnly)
                    {
                        if (!SurveyTools.CheckForAnswer(Q6Tramonto, true)
                            | !SurveyTools.CheckForAnswer(Q6TheBuffet, true)
                            | !SurveyTools.CheckForAnswer(Q6Curve, true)
                            | !SurveyTools.CheckForAnswer(Q6InRoomDining, true)
                            | !SurveyTools.CheckForAnswer(Q6FitnessCenter, true)
                            | !SurveyTools.CheckForAnswer(Q6PoolHotTub, true)
                            | !SurveyTools.CheckForAnswer(Q6Meeting, true)
                            | !SurveyTools.CheckForAnswer(Q6ValetParking, true)
                            | !SurveyTools.CheckForAnswer(Q6Concierge, true)
                            | !SurveyTools.CheckForAnswer(Q6BellDoorService, true))
                        {
                            retVal = false;
                        }
                    }
                    if (currentPage)
                    {
                        SurveyTools.SaveValue<int>(Q6Tramonto);
                        SurveyTools.SaveValue<int>(Q6TheBuffet);
                        SurveyTools.SaveValue<int>(Q6Curve);
                        SurveyTools.SaveValue<int>(Q6InRoomDining);
                        SurveyTools.SaveValue<int>(Q6FitnessCenter);
                        SurveyTools.SaveValue<int>(Q6PoolHotTub);
                        SurveyTools.SaveValue<int>(Q6Meeting);
                        SurveyTools.SaveValue<int>(Q6ValetParking);
                        SurveyTools.SaveValue<int>(Q6Concierge);
                        SurveyTools.SaveValue<int>(Q6BellDoorService);
                    }
                    break;

                    #endregion Page 4

                case 5: // Q7-16 - Locations Details

                    #region Page 5

                    if (!saveOnly)
                    {
                        if (Q6Tramonto.SelectedValue == 1)
                        {
                            if (!SurveyTools.CheckForAnswer(Q7A, true)
                            | !SurveyTools.CheckForAnswer(Q7B, true)
                            | !SurveyTools.CheckForAnswer(Q7C, true)
                            | !SurveyTools.CheckForAnswer(Q7D, true)
                            | !SurveyTools.CheckForAnswer(Q7E, true)
                            | !SurveyTools.CheckForAnswer(Q7F, true)
                            | !SurveyTools.CheckForAnswer(Q7G, true)
                            | !SurveyTools.CheckForAnswer(Q7H, true)
                            | !SurveyTools.CheckForAnswer(Q7I, true)
                            )
                            {
                                retVal = false;
                            }
                        }
                        if (Q6TheBuffet.SelectedValue == 1)
                        {
                            if (!SurveyTools.CheckForAnswer(Q8A, true)
                            | !SurveyTools.CheckForAnswer(Q8B, true)
                            | !SurveyTools.CheckForAnswer(Q8C, true)
                            | !SurveyTools.CheckForAnswer(Q8D, true)
                            | !SurveyTools.CheckForAnswer(Q8E, true)
                            | !SurveyTools.CheckForAnswer(Q8F, true)
                            )
                            {
                                retVal = false;
                            }
                        }
                        if (Q6Curve.SelectedValue == 1)
                        {
                            if (!SurveyTools.CheckForAnswer(Q9A, true)
                            | !SurveyTools.CheckForAnswer(Q9B, true)
                            | !SurveyTools.CheckForAnswer(Q9C, true)
                            | !SurveyTools.CheckForAnswer(Q9D, true)
                            | !SurveyTools.CheckForAnswer(Q9E, true)
                            | !SurveyTools.CheckForAnswer(Q9F, true)
                            | !SurveyTools.CheckForAnswer(Q9G, true)
                            | !SurveyTools.CheckForAnswer(Q9H, true)
                            | !SurveyTools.CheckForAnswer(Q9I, true)
                            )
                            {
                                retVal = false;
                            }
                        }
                        if (Q6InRoomDining.SelectedValue == 1)
                        {
                            if (!SurveyTools.CheckForAnswer(Q10A, true)
                            | !SurveyTools.CheckForAnswer(Q10B, true)
                            | !SurveyTools.CheckForAnswer(Q10C, true)
                            | !SurveyTools.CheckForAnswer(Q10D, true)
                            | !SurveyTools.CheckForAnswer(Q10E, true)
                            | !SurveyTools.CheckForAnswer(Q10F, true)
                            | !SurveyTools.CheckForAnswer(Q10G, true)
                            | !SurveyTools.CheckForAnswer(Q10H, true)
                            )
                            {
                                retVal = false;
                            }
                        }
                        if (Q6FitnessCenter.SelectedValue == 1)
                        {
                            if (!SurveyTools.CheckForAnswer(Q11A, true)
                            | !SurveyTools.CheckForAnswer(Q11B, true)
                            | !SurveyTools.CheckForAnswer(Q11C, true)
                            | !SurveyTools.CheckForAnswer(Q11D, true)
                            )
                            {
                                retVal = false;
                            }
                        }
                        if (Q6PoolHotTub.SelectedValue == 1)
                        {
                            if (!SurveyTools.CheckForAnswer(Q12A, true)
                            | !SurveyTools.CheckForAnswer(Q12B, true)
                            | !SurveyTools.CheckForAnswer(Q12C, true)
                            | !SurveyTools.CheckForAnswer(Q12D, true)
                            | !SurveyTools.CheckForAnswer(Q12E, true)
                            )
                            {
                                retVal = false;
                            }
                        }
                        if (Q6Meeting.SelectedValue == 1)
                        {
                            if (!SurveyTools.CheckForAnswer(txtQ13_MeetingDescription, true)
                                | !SurveyTools.CheckForAnswer(Q13A, true)
                                | !SurveyTools.CheckForAnswer(Q13B, true)
                                | !SurveyTools.CheckForAnswer(Q13C, true)
                                | !SurveyTools.CheckForAnswer(Q13D, true)
                                | !SurveyTools.CheckForAnswer(Q13E, true)
                                | !SurveyTools.CheckForAnswer(Q13F, true)
                                | !SurveyTools.CheckForAnswer(Q13G, true)
                            )
                            {
                                retVal = false;
                            }
                        }
                        if (Q6ValetParking.SelectedValue == 1)
                        {
                            if (!SurveyTools.CheckForAnswer(Q14A, true)
                            | !SurveyTools.CheckForAnswer(Q14B, true)
                            | !SurveyTools.CheckForAnswer(Q14C, true)
                            | !SurveyTools.CheckForAnswer(Q14D, true)
                            | !SurveyTools.CheckForAnswer(Q14E, true)
                            | !SurveyTools.CheckForAnswer(Q14F, true)
                            | !SurveyTools.CheckForAnswer(Q14G, true)
                            )
                            {
                                retVal = false;
                            }
                        }
                        if (Q6Concierge.SelectedValue == 1)
                        {
                            if (!SurveyTools.CheckForAnswer(Q15A, true)
                            | !SurveyTools.CheckForAnswer(Q15B, true)
                            | !SurveyTools.CheckForAnswer(Q15C, true)
                            | !SurveyTools.CheckForAnswer(Q15D, true)
                            | !SurveyTools.CheckForAnswer(Q15E, true)
                            )
                            {
                                retVal = false;
                            }
                        }
                        if (Q6BellDoorService.SelectedValue == 1)
                        {
                            if (!SurveyTools.CheckForAnswer(Q16A, true)
                            | !SurveyTools.CheckForAnswer(Q16B, true)
                            | !SurveyTools.CheckForAnswer(Q16C, true)
                            | !SurveyTools.CheckForAnswer(Q16D, true)
                            | !SurveyTools.CheckForAnswer(Q16E, true)
                            | !SurveyTools.CheckForAnswer(Q16F, true)
                            )
                            {
                                retVal = false;
                            }
                        }
                    }
                    if (currentPage)
                    {
                        SurveyTools.SaveValue<int>(Q7A);
                        SurveyTools.SaveValue<int>(Q7B);
                        SurveyTools.SaveValue<int>(Q7C);
                        SurveyTools.SaveValue<int>(Q7D);
                        SurveyTools.SaveValue<int>(Q7E);
                        SurveyTools.SaveValue<int>(Q7F);
                        SurveyTools.SaveValue<int>(Q7G);
                        SurveyTools.SaveValue<int>(Q7H);
                        SurveyTools.SaveValue<int>(Q7I);
                        SurveyTools.SaveValue<int>(Q8A);
                        SurveyTools.SaveValue<int>(Q8B);
                        SurveyTools.SaveValue<int>(Q8C);
                        SurveyTools.SaveValue<int>(Q8D);
                        SurveyTools.SaveValue<int>(Q8E);
                        SurveyTools.SaveValue<int>(Q8F);
                        SurveyTools.SaveValue<int>(Q9A);
                        SurveyTools.SaveValue<int>(Q9B);
                        SurveyTools.SaveValue<int>(Q9C);
                        SurveyTools.SaveValue<int>(Q9D);
                        SurveyTools.SaveValue<int>(Q9E);
                        SurveyTools.SaveValue<int>(Q9F);
                        SurveyTools.SaveValue<int>(Q9G);
                        SurveyTools.SaveValue<int>(Q9H);
                        SurveyTools.SaveValue<int>(Q9I);
                        SurveyTools.SaveValue<int>(Q10A);
                        SurveyTools.SaveValue<int>(Q10B);
                        SurveyTools.SaveValue<int>(Q10C);
                        SurveyTools.SaveValue<int>(Q10D);
                        SurveyTools.SaveValue<int>(Q10E);
                        SurveyTools.SaveValue<int>(Q10F);
                        SurveyTools.SaveValue<int>(Q10G);
                        SurveyTools.SaveValue<int>(Q10H);
                        SurveyTools.SaveValue<int>(Q11A);
                        SurveyTools.SaveValue<int>(Q11B);
                        SurveyTools.SaveValue<int>(Q11C);
                        SurveyTools.SaveValue<int>(Q11D);
                        SurveyTools.SaveValue<int>(Q12A);
                        SurveyTools.SaveValue<int>(Q12B);
                        SurveyTools.SaveValue<int>(Q12C);
                        SurveyTools.SaveValue<int>(Q12D);
                        SurveyTools.SaveValue<int>(Q12E);
                        SurveyTools.SaveValue<string>(txtQ13_MeetingDescription);
                        SurveyTools.SaveValue<int>(Q13A);
                        SurveyTools.SaveValue<int>(Q13B);
                        SurveyTools.SaveValue<int>(Q13C);
                        SurveyTools.SaveValue<int>(Q13D);
                        SurveyTools.SaveValue<int>(Q13E);
                        SurveyTools.SaveValue<int>(Q13F);
                        SurveyTools.SaveValue<int>(Q13G);
                        SurveyTools.SaveValue<int>(Q14A);
                        SurveyTools.SaveValue<int>(Q14B);
                        SurveyTools.SaveValue<int>(Q14C);
                        SurveyTools.SaveValue<int>(Q14D);
                        SurveyTools.SaveValue<int>(Q14E);
                        SurveyTools.SaveValue<int>(Q14F);
                        SurveyTools.SaveValue<int>(Q14G);
                        SurveyTools.SaveValue<int>(Q15A);
                        SurveyTools.SaveValue<int>(Q15B);
                        SurveyTools.SaveValue<int>(Q15C);
                        SurveyTools.SaveValue<int>(Q15D);
                        SurveyTools.SaveValue<int>(Q15E);
                        SurveyTools.SaveValue<int>(Q16A);
                        SurveyTools.SaveValue<int>(Q16B);
                        SurveyTools.SaveValue<int>(Q16C);
                        SurveyTools.SaveValue<int>(Q16D);
                        SurveyTools.SaveValue<int>(Q16E);
                        SurveyTools.SaveValue<int>(Q16F);
                    }
                    break;

                    #endregion Page 5

                case 6: // Q17-23

                    #region Page 6

                    if (!saveOnly)
                    {
                        if (!SurveyTools.CheckForAnswer(Q17A, true)
                            | !SurveyTools.CheckForAnswer(Q17B, true)
                            | !SurveyTools.CheckForAnswer(Q17C, true)
                            | !SurveyTools.CheckForAnswer(Q18A, true)
                            | !SurveyTools.CheckForAnswer(Q18B, true)
                            | !SurveyTools.CheckForAnswer(Q19, true)
                            | !SurveyTools.CheckForAnswer(Q20, true)
                            | !SurveyTools.CheckForAnswer(Q21, true)
                            | (Q21.SelectedValue == 1 && !SurveyTools.CheckForAnswer(txtQ21Explanation, true))
                            | !SurveyTools.CheckForAnswer(Q22, true)
                            | !SurveyTools.CheckForAnswer(Q23, true))
                        {
                            retVal = false;
                        }
                    }
                    if (currentPage)
                    {
                        SurveyTools.SaveValue<int>(Q17A);
                        SurveyTools.SaveValue<int>(Q17B);
                        SurveyTools.SaveValue<int>(Q17C);
                        SurveyTools.SaveValue<int>(Q18A);
                        SurveyTools.SaveValue<int>(Q18B);
                        SurveyTools.SaveValue<int>(Q19);
                        SurveyTools.SaveValue<int>(Q20);
                        SurveyTools.SaveValue<int>(Q21);
                        SurveyTools.SaveValue<string>(txtQ21Explanation);
                        SurveyTools.SaveValue<int>(Q22);
                        SurveyTools.SaveValue<int>(Q23);
                    }

                    break;

                    #endregion Page 6

                case 7: // Q24

                    #region Page 7

                    if (currentPage)
                    {
                        SurveyTools.SaveValue<bool>(chkQ24A_1);
                        SurveyTools.SaveValue<bool>(chkQ24A_2);
                        SurveyTools.SaveValue<bool>(chkQ24A_3);
                        SurveyTools.SaveValue<bool>(chkQ24A_4);
                        SurveyTools.SaveValue<bool>(chkQ24A_5);
                        SurveyTools.SaveValue<bool>(chkQ24A_6);
                        SurveyTools.SaveValue<bool>(chkQ24A_7);
                        SurveyTools.SaveValue<bool>(chkQ24A_8);
                        SurveyTools.SaveValue<string>(txtQ24A_OtherExplanation);
                        SurveyTools.SaveValue<string>(Q24B);
                        SurveyTools.SaveValue<int>(Q24C);
                        SurveyTools.SaveValue<int>(Q24D);
                        SurveyTools.SaveValue<int>(Q24E_1);
                        SurveyTools.SaveValue<int>(Q24E_2);
                        SurveyTools.SaveValue<int>(Q24E_3);
                        SurveyTools.SaveValue<int>(Q24E_4);
                        SurveyTools.SaveValue<int>(Q24E_5);
                    }
                    if (!saveOnly)
                    {
                        if (Q23.SelectedValue == 1)
                        {
                            bool noneSelected = !SurveyTools.GetValue(chkQ24A_1, currentPage, false) &&
                                                !SurveyTools.GetValue(chkQ24A_2, currentPage, false) &&
                                                !SurveyTools.GetValue(chkQ24A_3, currentPage, false) &&
                                                !SurveyTools.GetValue(chkQ24A_4, currentPage, false) &&
                                                !SurveyTools.GetValue(chkQ24A_5, currentPage, false) &&
                                                !SurveyTools.GetValue(chkQ24A_6, currentPage, false) &&
                                                !SurveyTools.GetValue(chkQ24A_7, currentPage, false) &&
                                                !SurveyTools.GetValue(chkQ24A_8, currentPage, false);

                            if (noneSelected)
                            {
                                mmQ24A.ErrorMessage = "Please select one of the following options.";
                                retVal = false;
                            }

                            if (!SurveyTools.CheckForAnswer(Q24B, true)
                                | !SurveyTools.CheckForAnswer(Q24C, true)
                                | !SurveyTools.CheckForAnswer(Q24D, true)
                                | !SurveyTools.CheckForAnswer(Q24E_1, true)
                                | !SurveyTools.CheckForAnswer(Q24E_2, true)
                                | !SurveyTools.CheckForAnswer(Q24E_3, true)
                                | !SurveyTools.CheckForAnswer(Q24E_4, true)
                                | !SurveyTools.CheckForAnswer(Q24E_5, true)
                                | (chkQ24A_8.Checked && !SurveyTools.CheckForAnswer(txtQ24A_OtherExplanation, true)))
                            {
                                retVal = false;
                            }
                        }
                    }
                    break;

                    #endregion Page 7

                case 8: // Q25-28

                    #region Page 8

                    if (currentPage)
                    {
                        SurveyTools.SaveRadioButtons(Q25_1, Q25_2, Q25_3, Q25_4, Q25_5, Q25_6, Q25_7, Q25_8, Q25_9, Q25_10);
                        SurveyTools.SaveValue(Q25_OtherExplanation);
                        SurveyTools.SaveValue(Q26_1);
                        SurveyTools.SaveValue(Q26_2);
                        SurveyTools.SaveValue(Q26_3);
                        SurveyTools.SaveValue(Q26_4);
                        SurveyTools.SaveValue(Q26_OtherExplanation);
                        SurveyTools.SaveValue(Q27);
                        SurveyTools.SaveValue(Q28);
                        SurveyTools.SaveValue(Q29);
                    }
                    if (!saveOnly)
                    {
                        bool noneSelected = !SurveyTools.GetValue(Q25_1, currentPage, false) &&
                                            !SurveyTools.GetValue(Q25_2, currentPage, false) &&
                                            !SurveyTools.GetValue(Q25_3, currentPage, false) &&
                                            !SurveyTools.GetValue(Q25_4, currentPage, false) &&
                                            !SurveyTools.GetValue(Q25_5, currentPage, false) &&
                                            !SurveyTools.GetValue(Q25_6, currentPage, false) &&
                                            !SurveyTools.GetValue(Q25_7, currentPage, false) &&
                                            !SurveyTools.GetValue(Q25_8, currentPage, false) &&
                                            !SurveyTools.GetValue(Q25_9, currentPage, false) &&
                                            !SurveyTools.GetValue(Q25_10, currentPage, false);

                        if (noneSelected)
                        {
                            mmQ25.ErrorMessage = "Please indicate your primary reason for choosing River Rock Casino Resort.";
                            retVal = false;
                        }

                        if (Q25_10.Checked && !SurveyTools.CheckForAnswer(Q25_OtherExplanation, true))
                        {
                            retVal = false;
                        }

                        noneSelected = !SurveyTools.GetValue(Q26_1, currentPage, false) &&
                                        !SurveyTools.GetValue(Q26_2, currentPage, false) &&
                                        !SurveyTools.GetValue(Q26_3, currentPage, false) &&
                                        !SurveyTools.GetValue(Q26_4, currentPage, false);

                        if (noneSelected)
                        {
                            mmQ26.ErrorMessage = "Please indicate the primary purpose of your visit.";
                            retVal = false;
                        }

                        if (Q26_4.Checked && !SurveyTools.CheckForAnswer(Q26_OtherExplanation, true))
                        {
                            retVal = false;
                        }

                        if (!SurveyTools.CheckForAnswer(Q27, true))
                        {
                            retVal = false;
                        }

                        if (!SurveyTools.CheckForAnswer(Q29, true))
                        {
                            retVal = false;
                        }
                    }

                    break;

                    #endregion Page 8

                case 9: //Contact info

                    #region Page 9

                    if (Q29.SelectedValue == 1)
                    {
                        if (!saveOnly)
                        {
                            if (!SurveyTools.CheckForAnswer(txtFirstName, true)
                            | !SurveyTools.CheckForAnswer(txtLastName, true)
                            | !SurveyTools.CheckForAnswer(txtFollowupReason, true)
                            )
                            {
                                retVal = false;
                            }

                            email = SurveyTools.GetValue(txtEmail2, currentPage, String.Empty);
                            if (!Validation.RegExCheck(email, ValidationType.Email))
                            {
                                txtEmail2.MessageManager.ErrorMessage = "Please enter a valid email address.";
                                retVal = false;
                            }
                        }
                        if (currentPage)
                        {
                            SurveyTools.SaveValue(txtFirstName);
                            SurveyTools.SaveValue(txtLastName);
                            SurveyTools.SaveValue(txtEmail2);
                            SurveyTools.SaveValue(txtTelephoneNumber);
                            SurveyTools.SaveValue(txtFollowupReason);
                        }
                    }

                    #endregion Page 9

                    break;
            }

            return retVal;
        }

        private bool PageShouldBeSkipped(int CurrentPage)
        {
            switch (CurrentPage)
            {
                case 5:
                    return (Q6Tramonto.SelectedValue != 1
                            && Q6TheBuffet.SelectedValue != 1
                            && Q6Curve.SelectedValue != 1
                            && Q6InRoomDining.SelectedValue != 1
                            && Q6FitnessCenter.SelectedValue != 1
                            && Q6PoolHotTub.SelectedValue != 1
                            && Q6Meeting.SelectedValue != 1
                            && Q6ValetParking.SelectedValue != 1
                            && Q6Concierge.SelectedValue != 1
                            && Q6BellDoorService.SelectedValue != 1);

                case 7:
                    return Q23.SelectedValue != 1;

                case 9:
                    return Q29.SelectedValue != 1;
            }
            return false;
        }

        protected string GetURL(int page, int redirDir)
        {
            bool isReset = (page == -1);
            if (isReset)
            {
                page = 1;
            }
            return String.Format("/HotelSurvey/{0}/{1}{2}{3}", Master.PropertyShortCode.ToString(), page, (redirDir == -1 ? "/-1" : String.Empty), (isReset ? "?r=1" : String.Empty));
        }

        protected bool SaveData(out int rowID)
        {
            StringBuilder columnList = new StringBuilder();
            SQLParamList sqlParams = new SQLParamList();

            txtEmail.PrepareQuestionForDB(columnList, sqlParams);
            Q1Overall.PrepareQuestionForDB(columnList, sqlParams);
            Q1A.PrepareQuestionForDB(columnList, sqlParams);
            Q1B.PrepareQuestionForDB(columnList, sqlParams);
            Q1C.PrepareQuestionForDB(columnList, sqlParams);
            Q1D.PrepareQuestionForDB(columnList, sqlParams);
            Q1E.PrepareQuestionForDB(columnList, sqlParams);
            Q1F.PrepareQuestionForDB(columnList, sqlParams);
            Q2.PrepareQuestionForDB(columnList, sqlParams);
            Q3A.PrepareQuestionForDB(columnList, sqlParams);
            Q3B.PrepareQuestionForDB(columnList, sqlParams);
            Q3C.PrepareQuestionForDB(columnList, sqlParams);
            Q3D.PrepareQuestionForDB(columnList, sqlParams);
            Q3E.PrepareQuestionForDB(columnList, sqlParams);
            Q3F.PrepareQuestionForDB(columnList, sqlParams);
            Q3G.PrepareQuestionForDB(columnList, sqlParams);
            Q3H.PrepareQuestionForDB(columnList, sqlParams);
            Q3I.PrepareQuestionForDB(columnList, sqlParams);
            Q3J.PrepareQuestionForDB(columnList, sqlParams);
            Q4A.PrepareQuestionForDB(columnList, sqlParams);
            Q4B.PrepareQuestionForDB(columnList, sqlParams);
            Q4C.PrepareQuestionForDB(columnList, sqlParams);
            Q5A.PrepareQuestionForDB(columnList, sqlParams);
            Q5B.PrepareQuestionForDB(columnList, sqlParams);
            Q5C.PrepareQuestionForDB(columnList, sqlParams);
            Q5D.PrepareQuestionForDB(columnList, sqlParams);
            txtQ5Amenities.PrepareQuestionForDB(columnList, sqlParams);
            Q6Tramonto.PrepareQuestionForDB(columnList, sqlParams);
            Q6TheBuffet.PrepareQuestionForDB(columnList, sqlParams);
            Q6Curve.PrepareQuestionForDB(columnList, sqlParams);
            Q6InRoomDining.PrepareQuestionForDB(columnList, sqlParams);
            Q6FitnessCenter.PrepareQuestionForDB(columnList, sqlParams);
            Q6PoolHotTub.PrepareQuestionForDB(columnList, sqlParams);
            Q6Meeting.PrepareQuestionForDB(columnList, sqlParams);
            Q6ValetParking.PrepareQuestionForDB(columnList, sqlParams);
            Q6Concierge.PrepareQuestionForDB(columnList, sqlParams);
            Q6BellDoorService.PrepareQuestionForDB(columnList, sqlParams);
            Q7A.PrepareQuestionForDB(columnList, sqlParams);
            Q7B.PrepareQuestionForDB(columnList, sqlParams);
            Q7C.PrepareQuestionForDB(columnList, sqlParams);
            Q7D.PrepareQuestionForDB(columnList, sqlParams);
            Q7E.PrepareQuestionForDB(columnList, sqlParams);
            Q7F.PrepareQuestionForDB(columnList, sqlParams);
            Q7G.PrepareQuestionForDB(columnList, sqlParams);
            Q7H.PrepareQuestionForDB(columnList, sqlParams);
            Q7I.PrepareQuestionForDB(columnList, sqlParams);
            Q8A.PrepareQuestionForDB(columnList, sqlParams);
            Q8B.PrepareQuestionForDB(columnList, sqlParams);
            Q8C.PrepareQuestionForDB(columnList, sqlParams);
            Q8D.PrepareQuestionForDB(columnList, sqlParams);
            Q8E.PrepareQuestionForDB(columnList, sqlParams);
            Q8F.PrepareQuestionForDB(columnList, sqlParams);
            Q9A.PrepareQuestionForDB(columnList, sqlParams);
            Q9B.PrepareQuestionForDB(columnList, sqlParams);
            Q9C.PrepareQuestionForDB(columnList, sqlParams);
            Q9D.PrepareQuestionForDB(columnList, sqlParams);
            Q9E.PrepareQuestionForDB(columnList, sqlParams);
            Q9F.PrepareQuestionForDB(columnList, sqlParams);
            Q9G.PrepareQuestionForDB(columnList, sqlParams);
            Q9H.PrepareQuestionForDB(columnList, sqlParams);
            Q9I.PrepareQuestionForDB(columnList, sqlParams);
            Q10A.PrepareQuestionForDB(columnList, sqlParams);
            Q10B.PrepareQuestionForDB(columnList, sqlParams);
            Q10C.PrepareQuestionForDB(columnList, sqlParams);
            Q10D.PrepareQuestionForDB(columnList, sqlParams);
            Q10E.PrepareQuestionForDB(columnList, sqlParams);
            Q10F.PrepareQuestionForDB(columnList, sqlParams);
            Q10G.PrepareQuestionForDB(columnList, sqlParams);
            Q10H.PrepareQuestionForDB(columnList, sqlParams);
            Q11A.PrepareQuestionForDB(columnList, sqlParams);
            Q11B.PrepareQuestionForDB(columnList, sqlParams);
            Q11C.PrepareQuestionForDB(columnList, sqlParams);
            Q11D.PrepareQuestionForDB(columnList, sqlParams);
            Q12A.PrepareQuestionForDB(columnList, sqlParams);
            Q12B.PrepareQuestionForDB(columnList, sqlParams);
            Q12C.PrepareQuestionForDB(columnList, sqlParams);
            Q12D.PrepareQuestionForDB(columnList, sqlParams);
            Q12E.PrepareQuestionForDB(columnList, sqlParams);
            txtQ13_MeetingDescription.PrepareQuestionForDB(columnList, sqlParams);
            Q13A.PrepareQuestionForDB(columnList, sqlParams);
            Q13B.PrepareQuestionForDB(columnList, sqlParams);
            Q13C.PrepareQuestionForDB(columnList, sqlParams);
            Q13D.PrepareQuestionForDB(columnList, sqlParams);
            Q13E.PrepareQuestionForDB(columnList, sqlParams);
            Q13F.PrepareQuestionForDB(columnList, sqlParams);
            Q13G.PrepareQuestionForDB(columnList, sqlParams);
            Q14A.PrepareQuestionForDB(columnList, sqlParams);
            Q14B.PrepareQuestionForDB(columnList, sqlParams);
            Q14C.PrepareQuestionForDB(columnList, sqlParams);
            Q14D.PrepareQuestionForDB(columnList, sqlParams);
            Q14E.PrepareQuestionForDB(columnList, sqlParams);
            Q14F.PrepareQuestionForDB(columnList, sqlParams);
            Q14G.PrepareQuestionForDB(columnList, sqlParams);
            Q15A.PrepareQuestionForDB(columnList, sqlParams);
            Q15B.PrepareQuestionForDB(columnList, sqlParams);
            Q15C.PrepareQuestionForDB(columnList, sqlParams);
            Q15D.PrepareQuestionForDB(columnList, sqlParams);
            Q15E.PrepareQuestionForDB(columnList, sqlParams);
            Q16A.PrepareQuestionForDB(columnList, sqlParams);
            Q16B.PrepareQuestionForDB(columnList, sqlParams);
            Q16C.PrepareQuestionForDB(columnList, sqlParams);
            Q16D.PrepareQuestionForDB(columnList, sqlParams);
            Q16E.PrepareQuestionForDB(columnList, sqlParams);
            Q16F.PrepareQuestionForDB(columnList, sqlParams);
            Q17A.PrepareQuestionForDB(columnList, sqlParams);
            Q17B.PrepareQuestionForDB(columnList, sqlParams);
            Q17C.PrepareQuestionForDB(columnList, sqlParams);
            Q18A.PrepareQuestionForDB(columnList, sqlParams);
            Q18B.PrepareQuestionForDB(columnList, sqlParams);
            Q19.PrepareQuestionForDB(columnList, sqlParams);
            Q20.PrepareQuestionForDB(columnList, sqlParams);
            Q21.PrepareQuestionForDB(columnList, sqlParams);
            txtQ21Explanation.PrepareQuestionForDB(columnList, sqlParams);
            Q22.PrepareQuestionForDB(columnList, sqlParams);
            Q23.PrepareQuestionForDB(columnList, sqlParams);
            chkQ24A_1.PrepareQuestionForDB(columnList, sqlParams);
            chkQ24A_2.PrepareQuestionForDB(columnList, sqlParams);
            chkQ24A_3.PrepareQuestionForDB(columnList, sqlParams);
            chkQ24A_4.PrepareQuestionForDB(columnList, sqlParams);
            chkQ24A_5.PrepareQuestionForDB(columnList, sqlParams);
            chkQ24A_6.PrepareQuestionForDB(columnList, sqlParams);
            chkQ24A_7.PrepareQuestionForDB(columnList, sqlParams);
            chkQ24A_8.PrepareQuestionForDB(columnList, sqlParams);
            txtQ24A_OtherExplanation.PrepareQuestionForDB(columnList, sqlParams);
            Q24B.PrepareQuestionForDB(columnList, sqlParams);
            Q24C.PrepareQuestionForDB(columnList, sqlParams);
            Q24D.PrepareQuestionForDB(columnList, sqlParams);
            Q24E_1.PrepareQuestionForDB(columnList, sqlParams);
            Q24E_2.PrepareQuestionForDB(columnList, sqlParams);
            Q24E_3.PrepareQuestionForDB(columnList, sqlParams);
            Q24E_4.PrepareQuestionForDB(columnList, sqlParams);
            Q24E_5.PrepareQuestionForDB(columnList, sqlParams);
            Q25_1.PrepareQuestionForDB(columnList, sqlParams);
            Q25_2.PrepareQuestionForDB(columnList, sqlParams);
            Q25_3.PrepareQuestionForDB(columnList, sqlParams);
            Q25_4.PrepareQuestionForDB(columnList, sqlParams);
            Q25_5.PrepareQuestionForDB(columnList, sqlParams);
            Q25_6.PrepareQuestionForDB(columnList, sqlParams);
            Q25_7.PrepareQuestionForDB(columnList, sqlParams);
            Q25_8.PrepareQuestionForDB(columnList, sqlParams);
            Q25_9.PrepareQuestionForDB(columnList, sqlParams);
            Q25_10.PrepareQuestionForDB(columnList, sqlParams);
            Q25_OtherExplanation.PrepareQuestionForDB(columnList, sqlParams);
            Q26_1.PrepareQuestionForDB(columnList, sqlParams);
            Q26_2.PrepareQuestionForDB(columnList, sqlParams);
            Q26_3.PrepareQuestionForDB(columnList, sqlParams);
            Q26_4.PrepareQuestionForDB(columnList, sqlParams);
            Q26_OtherExplanation.PrepareQuestionForDB(columnList, sqlParams);
            Q27.PrepareQuestionForDB(columnList, sqlParams);
            Q28.PrepareQuestionForDB(columnList, sqlParams);
            Q29.PrepareQuestionForDB(columnList, sqlParams);
            txtFirstName.PrepareQuestionForDB(columnList, sqlParams);
            txtLastName.PrepareQuestionForDB(columnList, sqlParams);
            txtEmail2.PrepareQuestionForDB(columnList, sqlParams);
            txtTelephoneNumber.PrepareQuestionForDB(columnList, sqlParams);
            txtFollowupReason.PrepareQuestionForDB(columnList, sqlParams);

            columnList.Append(",[DateEntered]");
            sqlParams.Add("@DateEntered", DateTime.Now);

            columnList.Remove(0, 1);
            SQLDatabase sql = new SQLDatabase();
            rowID = sql.QueryAndReturnIdentity(String.Format("INSERT INTO [tblSurveyHotel] ({0}) VALUES ({1});", columnList, columnList.ToString().Replace("[", "@").Replace("]", String.Empty)), sqlParams);
            if (!sql.HasError && rowID != -1)
            {
                Dictionary<string, int> wordCounts = SurveyTools.GetWordCount(txtQ5Amenities.Text, txtQ13_MeetingDescription.Text, txtQ21Explanation.Text, txtQ24A_OtherExplanation.Text, Q24B.Text, Q25_OtherExplanation.Text, Q28.Text, txtFollowupReason.Text);
                SurveyTools.SaveWordCounts(SharedClasses.SurveyType.Hotel, rowID, wordCounts);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}