using SharedClasses;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class SurveyGSEI : BasePage
    {
        private const int LAST_PAGE = 4;
        private const int COMPLETE_PAGE = 97;
        private const int NOVISIT_PAGE = 98;
        private const int DECLINE_PAGE = 99;

        protected enum GSEISurveyType
        {
            None = 0,

            /// <summary>
            /// BC Properties
            /// </summary>
            BC = 1,

            /// <summary>
            /// HPI
            /// </summary>
            HP = 2,

            /// <summary>
            /// Hotel
            /// </summary>
            HO = 3,

            /// <summary>
            /// TicketMaster
            /// </summary>
            TM = 4,

            /// <summary>
            /// Great American
            /// </summary>
            GA = 5
        }

        public GCCPropertyShortCode ForceSpecificProperty { get; set; }

        /// <summary>
        /// Gets the property short code for the current request.
        /// </summary>
        public GCCPropertyShortCode PropertyShortCode
        {
            get
            {
                if (ForceSpecificProperty != GCCPropertyShortCode.None)
                {
                    return ForceSpecificProperty;
                }
                object property = Page.RouteData.Values["propertyshortcode"];
                if (property != null)
                {
                    GCCPropertyShortCode sc;
                    if (Enum.TryParse<GCCPropertyShortCode>(property.ToString().ToUpper(), out sc))
                    {
                        return sc;
                    }
                    return GCCPropertyShortCode.GCC;
                }
                else
                {
                    return GCCPropertyShortCode.GCC;
                }
            }
        }

        protected GSEISurveyType SurveyType
        {
            get
            {
                MatchCollection regX = Regex.Matches(Request.Url.AbsolutePath, @"^/gsei?([bchpotmga]{2})?/.*", RegexOptions.IgnoreCase);
                if (regX.Count > 0)
                {
                    switch (regX[0].Groups[1].Value.ToLower())
                    {
                        case "hp":
                            return GSEISurveyType.HP;

                        case "ho":
                            return GSEISurveyType.HO;

                        case "tm":
                            return GSEISurveyType.TM;

                        case "ga":
                            return GSEISurveyType.GA;

                        default:
                            return GSEISurveyType.BC;
                    }
                }
                else
                {
                    return GSEISurveyType.BC;
                }
            }
        }

        protected bool SurveyComplete { get; set; }

        protected string CasinoName
        {
            get
            {
                switch (SurveyType)
                {
                    case GSEISurveyType.HP:
                        if (SurveyTools.GetValue(radLocation_EC, false, false))
                        {
                            return "Fraser Downs Racetrack & Casino / Elements Casino";
                        }
                        else if (SurveyTools.GetValue(radLocation_HA, false, false))
                        {
                            return "Hasting Racecourse & Casino";
                        }
                        else
                        {
                            return "the casino";
                        }
                    case GSEISurveyType.TM:
                        if (SurveyTools.GetValue(radLocation_RR, false, false))
                        {
                            return "River Rock Casino Resort";
                        }
                        else if (SurveyTools.GetValue(radLocation_HRCV, false, false))
                        {
                            return "Hard Rock Casino Vancouver";
                        }
                        else
                        {
                            return "the casino";
                        }
                    case GSEISurveyType.GA:
                        if (SurveyTools.GetValue(radLocation_Lakewood, false, false))
                        {
                            return "Great American Casino - Lakewood";
                        }
                        else if (SurveyTools.GetValue(radLocation_Tukwila, false, false))
                        {
                            return "Great American Casino - Tukwila";
                        }
                        else if (SurveyTools.GetValue(radLocation_Everett, false, false))
                        {
                            return "Great American Casino - Everett";
                        }
                        else
                        {
                            return "Great American Casino";
                        }
                    default:
                        return Master.CasinoName;
                }
            }
        }

        public GCCPropertyShortCode PropertyShortCodeOverride()
        {
            switch (SurveyType)
            {
                case GSEISurveyType.HP:
                    if (SurveyTools.GetValue(radLocation_EC, false, false))
                    {
                        return GCCPropertyShortCode.EC;
                    }
                    else if (SurveyTools.GetValue(radLocation_HA, false, false))
                    {
                        return GCCPropertyShortCode.HA;
                    }
                    else
                    {
                        return Master.OriginalPropertyShortCode;
                    }
                case GSEISurveyType.TM:
                    if (SurveyTools.GetValue(radLocation_RR, false, false))
                    {
                        return GCCPropertyShortCode.RR;
                    }
                    else if (SurveyTools.GetValue(radLocation_HRCV, false, false))
                    {
                        return GCCPropertyShortCode.HRCV;
                    }
                    else
                    {
                        return Master.OriginalPropertyShortCode;
                    }
                case GSEISurveyType.GA:
                    return GCCPropertyShortCode.GAG;

                default:
                    return Master.OriginalPropertyShortCode;
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Master.IsEmailOnlySurvey = true;
            Master.PropertyShortCodeOverride += PropertyShortCodeOverride;

            if (this.PropertyShortCode == GCCPropertyShortCode.SCTI)
            {
                // do nothing
            }
            else
            {
                //Check if we're past midnight PST on Jan 31st and close the survey
                //if (DateTime.Now >= new DateTime(2016, 2, 1, 3, 0, 0))                
                if (DateTime.Now >= new DateTime(2017, 6, 1, 3, 0, 0))
                {
                    Master.HideContent = true;
                    Master.TopMessage.WarningMessage = "The surveying period has now passed. Thank you for your interest.";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCGC &raquo; " + CasinoName;
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //Check all previous pages
            //Must do in LoadComplete because controls load values in Load method (Init didn't work because reasons...)
            if (Master.CurrentPage > 1 && Master.CurrentPage != DECLINE_PAGE && Master.CurrentPage != NOVISIT_PAGE && !IsPostBack)
            {
                for (int i = 1; i < Master.CurrentPage; i++)
                {
                    if (!ValidateAndSave(i, false, false))
                    {
                        Response.Redirect(GetURL(i, Master.RedirectDirection), true);
                        return;
                    }
                }
                if (PageShouldBeSkipped(Master.CurrentPage))
                {
                    int nextPage = Master.CurrentPage + Master.RedirectDirection;
                    if (Master.CurrentPage == LAST_PAGE && Master.RedirectDirection == 1)
                    {
                        nextPage = COMPLETE_PAGE;
                    }
                    Response.Redirect(GetURL(nextPage, Master.RedirectDirection), true);
                    return;
                }
                //If we've made it to COMPLETE_PAGE, save to database.
                if (Master.CurrentPage == COMPLETE_PAGE && !IsPostBack)
                {
                    int surveyID;
                    if (SaveData(out surveyID))
                    {
                        SendNotifications();
                        SurveyComplete = true;
                        Session.Abandon();
                        mmLastPage.SuccessMessage = "Thank you for taking our survey.  You will be notified via email if you are the winner of the survey contest.";
                    }
                    else
                    {
                        mmLastPage.ErrorMessage = "We were unable to save your responses. Please go back and try again.";
                    }
                }
            }
        }

        private void SendNotifications()
        {
            //Send thank you letter
            if (Master.EmailPINRow != null)
            {
                SurveyTools.SendNotifications(
                    Server,
                    Master.PropertyShortCode,
                    SharedClasses.SurveyType.GEI,
                    NotificationReason.ThankYou,
                    string.Empty,
                    new
                    {
                        CasinoName = PropertyTools.GetCasinoName((int)Master.PropertyShortCode),
                        FeedbackNoteHTML = String.Empty,
                        FeedbackNoteTXT = String.Empty,
                        Attachments = new SurveyTools.SurveyAttachmentDetails[] {
                        new SurveyTools.SurveyAttachmentDetails() { Path = "~/Images/headers/" + PropertyTools.GetCasinoHeaderImage( Master.PropertyShortCode ), ContentID = "HeaderImage" }
                    }
                    },
                    Master.EmailPINRow["EmailAddress"].ToString());
            }
        }

        protected void Prev_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine( "Survey Prev_Click PID: " + CurrentPage );
            if (ValidateAndSave(Master.CurrentPage, true, true))
            {
                int prevPage = Master.CurrentPage - 1;
                //Check if they're undoing their decline
                if (Master.CurrentPage == DECLINE_PAGE || Master.CurrentPage == NOVISIT_PAGE)
                {
                    prevPage = 1;
                }
                else if (Master.CurrentPage == COMPLETE_PAGE)
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
                    nextPage = COMPLETE_PAGE;
                }
                if (Master.CurrentPage == COMPLETE_PAGE)
                {
                    Response.Redirect("http://gcgaming.com", true);
                    return;
                }

                //Check for if they declined
                if (radDecline.Checked)
                {
                    nextPage = DECLINE_PAGE;
                }

                if (radLocation_None.Checked || radLocation_None2.Checked || radLocation_None3.Checked)
                {
                    nextPage = NOVISIT_PAGE;
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

                    #endregion Page 1

                case 2:

                    #region Page 2

                    if (!saveOnly)
                    {
                        if (SurveyType == GSEISurveyType.HP)
                        {
                            if (!SurveyTools.GetValue(radLocation_EC, currentPage, false)
                                && !SurveyTools.GetValue(radLocation_HA, currentPage, false)
                                && !SurveyTools.GetValue(radLocation_None, currentPage, false))
                            {
                                radLocation_EC.MessageManager.ErrorMessage = "Please select one of the following options.";
                                retVal = false;
                            }
                        }
                        else if (SurveyType == GSEISurveyType.TM)
                        {
                            if (!SurveyTools.GetValue(radLocation_RR, currentPage, false)
                                && !SurveyTools.GetValue(radLocation_HRCV, currentPage, false)
                                && !SurveyTools.GetValue(radLocation_None2, currentPage, false))
                            {
                                radLocation_RR.MessageManager.ErrorMessage = "Please select one of the following options.";
                                retVal = false;
                            }
                        }
                        else if (SurveyType == GSEISurveyType.GA)
                        {
                            if (!SurveyTools.GetValue(radLocation_Lakewood, currentPage, false)
                                && !SurveyTools.GetValue(radLocation_Tukwila, currentPage, false )
								&& !SurveyTools.GetValue( radLocation_Everett, currentPage, false )
								&& !SurveyTools.GetValue( radLocation_DeMoines, currentPage, false )
								&& !SurveyTools.GetValue(radLocation_None3, currentPage, false))
                            {
                                radLocation_Lakewood.MessageManager.ErrorMessage = "Please select one of the following options.";
                                retVal = false;
                            }
                        }
                    }
                    if (currentPage)
                    {
                        if (SurveyType == GSEISurveyType.HP)
                        {
                            SurveyTools.SaveRadioButtons(radLocation_EC, radLocation_HA, radLocation_None);
                        }
                        else if (SurveyType == GSEISurveyType.TM)
                        {
                            SurveyTools.SaveRadioButtons(radLocation_RR, radLocation_HRCV, radLocation_None2);
                        }
                        else if (SurveyType == GSEISurveyType.GA)
                        {
                            SurveyTools.SaveRadioButtons(radLocation_Lakewood, radLocation_Tukwila, radLocation_Everett, radLocation_DeMoines, radLocation_None3);
                        }
                    }
                    break;

                    #endregion Page 2

                case 3: // Staff

                    #region Page 3

                    if (!saveOnly)
                    {
                        if (!SurveyTools.CheckForAnswer(Q7A, true)
                            | !SurveyTools.CheckForAnswer(Q7B, true)
                            | !SurveyTools.CheckForAnswer(Q7C, true)
                            | !SurveyTools.CheckForAnswer(Q7D, true)
                            | !SurveyTools.CheckForAnswer(Q7E, true)
                            | !SurveyTools.CheckForAnswer(Q7F, true)
                            | !SurveyTools.CheckForAnswer(Q8, true)
                            )
                        {
                            retVal = false;
                        }
                    }
                    if (currentPage)
                    {
                        SurveyTools.SaveValue(Q7A);
                        SurveyTools.SaveValue(Q7B);
                        SurveyTools.SaveValue(Q7C);
                        SurveyTools.SaveValue(Q7D);
                        SurveyTools.SaveValue(Q7E);
                        SurveyTools.SaveValue(Q7F);
                        SurveyTools.SaveValue(Q8);
                    }
                    break;

                    #endregion Page 3

                case 4: // Demographics

                    #region Page 4

                    if (!saveOnly)
                    {
                        bool ageGroupNotSelected = !SurveyTools.GetValue(Q37_1, currentPage, false) &&
                                                    !SurveyTools.GetValue(Q37_2, currentPage, false) &&
                                                    !SurveyTools.GetValue(Q37_3, currentPage, false) &&
                                                    !SurveyTools.GetValue(Q37_4, currentPage, false) &&
                                                    !SurveyTools.GetValue(Q37_5, currentPage, false) &&
                                                    !SurveyTools.GetValue(Q37_6, currentPage, false);
                        //if ( ageGroupNotSelected ) {
                        //    Q37Message.ErrorMessage = "Please answer the following question.";
                        //    retVal = false;
                        //}

                        bool genderNotSelected = !SurveyTools.GetValue(Q36Male, currentPage, false) &&
                                                 !SurveyTools.GetValue(Q36Female, currentPage, false);
                        //if ( genderNotSelected ) {
                        //    Q36Message.ErrorMessage = "Please answer the following question.";
                        //    retVal = false;
                        //}

                        if (SurveyType != GSEISurveyType.GA)
                        {
                            bool noneSelected = !SurveyTools.GetValue(radQ1_Slots, currentPage, false) &&
                                                !SurveyTools.GetValue(radQ1_Tables, currentPage, false) &&
                                                !SurveyTools.GetValue(radQ1_Poker, currentPage, false) &&
                                                !SurveyTools.GetValue(radQ1_Food, currentPage, false) &&
                                                !SurveyTools.GetValue(radQ1_Entertainment, currentPage, false) &&
                                                !SurveyTools.GetValue(radQ1_Hotel, currentPage, false) &&
                                                !SurveyTools.GetValue(radQ1_LiveRacing, currentPage, false) &&
                                                !SurveyTools.GetValue(radQ1_Racebook, currentPage, false) &&
                                                !SurveyTools.GetValue(radQ1_Bingo, currentPage, false) &&
                                                !SurveyTools.GetValue(radQ1_Lottery, currentPage, false) &&
                                                !SurveyTools.GetValue(radQ1_None, currentPage, false);

                            if (!saveOnly && noneSelected)
                            {
                                mmQ1.ErrorMessage = "Please select one of the following options.";
                                return false;
                            }
                        }
                    }
                    if (currentPage)
                    {
                        SurveyTools.SaveRadioButtons(Q37_1, Q37_2, Q37_3, Q37_4, Q37_5, Q37_6);
                        SurveyTools.SaveRadioButtons(Q36Male, Q36Female);
                        if (SurveyType != GSEISurveyType.GA)
                        {
                            SurveyTools.SaveRadioButtons(radQ1_Slots, radQ1_Tables, radQ1_Poker, radQ1_Food, radQ1_Entertainment, radQ1_Hotel, radQ1_LiveRacing, radQ1_Racebook, radQ1_Bingo, radQ1_Lottery, radQ1_None);
                            SurveyTools.SaveValue(chkQ2_Slots);
                            SurveyTools.SaveValue(chkQ2_Tables);
                            SurveyTools.SaveValue(chkQ2_Poker);
                            SurveyTools.SaveValue(chkQ2_Food);
                            SurveyTools.SaveValue(chkQ2_Entertainment);
                            SurveyTools.SaveValue(chkQ2_Hotel);
                            SurveyTools.SaveValue(chkQ2_LiveRacing);
                            SurveyTools.SaveValue(chkQ2_Racebook);
                            SurveyTools.SaveValue(chkQ2_Bingo);
                            SurveyTools.SaveValue(chkQ2_Lottery);
                        }
                    }

                    break;

                    #endregion Page 4
            }
            return retVal;
        }

        private bool PageShouldBeSkipped(int currentPage)
        {
            switch (currentPage)
            {
                case 2: //Qualifier
                    return (SurveyType != GSEISurveyType.TM && SurveyType != GSEISurveyType.HP && SurveyType != GSEISurveyType.GA);
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
            return String.Format("/GSEI{5}/{0}/{1}/{2}{3}{4}", Master.PropertyShortCode.ToString(), Master.EmailPIN.ToString(), page, (redirDir == -1 ? "/-1" : String.Empty), (isReset ? "?r=1" : String.Empty), SurveyType.ToString());
        }

        protected bool SaveData(out int rowID)
        {
            StringBuilder columnList = new StringBuilder();
            SQLParamList sqlParams = new SQLParamList();

            radQ1_Slots.PrepareQuestionForDB(columnList, sqlParams);
            radQ1_Tables.PrepareQuestionForDB(columnList, sqlParams);
            radQ1_Poker.PrepareQuestionForDB(columnList, sqlParams);
            radQ1_Food.PrepareQuestionForDB(columnList, sqlParams);
            radQ1_Entertainment.PrepareQuestionForDB(columnList, sqlParams);
            radQ1_Hotel.PrepareQuestionForDB(columnList, sqlParams);
            radQ1_LiveRacing.PrepareQuestionForDB(columnList, sqlParams);
            radQ1_Racebook.PrepareQuestionForDB(columnList, sqlParams);
            radQ1_Bingo.PrepareQuestionForDB(columnList, sqlParams);
            radQ1_Lottery.PrepareQuestionForDB(columnList, sqlParams);
            radQ1_None.PrepareQuestionForDB(columnList, sqlParams);
            chkQ2_Slots.PrepareQuestionForDB(columnList, sqlParams);
            chkQ2_Tables.PrepareQuestionForDB(columnList, sqlParams);
            chkQ2_Poker.PrepareQuestionForDB(columnList, sqlParams);
            chkQ2_Food.PrepareQuestionForDB(columnList, sqlParams);
            chkQ2_Entertainment.PrepareQuestionForDB(columnList, sqlParams);
            chkQ2_Hotel.PrepareQuestionForDB(columnList, sqlParams);
            chkQ2_LiveRacing.PrepareQuestionForDB(columnList, sqlParams);
            chkQ2_Racebook.PrepareQuestionForDB(columnList, sqlParams);
            chkQ2_Bingo.PrepareQuestionForDB(columnList, sqlParams);
            chkQ2_Lottery.PrepareQuestionForDB(columnList, sqlParams);
            Q7A.PrepareQuestionForDB(columnList, sqlParams);
            Q7B.PrepareQuestionForDB(columnList, sqlParams);
            Q7C.PrepareQuestionForDB(columnList, sqlParams);
            Q7D.PrepareQuestionForDB(columnList, sqlParams);
            Q7E.PrepareQuestionForDB(columnList, sqlParams);
            Q7F.PrepareQuestionForDB(columnList, sqlParams);
            Q8.PrepareQuestionForDB(columnList, sqlParams);
            Q36Male.PrepareQuestionForDB(columnList, sqlParams);
            Q36Female.PrepareQuestionForDB(columnList, sqlParams);
            Q37_1.PrepareQuestionForDB(columnList, sqlParams);
            Q37_2.PrepareQuestionForDB(columnList, sqlParams);
            Q37_3.PrepareQuestionForDB(columnList, sqlParams);
            Q37_4.PrepareQuestionForDB(columnList, sqlParams);
            Q37_5.PrepareQuestionForDB(columnList, sqlParams);
            Q37_6.PrepareQuestionForDB(columnList, sqlParams);

            if (SurveyType == GSEISurveyType.GA)
            {
                radLocation_Lakewood.PrepareQuestionForDB(columnList, sqlParams);
                radLocation_Tukwila.PrepareQuestionForDB(columnList, sqlParams);
				radLocation_Everett.PrepareQuestionForDB( columnList, sqlParams );
				radLocation_DeMoines.PrepareQuestionForDB( columnList, sqlParams );
			}

            columnList.Append(",[SurveyType],[PropertyID],[DateEntered],[PIN],[BenchmarkYear]");
            sqlParams.Add("@SurveyType", (int)GEISurveyType.Email)
                     .Add("@PropertyID", (int)Master.PropertyShortCode)
                     .Add("@DateEntered", DateTime.Now)
                     .Add("@PIN", Master.EmailPIN)
                     .Add("@BenchmarkYear", 2017);
            if (Master.EmailPINRow != null)
            {
                columnList.Append(",[EmailBatch]");
                sqlParams.Add("@EmailBatch", Master.EmailPINRow["BatchID"]);
            }

            columnList.Remove(0, 1);
            SQLDatabase sql = new SQLDatabase();
            rowID = sql.QueryAndReturnIdentity(String.Format("INSERT INTO [tblSurveyGEI] ({0}) VALUES ({1});", columnList, columnList.ToString().Replace("[", "@").Replace("]", String.Empty)), sqlParams);
            if (!sql.HasError && rowID != -1)
            {
                sql.NonQuery("UPDATE [tblSurveyGEI_EmailPINs] SET [SurveyCompleted] = 1 WHERE PIN = @PIN", new SqlParameter("@PIN", Master.EmailPIN));
                if (sql.HasError)
                {
                    //TODO: Do we want to do something here?
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}