using SharedClasses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin {
	public partial class FeedbackItem : AuthenticatedPage {

		private string GCCPortalUrl = ConfigurationManager.AppSettings["GCCPortalURL"].ToString();

		/// <summary>
		/// Gets the GUID for the current request.
		/// </summary>
		public string GUID {
			get {
				object property = Page.RouteData.Values["guid"];
				if ( property != null ) {
					return property.ToString();
				} else {
					return String.Empty;
				}
			}
		}

		/// <summary>
		/// Gets whether or not the current issue is closed. Should only be referenced after the LoadComplete event.
		/// </summary>
		public bool IssueIsClosed {
			get {
				int feedbackStatus = Conversion.StringToInt( Data.Tables[0].Rows[0]["FeedbackStatusID"].ToString() );
				return ( feedbackStatus == (int)FeedbackStatus.ClosedGuestResponseComplete
						|| feedbackStatus == (int)FeedbackStatus.ClosedNoFurtherActionRequired
						|| feedbackStatus == (int)FeedbackStatus.ClosedNoResponse
						|| feedbackStatus == (int)FeedbackStatus.ClosedUnabletoSatisfyGuest );
			}
		}

		/// <summary>
		/// Gets the problem description for "Feedback Reason / Area:" field in Feedback Requests Details box
		/// </summary>
		public string ProblemDescription
		{ 
			get
			{
				DataRow questionDetailRow = Data.Tables[2].Rows[0];
				DataRow feedbackDetailRow = Data.Tables[0].Rows[0];
				string retVal = string.Empty;

				if (feedbackDetailRow["SurveyTypeID"].ToString() == "1")
				{
					if (questionDetailRow["Q27"].ToString() == "1")
					{
						if (questionDetailRow["Q27A_ArrivalAndParking"].ToString() == "1")
						{
							retVal = "Arrival and parking";
						}
						else if (questionDetailRow["Q27A_GuestServices"].ToString() == "1")
						{
							retVal = "Guest Services";
						}
						else if (questionDetailRow["Q27A_Cashiers"].ToString() == "1")
						{
							retVal = "Cashiers";
						}
						else if (questionDetailRow["Q27A_ManagerSupervisor"].ToString() == "1")
						{
							retVal = "Manager/Supervisor";
						}
						else if (questionDetailRow["Q27A_Security"].ToString() == "1")
						{
							retVal = "Security";
						}
						else if (questionDetailRow["Q27A_Slots"].ToString() == "1")
						{
							retVal = "Slots";
						}
						else if (questionDetailRow["Q27A_Tables"].ToString() == "1")
						{
							retVal = "Tables";
						}
						else if (questionDetailRow["Q27A_FoodAndBeverage"].ToString() == "1")
						{
							retVal = "Food & Beverage";
						}
						else if (questionDetailRow["Q27A_Hotel"].ToString() == "1")
						{
							retVal = "Hotel";
						}
						else if(questionDetailRow["Q27A_Bingo"].ToString() == "1")
						{
							retVal = "Bingo";
						}
						else if (questionDetailRow["Q27A_Entertainment"].ToString() == "1")
						{
							retVal = "Entertainment";
						}
						else if (questionDetailRow["Q27A_HorseRacing"].ToString() == "1")
						{
							retVal = "Horse Racing";
						}
						else if (questionDetailRow["Q27A_Other"].ToString() == "1")
						{
							retVal = "Other: <br /><br />" + questionDetailRow["Q27A_OtherExplanation"].ToString();
						}
					}
					else
					{
						// REC - 25-April-2016 -  This is in place to handle the new feedback categories implementation
						if(questionDetailRow["Q40"].ToString() == "1")
						{
							if (questionDetailRow["Q27A_ArrivalAndParking"].ToString() == "1")
							{
								retVal = "Arrival and parking";
							}
							else if (questionDetailRow["Q27A_GuestServices"].ToString() == "1")
							{
								retVal = "Guest Services";
							}
							else if (questionDetailRow["Q27A_Cashiers"].ToString() == "1")
							{
								retVal = "Cashiers";
							}
							else if (questionDetailRow["Q27A_ManagerSupervisor"].ToString() == "1")
							{
								retVal = "Manager/Supervisor";
							}
							else if (questionDetailRow["Q27A_Security"].ToString() == "1")
							{
								retVal = "Security";
							}
							else if (questionDetailRow["Q27A_Slots"].ToString() == "1")
							{
								retVal = "Slots";
							}
							else if (questionDetailRow["Q27A_Tables"].ToString() == "1")
							{
								retVal = "Tables";
							}
							else if (questionDetailRow["Q27A_FoodAndBeverage"].ToString() == "1")
							{
								retVal = "Food & Beverage";
							}
							else if (questionDetailRow["Q27A_Hotel"].ToString() == "1")
							{
								retVal = "Hotel";
							}
							else if (questionDetailRow["Q27A_Bingo"].ToString() == "1")
							{
								retVal = "Bingo";
							}
							else if (questionDetailRow["Q27A_Entertainment"].ToString() == "1")
							{
								retVal = "Entertainment";
							}
							else if (questionDetailRow["Q27A_HorseRacing"].ToString() == "1")
							{
								retVal = "Horse Racing";
							}
							else if (questionDetailRow["Q27A_Other"].ToString() == "1")
							{
								retVal = "Other: <br /><br />" + questionDetailRow["Q27A_OtherExplanation"].ToString();

							}
						}
						else
						{
							retVal = ReportingTools.CleanData(feedbackDetailRow["ReasonDescription"]);
						}
					}
				}
				else
				{
					retVal = ReportingTools.CleanData(feedbackDetailRow["ReasonDescription"]);
				}

				return retVal;
			}
		}

		protected DataSet Data = null;


		protected void Page_Load( object sender, EventArgs e ) {
			Title = "GCC Feedback";
			Master.HideAllFilters = true;
			MessageTimeline.OnReply += MessageTimeline_OnReply;
			MessageTimeline.OnAddNote += MessageTimeline_OnAddNote;
			if ( IsPostBack ) {
				//Go to the end of the messages, but only if this isn't the tier change button
				if ( !Request.Form["__EVENTTARGET"].Equals( btnSaveTier.UniqueID ) ) {
					ClientScript.RegisterStartupScript( this.GetType(), "hash", "location.hash = '#end-of-messages';", true );
				}
			} else {
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
			}
		}

		protected void Page_PreRender( object sender, EventArgs e ) {
			ViewState["CheckRefresh"] = Session["CheckRefresh"];
		}

		private void MessageTimeline_OnReply( TextBox textBox, MessageManager mm ) {
			//Prevent refresh from re-submitting form data
			if ( Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString() ) {
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
			} else {
				//Catch refresh and wipe the value again
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
				textBox.Text = String.Empty;
				return;
			}
			MessageTimeline.NoteTabActive = false;
			if ( !String.IsNullOrWhiteSpace( textBox.Text.Trim() ) ) {
				SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
				int rows = sql.NonQuery( @"	INSERT INTO [tblFeedbackEvents]  ( [FeedbackID], [FeedbackEventTypeID], [IsFromPlayer], [DateCreated], [Message], [NewStatusValue], [StaffMemberID] )
											VALUES ( ( SELECT FeedbackID FROM [tblFeedbackRequests] WHERE [UID] = @UID ), 2, 0, GETDATE(), @Message, NULL, @UserID);",
										new SQLParamList()
											.Add( "@Message", textBox.Text.Trim() )
											.Add( "@UID", GUID )
											.Add( "@UserID", User.UserID ) );
				if ( !sql.HasError && rows == 1 ) {
					SurveyTools.SendFeedbackNotifications( Server, GUID, true );
					mm.SuccessMessage = "Reply sent!";
					textBox.Text = String.Empty;
					int feedbackStatus = sql.QueryScalarValue<int>( "SELECT [FeedbackStatusID] FROM [tblFeedbackRequests] WHERE [UID] = @UID", new SQLParamList().Add( "@UID", GUID ) );
					if ( feedbackStatus == (int)FeedbackStatus.Open ) {
						sql.ExecStoredProcedureDataTable( "spFeedback_ChangeStatus",
															new SQLParamList()
																.Add( "@UID", GUID )
																.Add( "@FeedbackStatusID", (int)FeedbackStatus.AwaitingGuestResponse )
																.Add( "@IsFromPlayer", false )
																.Add( "@UserID", User.UserID ) );
					}
				} else {
					mm.ErrorMessage = "Oops! It looks like something went wrong. Please check the timeline and verify that the message was added.";
				}
			} else {
				mm.ErrorMessage = "Please enter something into the message box.";
			}
		}

		private void MessageTimeline_OnAddNote( TextBox textBox, MessageManager mm, DropDownList intList ) {
			//Prevent refresh from re-submitting form data
			if ( Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString() ) {
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
			} else {
				//Catch refresh and wipe the value again
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
				textBox.Text = String.Empty;
				intList.SelectedIndex = 0;
				return;
			}
			MessageTimeline.NoteTabActive = true;
			if ( String.IsNullOrWhiteSpace( textBox.Text.Trim() ) ) {
				mm.ErrorMessage = "Please enter something into the note box.";
				return;
			}

			if ( intList.SelectedIndex == 0 ) {
				mm.ErrorMessage = "Please indicate how the guest was contacted.";
				return;
			}

			SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
			int rows = sql.NonQuery( @" INSERT INTO [tblFeedbackEvents] ( [FeedbackID], [FeedbackEventTypeID], [IsFromPlayer], [DateCreated], [Message], [NewStatusValue], [StaffMemberID], [StaffContact] )
										VALUES ( ( SELECT FeedbackID FROM [tblFeedbackRequests] WHERE [UID] = @UID ), 4, 0, GETDATE(), @Message, NULL, @UserID, @StaffContact);",
									new SQLParamList()
										.Add( "@Message", textBox.Text.Trim() )
										.Add( "@UID", GUID )
										.Add( "@UserID", User.UserID )
										.Add( "@StaffContact", intList.SelectedValue ) );
			if ( !sql.HasError && rows == 1 ) {
				mm.SuccessMessage = "Note added successfully!";
				textBox.Text = String.Empty;
				intList.SelectedIndex = 0;
			} else {
				mm.ErrorMessage = "Oops! It looks like something went wrong. Please check the timeline and verify that the note was added.";
			}
		}
		
		protected void Page_LoadComplete( object sender, EventArgs e ) {

			if ( !String.IsNullOrEmpty( GUID ) ) {
				SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
				SQLParamList sqlParams = new SQLParamList();
				sqlParams.Add( "@GUID", GUID );
				DataSet ds = sql.ExecStoredProcedureDataSet( "[spFeedback_GetItem]", sqlParams );
				if ( !sql.HasError ) {
					Data = ds;

					//Check permissions
					GCCPropertyShortCode sc = (GCCPropertyShortCode)Data.Tables[0].Rows[0]["PropertyID"].ToString().StringToInt( 1 );
					if (   ( User.Group == UserGroups.PropertyManagers && User.PropertyShortCode != sc && User.PropertyShortCode != GCCPropertyShortCode.None )
						|| ( User.Group == UserGroups.PropertyStaff && User.PropertyShortCode != sc )
					) {
						Data = null;
						TopMessage.ErrorMessage = "You do not have permission to view this piece of feedback.";
						TopMessage.TitleOverride = "Unauthorized";
						return;
					}

					
					MessageTimeline.Messages = ds.Tables[1];
					int feedbackStatus = Conversion.StringToInt( ds.Tables[0].Rows[0]["FeedbackStatusID"].ToString() );
					MessageTimeline.HideReplyBox = IssueIsClosed;
					MessageTimeline.PropertyShortCode = (GCCPropertyShortCode)ds.Tables[0].Rows[0]["PropertyID"].ToString().StringToInt(1);

					//Reset tier DDL item labels
					ddlTier.Items[0].Text = "Tier 1";
					ddlTier.Items[1].Text = "Tier 2";
					ddlTier.Items[2].Text = "Tier 3";

					//Set the selected values and append " (Current)" to the current one for this record
					ddlTier.SelectedValue = Data.Tables[0].Rows[0]["Tier"].ToString();
					hdnOldTier.Value = ddlTier.SelectedValue;
					ddlTier.SelectedItem.Text += " (Current)";

					//Add JS check to notify the user what happens on change to Tier 3
					btnSaveTier.OnClientClick = "var oldTier=" + ds.Tables[0].Rows[0]["Tier"] + ",newTier=parseInt($('#" + ddlTier.ClientID + "').val());if(oldTier!=newTier&&3==newTier&&!confirm('Are you sure you want to set this to Tier 3? This will cause a notification to be sent to the appropriate staff members.'))return false;";
					
					//Hide Invalid Button if Case Closed
					if (IssueIsClosed) {
						//Work in progress
						//btnShowInvalidModal.Visible = false;
					}
					
				}
			}
		}

		protected void btnReopen_OnClick( object sender, EventArgs e ) {
			//Prevent refresh from re-submitting form data
			if ( Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString() ) {
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
			} else {
				//Catch refresh and wipe the value again
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
				return;
			}
			SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
			sql.ExecStoredProcedureDataTable( "spFeedback_ChangeStatus",
												new SQLParamList()
													.Add( "@UID", GUID )
													.Add( "@FeedbackStatusID", 1 )
													.Add( "@IsFromPlayer", false )
													.Add( "@UserID", User.UserID ) );
			if ( sql.HasError ) {
				TopMessage.ErrorMessage = "Oops! It looks like something went wrong trying to change the status. Please try again. (EFB100)";
			}
		}
		protected void btnClose_OnClick( object sender, EventArgs e ) {
			//Prevent refresh from re-submitting form data
			if ( Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString() ) {
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
			} else {
				//Catch refresh and wipe the value again
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
				return;
			}
			SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
			sql.ExecStoredProcedureDataTable( "spFeedback_ChangeStatus",
												new SQLParamList()
													.Add( "@UID", GUID )
													.Add( "@FeedbackStatusID", Conversion.StringToInt( ddlCloseReason.SelectedValue, 3 ) )
													.Add( "@IsFromPlayer", false )
													.Add( "@UserID", User.UserID ) );
			if ( sql.HasError ) {
				TopMessage.ErrorMessage = "Oops! It looks like something went wrong trying to change the status. Please try again. (EFB110)";
			}
		}

		protected void btnInvalid_OnClick(object sender, EventArgs e)
		{
			//Prevent refresh from re-submitting form data
			if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
			{
				Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
			}
			else
			{
				//Catch refresh and wipe the value again
				Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
				return;
			}
			SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
			sql.ExecStoredProcedureDataTable("spFeedback_ChangeStatus",
												new SQLParamList()
													.Add("@UID", GUID)
													.Add("@FeedbackStatusID", Conversion.StringToInt(ddlCloseReason.SelectedValue, 3))
													.Add("@IsFromPlayer", false)
													.Add("@UserID", User.UserID));
			if (sql.HasError)
			{
				TopMessage.ErrorMessage = "Oops! It looks like something went wrong trying to change the status. Please try again. (EFB110)";
			}
		}

		protected void btnSaveTier_Click( object sender, EventArgs e ) {
			//Prevent refresh from re-submitting form data
			if ( Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString() ) {
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
			} else {
				//Catch refresh and wipe the value again
				Session["CheckRefresh"] = Server.UrlDecode( System.DateTime.Now.ToString() );
				return;
			}
			//Don't do anything if the tier hasn't changed
			if ( ddlTier.SelectedValue.Equals( hdnOldTier.Value ) ) {
				return;
			}
			//Update the tier
			SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
			sql.ExecStoredProcedureDataTable( "spFeedback_ChangeTier",
												new SQLParamList()
													.Add( "@UID", GUID )
													.Add( "@Tier", Conversion.StringToInt( ddlTier.SelectedValue, 1 ) )
													.Add( "@UserID", User.UserID ) );
			//Send tier 3 notifications 
			if ( ddlTier.SelectedValue.Equals( "3" ) ) {
				SQLParamList sqlParams = new SQLParamList();
				sqlParams.Add( "@GUID", GUID );
				DataSet ds = sql.ExecStoredProcedureDataSet( "[spFeedback_GetItem]", sqlParams );
				if ( sql.HasError ) {
					TopMessage.ErrorMessage = "Unable to load the feedback item.";
					return;
				}
				DataRow fr = ds.Tables[0].Rows[0]; //Feedback row
				DataRow sr = ds.Tables[2].Rows[0]; //Survey row
				GCCPropertyShortCode sc = (GCCPropertyShortCode)fr["PropertyID"].ToString().StringToInt( 1 );
				SurveyType st = (SurveyType)fr["SurveyTypeID"].ToString().StringToInt( 1 );

				//Get the GAG location
				string gagLocation = String.Empty;
				switch ( st ) {
					case SurveyType.GEI:
						gagLocation = sr["Q3"].ToString();
						break;
					case SurveyType.Feedback:
						gagLocation = sr["GAGProperty"].ToString();
						break;
				}
				if ( gagLocation.Length > 0 ) {
					gagLocation = " - " + gagLocation;
				}

				//Send the notification
				SurveyTools.SendNotifications( Server, sc, SurveyType.Feedback, NotificationReason.Tier3Alert,string.Empty, new {
					Date = DateTime.Now.ToString( "MMMM dd, yyyy" ),
					CasinoName = fr["CasinoName"].ToString() + gagLocation,
					FeedbackLink = GCCPortalUrl + "Admin/Feedback/" + fr["UID"].ToString(),
					SurveyLink = String.Format(GCCPortalUrl + "Display/{0}/{1}", st.ToString(), fr["RecordID"] )
				} );
			}
			if ( sql.HasError ) {
				TopMessage.ErrorMessage = "Oops! It looks like something went wrong trying to change the tier. Please try again. (EFB120)";
			}
		}
	}
}