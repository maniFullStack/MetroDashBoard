using SharedClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteUtilities;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace GCC_Web_Portal
{
    public partial class HastingsSurvey : BasePage
    {
        /// <summary>
        /// Gets the current survey page.
        /// </summary>
        public int CurrentPage
        {
            get
            {
                object page = Page.RouteData.Values["page"];
                if (page != null)
                {
                    return Conversion.StringToInt(page.ToString(), 1);
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// Will return true if the survey questions should be mandatory (where applicable).
        /// </summary>
        public bool QuestionsAreMandatory
        {
            get
            {
                return !IsKioskOrStaffEntry;
            }
        }

        /// <summary>
        /// Returns true if this is a kiosk or staff entry survey.
        /// </summary>
        public bool IsKioskOrStaffEntry
        {
            get
            {
                return SurveyType == SharedClasses.GEISurveyType.Kiosk || SurveyType == SharedClasses.GEISurveyType.StaffSurvey;
            }
        }

        /// <summary>
        /// The current survey type for handling how it is displayed.
        /// </summary>
        public GEISurveyType SurveyType
        {
            get
            {
                MatchCollection regX = Regex.Matches(Request.Url.AbsolutePath, @"^/s(?:urvey)?([eks])?/.*", RegexOptions.IgnoreCase);
                if (regX.Count > 0)
                {
                    switch (regX[0].Groups[1].Value.ToLower())
                    {
                        case "e":
                            return SharedClasses.GEISurveyType.Email;
                        case "k":
                            return SharedClasses.GEISurveyType.Kiosk;
                        case "s":
                            return SharedClasses.GEISurveyType.StaffSurvey;
                        default:
                            return SharedClasses.GEISurveyType.DirectAccess;
                    }
                }
                else
                {
                    return SharedClasses.GEISurveyType.DirectAccess;
                }
            }
        }

        protected bool HastingsComplete { get; set; }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Check for a reset and abandon the session and redirect to page 1.
            if (RequestVars.Get("r", 0) == 1)
            {
                Session.Abandon();
                Response.Redirect(GetURL(1, 1), true);
                return;
            }

            //spbProgress.MaxValue = 22;
            //spbProgress.CurrentValue = CurrentPage;
            //spbProgress.Visible = (CurrentPage != 1 //First page
            //                        && CurrentPage != 99 //Quit early page
            //                        && (CurrentPage != 97 || !String.IsNullOrWhiteSpace(mmLastPage.SuccessMessage))); //Only show 100% on final page

            

        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {

            
            Title = "Hastings Racecourse Customer Survey";
            //Check all previous pages
            //Must do in LoadComplete because controls load values in Load method
            SessionWrapper.Add("HastingsSurveyPageNumber", CurrentPage);
            if (CurrentPage > 1 && CurrentPage != 99 && !IsPostBack)
            {
                for (int i = 1; i < CurrentPage; i++)
                {
                    //System.Diagnostics.Debug.WriteLine( "Checking Page: " + i );
                    if (!ValidateAndSave(i, false, false))
                    {
                        //System.Diagnostics.Debug.WriteLine( "Invalid Page: " + i );
                        if (CurrentPage == 97)
                        {
                            continue;
                        }
                        Response.Redirect(GetURL(i,1), true);
                        return;
                    }
                }
                //if (PageShouldBeSkipped(CurrentPage))
                //{
                //    int nextPage = CurrentPage + RedirectDirection;
                //    if (CurrentPage == 22 && RedirectDirection == 1)
                //    {
                //        nextPage = 97;
                //        //return;
                //    }
                //    Response.Redirect(GetSurveyURL(nextPage, RedirectDirection), true);
                //    return;
                //}
                
                //If we've made it to 97, save to database.
                if (CurrentPage == 97 && !IsPostBack)
                {
                    int surveyID;
                    if (SaveData(out surveyID))
                    {
                        //SendNotifications(surveyID);
                        HastingsComplete = true;
                        Session.Abandon();
                        SurveyComplete.SuccessMessage = "Thank you for your feedback. We will use your responses in our ongoing efforts to make your visits exciting and memorable.<br /><br />Please Note: If you have requested someone to contact you, please ensure you check your \"Junk Mail\" folder or add \"@gcgamingsurvey.com\" to your email account's white list.";
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (string key in Session.Keys)
                        {
                            object val = Session[key];
                            List<string> ls = val as List<string>;
                            if (ls != null)
                            {
                                sb.AppendFormat("\"{0}\": {1}", key, String.Join(", ", ls));
                            }
                            else
                            {
                                sb.AppendFormat("\"{0}\": {1}", key, val);
                            }
                        }
                        //ErrorHandler.WriteLog("GCC_Web_Portal.SurveyGEI", "Unable to save responses.\n\nSession Variables:\n\n" + sb.ToString(), ErrorHandler.ErrorEventID.General);
                        SurveyComplete.ErrorMessage = "We were unable to save your responses. Please go back and try again.";
                    }
                }
            }
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
                else if (Master.CurrentPage == 97)
                {
                    prevPage = 5;
                }
                Response.Redirect(GetURL(prevPage, -1), true);
            }
        }

        protected void Next_Click(object sender, EventArgs e)
        {
            if (ValidateAndSave(Master.CurrentPage, true, false))
            {
                //if (Master.CurrentPage == 99)
                //{
                //    Response.Redirect(PropertyTools.GetCasinoURL(Master.PropertyShortCode), true);
                //    return;
                //}

                int nextPage = Master.CurrentPage + 1;

                if (nextPage > 5)
                {
                    nextPage = 97;
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
                case 1:

                    #region Page 1
                    string email = GetValue(txtEmail, currentPage, String.Empty);
                    if (!Validation.RegExCheck(email, ValidationType.Email))
                    {
                        mmTxtEmail.ErrorMessage = "Please enter a valid email address.";
                        return false;
                    }
                    else if (currentPage)
                    {
                        SaveValue<string>(txtEmail);
                    }
                    break;

                    #endregion Page 1
                
                case 2:

                    #region Page 2
                    if ( !saveOnly ) {
                        bool accept = GetValue( radAccept, currentPage, false );
                        bool decline = GetValue( radDecline, currentPage, false );
                        if ( !accept && !decline ) {
                            mmAcceptGroup.ErrorMessage = "Please select one of the following options.";
                            return false;
                        }
                    }
                    if ( currentPage ) {
                        SurveyTools.SaveRadioButtons( radAccept, radDecline );
                    }
                    break;

                    #endregion Page 2
               
                case 3:

                    #region Page 3
                    if (!saveOnly)
                    {
                        bool Q1notselected = !GetValue(Q1_1, currentPage, false) &&
                                             !GetValue(Q1_2to4, currentPage, false) &&
                                             !GetValue(Q1_5to9, currentPage, false) &&
                                             !GetValue(Q1_10, currentPage, false);
                        if (Q1notselected)
                        {
                            Q1Message.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }

                        bool Q2notselected = !GetValue(Q2_Yes, currentPage, false) &&
                                             !GetValue(Q2_No, currentPage, false);
                        if (Q2notselected)
                        {
                            Q2Message.ErrorMessage = "Please select one of the following options.";
                        }

                        bool Q3notselected = !GetValue(Q3_No, currentPage, false) &&
                                             !GetValue(Q3_Yes, currentPage, false);
                        if (Q3notselected)
                        {
                            Q3Message.ErrorMessage = "Please select one of the following options.";
                        }

                        bool Q4notselected = !GetValue(Q4_Yes, currentPage, false) &&
                                             !GetValue(Q4_No, currentPage, false);
                        
                        if (Q4notselected)
                        {
                            Q4Message.ErrorMessage = "Please select Yes or No.";
                            retVal = false;
                        }
                    }
                    if (currentPage)
                    {
                        SaveValue(Q1_1);
                        SaveValue(Q1_2to4);
                        SaveValue(Q1_5to9);
                        SaveValue(Q1_10);
                        SaveValue(Q2_No);
                        SaveValue(Q2_Yes);
                        SaveValue(Q3_No);
                        SaveValue(Q3_Yes);
                        SaveValue(Q4_Yes);
                        SaveValue(Q4_No);
                        SaveValue(Q4_EncoreNumber);
                    }
                    break;

                    #endregion Page 3
                
                case 4:

                    #region Page 4
                    if (!saveOnly)
                    {
                        bool Q5notselected = !GetValue(Q5_Tarmac, currentPage, false) &&
                                             !GetValue(Q5_BoxSeats, currentPage, false) &&
                                             !GetValue(Q5_GroupPatio, currentPage, false) &&
                                             !GetValue(Q5_MarqueeTent, currentPage, false) &&
                                             !GetValue(Q5_SilksBuffet, currentPage, false) &&
                                             !GetValue(Q5_DiamondClub, currentPage, false);
                        if (Q5notselected)
                        {
                            Q5Message.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }

                        bool Q6notselected = !GetValue(Q6_Excellent, currentPage, false) &&
                                             !GetValue(Q6_VeryGood, currentPage, false) &&
                                             !GetValue(Q6_Good, currentPage, false) &&
                                             !GetValue(Q6_Fair, currentPage, false) &&
                                             !GetValue(Q6_Poor, currentPage, false);
                        if (Q6notselected)
                        {
                            Q6Message.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }

                        bool Q7notselected = !GetValue(Q7_Excellent, currentPage, false) &&
                                             !GetValue(Q7_VeryGood, currentPage, false) &&
                                             !GetValue(Q7_Good, currentPage, false) &&
                                             !GetValue(Q7_Fair, currentPage, false) &&
                                             !GetValue(Q7_Poor, currentPage, false);
                        if (Q7notselected)
                        {
                            Q7Message.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }

                        bool Q8notselected = !GetValue(Q8_DefinitelyWould, currentPage, false) &&
                                             !GetValue(Q8_ProbablyWould, currentPage, false) &&
                                             !GetValue(Q8_MightMightNot, currentPage, false) &&
                                             !GetValue(Q8_ProbablyWouldNot, currentPage, false) &&
                                             !GetValue(Q8_DefinitelyWouldNot, currentPage, false);
                        if (Q8notselected)
                        {
                            Q8Message.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }
                    }
                    if (currentPage)
                    {
                        SaveValue(Q5_Tarmac);
                        SaveValue(Q5_BoxSeats);
                        SaveValue(Q5_GroupPatio);
                        SaveValue(Q5_MarqueeTent);
                        SaveValue(Q5_SilksBuffet);
                        SaveValue(Q5_DiamondClub);
                        SaveValue(Q6_Excellent);
                        SaveValue(Q6_VeryGood);
                        SaveValue(Q6_Good);
                        SaveValue(Q6_Fair);
                        SaveValue(Q6_Poor);
                        SaveValue(Q7_Excellent);
                        SaveValue(Q7_VeryGood);
                        SaveValue(Q7_Good);
                        SaveValue(Q7_Fair);
                        SaveValue(Q7_Poor);
                        SaveValue(Q8_DefinitelyWould);
                        SaveValue(Q8_ProbablyWould);
                        SaveValue(Q8_MightMightNot);
                        SaveValue(Q8_ProbablyWouldNot);
                        SaveValue(Q8_DefinitelyWouldNot);
                    }
                    break;

                    #endregion Page 4

                case 5:

                    #region Page 5
                    if (!saveOnly)
                    {
                        bool Q9notselected = !GetValue(Q9_Male, currentPage, false) &&
                                             !GetValue(Q9_Female, currentPage, false);
                        if (Q9notselected)
                        {
                            Q9Message.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }

                        bool Q10notselected = !GetValue(Q10_19to24, currentPage, false) &&
                                             !GetValue(Q10_25to34, currentPage, false) &&
                                             !GetValue(Q10_35to44, currentPage, false) &&
                                             !GetValue(Q10_45to54, currentPage, false) &&
                                             !GetValue(Q10_55to64, currentPage, false) &&
                                             !GetValue(Q10_65orOlder, currentPage, false);
                        if (Q10notselected)
                        {
                            Q10Message.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }

                        bool Q11notselected = !GetValue(Q11_35000, currentPage, false) &&
                                              !GetValue(Q11_35000to59999, currentPage, false) &&
                                              !GetValue(Q11_60000to89999, currentPage, false) &&
                                              !GetValue(Q11_90000, currentPage, false) &&
                                              !GetValue(Q11_NoSay, currentPage, false);
                        if (Q11notselected)
                        {
                            Q10Message.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }

                        bool Q12check = !String.IsNullOrEmpty(GetValue(Q12_PostalCode, currentPage, String.Empty));

                        if (!Q12check)
                        {
                            Q12_PostalCode.MessageManager.ErrorMessage = "Please enter the first 3 digits of your Postal Code.";
                            retVal = false;
                        }

                        bool Q13FirstNameCheck = !String.IsNullOrEmpty(GetValue(Q13_FirstName, currentPage, String.Empty));

                        if (!Q13FirstNameCheck)
                        {
                            Q13_FirstName.MessageManager.ErrorMessage = "Please enter your First Name.";
                            retVal = false;
                        }

                        bool Q13LastNameCheck = !String.IsNullOrEmpty(GetValue(Q13_LastName, currentPage, String.Empty));

                        if (!Q13LastNameCheck)
                        {
                            Q13_LastName.MessageManager.ErrorMessage = "Please enter your Last Name.";
                            retVal = false;
                        }                        
                         
                        bool Q13EmailCheck = !String.IsNullOrEmpty(GetValue(Q13_Email, currentPage, String.Empty));

                        if (!Q13EmailCheck)
                        {
                            Q13_Email.MessageManager.ErrorMessage = "Please enter your Email Address.";
                            retVal = false;
                        }
                    }
                    if (currentPage)
                    {
                        SaveValue(Q9_Male);
                        SaveValue(Q9_Female);
                        SaveValue(Q10_19to24);
                        SaveValue(Q10_25to34);
                        SaveValue(Q10_35to44);
                        SaveValue(Q10_45to54);
                        SaveValue(Q10_55to64);
                        SaveValue(Q10_65orOlder);
                        SaveValue(Q11_35000);
                        SaveValue(Q11_35000to59999);
                        SaveValue(Q11_60000to89999);
                        SaveValue(Q11_90000);
                        SaveValue(Q11_NoSay);
                        SaveValue(Q12_PostalCode);
                        SaveValue(Q13_FirstName);
                        SaveValue(Q13_LastName);
                        SaveValue(Q13_Email);
                    }
                    break;

                    #endregion Page 5

            }
            return retVal;
        }

        protected T GetValue<T>(ISurveyControl<T> control, bool getCurrentValue, T defaultValue)
        {
            return SurveyTools.GetValue(control, getCurrentValue, defaultValue);
        }

        protected void SaveValue<T>(ISurveyControl<T> control)
        {
            SurveyTools.SaveValue(control);
        }

        protected bool CheckForAnswer(ISurveyControl<int> control)
        {
            return SurveyTools.CheckForAnswer(control, QuestionsAreMandatory);
        }

        protected bool CheckForAnswer(ISurveyControl<string> control)
        {
            return SurveyTools.CheckForAnswer(control, QuestionsAreMandatory);
        }

        protected string GetURL(int page, int redirDir)
        {            
            return String.Format("/HastingsSurvey/{0}/{1}", page, (redirDir == -1 ? "/-1" : String.Empty));
        }

        protected bool SaveData(out int rowID)
        {
            StringBuilder columnList = new StringBuilder();
            SQLParamList sqlParams = new SQLParamList();
            
            //Add details for email surveys
            //if (SurveyType == GEISurveyType.Email)
            //{
            //    columnList.Append(",[EmailBatch],[PIN]");
            //    sqlParams.Add("@EmailBatch", EmailPINRow["BatchID"])
            //             .Add("@PIN", EmailPIN);
            //    if (String.IsNullOrWhiteSpace(txtQ4_CardNumber.Text))
            //    {
            //        //If they erased the encore number, we'll put in a new one
            //        Q4.SelectedValue = 1;
            //        txtQ4_CardNumber.Text = EmailPINRow["Encore"].ToString();
            //    }
            //}

            txtEmail.PrepareQuestionForDB(columnList, sqlParams);
            Q1_1.PrepareQuestionForDB(columnList, sqlParams);
            Q1_2to4.PrepareQuestionForDB(columnList, sqlParams);
            Q1_5to9.PrepareQuestionForDB(columnList, sqlParams);
            Q1_10.PrepareQuestionForDB(columnList, sqlParams);

            Q2_No.PrepareQuestionForDB(columnList, sqlParams);
            Q2_Yes.PrepareQuestionForDB(columnList, sqlParams);

            Q3_No.PrepareQuestionForDB(columnList, sqlParams);
            Q3_Yes.PrepareQuestionForDB(columnList, sqlParams);
          
            Q4_Yes.PrepareQuestionForDB(columnList, sqlParams);
            Q4_No.PrepareQuestionForDB(columnList, sqlParams);
            Q4_EncoreNumber.PrepareQuestionForDB(columnList, sqlParams);

            Q5_Tarmac.PrepareQuestionForDB(columnList, sqlParams);
            Q5_BoxSeats.PrepareQuestionForDB(columnList, sqlParams);
            Q5_GroupPatio.PrepareQuestionForDB(columnList, sqlParams);
            Q5_MarqueeTent.PrepareQuestionForDB(columnList, sqlParams);
            Q5_SilksBuffet.PrepareQuestionForDB(columnList, sqlParams);
            Q5_DiamondClub.PrepareQuestionForDB(columnList, sqlParams);

            Q6_Excellent.PrepareQuestionForDB(columnList, sqlParams);
            Q6_VeryGood.PrepareQuestionForDB(columnList, sqlParams);
            Q6_Good.PrepareQuestionForDB(columnList, sqlParams);
            Q6_Fair.PrepareQuestionForDB(columnList, sqlParams);
            Q6_Poor.PrepareQuestionForDB(columnList, sqlParams);

            Q7_Excellent.PrepareQuestionForDB(columnList, sqlParams);
            Q7_VeryGood.PrepareQuestionForDB(columnList, sqlParams);
            Q7_Good.PrepareQuestionForDB(columnList, sqlParams);
            Q7_Fair.PrepareQuestionForDB(columnList, sqlParams);
            Q7_Poor.PrepareQuestionForDB(columnList, sqlParams);

            Q8_DefinitelyWould.PrepareQuestionForDB(columnList, sqlParams);
            Q8_ProbablyWould.PrepareQuestionForDB(columnList, sqlParams);
            Q8_MightMightNot.PrepareQuestionForDB(columnList, sqlParams);
            Q8_ProbablyWouldNot.PrepareQuestionForDB(columnList, sqlParams);
            Q8_DefinitelyWouldNot.PrepareQuestionForDB(columnList, sqlParams);

            Q9_Male.PrepareQuestionForDB(columnList, sqlParams);
            Q9_Female.PrepareQuestionForDB(columnList, sqlParams);

            Q10_19to24.PrepareQuestionForDB(columnList, sqlParams);
            Q10_25to34.PrepareQuestionForDB(columnList, sqlParams);
            Q10_35to44.PrepareQuestionForDB(columnList, sqlParams);
            Q10_45to54.PrepareQuestionForDB(columnList, sqlParams);
            Q10_55to64.PrepareQuestionForDB(columnList, sqlParams);
            Q10_65orOlder.PrepareQuestionForDB(columnList, sqlParams);

            Q11_35000.PrepareQuestionForDB(columnList, sqlParams);
            Q11_35000to59999.PrepareQuestionForDB(columnList, sqlParams);
            Q11_60000to89999.PrepareQuestionForDB(columnList, sqlParams);
            Q11_90000.PrepareQuestionForDB(columnList, sqlParams);
            Q11_NoSay.PrepareQuestionForDB(columnList, sqlParams);

            Q12_PostalCode.PrepareQuestionForDB(columnList, sqlParams);

            Q13_FirstName.PrepareQuestionForDB(columnList, sqlParams);
            Q13_LastName.PrepareQuestionForDB(columnList, sqlParams);
            Q13_Email.PrepareQuestionForDB(columnList, sqlParams);

            columnList.Append(",[PropertyID],[DateEntered]");
            sqlParams.Add("@PropertyID", 5)
                     .Add("@DateEntered", DateTime.Now);


   
            columnList.Remove(0, 1);
            SQLDatabase sql = new SQLDatabase();
            rowID = sql.QueryAndReturnIdentity(String.Format("INSERT INTO [tblHastingsSurvey] ({0}) VALUES ({1});", columnList, columnList.ToString().Replace("[", "@").Replace("]", String.Empty)), sqlParams);
            if (!sql.HasError && rowID != -1)
            {
                //Dictionary<string, int> wordCounts = SurveyTools.GetWordCount(Q11.Text, txtQ27A_OtherExplanation.Text, txtQ27B.Text, Q32.Text, Q34.Text, Q35.Text, Q39_16Explanation.Text);
                //SurveyTools.SaveWordCounts(SharedClasses.SurveyType.GEI, rowID, wordCounts);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        
    }
}