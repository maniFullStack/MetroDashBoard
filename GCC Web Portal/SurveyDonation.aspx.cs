using SharedClasses;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class SurveyDonation : BasePage
    {
        protected bool SurveyComplete { get; set; }
        private string GCCPortalUrl = ConfigurationManager.AppSettings["GCCPortalURL"].ToString();

        protected override void InitializeCulture()
        {
            if (Session["CurrentUI"] != null)
            {
                String selectedLanguage = (string)Session["CurrentUI"];
                UICulture = selectedLanguage;
                Culture = selectedLanguage;

                Thread.CurrentThread.CurrentCulture =
                    CultureInfo.CreateSpecificCulture(selectedLanguage);
                Thread.CurrentThread.CurrentUICulture = new
                    CultureInfo(selectedLanguage);
            }

            base.InitializeCulture();
        }

        protected void EnglishLinkButton_Click(object sender, EventArgs e)
        {
            Session["CurrentUI"] = "en-CA";
            Response.Redirect(Request.Url.OriginalString);




        }

        protected void FrenchLinkButton_Click(object sender, EventArgs e)
        {
            Session["CurrentUI"] = "fr-CA";
            Response.Redirect(Request.Url.OriginalString);



        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if ((new GCCPropertyShortCode[] { GCCPropertyShortCode.CNB }).Contains(Master.PropertyShortCode) || (new GCCPropertyShortCode[] { GCCPropertyShortCode.SCTI }).Contains(Master.PropertyShortCode) || (new GCCPropertyShortCode[] { GCCPropertyShortCode.WDB }).Contains(Master.PropertyShortCode))
            {
                btnEnglish.Visible = true;
                btnFrench.Visible = true;
                if (Session["CurrentUI"] == null)
                {
                    Session["CurrentUI"] = "en-CA";
                }
            }
            else
            {
                btnEnglish.Visible = false;
                btnFrench.Visible = false;
                Session["CurrentUI"] = "en-CA";
            }



            //if (Session["CurrentUI"].ToString() == "fr-CA")
            //{
            //    Title = "Donation Application Form &raquo; " + Master.CasinoNameFrench;
            //}
            //else
            //{
            //    Title = "Formulaire de demande de dons &raquo; " + Master.CasinoName;

            //};

            
            //Check all previous pages
            //Must do in LoadComplete because controls load values in Load method (Init didn't work because reasons...)
            if (Master.CurrentPage > 1 && !IsPostBack)
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
                    if (Master.CurrentPage == 4 && Master.RedirectDirection == 1)
                    {
                        nextPage = 99;
                    }
                    Response.Redirect(GetURL(nextPage, Master.RedirectDirection), true);
                    return;
                }
                //If we've made it to 99, save to database.
                if (Master.CurrentPage == 99 && !IsPostBack)
                {
                    int surveyID;
                    if (SaveData(out surveyID))
                    {
                        SendNotifications(surveyID);
                        SurveyComplete = true;
                        Session.Abandon();
                        if (Session["CurrentUI"].ToString() == "fr-CA")
                        {
                            mmLastPage.FrSuccessMessage = "Merci d’avoir donné vos impressions! Vos réponses ont été transférées à un représentant. Si vous avez demandé une réponse de notre part, veuillez attendre de nos nouvelles d’ici 12 à 24 heures. Veuillez vérifier votre dossier de pourriels ou ajouter « @gcgamingsurvey.com » à votre carnet d’adresses.";
                        }
                        else
                        {
                            mmLastPage.SuccessMessage = "Thank you for your feedback! Your responses have been forwarded to the appropriate representative. If you have requested a response, please expect contact within 12-24 hours. Please ensure you check your \"Junk Mail\" folder or add \"@gcgamingsurvey.com\" to your email account's white list.";
                        }
                    }
                    else
                    {
                        mmLastPage.ErrorMessage = "We were unable to save your responses. Please go back and try again.";
                    }
                }
            }
        }

        private void SendNotifications(int surveyID)
        {
            //Add the feedback
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            SqlParameter feedbackUIDParam = new SqlParameter("@UID", System.Data.SqlDbType.UniqueIdentifier);
            feedbackUIDParam.Direction = System.Data.ParameterDirection.Output;

            sql.ExecStoredProcedureDataSet("spFeedback_Create",
                new SQLParamList()
                        .Add("@PropertyID", (int)Master.PropertyShortCode)
                        .Add("@SurveyTypeID", (int)SurveyType.Donation)
                        .Add("@RecordID", surveyID)
                        .Add("@ReasonID", (int)NotificationReason.SponsorshipRequest)
                        .Add(feedbackUIDParam)
            );

            //Notify the location
            SurveyTools.SendNotifications(
                Server,
                Master.PropertyShortCode,
                SharedClasses.SurveyType.Donation,
                NotificationReason.SponsorshipRequest,
                string.Empty,
                new
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    CasinoName = Master.CasinoName,
                    FeedbackLink = GCCPortalUrl + "Admin/Feedback/" + feedbackUIDParam.Value.ToString(),
                    Link = GCCPortalUrl + "Display/Donation/" + surveyID
                });
        }

        protected void Prev_Click(object sender, EventArgs e)
        {
            if (ValidateAndSave(Master.CurrentPage, true, true))
            {
                int prevPage = Master.CurrentPage - 1;
                if (Master.CurrentPage == 99)
                {
                    prevPage = 2;
                }
                Response.Redirect(GetURL(prevPage, -1), true);
            }
        }

        protected void Next_Click(object sender, EventArgs e)
        {
            if (ValidateAndSave(Master.CurrentPage, true, false))
            {
                if (Master.CurrentPage == 99)
                {
                    Response.Redirect(PropertyTools.GetCasinoURL(Master.PropertyShortCode), true);
                    return;
                }

                int nextPage = Master.CurrentPage + 1;
                if (nextPage > 2)
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
                case 1:

                    #region Page 1

                    if (currentPage)
                    {
                        SurveyTools.SaveValue<string>(Q1);
                        SurveyTools.SaveValue<string>(Q1);

                        if (Session["CurrentUI"].ToString() == "fr-CA")
                        {
                            SurveyTools.SaveValue<int>(Q2_F);
                        }
                        else
                        {
                            SurveyTools.SaveValue<int>(Q2);
                        }


                        SurveyTools.SaveValue<string>(Q3);
                        SurveyTools.SaveValue<string>(Q4);
                        SurveyTools.SaveValue<string>(Q5Name);
                        SurveyTools.SaveValue<string>(Q5Title);
                        SurveyTools.SaveValue<string>(Q5Telephone);
                        SurveyTools.SaveValue<string>(Q5Email);
                        SurveyTools.SaveValue<string>(Q6Street);
                        SurveyTools.SaveValue<string>(Q6City);
                        SurveyTools.SaveValue<string>(Q6Province);
                        SurveyTools.SaveValue<string>(Q6PostalCode);
                        SurveyTools.SaveValue<string>(Q7);
                        SurveyTools.SaveValue<string>(Q8);
                        SurveyTools.SaveRadioButtons(Q9A, Q9B, Q9C);
                        SurveyTools.SaveValue<string>(Q9C_Explanation);
                        SurveyTools.SaveValue<string>(Q10);
                        SurveyTools.SaveValue<bool>(Q11A_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11A_CurrentRequest);
                        SurveyTools.SaveValue<bool>(Q11B_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11B_CurrentRequest);
                        SurveyTools.SaveValue<bool>(Q11C_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11C_CurrentRequest);
                        //SurveyTools.SaveValue<bool>( Q11D_PastSupport );
                        //SurveyTools.SaveValue<bool>( Q11D_CurrentRequest );
                        SurveyTools.SaveValue<bool>(Q11E_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11E_CurrentRequest);
                        SurveyTools.SaveValue<bool>(Q11F_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11F_CurrentRequest);
                        SurveyTools.SaveValue<bool>(Q11G_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11G_CurrentRequest);
                        SurveyTools.SaveValue<bool>(Q11H_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11H_CurrentRequest);
                        SurveyTools.SaveValue<bool>(Q11I_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11I_CurrentRequest);
                        SurveyTools.SaveValue<bool>(Q11J_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11J_CurrentRequest);
                        SurveyTools.SaveValue<bool>(Q11K_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11K_CurrentRequest);
                        SurveyTools.SaveValue<bool>(Q11L_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11L_CurrentRequest);
                        SurveyTools.SaveValue<bool>(Q11M_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11M_CurrentRequest);

                        //2017-11-14 aDDING 3 NEW LOCATIONS
                        SurveyTools.SaveValue<bool>(Q11N_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11N_CurrentRequest);


                        SurveyTools.SaveValue<bool>(Q11O_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11O_CurrentRequest);


                        SurveyTools.SaveValue<bool>(Q11P_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11P_CurrentRequest);


                        SurveyTools.SaveValue<bool>(Q11Q_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11Q_CurrentRequest);


                        SurveyTools.SaveValue<bool>(Q11R_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11R_CurrentRequest);


                        SurveyTools.SaveValue<bool>(Q11S_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11S_CurrentRequest);


                        SurveyTools.SaveValue<bool>(Q11T_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11T_CurrentRequest);


                        SurveyTools.SaveValue<bool>(Q11U_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11U_CurrentRequest);



                        SurveyTools.SaveValue<bool>(Q11V_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11V_CurrentRequest);

                        SurveyTools.SaveValue<bool>(Q11W_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11W_CurrentRequest);

                        SurveyTools.SaveValue<bool>(Q11X_PastSupport);
                        SurveyTools.SaveValue<bool>(Q11X_CurrentRequest);


                        SurveyTools.SaveValue<string>(Q12);
                        SurveyTools.SaveValue<string>(Q13);
                        SurveyTools.SaveValue<string>(Q14);
                    }

                    if (!saveOnly)
                    {
                        if (!SurveyTools.CheckForAnswer(Q1, true)
                            | !SurveyTools.CheckForAnswer(Q2, true)
                            | !SurveyTools.CheckForAnswer(Q5Name, true)
                            | !SurveyTools.CheckForAnswer(Q5Title, true)
                            | !SurveyTools.CheckForAnswer(Q5Telephone, true)
                            | !SurveyTools.CheckForAnswer(Q5Email, true)
                            | !SurveyTools.CheckForAnswer(Q6Street, true)
                            | !SurveyTools.CheckForAnswer(Q6City, true)
                            | !SurveyTools.CheckForAnswer(Q6Province, true)
                            | !SurveyTools.CheckForAnswer(Q6PostalCode, true)
                            | !SurveyTools.CheckForAnswer(Q7, true)
                            | !SurveyTools.CheckForAnswer(Q10, true)
                            | !SurveyTools.CheckForAnswer(Q13, true)
                            | !SurveyTools.CheckForAnswer(Q14, true)
                            )
                        {
                            retVal = false;
                        }

                        bool noneSelected = !SurveyTools.GetValue(Q9A, currentPage, false) &&
                                            !SurveyTools.GetValue(Q9B, currentPage, false) &&
                                            !SurveyTools.GetValue(Q9C, currentPage, false);
                        if (noneSelected)
                        {
                            mmQ9.ErrorMessage = "Please select one of the following values.";
                            retVal = false;
                        }

                        string email = SurveyTools.GetValue(Q5Email, currentPage, String.Empty);
                        if (!Validation.RegExCheck(email, ValidationType.Email))
                        {
                            Q5Email.MessageManager.ErrorMessage = "Please enter a valid email address.";
                            return false;
                        }

                        string postcode = SurveyTools.GetValue(Q6PostalCode, currentPage, String.Empty);
                        if (!Validation.RegExCheck(postcode, ValidationType.Postal))
                        {
                            Q6PostalCode.MessageManager.ErrorMessage = "Please enter a valid postal code in the format 'A1A 1A1'.";
                            return false;
                        }


                        if (Session["CurrentUI"].ToString() == "fr-CA")
                        {
                            if ((Q2_F.SelectedValue_F == 1 && !SurveyTools.CheckForAnswer(Q3, true))
                                || (Q2_F.SelectedValue_F == 0 && !SurveyTools.CheckForAnswer(Q4, true)))
                            {
                                retVal = false;
                            }
                        }
                        else
                        {
                            if ((Q2.SelectedValue == 1 && !SurveyTools.CheckForAnswer(Q3, true))
                                || (Q2.SelectedValue == 0 && !SurveyTools.CheckForAnswer(Q4, true)))
                            {
                                retVal = false;
                            }
                        }
                        if (Q9C.Checked && !SurveyTools.CheckForAnswer(Q9C_Explanation, true))
                        {
                            retVal = false;
                        }
                    }
                    break;

                    #endregion Page 1

                case 2: // Final confirm

                    #region Page 2

                    break;

                    #endregion Page 2
            }
            return retVal;
        }

        private bool PageShouldBeSkipped(int CurrentPage)
        {
            //switch ( CurrentPage ) {
            //    case 3:
            //        return fbkQ5.SelectedValue.Equals( "I do not want to be contacted" );
            //}
            return false;
        }

        protected string GetURL(int page, int redirDir)
        {
            bool isReset = (page == -1);
            if (isReset)
            {
                page = 1;
            }
            return String.Format("/DonationRequest/{0}/{1}{2}{3}", Master.PropertyShortCode.ToString(), page, (redirDir == -1 ? "/-1" : String.Empty), (isReset ? "?r=1" : String.Empty));
        }

        protected bool SaveData(out int rowID)
        {
            StringBuilder columnList = new StringBuilder();
            SQLParamList sqlParams = new SQLParamList();

            Q1.PrepareQuestionForDB(columnList, sqlParams);
            if (Session["CurrentUI"].ToString() == "fr-CA")
            {
                Q2_F.PrepareQuestionForDB(columnList, sqlParams);
            }
            else
            {
                Q2.PrepareQuestionForDB(columnList, sqlParams);
            }
            Q3.PrepareQuestionForDB(columnList, sqlParams);
            Q4.PrepareQuestionForDB(columnList, sqlParams);
            Q5Name.PrepareQuestionForDB(columnList, sqlParams);
            Q5Title.PrepareQuestionForDB(columnList, sqlParams);
            Q5Telephone.PrepareQuestionForDB(columnList, sqlParams);
            Q5Email.PrepareQuestionForDB(columnList, sqlParams);
            Q6Street.PrepareQuestionForDB(columnList, sqlParams);
            Q6City.PrepareQuestionForDB(columnList, sqlParams);
            Q6Province.PrepareQuestionForDB(columnList, sqlParams);
            Q6PostalCode.PrepareQuestionForDB(columnList, sqlParams);
            Q7.PrepareQuestionForDB(columnList, sqlParams);
            Q8.PrepareQuestionForDB(columnList, sqlParams);
            Q9A.PrepareQuestionForDB(columnList, sqlParams);
            Q9B.PrepareQuestionForDB(columnList, sqlParams);
            Q9C.PrepareQuestionForDB(columnList, sqlParams);
            Q9C_Explanation.PrepareQuestionForDB(columnList, sqlParams);
            Q10.PrepareQuestionForDB(columnList, sqlParams);
            Q11A_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11A_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            Q11B_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11B_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            Q11C_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11C_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            //Q11D_PastSupport.PrepareQuestionForDB( columnList, sqlParams );
            //Q11D_CurrentRequest.PrepareQuestionForDB( columnList, sqlParams );
            Q11E_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11E_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            Q11F_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11F_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            Q11G_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11G_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            Q11H_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11H_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            Q11I_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11I_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            Q11J_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11J_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            Q11K_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11K_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            Q11L_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11L_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);
            Q11M_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11M_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            //2017-11-14  Adding 3 new ontario locations
            Q11N_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11N_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            Q11O_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11O_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            Q11P_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11P_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            Q11Q_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11Q_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            Q11R_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11R_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            Q11S_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11S_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            Q11T_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11T_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            Q11U_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11U_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);


            Q11V_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11V_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            Q11W_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11W_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            Q11X_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11X_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);



            Q11Y_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11Y_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);

            Q11Z_PastSupport.PrepareQuestionForDB(columnList, sqlParams);
            Q11Z_CurrentRequest.PrepareQuestionForDB(columnList, sqlParams);





            Q12.PrepareQuestionForDB(columnList, sqlParams);
            Q13.PrepareQuestionForDB(columnList, sqlParams);
            Q14.PrepareQuestionForDB(columnList, sqlParams);

            columnList.Append(",[PropertyID],[DateEntered]");
            sqlParams.Add("@PropertyID", Master.PropertyID)
                     .Add("@DateEntered", DateTime.Now);

            columnList.Remove(0, 1);
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            rowID = sql.QueryAndReturnIdentity(String.Format("INSERT INTO [tblSurveyDonation] ({0}) VALUES ({1});", columnList, columnList.ToString().Replace("[", "@").Replace("]", String.Empty)), sqlParams);
            return (!sql.HasError && rowID != -1);
        }
    }
}