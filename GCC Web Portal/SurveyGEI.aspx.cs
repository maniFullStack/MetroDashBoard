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
	public partial class SurveyGEI : BasePage
	{
		protected string strSurveyLang { get; set; }
		
		protected bool SurveyComplete { get; set; }

		protected DataRow EmailPINRow { get; set; }

		protected string HeaderTitle { get; set; }

		protected string FrenchEntertainment { get; set; }

		// REC - Added this for Staging Site to replace hard-coding URLs
		private string GCCPortalUrl = ConfigurationManager.AppSettings["GCCPortalURL"].ToString();

		protected UserInfo User {
			get {
				return SessionWrapper.Get<UserInfo>( "UserInfo", null );
			}
		}

		public GCCPropertyShortCode ForceSpecificProperty { get; set; }

		/// <summary>
		/// Gets the property short code for the current request.
		/// </summary>
		public GCCPropertyShortCode PropertyShortCode
		{
			get {
				if ( ForceSpecificProperty != GCCPropertyShortCode.None ) {
					return ForceSpecificProperty;
				}
				object property = Page.RouteData.Values["propertyshortcode"];
				if (property != null)
				{
					GCCPropertyShortCode sc;
					if (Enum.TryParse<GCCPropertyShortCode>(property.ToString().ToUpper(), out sc)) {
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
		
		/// <summary>
		/// Gets the property ID to load the theme CSS file for.
		/// </summary>
		public int PropertyID   
		{
			get
			{
				return (int)PropertyShortCode;
			}
		}


		/// <summary>
		/// The property associated with this survey.
		/// </summary>
		public GCCProperty ThemeProperty
		{
			get
			{
				return (GCCProperty)PropertyID;
			}
		}

		/// <summary>
		/// Gets the GUID PIN for the email surveys.
		/// </summary>
		public Guid EmailPIN {
			get {
				object pin = Page.RouteData.Values["pin"];
				if ( pin != null ) {
					Guid gPin;
					if ( Guid.TryParse( pin.ToString(), out gPin ) ) {
						return gPin;
					}
					return Guid.Empty;
				} else {
					return Guid.Empty;
				}
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
		/// Gets the casino's name. Short for calling SharedClasses.Config.GetCasinoName(PropertyID).
		/// </summary>
		protected string CasinoName {
			get {
				if (strSurveyLang != "French")
				{
					return SharedClasses.PropertyTools.GetCasinoName(PropertyID);
				}
				else
				{
					return SharedClasses.PropertyTools.GetCasinoName_French(PropertyID);
				}
			}
		}

		/// <summary>
		/// Will return true if the survey questions should be mandatory (where applicable).
		/// </summary>
		public bool QuestionsAreMandatory {
			get {
				return !IsKioskOrStaffEntry;
			}
		}
		/// <summary>
		/// Returns true if this is a kiosk or staff entry survey.
		/// </summary>
		public bool IsKioskOrStaffEntry {
			get {
				return SurveyType == SharedClasses.GEISurveyType.Kiosk || SurveyType == SharedClasses.GEISurveyType.StaffSurvey;
			}
		}

		/// <summary>
		/// Returns 1 or -1 depending on which direction the redirect is going. This is used to manage skips during presses of the Prev / Next buttons.
		/// </summary>
		protected int RedirectDirection {
			get {
				object rdo = Page.RouteData.Values["redirectdirection"];
				if ( rdo == null ) { return 1; }
				int rd = Conversion.StringToInt( rdo.ToString(), 1 );
				return ( rd == 1 || rd == -1 ) ? rd : 1;
			}
		}

		/// <summary>
		/// Gets the name of the food and beverage location for this casino, for a specific mention number (1-13). Returns blank for any unmatched values.
		/// </summary>
		/// <param name="mentionNumber"></param>
		/// <returns></returns>
		protected string GetFoodAndBevName( int mentionNumber ) {
			return PropertyTools.GetFoodAndBevName( PropertyShortCode, mentionNumber );
		}


		protected string GetFoodAndBevName_French(int mentionNumber)
		{
			return PropertyTools.GetFoodAndBevName_French(PropertyShortCode, mentionNumber);
		}


		public string GetShowLoungeName() {
			return GetShowLoungeName( false );
		}


		


		public string GetShowLoungeName(bool checkHRCV) {

			if (strSurveyLang != "French")
			{
				return PropertyTools.GetShowLoungeName(PropertyShortCode, ((!checkHRCV || (!radQ21_HRCV_LoungeA.Checked && !radQ21_HRCV_LoungeU.Checked && !radQ21_EC_LoungeM.Checked && !radQ21_EC_LoungeE.Checked)) ? 0 : (radQ21_HRCV_LoungeA.Checked || radQ21_EC_LoungeM.Checked ? 1 : 2)));
			}
			else
			{
				return PropertyTools.GetShowLoungeName_French(PropertyShortCode, ((!checkHRCV || (!radQ21_HRCV_LoungeA_F.Checked && !radQ21_HRCV_LoungeU_F.Checked && !radQ21_EC_LoungeM_F.Checked && !radQ21_EC_LoungeE_F.Checked)) ? 0 : (radQ21_HRCV_LoungeA_F.Checked || radQ21_EC_LoungeM_F.Checked ? 1 : 2)));
			}
		}

		protected string GetCasinoURL() {
			return PropertyTools.GetCasinoURL( PropertyShortCode );
		}

		protected void Page_PreInit( object sender, EventArgs e ) {


			//// Passing Language information from staff selection page to SurveyGEI
			//GEISurveyLanguage GEILIndex;

			
			if (Session["SurveyLang"] != null)
			{

					strSurveyLang = Session["SurveyLang"].ToString();
				
			}


			if (PropertyShortCode != GCCPropertyShortCode.CNB && PropertyShortCode != GCCPropertyShortCode.SCTI && PropertyShortCode != GCCPropertyShortCode.WDB)
			{
				Session["SurveyLang"] = "English";
				strSurveyLang = "English";
			}
			else if (PropertyShortCode == GCCPropertyShortCode.CNB || PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.WDB)
			{


				//if(CurrentPage == 1)
				//{
				//    if(PINSurveyLang.SelectedValue != "French")
				//    {
				//        PINSurveyLang.ClearSelection();
				//        PINSurveyLang.Items.FindByValue("English").Selected = true;// 1 is the value of option2

				//        HeaderTitle = "Guest Experience Survey";
				//    }
				//    else
				//    {
				//        PINSurveyLang.ClearSelection();
				//        PINSurveyLang.Items.FindByValue("French").Selected = true;// 1 is the value of option2

				//        HeaderTitle = "Sondage sur l'expérience des clients";

				//    }
				//}


				//// 20171115 - adding different logic to set language filter

				if (CurrentPage == 1)
				{
					if (strSurveyLang != "French")
					{
						PINSurveyLang.ClearSelection();
						PINSurveyLang.Items.FindByValue("English").Selected = true;// 1 is the value of option2

						HeaderTitle = "Guest Experience Survey";
					}
					else
					{
						PINSurveyLang.ClearSelection();
						PINSurveyLang.Items.FindByValue("French").Selected = true;// 1 is the value of option2

						HeaderTitle = "Sondage sur l'expérience des clients";

					}
				}


			}
			


			//GEILIndex = (GEISurveyLanguage)Session["SurveyLang"];

			//System.Diagnostics.Debug.WriteLine( "Survey Page_PreInit PID: " + CurrentPage );
			//Check for a reset and abandon the session and redirect to page 1.
			if ( RequestVars.Get( "r", 0 ) == 1 ) {
				Session.Abandon();
				Response.Redirect( GetSurveyURL( 1 ), true );
				return;
			}
			//Ensure that a user is logged in for the staff survey entry
			if ( SurveyType == GEISurveyType.StaffSurvey && User == null ) {
				Response.Redirect( "/Login?rd=" + Server.UrlPathEncode( Request.Url.PathAndQuery ), true );
				return;
			}

			if ( SurveyType == GEISurveyType.Email ) {
				if ( EmailPIN.Equals( Guid.Empty ) ) {
					TopMessage.ErrorMessage = "Invalid link. Please ensure you copied the full link into the address bar.";
					return;
				}
				SQLDatabase sql = new SQLDatabase();
				DataTable dt = sql.QueryDataTable( @"
												   SELECT [BatchID],[EmailAddress],[PropertyID],[Encore],[PIN],[SurveyCompleted]
												   FROM [tblSurveyGEI_EmailPINs]
												   WHERE [PIN] = @PIN", new SqlParameter( "@PIN", EmailPIN ) );
				if ( sql.HasError ) {
					TopMessage.ErrorMessage = "Unable to verify link. Please try again.";
					return;
				} else if ( dt.Rows.Count == 0 ) {
					TopMessage.ErrorMessage = "Invalid link. Please ensure you copied the full link into the address bar.";
					return;
				} else if ( dt.Rows[0]["SurveyCompleted"].Equals( true ) ) {
					ForceSpecificProperty = (GCCPropertyShortCode)dt.Rows[0]["PropertyID"].ToString().StringToInt( 0 );
					TopMessage.ErrorMessage = "It looks like you have already completed the survey. Thank you!";
					return;
				} else {
					EmailPINRow = dt.Rows[0];
					if ( !IsPostBack ) {
						ForceSpecificProperty = (GCCPropertyShortCode)EmailPINRow["PropertyID"].ToString().StringToInt( 0 );


						if (strSurveyLang == "French")
						{
							txtEmail.Text = EmailPINRow["EmailAddress"].ToString();
							Q4.SelectedValue = String.IsNullOrWhiteSpace(EmailPINRow["Encore"].ToString()) ? 0 : 1;
							txtQ4_CardNumber.Text = EmailPINRow["Encore"].ToString();
							txtEmail_F.Text = EmailPINRow["EmailAddress"].ToString();
							Q4_F.SelectedValue_F = String.IsNullOrWhiteSpace(EmailPINRow["Encore"].ToString()) ? 0 : 1;
							txtQ4_CardNumber_F.Text = EmailPINRow["Encore"].ToString();
						}
						else
						{
							Q4_F.SelectedValue_F = String.IsNullOrWhiteSpace(EmailPINRow["Encore"].ToString()) ? 0 : 1;
							txtQ4_CardNumber_F.Text = EmailPINRow["Encore"].ToString();
							txtEmail_F.Text = EmailPINRow["EmailAddress"].ToString();
							txtEmail.Text = EmailPINRow["EmailAddress"].ToString();
							Q4.SelectedValue = String.IsNullOrWhiteSpace(EmailPINRow["Encore"].ToString()) ? 0 : 1;
							txtQ4_CardNumber.Text = EmailPINRow["Encore"].ToString();
						}
					}
				}
			}

			spbProgress.MaxValue = 22;
			spbProgress.CurrentValue = CurrentPage;
			spbProgress.Visible = ( CurrentPage != 1 //First page
									&& CurrentPage != 99 //Quit early page
									&& ( CurrentPage != 97 || !String.IsNullOrWhiteSpace( mmLastPage.SuccessMessage ) ) ); //Only show 100% on final page
		}
		protected void Page_Load( object sender, EventArgs e ) {
			if ( !IsPostBack ) {
				//Set up labels

				if (strSurveyLang != "French")
				{
					Q5A.Label = Q5A.Label.Replace("{CasinoName}", CasinoName);
					Q5B.Label = Q5B.Label.Replace("{CasinoName}", CasinoName);

					Q6A.Label = Q6A.Label.Replace("{CasinoName}", CasinoName);
					Q6B.Label = Q6B.Label.Replace("{CasinoName}", CasinoName);
					Q6C.Label = Q6C.Label.Replace("{CasinoName}", CasinoName);
					Q6D.Label = Q6D.Label.Replace("{CasinoName}", CasinoName);

					string[] answerLabels = new string[] { "N/A", "Definitely Would Not", "Probably Would Not", "Might or Might Not", "Probably Would", "Definitely Would" };
					Q6A.SetAnswerLabels(answerLabels);
					Q6B.SetAnswerLabels(answerLabels);
					Q6C.SetAnswerLabels(answerLabels);
					Q6D.SetAnswerLabels(answerLabels);

					answerLabels = new string[] { "No Interaction", "Poor", "Fair", "Good", "Very Good", "Excellent" };
					Q9A.SetAnswerLabels(answerLabels);
					Q9B.SetAnswerLabels(answerLabels);
					Q9C.SetAnswerLabels(answerLabels);
					Q9D.SetAnswerLabels(answerLabels);
					Q9E.SetAnswerLabels(answerLabels);
					Q9F.SetAnswerLabels(answerLabels);
					Q9G.SetAnswerLabels(answerLabels);
					Q9H.SetAnswerLabels(answerLabels);
					Q9I.SetAnswerLabels(answerLabels);
					Q9J.SetAnswerLabels(answerLabels);
				}

				else if(strSurveyLang == "French")
				{

					Q5A_F.Label = Q5A_F.Label.Replace("{CasinoName}", CasinoName);
					Q5B_F.Label = Q5B_F.Label.Replace("{CasinoName}", CasinoName);

					Q6A_F.Label = Q6A_F.Label.Replace("{CasinoName}", CasinoName);
					Q6B_F.Label = Q6B_F.Label.Replace("{CasinoName}", CasinoName);
					Q6C_F.Label = Q6C_F.Label.Replace("{CasinoName}", CasinoName);
					Q6D_F.Label = Q6D_F.Label.Replace("{CasinoName}", CasinoName);

					string[] answerLabels = new string[] { "N/A", "Definitely Would Not", "Probably Would Not", "Might or Might Not", "Probably Would", "Definitely Would" };
					Q6A_F.SetAnswerLabels(answerLabels);
					Q6B_F.SetAnswerLabels(answerLabels);
					Q6C_F.SetAnswerLabels(answerLabels);
					Q6D_F.SetAnswerLabels(answerLabels);

					answerLabels = new string[] { "No Interaction", "Poor", "Fair", "Good", "Very Good", "Excellent" };
					Q9A_F.SetAnswerLabels(answerLabels);
					Q9B_F.SetAnswerLabels(answerLabels);
					Q9C_F.SetAnswerLabels(answerLabels);
					Q9D_F.SetAnswerLabels(answerLabels);
					Q9E_F.SetAnswerLabels(answerLabels);
					Q9F_F.SetAnswerLabels(answerLabels);
					Q9G_F.SetAnswerLabels(answerLabels);
					Q9H_F.SetAnswerLabels(answerLabels);
					Q9I_F.SetAnswerLabels(answerLabels);
					Q9J_F.SetAnswerLabels(answerLabels);

				}
				

				#region QuestionTextReplacements/Visibility


				if (strSurveyLang != "French")
				{


					//Remove Coffee Servers as an option for RR as per Colin (May 3rd)
					Q9G.Visible = PropertyShortCode != GCCPropertyShortCode.RR;

					//Change Question 9 option for RR only as per Colin (May 3rd)
					if (PropertyShortCode == GCCPropertyShortCode.RR)
					{
						Q9F.Label = Q9F.Label.Replace("Cocktail Servers", "Beverage Servers");
					}

					//Replace text for entertainment question (Page 3) on GEI and GSEI Surveys for CNB
					if (PropertyShortCode == GCCPropertyShortCode.CNB)
					{
						radQ1_Entertainment.Text.Remove(0);
						radQ1_Entertainment.Text = "&nbsp;Watching Live Entertainment at our theatre or pub";
					}

					//Replace text for entertainment question (Page 4) on GEI and GSEI Surveys for CNB
					if (PropertyShortCode == GCCPropertyShortCode.CNB)
					{
						chkQ2_Entertainment.Text.Remove(0);
						chkQ2_Entertainment.Text = "&nbsp;Watching Live Entertainment at our theatre or pub";
					}

					radQ27A_6.Visible = PropertyShortCode != GCCPropertyShortCode.GAG;
					radQ27A_7.Visible = !new string[] { "HA", "CCH", "CMR", "CDC" }.Contains(PropertyShortCode.ToString());
					radQ27A_9.Visible = (PropertyShortCode == GCCPropertyShortCode.RR
						|| PropertyShortCode == GCCPropertyShortCode.CNB);
					radQ27A_11.Visible = (PropertyShortCode == GCCPropertyShortCode.CCH
						|| PropertyShortCode == GCCPropertyShortCode.CMR
						|| PropertyShortCode == GCCPropertyShortCode.CDC);
					radQ27A_12.Visible = (PropertyShortCode == GCCPropertyShortCode.RR
						|| PropertyShortCode == GCCPropertyShortCode.HRCV
						|| PropertyShortCode == GCCPropertyShortCode.CCH
						|| PropertyShortCode == GCCPropertyShortCode.CMR
						|| PropertyShortCode == GCCPropertyShortCode.CDC
						|| PropertyShortCode == GCCPropertyShortCode.CNSH
						|| PropertyShortCode == GCCPropertyShortCode.CNB
						|| PropertyShortCode == GCCPropertyShortCode.EC);
					radQ27A_13.Visible = (PropertyShortCode == GCCPropertyShortCode.HA
						|| PropertyShortCode == GCCPropertyShortCode.EC);

					radQ40A_6.Visible = PropertyShortCode != GCCPropertyShortCode.GAG;
					radQ40A_7.Visible = !new string[] { "HA", "CCH", "CMR", "CDC" }.Contains(PropertyShortCode.ToString());
					radQ40A_9.Visible = (PropertyShortCode == GCCPropertyShortCode.RR
						|| PropertyShortCode == GCCPropertyShortCode.CNB);
					radQ40A_11.Visible = (PropertyShortCode == GCCPropertyShortCode.CCH
						|| PropertyShortCode == GCCPropertyShortCode.CMR
						|| PropertyShortCode == GCCPropertyShortCode.CDC);
					radQ40A_12.Visible = (PropertyShortCode == GCCPropertyShortCode.RR
						|| PropertyShortCode == GCCPropertyShortCode.HRCV
						|| PropertyShortCode == GCCPropertyShortCode.CCH
						|| PropertyShortCode == GCCPropertyShortCode.CMR
						|| PropertyShortCode == GCCPropertyShortCode.CDC
						|| PropertyShortCode == GCCPropertyShortCode.CNSH
						|| PropertyShortCode == GCCPropertyShortCode.CNB
						|| PropertyShortCode == GCCPropertyShortCode.EC);
					radQ40A_13.Visible = (PropertyShortCode == GCCPropertyShortCode.HA
						|| PropertyShortCode == GCCPropertyShortCode.EC);


				}

				else if(strSurveyLang == "French")
				{


					//Remove Coffee Servers as an option for RR as per Colin (May 3rd)
					Q9G_F.Visible = PropertyShortCode != GCCPropertyShortCode.RR;

					//Change Question 9 option for RR only as per Colin (May 3rd)
					if (PropertyShortCode == GCCPropertyShortCode.RR)
					{
						Q9F_F.Label = Q9F_F.Label.Replace("Serveurs de cocktails", "Serveurs de boissons");
					}

					//Replace text for entertainment question (Page 3) on GEI and GSEI Surveys for CNB
					if (PropertyShortCode == GCCPropertyShortCode.CNB)
					{
					   

						radQ1_Entertainment_F.Text.Remove(0);
						radQ1_Entertainment_F.Text = "&nbsp;Assister à un spectacle à notre salle de spectacles ou à notre pub";
					}

					//Replace text for entertainment question (Page 4) on GEI and GSEI Surveys for CNB
					if (PropertyShortCode == GCCPropertyShortCode.CNB)
					{
						chkQ2_Entertainment_F.Text.Remove(0);
						chkQ2_Entertainment_F.Text = "&nbsp;Assister à un spectacle à notre salle de spectacles ou à notre pub";
						//Regarder un spectacle à notre salle ou du divertissement à notre pub
					}

					radQ27A_6_F.Visible = PropertyShortCode != GCCPropertyShortCode.GAG;
					radQ27A_7_F.Visible = !new string[] { "HA", "CCH", "CMR", "CDC" }.Contains(PropertyShortCode.ToString());
					radQ27A_9_F.Visible = (PropertyShortCode == GCCPropertyShortCode.RR
						|| PropertyShortCode == GCCPropertyShortCode.CNB);
					radQ27A_11_F.Visible = (PropertyShortCode == GCCPropertyShortCode.CCH
						|| PropertyShortCode == GCCPropertyShortCode.CMR
						|| PropertyShortCode == GCCPropertyShortCode.CDC);
					radQ27A_12_F.Visible = (PropertyShortCode == GCCPropertyShortCode.RR
						|| PropertyShortCode == GCCPropertyShortCode.HRCV
						|| PropertyShortCode == GCCPropertyShortCode.CCH
						|| PropertyShortCode == GCCPropertyShortCode.CMR
						|| PropertyShortCode == GCCPropertyShortCode.CDC
						|| PropertyShortCode == GCCPropertyShortCode.CNSH
						|| PropertyShortCode == GCCPropertyShortCode.CNB
						|| PropertyShortCode == GCCPropertyShortCode.EC);
					radQ27A_13_F.Visible = (PropertyShortCode == GCCPropertyShortCode.HA
						|| PropertyShortCode == GCCPropertyShortCode.EC);

					radQ40A_6_F.Visible = PropertyShortCode != GCCPropertyShortCode.GAG;
					radQ40A_7_F.Visible = !new string[] { "HA", "CCH", "CMR", "CDC" }.Contains(PropertyShortCode.ToString());
					radQ40A_9_F.Visible = (PropertyShortCode == GCCPropertyShortCode.RR
						|| PropertyShortCode == GCCPropertyShortCode.CNB);
					radQ40A_11_F.Visible = (PropertyShortCode == GCCPropertyShortCode.CCH
						|| PropertyShortCode == GCCPropertyShortCode.CMR
						|| PropertyShortCode == GCCPropertyShortCode.CDC);
					radQ40A_12_F.Visible = (PropertyShortCode == GCCPropertyShortCode.RR
						|| PropertyShortCode == GCCPropertyShortCode.HRCV
						|| PropertyShortCode == GCCPropertyShortCode.CCH
						|| PropertyShortCode == GCCPropertyShortCode.CMR
						|| PropertyShortCode == GCCPropertyShortCode.CDC
						|| PropertyShortCode == GCCPropertyShortCode.CNSH
						|| PropertyShortCode == GCCPropertyShortCode.CNB
						|| PropertyShortCode == GCCPropertyShortCode.EC);
					radQ40A_13_F.Visible = (PropertyShortCode == GCCPropertyShortCode.HA
						|| PropertyShortCode == GCCPropertyShortCode.EC);


				}


				#endregion








			 


			}



			if (PINSurveyLang.SelectedValue != "French")
			{

				English.Visible = true;
				French.Visible = false;
			}
			else
			{
				English.Visible = false;
				French.Visible = true;
				
			}


		}

		protected void Page_LoadComplete( object sender, EventArgs e ) {
			//System.Diagnostics.Debug.WriteLine( "Survey Page_LoadComplete PID: " + CurrentPage );
			//Check all previous pages
			//Must do in LoadComplete because controls load values in Load method (Init didn't work because reasons...)
			SessionWrapper.Add( "GEIPageNumber", CurrentPage );
			if ( CurrentPage > 1 && CurrentPage != 99 && !IsPostBack ) {
				for ( int i = 1; i < CurrentPage; i++ ) {
					//System.Diagnostics.Debug.WriteLine( "Checking Page: " + i );
					if ( !ValidateAndSave( i, false, false ) ) {
						//System.Diagnostics.Debug.WriteLine( "Invalid Page: " + i );
						if (CurrentPage == 97)
						{
							continue;
						}
						Response.Redirect( GetSurveyURL( i ), true );
						return;
					}
				}
				if ( PageShouldBeSkipped( CurrentPage ) ) {
					int nextPage = CurrentPage + RedirectDirection;
					if ( CurrentPage == 22 && RedirectDirection == 1 ) {
						nextPage = 97; 
						//return;
					}
					Response.Redirect( GetSurveyURL( nextPage, RedirectDirection ), true );
					return;
				}
				//If we've made it to 97, save to database.
				if ( CurrentPage == 97 && !IsPostBack ) {
					int surveyID;
					if ( SaveData( out surveyID ) ) {
						SendNotifications( surveyID );
						SurveyComplete = true;
						

						if (strSurveyLang != "French")
						{
							mmLastPage.SuccessMessage = "Thank you for your feedback. We will use your responses in our ongoing efforts to make your visits exciting and memorable.<br /><br /> <p>Please Note: If you have requested someone to contact you, please ensure you check your \"Junk Mail\" folder or add \"@gcgamingsurvey.com\" to your email account's white list.</p>";
						}
						else if(strSurveyLang == "French")
						{
							mmLastPage.FrSuccessMessage = "Merci d’avoir partagé votre avis. Nous utiliserons vos réponses dans nos efforts continus de rendre vos visites passionnantes et mémorables.<br/> <p>Note : Si vous avez demandé qu’on communique avec vous, assurez-vous de vérifier votre courrier poubelle ou ajoutez « @gcgamingsurvey.com » à la liste blanche de votre compte de messagerie.</p>"; 

						  //  mmLastPage_F.SuccessMessage = "Merci pour votre avis. Nous allons utiliser vos réponses dans nos efforts continus pour rendre vos visites passionnantes et mémorables. S'il vous plaît Note: Si vous avez demandé à quelqu'un de vous contacter, assurez-vous de vérifier votre \"Junk Mail\" dossier ou Ajouter \"@gcgamingsurvey.com\" à la liste blanche de votre compte de messagerie.";
						}

						Session.Abandon();

					} else {
						StringBuilder sb = new StringBuilder();
						foreach ( string key in Session.Keys ) {
							object val = Session[key];
							List<string> ls = val as List<string>;
							if ( ls != null ) {
								sb.AppendFormat( "\"{0}\": {1}", key, String.Join( ", ", ls ) );
							} else {
								sb.AppendFormat( "\"{0}\": {1}", key, val );
							}
						}
						ErrorHandler.WriteLog( "GCC_Web_Portal.SurveyGEI", "Unable to save responses.\n\nSession Variables:\n\n" + sb.ToString(), ErrorHandler.ErrorEventID.General );
						mmLastPage.ErrorMessage = "We were unable to save your responses. Please go back and try again.";
					}
				}
			}




			if (CurrentPage == 3 || CurrentPage == 4 && !IsPostBack)
			{
				if (PropertyShortCode == GCCPropertyShortCode.CNB)
				{
					FrenchEntertainment = "&nbsp;Assister à un spectacle à notre salle de spectacles ou à notre pub";
				}

				else
				{
					FrenchEntertainment = "&nbsp;Profiter du divertissement à notre Lounge";
				}


			}

			if(CurrentPage == 1 && !IsKioskOrStaffEntry)
			{
				if (this.PINSurveyLang.SelectedValue != "French")
				{
					this.English.Visible = true;
					this.French.Visible = false;
					HeaderTitle = "Guest Experience Survey";
					strSurveyLang = "English";

					Session["SurveyLang"] = "English";
				}
				else
				{
					this.French.Visible = true;
					this.English.Visible = false;
					txtEmail_F.Text = txtEmail.Text;
					HeaderTitle = "Sondage sur l'expérience des clients";
					strSurveyLang = "French";
					Session["SurveyLang"] = "French";
				}
			}



			if(strSurveyLang != "French")
			{
				HeaderTitle = "Guest Experience Survey";  
			}
			else
			{
				HeaderTitle = "Sondage sur l'expérience des clients";
				
			}




		}

		private void SendNotifications( int surveyID ) 
					 
		{
			string feedbackUID = null;

			bool createTicket = false;



			if(strSurveyLang != "French")
			{


				createTicket = Q33.SelectedValue == 1 || Q40.SelectedValue == 1;

				// BEGIN - REC - 3March2016 - Remove staff notifications as per Colin M's request - “Filters need to be applied - too many notifications. Need to adjust notifications from Hotel survey so that not all surveys cause a feedback notification.”

				//bool notifyStaff = Q27.SelectedValue == 1 || !String.IsNullOrEmpty( Q34.Text.Trim() ) || !String.IsNullOrEmpty( Q35.Text.Trim() );

				//Determine triggers for feedback request
				//if ( createTicket || notifyStaff ) {

				// END - REC - 3March2016

				//Determine triggers for feedback request
				if (createTicket)
				{

					NotificationReason nr = NotificationReason.None;

					if (Q27.SelectedValue == 1)
					{
						if (radQ27A_1.Checked)
						{
							nr = NotificationReason.ArrivalParking;
						}
						if (radQ27A_2.Checked)
						{
							nr = NotificationReason.GuestServices;
						}
						if (radQ27A_2.Checked)
						{
							nr = NotificationReason.Cashiers;
						}
						if (radQ27A_4.Checked)
						{
							nr = NotificationReason.ManagerSupervisor;
						}
						if (radQ27A_5.Checked)
						{
							nr = NotificationReason.Security;
						}
						if (radQ27A_6.Checked)
						{
							nr = NotificationReason.Slots;
						}
						if (radQ27A_7.Checked)
						{
							nr = NotificationReason.Tables;
						}
						if (radQ27A_8.Checked)
						{
							nr = NotificationReason.FoodBeverage;
						}
						if (radQ27A_9.Checked)
						{
							nr = NotificationReason.HotelDependsOnProperty;
						}
						if (radQ27A_10.Checked)
						{
							nr = NotificationReason.Other;
						}
						if (radQ27A_11.Checked)
						{
							nr = NotificationReason.Bingo;
						}
						if (radQ27A_12.Checked)
						{
							nr = NotificationReason.Entertainment;
						}
						if (radQ27A_13.Checked)
						{
							nr = NotificationReason.HorseRacing;
						}
					}
					else
					{
						if (Q40.SelectedValue == 1)
						{
							if (radQ40A_1.Checked)
							{
								nr = NotificationReason.ArrivalParking;
							}
							if (radQ40A_2.Checked)
							{
								nr = NotificationReason.GuestServices;
							}
							if (radQ40A_2.Checked)
							{
								nr = NotificationReason.Cashiers;
							}
							if (radQ40A_4.Checked)
							{
								nr = NotificationReason.ManagerSupervisor;
							}
							if (radQ40A_5.Checked)
							{
								nr = NotificationReason.Security;
							}
							if (radQ40A_6.Checked)
							{
								nr = NotificationReason.Slots;
							}
							if (radQ40A_7.Checked)
							{
								nr = NotificationReason.Tables;
							}
							if (radQ40A_8.Checked)
							{
								nr = NotificationReason.FoodBeverage;
							}
							if (radQ40A_9.Checked)
							{
								nr = NotificationReason.HotelDependsOnProperty;
							}
							if (radQ40A_10.Checked)
							{
								nr = NotificationReason.Other;
							}
							if (radQ40A_11.Checked)
							{
								nr = NotificationReason.Bingo;
							}
							if (radQ40A_12.Checked)
							{
								nr = NotificationReason.Entertainment;
							}
							if (radQ40A_13.Checked)
							{
								nr = NotificationReason.HorseRacing;
							}
						}
						else
						{
							if (radQ1_Bingo.Checked)
							{
								nr = NotificationReason.Bingo;
							}
							else if (radQ1_Entertainment.Checked)
							{
								nr = NotificationReason.Entertainment;
							}
							else if (radQ1_Food.Checked)
							{
								nr = NotificationReason.FoodBeverage;
							}
							else if (radQ1_Hotel.Checked)
							{
								nr = NotificationReason.Hotel;
							}
							else if (radQ1_LiveRacing.Checked)
							{
								nr = NotificationReason.LiveRacing;
							}
							else if (radQ1_Lottery.Checked)
							{
								nr = NotificationReason.Lottery;
							}
							else if (radQ1_Poker.Checked)
							{
								nr = NotificationReason.TableGamesPoker;
							}
							else if (radQ1_Racebook.Checked)
							{
								nr = NotificationReason.Racebook;
							}
							else if (radQ1_Slots.Checked)
							{
								nr = NotificationReason.Slots;
							}
							else if (radQ1_Tables.Checked)
							{
								nr = NotificationReason.TableGamesPoker;
							}
							else if (radQ1_None.Checked)
							{
								nr = NotificationReason.None;
							}
						}
					}

					if (createTicket)
					{
						//Add the feedback
						SQLDatabase sql = new SQLDatabase();
						SqlParameter feedbackUIDParam = new SqlParameter("@UID", System.Data.SqlDbType.UniqueIdentifier);
						feedbackUIDParam.Direction = System.Data.ParameterDirection.Output;

						sql.ExecStoredProcedureDataSet("spFeedback_Create",
							new SQLParamList()
									.Add("@PropertyID", PropertyID)
									.Add("@SurveyTypeID", 1)
									.Add("@RecordID", surveyID)
									.Add("@ReasonID", (int)nr)
									.Add(feedbackUIDParam)
						);
						if (!sql.HasError)
						{
							feedbackUID = feedbackUIDParam.Value.ToString();
						}
					}

					string gagLocation = String.Empty;
					if (PropertyShortCode == GCCPropertyShortCode.GAG)
					{
						if (radQ3_Everett.Checked)
						{
							gagLocation = "Everett";
						}
						else if (radQ3_Lakewood.Checked)
						{
							gagLocation = "Lakewood";
						}
						else if (radQ3_Tukwila.Checked)
						{
							gagLocation = "Tukwila";
						}
						else if (radQ3_DeMoines.Checked)
						{
							gagLocation = "Des Moines";
						}
						if (gagLocation.Length > 0)
						{
							gagLocation = " - " + gagLocation;
						}
					}

					string fbLink = GCCPortalUrl + "Admin/Feedback/" + feedbackUID;

					//Use stringbuilder to compile answer to Q27A_1 to Q27A_10 checkboxes
					StringBuilder sb = new StringBuilder();

					if (Q27.SelectedValue == 1)
					{
						if (radQ27A_1.GetValue())
						{
							sb.Append("Arrival and parking");
						}
						if (radQ27A_2.GetValue())
						{
							sb.Append("Guest Services");
						}
						if (radQ27A_3.GetValue())
						{
							sb.Append("Cashiers");
						}
						if (radQ27A_4.GetValue())
						{
							sb.Append("Manager/Supervisor");
						}
						if (radQ27A_5.GetValue())
						{
							sb.Append("Security");
						}
						if (radQ27A_6.GetValue())
						{
							sb.Append("Slots");
						}
						if (radQ27A_7.GetValue())
						{
							sb.Append("Tables");
						}
						if (radQ27A_8.GetValue())
						{
							sb.Append("Food & Beverage");
						}
						if (radQ27A_9.GetValue())
						{
							sb.Append("Hotel");
						}
						if (radQ27A_10.GetValue())
						{
							sb.Append("Other:");
							sb.Append(txtQ27A_OtherExplanation.Text);
						}
						if (radQ27A_11.GetValue())
						{
							sb.Append("Bingo");
						}
						if (radQ27A_12.GetValue())
						{
							sb.Append("Entertainment");
						}
						if (radQ27A_13.GetValue())
						{
							sb.Append("Horse Racing");
						}
					}

					if (Q40.SelectedValue == 1 && Q27.SelectedValue != 1)
					{
						if (radQ40A_1.GetValue())
						{
							sb.Append("Arrival and parking");
						}
						if (radQ40A_2.GetValue())
						{
							sb.Append("Guest Services");
						}
						if (radQ40A_3.GetValue())
						{
							sb.Append("Cashiers");
						}
						if (radQ40A_4.GetValue())
						{
							sb.Append("Manager/Supervisor");
						}
						if (radQ40A_5.GetValue())
						{
							sb.Append("Security");
						}
						if (radQ40A_6.GetValue())
						{
							sb.Append("Slots");
						}
						if (radQ40A_7.GetValue())
						{
							sb.Append("Tables");
						}
						if (radQ40A_8.GetValue())
						{
							sb.Append("Food & Beverage");
						}
						if (radQ40A_9.GetValue())
						{
							sb.Append("Hotel");
						}
						if (radQ40A_10.GetValue())
						{
							sb.Append("Other:");
							sb.Append(txtQ40OtherExplanation.Text);
						}
						if (radQ40A_11.GetValue())
						{
							sb.Append("Bingo");
						}
						if (radQ40A_12.GetValue())
						{
							sb.Append("Entertainment");
						}
						if (radQ40A_13.GetValue())
						{
							sb.Append("Horse Racing");
						}
					}

					//Send the notifications
					string OtherExplanation = String.Empty;
					if (txtQ27B.Text == String.Empty)
					{
						OtherExplanation = ":" + txtQ27B.Text;
					}

					//REC - 4April2016 - As part of Item #5 in Colin's ToDo list, have the system send different type of notification 
					//if user indicates there was a problem (called GEI - Problem Resolution).
					if (Q27.SelectedValue == 1)
					{
						SurveyTools.SendNotifications(
							Server,
							PropertyShortCode,
							SharedClasses.SurveyType.GEIProblemResolution,
							nr,
							string.Empty,
							new
							{
								Date = DateTime.Now.ToString("yyyy-MM-dd"),
								CasinoName = PropertyTools.GetCasinoName((int)PropertyShortCode) + gagLocation,
								Problems = "Yes",
								Response = ((Q33.SelectedValue == 1 || Q40.SelectedValue == 1) ? "Yes" : "No"),
								ProblemDescription = sb.ToString() + OtherExplanation,
								StaffComment = Q11.Text,
								GeneralComments = Q34.Text,
								MemorableEmployee = Q35.Text,
								FeedbackLinkTXT = (!createTicket ? String.Empty : "\n\nRespond to this feedback:\n" + fbLink + "\n"),
								FeedbackLinkHTML = (!createTicket ? String.Empty : @"<br /><br /><p><b>Respond to this feedback:</b></p><p>" + fbLink + "</p>"),
								SurveyLink = GCCPortalUrl + "Display/GEI/" + surveyID
							});
					}
					else
						if (Q40.SelectedValue == 1)
						{
							SurveyTools.SendNotifications(
								Server,
								PropertyShortCode,
								SharedClasses.SurveyType.GEIProblemResolution,
								nr,
								string.Empty,
								new
								{
									Date = DateTime.Now.ToString("yyyy-MM-dd"),
									CasinoName = PropertyTools.GetCasinoName((int)PropertyShortCode) + gagLocation,
									Problems = "No",
									Response = "Yes",
									FeedbackCategory = sb.ToString(),
									ProblemDescription = "",
									StaffComment = Q11.Text,
									GeneralComments = Q34.Text,
									MemorableEmployee = Q35.Text,
									FeedbackLinkTXT = (!createTicket ? String.Empty : "\n\nRespond to this feedback:\n" + fbLink + "\n"),
									FeedbackLinkHTML = (!createTicket ? String.Empty : @"<br /><br /><p><b>Respond to this feedback:</b></p><p>" + fbLink + "</p>"),
									SurveyLink = GCCPortalUrl + "Display/GEI/" + surveyID
								});
						}
						else
						{
							SurveyTools.SendNotifications(
								Server,
								PropertyShortCode,
								SharedClasses.SurveyType.GEI,
								nr,
								string.Empty,
								new
								{
									Date = DateTime.Now.ToString("yyyy-MM-dd"),
									CasinoName = PropertyTools.GetCasinoName((int)PropertyShortCode) + gagLocation,
									Problems = "No",
									Response = ((Q33.SelectedValue == 1 || Q40.SelectedValue == 1) ? "Yes" : "No"),
									ProblemDescription = txtQ27B.Text,
									StaffComment = Q11.Text,
									GeneralComments = Q34.Text,
									MemorableEmployee = Q35.Text,
									FeedbackLinkTXT = (!createTicket ? String.Empty : "\n\nRespond to this feedback:\n" + fbLink + "\n"),
									FeedbackLinkHTML = (!createTicket ? String.Empty : @"<br /><br /><p><b>Respond to this feedback:</b></p><p>" + fbLink + "</p>"),
									SurveyLink = GCCPortalUrl + "Display/GEI/" + surveyID
								});
						}
				}

				if (createTicket)
				{
					//Send thank you letter
					SurveyTools.SendNotifications(
						Server,
						PropertyShortCode,
						SharedClasses.SurveyType.GEI,
						NotificationReason.ThankYou,
						string.Empty,
						new
						{
							CasinoName = PropertyTools.GetCasinoName(PropertyID),
							FeedbackNoteHTML = String.IsNullOrEmpty(feedbackUID) ? String.Empty : "<p>You can view and respond to your feedback request by clicking on (or copying and pasting) the following link:<br />" + GCCPortalUrl + "F/" + feedbackUID + "</p>",
							FeedbackNoteTXT = String.IsNullOrEmpty(feedbackUID) ? String.Empty : "You can view and respond to your feedback request by clicking on (or copying and pasting) the following link:\n" + GCCPortalUrl + "F/" + feedbackUID + "\n\n",
							Attachments = new SurveyTools.SurveyAttachmentDetails[] 
						{
						new SurveyTools.SurveyAttachmentDetails() { Path = "~/Images/headers/" + PropertyTools.GetCasinoHeaderImage( PropertyShortCode ), ContentID = "HeaderImage" }
						}
						},
						!String.IsNullOrWhiteSpace(txtEmail2.Text) ? txtEmail2.Text : txtEmail.Text);
				}




			}

			else if(strSurveyLang == "French")
			{


				createTicket = Q33_F.SelectedValue_F == 1 || Q40_F.SelectedValue_F == 1;

				// BEGIN - REC - 3March2016 - Remove staff notifications as per Colin M's request - “Filters need to be applied - too many notifications. Need to adjust notifications from Hotel survey so that not all surveys cause a feedback notification.”

				//bool notifyStaff = Q27_F.SelectedValue == 1 || !String.IsNullOrEmpty( Q34_F.Text.Trim() ) || !String.IsNullOrEmpty( Q35_F.Text.Trim() );

				//Determine triggers for feedback request
				//if ( createTicket || notifyStaff ) {

				// END - REC - 3March2016

				//Determine triggers for feedback request
				if (createTicket)
				{

					NotificationReason nr = NotificationReason.None;

					if (Q27_F.SelectedValue_F == 1)
					{
						if (radQ27A_1_F.Checked)
						{
							nr = NotificationReason.ArrivalParking;
						}
						if (radQ27A_2_F.Checked)
						{
							nr = NotificationReason.GuestServices;
						}
						if (radQ27A_2_F.Checked)
						{
							nr = NotificationReason.Cashiers;
						}
						if (radQ27A_4_F.Checked)
						{
							nr = NotificationReason.ManagerSupervisor;
						}
						if (radQ27A_5_F.Checked)
						{
							nr = NotificationReason.Security;
						}
						if (radQ27A_6_F.Checked)
						{
							nr = NotificationReason.Slots;
						}
						if (radQ27A_7_F.Checked)
						{
							nr = NotificationReason.Tables;
						}
						if (radQ27A_8_F.Checked)
						{
							nr = NotificationReason.FoodBeverage;
						}
						if (radQ27A_9_F.Checked)
						{
							nr = NotificationReason.HotelDependsOnProperty;
						}
						if (radQ27A_10_F.Checked)
						{
							nr = NotificationReason.Other;
						}
						if (radQ27A_11_F.Checked)
						{
							nr = NotificationReason.Bingo;
						}
						if (radQ27A_12_F.Checked)
						{
							nr = NotificationReason.Entertainment;
						}
						if (radQ27A_13_F.Checked)
						{
							nr = NotificationReason.HorseRacing;
						}
					}
					else
					{
						if (Q40_F.SelectedValue_F == 1)
						{
							if (radQ40A_1_F.Checked)
							{
								nr = NotificationReason.ArrivalParking;
							}
							if (radQ40A_2_F.Checked)
							{
								nr = NotificationReason.GuestServices;
							}
							if (radQ40A_2_F.Checked)
							{
								nr = NotificationReason.Cashiers;
							}
							if (radQ40A_4_F.Checked)
							{
								nr = NotificationReason.ManagerSupervisor;
							}
							if (radQ40A_5_F.Checked)
							{
								nr = NotificationReason.Security;
							}
							if (radQ40A_6_F.Checked)
							{
								nr = NotificationReason.Slots;
							}
							if (radQ40A_7_F.Checked)
							{
								nr = NotificationReason.Tables;
							}
							if (radQ40A_8_F.Checked)
							{
								nr = NotificationReason.FoodBeverage;
							}
							if (radQ40A_9_F.Checked)
							{
								nr = NotificationReason.HotelDependsOnProperty;
							}
							if (radQ40A_10_F.Checked)
							{
								nr = NotificationReason.Other;
							}
							if (radQ40A_11_F.Checked)
							{
								nr = NotificationReason.Bingo;
							}
							if (radQ40A_12_F.Checked)
							{
								nr = NotificationReason.Entertainment;
							}
							if (radQ40A_13_F.Checked)
							{
								nr = NotificationReason.HorseRacing;
							}
						}
						else
						{
							if (radQ1_Bingo_F.Checked)
							{
								nr = NotificationReason.Bingo;
							}
							else if (radQ1_Entertainment_F.Checked)
							{
								nr = NotificationReason.Entertainment;
							}
							else if (radQ1_Food_F.Checked)
							{
								nr = NotificationReason.FoodBeverage;
							}
							else if (radQ1_Hotel_F.Checked)
							{
								nr = NotificationReason.Hotel;
							}
							else if (radQ1_LiveRacing_F.Checked)
							{
								nr = NotificationReason.LiveRacing;
							}
							else if (radQ1_Lottery_F.Checked)
							{
								nr = NotificationReason.Lottery;
							}
							else if (radQ1_Poker_F.Checked)
							{
								nr = NotificationReason.TableGamesPoker;
							}
							else if (radQ1_Racebook_F.Checked)
							{
								nr = NotificationReason.Racebook;
							}
							else if (radQ1_Slots_F.Checked)
							{
								nr = NotificationReason.Slots;
							}
							else if (radQ1_Tables_F.Checked)
							{
								nr = NotificationReason.TableGamesPoker;
							}
							else if (radQ1_None_F.Checked)
							{
								nr = NotificationReason.None;
							}
						}
					}

					if (createTicket)
					{
						//Add the feedback
						SQLDatabase sql = new SQLDatabase();
						SqlParameter feedbackUIDParam = new SqlParameter("@UID", System.Data.SqlDbType.UniqueIdentifier);
						feedbackUIDParam.Direction = System.Data.ParameterDirection.Output;

						sql.ExecStoredProcedureDataSet("spFeedback_Create",
							new SQLParamList()
									.Add("@PropertyID", PropertyID)
									.Add("@SurveyTypeID", 1)
									.Add("@RecordID", surveyID)
									.Add("@ReasonID", (int)nr)
									.Add(feedbackUIDParam)
						);
						if (!sql.HasError)
						{
							feedbackUID = feedbackUIDParam.Value.ToString();
						}
					}

					string gagLocation = String.Empty;
					if (PropertyShortCode == GCCPropertyShortCode.GAG)
					{
						if (radQ3_Everett_F.Checked)
						{
							gagLocation = "Everett";
						}
						else if (radQ3_Lakewood_F.Checked)
						{
							gagLocation = "Lakewood";
						}
						else if (radQ3_Tukwila_F.Checked)
						{
							gagLocation = "Tukwila";
						}
						else if (radQ3_DeMoines_F.Checked)
						{
							gagLocation = "Des Moines";
						}
						if (gagLocation.Length > 0)
						{
							gagLocation = " - " + gagLocation;
						}
					}

					string fbLink = GCCPortalUrl + "Admin/Feedback/" + feedbackUID;

					//Use stringbuilder to compile answer to Q27A_1 to Q27A_10 checkboxes
					StringBuilder sb = new StringBuilder();

					if (Q27_F.SelectedValue_F == 1)
					{
						if (radQ27A_1_F.GetValue())
						{
							sb.Append("Arrivée et stationnement");
						}
						if (radQ27A_2_F.GetValue())
						{
							sb.Append("Service à la clientèle");
						}
						if (radQ27A_3_F.GetValue())
						{
							sb.Append("caissiers");
						}
						if (radQ27A_4_F.GetValue())
						{
							sb.Append("Gestionnaire / Superviseur");
						}
						if (radQ27A_5_F.GetValue())
						{
							sb.Append("la sécurité");
						}
						if (radQ27A_6_F.GetValue())
						{
							sb.Append("Machines à sous");
						}
						if (radQ27A_7_F.GetValue())
						{
							sb.Append("les tables");
						}
						if (radQ27A_8_F.GetValue())
						{
							sb.Append("nourriture et boissons");
						}
						if (radQ27A_9_F.GetValue())
						{
							sb.Append("Un hôtel");
						}
						if (radQ27A_10_F.GetValue())
						{
							sb.Append("Autre:");
							sb.Append(txtQ27A_OtherExplanation_F.Text);
						}
						if (radQ27A_11_F.GetValue())
						{
							sb.Append("Bingo");
						}
						if (radQ27A_12_F.GetValue())
						{
							sb.Append("Divertissement");
						}
						if (radQ27A_13_F.GetValue())
						{
							sb.Append("Course de chevaux");
						}
					}

					if (Q40_F.SelectedValue_F == 1 && Q27_F.SelectedValue_F != 1)
					{
						if (radQ40A_1_F.GetValue())
						{
							sb.Append("Arrivée et stationnement");
						}
						if (radQ40A_2_F.GetValue())
						{
							sb.Append("Service à la clientèle");
						}
						if (radQ40A_3_F.GetValue())
						{
							sb.Append("caissiers");
						}
						if (radQ40A_4_F.GetValue())
						{
							sb.Append("Gestionnaire / Superviseur");
						}
						if (radQ40A_5_F.GetValue())
						{
							sb.Append("la sécurité");
						}
						if (radQ40A_6_F.GetValue())
						{
							sb.Append("Machines à sous");
						}
						if (radQ40A_7_F.GetValue())
						{
							sb.Append("les tables");
						}
						if (radQ40A_8_F.GetValue())
						{
							sb.Append("nourriture et boissons");
						}
						if (radQ40A_9_F.GetValue())
						{
							sb.Append("Un hôtel");
						}
						if (radQ40A_10_F.GetValue())
						{
							sb.Append("Autre:");
							sb.Append(txtQ40OtherExplanation_F.Text);
						}
						if (radQ40A_11_F.GetValue())
						{
							sb.Append("Bingo");
						}
						if (radQ40A_12_F.GetValue())
						{
							sb.Append("Divertissement");
						}
						if (radQ40A_13_F.GetValue())
						{
							sb.Append("Course de chevaux");
						}
					}

					//Send the notifications
					string OtherExplanation = String.Empty;
					if (txtQ27B_F.Text == String.Empty)
					{
						OtherExplanation = ":" + txtQ27B_F.Text;
					}

					//REC - 4April2016 - As part of Item #5 in Colin's ToDo list, have the system send different type of notification 
					//if user indicates there was a problem (called GEI - Problem Resolution).
					if (Q27_F.SelectedValue_F == 1)
					{
						SurveyTools.SendNotifications(
							Server,
							PropertyShortCode,
							SharedClasses.SurveyType.GEIProblemResolution,
							nr,
							string.Empty,
							new
							{
								Date = DateTime.Now.ToString("yyyy-MM-dd"),
								CasinoName = PropertyTools.GetCasinoName((int)PropertyShortCode) + gagLocation,
								Problems = "Yes",
								Response = ((Q33_F.SelectedValue_F == 1 || Q40_F.SelectedValue_F == 1) ? "Yes" : "No"),
								ProblemDescription = sb.ToString() + OtherExplanation,
								StaffComment = Q11_F.Text,
								GeneralComments = Q34_F.Text,
								MemorableEmployee = Q35_F.Text,
								FeedbackLinkTXT = (!createTicket ? String.Empty : "\n\nRépondre à cette rétroaction:\n" + fbLink + "\n"),
								FeedbackLinkHTML = (!createTicket ? String.Empty : @"<br /><br /><p><b>Répondre à cette rétroaction:</b></p><p>" + fbLink + "</p>"),
								SurveyLink = GCCPortalUrl + "Display/GEI/" + surveyID
							});
					}
					else
						if (Q40_F.SelectedValue_F == 1)
						{
							SurveyTools.SendNotifications(
								Server,
								PropertyShortCode,
								SharedClasses.SurveyType.GEIProblemResolution,
								nr,
								string.Empty,
								new
								{
									Date = DateTime.Now.ToString("yyyy-MM-dd"),
									CasinoName = PropertyTools.GetCasinoName((int)PropertyShortCode) + gagLocation,
									Problems = "No",
									Response = "Yes",
									FeedbackCategory = sb.ToString(),
									ProblemDescription = "",
									StaffComment = Q11_F.Text,
									GeneralComments = Q34_F.Text,
									MemorableEmployee = Q35_F.Text,
									FeedbackLinkTXT = (!createTicket ? String.Empty : "\n\nRépondre à cette rétroaction:\n" + fbLink + "\n"),
									FeedbackLinkHTML = (!createTicket ? String.Empty : @"<br /><br /><p><b>Répondre à cette rétroaction:</b></p><p>" + fbLink + "</p>"),
									SurveyLink = GCCPortalUrl + "Display/GEI/" + surveyID
								});
						}
						else
						{
							SurveyTools.SendNotifications(
								Server,
								PropertyShortCode,
								SharedClasses.SurveyType.GEI,
								nr,
								string.Empty,
								new
								{
									Date = DateTime.Now.ToString("yyyy-MM-dd"),
									CasinoName = PropertyTools.GetCasinoName((int)PropertyShortCode) + gagLocation,
									Problems = "No",
									Response = ((Q33_F.SelectedValue_F == 1 || Q40_F.SelectedValue_F == 1) ? "Yes" : "No"),
									ProblemDescription = txtQ27B_F.Text,
									StaffComment = Q11_F.Text,
									GeneralComments = Q34_F.Text,
									MemorableEmployee = Q35_F.Text,
									FeedbackLinkTXT = (!createTicket ? String.Empty : "\n\nRépondre à cette rétroaction:\n" + fbLink + "\n"),
									FeedbackLinkHTML = (!createTicket ? String.Empty : @"<br /><br /><p><b>Répondre à cette rétroaction:</b></p><p>" + fbLink + "</p>"),
									SurveyLink = GCCPortalUrl + "Display/GEI/" + surveyID
								});
						}
				}

				if (createTicket)
				{
					//Send thank you letter
					SurveyTools.SendNotifications(
						Server,
						PropertyShortCode,
						SharedClasses.SurveyType.GEI,
						NotificationReason.ThankYou,
						string.Empty,
						new
						{
							CasinoName = PropertyTools.GetCasinoName(PropertyID),
							FeedbackNoteHTML = String.IsNullOrEmpty(feedbackUID) ? String.Empty : "<p>Vous pouvez afficher et répondre à votre demande de rétroaction en cliquant sur (ou copier et coller) le lien suivant:<br />" + GCCPortalUrl + "F/" + feedbackUID + "</p>",
							FeedbackNoteTXT = String.IsNullOrEmpty(feedbackUID) ? String.Empty : "Vous pouvez afficher et répondre à votre demande de rétroaction en cliquant sur (ou copier et coller) le lien suivant:\n" + GCCPortalUrl + "F/" + feedbackUID + "\n\n",
							Attachments = new SurveyTools.SurveyAttachmentDetails[] 
						{
						new SurveyTools.SurveyAttachmentDetails() { Path = "~/Images/headers/" + PropertyTools.GetCasinoHeaderImage( PropertyShortCode ), ContentID = "HeaderImage" }
						}
						},
						!String.IsNullOrWhiteSpace(txtEmail2_F.Text) ? txtEmail2_F.Text : txtEmail_F.Text);
				}


			}




					}

		private bool PageShouldBeSkipped( int CurrentPage ) {
			switch ( CurrentPage ) {
				case 2:
					return IsKioskOrStaffEntry;
				case 9:

					if(strSurveyLang != "French")
					{
					if ( Q17.SelectedValue != 1 ) 
					
					{
						return true;
					}
					}
					else if(strSurveyLang == "French")
					{
						  if ( Q17_F.SelectedValue_F != 1 ) 
					
					{
						return true;
					}
				  

					}
					
					
					
					break;


				case 10:

					if(strSurveyLang != "French") {

					if ( !Q18_1.Checked
						&& !Q18_2.Checked
						&& !Q18_3.Checked
						&& !Q18_4.Checked
						&& !Q18_5.Checked
						&& !Q18_6.Checked
						&& !Q18_7.Checked
						&& !Q18_8.Checked
						&& !Q18_9.Checked
						&& !Q18_10.Checked
						&& !Q18_11.Checked
						&& !Q18_12.Checked
						&& !Q18_13.Checked
						&& !Q18_14.Checked
						&& !Q18_15.Checked
						&& !Q18_16.Checked
						&& !Q18_17.Checked
						&& !Q18_18.Checked
						&& !Q18_19.Checked
						&& !Q18_20.Checked
						&& !Q18_21.Checked
						//&& !Q18_22.Checked
						//&& !Q18_23.Checked
						//&& !Q18_24.Checked
						&& !Q18_25.Checked
						&& !Q18_26.Checked
						&& !Q18_27.Checked
						&& !Q18_28.Checked
						&& !Q18_29.Checked
						&& !Q18_30.Checked
						&& !Q18_31.Checked
						&& !Q18_32.Checked
						&& !Q18_33.Checked
						&& !Q18_34.Checked
						&& !Q18_35.Checked
						&& !Q18_36.Checked
						&& !Q18_37.Checked
						&& !Q18_38.Checked
						&& !Q18_39.Checked
						&& !Q18_40.Checked
						&& !Q18_41.Checked
						&& !Q18_42.Checked
						&& !Q18_43.Checked
						&& !Q18_44.Checked
						&& !Q18_45.Checked
						&& !Q18_46.Checked
						&& !Q18_47.Checked
						&& !Q18_48.Checked
						&& !Q18_49.Checked
						&& !Q18_50.Checked
						&& !Q18_51.Checked
						&& !Q18_52.Checked
						&& !Q18_53.Checked
						&& !Q18_54.Checked) {
						return true;
					}
					}
					else if(strSurveyLang == "French")
					{



						
					if ( !Q18_1_F.Checked
						&& !Q18_2_F.Checked
						&& !Q18_3_F.Checked
						&& !Q18_4_F.Checked
						&& !Q18_5_F.Checked
						&& !Q18_6_F.Checked
						&& !Q18_7_F.Checked
						&& !Q18_8_F.Checked
						&& !Q18_9_F.Checked
						&& !Q18_10_F.Checked
						&& !Q18_11_F.Checked
						&& !Q18_12_F.Checked
						&& !Q18_13_F.Checked
						&& !Q18_14_F.Checked
						&& !Q18_15_F.Checked
						&& !Q18_16_F.Checked
						&& !Q18_17_F.Checked
						&& !Q18_18_F.Checked
						&& !Q18_19_F.Checked
						&& !Q18_20_F.Checked
						&& !Q18_21_F.Checked
						//&& !Q18_22.Checked
						//&& !Q18_23.Checked
						//&& !Q18_24.Checked
						&& !Q18_25_F.Checked
						&& !Q18_26_F.Checked
						&& !Q18_27_F.Checked
						&& !Q18_28_F.Checked
						&& !Q18_29_F.Checked
						&& !Q18_30_F.Checked
						&& !Q18_31_F.Checked
						&& !Q18_32_F.Checked
						&& !Q18_33_F.Checked
						&& !Q18_34_F.Checked
						&& !Q18_35_F.Checked
						&& !Q18_36_F.Checked
						&& !Q18_37_F.Checked
						&& !Q18_38_F.Checked
						&& !Q18_39_F.Checked
						&& !Q18_40_F.Checked
						&& !Q18_41_F.Checked
						&& !Q18_42_F.Checked
						&& !Q18_43_F.Checked
						&& !Q18_44_F.Checked
						&& !Q18_45_F.Checked
						&& !Q18_46_F.Checked
						&& !Q18_47_F.Checked
						&& !Q18_48_F.Checked
						&& !Q18_49_F.Checked
						&& !Q18_50_F.Checked
						&& !Q18_51_F.Checked
						&& !Q18_52_F.Checked
						&& !Q18_53_F.Checked
						&& !Q18_54_F.Checked)
					{
						return true;
					}


					}
					break;


				case 11: //Entertainment / Show Lounge


					
					if ( PropertyShortCode != GCCPropertyShortCode.RR
						&& PropertyShortCode != GCCPropertyShortCode.HRCV
						&& PropertyShortCode != GCCPropertyShortCode.VRL
						&& PropertyShortCode != GCCPropertyShortCode.CCH
						&& PropertyShortCode != GCCPropertyShortCode.CMR
						&& PropertyShortCode != GCCPropertyShortCode.CDC
						&& PropertyShortCode != GCCPropertyShortCode.CNSH
						&& PropertyShortCode != GCCPropertyShortCode.EC
						&& PropertyShortCode != GCCPropertyShortCode.SCTI
						&& PropertyShortCode != GCCPropertyShortCode.CNB
						&& PropertyShortCode != GCCPropertyShortCode.SCBE
						) {
						return true;
					}

					break;


				case 12:
					
					
					if(strSurveyLang != "French"){
					if ( Q21.SelectedValue != 1 ) {
						return true;
					}
					}
					else if(strSurveyLang == "French"){
if ( Q21_F.SelectedValue_F != 1 ) {
						return true;
					}
					
					}
					break;




				case 13: //Theatre
					if ( PropertyShortCode != GCCPropertyShortCode.RR
						&& PropertyShortCode != GCCPropertyShortCode.HRCV
						&& PropertyShortCode != GCCPropertyShortCode.CNSH 
						&& PropertyShortCode != GCCPropertyShortCode.CNB ) {
						return true;
					}
					break;



				case 14: //Theatre Experience


					if(strSurveyLang != "French"){
					if ( Q24.SelectedValue != 1
						|| ( PropertyShortCode != GCCPropertyShortCode.RR
							&& PropertyShortCode != GCCPropertyShortCode.HRCV
							&& PropertyShortCode != GCCPropertyShortCode.CNSH
							&& PropertyShortCode != GCCPropertyShortCode.CNB ) ) {
						return true;
					}
					}
					else if(strSurveyLang == "French"){

						if ( Q24_F.SelectedValue_F != 1
						|| ( PropertyShortCode != GCCPropertyShortCode.RR
							&& PropertyShortCode != GCCPropertyShortCode.HRCV
							&& PropertyShortCode != GCCPropertyShortCode.CNSH
							&& PropertyShortCode != GCCPropertyShortCode.CNB ) ) {
						return true;
					}
					
					}
					break;


				case 16:

					if(strSurveyLang != "French"){
					if ( Q27.SelectedValue != 1 ) {
						return true;
					}
					}
					else if(strSurveyLang == "French"){

							if ( Q27_F.SelectedValue_F != 1 ) {
						return true;
					}
				
					}
					break;
				
				
				case 17:
					
					if(strSurveyLang != "French"){
					if ( Q29.SelectedValue != 1 ) {
						return true;
					}
					}
					else if(strSurveyLang == "French")
					{

						if ( Q29_F.SelectedValue_F != 1 ) {
						return true;
					}
					
					
					}
					
					
					
					break;



				case 18:


					if(strSurveyLang != "French"){
						
					if ( !( !IsKioskOrStaffEntry
						&& ( Q29.SelectedValue == 0
						|| Q30.SelectedValue == 1 || Q30.SelectedValue == 2
						|| Q31A.SelectedValue == 1 || Q31A.SelectedValue == 2
						|| Q31B.SelectedValue == 1 || Q31B.SelectedValue == 2
						|| Q31C.SelectedValue == 1 || Q31C.SelectedValue == 2
						|| Q31D.SelectedValue == 1 || Q31D.SelectedValue == 2
						|| Q31E.SelectedValue == 1 || Q31E.SelectedValue == 2 ) ) ) {
						return true;
					}
					}
					else if(strSurveyLang == "French")
					{
						if ( !( !IsKioskOrStaffEntry
						&& ( Q29_F.SelectedValue_F == 0
						|| Q30_F.SelectedValue_F == 1 || Q30_F.SelectedValue_F == 2
						|| Q31A_F.SelectedValue == 1 || Q31A_F.SelectedValue == 2
						|| Q31B_F.SelectedValue == 1 || Q31B_F.SelectedValue == 2
						|| Q31C_F.SelectedValue == 1 || Q31C_F.SelectedValue == 2
						|| Q31D_F.SelectedValue == 1 || Q31D_F.SelectedValue == 2
						|| Q31E_F.SelectedValue == 1 || Q31E_F.SelectedValue == 2 ) ) ) {
						return true;
					}

					}

					break;



				case 21:
					if(strSurveyLang != "French"){
					if ( Q33.SelectedValue != 1
						&& Q40.SelectedValue != 1 ) {
						return true;
					}
					}
					else if(strSurveyLang == "French")
					{
						if ( Q33_F.SelectedValue_F != 1
						&& Q40_F.SelectedValue_F != 1 ) {
						return true;
					}

					}
					break;


				case 22:

					if(strSurveyLang != "French"){
					if (OLGYesNo.SelectedValue == -1 || OLGYesNo.SelectedValue == 0) {
						return true;
					}
					}
					else if(strSurveyLang == "French"){
						if (OLGYesNo_F.SelectedValue_F == -1 || OLGYesNo_F.SelectedValue_F == 0) {
						return true;
					}

					}
					break;                    
			}
			return false;
		}

		protected void Prev_Click( object sender, EventArgs e ) {
			//System.Diagnostics.Debug.WriteLine( "Survey Prev_Click PID: " + CurrentPage );
			if ( ValidateAndSave( CurrentPage, true, true ) ) {
				int prevPage = CurrentPage - 1;
				//Check if they're undoing their decline
				if ( CurrentPage == 99 ) {
					prevPage = 2;
				} else if ( CurrentPage == 97 ) {
					prevPage = 22;
				}





				if(SurveyType == GEISurveyType.Email && CurrentPage == 2)
				{

					if(strSurveyLang == "French")
					{

						PINSurveyLang.SelectedValue = "French";
						PINSurveyLang.ClearSelection();
						PINSurveyLang.SelectedIndex = 2;
					}
					else
					{
						PINSurveyLang.SelectedValue = "English";
						PINSurveyLang.ClearSelection();
						PINSurveyLang.SelectedIndex = 2;
					}
				}




				Response.Redirect( GetSurveyURL( prevPage, -1 ), true );
			}
		}

		protected void Next_Click( object sender, EventArgs e ) {
			//System.Diagnostics.Debug.WriteLine( "Survey Next_Click PID: " + CurrentPage );
			if ( ValidateAndSave( CurrentPage, true, false ) ) {
				int nextPage = CurrentPage + 1;
				if ( nextPage > 22 ) {
					nextPage = 97;
				}
				if ( CurrentPage == 97 ) {
					if ( SurveyType == SharedClasses.GEISurveyType.Kiosk ) {
						Response.Redirect( GetSurveyURL( -1 ), true );
						return;
					} else {
						Response.Redirect( GetCasinoURL(), true );
						return;
					}
				}

				//Check for if they declined
				if ( radDecline.Checked ) {
					nextPage = 99;
				}


				if(SurveyType == GEISurveyType.Email && CurrentPage == 1)
				{
					if(PINSurveyLang.SelectedValue == "French")
					{
						Session["SurveyLang"] = "French";
						strSurveyLang = "French";
						HeaderTitle = "Sondage sur l'expérience des clients";
					}
					else
					{
						Session["SurveyLang"] = "English";
						strSurveyLang = "English";
						HeaderTitle = "Guest Experience Survey";

					}
			   
				}


				Response.Redirect( GetSurveyURL( nextPage ), true );
			}
		}

		/// <summary>
		/// Validates and saves a page's values. Returns true if the page is valid.
		/// </summary>
		/// <param name="page">The page ID to check.</param>
		/// <param name="currentPage">Whether this is the current page. If true, the values in the controls will be checked, otherwise the session will be checked.</param>
		/// <param name="saveOnly">If save only is true, the validation will be ignored and values will be saved so that they can be returned to. This is for use with the "Back" button.</param>
		protected bool ValidateAndSave( int page, bool currentPage, bool saveOnly ) {
			bool retVal = true;
			switch ( page ) {
				case 1:

					#region Page 1

					if ( !IsKioskOrStaffEntry ) {

						if(PINSurveyLang.SelectedValue == "French")
						{
							strSurveyLang = "French";
						}

						string email = null;

						if (strSurveyLang != "French")
						{

							email = GetValue(txtEmail, currentPage, String.Empty);

							
							if (!Validation.RegExCheck(email, ValidationType.Email))
							{
								mmTxtEmail.ErrorMessage = "Please enter a valid email address.";
								return false;
							}
							else if (currentPage)
							{
								SaveValue<string>(txtEmail);
							}
						}
						else
						
						{
							if(txtEmail_F.Text == null || txtEmail_F.Text == string.Empty)
							{

								txtEmail_F.Text = txtEmail.Text;
							}

						 email =   GetValue(txtEmail_F, currentPage, String.Empty);




							if (!Validation.RegExCheck(email, ValidationType.Email))
							{
								MessageManager3.ErrorMessage = "S'il vous plaît, mettez une adresse courriel valide.";
								return false;
							}
							else if (currentPage)
							{
								SaveValue<string>(txtEmail_F);
							}
						}
						
					}
					break;
					#endregion Page 1
				case 2: // Agreement
					#region Page 2
					if ( IsKioskOrStaffEntry ) {
						return true;
					}
					if ( !saveOnly ) {


						if (strSurveyLang != "French")
						{

							bool accept = GetValue(radAccept, currentPage, false);
							bool decline = GetValue(radDecline, currentPage, false);
							if (!accept && !decline)
							{
								mmAcceptGroup.ErrorMessage = "Please select one of the following options.";
								return false;
							}
						}
						else
						{
							bool accept = GetValue(radAccept_F, currentPage, false);
							bool decline = GetValue(radDecline_F, currentPage, false);
							if (!accept && !decline)
							{
								MessageManager2.ErrorMessage = "Veuillez sélectionner l'une des options suivantes.";
								return false;
							}


						}
					}
					if ( currentPage ) {

						if (strSurveyLang != "French")
						{
							SurveyTools.SaveRadioButtons(radAccept, radDecline);
						}
						else
						{
							SurveyTools.SaveRadioButtons(radAccept_F, radDecline_F);
						}
						
					}
					break;
					#endregion Page 2
				case 3: // Q1

					#region Page 3
					if (strSurveyLang != "French")
					{

						
						if (!saveOnly)
						{
							bool noneSelected = !GetValue(radQ1_Slots, currentPage, false) &&
												!GetValue(radQ1_Tables, currentPage, false) &&
												!GetValue(radQ1_Poker, currentPage, false) &&
												!GetValue(radQ1_Food, currentPage, false) &&
												!GetValue(radQ1_Entertainment, currentPage, false) &&
												!GetValue(radQ1_Hotel, currentPage, false) &&
												!GetValue(radQ1_LiveRacing, currentPage, false) &&
												!GetValue(radQ1_Racebook, currentPage, false) &&
												!GetValue(radQ1_Bingo, currentPage, false) &&
												!GetValue(radQ1_Lottery, currentPage, false) &&
												!GetValue(radQ1_None, currentPage, false);

							if (!saveOnly && noneSelected)
							{
								mmQ1.ErrorMessage = "Please select one of the following options.";

								return false;
							}
						}
						if (currentPage)
						{
							SurveyTools.SaveRadioButtons(radQ1_Slots, radQ1_Tables, radQ1_Poker, radQ1_Food, radQ1_Entertainment, radQ1_Hotel, radQ1_LiveRacing, radQ1_Racebook, radQ1_Bingo, radQ1_Lottery, radQ1_None);

						}
						

					}
					else if (strSurveyLang == "French")
					{


						
						if (!saveOnly)
						{
							bool noneSelected = !GetValue(radQ1_Slots_F, currentPage, false) &&
												!GetValue(radQ1_Tables_F, currentPage, false) &&
												!GetValue(radQ1_Poker_F, currentPage, false) &&
												!GetValue(radQ1_Food_F, currentPage, false) &&
												!GetValue(radQ1_Entertainment_F, currentPage, false) &&
												!GetValue(radQ1_Hotel_F, currentPage, false) &&
												!GetValue(radQ1_LiveRacing_F, currentPage, false) &&
												!GetValue(radQ1_Racebook_F, currentPage, false) &&
												!GetValue(radQ1_Bingo_F, currentPage, false) &&
												!GetValue(radQ1_Lottery_F, currentPage, false) &&
												!GetValue(radQ1_None_F, currentPage, false);

							if (!saveOnly && noneSelected)
							{
								MessageManager1.ErrorMessage = "Veuillez sélectionner l'une des options suivantes.";

								return false;
							}
						}
						if (currentPage)
						{
							SurveyTools.SaveRadioButtons(radQ1_Slots_F, radQ1_Tables_F, radQ1_Poker_F, radQ1_Food_F, radQ1_Entertainment_F, radQ1_Hotel_F, radQ1_LiveRacing_F, radQ1_Racebook_F, radQ1_Bingo_F, radQ1_Lottery_F, radQ1_None_F);

						}
						
					}

					break;
						#endregion Page 3


				
				
				
				case 4: // Q2-Q4
					#region Page 4

					if(strSurveyLang != "French")
					{
						if (!saveOnly)
						{

							if (!IsKioskOrStaffEntry)
							{
								bool q3Check = (PropertyShortCode != GCCPropertyShortCode.GAG)
												|| GetValue(radQ3_Everett, currentPage, false)
												|| GetValue(radQ3_Lakewood, currentPage, false)
												|| GetValue(radQ3_Tukwila, currentPage, false)
												|| GetValue(radQ3_DeMoines, currentPage, false);

								if (!q3Check)
								{
									mmQ3.ErrorMessage = "Please select one of the following options.";
									retVal = false;
								}
							}

							bool q4Check = (GetValue(Q4, currentPage, 0) == 1 && !String.IsNullOrEmpty(GetValue(txtQ4_CardNumber, currentPage, String.Empty)))
											|| GetValue(Q4, currentPage, 0) == 0;

							if (!q4Check)
							{
								Q4.MessageManager.ErrorMessage = "Please ensure you selected one of the following and, if you selected \"Yes\", please enter your member number / player card number.";
								retVal = false;
							}
						}
						if (currentPage)
						{
							//Q2
							SaveValue<bool>(chkQ2_Slots);
							SaveValue<bool>(chkQ2_Tables);
							SaveValue<bool>(chkQ2_Poker);
							SaveValue<bool>(chkQ2_Food);
							SaveValue<bool>(chkQ2_Entertainment);
							SaveValue<bool>(chkQ2_Hotel);
							SaveValue<bool>(chkQ2_LiveRacing);
							SaveValue<bool>(chkQ2_Racebook);
							SaveValue<bool>(chkQ2_Bingo);
							SaveValue<bool>(chkQ2_Lottery);
							//Q3
							if (PropertyShortCode == GCCPropertyShortCode.GAG && !IsKioskOrStaffEntry)
							{
								SurveyTools.SaveRadioButtons(radQ3_Everett, radQ3_Lakewood, radQ3_Tukwila, radQ3_DeMoines);
							}
							//Q4
							SaveValue<int>(Q4);
							SaveValue<string>(txtQ4_CardNumber);
						}


					}
					else if(strSurveyLang == "French")
					{

						if (!saveOnly)
						{

							if (!IsKioskOrStaffEntry)
							{
								bool q3Check = (PropertyShortCode != GCCPropertyShortCode.GAG)
												|| GetValue(radQ3_Everett_F, currentPage, false)
												|| GetValue(radQ3_Lakewood_F, currentPage, false)
												|| GetValue(radQ3_Tukwila_F, currentPage, false)
												|| GetValue(radQ3_DeMoines_F, currentPage, false);

								if (!q3Check)
								{
									mmQ3_F.ErrorMessage = "Veuillez sélectionner l'une des options suivantes.";
									retVal = false;
								}
							}

							bool q4Check = (GetValue(Q4_F, currentPage, 0) == 1 && !String.IsNullOrEmpty(GetValue(txtQ4_CardNumber_F, currentPage, String.Empty)))
											|| GetValue(Q4_F, currentPage, 0) == 0;

							if (!q4Check)
							{
								Q4.MessageManager.ErrorMessage = "Veuillez vous assurer que vous avez sélectionné l'une des options suivantes et, si vous avez sélectionné «Oui», veuillez entrer votre numéro de membre / numéro de carte de joueur.";
								retVal = false;
							}
						}
						if (currentPage)
						{
							//Q2
							SaveValue<bool>(chkQ2_Slots_F);
							SaveValue<bool>(chkQ2_Tables_F);
							SaveValue<bool>(chkQ2_Poker_F);
							SaveValue<bool>(chkQ2_Food_F);
							SaveValue<bool>(chkQ2_Entertainment_F);
							SaveValue<bool>(chkQ2_Hotel_F);
							SaveValue<bool>(chkQ2_LiveRacing_F);
							SaveValue<bool>(chkQ2_Racebook_F);
							SaveValue<bool>(chkQ2_Bingo_F);
							SaveValue<bool>(chkQ2_Lottery_F);
							//Q3
							if (PropertyShortCode == GCCPropertyShortCode.GAG && !IsKioskOrStaffEntry)
							{
								SurveyTools.SaveRadioButtons(radQ3_Everett_F, radQ3_Lakewood_F, radQ3_Tukwila_F, radQ3_DeMoines_F);
							}
							//Q4
							SaveValue<int>(Q4_F);
							SaveValue<string>(txtQ4_CardNumber_F);
						}


					}



										break;
					#endregion Page 4
				case 5: //Q5
					#region Page 5
				   if(strSurveyLang != "French")
				   {
					   if (!saveOnly)
					   {
						   if (!CheckForAnswer(Q5A)
							   | !CheckForAnswer(Q5B)
							   | !CheckForAnswer(Q6A)
							   | !CheckForAnswer(Q6B)
							   | !CheckForAnswer(Q6C)
							   | !CheckForAnswer(Q6D))
						   {
							   mmQ5.ErrorMessage = "Please select one of the following options.";
							   
							   retVal = false;


													   }
					   }
					   if (currentPage)
					   {
						   SaveValue(Q5A);
						   SaveValue(Q5B);
						   SaveValue(Q6A);
						   SaveValue(Q6B);
						   SaveValue(Q6C);
						   SaveValue(Q6D);
					   }
				   
					   
				   }
				   else if (strSurveyLang == "French")
				   {

					   if (!saveOnly)
					   {
						   if (!CheckForAnswer(Q5A_F)
							   | !CheckForAnswer(Q5B_F)
							   | !CheckForAnswer(Q6A_F)
							   | !CheckForAnswer(Q6B_F)
							   | !CheckForAnswer(Q6C_F)
							   | !CheckForAnswer(Q6D_F))
						   {
							   mmQ5_F.ErrorMessage = "Veuillez sélectionner l'une des options suivantes.";
							   retVal = false;
						   }
					   }
					   if (currentPage)
					   {
						   SaveValue(Q5A_F);
						   SaveValue(Q5B_F);
						   SaveValue(Q6A_F);
						   SaveValue(Q6B_F);
						   SaveValue(Q6C_F);
						   SaveValue(Q6D_F);
					   }
					   
				   }
				   break;
					#endregion Page 5
				case 6: //Q7-11
					#region Page 6

					if(strSurveyLang != "French")
					{
						if (!saveOnly)
						{
							bool isChances = PropertyShortCode == GCCPropertyShortCode.CCH || PropertyShortCode == GCCPropertyShortCode.CMR || PropertyShortCode == GCCPropertyShortCode.CDC;

							if (!CheckForAnswer(Q7A)
								| !CheckForAnswer(Q7B)
								| !CheckForAnswer(Q7C)
								| !CheckForAnswer(Q7D)
								| !CheckForAnswer(Q7E)
								| !CheckForAnswer(Q7F)
								| !CheckForAnswer(Q8)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q9A))
								| (!IsKioskOrStaffEntry && PropertyShortCode != GCCPropertyShortCode.GAG && !isChances && !CheckForAnswer(Q9B))
								| (!IsKioskOrStaffEntry && PropertyShortCode != GCCPropertyShortCode.GAG && !CheckForAnswer(Q9C))
								| (!IsKioskOrStaffEntry && PropertyShortCode != GCCPropertyShortCode.HA && !isChances && !CheckForAnswer(Q9D))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q9E))
								| (!IsKioskOrStaffEntry && !isChances && !CheckForAnswer(Q9F))
								| (!IsKioskOrStaffEntry && PropertyShortCode != GCCPropertyShortCode.GAG && PropertyShortCode != GCCPropertyShortCode.RR && !CheckForAnswer(Q9G))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q9H))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q9I))
								| (!IsKioskOrStaffEntry && (PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNB) && !CheckForAnswer(Q9J))
								| (!IsKioskOrStaffEntry && !isChances && !CheckForAnswer(Q9F))
								| (!IsKioskOrStaffEntry && (PropertyShortCode == GCCPropertyShortCode.CNSH) && !CheckForAnswer(Q9K_F))
								| !CheckForAnswer(Q10A)
								| !CheckForAnswer(Q10B)
								| !CheckForAnswer(Q10C)
								| !SurveyTools.CheckForAnswer(Q11, false) //Optional
								)
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							SaveValue(Q7A);
							SaveValue(Q7B);
							SaveValue(Q7C);
							SaveValue(Q7D);
							SaveValue(Q7E);
							SaveValue(Q7F);
							SaveValue(Q8);
							SaveValue(Q9A);
							SaveValue(Q9B);
							SaveValue(Q9C);
							SaveValue(Q9D);
							SaveValue(Q9E);
							SaveValue(Q9F);
							SaveValue(Q9G);
							SaveValue(Q9H);
							SaveValue(Q9I);
							SaveValue(Q9J);
							SaveValue(Q9K);

							SaveValue(Q10A);
							SaveValue(Q10B);
							SaveValue(Q10C);
							SaveValue(Q11);
						}
					

					}
					else if (strSurveyLang == "French")
					{

						if (!saveOnly)
						{
							bool isChances = PropertyShortCode == GCCPropertyShortCode.CCH || PropertyShortCode == GCCPropertyShortCode.CMR || PropertyShortCode == GCCPropertyShortCode.CDC;

							if (!CheckForAnswer(Q7A_F)
								| !CheckForAnswer(Q7B_F)
								| !CheckForAnswer(Q7C_F)
								| !CheckForAnswer(Q7D_F)
								| !CheckForAnswer(Q7E_F)
								| !CheckForAnswer(Q7F_F)
								| !CheckForAnswer(Q8_FN)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q9A_F))
								| (!IsKioskOrStaffEntry && PropertyShortCode != GCCPropertyShortCode.GAG && !isChances && !CheckForAnswer(Q9B_F))
								| (!IsKioskOrStaffEntry && PropertyShortCode != GCCPropertyShortCode.GAG && !CheckForAnswer(Q9C_F))
								| (!IsKioskOrStaffEntry && PropertyShortCode != GCCPropertyShortCode.HA && !isChances && !CheckForAnswer(Q9D_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q9E_F))
								| (!IsKioskOrStaffEntry && !isChances && !CheckForAnswer(Q9F_F))
								| (!IsKioskOrStaffEntry && PropertyShortCode != GCCPropertyShortCode.GAG && PropertyShortCode != GCCPropertyShortCode.RR && !CheckForAnswer(Q9G_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q9H_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q9I_F))
								| (!IsKioskOrStaffEntry && (PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNB) && !CheckForAnswer(Q9J_F))
								| (!IsKioskOrStaffEntry && (PropertyShortCode == GCCPropertyShortCode.CNSH) && !CheckForAnswer(Q9K_F))
								| !CheckForAnswer(Q10A_F)
								| !CheckForAnswer(Q10B_F)
								| !CheckForAnswer(Q10C_F)
								| !SurveyTools.CheckForAnswer(Q11_F, false) //Optional
								)
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							SaveValue(Q7A_F);
							SaveValue(Q7B_F);
							SaveValue(Q7C_F);
							SaveValue(Q7D_F);
							SaveValue(Q7E_F);
							SaveValue(Q7F_F);
							SaveValue(Q8_FN);
							SaveValue(Q9A_F);
							SaveValue(Q9B_F);
							SaveValue(Q9C_F);
							SaveValue(Q9D_F);
							SaveValue(Q9E_F);
							SaveValue(Q9F_F);
							SaveValue(Q9G_F);
							SaveValue(Q9H_F);
							SaveValue(Q9I_F);
							SaveValue(Q9J_F);
							SaveValue(Q9K_F);
							SaveValue(Q10A_F);
							SaveValue(Q10B_F);
							SaveValue(Q10C_F);
							SaveValue(Q11_F);
						}
					
					}

					break;
					#endregion Page 6
				case 7: //Q12-13
					#region Page 7
					
					if(strSurveyLang != "French")
					{

						if (!saveOnly)
						{
							if (!CheckForAnswer(Q12)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13A))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13B))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13C))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13D))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13E))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13G)))
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							SaveValue(Q12);
							SaveValue(Q13A);
							SaveValue(Q13B);
							SaveValue(Q13C);
							SaveValue(Q13D);
							SaveValue(Q13E);
							SaveValue(Q13F);
							SaveValue(Q13G);
						}
					}
					else if(strSurveyLang == "French")
					{

						if (!saveOnly)
						{
							if (!CheckForAnswer(Q12_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13A_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13B_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13C_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13D_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13E_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13F_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q13G_F)))
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							SaveValue(Q12_F);
							SaveValue(Q13A_F);
							SaveValue(Q13B_F);
							SaveValue(Q13C_F);
							SaveValue(Q13D_F);
							SaveValue(Q13E_F);
							SaveValue(Q13F_F);
							SaveValue(Q13G_F);
						}
					}



					break;
					#endregion Page 7
				case 8: //Q14-17
					#region Page 8
				   
					if(strSurveyLang != "French")
					{

						if (!saveOnly)
						{
							bool check1415 = (radQ1_Slots.Checked || chkQ2_Slots.Checked
										   || radQ1_Tables.Checked || chkQ2_Tables.Checked
										   || radQ1_Poker.Checked || chkQ2_Poker.Checked
										   || radQ1_LiveRacing.Checked || chkQ2_LiveRacing.Checked
										   || radQ1_Racebook.Checked || chkQ2_Racebook.Checked
										   || radQ1_Bingo.Checked || chkQ2_Bingo.Checked
										   || radQ1_Lottery.Checked || chkQ2_Lottery.Checked);
							bool check16 = (Q4.SelectedValue == 1 && !IsKioskOrStaffEntry);
							if ((check1415 && //Checking Q14/15
									(!CheckForAnswer(Q14)
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15A))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15B))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15C))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15D))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15E))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15F))
								))
								| (check16 &&
									(!CheckForAnswer(Q16A)
									| !CheckForAnswer(Q16B)
									| !CheckForAnswer(Q16C)
									| (PropertyShortCode != GCCPropertyShortCode.GAG && !CheckForAnswer(Q16D))
								))
								| !CheckForAnswer(Q17)
								)
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							SaveValue(Q14);
							SaveValue(Q15A);
							SaveValue(Q15B);
							SaveValue(Q15C);
							SaveValue(Q15D);
							SaveValue(Q15E);
							SaveValue(Q15F);
							SaveValue(Q16A);
							SaveValue(Q16B);
							SaveValue(Q16C);
							SaveValue(Q16D);
							SaveValue(Q17);
						}
					}

					else if(strSurveyLang == "French")
					{

						if (!saveOnly)
						{
							bool check1415 = (radQ1_Slots_F.Checked || chkQ2_Slots_F.Checked
										   || radQ1_Tables_F.Checked || chkQ2_Tables_F.Checked
										   || radQ1_Poker_F.Checked || chkQ2_Poker_F.Checked
										   || radQ1_LiveRacing_F.Checked || chkQ2_LiveRacing_F.Checked
										   || radQ1_Racebook_F.Checked || chkQ2_Racebook_F.Checked
										   || radQ1_Bingo_F.Checked || chkQ2_Bingo_F.Checked
										   || radQ1_Lottery_F.Checked || chkQ2_Lottery_F.Checked);
							bool check16 = (Q4_F.SelectedValue_F == 1 && !IsKioskOrStaffEntry);
							if ((check1415 && //Checking Q14/15
									(!CheckForAnswer(Q14_F)
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15A_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15B_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15C_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15D_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15E_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q15F_F))
								))
								| (check16 &&
									(!CheckForAnswer(Q16A_F)
									| !CheckForAnswer(Q16B_F)
									| !CheckForAnswer(Q16C_F)
									| (PropertyShortCode != GCCPropertyShortCode.GAG && !CheckForAnswer(Q16D_F))
								))
								| !CheckForAnswer(Q17_F)
								)
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							SaveValue(Q14_F);
							SaveValue(Q15A_F);
							SaveValue(Q15B_F);
							SaveValue(Q15C_F);
							SaveValue(Q15D_F);
							SaveValue(Q15E_F);
							SaveValue(Q15F_F);
							SaveValue(Q16A_F);
							SaveValue(Q16B_F);
							SaveValue(Q16C_F);
							SaveValue(Q16D_F);
							SaveValue(Q17_F);
						}

					}


					break;
					#endregion Page 8
				case 9: //Q18
					#region Page 9


					if(strSurveyLang != "French")
					{
						//This contains a single checkbox question so nothing to validate here
						if (currentPage)
						{
							SaveValue(Q18_1);
							SaveValue(Q18_2);
							SaveValue(Q18_3);
							SaveValue(Q18_4);
							SaveValue(Q18_5);
							SaveValue(Q18_6);
							SaveValue(Q18_7);
							SaveValue(Q18_8);
							SaveValue(Q18_9);
							SaveValue(Q18_10);
							SaveValue(Q18_11);
							SaveValue(Q18_12);
							SaveValue(Q18_13);
							SaveValue(Q18_14);
							SaveValue(Q18_15);
							SaveValue(Q18_16);
							SaveValue(Q18_17);
							SaveValue(Q18_18);
							SaveValue(Q18_19);
							SaveValue(Q18_20);
							SaveValue(Q18_21);
							//SaveValue( Q18_22 );
							//SaveValue( Q18_23 );
							//SaveValue( Q18_24 );
							SaveValue(Q18_25);
							SaveValue(Q18_26);
							SaveValue(Q18_27);
							SaveValue(Q18_28);
							SaveValue(Q18_29);
							SaveValue(Q18_30);
							SaveValue(Q18_31);
							SaveValue(Q18_32);
							SaveValue(Q18_33);
							SaveValue(Q18_34);
							SaveValue(Q18_35);
							SaveValue(Q18_36);
							SaveValue(Q18_37);
							SaveValue(Q18_38);
							SaveValue(Q18_39);
							SaveValue(Q18_40);
							SaveValue(Q18_41);
							SaveValue(Q18_42);
							SaveValue(Q18_43);
							SaveValue(Q18_44);
							SaveValue(Q18_45);
							SaveValue(Q18_46);
							SaveValue(Q18_47);
							SaveValue(Q18_48);
							SaveValue(Q18_49);
							SaveValue(Q18_50);
							SaveValue(Q18_51);
							SaveValue(Q18_52);
							SaveValue(Q18_53);
							SaveValue(Q18_54);
							SaveValue(Q18_55);
							SaveValue(Q18_56);
							SaveValue(Q18_57);
							SaveValue(Q18_58);
							SaveValue(Q18_59);
							SaveValue(Q18_60);
							SaveValue(Q18_61);
							SaveValue(Q18_62);
						}
				 

					}
					else if (strSurveyLang == "French")
					{
						//This contains a single checkbox question so nothing to validate here
						if (currentPage)
						{
							SaveValue(Q18_1_F);
							SaveValue(Q18_2_F);
							SaveValue(Q18_3_F);
							SaveValue(Q18_4_F);
							SaveValue(Q18_5_F);
							SaveValue(Q18_6_F);
							SaveValue(Q18_7_F);
							SaveValue(Q18_8_F);
							SaveValue(Q18_9_F);
							SaveValue(Q18_10_F);
							SaveValue(Q18_11_F);
							SaveValue(Q18_12_F);
							SaveValue(Q18_13_F);
							SaveValue(Q18_14_F);
							SaveValue(Q18_15_F);
							SaveValue(Q18_16_F);
							SaveValue(Q18_17_F);
							SaveValue(Q18_18_F);
							SaveValue(Q18_19_F);
							SaveValue(Q18_20_F);
							SaveValue(Q18_21_F);
							//SaveValue( Q18_22 );
							//SaveValue( Q18_23 );
							//SaveValue( Q18_24 );
							SaveValue(Q18_25_F);
							SaveValue(Q18_26_F);
							SaveValue(Q18_27_F);
							SaveValue(Q18_28_F);
							SaveValue(Q18_29_F);
							SaveValue(Q18_30_F);
							SaveValue(Q18_31_F);
							SaveValue(Q18_32_F);
							SaveValue(Q18_33_F);
							SaveValue(Q18_34_F);
							SaveValue(Q18_35_F);
							SaveValue(Q18_36_F);
							SaveValue(Q18_37_F);
							SaveValue(Q18_38_F);
							SaveValue(Q18_39_F);
							SaveValue(Q18_40_F);
							SaveValue(Q18_41_F);
							SaveValue(Q18_42_F);
							SaveValue(Q18_43_F);
							SaveValue(Q18_44_F);
							SaveValue(Q18_45_F);
							SaveValue(Q18_46_F);
							SaveValue(Q18_47_F);
							SaveValue(Q18_48_F);
							SaveValue(Q18_49_F);
							SaveValue(Q18_50_F);
							SaveValue(Q18_51_F);
							SaveValue(Q18_52_F);
							SaveValue(Q18_53_F);
							SaveValue(Q18_54_F);
							SaveValue(Q18_55_F);
							SaveValue(Q18_56_F);
							SaveValue(Q18_57_F);
							SaveValue(Q18_58_F);
							SaveValue(Q18_59_F);
							SaveValue(Q18_60_F);
							SaveValue(Q18_61_F);
							SaveValue(Q18_62_F);
						}
				 

					}



					break;
					#endregion Page 9
				
				
				case 10: //Q19-20
					#region Page 10
				
   
					if(strSurveyLang =="English")
					{

						if (!saveOnly)
						{
							if ((Q18_1.Checked || Q18_42.Checked) && (
								  !CheckForAnswer(Q19_M1)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M1))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M1))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M1))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M1))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M1))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M1))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M1))
							))
							{
								retVal = false;
							}
							if ((Q18_2.Checked || Q18_14.Checked || Q18_21.Checked || Q18_26.Checked || Q18_32.Checked || Q18_34.Checked || Q18_35.Checked || Q18_36.Checked || Q18_37.Checked || Q18_41.Checked || Q18_47.Checked || Q18_48.Checked || Q18_42.Checked || Q18_49.Checked || Q18_53.Checked || Q18_55.Checked || Q18_58.Checked || Q18_59.Checked) && (
								  !CheckForAnswer(Q19_M2)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M2))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M2))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M2))
							   // | (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M2))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M2))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M2))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M2))
							))
							{
								retVal = false;
							}
							//if ( ( Q18_3.Checked || Q18_15.Checked || Q18_22.Checked || ( Q18_25.Checked && PropertyShortCode == GCCPropertyShortCode.EC ) || Q18_27.Checked || Q18_33.Checked || Q18_38.Checked ) && (
							if ((Q18_3.Checked || Q18_15.Checked || (Q18_25.Checked && PropertyShortCode == GCCPropertyShortCode.EC) || Q18_27.Checked || Q18_33.Checked || Q18_38.Checked || Q18_50.Checked || Q18_54.Checked || Q18_56.Checked || Q18_60.Checked) && (
								  !CheckForAnswer(Q19_M3)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M3))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M3))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M3))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M3))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M3))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M3))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M3))
							))
							{
								retVal = false;
							}
							//if ( ( Q18_4.Checked || Q18_16.Checked || Q18_23.Checked || Q18_28.Checked || Q18_39.Checked || Q18_43.Checked ) && (
							if ((Q18_4.Checked || Q18_16.Checked || Q18_28.Checked || Q18_39.Checked || Q18_43.Checked || Q18_51.Checked || Q18_57.Checked || Q18_61.Checked) && (
								  !CheckForAnswer(Q19_M4)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M4))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M4))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M4))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M4))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M4))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M4))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M4))
							))
							{
								retVal = false;
							}
							//if ( ( Q18_5.Checked || Q18_17.Checked || Q18_24.Checked || Q18_29.Checked || Q18_40.Checked || Q18_44.Checked ) && (
							if ((Q18_5.Checked || Q18_17.Checked || Q18_29.Checked || Q18_40.Checked || Q18_44.Checked || Q18_52.Checked || Q18_62.Checked) && (
								  !CheckForAnswer(Q19_M5)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M5))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M5))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M5))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M5))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M5))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M5))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M5))
							))
							{
								retVal = false;
							}
							if ((Q18_6.Checked || Q18_18.Checked || (Q18_25.Checked && PropertyShortCode != GCCPropertyShortCode.EC) || Q18_30.Checked || (Q18_9.Checked && PropertyShortCode == GCCPropertyShortCode.CNSH) || Q18_45.Checked) && (
								  !CheckForAnswer(Q19_M6)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M6))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M6))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M6))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M6))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M6))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M6))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M6))
							))
							{
								retVal = false;
							}
							if ((Q18_7.Checked || Q18_19.Checked || Q18_31.Checked || Q18_46.Checked) && (
								  !CheckForAnswer(Q19_M7)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M7))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M7))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M7))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M7))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M7))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M7))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M7))
							))
							{
								retVal = false;
							}
							if ((Q18_8.Checked || Q18_20.Checked) && (
								  !CheckForAnswer(Q19_M8)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M8))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M8))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M8))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M8))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M8))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M8))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M8))
							))
							{
								retVal = false;
							}
							if ((Q18_9.Checked && PropertyShortCode != GCCPropertyShortCode.CNSH) && (
								  !CheckForAnswer(Q19_M9)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M9))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M9))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M9))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M9))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M9))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M9))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M9))
							))
							{
								retVal = false;
							}
							if ((Q18_10.Checked) && (
								  !CheckForAnswer(Q19_M10)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M10))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M10))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M10))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M10))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M10))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M10))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M10))
							))
							{
								retVal = false;
							}
							if ((Q18_11.Checked) && (
								  !CheckForAnswer(Q19_M11)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M11))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M11))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M11))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M11))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M11))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M11))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M11))
							))
							{
								retVal = false;
							}
							if ((Q18_12.Checked) && (
								  !CheckForAnswer(Q19_M12)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M12))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M12))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M12))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M12))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M12))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M12))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M12))
							))
							{
								retVal = false;
							}
							if ((Q18_13.Checked) && (
								  !CheckForAnswer(Q19_M13)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M13))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M13))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M13))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M13))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M13))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M13))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M13))
							))
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							if (Q18_1.Checked || Q18_42.Checked)
							{
								SaveValue(Q19_M1);
								SaveValue(Q20A_M1);
								SaveValue(Q20B_M1);
								SaveValue(Q20C_M1);
								SaveValue(Q20D_M1);
								SaveValue(Q20E_M1);
								SaveValue(Q20F_M1);
								SaveValue(Q20G_M1);
							}
							if (Q18_2.Checked || Q18_14.Checked || Q18_21.Checked || Q18_26.Checked || Q18_32.Checked || Q18_34.Checked || Q18_35.Checked || Q18_36.Checked || Q18_37.Checked || Q18_41.Checked || Q18_47.Checked || Q18_48.Checked || Q18_42.Checked || Q18_49.Checked || Q18_53.Checked || Q18_55.Checked || Q18_58.Checked || Q18_59.Checked)
							{
								SaveValue(Q19_M2);
								SaveValue(Q20A_M2);
								SaveValue(Q20B_M2);
								SaveValue(Q20C_M2);
								if (PropertyShortCode != GCCPropertyShortCode.CNSH)
								{
									SaveValue(Q20D_M2);
								}
								SaveValue(Q20E_M2);
								SaveValue(Q20F_M2);
								SaveValue(Q20G_M2);
							}
							//if ( Q18_3.Checked || Q18_15.Checked || Q18_22.Checked || ( Q18_25.Checked && PropertyShortCode == GCCPropertyShortCode.EC ) || Q18_27.Checked || Q18_33.Checked || Q18_38.Checked ) {
							if (Q18_3.Checked || Q18_15.Checked || (Q18_25.Checked && PropertyShortCode == GCCPropertyShortCode.EC) || Q18_27.Checked || Q18_33.Checked || Q18_38.Checked || Q18_50.Checked || Q18_54.Checked || Q18_56.Checked || Q18_60.Checked)
							{
								SaveValue(Q19_M3);
								SaveValue(Q20A_M3);
								SaveValue(Q20B_M3);
								SaveValue(Q20C_M3);
								SaveValue(Q20D_M3);
								SaveValue(Q20E_M3);
								SaveValue(Q20F_M3);
								SaveValue(Q20G_M3);
							}
							//if ( Q18_4.Checked || Q18_16.Checked || Q18_23.Checked || Q18_28.Checked || Q18_39.Checked || Q18_43.Checked ) {
							if (Q18_4.Checked || Q18_16.Checked || Q18_28.Checked || Q18_39.Checked || Q18_43.Checked || Q18_51.Checked || Q18_57.Checked || Q18_61.Checked)
							{
								SaveValue(Q19_M4);
								SaveValue(Q20A_M4);
								SaveValue(Q20B_M4);
								SaveValue(Q20C_M4);
								SaveValue(Q20D_M4);
								SaveValue(Q20E_M4);
								SaveValue(Q20F_M4);
								SaveValue(Q20G_M4);
							}
							//if ( Q18_5.Checked || Q18_17.Checked || Q18_24.Checked || Q18_29.Checked || Q18_40.Checked || Q18_44.Checked ) {
							if (Q18_5.Checked || Q18_17.Checked || Q18_29.Checked || Q18_40.Checked || Q18_44.Checked || Q18_52.Checked || Q18_62.Checked)
							{
								SaveValue(Q19_M5);
								SaveValue(Q20A_M5);
								SaveValue(Q20B_M5);
								SaveValue(Q20C_M5);
								SaveValue(Q20D_M5);
								SaveValue(Q20E_M5);
								SaveValue(Q20F_M5);
								SaveValue(Q20G_M5);
							}
							if (Q18_6.Checked || Q18_18.Checked || (Q18_25.Checked && PropertyShortCode != GCCPropertyShortCode.EC) || Q18_30.Checked || (Q18_9.Checked && PropertyShortCode == GCCPropertyShortCode.CNSH) || Q18_45.Checked)
							{
								SaveValue(Q19_M6);
								SaveValue(Q20A_M6);
								SaveValue(Q20B_M6);
								SaveValue(Q20C_M6);
								SaveValue(Q20D_M6);
								SaveValue(Q20E_M6);
								SaveValue(Q20F_M6);
								SaveValue(Q20G_M6);
							}
							if (Q18_7.Checked || Q18_19.Checked || Q18_31.Checked || Q18_46.Checked)
							{
								SaveValue(Q19_M7);
								SaveValue(Q20A_M7);
								SaveValue(Q20B_M7);
								SaveValue(Q20C_M7);
								SaveValue(Q20D_M7);
								SaveValue(Q20E_M7);
								SaveValue(Q20F_M7);
								SaveValue(Q20G_M7);
							}
							if (Q18_8.Checked || Q18_20.Checked)
							{
								SaveValue(Q19_M8);
								SaveValue(Q20A_M8);
								SaveValue(Q20B_M8);
								SaveValue(Q20C_M8);
								SaveValue(Q20D_M8);
								SaveValue(Q20E_M8);
								SaveValue(Q20F_M8);
								SaveValue(Q20G_M8);
							}
							if (Q18_9.Checked && PropertyShortCode != GCCPropertyShortCode.CNSH)
							{
								SaveValue(Q19_M9);
								SaveValue(Q20A_M9);
								SaveValue(Q20B_M9);
								SaveValue(Q20C_M9);
								SaveValue(Q20D_M9);
								SaveValue(Q20E_M9);
								SaveValue(Q20F_M9);
								SaveValue(Q20G_M9);
							}
							if (Q18_10.Checked)
							{
								SaveValue(Q19_M10);
								SaveValue(Q20A_M10);
								SaveValue(Q20B_M10);
								SaveValue(Q20C_M10);
								SaveValue(Q20D_M10);
								SaveValue(Q20E_M10);
								SaveValue(Q20F_M10);
								SaveValue(Q20G_M10);
							}
							if (Q18_11.Checked)
							{
								SaveValue(Q19_M11);
								SaveValue(Q20A_M11);
								SaveValue(Q20B_M11);
								SaveValue(Q20C_M11);
								SaveValue(Q20D_M11);
								SaveValue(Q20E_M11);
								SaveValue(Q20F_M11);
								SaveValue(Q20G_M11);
							}
							if (Q18_12.Checked)
							{
								SaveValue(Q19_M12);
								SaveValue(Q20A_M12);
								SaveValue(Q20B_M12);
								SaveValue(Q20C_M12);
								SaveValue(Q20D_M12);
								SaveValue(Q20E_M12);
								SaveValue(Q20F_M12);
								SaveValue(Q20G_M12);
							}
							if (Q18_13.Checked)
							{
								SaveValue(Q19_M13);
								SaveValue(Q20A_M13);
								SaveValue(Q20B_M13);
								SaveValue(Q20C_M13);
								SaveValue(Q20D_M13);
								SaveValue(Q20E_M13);
								SaveValue(Q20F_M13);
								SaveValue(Q20G_M13);
							}
						}

					}

					else if(strSurveyLang == "French")
					{

						if (!saveOnly)
						{
							if ((Q18_1_F.Checked || Q18_42_F.Checked) && (
								  !CheckForAnswer(Q19_M1_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M1_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M1_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M1_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M1_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M1_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M1_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M1_F))
							))
							{
								retVal = false;
							}
							if ((Q18_2_F.Checked || Q18_14_F.Checked || Q18_21_F.Checked || Q18_26_F.Checked || Q18_32_F.Checked || Q18_34_F.Checked || Q18_35_F.Checked || Q18_36_F.Checked || Q18_37_F.Checked || Q18_41_F.Checked || Q18_47_F.Checked || Q18_48_F.Checked || Q18_42_F.Checked || Q18_49_F.Checked || Q18_53_F.Checked || Q18_55_F.Checked || Q18_58_F.Checked || Q18_59_F.Checked) && (
								  !CheckForAnswer(Q19_M2_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M2_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M2_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M2_F))
								//| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M2_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M2_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M2_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M2_F))
							))
							{
								retVal = false;
							}
							//if ( ( Q18_3.Checked || Q18_15.Checked || Q18_22.Checked || ( Q18_25.Checked && PropertyShortCode == GCCPropertyShortCode.EC ) || Q18_27.Checked || Q18_33.Checked || Q18_38.Checked ) && (
							if ((Q18_3_F.Checked || Q18_15_F.Checked || (Q18_25_F.Checked && PropertyShortCode == GCCPropertyShortCode.EC) || Q18_27_F.Checked || Q18_33_F.Checked || Q18_38_F.Checked || Q18_50_F.Checked || Q18_54_F.Checked || Q18_56_F.Checked || Q18_60_F.Checked) && (
								  !CheckForAnswer(Q19_M3_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M3_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M3_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M3_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M3_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M3_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M3_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M3_F))
							))
							{
								retVal = false;
							}
							//if ( ( Q18_4.Checked || Q18_16.Checked || Q18_23.Checked || Q18_28.Checked || Q18_39.Checked || Q18_43.Checked ) && (
							if ((Q18_4_F.Checked || Q18_16_F.Checked || Q18_28_F.Checked || Q18_39_F.Checked || Q18_43_F.Checked || Q18_51_F.Checked || Q18_57_F.Checked || Q18_61_F.Checked) && (
								  !CheckForAnswer(Q19_M4_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M4_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M4_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M4_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M4_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M4_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M4_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M4_F))
							))
							{
								retVal = false;
							}
							//if ( ( Q18_5.Checked || Q18_17.Checked || Q18_24.Checked || Q18_29.Checked || Q18_40.Checked || Q18_44.Checked ) && (
							if ((Q18_5_F.Checked || Q18_17_F.Checked || Q18_29_F.Checked || Q18_40_F.Checked || Q18_44_F.Checked || Q18_52_F.Checked || Q18_62_F.Checked) && (
								  !CheckForAnswer(Q19_M5_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M5_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M5_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M5_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M5_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M5_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M5_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M5_F))
							))
							{
								retVal = false;
							}
							if ((Q18_6_F.Checked || Q18_18_F.Checked || (Q18_25_F.Checked && PropertyShortCode != GCCPropertyShortCode.EC) || Q18_30_F.Checked || (Q18_9_F.Checked && PropertyShortCode == GCCPropertyShortCode.CNSH) || Q18_45_F.Checked) && (
								  !CheckForAnswer(Q19_M6_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M6_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M6_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M6_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M6_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M6_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M6_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M6_F))
							))
							{
								retVal = false;
							}
							if ((Q18_7_F.Checked || Q18_19_F.Checked || Q18_31_F.Checked || Q18_46_F.Checked) && (
								  !CheckForAnswer(Q19_M7_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M7_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M7_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M7_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M7_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M7_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M7_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M7_F))
							))
							{
								retVal = false;
							}
							if ((Q18_8_F.Checked || Q18_20_F.Checked) && (
								  !CheckForAnswer(Q19_M8_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M8_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M8_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M8_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M8_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M8_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M8_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M8_F))
							))
							{
								retVal = false;
							}
							if ((Q18_9_F.Checked && PropertyShortCode != GCCPropertyShortCode.CNSH) && (
								  !CheckForAnswer(Q19_M9)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M9_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M9_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M9_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M9_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M9_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M9_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M9_F))
							))
							{
								retVal = false;
							}
							if ((Q18_10_F.Checked) && (
								  !CheckForAnswer(Q19_M10_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M10_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M10_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M10_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M10_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M10_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M10_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M10_F))
							))
							{
								retVal = false;
							}
							if ((Q18_11_F.Checked) && (
								  !CheckForAnswer(Q19_M11_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M11_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M11_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M11_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M11_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M11_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M11_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M11_F))
							))
							{
								retVal = false;
							}
							if ((Q18_12_F.Checked) && (
								  !CheckForAnswer(Q19_M12_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M12_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M12_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M12_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M12_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M12_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M12_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M12_F))
							))
							{
								retVal = false;
							}
							if ((Q18_13_F.Checked) && (
								  !CheckForAnswer(Q19_M13_F)
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20A_M13_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20B_M13_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20C_M13_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20D_M13_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20E_M13_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20F_M13_F))
								| (!IsKioskOrStaffEntry && !CheckForAnswer(Q20G_M13_F))
							))
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							if (Q18_1_F.Checked || Q18_42_F.Checked)
							{
								SaveValue(Q19_M1_F);
								SaveValue(Q20A_M1_F);
								SaveValue(Q20B_M1_F);
								SaveValue(Q20C_M1_F);
								SaveValue(Q20D_M1_F);
								SaveValue(Q20E_M1_F);
								SaveValue(Q20F_M1_F);
								SaveValue(Q20G_M1_F);
							}
							if (Q18_2_F.Checked || Q18_14_F.Checked || Q18_21_F.Checked || Q18_26_F.Checked || Q18_32_F.Checked || Q18_34_F.Checked || Q18_35_F.Checked || Q18_36_F.Checked || Q18_37_F.Checked || Q18_41_F.Checked || Q18_47_F.Checked || Q18_48_F.Checked || Q18_42_F.Checked || Q18_49_F.Checked || Q18_53_F.Checked)
							{
								SaveValue(Q19_M2_F);
								SaveValue(Q20A_M2_F);
								SaveValue(Q20B_M2_F);
								SaveValue(Q20C_M2_F);
								if (PropertyShortCode != GCCPropertyShortCode.CNSH)
								{
									SaveValue(Q20D_M2_F);
								}
								SaveValue(Q20E_M2_F);
								SaveValue(Q20F_M2_F);
								SaveValue(Q20G_M2_F);
							}
							//if ( Q18_3.Checked || Q18_15.Checked || Q18_22.Checked || ( Q18_25.Checked && PropertyShortCode == GCCPropertyShortCode.EC ) || Q18_27.Checked || Q18_33.Checked || Q18_38.Checked ) {
							if (Q18_3_F.Checked || Q18_15_F.Checked || (Q18_25_F.Checked && PropertyShortCode == GCCPropertyShortCode.EC) || Q18_27_F.Checked || Q18_33_F.Checked || Q18_38_F.Checked || Q18_50_F.Checked)
							{
								SaveValue(Q19_M3_F);
								SaveValue(Q20A_M3_F);
								SaveValue(Q20B_M3_F);
								SaveValue(Q20C_M3_F);
								SaveValue(Q20D_M3_F);
								SaveValue(Q20E_M3_F);
								SaveValue(Q20F_M3_F);
								SaveValue(Q20G_M3_F);
							}
							//if ( Q18_4.Checked || Q18_16.Checked || Q18_23.Checked || Q18_28.Checked || Q18_39.Checked || Q18_43.Checked ) {
							if (Q18_4_F.Checked || Q18_16_F.Checked || Q18_28_F.Checked || Q18_39_F.Checked || Q18_43_F.Checked || Q18_51_F.Checked)
							{
								SaveValue(Q19_M4_F);
								SaveValue(Q20A_M4_F);
								SaveValue(Q20B_M4_F);
								SaveValue(Q20C_M4_F);
								SaveValue(Q20D_M4_F);
								SaveValue(Q20E_M4_F);
								SaveValue(Q20F_M4_F);
								SaveValue(Q20G_M4_F);
							}
							//if ( Q18_5.Checked || Q18_17.Checked || Q18_24.Checked || Q18_29.Checked || Q18_40.Checked || Q18_44.Checked ) {
							if (Q18_5_F.Checked || Q18_17_F.Checked || Q18_29_F.Checked || Q18_40_F.Checked || Q18_44_F.Checked || Q18_52_F.Checked || Q18_54.Checked)
							{
								SaveValue(Q19_M5_F);
								SaveValue(Q20A_M5_F);
								SaveValue(Q20B_M5_F);
								SaveValue(Q20C_M5_F);
								SaveValue(Q20D_M5_F);
								SaveValue(Q20E_M5_F);
								SaveValue(Q20F_M5_F);
								SaveValue(Q20G_M5_F);
							}
							if (Q18_6_F.Checked || Q18_18_F.Checked || (Q18_25_F.Checked && PropertyShortCode != GCCPropertyShortCode.EC) || Q18_30_F.Checked || (Q18_9_F.Checked && PropertyShortCode == GCCPropertyShortCode.CNSH) || Q18_45_F.Checked)
							{
								SaveValue(Q19_M6_F);
								SaveValue(Q20A_M6_F);
								SaveValue(Q20B_M6_F);
								SaveValue(Q20C_M6_F);
								SaveValue(Q20D_M6_F);
								SaveValue(Q20E_M6_F);
								SaveValue(Q20F_M6_F);
								SaveValue(Q20G_M6_F);
							}
							if (Q18_7_F.Checked || Q18_19_F.Checked || Q18_31_F.Checked || Q18_46_F.Checked)
							{
								SaveValue(Q19_M7_F);
								SaveValue(Q20A_M7_F);
								SaveValue(Q20B_M7_F);
								SaveValue(Q20C_M7_F);
								SaveValue(Q20D_M7_F);
								SaveValue(Q20E_M7_F);
								SaveValue(Q20F_M7_F);
								SaveValue(Q20G_M7_F);
							}
							if (Q18_8_F.Checked || Q18_20_F.Checked)
							{
								SaveValue(Q19_M8_F);
								SaveValue(Q20A_M8_F);
								SaveValue(Q20B_M8_F);
								SaveValue(Q20C_M8_F);
								SaveValue(Q20D_M8_F);
								SaveValue(Q20E_M8_F);
								SaveValue(Q20F_M8_F);
								SaveValue(Q20G_M8_F);
							}
							if (Q18_9_F.Checked && PropertyShortCode != GCCPropertyShortCode.CNSH)
							{
								SaveValue(Q19_M9_F);
								SaveValue(Q20A_M9_F);
								SaveValue(Q20B_M9_F);
								SaveValue(Q20C_M9_F);
								SaveValue(Q20D_M9_F);
								SaveValue(Q20E_M9_F);
								SaveValue(Q20F_M9_F);
								SaveValue(Q20G_M9_F);
							}
							if (Q18_10_F.Checked)
							{
								SaveValue(Q19_M10_F);
								SaveValue(Q20A_M10_F);
								SaveValue(Q20B_M10_F);
								SaveValue(Q20C_M10_F);
								SaveValue(Q20D_M10_F);
								SaveValue(Q20E_M10_F);
								SaveValue(Q20F_M10_F);
								SaveValue(Q20G_M10_F);
							}
							if (Q18_11_F.Checked)
							{
								SaveValue(Q19_M11_F);
								SaveValue(Q20A_M11_F);
								SaveValue(Q20B_M11_F);
								SaveValue(Q20C_M11_F);
								SaveValue(Q20D_M11_F);
								SaveValue(Q20E_M11_F);
								SaveValue(Q20F_M11_F);
								SaveValue(Q20G_M11_F);
							}
							if (Q18_12_F.Checked)
							{
								SaveValue(Q19_M12_F);
								SaveValue(Q20A_M12_F);
								SaveValue(Q20B_M12_F);
								SaveValue(Q20C_M12_F);
								SaveValue(Q20D_M12_F);
								SaveValue(Q20E_M12_F);
								SaveValue(Q20F_M12_F);
								SaveValue(Q20G_M12_F);
							}
							if (Q18_13_F.Checked)
							{
								SaveValue(Q19_M13_F);
								SaveValue(Q20A_M13_F);
								SaveValue(Q20B_M13_F);
								SaveValue(Q20C_M13_F);
								SaveValue(Q20D_M13_F);
								SaveValue(Q20E_M13_F);
								SaveValue(Q20F_M13_F);
								SaveValue(Q20G_M13_F);
							}
						}



					}



					break;
					#endregion Page 10
				case 11: //Q21
					#region Page 11

					if(strSurveyLang =="English")
					{

						if (!saveOnly)
						{
							if (PropertyShortCode == GCCPropertyShortCode.RR
								|| PropertyShortCode == GCCPropertyShortCode.HRCV
								|| PropertyShortCode == GCCPropertyShortCode.VRL
								|| PropertyShortCode == GCCPropertyShortCode.CCH
								|| PropertyShortCode == GCCPropertyShortCode.CMR
								|| PropertyShortCode == GCCPropertyShortCode.CDC
								|| PropertyShortCode == GCCPropertyShortCode.CNSH
								|| PropertyShortCode == GCCPropertyShortCode.EC
								|| PropertyShortCode == GCCPropertyShortCode.SCTI
								|| PropertyShortCode == GCCPropertyShortCode.CNB
								|| PropertyShortCode == GCCPropertyShortCode.SCBE)
							{
								//if ( !CheckForAnswer( Q21 )
								//    | ( Q21.SelectedValue == 1 && ( String.IsNullOrEmpty( txtShowVisitDate.GetValue() ) || txtShowVisitDate.GetValue().Trim().Length == 0 ) ) ) {
								//    mmQ21A.ErrorMessage = String.Format( "Please select the date you visited {0}.", GetShowLoungeName() );
								//    retVal = false;
								//}
							}
							if (PropertyShortCode == GCCPropertyShortCode.HRCV && Q21.SelectedValue == 1)
							{
								bool noneSelected = !GetValue(radQ21_HRCV_LoungeA, currentPage, false) &&
													!GetValue(radQ21_HRCV_LoungeU, currentPage, false);

								if (!saveOnly && noneSelected)
								{
									radQ21_HRCV_LoungeA.MessageManager.ErrorMessage = "Please select one of the following options.";
									retVal = false;
								}
							}
							if (PropertyShortCode == GCCPropertyShortCode.EC && Q21.SelectedValue == 1)
							{
								bool noneSelected = !GetValue(radQ21_EC_LoungeM, currentPage, false) &&
													!GetValue(radQ21_EC_LoungeE, currentPage, false);

								if (!saveOnly && noneSelected)
								{
									radQ21_EC_LoungeM.MessageManager.ErrorMessage = "Please select one of the following options.";
									retVal = false;
								}
							}
						}
						if (currentPage)
						{
							SaveValue(Q21);
							SaveValue(txtShowVisitDate);
							if (PropertyShortCode == GCCPropertyShortCode.HRCV)
							{
								SurveyTools.SaveRadioButtons(radQ21_HRCV_LoungeA, radQ21_HRCV_LoungeU);
							}
							if (PropertyShortCode == GCCPropertyShortCode.EC)
							{
								SurveyTools.SaveRadioButtons(radQ21_EC_LoungeM, radQ21_EC_LoungeE);
							}
						}
				   

					}

					else if(strSurveyLang =="French")
					{

						if (!saveOnly)
						{
							if (PropertyShortCode == GCCPropertyShortCode.RR
								|| PropertyShortCode == GCCPropertyShortCode.HRCV
								|| PropertyShortCode == GCCPropertyShortCode.VRL
								|| PropertyShortCode == GCCPropertyShortCode.CCH
								|| PropertyShortCode == GCCPropertyShortCode.CMR
								|| PropertyShortCode == GCCPropertyShortCode.CDC
								|| PropertyShortCode == GCCPropertyShortCode.CNSH
								|| PropertyShortCode == GCCPropertyShortCode.EC
								|| PropertyShortCode == GCCPropertyShortCode.SCTI
								|| PropertyShortCode == GCCPropertyShortCode.CNB
								|| PropertyShortCode == GCCPropertyShortCode.SCBE)
							{
								//if ( !CheckForAnswer( Q21 )
								//    | ( Q21.SelectedValue == 1 && ( String.IsNullOrEmpty( txtShowVisitDate.GetValue() ) || txtShowVisitDate.GetValue().Trim().Length == 0 ) ) ) {
								//    mmQ21A.ErrorMessage = String.Format( "Please select the date you visited {0}.", GetShowLoungeName() );
								//    retVal = false;
								//}
							}
						   
							if (PropertyShortCode == GCCPropertyShortCode.HRCV && Q21_F.SelectedValue_F == 1)
							{
								bool noneSelected = !GetValue(radQ21_HRCV_LoungeA_F, currentPage, false) &&
													!GetValue(radQ21_HRCV_LoungeU_F, currentPage, false);

								if (!saveOnly && noneSelected)
								{
									radQ21_HRCV_LoungeA_F.MessageManager.ErrorMessage = "Veuillez sélectionner l'une des options suivantes.";
									retVal = false;
								}
							}
							if (PropertyShortCode == GCCPropertyShortCode.EC && Q21_F.SelectedValue_F == 1)
							{
								bool noneSelected = !GetValue(radQ21_EC_LoungeM_F, currentPage, false) &&
													!GetValue(radQ21_EC_LoungeE_F, currentPage, false);

								if (!saveOnly && noneSelected)
								{
									radQ21_EC_LoungeM_F.MessageManager.ErrorMessage = "Veuillez sélectionner l'une des options suivantes.";
									retVal = false;
								}
							}
						}
						if (currentPage)
						{
							SaveValue(Q21_F);
							SaveValue(txtShowVisitDate);
							if (PropertyShortCode == GCCPropertyShortCode.HRCV)
							{
								SurveyTools.SaveRadioButtons(radQ21_HRCV_LoungeA_F, radQ21_HRCV_LoungeU_F);
							}
							if (PropertyShortCode == GCCPropertyShortCode.EC)
							{
								SurveyTools.SaveRadioButtons(radQ21_EC_LoungeM_F, radQ21_EC_LoungeE_F);
							}
						}
				   

					}




					break;
					#endregion Page 11
				case 12: //Q22-23
					#region Page 12

					if(strSurveyLang != "French")
					{

						if (Q21.SelectedValue == 1)
						{
							if (!saveOnly)
							{
								if (!CheckForAnswer(Q22)
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q23A))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q23B))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q23C))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q23D)))
								{
									retVal = false;
								}
							}
							if (currentPage)
							{
								SaveValue(Q22);
								SaveValue(Q23A);
								SaveValue(Q23B);
								SaveValue(Q23C);
								SaveValue(Q23D);
							}
						}
				  
					}

					else if(strSurveyLang == "French")
					{
						if (Q21_F.SelectedValue_F == 1)
						{
							if (!saveOnly)
							{
								if (!CheckForAnswer(Q22_F)
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q23A_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q23B_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q23C_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q23D_F)))
								{
									retVal = false;
								}
							}
							if (currentPage)
							{
								SaveValue(Q22_F);
								SaveValue(Q23A_F);
								SaveValue(Q23B_F);
								SaveValue(Q23C_F);
								SaveValue(Q23D_F);
							}
						}
				  

					}


					break;
					#endregion Page 12
				case 13: //24
					#region Page 13
					
					if(strSurveyLang != "French")
					{
						if (PropertyShortCode == GCCPropertyShortCode.RR
												|| PropertyShortCode == GCCPropertyShortCode.HRCV
												|| PropertyShortCode == GCCPropertyShortCode.CNSH
												|| PropertyShortCode == GCCPropertyShortCode.CNB)
						{
							if (!saveOnly)
							{
								//if ( !CheckForAnswer( Q24 )
								//    | ( Q24.SelectedValue == 1 && ( String.IsNullOrEmpty( Q24_VisitDate.GetValue() ) || Q24_VisitDate.GetValue().Trim().Length == 0 ) ) ) {
								//    mmQ24A.ErrorMessage = String.Format( "Please select the date you visited the {0} Show Theatre.", CasinoName );
								//    retVal = false;
								//}
							}
							if (currentPage)
							{
								SaveValue(Q24);
								SaveValue(Q24_VisitDate);
							}
						}

					}
					else if(strSurveyLang == "French")
					{

						if (PropertyShortCode == GCCPropertyShortCode.RR
												|| PropertyShortCode == GCCPropertyShortCode.HRCV
												|| PropertyShortCode == GCCPropertyShortCode.CNSH
												|| PropertyShortCode == GCCPropertyShortCode.CNB)
						{
							if (!saveOnly)
							{
								//if ( !CheckForAnswer( Q24 )
								//    | ( Q24.SelectedValue == 1 && ( String.IsNullOrEmpty( Q24_VisitDate.GetValue() ) || Q24_VisitDate.GetValue().Trim().Length == 0 ) ) ) {
								//    mmQ24A.ErrorMessage = String.Format( "Please select the date you visited the {0} Show Theatre.", CasinoName );
								//    retVal = false;
								//}
							}
							if (currentPage)
							{
								SaveValue(Q24_F);
								SaveValue(Q24_VisitDate_F);
							}
						}

					}



					break;
					#endregion Page 13
				case 14: //Q25-26
					#region Page 14
				  

					if(strSurveyLang != "French")
					{
						if (Q24.SelectedValue == 1
					  && (PropertyShortCode == GCCPropertyShortCode.RR
						  || PropertyShortCode == GCCPropertyShortCode.HRCV
						  || PropertyShortCode == GCCPropertyShortCode.CNSH
						  || PropertyShortCode == GCCPropertyShortCode.CNB))
						{
							if (!saveOnly)
							{
								if (!CheckForAnswer(Q25)
										| (!IsKioskOrStaffEntry && !CheckForAnswer(Q26A))
										| (!IsKioskOrStaffEntry && !CheckForAnswer(Q26B))
										| (!IsKioskOrStaffEntry && !CheckForAnswer(Q26C))
										| (!IsKioskOrStaffEntry && !CheckForAnswer(Q26D))
										| (!IsKioskOrStaffEntry && !CheckForAnswer(Q26E)))
								{
									retVal = false;
								}
							}
							if (currentPage)
							{
								SaveValue(Q25);
								SaveValue(Q26A);
								SaveValue(Q26B);
								SaveValue(Q26C);
								SaveValue(Q26D);
								SaveValue(Q26E);
							}
						}

					}

					else if(strSurveyLang == "French")
					{

						if (Q24_F.SelectedValue_F == 1
					  && (PropertyShortCode == GCCPropertyShortCode.RR
						  || PropertyShortCode == GCCPropertyShortCode.HRCV
						  || PropertyShortCode == GCCPropertyShortCode.CNSH
						  || PropertyShortCode == GCCPropertyShortCode.CNB))
						{
							if (!saveOnly)
							{
								if (!CheckForAnswer(Q25_F)
										| (!IsKioskOrStaffEntry && !CheckForAnswer(Q26A_F))
										| (!IsKioskOrStaffEntry && !CheckForAnswer(Q26B_F))
										| (!IsKioskOrStaffEntry && !CheckForAnswer(Q26C_F))
										| (!IsKioskOrStaffEntry && !CheckForAnswer(Q26D_F))
										| (!IsKioskOrStaffEntry && !CheckForAnswer(Q26E_F)))
								{
									retVal = false;
								}
							}
							if (currentPage)
							{
								SaveValue(Q25_F);
								SaveValue(Q26A_F);
								SaveValue(Q26B_F);
								SaveValue(Q26C_F);
								SaveValue(Q26D_F);
								SaveValue(Q26E_F);
							}
						}



					}


					break;
					#endregion Page 14
				case 15: //Q27
					#region Page 15
				  
					if(strSurveyLang != "French")
					{
						if (!saveOnly)
						{
							if (!CheckForAnswer(Q27))
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							SaveValue(Q27);
						}

					}

					else if(strSurveyLang == "French")
					{


						if (!saveOnly)
						{
							if (!CheckForAnswer(Q27_F))
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							SaveValue(Q27_F);
						}
					}


					break;
					#endregion Page 15
				case 16: //Q27A-Q29
					#region Page 16


					if(strSurveyLang != "French")
					{
						if (Q27.SelectedValue == 1)
						{
							if (!saveOnly)
							{
								bool noneSelected = !IsKioskOrStaffEntry &&
													!GetValue(radQ27A_1, currentPage, false) &&
													!GetValue(radQ27A_2, currentPage, false) &&
													!GetValue(radQ27A_3, currentPage, false) &&
													!GetValue(radQ27A_4, currentPage, false) &&
													!GetValue(radQ27A_5, currentPage, false) &&
													!GetValue(radQ27A_6, currentPage, false) &&
													!GetValue(radQ27A_7, currentPage, false) &&
													!GetValue(radQ27A_8, currentPage, false) &&
													!GetValue(radQ27A_9, currentPage, false) &&
													!GetValue(radQ27A_10, currentPage, false) &&
													!GetValue(radQ27A_11, currentPage, false) &&
													!GetValue(radQ27A_12, currentPage, false) &&
													!GetValue(radQ27A_13, currentPage, false);

								if (noneSelected)
								{
									mmQ27A.ErrorMessage = "Please select one of the following options.";
									retVal = false;
								}

								if (!CheckForAnswer(txtQ27B)
									| !CheckForAnswer(Q28)
									| !CheckForAnswer(Q29))
								{
									retVal = false;
								}
							}
							if (currentPage)
							{
								SaveValue(radQ27A_1);
								SaveValue(radQ27A_2);
								SaveValue(radQ27A_3);
								SaveValue(radQ27A_4);
								SaveValue(radQ27A_5);
								SaveValue(radQ27A_6);
								SaveValue(radQ27A_7);
								SaveValue(radQ27A_8);
								SaveValue(radQ27A_9);
								SaveValue(radQ27A_10);
								SaveValue(radQ27A_11);
								SaveValue(radQ27A_12);
								SaveValue(radQ27A_13);
								SaveValue(txtQ27A_OtherExplanation);
								SaveValue(txtQ27B);
								SaveValue(Q28);
								SaveValue(Q29);
							}
						}
				   

					}
					else if(strSurveyLang == "French")
					{

						if (Q27_F.SelectedValue_F == 1)
						{
							if (!saveOnly)
							{
								bool noneSelected = !IsKioskOrStaffEntry &&
													!GetValue(radQ27A_1_F, currentPage, false) &&
													!GetValue(radQ27A_2_F, currentPage, false) &&
													!GetValue(radQ27A_3_F, currentPage, false) &&
													!GetValue(radQ27A_4_F, currentPage, false) &&
													!GetValue(radQ27A_5_F, currentPage, false) &&
													!GetValue(radQ27A_6_F, currentPage, false) &&
													!GetValue(radQ27A_7_F, currentPage, false) &&
													!GetValue(radQ27A_8_F, currentPage, false) &&
													!GetValue(radQ27A_9_F, currentPage, false) &&
													!GetValue(radQ27A_10_F, currentPage, false) &&
													!GetValue(radQ27A_11_F, currentPage, false) &&
													!GetValue(radQ27A_12_F, currentPage, false) &&
													!GetValue(radQ27A_13_F, currentPage, false);

								if (noneSelected)
								{
									mmQ27A_F.ErrorMessage = "Veuillez sélectionner l'une des options suivantes.";
									retVal = false;
								}

								if (!CheckForAnswer(txtQ27B_F)
									| !CheckForAnswer(Q28_F)
									| !CheckForAnswer(Q29_F))
								{
									retVal = false;
								}
							}
							if (currentPage)
							{
								SaveValue(radQ27A_1_F);
								SaveValue(radQ27A_2_F);
								SaveValue(radQ27A_3_F);
								SaveValue(radQ27A_4_F);
								SaveValue(radQ27A_5_F);
								SaveValue(radQ27A_6_F);
								SaveValue(radQ27A_7_F);
								SaveValue(radQ27A_8_F);
								SaveValue(radQ27A_9_F);
								SaveValue(radQ27A_10_F);
								SaveValue(radQ27A_11_F);
								SaveValue(radQ27A_12_F);
								SaveValue(radQ27A_13_F);
								SaveValue(txtQ27A_OtherExplanation_F);
								SaveValue(txtQ27B_F);
								SaveValue(Q28_F);
								SaveValue(Q29_F);
							}
						}
				   

					}



					break;
					#endregion Page 16
				case 17: //Q30-32
					#region Page 17

					if(strSurveyLang != "French")
					{

						if (Q29.SelectedValue == 1)
						{
							if (!saveOnly)
							{
								if (!CheckForAnswer(Q30)
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q31A))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q31B))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q31C))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q31D))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q31E)))
								{
									retVal = false;
								}
							}
							if (currentPage)
							{
								SaveValue(Q30);
								SaveValue(Q31A);
								SaveValue(Q31B);
								SaveValue(Q31C);
								SaveValue(Q31D);
								SaveValue(Q31E);
								SaveValue(Q32);
							}
						}
				  
					}
					else if(strSurveyLang == "French")
					{

						if (Q29_F.SelectedValue_F == 1)
						{
							if (!saveOnly)
							{
								if (!CheckForAnswer(Q30_F)
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q31A_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q31B_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q31C_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q31D_F))
									| (!IsKioskOrStaffEntry && !CheckForAnswer(Q31E_F)))
								{
									retVal = false;
								}
							}
							if (currentPage)
							{
								SaveValue(Q30_F);
								SaveValue(Q31A_F);
								SaveValue(Q31B_F);
								SaveValue(Q31C_F);
								SaveValue(Q31D_F);
								SaveValue(Q31E_F);
								SaveValue(Q32_F);
							}
						}
				  
					}

					break;
					#endregion Page 17
				case 18: //Q33
					#region Page 18

					if(strSurveyLang != "French")
					{
						if (!IsKioskOrStaffEntry
					&& (Q29.SelectedValue == 0
						|| Q30.SelectedValue == 1 || Q30.SelectedValue == 2
						|| Q31A.SelectedValue == 1 || Q31A.SelectedValue == 2
						|| Q31B.SelectedValue == 1 || Q31B.SelectedValue == 2
						|| Q31C.SelectedValue == 1 || Q31C.SelectedValue == 2
						|| Q31D.SelectedValue == 1 || Q31D.SelectedValue == 2
						|| Q31E.SelectedValue == 1 || Q31E.SelectedValue == 2))
						{
							if (!saveOnly)
							{
								CheckForAnswer(Q33);
							}
							if (currentPage)
							{
								SaveValue(Q33);
							}
						}
				

					}
					else if(strSurveyLang == "French")
					{
						if (!IsKioskOrStaffEntry
					&& (Q29_F.SelectedValue_F == 0
						|| Q30_F.SelectedValue_F== 1 || Q30_F.SelectedValue_F == 2
						|| Q31A_F.SelectedValue == 1 || Q31A_F.SelectedValue == 2
						|| Q31B_F.SelectedValue == 1 || Q31B_F.SelectedValue == 2
						|| Q31C_F.SelectedValue == 1 || Q31C_F.SelectedValue == 2
						|| Q31D_F.SelectedValue == 1 || Q31D_F.SelectedValue == 2
						|| Q31E_F.SelectedValue == 1 || Q31E_F.SelectedValue == 2))
						{
							if (!saveOnly)
							{
								CheckForAnswer(Q33_F);
							}
							if (currentPage)
							{
								SaveValue(Q33_F);
							}
						}
				

					}




					break;
					#endregion Page 18
				case 19: //Q34-35
					#region Page 19

					if (strSurveyLang != "French")
					{

						if (currentPage)
						{
							SaveValue(Q34);
							SaveValue(Q35);
						}


					}

					else if(strSurveyLang == "French")
					{
						if (currentPage)
						{
							SaveValue(Q34_F);
							SaveValue(Q35_F);
						}


					}

					break;
					#endregion Page 19


				case 20: //Q36-40
					#region Page 20



					if(strSurveyLang != "French")
					{

						if (!saveOnly)
						{
							bool genderNotSelected = !GetValue(Q36Male, currentPage, false) &&
													 !GetValue(Q36Female, currentPage, false);
							//if ( genderNotSelected ) {
							//    Q36Message.ErrorMessage = "Please answer the following question.";
							//    retVal = false;
							//}

							bool ageGroupNotSelected = !GetValue(Q37_1, currentPage, false) &&
														!GetValue(Q37_2, currentPage, false) &&
														!GetValue(Q37_3, currentPage, false) &&
														!GetValue(Q37_4, currentPage, false) &&
														!GetValue(Q37_5, currentPage, false) &&
														!GetValue(Q37_6, currentPage, false);
							//if ( ageGroupNotSelected ) {
							//    Q37Message.ErrorMessage = "Please answer the following question.";
							//    retVal = false;
							//}

							bool visitNotSelected = !GetValue(Q38_1, currentPage, false) &&
													!GetValue(Q38_2, currentPage, false) &&
													!GetValue(Q38_3, currentPage, false) &&
													!GetValue(Q38_4, currentPage, false) &&
													!GetValue(Q38_5, currentPage, false) &&
													!GetValue(Q38_6, currentPage, false);
							//if ( visitNotSelected ) {
							//    Q38Message.ErrorMessage = "Please answer the following question.";
							//    retVal = false;
							//}

							if (Q39_16.Checked && String.IsNullOrEmpty(Q39_16Explanation.Text))
							{
								Q39_16ExpMessage.ErrorMessage = "Please enter the other language(s) that you regularly speak at home.";
								retVal = false;
							}
							if (Q33.SelectedValue != 1 && !CheckForAnswer(Q40))
							{
								retVal = false;
							}
							if (!CheckForAnswer(OLGYesNo) && (PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.WDB || PropertyShortCode == GCCPropertyShortCode.GBH || PropertyShortCode == GCCPropertyShortCode.SSKD || PropertyShortCode == GCCPropertyShortCode.AJA))
							{
								retVal = false;
							}
							else
							{
								retVal = true;
							}
						}
						if (currentPage)
						{
							SurveyTools.SaveRadioButtons(Q36Male, Q36Female);
							SurveyTools.SaveRadioButtons(Q37_1, Q37_2, Q37_3, Q37_4, Q37_5, Q37_6);
							SurveyTools.SaveRadioButtons(Q38_1, Q38_2, Q38_3, Q38_4, Q38_5, Q38_6);
							SaveValue(Q39_1);
							SaveValue(Q39_2);
							SaveValue(Q39_3);
							SaveValue(Q39_4);
							SaveValue(Q39_5);
							SaveValue(Q39_6);
							SaveValue(Q39_7);
							SaveValue(Q39_8);
							SaveValue(Q39_9);
							SaveValue(Q39_10);
							SaveValue(Q39_11);
							SaveValue(Q39_12);
							SaveValue(Q39_13);
							SaveValue(Q39_14);
							SaveValue(Q39_15);
							SaveValue(Q39_16);
							SaveValue(Q39_16Explanation);
							if (Q33.SelectedValue != 1)
							{
								SaveValue(Q40);
							}
							SaveValue(OLGYesNo);
						}
				   



					}

					else if(strSurveyLang == "French")
					{


						if (!saveOnly)
						{
							bool genderNotSelected = !GetValue(Q36Male_F, currentPage, false) &&
													 !GetValue(Q36Female_F, currentPage, false);
							//if ( genderNotSelected ) {
							//    Q36Message.ErrorMessage = "Please answer the following question.";
							//    retVal = false;
							//}

							bool ageGroupNotSelected = !GetValue(Q37_1_F, currentPage, false) &&
														!GetValue(Q37_2_F, currentPage, false) &&
														!GetValue(Q37_3_F, currentPage, false) &&
														!GetValue(Q37_4_F, currentPage, false) &&
														!GetValue(Q37_5_F, currentPage, false) &&
														!GetValue(Q37_6_F, currentPage, false);
							//if ( ageGroupNotSelected ) {
							//    Q37Message.ErrorMessage = "Please answer the following question.";
							//    retVal = false;
							//}

							bool visitNotSelected = !GetValue(Q38_1_F, currentPage, false) &&
													!GetValue(Q38_2_F, currentPage, false) &&
													!GetValue(Q38_3_F, currentPage, false) &&
													!GetValue(Q38_4_F, currentPage, false) &&
													!GetValue(Q38_5_F, currentPage, false) &&
													!GetValue(Q38_6_F, currentPage, false);
							//if ( visitNotSelected ) {
							//    Q38Message.ErrorMessage = "Please answer the following question.";
							//    retVal = false;
							//}

							if (Q39_16_F.Checked && String.IsNullOrEmpty(Q39_16Explanation_F.Text))
							{
								Q39_16ExpMessage.ErrorMessage = "Veuillez entrer l'autre langue que vous parlez régulièrement à la maison.";
								retVal = false;
							}
							if (Q33_F.SelectedValue_F != 1 && !CheckForAnswer(Q40_F))
							{
								retVal = false;
							}
							if (!CheckForAnswer(OLGYesNo_F) && (PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.SSKD || PropertyShortCode == GCCPropertyShortCode.AJA || PropertyShortCode == GCCPropertyShortCode.WDB || PropertyShortCode == GCCPropertyShortCode.GBH))
							{
								retVal = false;
							}
							else
							{
								retVal = true;
							}
						}
						if (currentPage)
						{
							SurveyTools.SaveRadioButtons(Q36Male_F, Q36Female_F);
							SurveyTools.SaveRadioButtons(Q37_1_F, Q37_2_F, Q37_3_F, Q37_4_F, Q37_5_F, Q37_6_F);
							SurveyTools.SaveRadioButtons(Q38_1_F, Q38_2_F, Q38_3_F, Q38_4_F, Q38_5_F, Q38_6_F);
							SaveValue(Q39_1_F);
							SaveValue(Q39_2_F);
							SaveValue(Q39_3_F);
							SaveValue(Q39_4_F);
							SaveValue(Q39_5_F);
							SaveValue(Q39_6_F);
							SaveValue(Q39_7_F);
							SaveValue(Q39_8_F);
							SaveValue(Q39_9_F);
							SaveValue(Q39_10_F);
							SaveValue(Q39_11_F);
							SaveValue(Q39_12_F);
							SaveValue(Q39_13_F);
							SaveValue(Q39_14_F);
							SaveValue(Q39_15_F);
							SaveValue(Q39_16_F);
							SaveValue(Q39_16Explanation_F);
							if (Q33_F.SelectedValue_F != 1)
							{
								SaveValue(Q40_F);
							}
							SaveValue(OLGYesNo_F);
						}
				   




					}



					break;
					#endregion Page 20
				case 21: //Contact info
					#region Page 21
				   

					if(strSurveyLang != "French")
					{

						if (Q33.SelectedValue == 1 || Q40.SelectedValue == 1)
						{
							if (!saveOnly)
							{
								bool noneSelected = !IsKioskOrStaffEntry &&
													!GetValue(radQ40A_1, currentPage, false) &&
													!GetValue(radQ40A_2, currentPage, false) &&
													!GetValue(radQ40A_3, currentPage, false) &&
													!GetValue(radQ40A_4, currentPage, false) &&
													!GetValue(radQ40A_5, currentPage, false) &&
													!GetValue(radQ40A_6, currentPage, false) &&
													!GetValue(radQ40A_7, currentPage, false) &&
													!GetValue(radQ40A_8, currentPage, false) &&
													!GetValue(radQ40A_9, currentPage, false) &&
													!GetValue(radQ40A_10, currentPage, false) &&
													!GetValue(radQ40A_11, currentPage, false) &&
													!GetValue(radQ40A_12, currentPage, false) &&
													!GetValue(radQ40A_13, currentPage, false);

								if (noneSelected)
								{
									Q40Message.ErrorMessage = "Please select one of the following options.";
									retVal = false;
								}

								if (!CheckForAnswer(txtFirstName)
									| !CheckForAnswer(txtLastName)
									| (IsKioskOrStaffEntry && !CheckForAnswer(txtEmail2))
									)
								{
									retVal = false;
								}
								if (IsKioskOrStaffEntry)
								{
									string email = GetValue(txtEmail2, currentPage, String.Empty);
									if (!Validation.RegExCheck(email, ValidationType.Email))
									{
										txtEmail2.MessageManager.ErrorMessage = "Please enter a valid email address.";
										retVal = false;
									}
								}
							}
							if (currentPage)
							{
								SaveValue(radQ40A_1);
								SaveValue(radQ40A_2);
								SaveValue(radQ40A_3);
								SaveValue(radQ40A_4);
								SaveValue(radQ40A_5);
								SaveValue(radQ40A_6);
								SaveValue(radQ40A_7);
								SaveValue(radQ40A_8);
								SaveValue(radQ40A_9);
								SaveValue(radQ40A_10);
								SaveValue(radQ40A_11);
								SaveValue(radQ40A_12);
								SaveValue(radQ40A_13);
								SaveValue(txtQ40OtherExplanation);
								SaveValue(txtFirstName);
								SaveValue(txtLastName);
								if (IsKioskOrStaffEntry)
								{
									SaveValue(txtEmail2);
								}
								SaveValue(txtTelephoneNumber);
							}
						}
					}

					else if (strSurveyLang == "French")
					{

						if (Q33_F.SelectedValue_F == 1 || Q40_F.SelectedValue_F == 1)
						{
							if (!saveOnly)
							{
								bool noneSelected = !IsKioskOrStaffEntry &&
													!GetValue(radQ40A_1_F, currentPage, false) &&
													!GetValue(radQ40A_2_F, currentPage, false) &&
													!GetValue(radQ40A_3_F, currentPage, false) &&
													!GetValue(radQ40A_4_F, currentPage, false) &&
													!GetValue(radQ40A_5_F, currentPage, false) &&
													!GetValue(radQ40A_6_F, currentPage, false) &&
													!GetValue(radQ40A_7_F, currentPage, false) &&
													!GetValue(radQ40A_8_F, currentPage, false) &&
													!GetValue(radQ40A_9_F, currentPage, false) &&
													!GetValue(radQ40A_10_F, currentPage, false) &&
													!GetValue(radQ40A_11_F, currentPage, false) &&
													!GetValue(radQ40A_12_F, currentPage, false) &&
													!GetValue(radQ40A_13_F, currentPage, false);

								if (noneSelected)
								{
									Q40Message.ErrorMessage = "Veuillez sélectionner l'une des options suivantes.";
									retVal = false;
								}

								if (!CheckForAnswer(txtFirstName_F)
									| !CheckForAnswer(txtLastName_F)
									| (IsKioskOrStaffEntry && !CheckForAnswer(txtEmail2_F))
									)
								{
									retVal = false;
								}
								if (IsKioskOrStaffEntry)
								{
									string email = GetValue(txtEmail2_F, currentPage, String.Empty);
									if (!Validation.RegExCheck(email, ValidationType.Email))
									{
										txtEmail2_F.MessageManager.ErrorMessage = "S'il vous plaît, mettez une adresse courriel valide.";
										retVal = false;
									}
								}
							}
							if (currentPage)
							{
								SaveValue(radQ40A_1_F);
								SaveValue(radQ40A_2_F);
								SaveValue(radQ40A_3_F);
								SaveValue(radQ40A_4_F);
								SaveValue(radQ40A_5_F);
								SaveValue(radQ40A_6_F);
								SaveValue(radQ40A_7_F);
								SaveValue(radQ40A_8_F);
								SaveValue(radQ40A_9_F);
								SaveValue(radQ40A_10_F);
								SaveValue(radQ40A_11_F);
								SaveValue(radQ40A_12_F);
								SaveValue(radQ40A_13_F);
								SaveValue(txtQ40OtherExplanation_F);
								SaveValue(txtFirstName_F);
								SaveValue(txtLastName_F);
								if (IsKioskOrStaffEntry)
								{
									SaveValue(txtEmail2_F);
								}
								SaveValue(txtTelephoneNumber_F);
							}
						}

					}



					break;
					#endregion Page 21
				case 22: //OLG questions
					#region Page 22
					// TODO: add if construct to evaluate the newly added question answers and call SaveValue for each
				 
					if(strSurveyLang != "French")
					{

						if (!saveOnly)
						{
							if (!CheckForAnswer(OLGQ1) ||
								!CheckForAnswer(OLGQ2) ||
								!CheckForAnswer(OLGQ3) ||
								!CheckForAnswer(OLGQ4) ||
								!CheckForAnswer(OLGQ5) ||
								!CheckForAnswer(OLGQ6) ||
								!CheckForAnswer(OLGQ7))
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							SaveValue(OLGQ1);
							SaveValue(OLGQ2);
							SaveValue(OLGQ3);
							SaveValue(OLGQ4);
							SaveValue(OLGQ5);
							SaveValue(OLGQ6);
							SaveValue(OLGQ7);
						}
					}
					else if(strSurveyLang == "French")
					{

						if (!saveOnly)
						{
							if (!CheckForAnswer(OLGQ1_F) ||
								!CheckForAnswer(OLGQ2_F) ||
								!CheckForAnswer(OLGQ3_F) ||
								!CheckForAnswer(OLGQ4_F) ||
								!CheckForAnswer(OLGQ5_F) ||
								!CheckForAnswer(OLGQ6_F) ||
								!CheckForAnswer(OLGQ7_F))
							{
								retVal = false;
							}
						}
						if (currentPage)
						{
							SaveValue(OLGQ1_F);
							SaveValue(OLGQ2_F);
							SaveValue(OLGQ3_F);
							SaveValue(OLGQ4_F);
							SaveValue(OLGQ5_F);
							SaveValue(OLGQ6_F);
							SaveValue(OLGQ7_F);
						}
					}



					break;
				#endregion
			}
			return retVal;
		}

		protected T GetValue<T>( ISurveyControl<T> control, bool getCurrentValue, T defaultValue ) {
			return SurveyTools.GetValue( control, getCurrentValue, defaultValue );
		}
		protected void SaveValue<T>( ISurveyControl<T> control ) {
			SurveyTools.SaveValue( control );
		}

		protected bool CheckForAnswer( ISurveyControl<int> control ) {
			return SurveyTools.CheckForAnswer( control, QuestionsAreMandatory );
		}
		protected bool CheckForAnswer( ISurveyControl<string> control ) {
			return SurveyTools.CheckForAnswer( control, QuestionsAreMandatory );
		}

		protected string GetSurveyURL( int page ) {
			return GetSurveyURL( page, 1 );
		}
		protected string GetSurveyURL( int page, int redirDir ) {
			string stVal = GetSurveyTypeLetter();
			bool isReset = (page == -1);
			if (isReset) {
				page = 1;
			}
			if ( SurveyType == GEISurveyType.Email ) {
				return String.Format( "/SE/{0}/{1}/{2}{3}{4}", PropertyShortCode.ToString(), EmailPIN.ToString(), page, ( redirDir == -1 ? "/-1" : String.Empty ), ( isReset ? "?r=1" : String.Empty ) );
			} else {
				return String.Format( "/Survey{0}/{1}/{2}{3}{4}", stVal, PropertyShortCode.ToString(), page, ( redirDir == -1 ? "/-1" : String.Empty ), ( isReset ? "?r=1" : String.Empty ) );
			}
		}

		protected string GetSurveyTypeLetter() {
			string stVal = String.Empty;
			switch ( SurveyType ) {
				case GEISurveyType.Email:
					stVal = "E";
					break;
				case GEISurveyType.Kiosk:
					stVal = "K";
					break;
				case GEISurveyType.StaffSurvey:
					stVal = "S";
					break;
			}
			return stVal;
		}
		protected int GetSurveyTypeNumber() {
			int stVal = 0;
			switch ( SurveyType ) {
				case GEISurveyType.DirectAccess:
					stVal = 1;
					break;
				case GEISurveyType.Email:
					stVal = 2;
					break;
				case GEISurveyType.Kiosk:
					stVal = 3;
					break;
				case GEISurveyType.StaffSurvey:
					stVal = 4;
					break;
			}
			return stVal;
		}

		protected bool SaveData(out int rowID) {


			StringBuilder columnList = new StringBuilder();
			SQLParamList sqlParams = new SQLParamList();

#region inseting data into database


			if(strSurveyLang != "French"){

 
			if ( !IsKioskOrStaffEntry ) {
				txtEmail.PrepareQuestionForDB( columnList, sqlParams );
			} else {
				txtEmail2.PrepareQuestionForDB( columnList, sqlParams );
			}

			//Add details for email surveys
			if ( SurveyType == GEISurveyType.Email ) {
				columnList.Append( ",[EmailBatch],[PIN]" );
				sqlParams.Add( "@EmailBatch", EmailPINRow["BatchID"] )
						 .Add( "@PIN", EmailPIN );
				if ( String.IsNullOrWhiteSpace( txtQ4_CardNumber.Text ) ) {
					//If they erased the encore number, we'll put in a new one

					if (strSurveyLang != "French")
					{
						Q4.SelectedValue = 1;
						txtQ4_CardNumber.Text = EmailPINRow["Encore"].ToString();
					}
					else
					{
						Q4_F.SelectedValue_F = 1;
						txtQ4_CardNumber_F.Text = EmailPINRow["Encore"].ToString();
					}
				}





			}

			radQ1_Slots.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Tables.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Poker.PrepareQuestionForDB( columnList, sqlParams );           
			radQ1_Food.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Entertainment.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Hotel.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_LiveRacing.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Racebook.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Bingo.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Lottery.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_None.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Slots.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Tables.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Poker.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Food.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Entertainment.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Hotel.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_LiveRacing.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Racebook.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Bingo.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Lottery.PrepareQuestionForDB( columnList, sqlParams );
			radQ3_Everett.PrepareQuestionForDB( columnList, sqlParams );
			radQ3_Lakewood.PrepareQuestionForDB( columnList, sqlParams );
			radQ3_Tukwila.PrepareQuestionForDB( columnList, sqlParams );
			radQ3_DeMoines.PrepareQuestionForDB( columnList, sqlParams );
			Q4.PrepareQuestionForDB( columnList, sqlParams );
			txtQ4_CardNumber.PrepareQuestionForDB( columnList, sqlParams );
			Q5A.PrepareQuestionForDB( columnList, sqlParams );
			Q5B.PrepareQuestionForDB( columnList, sqlParams );
			Q6A.PrepareQuestionForDB( columnList, sqlParams );
			Q6B.PrepareQuestionForDB( columnList, sqlParams );
			Q6C.PrepareQuestionForDB( columnList, sqlParams );
			Q6D.PrepareQuestionForDB( columnList, sqlParams );
			Q7A.PrepareQuestionForDB( columnList, sqlParams );
			Q7B.PrepareQuestionForDB( columnList, sqlParams );
			Q7C.PrepareQuestionForDB( columnList, sqlParams );
			Q7D.PrepareQuestionForDB( columnList, sqlParams );
			Q7E.PrepareQuestionForDB( columnList, sqlParams );
			Q7F.PrepareQuestionForDB( columnList, sqlParams );
			Q8.PrepareQuestionForDB( columnList, sqlParams );
			Q9A.PrepareQuestionForDB( columnList, sqlParams );
			Q9B.PrepareQuestionForDB( columnList, sqlParams );
			Q9C.PrepareQuestionForDB( columnList, sqlParams );
			Q9D.PrepareQuestionForDB( columnList, sqlParams );
			Q9E.PrepareQuestionForDB( columnList, sqlParams );
			Q9F.PrepareQuestionForDB( columnList, sqlParams );
			Q9G.PrepareQuestionForDB( columnList, sqlParams );
			Q9H.PrepareQuestionForDB( columnList, sqlParams );
			Q9I.PrepareQuestionForDB( columnList, sqlParams );
			Q9J.PrepareQuestionForDB( columnList, sqlParams );
			Q9K.PrepareQuestionForDB(columnList, sqlParams);
			Q10A.PrepareQuestionForDB( columnList, sqlParams );
			Q10B.PrepareQuestionForDB( columnList, sqlParams );
			Q10C.PrepareQuestionForDB( columnList, sqlParams );
			Q11.PrepareQuestionForDB( columnList, sqlParams );
			Q12.PrepareQuestionForDB( columnList, sqlParams );
			Q13A.PrepareQuestionForDB( columnList, sqlParams );
			Q13B.PrepareQuestionForDB( columnList, sqlParams );
			Q13C.PrepareQuestionForDB( columnList, sqlParams );
			Q13D.PrepareQuestionForDB( columnList, sqlParams );
			Q13E.PrepareQuestionForDB( columnList, sqlParams );
			Q13F.PrepareQuestionForDB( columnList, sqlParams );
			Q13G.PrepareQuestionForDB( columnList, sqlParams );
			Q14.PrepareQuestionForDB( columnList, sqlParams );
			Q15A.PrepareQuestionForDB( columnList, sqlParams );
			Q15B.PrepareQuestionForDB( columnList, sqlParams );
			Q15C.PrepareQuestionForDB( columnList, sqlParams );
			Q15D.PrepareQuestionForDB( columnList, sqlParams );
			Q15E.PrepareQuestionForDB( columnList, sqlParams );
			Q15F.PrepareQuestionForDB( columnList, sqlParams );
			Q16A.PrepareQuestionForDB( columnList, sqlParams );
			Q16B.PrepareQuestionForDB( columnList, sqlParams );
			Q16C.PrepareQuestionForDB( columnList, sqlParams );
			Q16D.PrepareQuestionForDB( columnList, sqlParams );
			Q17.PrepareQuestionForDB( columnList, sqlParams );
			Q18_1.PrepareQuestionForDB( columnList, sqlParams );
			Q18_2.PrepareQuestionForDB( columnList, sqlParams );
			Q18_3.PrepareQuestionForDB( columnList, sqlParams );
			Q18_4.PrepareQuestionForDB( columnList, sqlParams );
			Q18_5.PrepareQuestionForDB( columnList, sqlParams );
			Q18_6.PrepareQuestionForDB( columnList, sqlParams );
			Q18_7.PrepareQuestionForDB( columnList, sqlParams );
			Q18_8.PrepareQuestionForDB( columnList, sqlParams );
			Q18_14.PrepareQuestionForDB( columnList, sqlParams );
			Q18_15.PrepareQuestionForDB( columnList, sqlParams );
			Q18_16.PrepareQuestionForDB( columnList, sqlParams );
			Q18_17.PrepareQuestionForDB( columnList, sqlParams );
			Q18_18.PrepareQuestionForDB( columnList, sqlParams );
			Q18_19.PrepareQuestionForDB( columnList, sqlParams );
			Q18_20.PrepareQuestionForDB( columnList, sqlParams );
			Q18_9.PrepareQuestionForDB( columnList, sqlParams );
			Q18_10.PrepareQuestionForDB( columnList, sqlParams );
			Q18_11.PrepareQuestionForDB( columnList, sqlParams );
			Q18_12.PrepareQuestionForDB( columnList, sqlParams );
			Q18_13.PrepareQuestionForDB( columnList, sqlParams );
			Q18_21.PrepareQuestionForDB( columnList, sqlParams );
			//Q18_22.PrepareQuestionForDB( columnList, sqlParams );
			//Q18_23.PrepareQuestionForDB( columnList, sqlParams );
			//Q18_24.PrepareQuestionForDB( columnList, sqlParams );
			Q18_25.PrepareQuestionForDB( columnList, sqlParams );
			Q18_26.PrepareQuestionForDB( columnList, sqlParams );
			Q18_27.PrepareQuestionForDB( columnList, sqlParams );
			Q18_28.PrepareQuestionForDB( columnList, sqlParams );
			Q18_29.PrepareQuestionForDB( columnList, sqlParams );
			Q18_30.PrepareQuestionForDB( columnList, sqlParams );
			Q18_31.PrepareQuestionForDB( columnList, sqlParams );
			Q18_32.PrepareQuestionForDB( columnList, sqlParams );
			Q18_33.PrepareQuestionForDB( columnList, sqlParams );
			Q18_34.PrepareQuestionForDB( columnList, sqlParams );
			Q18_35.PrepareQuestionForDB( columnList, sqlParams );
			Q18_36.PrepareQuestionForDB( columnList, sqlParams );
			Q18_37.PrepareQuestionForDB( columnList, sqlParams );
			Q18_38.PrepareQuestionForDB( columnList, sqlParams );
			Q18_39.PrepareQuestionForDB( columnList, sqlParams );
			Q18_40.PrepareQuestionForDB( columnList, sqlParams );
			Q18_41.PrepareQuestionForDB( columnList, sqlParams );
			Q18_42.PrepareQuestionForDB( columnList, sqlParams );
			Q18_43.PrepareQuestionForDB( columnList, sqlParams );
			Q18_44.PrepareQuestionForDB( columnList, sqlParams );
			Q18_45.PrepareQuestionForDB( columnList, sqlParams );
			Q18_46.PrepareQuestionForDB( columnList, sqlParams );
			Q18_47.PrepareQuestionForDB( columnList, sqlParams );
			Q18_48.PrepareQuestionForDB( columnList, sqlParams );
			Q18_49.PrepareQuestionForDB( columnList, sqlParams );
			Q18_50.PrepareQuestionForDB( columnList, sqlParams );
			Q18_51.PrepareQuestionForDB( columnList, sqlParams );
			Q18_52.PrepareQuestionForDB( columnList, sqlParams );
			Q18_53.PrepareQuestionForDB( columnList, sqlParams);
			Q18_54.PrepareQuestionForDB( columnList, sqlParams);

			Q19_M1.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M1.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M1.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M1.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M1.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M1.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M1.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M1.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M2.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M2.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M2.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M2.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M2.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M2.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M2.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M2.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M3.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M3.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M3.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M3.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M3.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M3.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M3.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M3.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M4.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M4.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M4.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M4.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M4.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M4.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M4.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M4.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M5.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M5.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M5.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M5.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M5.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M5.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M5.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M5.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M6.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M6.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M6.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M6.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M6.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M6.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M6.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M6.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M7.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M7.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M7.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M7.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M7.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M7.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M7.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M7.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M8.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M8.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M8.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M8.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M8.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M8.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M8.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M8.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M9.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M9.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M9.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M9.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M9.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M9.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M9.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M9.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M10.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M10.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M10.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M10.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M10.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M10.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M10.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M10.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M11.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M11.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M11.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M11.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M11.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M11.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M11.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M11.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M12.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M12.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M12.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M12.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M12.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M12.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M12.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M12.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M13.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M13.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M13.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M13.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M13.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M13.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M13.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M13.PrepareQuestionForDB( columnList, sqlParams );
			Q21.PrepareQuestionForDB( columnList, sqlParams );
			txtShowVisitDate.PrepareQuestionForDB( columnList, sqlParams );
			radQ21_HRCV_LoungeA.PrepareQuestionForDB( columnList, sqlParams );
			radQ21_HRCV_LoungeU.PrepareQuestionForDB( columnList, sqlParams );
			Q22.PrepareQuestionForDB( columnList, sqlParams );
			Q23A.PrepareQuestionForDB( columnList, sqlParams );
			Q23B.PrepareQuestionForDB( columnList, sqlParams );
			Q23C.PrepareQuestionForDB( columnList, sqlParams );
			Q23D.PrepareQuestionForDB( columnList, sqlParams );
			Q24.PrepareQuestionForDB( columnList, sqlParams );
			Q24_VisitDate.PrepareQuestionForDB( columnList, sqlParams );
			Q25.PrepareQuestionForDB( columnList, sqlParams );
			Q26A.PrepareQuestionForDB( columnList, sqlParams );
			Q26B.PrepareQuestionForDB( columnList, sqlParams );
			Q26C.PrepareQuestionForDB( columnList, sqlParams );
			Q26D.PrepareQuestionForDB( columnList, sqlParams );
			Q26E.PrepareQuestionForDB( columnList, sqlParams );
			Q27.PrepareQuestionForDB( columnList, sqlParams );
			// Only prepare Q27A fields if user answered 'yes' to Q27
			if(Q27.SelectedValue == 1)
			{
				radQ27A_1.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_2.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_3.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_4.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_5.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_6.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_7.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_8.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_9.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_10.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_11.PrepareQuestionForDB(columnList, sqlParams);
				radQ27A_12.PrepareQuestionForDB(columnList, sqlParams);
				radQ27A_13.PrepareQuestionForDB(columnList, sqlParams);
				txtQ27A_OtherExplanation.PrepareQuestionForDB( columnList, sqlParams );
				txtQ27B.PrepareQuestionForDB( columnList, sqlParams );
			}
			Q28.PrepareQuestionForDB( columnList, sqlParams );
			Q29.PrepareQuestionForDB( columnList, sqlParams );
			Q30.PrepareQuestionForDB( columnList, sqlParams );
			Q31A.PrepareQuestionForDB( columnList, sqlParams );
			Q31B.PrepareQuestionForDB( columnList, sqlParams );
			Q31C.PrepareQuestionForDB( columnList, sqlParams );
			Q31D.PrepareQuestionForDB( columnList, sqlParams );
			Q31E.PrepareQuestionForDB( columnList, sqlParams );
			Q32.PrepareQuestionForDB( columnList, sqlParams );
			Q33.PrepareQuestionForDB( columnList, sqlParams );
			Q34.PrepareQuestionForDB( columnList, sqlParams );
			Q35.PrepareQuestionForDB( columnList, sqlParams );
			Q36Male.PrepareQuestionForDB( columnList, sqlParams );
			Q36Female.PrepareQuestionForDB( columnList, sqlParams );
			Q37_1.PrepareQuestionForDB( columnList, sqlParams );
			Q37_2.PrepareQuestionForDB( columnList, sqlParams );
			Q37_3.PrepareQuestionForDB( columnList, sqlParams );
			Q37_4.PrepareQuestionForDB( columnList, sqlParams );
			Q37_5.PrepareQuestionForDB( columnList, sqlParams );
			Q37_6.PrepareQuestionForDB( columnList, sqlParams );
			Q38_1.PrepareQuestionForDB( columnList, sqlParams );
			Q38_2.PrepareQuestionForDB( columnList, sqlParams );
			Q38_3.PrepareQuestionForDB( columnList, sqlParams );
			Q38_4.PrepareQuestionForDB( columnList, sqlParams );
			Q38_5.PrepareQuestionForDB( columnList, sqlParams );
			Q38_6.PrepareQuestionForDB( columnList, sqlParams );
			Q39_1.PrepareQuestionForDB( columnList, sqlParams );
			Q39_2.PrepareQuestionForDB( columnList, sqlParams );
			Q39_3.PrepareQuestionForDB( columnList, sqlParams );
			Q39_4.PrepareQuestionForDB( columnList, sqlParams );
			Q39_5.PrepareQuestionForDB( columnList, sqlParams );
			Q39_6.PrepareQuestionForDB( columnList, sqlParams );
			Q39_7.PrepareQuestionForDB( columnList, sqlParams );
			Q39_8.PrepareQuestionForDB( columnList, sqlParams );
			Q39_9.PrepareQuestionForDB( columnList, sqlParams );
			Q39_10.PrepareQuestionForDB( columnList, sqlParams );
			Q39_11.PrepareQuestionForDB( columnList, sqlParams );
			Q39_12.PrepareQuestionForDB( columnList, sqlParams );
			Q39_13.PrepareQuestionForDB( columnList, sqlParams );
			Q39_14.PrepareQuestionForDB( columnList, sqlParams );
			Q39_15.PrepareQuestionForDB( columnList, sqlParams );
			Q39_16.PrepareQuestionForDB( columnList, sqlParams );
			Q39_16Explanation.PrepareQuestionForDB( columnList, sqlParams );
			Q40.PrepareQuestionForDB( columnList, sqlParams );
			// Only prepare Q40A fields for saving if user selected yes to Q40 and no to Q27.
			if (Q40.SelectedValue == 1 && Q27.SelectedValue == 0)
			{
				radQ40A_1.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_2.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_3.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_4.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_5.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_6.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_7.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_8.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_9.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_10.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_11.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_12.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_13.PrepareQuestionForDB(columnList, sqlParams);
				txtQ40OtherExplanation.PrepareQuestionForDB(columnList, sqlParams);
			}
			
			OLGYesNo.PrepareQuestionForDB(columnList, sqlParams);
			OLGQ1.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ2.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ3.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ4.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ5.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ6.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ7.PrepareQuestionForDB(columnList, sqlParams);

			txtFirstName.PrepareQuestionForDB( columnList, sqlParams );
			txtLastName.PrepareQuestionForDB( columnList, sqlParams );
			txtTelephoneNumber.PrepareQuestionForDB( columnList, sqlParams );

			columnList.Append( ",[SurveyType],[PropertyID],[DateEntered]");
			sqlParams.Add( "@SurveyType", GetSurveyTypeNumber() )
					 .Add( "@PropertyID", PropertyID )
					 .Add( "@DateEntered", DateTime.Now );


			//Apply sentiment analysis
			if ( !String.IsNullOrWhiteSpace( Q11.Text ) ) {
				columnList.Append( ",[Q11SentimentScore]" );
				sqlParams.Add( "@Q11SentimentScore", SurveyTools.GetSentimentScore( Q11.Text ) );
			}
			if ( !String.IsNullOrWhiteSpace( txtQ27B.Text ) ) {
				columnList.Append( ",[Q27BSentimentScore]" );
				sqlParams.Add( "@Q27BSentimentScore", SurveyTools.GetSentimentScore( txtQ27B.Text ) );
			}
			if ( !String.IsNullOrWhiteSpace( Q32.Text ) ) {
				columnList.Append( ",[Q32SentimentScore]" );
				sqlParams.Add( "@Q32SentimentScore", SurveyTools.GetSentimentScore( Q32.Text ) );
			}
			if ( !String.IsNullOrWhiteSpace( Q34.Text ) ) {
				columnList.Append( ",[Q34SentimentScore]" );
				sqlParams.Add( "@Q34SentimentScore", SurveyTools.GetSentimentScore( Q34.Text ) );
			}
			if ( !String.IsNullOrWhiteSpace( Q35.Text ) ) {
				columnList.Append( ",[Q35SentimentScore]" );
				sqlParams.Add( "@Q35SentimentScore", SurveyTools.GetSentimentScore( Q35.Text ) );
			}

			if ( SurveyType == GEISurveyType.StaffSurvey ) {
				columnList.Append( ",[StaffMemberID]" );
				sqlParams.Add( "@StaffMemberID", User.UserID );
				txtVisitDate.PrepareQuestionForDB( columnList, sqlParams );
			}



			} 
				// fRENCH pART
				else if(strSurveyLang == "French")
				{

					
			if ( !IsKioskOrStaffEntry ) {
				txtEmail_F.PrepareQuestionForDB( columnList, sqlParams );
			} else {
				txtEmail2_F.PrepareQuestionForDB( columnList, sqlParams );
			}

			//Add details for email surveys
			if ( SurveyType == GEISurveyType.Email ) {
				columnList.Append( ",[EmailBatch],[PIN]" );
				sqlParams.Add( "@EmailBatch", EmailPINRow["BatchID"] )
						 .Add( "@PIN", EmailPIN );
				if ( String.IsNullOrWhiteSpace( txtQ4_CardNumber_F.Text ) ) {
					//If they erased the encore number, we'll put in a new one
					Q4_F.SelectedValue_F = 1;
					txtQ4_CardNumber_F.Text = EmailPINRow["Encore"].ToString();
				}
			}

			radQ1_Slots_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Tables_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Poker_F.PrepareQuestionForDB( columnList, sqlParams );           
			radQ1_Food_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Entertainment_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Hotel_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_LiveRacing_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Racebook_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Bingo_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_Lottery_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ1_None_F.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Slots_F.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Tables_F.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Poker_F.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Food_F.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Entertainment_F.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Hotel_F.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_LiveRacing_F.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Racebook_F.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Bingo_F.PrepareQuestionForDB( columnList, sqlParams );
			chkQ2_Lottery_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ3_Everett_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ3_Lakewood_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ3_Tukwila_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ3_DeMoines_F.PrepareQuestionForDB( columnList, sqlParams );
			Q4_F.PrepareQuestionForDB( columnList, sqlParams );
			txtQ4_CardNumber_F.PrepareQuestionForDB( columnList, sqlParams );
			Q5A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q5B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q6A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q6B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q6C_F.PrepareQuestionForDB( columnList, sqlParams );
			Q6D_F.PrepareQuestionForDB( columnList, sqlParams );
			Q7A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q7B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q7C_F.PrepareQuestionForDB( columnList, sqlParams );
			Q7D_F.PrepareQuestionForDB( columnList, sqlParams );
			Q7E_F.PrepareQuestionForDB( columnList, sqlParams );
			Q7F_F.PrepareQuestionForDB( columnList, sqlParams );
			Q8_FN.PrepareQuestionForDB( columnList, sqlParams );
			Q9A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q9B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q9C_F.PrepareQuestionForDB( columnList, sqlParams );
			Q9D_F.PrepareQuestionForDB( columnList, sqlParams );
			Q9E_F.PrepareQuestionForDB( columnList, sqlParams );
			Q9F_F.PrepareQuestionForDB( columnList, sqlParams );
			Q9G_F.PrepareQuestionForDB( columnList, sqlParams );
			Q9H_F.PrepareQuestionForDB( columnList, sqlParams );
			Q9I_F.PrepareQuestionForDB( columnList, sqlParams );
			Q9J_F.PrepareQuestionForDB( columnList, sqlParams );
			Q9K_F.PrepareQuestionForDB(columnList, sqlParams);
			Q10A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q10B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q10C_F.PrepareQuestionForDB( columnList, sqlParams );
			Q11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q13A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q13B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q13C_F.PrepareQuestionForDB( columnList, sqlParams );
			Q13D_F.PrepareQuestionForDB( columnList, sqlParams );
			Q13E_F.PrepareQuestionForDB( columnList, sqlParams );
			Q13F_F.PrepareQuestionForDB( columnList, sqlParams );
			Q13G_F.PrepareQuestionForDB( columnList, sqlParams );
			Q14_F.PrepareQuestionForDB( columnList, sqlParams );
			Q15A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q15B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q15C_F.PrepareQuestionForDB( columnList, sqlParams );
			Q15D_F.PrepareQuestionForDB( columnList, sqlParams );
			Q15E_F.PrepareQuestionForDB( columnList, sqlParams );
			Q15F_F.PrepareQuestionForDB( columnList, sqlParams );
			Q16A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q16B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q16C_F.PrepareQuestionForDB( columnList, sqlParams );
			Q16D_F.PrepareQuestionForDB( columnList, sqlParams );
			Q17_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_7_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_8_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_14_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_15_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_16_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_17_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_18_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_19_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_20_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_9_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_10_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_13_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_21_F.PrepareQuestionForDB( columnList, sqlParams );
			//Q18_22_F.PrepareQuestionForDB( columnList, sqlParams );
			//Q18_23_F.PrepareQuestionForDB( columnList, sqlParams );
			//Q18_24_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_25_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_26_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_27_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_28_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_29_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_30_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_31_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_32_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_33_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_34_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_35_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_36_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_37_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_38_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_39_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_40_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_41_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_42_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_43_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_44_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_45_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_46_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_47_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_48_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_49_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_50_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_51_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_52_F.PrepareQuestionForDB( columnList, sqlParams );
			Q18_53_F.PrepareQuestionForDB( columnList, sqlParams);
			Q18_54_F.PrepareQuestionForDB( columnList, sqlParams);

			Q19_M1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M7_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M7_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M7_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M7_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M7_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M7_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M7_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M7_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M8_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M8_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M8_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M8_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M8_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M8_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M8_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M8_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M9_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M9_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M9_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M9_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M9_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M9_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M9_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M9_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M10_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M10_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M10_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M10_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M10_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M10_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M10_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M10_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q19_M13_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20A_M13_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20B_M13_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20C_M13_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20D_M13_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20E_M13_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20F_M13_F.PrepareQuestionForDB( columnList, sqlParams );
			Q20G_M13_F.PrepareQuestionForDB( columnList, sqlParams );
			Q21_F.PrepareQuestionForDB( columnList, sqlParams );
			txtShowVisitDate_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ21_HRCV_LoungeA_F.PrepareQuestionForDB( columnList, sqlParams );
			radQ21_HRCV_LoungeU_F.PrepareQuestionForDB( columnList, sqlParams );
			Q22_F.PrepareQuestionForDB( columnList, sqlParams );
			Q23A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q23B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q23C_F.PrepareQuestionForDB( columnList, sqlParams );
			Q23D_F.PrepareQuestionForDB( columnList, sqlParams );
			Q24_F.PrepareQuestionForDB( columnList, sqlParams );
			Q24_VisitDate_F.PrepareQuestionForDB( columnList, sqlParams );
			Q25_F.PrepareQuestionForDB( columnList, sqlParams );
			Q26A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q26B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q26C_F.PrepareQuestionForDB( columnList, sqlParams );
			Q26D_F.PrepareQuestionForDB( columnList, sqlParams );
			Q26E_F.PrepareQuestionForDB( columnList, sqlParams );
			Q27_F.PrepareQuestionForDB( columnList, sqlParams );
			// Only prepare Q27A fields if user answered 'yes' to Q27
			if(Q27.SelectedValue == 1)
			{
				radQ27A_1_F.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_2_F.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_3_F.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_4_F.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_5_F.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_6_F.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_7_F.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_8_F.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_9_F.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_10_F.PrepareQuestionForDB( columnList, sqlParams );
				radQ27A_11_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ27A_12_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ27A_13_F.PrepareQuestionForDB(columnList, sqlParams);
				txtQ27A_OtherExplanation_F.PrepareQuestionForDB( columnList, sqlParams );
				txtQ27B_F.PrepareQuestionForDB( columnList, sqlParams );
			}
			Q28_F.PrepareQuestionForDB( columnList, sqlParams );
			Q29_F.PrepareQuestionForDB( columnList, sqlParams );
			Q30_F.PrepareQuestionForDB( columnList, sqlParams );
			Q31A_F.PrepareQuestionForDB( columnList, sqlParams );
			Q31B_F.PrepareQuestionForDB( columnList, sqlParams );
			Q31C_F.PrepareQuestionForDB( columnList, sqlParams );
			Q31D_F.PrepareQuestionForDB( columnList, sqlParams );
			Q31E_F.PrepareQuestionForDB( columnList, sqlParams );
			Q32_F.PrepareQuestionForDB( columnList, sqlParams );
			Q33_F.PrepareQuestionForDB( columnList, sqlParams );
			Q34_F.PrepareQuestionForDB( columnList, sqlParams );
			Q35_F.PrepareQuestionForDB( columnList, sqlParams );
			Q36Male_F.PrepareQuestionForDB( columnList, sqlParams );
			Q36Female_F.PrepareQuestionForDB( columnList, sqlParams );
			Q37_1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q37_2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q37_3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q37_4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q37_5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q37_6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q38_1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q38_2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q38_3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q38_4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q38_5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q38_6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_1_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_2_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_3_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_4_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_5_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_6_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_7_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_8_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_9_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_10_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_11_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_12_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_13_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_14_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_15_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_16_F.PrepareQuestionForDB( columnList, sqlParams );
			Q39_16Explanation_F.PrepareQuestionForDB( columnList, sqlParams );
			Q40_F.PrepareQuestionForDB( columnList, sqlParams );
			// Only prepare Q40A fields for saving if user selected yes to Q40 and no to Q27.
			if (Q40_F.SelectedValue_F == 1 && Q27_F.SelectedValue_F == 0)
			{
				radQ40A_1_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_2_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_3_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_4_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_5_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_6_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_7_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_8_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_9_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_10_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_11_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_12_F.PrepareQuestionForDB(columnList, sqlParams);
				radQ40A_13_F.PrepareQuestionForDB(columnList, sqlParams);
				txtQ40OtherExplanation_F.PrepareQuestionForDB(columnList, sqlParams);
			}
			
			OLGYesNo_F.PrepareQuestionForDB(columnList, sqlParams);
			OLGQ1_F.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ2_F.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ3_F.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ4_F.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ5_F.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ6_F.PrepareQuestionForDB(columnList,sqlParams);
			OLGQ7_F.PrepareQuestionForDB(columnList, sqlParams);

			txtFirstName_F.PrepareQuestionForDB( columnList, sqlParams );
			txtLastName_F.PrepareQuestionForDB( columnList, sqlParams );
			txtTelephoneNumber_F.PrepareQuestionForDB( columnList, sqlParams );

			columnList.Append( ",[SurveyType],[PropertyID],[DateEntered]");
			sqlParams.Add( "@SurveyType", GetSurveyTypeNumber() )
					 .Add( "@PropertyID", PropertyID )
					 .Add( "@DateEntered", DateTime.Now );


			//Apply sentiment analysis
			if ( !String.IsNullOrWhiteSpace( Q11_F.Text ) ) {
				columnList.Append( ",[Q11SentimentScore]" );
				sqlParams.Add( "@Q11SentimentScore", SurveyTools.GetSentimentScore( Q11_F.Text ) );
			}
			if ( !String.IsNullOrWhiteSpace( txtQ27B_F.Text ) ) {
				columnList.Append( ",[Q27BSentimentScore]" );
				sqlParams.Add( "@Q27BSentimentScore", SurveyTools.GetSentimentScore( txtQ27B_F.Text ) );
			}
			if ( !String.IsNullOrWhiteSpace( Q32_F.Text ) ) {
				columnList.Append( ",[Q32SentimentScore]" );
				sqlParams.Add( "@Q32SentimentScore", SurveyTools.GetSentimentScore( Q32_F.Text ) );
			}
			if ( !String.IsNullOrWhiteSpace( Q34_F.Text ) ) {
				columnList.Append( ",[Q34SentimentScore]" );
				sqlParams.Add( "@Q34SentimentScore", SurveyTools.GetSentimentScore( Q34_F.Text ) );
			}
			if ( !String.IsNullOrWhiteSpace( Q35_F.Text ) ) {
				columnList.Append( ",[Q35SentimentScore]" );
				sqlParams.Add( "@Q35SentimentScore", SurveyTools.GetSentimentScore( Q35_F.Text ) );
			}

			if ( SurveyType == GEISurveyType.StaffSurvey ) {
				columnList.Append( ",[StaffMemberID]" );
				sqlParams.Add( "@StaffMemberID", User.UserID );
				txtVisitDate_F.PrepareQuestionForDB( columnList, sqlParams );
			}




				}





			Dictionary<string, int> wordCounts = null;
			columnList.Remove( 0, 1 );
			SQLDatabase sql = new SQLDatabase();
			rowID = sql.QueryAndReturnIdentity( String.Format( "INSERT INTO [tblSurveyGEI] ({0}) VALUES ({1});", columnList, columnList.ToString().Replace( "[", "@" ).Replace( "]", String.Empty ) ), sqlParams );
			if ( !sql.HasError && rowID != -1 ) {

				if (strSurveyLang != "French")
				{
					wordCounts = SurveyTools.GetWordCount(Q11.Text, txtQ27A_OtherExplanation.Text, txtQ27B.Text, Q32.Text, Q34.Text, Q35.Text, Q39_16Explanation.Text);
				}
				else if (strSurveyLang == "French")
				{
					wordCounts = SurveyTools.GetWordCount(Q11_F.Text, txtQ27A_OtherExplanation_F.Text, txtQ27B_F.Text, Q32_F.Text, Q34_F.Text, Q35_F.Text, Q39_16Explanation_F.Text);
				}
				SurveyTools.SaveWordCounts( SharedClasses.SurveyType.GEI, rowID, wordCounts );
				if ( SurveyType == GEISurveyType.Email ) {
					sql.NonQuery( "UPDATE [tblSurveyGEI_EmailPINs] SET [SurveyCompleted] = 1 WHERE PIN = @PIN", new SqlParameter( "@PIN", EmailPIN ) );
					if ( sql.HasError ) {
						//TODO: Do we want to do something here?
					}
				}
				return true;
			} else {
				return false;
			}




		
#endregion
		}

		protected void Unnamed_Click(object sender, EventArgs e)
		{

		}

		protected void PINSurveyLang_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.PINSurveyLang.SelectedValue == "English")
			{
				this.English.Visible = true;
				this.French.Visible = false;
			}
			else
			{
				this.French.Visible = true;
				this.English.Visible = false;
			}
		}
	}

}