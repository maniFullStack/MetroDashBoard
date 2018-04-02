using Resources;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
	public partial class SurveyFeedback : BasePage
	{
		protected bool SurveyComplete { get; set; }
		protected int FeedbackID { get; set; }
		protected string FeedbackUID { get; set; }

		private string GCCPortalUrl = ConfigurationManager.AppSettings["GCCPortalURL"].ToString();

		protected bool IsStaffSurvey
		{
			get
			{
				return Request.Url.AbsolutePath.ToLower().StartsWith("/sfeedback");
			}
		}

		protected UserInfo User
		{
			get
			{
				return SessionWrapper.Get<UserInfo>("UserInfo", null);
			}
		}

		public GCCPropertyShortCode ForceSpecificProperty { get; set; }

		/// <summary>
		/// Gets the property ID to load the theme CSS file for.
		/// </summary>
		public int PropertyID
		{
			get
			{
				return (int)AlignedPropertyShortCode;
			}
		}

		protected GCCPropertyShortCode AlignedPropertyShortCode
		{
			get
			{
				if (Master.PropertyShortCode == GCCPropertyShortCode.GCC)
				{
					int selectedProp = Conversion.StringToInt(fbkProperty.SelectedValue, 0);
					if (selectedProp != 0)
					{
						try
						{
							GCCPropertyShortCode sc = (GCCPropertyShortCode)selectedProp;
							return sc;
						}
						catch
						{
							return GCCPropertyShortCode.GCC;
						}
					}
					else
					{
						return GCCPropertyShortCode.GCC;
					}
				}
				else
				{
					return Master.PropertyShortCode;
				}
			}
		}

		protected void Page_PreInit(object sender, EventArgs e)
		{
			//Ensure that a user is logged in for the staff survey entry
			if (IsStaffSurvey && User == null)
			{
				Response.Redirect("/Login?rd=" + Server.UrlPathEncode(Request.Url.PathAndQuery), true);
				return;
			}
		}

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
			//Session["CurrentUI"] = "en-CA";
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

		protected void Page_Load(object sender, EventArgs e)
		{
			Title = "Feedback &raquo; " + Master.CasinoName;

			if (!IsPostBack)
			{

				//PAGE 1 -- Initialize Drop Down Menus
				ddlStaffContact.Items.Insert(1, new ListItem(Lang.Page1_Question1_1, "In-Person"));
				ddlStaffContact.Items.Insert(2, new ListItem(Lang.Page1_Question1_2, "Phone"));

				//Alphabetical
				fbkProperty.Items.Insert(1, new ListItem(Lang.Page1_Question2_2, "7"));
				fbkProperty.Items.Insert(2, new ListItem(Lang.Page1_Question2_17, "19"));
				fbkProperty.Items.Insert(3, new ListItem(Lang.Page1_Question2_3, "11"));
				fbkProperty.Items.Insert(4, new ListItem(Lang.Page1_Question2_4, "12"));
				fbkProperty.Items.Insert(5, new ListItem(Lang.Page1_Question2_5, "8"));
				fbkProperty.Items.Insert(6, new ListItem(Lang.Page1_Question2_6, "10"));
				fbkProperty.Items.Insert(7, new ListItem(Lang.Page1_Question2_7, "9"));
				fbkProperty.Items.Insert(8, new ListItem(Lang.Page1_Question2_8, "14"));
				//fbkProperty.Items.Insert(SequenceNumberHere, new ListItem(Lang.Page1_Question2_9, "4"));
				fbkProperty.Items.Insert(9, new ListItem(Lang.Page1_Question2_10, "13"));
				fbkProperty.Items.Insert(10, new ListItem(Lang.Page1_Question2_11, "3"));
				fbkProperty.Items.Insert(11, new ListItem(Lang.Page1_Question2_12, "5"));
				fbkProperty.Items.Insert(12, new ListItem(Lang.Page1_Question2_13, "2"));
				fbkProperty.Items.Insert(13, new ListItem(Lang.Page1_Question2_14, "18"));
				fbkProperty.Items.Insert(14, new ListItem(Lang.Page1_Question2_15, "17"));
				fbkProperty.Items.Insert(15, new ListItem(Lang.Page1_Question2_16, "6"));
				fbkProperty.Items.Insert(16, new ListItem(Lang.Page1_Question2_18, "20"));
				fbkProperty.Items.Insert(17, new ListItem(Lang.Page1_Question2_19, "22"));
				fbkProperty.Items.Insert(18, new ListItem(Lang.Page1_Question2_20, "23"));
				fbkProperty.Items.Insert(19, new ListItem(Lang.Page1_Question2_21, "24"));
				
				fbkProperty.Items.Insert(20, new ListItem(Lang.Page1_Question2_1, "1")); //No Specific Property

				fbkDirectQuestion.Items.Insert(1, new ListItem(Lang.Page1_Question2B_1, "1"));
				fbkDirectQuestion.Items.Insert(2, new ListItem(Lang.Page1_Question2B_2, "2"));
				fbkDirectQuestion.Items.Insert(3, new ListItem(Lang.Page1_Question2B_3, "3"));
				fbkDirectQuestion.Items.Insert(4, new ListItem(Lang.Page1_Question2B_4, "4"));
				fbkDirectQuestion.Items.Insert(5, new ListItem(Lang.Page1_Question2B_5, "5"));

				//fbkProperty.Items.Insert(16, new ListItem(Lang.Page1_Question2_17, "19"));

				//PAGE 2 -- Initialize Radio Buttons and Drop Down Menus
				radGAG_Everett.Text = Lang.Page2_Radio1;
				radGAG_Everett.GroupName = "GAG";
				radGAG_Everett.SessionKey = "GAG_Everett";
				radGAG_Everett.DBColumn = "GAGProperty";
				radGAG_Everett.DBValue = "Everett";

				radGAG_Lakewood.Text = Lang.Page2_Radio2;
				radGAG_Lakewood.GroupName = "GAG";
				radGAG_Lakewood.SessionKey = "GAG_Lakewood";
				radGAG_Lakewood.DBColumn = "GAGProperty";
				radGAG_Lakewood.DBValue = "Lakewood";

				radGAG_Tukwila.Text = Lang.Page2_Radio3;
				radGAG_Tukwila.GroupName = "GAG";
				radGAG_Tukwila.SessionKey = "GAG_Tukwila";
				radGAG_Tukwila.DBColumn = "GAGProperty";
				radGAG_Tukwila.DBValue = "Tukwila";

				radGAG_DeMoines.Text = Lang.Page2_Radio4;
				radGAG_DeMoines.GroupName = "GAG";
				radGAG_DeMoines.SessionKey = "GAG_DeMoines";
				radGAG_DeMoines.DBColumn = "GAGProperty";
				radGAG_DeMoines.DBValue = "DeMoines";

				fbkQ1.Items.Insert(1, new ListItem(Lang.Page2_Question2_1, "Ask a question"));
				fbkQ1.Items.Insert(2, new ListItem(Lang.Page2_Question2_2, "Report a problem"));
				fbkQ1.Items.Insert(3, new ListItem(Lang.Page2_Question2_3, "Offer a compliment"));
				fbkQ1.Items.Insert(4, new ListItem(Lang.Page2_Question2_4, "Suggest an improvement"));
				fbkQ1.Items.Insert(5, new ListItem(Lang.Page2_Question2_5, "Ask for Donation / Support"));

				//PAGE 3 -- Initialize Drop Down Menus
				fbkQ2.Items.Insert(1, new ListItem(Lang.Page3_Question1_1, "15"));
				fbkQ2.Items.Insert(2, new ListItem(Lang.Page3_Question1_2, "17"));
				fbkQ2.Items.Insert(3, new ListItem(Lang.Page3_Question1_3, "1"));
				fbkQ2.Items.Insert(4, new ListItem(Lang.Page3_Question1_4, "3"));
				fbkQ2.Items.Insert(5, new ListItem(Lang.Page3_Question1_5, "2"));
				fbkQ2.Items.Insert(6, new ListItem(Lang.Page3_Question1_6, "6"));
				fbkQ2.Items.Insert(7, new ListItem(Lang.Page3_Question1_7, "13"));
				fbkQ2.Items.Insert(8, new ListItem(Lang.Page3_Question1_8, "5"));
				fbkQ2.Items.Insert(9, new ListItem(Lang.Page3_Question1_9, "21"));
				fbkQ2.Items.Insert(10, new ListItem(Lang.Page3_Question1_10, "4"));
				fbkQ2.Items.Insert(11, new ListItem(Lang.Page3_Question1_11, "20"));
				fbkQ2.Items.Insert(12, new ListItem(Lang.Page3_Question1_12, "10"));
				fbkQ2.Items.Insert(13, new ListItem(Lang.Page3_Question1_13, "19"));
				fbkQ2.Items.Insert(14, new ListItem(Lang.Page3_Question1_14, "16"));
				fbkQ2.Items.Insert(15, new ListItem(Lang.Page3_Question1_15, "14"));
				fbkQ2.Items.Insert(16, new ListItem(Lang.Page3_Question1_16, "7"));
				fbkQ2.Items.Insert(17, new ListItem(Lang.Page3_Question1_17, "11"));
				fbkQ2.Items.Insert(18, new ListItem(Lang.Page3_Question1_18, "12"));
		   

				fbkQ5.Items.Insert(1, new ListItem(Lang.Page3_Question4_1, "By e-mail"));
				fbkQ5.Items.Insert(2, new ListItem(Lang.Page3_Question4_2, "By telephone"));
				fbkQ5.Items.Insert(3, new ListItem(Lang.Page3_Question4_3, "I do not want to be contacted"));
			}
		}

		protected void Page_LoadComplete(object sender, EventArgs e)
		{
			int j = 1;

			//slots
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "SCBE", "WDB", "GBH","AJA","ECB","ECF","ECGR","ECM" }.Contains(AlignedPropertyShortCode.ToString()));
			//Table Games / Poker
            fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "VRL", "NAN", "GAG", "CNSH", "CNSS", "EC", "SCTI", "CNB", "SCBE", "WDB", "GBH", "ECB", "ECF", "ECM" }.Contains(AlignedPropertyShortCode.ToString()));
			//Bingo
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "CCH", "CMR", "CDC" }.Contains(AlignedPropertyShortCode.ToString()));
			//Food & Beverage
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "SCBE", "WDB", "GBH", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));
			//Entertainment
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "CCH", "CMR", "CDC", "CNSH", "EC" }.Contains(AlignedPropertyShortCode.ToString()));
			//Hotel
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR" }.Contains(AlignedPropertyShortCode.ToString()));
			//Racebook
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "HA", "NAN", "CMR", "CNSS", "EC", "SSKD", "SCBE", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));
			//Horse Racing
            fbkQ2.Items[j++].Enabled = (new[] { "GCC", "HA", "CNSS", "EC", "ECF", "ECGR", "ECM" }.Contains(AlignedPropertyShortCode.ToString()));
			//Motorcoach / Bus Tours
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "CNSH" }.Contains(AlignedPropertyShortCode.ToString()));
			//Guest Services
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "SCBE", "WDB", "GBH", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));
			//Parking
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "SSKD", "SCTI", "CNB", "SCBE", "WDB", "GBH", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));
			//Marketing & Promotions
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "SCBE", "WDB", "GBH", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));
			//Group Sales / Catering / Events
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "CNSH", "SSKD", "SCTI", "SCBE", "CNB", "WDB", "GBH", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));
			//Sponsorship Request
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "WDB", "GBH", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));
			//Responsible Gaming
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "SCBE", "WDB", "GBH", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));
			//Investor Relations
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "SCBE", "WDB", "GBH", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));
			//Media Requests & Inquiries
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "SCBE", "WDB", "GBH", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));
			//Other
			fbkQ2.Items[j++].Enabled = (new[] { "GCC", "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "SCBE", "AJA" }.Contains(AlignedPropertyShortCode.ToString()));

			//Slots
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Table Games / Poker
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "VRL", "NAN", "GAG", "CNSH", "CNSS", "EC", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Bingo
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "CCH", "CMR", "CDC" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Food & Beverage
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Entertainment
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "CCH", "CMR", "CDC", "CNSH", "EC" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Hotel
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Racebook
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "HA", "NAN", "CMR", "CNSS", "EC", "SSKD" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Horse Racing
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "FD", "HA", "CNSS", "EC" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Motorcoach / Bus Tours
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "CNSH" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Guest Services
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Parking
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Marketing & Promotions
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Group Sales / Catering / Events
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "CNSH", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Sponsorship Request
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Responsible Gaming
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Investor Relations
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Media Requests & Inquiries
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Other
			//fbkQ2.Items[j++].Enabled = ( new[] { "GCC", "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "GAG", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( AlignedPropertyShortCode.ToString() ) );
			////Check all previous pages
			//Must do in LoadComplete because controls load values in Load method (Init didn't work because reasons...)

			if (AlignedPropertyShortCode.ToString() == "CNB" || AlignedPropertyShortCode.ToString() == "SCTI" || AlignedPropertyShortCode.ToString() == "WDB")
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
						FeedbackID = surveyID;
						string feedbackUID;
						AddFeedback(surveyID, out feedbackUID);
						SendNotifications(surveyID, feedbackUID);
						FeedbackUID = feedbackUID;
						SurveyComplete = true;
						//If not a staff survey, abandon the session, else, remove control session keys.
						if (!IsStaffSurvey)
						{
							Session.Abandon();
						}
						else
						{
							WipeSurveyControls(Controls);
						}
						if (Session["CurrentUI"].ToString() == "fr-CA")
						{
							mmLastPage.FrSuccessMessage = String.Format("<p>Merci d’avoir donné vos impressions! Vos réponses ont été transférées à un représentant. Si vous avez demandé une réponse de notre part, veuillez attendre de nos nouvelles d’ici 12 à 24 heures. Veuillez vérifier votre dossier de pourriels ou ajouter « @gcgamingsurvey.com » à votre carnet d’adresses.<br /><br />Si votre question est plus urgente, veuillez communiquer avec le service à la clientèle au {0}.</p>", PropertyTools.GetPhoneNumber(AlignedPropertyShortCode, 0));
						}
						else
						{
							mmLastPage.SuccessMessage = String.Format("<p>Thank you for your feedback! Your responses have been forwarded to the appropriate representative. If you have requested a response, please expect contact within 12-24 hours. Please ensure you check your \"Junk Mail\" folder or add \"@gcgamingsurvey.com\" to your email account's white list.<br /><br />If your question is more urgent, please contact Guest Services at {0}.</p>", PropertyTools.GetPhoneNumber(AlignedPropertyShortCode, 0));
						}
						
					}
					else
					{
						mmLastPage.ErrorMessage = "We were unable to save your responses. Please go back and try again.";
					}
				}
			}
		}
		
		private void WipeSurveyControls(ControlCollection controls)
		{
			foreach (Control ctl in controls)
			{
				//Wipe this control
				if (ctl is ISurveyControl<string>)
				{
					ISurveyControl<string> sc = ctl as ISurveyControl<string>;
					if (sc != null)
					{
						SessionWrapper.Remove(sc.SessionKey);
					}
				}
				if (ctl is ISurveyControl<int>)
				{
					ISurveyControl<int> sc = ctl as ISurveyControl<int>;
					if (sc != null)
					{
						SessionWrapper.Remove(sc.SessionKey);
					}
				}
				if (ctl is ISurveyControl<bool>)
				{
					ISurveyControl<bool> sc = ctl as ISurveyControl<bool>;
					if (sc != null)
					{
						SessionWrapper.Remove(sc.SessionKey);
					}
				}
				//Recursively check child controls
				if (ctl.Controls != null && ctl.Controls.Count > 0)
				{
					WipeSurveyControls(ctl.Controls);
				}
			}
		}

		private void AddFeedback(int surveyID, out string feedbackUID)
		{
			if (!fbkQ5.SelectedValue.Equals("I do not want to be contacted"))
			{
				int reasonID = fbkQ2.SelectedValue.StringToInt();
				//Add the feedback
				SQLDatabase sql = new SQLDatabase();
				SqlParameter feedbackUIDParam = new SqlParameter("@UID", System.Data.SqlDbType.UniqueIdentifier);
				feedbackUIDParam.Direction = System.Data.ParameterDirection.Output;

				sql.ExecStoredProcedureDataSet("spFeedback_Create",
					new SQLParamList()
							.Add("@PropertyID", (int)AlignedPropertyShortCode)
							.Add("@SurveyTypeID", SharedClasses.SurveyType.Feedback)
							.Add("@RecordID", surveyID)
							.Add("@ReasonID", reasonID)
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
			string gagLocation = String.Empty;
			if (AlignedPropertyShortCode == GCCPropertyShortCode.GAG)
			{
				if (radGAG_Everett.Checked)
				{
					gagLocation = "Everett";
				}
				else if (radGAG_Lakewood.Checked)
				{
					gagLocation = "Lakewood";
				}
				else if (radGAG_Tukwila.Checked)
				{
					gagLocation = "Tukwila";
				}
				else if (radGAG_DeMoines.Checked)
				{
					gagLocation = "DeMoines";
				}
				if (gagLocation.Length > 0)
				{
					gagLocation = " - " + gagLocation;
				}
			}

			NotificationReason nr = (NotificationReason)fbkQ2.SelectedValue.StringToInt();
			//Send the notification
			SurveyTools.SendNotifications(
				Server,
				AlignedPropertyShortCode,
				SharedClasses.SurveyType.Feedback,
				nr,
				fbkQ3.Text.ToString(),
				new
				{
					
					Date = DateTime.Now.ToString("yyyy-MM-dd"),
					CasinoName = Master.CasinoName + gagLocation,
					FeedbackNoteHTML = String.IsNullOrEmpty(feedbackUID) ? String.Empty : "<p><b>This guest has requested a response.</b> You can view and respond to the feedback request by clicking on (or copying and pasting) the following link:</p>\n<p>" + GCCPortalUrl + "Admin/Feedback/" + feedbackUID + "</p>",
					FeedbackNoteTXT = String.IsNullOrEmpty(feedbackUID) ? String.Empty : "The guest requested feedback. You can view and respond to the feedback request by clicking on (or copying and pasting) the following link:\n" + GCCPortalUrl + "Admin/Feedback/" + feedbackUID + "\n\n",
					SurveyLink = GCCPortalUrl + "Display/Feedback/" + surveyID

				},
				String.Empty,
				String.Empty,
				fbkDirectQuestion.SelectedValue.StringToInt(-1)
			);
			//Determine triggers for feedback request
			if (!IsStaffSurvey
				&& !fbkQ5.SelectedValue.Equals("I do not want to be contacted")
				&& !String.IsNullOrEmpty(txtEmailContact.Text))
			{
				//Send thank you letter
				SurveyTools.SendNotifications(
					Server,
					AlignedPropertyShortCode,
					SharedClasses.SurveyType.Feedback,
					NotificationReason.ThankYou,
					string.Empty,
					new
					{
						CasinoName = PropertyTools.GetCasinoName(Master.PropertyID),

						Attachments = new SurveyTools.SurveyAttachmentDetails[] 
						{
							new SurveyTools.SurveyAttachmentDetails() { Path = "~/Images/headers/" + PropertyTools.GetCasinoHeaderImage( AlignedPropertyShortCode ), ContentID = "HeaderImage" }
						}
					},
					txtEmailContact.Text,
					String.Empty,
					fbkDirectQuestion.SelectedValue.StringToInt(-1) );
			}
		}

		protected void Prev_Click(object sender, EventArgs e)
		{
			if (ValidateAndSave(Master.CurrentPage, true, true))
			{
				int prevPage = Master.CurrentPage - 1;
				if (Master.CurrentPage == 99)
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
				//If the user selects “Ask for Donation/Support”, redirect to {propertyshortcode}/SurveyDonation.aspx
				//if (fbkQ1.SelectedIndex == 10 && Master.CurrentPage == 2)
				//20171219_Redirecting to donation request based on selection
				if (fbkQ1.SelectedIndex == 5 && Master.CurrentPage == 2)
				{
					Response.Redirect(String.Format("/DonationRequest/{0}/", AlignedPropertyShortCode.ToString()), true);
					return;
				}

				int nextPage = Master.CurrentPage + 1;
				if (nextPage > 5)
				{
					nextPage = 99;
				}
				if (Master.CurrentPage == 99)
				{
					Response.Redirect(PropertyTools.GetCasinoURL(AlignedPropertyShortCode), true);
					return;
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
						SurveyTools.SaveValue<string>(fbkEmail);
						SurveyTools.SaveValue<string>(fbkProperty);
						SurveyTools.SaveValue<string>(fbkDirectQuestion);
						if (IsStaffSurvey)
						{
							SurveyTools.SaveValue<string>(ddlStaffContact);
						}
					}
					string email = SurveyTools.GetValue(fbkEmail, currentPage, String.Empty);
					if (!Validation.RegExCheck(email, ValidationType.Email)
						&& !(String.IsNullOrWhiteSpace(email) && IsStaffSurvey))
					{ //Allow blanks on staff survey
						mmTrackingEmail.ErrorMessage = "Please enter a valid email address.";
						retVal = false;
					}
					if (Master.PropertyShortCode == GCCPropertyShortCode.GCC && fbkProperty.SelectedIndex == 0)
					{
						fbkProperty.MessageManager.ErrorMessage = "Please indicate which property you are providing feedback for or select \"No Specific Property\".";
						retVal = false;
					}

					if (fbkProperty.GetValue() == "1" && fbkDirectQuestion.SelectedIndex == 0)
					{
						fbkDirectQuestion.MessageManager.ErrorMessage = "Please select which area of operations you are providing feedback for.";
						retVal = false;
					}
					if (IsStaffSurvey && ddlStaffContact.SelectedIndex == 0)
					{
						ddlStaffContact.MessageManager.ErrorMessage = "Please indicate how the guest was contacted.";
						retVal = false;
					}
					break;

					#endregion Page 1

				case 2: // Initial question

					#region Page 2

					if (currentPage)
					{
						SurveyTools.SaveValue<string>(fbkQ1);
						if (AlignedPropertyShortCode == GCCPropertyShortCode.GAG)
						{
							SurveyTools.SaveRadioButtons(radGAG_Everett, radGAG_Lakewood, radGAG_Tukwila, radGAG_DeMoines);
						}
					}
					if (!saveOnly)
					{
						bool q3Check = (AlignedPropertyShortCode != GCCPropertyShortCode.GAG)
										|| SurveyTools.GetValue(radGAG_Everett, currentPage, false)
										|| SurveyTools.GetValue(radGAG_Lakewood, currentPage, false )
										|| SurveyTools.GetValue( radGAG_Tukwila, currentPage, false )
										|| SurveyTools.GetValue( radGAG_DeMoines, currentPage, false );
						if (!q3Check)
						{
							mmGAG.ErrorMessage = "Please select one of the following options.";
							retVal = false;
						}
						if (fbkQ1.SelectedIndex == 0)
						{
							fbkQ1.MessageManager.ErrorMessage = "Please select an answer.";
							retVal = false;
						}
					}
					break;

					#endregion Page 2

				case 3: // Basic Form

					#region Page 3

					if (currentPage)
					{
						SurveyTools.SaveValue<string>(fbkQ2);
						SurveyTools.SaveValue<string>(fbkQ3);
						SurveyTools.SaveValue<string>(fbkQ4);
						SurveyTools.SaveValue<string>(fbkQ5);
					}

					if (!saveOnly)
					{
						if (fbkQ2.SelectedIndex == 0)
						{
							fbkQ2.MessageManager.ErrorMessage = "Please select an answer.";
							retVal = false;
						}

						if (!SurveyTools.CheckForAnswer(fbkQ3, true))
						{
							retVal = false;
						}

						if (!SurveyTools.CheckForAnswer(fbkQ4, false))
						{
							retVal = false;
						}

						if (fbkQ5.SelectedIndex == 0)
						{
							fbkQ5.MessageManager.ErrorMessage = "Please select an answer.";
							retVal = false;
						}
					}

					break;

					#endregion Page 3

				case 4: // Contact Info

					#region Page 4

					if (!saveOnly && !fbkQ5.GetValue().Equals("I do not want to be contacted"))
					{
						//Check for phone or email validation depending on Q5
						email = SurveyTools.GetValue(txtEmailContact, currentPage, String.Empty);
						if (fbkQ5.GetValue().Equals("By e-mail")
							&& (String.IsNullOrEmpty(email)
								|| !Validation.RegExCheck(email, ValidationType.Email)))
						{
							txtEmailContact.MessageManager.ErrorMessage = "Please enter a valid email address.";
							retVal = false;
						}
						if (fbkQ5.GetValue().Equals("By telephone")
							&& !SurveyTools.CheckForAnswer(txtTelephoneNumber, true))
						{
							retVal = false;
						}

						if (!SurveyTools.CheckForAnswer(txtName, true))
						{
							retVal = false;
						}
					}
					if (currentPage)
					{
						SurveyTools.SaveValue<string>(txtName);
						SurveyTools.SaveValue<string>(txtTelephoneNumber);
						SurveyTools.SaveValue<string>(txtEmailContact);
					}
					break;

					#endregion Page 4

				case 5: // Confirmation

					#region Page 5

					break;

					#endregion Page 5
			}
			return retVal;
		}

		private bool PageShouldBeSkipped(int CurrentPage)
		{
			switch (CurrentPage)
			{
				case 4: //Change this to No ???
					return fbkQ5.SelectedValue.Equals("I do not want to be contacted");
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
			return String.Format("/{4}Feedback/{0}/{1}{2}{3}", Master.PropertyShortCode.ToString(), page, (redirDir == -1 ? "/-1" : String.Empty), (isReset ? "?r=1" : String.Empty), (IsStaffSurvey ? "S" : String.Empty));
		}

		protected bool SaveData(out int rowID)
		{
			StringBuilder columnList = new StringBuilder();
			SQLParamList sqlParams = new SQLParamList();

			fbkEmail.PrepareQuestionForDB(columnList, sqlParams);
			fbkProperty.PrepareQuestionForDB(columnList, sqlParams);
			radGAG_Everett.PrepareQuestionForDB(columnList, sqlParams);
			radGAG_Lakewood.PrepareQuestionForDB(columnList, sqlParams);
			radGAG_Tukwila.PrepareQuestionForDB( columnList, sqlParams );
			radGAG_DeMoines.PrepareQuestionForDB( columnList, sqlParams );
			fbkQ1.PrepareQuestionForDB(columnList, sqlParams);
			fbkQ2.PrepareQuestionForDB(columnList, sqlParams);
			fbkQ3.PrepareQuestionForDB(columnList, sqlParams);
			fbkQ4.PrepareQuestionForDB(columnList, sqlParams);
			fbkQ5.PrepareQuestionForDB(columnList, sqlParams);
			txtName.PrepareQuestionForDB(columnList, sqlParams);
			txtTelephoneNumber.PrepareQuestionForDB(columnList, sqlParams);
			txtEmailContact.PrepareQuestionForDB(columnList, sqlParams);
		   // fbkDirectQuestion.PrepareQuestionForDB(columnList, sqlParams);

			columnList.Append(",[PropertyID],[DateEntered]");
			sqlParams.Add("@PropertyID", Master.PropertyID)
					 .Add("@DateEntered", DateTime.Now);

			if (IsStaffSurvey)
			{
				columnList.Append(",[StaffMemberID]");
				sqlParams.Add("@StaffMemberID", User.UserID);
				ddlStaffContact.PrepareQuestionForDB(columnList, sqlParams);
			}

			columnList.Remove(0, 1);
			SQLDatabase sql = new SQLDatabase();
			rowID = sql.QueryAndReturnIdentity(String.Format("INSERT INTO [tblSurveyFeedback] ({0}) VALUES ({1});", columnList, columnList.ToString().Replace("[", "@").Replace("]", String.Empty)), sqlParams);
			if (!sql.HasError && rowID != -1)
			{
				Dictionary<string, int> wordCounts = SurveyTools.GetWordCount(fbkQ3.Text);
				SurveyTools.SaveWordCounts(SharedClasses.SurveyType.Feedback, rowID, wordCounts);
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}