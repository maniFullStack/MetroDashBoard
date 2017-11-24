using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class SurveySnapshot2017 : BasePage
    {
        protected enum GLocation
        {
            None = 0,
            Everett = 1,
            Lakewood = 2,
            Tukwila = 3,
            DesMoines = 4
        }

        protected bool SurveyComplete { get; set; }

        protected bool IsKioskSurvey
        {
            get
            {
                return Request.Url.AbsolutePath.ToLower().Contains("/snapshotk/");
            }
        }

        protected GCCPropertyShortCode QueryPropertyShortCode
        {
            get
            {
                if (Master.ForceSpecificProperty != GCCPropertyShortCode.None)
                {
                    return Master.ForceSpecificProperty;
                }
                object property = Page.RouteData.Values["propertyshortcode"];
                if (property != null)
                {
                    GCCPropertyShortCode sc;
                    string strProp = property.ToString();
                    if (Enum.TryParse<GCCPropertyShortCode>(strProp.ToUpper(), out sc))
                    {
                        return sc;
                    }
                    else
                    {
                        //Get check for query string GAG version
                        if (strProp.Length > 3 && strProp.Substring(0, 3).Equals("GAG"))
                        {
                            return GCCPropertyShortCode.GAG;
                        }
                    }
                }
                return GCCPropertyShortCode.GCC;
            }
        }

        protected GLocation GAGLocation
        {
            get
            {
                if (Master.PropertyShortCode == GCCPropertyShortCode.GAG)
                {
                    object property = Page.RouteData.Values["propertyshortcode"];
                    if (property != null)
                    {
                        string strProp = property.ToString();
                        if (strProp.Length > 3 && strProp.Substring(0, 3).Equals("GAG"))
                        {
                            switch (strProp[3])
                            {
                                case 'E':
                                    return GLocation.Everett;

                                case 'L':
                                    return GLocation.Lakewood;

                                case 'T':
                                    return GLocation.Tukwila;
                                case 'D':
                                    return GLocation.DesMoines;
                            }
                        }
                    }
                    string selectedProp = fbkProperty.SelectedValue;
                    if (selectedProp != null)
                    {
                        if (selectedProp.Length > 3 && selectedProp.Substring(0, 2).Equals("13"))
                        {
                            switch (selectedProp[3])
                            {
                                case '1':
                                    return GLocation.Everett;

                                case '2':
                                    return GLocation.Lakewood;

                                case '3':
                                    return GLocation.Tukwila;
                                case '4':
                                    return GLocation.DesMoines;
                            }
                        }
                    }
                    return GLocation.None;
                }
                else
                {
                    return GLocation.None;
                }
            }
        }

        protected GLocation QueryGAGLocation
        {
            get
            {
                if (Master.PropertyShortCode == GCCPropertyShortCode.GAG)
                {
                    object property = Page.RouteData.Values["propertyshortcode"];
                    if (property != null)
                    {
                        string strProp = property.ToString();
                        if (strProp.Length > 3 && strProp.Substring(0, 3).Equals("GAG"))
                        {
                            switch (strProp[3])
                            {
                                case 'E':
                                    return GLocation.Everett;

                                case 'L':
                                    return GLocation.Lakewood;

                                case 'T':
                                    return GLocation.Tukwila;
                                case 'D':
                                    return GLocation.DesMoines;
                            }
                        }
                    }
                    return GLocation.None;
                }
                else
                {
                    return GLocation.None;
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            spbProgress.MaxValue = 11;
            spbProgress.CurrentValue = Master.CurrentPage;
            spbProgress.Visible = (Master.CurrentPage != 1 && Master.CurrentPage != 99);

            Master.PropertyShortCodeOverride = () =>
            {
                if (Master.ForceSpecificProperty != GCCPropertyShortCode.None)
                {
                    return Master.ForceSpecificProperty;
                }
                object property = Page.RouteData.Values["propertyshortcode"];
                if (property != null && Convert.ToString(property) != "rr")
                {
                    GCCPropertyShortCode sc;
                    string strProp = property.ToString();
                    if (Enum.TryParse<GCCPropertyShortCode>(strProp.ToUpper(), out sc))
                    {
                        if (sc != GCCPropertyShortCode.GCC)
                        {
                            return sc;
                        }
                        else
                        {
                            string selectedProp = fbkProperty.SelectedValue;
                            if (selectedProp != null)
                            {
                                if (selectedProp.Length > 2 && selectedProp.Substring(0, 2).Equals("13"))
                                {
                                    //GAG
                                    return GCCPropertyShortCode.GAG;
                                }
                                try
                                {
                                    return (GCCPropertyShortCode)Conversion.StringToInt(selectedProp, 1);
                                }
                                catch
                                {
                                    return GCCPropertyShortCode.GCC;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Get check for query string GAG version
                        if (strProp.Length > 3 && strProp.Substring(0, 3).Equals("GAG"))
                        {
                            return GCCPropertyShortCode.GAG;
                        }
                    }
                }
                return GCCPropertyShortCode.GCC;
            };
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string[] answerLabels = new string[] { "Very Poor", "Poor", "Moderate", "Strong", "Very Strong" };
            CSR_Q3.SetAnswerLabels(answerLabels);
            CSR_Q6.SetAnswerLabels(answerLabels);

            answerLabels = new string[] { "N/A", "Not important to me", "Somewhat important to me", "Very important to me" };
            CSR_Q4.SetAnswerLabels(answerLabels);

            answerLabels = new string[] { "N/A", "Not at all", "Somewhat", "Very" };
            CSR_Q7.SetAnswerLabels(answerLabels);

            answerLabels = new string[] { "N/A", "Very Poor", "Poor", "Moderate", "Good", "Very Good" };
            CSR_Q9A.SetAnswerLabels(answerLabels);
            CSR_Q9B.SetAnswerLabels(answerLabels);
            CSR_Q9C.SetAnswerLabels(answerLabels);
            CSR_Q9D.SetAnswerLabels(answerLabels);
            CSR_Q9E.SetAnswerLabels(answerLabels);
            CSR_Q9F.SetAnswerLabels(answerLabels);
            CSR_Q9G.SetAnswerLabels(answerLabels);
            CSR_Q9H.SetAnswerLabels(answerLabels);

            answerLabels = new string[] { "N/A", "1 - Very Uncomfortable", "2", "3", "4", "5 - Very Comfortable" };
            CSR_Q10A.SetAnswerLabels(answerLabels);
            CSR_Q10B.SetAnswerLabels(answerLabels);
            CSR_Q10C.SetAnswerLabels(answerLabels);
            CSR_Q10D.SetAnswerLabels(answerLabels);
            CSR_Q10E.SetAnswerLabels(answerLabels);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Title = "Snapshot 2017 Survey &raquo; " + Master.CasinoName;

            if (QueryPropertyShortCode == GCCPropertyShortCode.GAG
                && QueryGAGLocation == GLocation.None)
            {
                foreach (ListItem li in fbkProperty.Items)
                {
                    li.Enabled = (li.Value == String.Empty || li.Value.StartsWith("13-"));
                }
            }

            int ind = 1;
            int propID = (int)Master.PropertyShortCode;
            //Be sure to also update Reports/SnapshotExport.aspx.cs > ddlProperty_SelectedIndexChanged


            //All
            ddlDepartment.Items[ind++].Enabled = (new int[] {  16 }.Contains(propID));


         

            //Accounting / Receiving / Human Resources
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            ////Administration & Miscellaneous
            //ddlDepartment.Items[ind++].Enabled = (propID == 12);
            //Banquets
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //BC Operations & Development Management
            ddlDepartment.Items[ind++].Enabled = (propID == 1);
            //Bingo, Cage & Slots
            ddlDepartment.Items[ind++].Enabled = (new int[] { 8, 10 }.Contains(propID));
            //Cage & Countroom
            ddlDepartment.Items[ind++].Enabled = (propID == 12);
            //Cage / Count
            ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 6, 7, 19, 18, 17,20 }.Contains(propID));
            //Cage / Count, Surveillance & Security
            ddlDepartment.Items[ind++].Enabled = (propID == 13);

            //Cage/Countroom/Guest Services
            ddlDepartment.Items[ind++].Enabled = (propID == 11);
            //Casino: Guest Services
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            ////Casino Guest Services & Entertainment & Spa
            //ddlDepartment.Items[ind++].Enabled = (propID == 19);

            //Casino Guest Services including GS Manager and Marketing Coordintor
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            ////Casino Housekeeping
            //ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Casino Housekeeping including Leads
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            
            //Casino Operations
            ddlDepartment.Items[ind++].Enabled = (propID == 12);
            //Corporate Support Services
            ddlDepartment.Items[ind++].Enabled = (propID == 1);
            //Culinary
            ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 12, 19 }.Contains(propID));
            //Culinary / Food & Beverage Management & Admin
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            //Culinary / Stewarding
            ddlDepartment.Items[ind++].Enabled = (propID == 11);
            //Executive & Senior Management
            ddlDepartment.Items[ind++].Enabled = (propID == 1);
            //Exec. Hskper, HGS Mgr, Exec Chef, Fac. Mgr, Buf. Mgr, Pub Mgr, Banq. Mgr, S&C Mgr, S&C Coord, Hot Hskping 
            ddlDepartment.Items[ind++].Enabled = (propID == 19);

            //Facilities
            ddlDepartment.Items[ind++].Enabled = (new int[] { 17, 20 }.Contains(propID));
            //Facilities / Maintenance
            ddlDepartment.Items[ind++].Enabled = (propID == 18);
            //Finance & Accounting
            ddlDepartment.Items[ind++].Enabled = (propID == 1);
            //Finance /Database Analyst / Human Resources/IT(TSG)
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Food & Beverage
            ddlDepartment.Items[ind++].Enabled = (new int[] { 3, 14, 5, 6, 7, 8, 9, 10, 12, 13,  18,20 }.Contains(propID));
            //Food & Beverage Culinary
            ddlDepartment.Items[ind++].Enabled = (propID == 15);
            //Food & Beverage Buffet including Buffet Sups
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Food & Beverage Pub & Beverage including Pub Sups and Bev Hostesses
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Food & Beverage: Outlets
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            //Food & Beverage: Banquets
            ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 11 }.Contains(propID));
            //Food & Beverage: Beverage
            ddlDepartment.Items[ind++].Enabled = (propID == 11);
            //Food & Beverage: Restaurants
            ddlDepartment.Items[ind++].Enabled = (propID == 11);
            //Food & Beverage: Gaming
            ddlDepartment.Items[ind++].Enabled = (propID == 15);
            
            //Food & Beverage: Non gaming
            ddlDepartment.Items[ind++].Enabled = (propID == 15);
            //Gaming Operations: Bingo, Slots and Cage
            ddlDepartment.Items[ind++].Enabled = (propID == 9);
            //Gaming Operations: Bingo, F&B
            ddlDepartment.Items[ind++].Enabled = (propID == 21);
            //Guest Services
            ddlDepartment.Items[ind++].Enabled = (new int[] { 18, 20 }.Contains(propID));
            //Guest Services & Retail
            ddlDepartment.Items[ind++].Enabled = (propID == 3);
            
            //ddlDepartment.Items[ind++].Enabled = (propID == 4);

            //Guest Services & Slots
            ddlDepartment.Items[ind++].Enabled = (propID == 14);
            //Guest Services/Slots
            ddlDepartment.Items[ind++].Enabled = (new int[] { 6, 7 }.Contains(propID));
            ////Hotel Ops Mrgs & Sups, Sales & Catering, Fac. Lead, Hotel Hskping Lead
            //ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Hotel: Reservations, Front Office, Concierge and Guest Services
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            //Hotel: Res., Concierge and GS, Night Audit, GS Supervisor 
            ddlDepartment.Items[ind++].Enabled = (propID == 19);           
            //Housekeeping
            ddlDepartment.Items[ind++].Enabled = (propID == 2);           
            ////Hotel Housekeeping
            //ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Hotel Housekeeping including Lead and supervisor
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Housekeeping/Maintenance/Facilities
            ddlDepartment.Items[ind++].Enabled = (propID == 11);
            //HPI & Customer Service
            ddlDepartment.Items[ind++].Enabled = (propID == 5);
            //Human Resources & Payroll
            ddlDepartment.Items[ind++].Enabled = (propID == 1);

            //Human Resources,Accounting, Maintenance (Prperty)
            ddlDepartment.Items[ind++].Enabled = (propID == 3);


            //Janitorial & Maintenance
            ddlDepartment.Items[ind++].Enabled = (propID == 15);


            ////Leadership / Administrative Assistant
            //ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Leadership, Admin Assist Mktg Mgr, Cage Mgr, Spa Mgr, Asian Host, Receiving, IT Mgr, Sec. Mgr, db Mgr, Ent.
            ddlDepartment.Items[ind++].Enabled = (propID == 19);

            //Marketing & Player Development
            ddlDepartment.Items[ind++].Enabled = (propID == 17);
            //Mutuels
            ddlDepartment.Items[ind++].Enabled = (propID == 5);
            //Mutuels, Security & Facilities
            ddlDepartment.Items[ind++].Enabled = (propID == 15);
            //Mutuels & Racing
            ddlDepartment.Items[ind++].Enabled = (propID == 14);
            //Operations Management
            ddlDepartment.Items[ind++].Enabled = (new int[] {  14, 6, 7, 8, 9, 10,13, 18,20 }.Contains(propID));
            //Operations Management/Admin
            ddlDepartment.Items[ind++].Enabled = (new int[] { 12, 15 }.Contains(propID));
            
            //Operations Management (HR, Audit, IT, Managers)
            ddlDepartment.Items[ind++].Enabled = (propID == 17);
            //Operations Management & Marketing
            ddlDepartment.Items[ind++].Enabled = (propID == 5);


            //Operations Support
            ddlDepartment.Items[ind++].Enabled = (propID == 18);
            //Operations Support (HR/Audit/IT)
            ddlDepartment.Items[ind++].Enabled = (propID == 20);
            //Operations Support & Specialists
            ddlDepartment.Items[ind++].Enabled = (propID == 11);
            //Ops Management & Department Heads
            ddlDepartment.Items[ind++].Enabled = (propID == 11);
            
            //ddlDepartment.Items[ind++].Enabled = (propID == 4);

            ////Property / Janitorial
            //ddlDepartment.Items[ind++].Enabled = (propID == 15);

            //Property Services
            ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 5, 12 }.Contains(propID));
            //Racing and First Aid
            ddlDepartment.Items[ind++].Enabled = (propID == 5);
            
            //ddlDepartment.Items[ind++].Enabled = (propID == 4);

            //Racing/Race Office
            //ddlDepartment.Items[ind++].Enabled = (propID == 14);
            //Resort Management
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            //Sales, Marketing & Player Relations
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            //Sales, Marketing, Buyer
            ddlDepartment.Items[ind++].Enabled = (propID == 3);

            //Security
            ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 6, 7, 8, 9, 10, 11, 12, 18, 17,20 }.Contains(propID));
            ////Security / Surveillance
            //ddlDepartment.Items[ind++].Enabled = (propID == 13);
            //Security (Incl. Event Safety Officers) 
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Senior Management
            ddlDepartment.Items[ind++].Enabled = (new int[] { 2 }.Contains(propID));

            //Senior Management (includes DHs)
            ddlDepartment.Items[ind++].Enabled = (new int[] { 3 }.Contains(propID));

            //Slot Operations
            ddlDepartment.Items[ind++].Enabled = (propID == 17);
            //Slots
            ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 11, 18,20 }.Contains(propID));
            //Slots (Includes Slot Techs)
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Slots & Guest Services
            ddlDepartment.Items[ind++].Enabled = (propID == 5);
            //Spa Aesticians & Lead
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Stewarding
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            //Surveillance
            ddlDepartment.Items[ind++].Enabled = (new int[] { 2, 3, 14, 5, 6, 7, 11, 12, 18, 17,20,19 }.Contains(propID));
            //Table Games
            ddlDepartment.Items[ind++].Enabled = (new int[] { 14, 6, 7, 11, 13, 18 }.Contains(propID));
            //Table Games & Customer Loyalty
            ddlDepartment.Items[ind++].Enabled = (propID == 3);
            //Table Games: Dealers
            ddlDepartment.Items[ind++].Enabled = (new int [] {2,20}.Contains(propID));
            //Table Games: Dealers & Dual Supervisors
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Table Games: Dealer Supervisors
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            //Table Games Management
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            ////Table Games Management & Slot Supervisors
            //ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Table Games Management including CSMs and Pit/Dual Pit Managers, Full Table Sups
            ddlDepartment.Items[ind++].Enabled = (propID == 19);
            //Table GamesSups/PitMan/CSM
            ddlDepartment.Items[ind++].Enabled = (propID == 20);
            //Technology Services Group
            ddlDepartment.Items[ind++].Enabled = (propID == 1);
            //Theatre
            ddlDepartment.Items[ind++].Enabled = (propID == 2);
            //Theatre: Box Office, Ushers
            ddlDepartment.Items[ind++].Enabled = (propID == 3);

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
                    if (Master.CurrentPage == 10 && Master.RedirectDirection == 1)
                    {
                        nextPage = 99;
                    }
                    Response.Redirect(GetURL(nextPage, Master.RedirectDirection), true);
                    return;
                }
                //If we've made it to 99, save to database.
                if (Master.CurrentPage == 99 && !IsPostBack)
                {
                    if (SaveData())
                    {
                        SurveyComplete = true;
                        Session.Abandon();
                        mmLastPage.SuccessMessage = "If you are on a shared computer, please click the \"Start Over\" button to get the survey ready for the next colleague who will take the survey. If you are not on a shared computer simply exit out of the survey.<br />Have a GREAT day!";
                        mmLastPage.TitleOverride = "You have successfully completed the Great Canadian Snapshot Survey. Thank you for answering our questions.  We value and appreciate your feedback.";

                        spbProgress.CurrentValue = spbProgress.MaxValue;
                        spbProgress.Visible = true;
                    }
                    else
                    {
                        mmLastPage.ErrorMessage = "We were unable to save your responses. Please go back and try again.";
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
                    prevPage = 10;
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
                if (nextPage > 10)
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

                    break;

                    #endregion Page 1

                case 2:

                    #region Page 2

                    if (QueryPropertyShortCode == GCCPropertyShortCode.GCC || (QueryPropertyShortCode == GCCPropertyShortCode.GAG && QueryGAGLocation == GLocation.None))
                    {
                        if (currentPage)
                        {
                            SurveyTools.SaveValue(fbkProperty);
                        }
                        if (!saveOnly)
                        {
                            if (fbkProperty.SelectedIndex == 0)
                            {
                                fbkProperty.MessageManager.ErrorMessage = "Please select an answer.";
                                retVal = false;
                            }
                        }
                    }
                    break;

                    #endregion Page 2

                case 3: // Department

                    #region Page 3

                    if (currentPage)
                    {
                        SurveyTools.SaveValue(ddlDepartment);
                    }
                    if (!saveOnly)
                    {
                        if (ddlDepartment.SelectedIndex == 0)
                        {
                            ddlDepartment.MessageManager.ErrorMessage = "Please select your department.";
                            retVal = false;
                        }
                    }
                    break;

                    #endregion Page 3

                case 4: // Main survey

                    #region Page 4

                    if (currentPage)
                    {
                        SurveyTools.SaveValue(Q1);
                        SurveyTools.SaveValue(Q2);
                        SurveyTools.SaveValue(Q3);
                        SurveyTools.SaveValue(Q4);
                        SurveyTools.SaveValue(Q5);
                        SurveyTools.SaveValue(Q6);
                        SurveyTools.SaveValue(Q7);
                    }
                    if (!saveOnly)
                    {
                        if (!SurveyTools.CheckForAnswer(Q1, true)
                            | !SurveyTools.CheckForAnswer(Q2, true)
                            | !SurveyTools.CheckForAnswer(Q3, true)
                            | !SurveyTools.CheckForAnswer(Q4, true)
                            | !SurveyTools.CheckForAnswer(Q5, true)
                            | !SurveyTools.CheckForAnswer(Q6, true)
                            | !SurveyTools.CheckForAnswer(Q7, true))
                        {
                            retVal = false;
                        }
                    }
                    break;

                    #endregion Page 4

                case 5: // Main survey

                    #region Page 5

                    if (currentPage)
                    {
                        SurveyTools.SaveValue(Q8);
                        SurveyTools.SaveValue(Q9);
                        SurveyTools.SaveValue(Q10);
                        SurveyTools.SaveValue(Q11);
                        SurveyTools.SaveValue(Q12);
                        SurveyTools.SaveValue(Q13);
                        SurveyTools.SaveValue(Q14);
                        SurveyTools.SaveValue(Q15);
                        SurveyTools.SaveValue(Q16);
                        SurveyTools.SaveValue(Q17);
                    }
                    if (!saveOnly)
                    {
                        if (!SurveyTools.CheckForAnswer(Q8, true)
                            | !SurveyTools.CheckForAnswer(Q9, true)
                            | !SurveyTools.CheckForAnswer(Q10, true)
                            | !SurveyTools.CheckForAnswer(Q11, true)
                            | !SurveyTools.CheckForAnswer(Q12, true)
                            | !SurveyTools.CheckForAnswer(Q13, true)
                            | !SurveyTools.CheckForAnswer(Q14, true)
                            | !SurveyTools.CheckForAnswer(Q15, true)
                            | !SurveyTools.CheckForAnswer(Q16, true)
                            | !SurveyTools.CheckForAnswer(Q17, true))
                        {
                            retVal = false;
                        }
                    }
                    break;

                    #endregion Page 5

                case 6: // Main survey

                    #region Page 6

                    if (currentPage)
                    {
                        SurveyTools.SaveValue(Q18);
                        SurveyTools.SaveValue(Q19);
                        SurveyTools.SaveValue(Q20);
                        SurveyTools.SaveValue(Q21);
                        SurveyTools.SaveValue(Q22);
                        SurveyTools.SaveValue(Q23);
                    }
                    if (!saveOnly)
                    {
                        if (!SurveyTools.CheckForAnswer(Q18, true)
                            | !SurveyTools.CheckForAnswer(Q19, true)
                            | !SurveyTools.CheckForAnswer(Q20, true)
                            | !SurveyTools.CheckForAnswer(Q21, true)
                            | !SurveyTools.CheckForAnswer(Q22, true)
                            | !SurveyTools.CheckForAnswer(Q23, true))
                        {
                            retVal = false;
                        }
                    }
                    break;

                    #endregion Page 6

                case 7: // Main survey

                    #region Page 7

                    if (currentPage)
                    {
                        SurveyTools.SaveValue(Q24);
                        SurveyTools.SaveValue(Q25);
                        SurveyTools.SaveValue(Q26);
                        SurveyTools.SaveValue(Q27);
                        SurveyTools.SaveValue(Q28);
                        SurveyTools.SaveValue(Q29A);
                        SurveyTools.SaveValue(Q29B);
                        SurveyTools.SaveValue(Q29C);
                        SurveyTools.SaveValue(Q30);
                    }
                    if (!saveOnly)
                    {
                        if (!SurveyTools.CheckForAnswer(Q24, true)
                            | !SurveyTools.CheckForAnswer(Q25, true)
                            | !SurveyTools.CheckForAnswer(Q26, true)
                            | !SurveyTools.CheckForAnswer(Q27, true)
                            | !SurveyTools.CheckForAnswer(Q28, true)
                            | !SurveyTools.CheckForAnswer(Q29A, true)
                            | !SurveyTools.CheckForAnswer(Q29B, true)
                            | !SurveyTools.CheckForAnswer(Q29C, true)
                            | !SurveyTools.CheckForAnswer(Q30, true))
                        {
                            retVal = false;
                        }
                    }
                    break;

                    #endregion Page 7

                case 8: // Demographics

                    #region Page 8

                    if (currentPage)
                    {
                        SurveyTools.SaveRadioButtons(radQ31_Hourly, radQ31_Salary);
                        SurveyTools.SaveRadioButtons(radQ32_1, radQ32_2, radQ32_3, radQ32_4, radQ32_5);
                        SurveyTools.SaveValue(Q33);
                        SurveyTools.SaveValue(chkQ34_1);
                        SurveyTools.SaveValue(chkQ34_2);
                        SurveyTools.SaveValue(chkQ34_3);
                        SurveyTools.SaveValue(chkQ34_4);
                        SurveyTools.SaveValue(chkQ34_5);
                        SurveyTools.SaveValue(chkQ34_6);
                        SurveyTools.SaveValue(chkQ34_7);
                        SurveyTools.SaveValue(chkQ34_8);
                        SurveyTools.SaveValue(chkQ34_9);
                        SurveyTools.SaveValue(chkQ34_10);
                        SurveyTools.SaveValue(chkQ34_11);
                        SurveyTools.SaveValue(chkQ34_12);
                    }
                    if (!saveOnly)
                    {
                        bool payTypeNotSelected = !SurveyTools.GetValue(radQ31_Hourly, currentPage, false) &&
                                                  !SurveyTools.GetValue(radQ31_Salary, currentPage, false);

                        if (payTypeNotSelected)
                        {
                            radQ31_Hourly.MessageManager.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }

                        bool tenureNotSelected = !SurveyTools.GetValue(radQ32_1, currentPage, false) &&
                                                 !SurveyTools.GetValue(radQ32_2, currentPage, false) &&
                                                 !SurveyTools.GetValue(radQ32_3, currentPage, false) &&
                                                 !SurveyTools.GetValue(radQ32_4, currentPage, false) &&
                                                 !SurveyTools.GetValue(radQ32_5, currentPage, false);
                        if (tenureNotSelected)
                        {
                            radQ32_1.MessageManager.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }
                    }
                    break;

                    #endregion Page 8

                case 9: // First Final Confirmation

                    #region Page 9

                    if (currentPage)
                    {
                        SurveyTools.SaveRadioButtons(radFFNoThanks, radFFContinue);
                    }
                    if (!saveOnly)
                    {
                        bool continueNotSelected = !SurveyTools.GetValue(radFFNoThanks, currentPage, false) &&
                                                    !SurveyTools.GetValue(radFFContinue, currentPage, false);

                        if (continueNotSelected)
                        {
                            radFFNoThanks.MessageManager.ErrorMessage = "Please select one of the following options.";
                            retVal = false;
                        }
                    }
                    break;

                    #endregion Page 9

                case 10: // CSR

                    #region Page 10

                    if (radFFContinue.Checked)
                    {
                        if (currentPage)
                        {
                            SurveyTools.SaveValue(CSR_Q1);
                            SurveyTools.SaveValue(CSR_Q2);
                            SurveyTools.SaveValue(CSR_Q3);
                            SurveyTools.SaveValue(CSR_Q4);
                            SurveyTools.SaveValue(CSR_Q5_1);
                            SurveyTools.SaveValue(CSR_Q5_2);
                            SurveyTools.SaveValue(CSR_Q5_3);
                            SurveyTools.SaveValue(CSR_Q5_4);
                            SurveyTools.SaveValue(CSR_Q6);
                            SurveyTools.SaveValue(CSR_Q7);
                            SurveyTools.SaveValue(CSR_Q8_1);
                            SurveyTools.SaveValue(CSR_Q8_2);
                            SurveyTools.SaveValue(CSR_Q8_3);
                            SurveyTools.SaveValue(CSR_Q8_4);
                            SurveyTools.SaveValue(CSR_Q8_5);
                            SurveyTools.SaveValue(CSR_Q8_6);
                            SurveyTools.SaveValue(CSR_Q8_7);
                            SurveyTools.SaveValue(CSR_Q8_8);
                            SurveyTools.SaveValue(CSR_Q8_9);
                            SurveyTools.SaveValue(CSR_Q8_OtherExplanation);
                            SurveyTools.SaveValue(CSR_Q9A);
                            SurveyTools.SaveValue(CSR_Q9B);
                            SurveyTools.SaveValue(CSR_Q9C);
                            SurveyTools.SaveValue(CSR_Q9D);
                            SurveyTools.SaveValue(CSR_Q9E);
                            SurveyTools.SaveValue(CSR_Q9F);
                            SurveyTools.SaveValue(CSR_Q9G);
                            SurveyTools.SaveValue(CSR_Q9H);
                            SurveyTools.SaveValue(CSR_Q10A);
                            SurveyTools.SaveValue(CSR_Q10B);
                            SurveyTools.SaveValue(CSR_Q10C);
                            SurveyTools.SaveValue(CSR_Q10D);
                            SurveyTools.SaveValue(CSR_Q10E);
                            SurveyTools.SaveRadioButtons(radCSR_Q11_1, radCSR_Q11_2, radCSR_Q11_3, radCSR_Q11_4);
                            SurveyTools.SaveValue(CSR_Q12);
                            SurveyTools.SaveValue(CSR_Q13);
                        }
                        if (!saveOnly)
                        {
                            if (!SurveyTools.CheckForAnswer(CSR_Q1, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q2, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q3, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q4, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q6, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q7, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q9A, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q9B, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q9C, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q9D, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q9E, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q9F, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q9G, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q9H, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q10A, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q10B, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q10C, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q10D, true)
                                | !SurveyTools.CheckForAnswer(CSR_Q10E, true)
                                | (!(new GCCPropertyShortCode[] { GCCPropertyShortCode.GAG, GCCPropertyShortCode.FL, GCCPropertyShortCode.GD, GCCPropertyShortCode.CNB }).Contains(Master.PropertyShortCode) && !SurveyTools.CheckForAnswer(CSR_Q12, true))
                                )
                            {
                                retVal = false;
                            }
                            bool informedNotSelected = !SurveyTools.GetValue(radCSR_Q11_1, currentPage, false) &&
                                                       !SurveyTools.GetValue(radCSR_Q11_2, currentPage, false) &&
                                                       !SurveyTools.GetValue(radCSR_Q11_3, currentPage, false) &&
                                                       !SurveyTools.GetValue(radCSR_Q11_4, currentPage, false);
                            if (informedNotSelected)
                            {
                                radCSR_Q11_4.MessageManager.ErrorMessage = "Please select one of the following options.";
                                retVal = false;
                            }

                            if (CSR_Q8_9.Checked && String.IsNullOrEmpty(CSR_Q8_OtherExplanation.Text))
                            {
                                CSR_Q8_9.MessageManager.ErrorMessage = "Please explain why you chose \"Other\".";
                                retVal = false;
                            }
                        }
                    }
                    break;

                    #endregion Page 10
            }
            return retVal;
        }

        private bool PageShouldBeSkipped(int CurrentPage)
        {
            switch (CurrentPage)
            {
                case 2:
                    return !(QueryPropertyShortCode == GCCPropertyShortCode.GCC || (QueryPropertyShortCode == GCCPropertyShortCode.GAG && QueryGAGLocation == GLocation.None));

                case 10:
                    return !radFFContinue.Checked;
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
            return String.Format("/Snapshot{4}/{0}{5}/{1}{2}{3}",
                                    QueryPropertyShortCode.ToString(),
                                    page,
                                    (redirDir == -1 ? "/-1" : String.Empty),
                                    (isReset ? "?r=1" : String.Empty),
                                    IsKioskSurvey ? "K" : String.Empty,
                                    QueryGAGLocation == GLocation.None ? String.Empty : QueryGAGLocation.ToString().Substring(0, 1)
                                );
        }

        protected bool SaveData()
        {
            StringBuilder columnList = new StringBuilder();
            SQLParamList sqlParams = new SQLParamList();

            if (QueryPropertyShortCode == GCCPropertyShortCode.GCC || (QueryPropertyShortCode == GCCPropertyShortCode.GAG && QueryGAGLocation == GLocation.None))
            {
                //User had to select location
                string propSel = fbkProperty.SelectedValue;
                if (propSel.Length > 3)
                {
                    //GAG
                    if (propSel.Substring(0, 2).Equals("13"))
                    {
                        columnList.AppendFormat(",[{0}]", fbkProperty.DBColumn);
                        sqlParams.Add("@" + fbkProperty.DBColumn, 13);
                    }
                }
                else
                {
                    fbkProperty.PrepareQuestionForDB(columnList, sqlParams);
                }
            }

            ddlDepartment.PrepareQuestionForDB(columnList, sqlParams);
            Q1.PrepareQuestionForDB(columnList, sqlParams);
            Q2.PrepareQuestionForDB(columnList, sqlParams);
            Q3.PrepareQuestionForDB(columnList, sqlParams);
            Q4.PrepareQuestionForDB(columnList, sqlParams);
            Q5.PrepareQuestionForDB(columnList, sqlParams);
            Q6.PrepareQuestionForDB(columnList, sqlParams);
            Q7.PrepareQuestionForDB(columnList, sqlParams);
            Q8.PrepareQuestionForDB(columnList, sqlParams);
            Q9.PrepareQuestionForDB(columnList, sqlParams);
            Q10.PrepareQuestionForDB(columnList, sqlParams);
            Q11.PrepareQuestionForDB(columnList, sqlParams);
            Q12.PrepareQuestionForDB(columnList, sqlParams);
            Q13.PrepareQuestionForDB(columnList, sqlParams);
            Q14.PrepareQuestionForDB(columnList, sqlParams);
            Q15.PrepareQuestionForDB(columnList, sqlParams);
            Q16.PrepareQuestionForDB(columnList, sqlParams);
            Q17.PrepareQuestionForDB(columnList, sqlParams);
            Q18.PrepareQuestionForDB(columnList, sqlParams);
            Q19.PrepareQuestionForDB(columnList, sqlParams);
            Q20.PrepareQuestionForDB(columnList, sqlParams);
            Q21.PrepareQuestionForDB(columnList, sqlParams);
            Q22.PrepareQuestionForDB(columnList, sqlParams);
            Q23.PrepareQuestionForDB(columnList, sqlParams);
            Q24.PrepareQuestionForDB(columnList, sqlParams);
            Q25.PrepareQuestionForDB(columnList, sqlParams);
            Q26.PrepareQuestionForDB(columnList, sqlParams);
            Q27.PrepareQuestionForDB(columnList, sqlParams);
            Q28.PrepareQuestionForDB(columnList, sqlParams);
            Q29A.PrepareQuestionForDB(columnList, sqlParams);
            Q29B.PrepareQuestionForDB(columnList, sqlParams);
            Q29C.PrepareQuestionForDB(columnList, sqlParams);
            Q30.PrepareQuestionForDB(columnList, sqlParams);
            radQ31_Hourly.PrepareQuestionForDB(columnList, sqlParams);
            radQ31_Salary.PrepareQuestionForDB(columnList, sqlParams);
            radQ32_1.PrepareQuestionForDB(columnList, sqlParams);
            radQ32_2.PrepareQuestionForDB(columnList, sqlParams);
            radQ32_3.PrepareQuestionForDB(columnList, sqlParams);
            radQ32_4.PrepareQuestionForDB(columnList, sqlParams);
            radQ32_5.PrepareQuestionForDB(columnList, sqlParams);
            Q33.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_1.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_2.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_3.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_4.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_5.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_6.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_7.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_8.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_9.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_10.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_11.PrepareQuestionForDB(columnList, sqlParams);
            chkQ34_12.PrepareQuestionForDB(columnList, sqlParams);

            if (radFFContinue.Checked)
            {
                CSR_Q1.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q2.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q3.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q4.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q5_1.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q5_2.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q5_3.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q5_4.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q6.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q7.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q8_1.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q8_2.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q8_3.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q8_4.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q8_5.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q8_6.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q8_7.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q8_8.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q8_9.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q8_OtherExplanation.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q9A.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q9B.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q9C.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q9D.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q9E.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q9F.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q9G.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q9H.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q10A.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q10B.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q10C.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q10D.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q10E.PrepareQuestionForDB(columnList, sqlParams);
                radCSR_Q11_1.PrepareQuestionForDB(columnList, sqlParams);
                radCSR_Q11_2.PrepareQuestionForDB(columnList, sqlParams);
                radCSR_Q11_3.PrepareQuestionForDB(columnList, sqlParams);
                radCSR_Q11_4.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q12.PrepareQuestionForDB(columnList, sqlParams);
                CSR_Q13.PrepareQuestionForDB(columnList, sqlParams);
            }

            columnList.Append(",[PropertyID],[DateEntered],[SurveyType]");
            sqlParams.Add("@PropertyID", Master.PropertyID)
                     .Add("@DateEntered", DateTime.Now)
                     .Add("@SurveyType", IsKioskSurvey ? "K" : "D");
            if (GAGLocation != GLocation.None)
            {
                columnList.Append(",[GAGLocation]");
                sqlParams.Add("@GAGLocation", GAGLocation.ToString());
            }

            columnList.Remove(0, 1);
            SQLDatabase sql = new SQLDatabase();
            int rowID = sql.QueryAndReturnIdentity(String.Format("INSERT INTO [tblSurveySnapshot2017] ({0}) VALUES ({1});", columnList, columnList.ToString().Replace("[", "@").Replace("]", String.Empty)), sqlParams);
            if (!sql.HasError && rowID != -1)
            {
                Dictionary<string, int> wordCounts = SurveyTools.GetWordCount(Q33.Text, CSR_Q8_OtherExplanation.Text, CSR_Q13.Text);
                SurveyTools.SaveWordCounts(SharedClasses.SurveyType.Employee, rowID, wordCounts);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}