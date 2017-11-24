using SentimentAnalyzer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebsiteUtilities;

namespace SharedClasses {
	public class SurveyTools {

		/// <summary>
		/// Saves the value of a control into the session.
		/// </summary>
		/// <param name="control">The control to save the value of session.</param>
		public static void SaveValue<T>( ISurveyControl<T> control ) {
			if ( control is SurveyCheckBox ) {
				SaveValue( control as SurveyCheckBox );
			} else {
				SessionWrapper.Add( control.SessionKey, new SurveySessionControl<T>( control.GetValue() ) );
			}
		}

		/// <summary>
		/// Saves the value of a survey checkbox control. Should only be called when the control is visible.
		/// </summary>
		/// <param name="control"></param>
		public static void SaveValue( SurveyCheckBox control ) {
			//Get value from postback
			bool postValue = ( !String.IsNullOrEmpty( RequestVars.Post<string>( control.UniqueID, null ) ) );
			control.Checked = postValue;
			SessionWrapper.Add( control.SessionKey, new SurveySessionControl<bool>( postValue ) );
			//if ( control.Checked ) {
			//    if ( !SessionWrapper.Get( control.SessionKey, new SurveySessionControl<bool>( false ) ).Value || postValue ) {
			//        //If it wasn't checked already or it's the same one as before, check it.
			//        SessionWrapper.Add( control.SessionKey, new SurveySessionControl<bool>( true ) );
			//        control.Checked = true;
			//    } else {
			//        //Uncheck other radio buttons that may have been flagged as checked if we know that we've already found a checked one.
			//        SessionWrapper.Add( control.SessionKey, new SurveySessionControl<bool>( false ) );
			//        control.Checked = false;
			//    }
			//} else {
			//    SessionWrapper.Add( control.SessionKey, new SurveySessionControl<bool>( false ) );
			//    control.Checked = false;
			//}
			//SurveyRadioButton rad = control as SurveyRadioButton;
			//if ( rad != null ) {

			//    return;
			//}
			//SaveValue<bool>( control );

		}
		/// <summary>
		/// Checks a control with an int value for an answer.
		/// </summary>
		/// <param name="control">The control to check.</param>
		/// <param name="questionsAreMandatory">Whether questions are mandatory or not.</param>
		public static bool CheckForAnswer( ISurveyControl<int> control, bool questionsAreMandatory ) {
			if ( control.GetValue() == -1 && questionsAreMandatory ) {
				control.MessageManager.ErrorMessage = "Please select a value for the following question.";
				return false;
			}
			return true;
		}

		/// <summary>
		/// Checks a control with a string value for an answer.
		/// </summary>
		/// <param name="control">The control to check.</param>
		/// <param name="questionsAreMandatory">Whether questions are mandatory or not.</param>
		public static bool CheckForAnswer( ISurveyControl<string> control, bool questionsAreMandatory ) {
			if ( ( String.IsNullOrEmpty( control.GetValue() )
					|| control.GetValue().Trim().Length == 0 )
				&& questionsAreMandatory ) {
				control.MessageManager.ErrorMessage = "Please enter a value for the following question.";
				return false;
			}
			SurveyTextBox stb = control as SurveyTextBox;
			if ( stb != null ) {
				if (    stb.MaxLength > 0
					 && stb.GetValue().Length > stb.MaxLength ) {
					stb.MessageManager.ErrorMessage = String.Format( "The following answer cannot be longer than {0} character(s). You have entered {1} character(s).", stb.MaxLength, stb.GetValue().Length );
					return false;
				}
			}
			return true;
		}


		/// <summary>
		/// Gets a value from a control or from the session depending on the value of the getCurrentValue parameter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="control"></param>
		/// <param name="getCurrentValue"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T GetValue<T>( ISurveyControl<T> control, bool getCurrentValue, T defaultValue ) {
			if ( getCurrentValue ) {
				return control.GetValue();
			} else {
				var sval = SessionWrapper.Get<SurveySessionControl<T>>( control.SessionKey );
				if ( sval != null ) {
					return sval.Value;
				} else {
					return defaultValue;
				}
			}
		}

		public static void SaveRadioButtons( params SurveyRadioButton[] buttonsInGroup ) {
			bool foundNew = false;
			foreach ( SurveyRadioButton rad in buttonsInGroup ) {
				if ( rad.Checked && !SessionWrapper.Get( rad.SessionKey, new SurveySessionControl<bool>( false ) ).Value ) {
					foundNew = true;
				}
			}
			foreach ( SurveyRadioButton rad in buttonsInGroup ) {
				if ( rad.Checked ) {
					if ( !SessionWrapper.Get( rad.SessionKey, new SurveySessionControl<bool>( false ) ).Value || !foundNew ) {
						//If it wasn't checked already or it's the same one as before, check it.
						SessionWrapper.Add( rad.SessionKey, new SurveySessionControl<bool>( true ) );
						rad.Checked = true;
					} else if ( foundNew ) {
						//Uncheck other radio buttons that may have been flagged as checked if we know that we've already found a checked one.
						SessionWrapper.Add( rad.SessionKey, new SurveySessionControl<bool>( false ) );
						rad.Checked = false;
					}
				} else {
					SessionWrapper.Add( rad.SessionKey, new SurveySessionControl<bool>( false ) );
					rad.Checked = false;
				}

			}
		}

		public static void SendNotifications<T>( HttpServerUtility server, GCCPropertyShortCode property, SurveyType surveyType, NotificationReason reason,string Comments, T replacementModel )
			where T : class {
			SendNotifications( server, property, surveyType, reason,Comments, replacementModel, String.Empty );
		}

		public static void SendNotifications<T>(HttpServerUtility server, GCCPropertyShortCode property, SurveyType surveyType, NotificationReason reason, string Comments, T replacementModel, string emailAddress)
			where T : class {
			SendNotifications( server, property, surveyType, reason,Comments, replacementModel, emailAddress, String.Empty);
		}

		public static void SendNotifications<T>(HttpServerUtility server, GCCPropertyShortCode property, SurveyType surveyType, NotificationReason reason, string Comments, T replacementModel, string emailAddress, string subjectPrefix)
			where T : class
		{
			SendNotifications(server, property, surveyType, reason, Comments, replacementModel, emailAddress, subjectPrefix, -1);
		}

		//20170116 adding commentws for email notification
		//public static void SendNotifications<T>( HttpServerUtility server, GCCPropertyShortCode property, SurveyType surveyType, NotificationReason reason, T replacementModel, string emailAddress, string subjectPrefix )
		public static void SendNotifications<T>(HttpServerUtility server, GCCPropertyShortCode property, SurveyType surveyType, NotificationReason reason, string Comments, T replacementModel, string emailAddress, string subjectPrefix, int operationsArea)
			where T : class {
				
			string template = String.Empty;
			string title = String.Empty;
			string propertyName = PropertyTools.GetCasinoName( (int)property );
			if ( property == GCCPropertyShortCode.GAG ) {
				PropertyInfo nameProp = replacementModel.GetType().GetProperty( "CasinoName" );
				if ( nameProp != null ) {
					string name = nameProp.GetValue( replacementModel ) as string;
					if ( !String.IsNullOrWhiteSpace( name ) ) {
						propertyName = name;
					}
				}
			}
			switch ( surveyType ) {
				case SurveyType.GEI:
					if ( reason == NotificationReason.ThankYou ) {
						title = "Thank You For Your Feedback";
						template = "GEIThankYou";
					} else {
						template = "GEITemplate";
						title = String.Format( "{0}GEI Feedback Notification for {1} - {2}", subjectPrefix, propertyName, DateTime.Now.ToString( "MMMM dd, yyyy" ) );
					}
					break;
				case SurveyType.GEIProblemResolution:
					if ( reason == NotificationReason.ThankYou ) {
						title = "Thank You For Your Feedback";
						template = "GEIThankYou";
					} else {
						if (replacementModel.ToString().Contains("FeedbackCategory"))
						{
							template = "GEIFeedbackCategoryTemplate";
							title = String.Format("{0}GEI Feedback Category Notification for {1} - {2}", subjectPrefix, propertyName, DateTime.Now.ToString("MMMM dd, yyyy"));
						}
						else
						{
							template = "GEITemplate";
							title = String.Format("{0}GEI Problem Resolution Feedback Notification for {1} - {2}", subjectPrefix, propertyName, DateTime.Now.ToString("MMMM dd, yyyy"));
						}
					}
					break;
				case SurveyType.Hotel:
					if ( reason == NotificationReason.ThankYou ) {
						title = "Thank You For Your Feedback";
						template = "HotelThankYou";
					} else {
						template = "HotelTemplate";
						title = String.Format( "{0}Hotel Survey Notification - {1}", subjectPrefix, DateTime.Now.ToString( "MMMM dd, yyyy" ) );
					}
					break;
				case SurveyType.Feedback:
					if ( reason == NotificationReason.ThankYou ) {
						title = "Thank You For Your Feedback";
						template = "FeedbackThankYou";
					} else if ( reason == NotificationReason.Tier3Alert ) {
						title = String.Format( "{0}Tier 3 Alert for {1} - {2}", subjectPrefix, propertyName, DateTime.Now.ToString( "MMMM dd, yyyy" ) );
						template = "Tier3Alert";
					} else {
						template = "FeedbackTemplate";
						title = String.Format( "{0}Feedback Follow-up Notification for {1} - {2}", subjectPrefix, propertyName, DateTime.Now.ToString( "MMMM dd, yyyy" ) );
					}
					break;
				case SurveyType.Donation:
					template = "DonationTemplate";
					title = String.Format( "{0}Sponsorship / Donation Request Notification for {1} - {2}", subjectPrefix, propertyName, DateTime.Now.ToString( "MMMM dd, yyyy" ) );
					break;
			}
			if ( template.Equals( String.Empty ) ) { return; }
			MailMessage msg = null;
			try {
				string path = server.MapPath( "~/Content/notifications/" );
				msg = EmailManager.CreateEmailFromTemplate(
									Path.Combine( path, template + ".htm" ),
									Path.Combine( path, template + ".txt" ),
									replacementModel );
				PropertyInfo attachmentProp = replacementModel.GetType().GetProperty( "Attachments" );
				if ( attachmentProp != null ) {
					SurveyAttachmentDetails[] attachments = attachmentProp.GetValue( replacementModel ) as SurveyAttachmentDetails[];
					foreach ( SurveyAttachmentDetails att in attachments ) {
						LinkedResource lr = new LinkedResource( server.MapPath( att.Path ) );
						lr.ContentId = att.ContentID;
						msg.AlternateViews[0].LinkedResources.Add( lr );
					}
				}
				msg.From = new MailAddress( "no-reply@gcgamingsurvey.com" );
				msg.Subject = title;
				//Add high priority flag to tier 3 alerts
				if ( reason == NotificationReason.Tier3Alert ) {
					msg.Priority = MailPriority.High;
				}
				bool hasAddress = false;
				if ( !String.IsNullOrEmpty( emailAddress ) ) {
					msg.To.Add( emailAddress );
					hasAddress = true;
				} else {
					SQLDatabase sql = new SQLDatabase();
					DataTable dt = sql.QueryDataTable(@"
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
						//AND ( ( @OperationsAreaID < 0 AND psr.OperationsAreaID IS NULL ) OR psr.OperationsAreaID = @OperationsAreaID )
  new SQLParamList()
	.Add( "@PropertyID", (int)property )
	.Add( "@SurveyID", (int)surveyType)
	.Add( "@ReasonID", (int)reason)
	.Add( "@OperationsAreaID", operationsArea )
);
					if ( !sql.HasError && dt.Rows.Count > 0 ) {
						StringBuilder addrs = new StringBuilder();
						foreach ( DataRow dr in dt.Rows ) {
							switch ( dr["SendType"].ToString() ) {
								case "1":
									msg.To.Add( dr["Email"].ToString() );
									//201701 Testing Email error
									//msg.Bcc.Add("mchand@forumresearch.com");
									addrs.Append( dr["FirstName"].ToString() + " " + dr["LastName"].ToString() + " <" + dr["Email"].ToString() + ">" + "\n" );
									hasAddress = true;
									break;
								case "2":
									msg.CC.Add( dr["Email"].ToString() );
									//201701 Testing Email error
									//msg.Bcc.Add("mchand@forumresearch.com");
									//Colin requested that CC addresses not show on the call Aug 10,2015
									//addrs.Append( dr["FirstName"].ToString() + " " + dr["LastName"].ToString() + " <" + dr["Email"].ToString() + ">" + "\n" );
									hasAddress = true;
									break;
								case "3":
									msg.Bcc.Add( dr["Email"].ToString() );
									//201701 Testing Email error
								   // msg.Bcc.Add("mchand@forumresearch.com");
									hasAddress = true;
									break;
							}
						}
						using (StreamReader sr = new StreamReader(msg.AlternateViews[0].ContentStream)) {
							msg.AlternateViews[0] = AlternateView.CreateAlternateViewFromString(sr.ReadToEnd().Replace("{Recipients}", server.HtmlEncode(addrs.ToString()).Replace("\n", "<br />")).Replace("{Business}", server.HtmlEncode(reason.ToString()).Replace("\n", "<br />")).Replace("{Comments}", server.HtmlEncode(Comments.ToString()).Replace("\n", "<br />")), null, MediaTypeNames.Text.Html);
						}
						using ( StreamReader sr = new StreamReader( msg.AlternateViews[1].ContentStream ) ) {
							msg.AlternateViews[1] = AlternateView.CreateAlternateViewFromString(sr.ReadToEnd().Replace("{Recipients}", addrs.ToString()).Replace("{Business}", reason.ToString()).Replace("{Comments}", Comments.ToString()), null, MediaTypeNames.Text.Plain);
						}
					}
				}

				if ( hasAddress ) {
					msg.Send();
				}
			} catch ( Exception ex ) {

			} finally {
				if ( msg != null ) {
					msg.Dispose();
					msg = null;
				}
			}
		}

		public static void SendFeedbackNotifications( HttpServerUtility server, string feedbackUID, bool toGuest ) {
			SQLDatabase sql = new SQLDatabase();
			SQLParamList sqlParams = new SQLParamList().Add( "GUID", feedbackUID );
			DataSet ds = sql.ExecStoredProcedureDataSet( "spFeedback_GetItem", sqlParams );
			string GCCPortalUrl = ConfigurationManager.AppSettings["GCCPortalURL"].ToString();

			if ( !sql.HasError && ds.Tables[0].Rows.Count > 0 ) {
				DataRow fbkDR = ds.Tables[0].Rows[0];
				GCCPropertyShortCode property = (GCCPropertyShortCode)fbkDR["PropertyID"].ToString().StringToInt();
				SurveyType surveyType = (SurveyType)fbkDR["SurveyTypeID"].ToString().StringToInt();
				NotificationReason reason = (NotificationReason)fbkDR["ReasonID"].ToString().StringToInt();

				string emailAddress = String.Empty;
				if ( toGuest ) {
					if ( ds.Tables[2].Columns.Contains( "ContactEmail" ) ) {
						emailAddress = ds.Tables[2].Rows[0]["ContactEmail"].ToString();
					}
					if ( String.IsNullOrWhiteSpace( emailAddress ) && ds.Tables[2].Columns.Contains( "Email" ) ) {
						emailAddress = ds.Tables[2].Rows[0]["Email"].ToString();
					}
					if ( String.IsNullOrWhiteSpace( emailAddress ) && ds.Tables[2].Columns.Contains( "Q5Email" ) ) {
						emailAddress = ds.Tables[2].Rows[0]["Q5Email"].ToString();
					}
					if ( String.IsNullOrWhiteSpace( emailAddress ) ) {
						//Nothing to do
						return;
					}
				}

				string template = String.Empty;
				string title = String.Empty;
				object replacementModel;

				title = PropertyTools.GetCasinoName( (int)property ) + " - Feedback Reply Notification";
				if ( toGuest ) {
					template = "GuestFeedbackNotification";
					replacementModel = new {
						CasinoName = PropertyTools.GetCasinoName( (int)property ),
						Link = GCCPortalUrl + "F/" + feedbackUID,
						Attachments = new SurveyTools.SurveyAttachmentDetails[] {
									new SurveyTools.SurveyAttachmentDetails() { Path = "~/Images/headers/" + PropertyTools.GetCasinoHeaderImage( property ), ContentID = "HeaderImage" }
								}
					};
				} else {
					template = "StaffFeedbackNotification";
					replacementModel = new {
						Date = DateTime.Now.ToString( "yyyy-MM-dd" ),
						CasinoName = PropertyTools.GetCasinoName( (int)property ),
						Link = GCCPortalUrl + "Admin/Feedback/" + feedbackUID
					};
				}

				MailMessage msg = null;
				try {
					string path = server.MapPath( "~/Content/notifications/" );
					msg = EmailManager.CreateEmailFromTemplate(
										Path.Combine( path, template + ".htm" ),
										Path.Combine( path, template + ".txt" ),
										replacementModel );
					PropertyInfo attachmentProp = replacementModel.GetType().GetProperty( "Attachments" );
					if ( attachmentProp != null ) {
						SurveyAttachmentDetails[] attachments = attachmentProp.GetValue( replacementModel ) as SurveyAttachmentDetails[];
						foreach ( SurveyAttachmentDetails att in attachments ) {
							LinkedResource lr = new LinkedResource( server.MapPath( att.Path ) );
							lr.ContentId = att.ContentID;
							msg.AlternateViews[0].LinkedResources.Add( lr );
						}
					}
					msg.From = new MailAddress( "no-reply@gcgamingsurvey.com" );
					msg.Subject = title;
					bool hasAddress = false;
					if ( !String.IsNullOrWhiteSpace( emailAddress ) ) {
						msg.To.Add( emailAddress );
						hasAddress = true;
					} else {
						sql = new SQLDatabase();
						DataTable dt = sql.QueryDataTable( @"
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
		.Add( "@PropertyID", (int)property )
		.Add( "@SurveyID", (int)surveyType )
		.Add( "@ReasonID", (int)reason )
	);
						if ( !sql.HasError && dt.Rows.Count > 0 ) {
							StringBuilder addrs = new StringBuilder();
							foreach ( DataRow dr in dt.Rows ) {
								switch ( dr["SendType"].ToString() ) {
									case "1":
										msg.To.Add( dr["Email"].ToString() );
										addrs.Append( dr["FirstName"].ToString() + " " + dr["LastName"].ToString() + " <" + dr["Email"].ToString() + ">" + "\n" );
										hasAddress = true;
										break;
									case "2":
										msg.CC.Add( dr["Email"].ToString() );
										//Colin requested that CC addresses not show on the call Aug 10,2015
										//addrs.Append( dr["FirstName"].ToString() + " " + dr["LastName"].ToString() + " <" + dr["Email"].ToString() + ">" + "\n" );
										hasAddress = true;
										break;
									case "3":
										msg.Bcc.Add( dr["Email"].ToString() );
										hasAddress = true;
										break;
								}
							}
							using (StreamReader sr = new StreamReader(msg.AlternateViews[0].ContentStream))
							{
								msg.AlternateViews[0] = AlternateView.CreateAlternateViewFromString(sr.ReadToEnd().Replace("{Recipients}", server.HtmlEncode(addrs.ToString()).Replace("\n", "<br />")), null, MediaTypeNames.Text.Html);
							}
							using (StreamReader sr = new StreamReader(msg.AlternateViews[1].ContentStream))
							{
								msg.AlternateViews[1] = AlternateView.CreateAlternateViewFromString(sr.ReadToEnd().Replace("{Recipients}", addrs.ToString()), null, MediaTypeNames.Text.Plain);
							}



							//using (StreamReader sr = new StreamReader(msg.AlternateViews[0].ContentStream))
							//{
							//    msg.AlternateViews[0] = AlternateView.CreateAlternateViewFromString(sr.ReadToEnd().Replace("{Recipients}", server.HtmlEncode(addrs.ToString()).Replace("\n", "<br />")).Replace("{Business}", server.HtmlEncode(reason.ToString()).Replace("\n", "<br />")).Replace("{Comments}", server.HtmlEncode(Comments.ToString()).Replace("\n", "<br />")), null, MediaTypeNames.Text.Html);
							//}
							//using (StreamReader sr = new StreamReader(msg.AlternateViews[1].ContentStream))
							//{
							//    msg.AlternateViews[1] = AlternateView.CreateAlternateViewFromString(sr.ReadToEnd().Replace("{Recipients}", addrs.ToString()).Replace("{Business}", reason.ToString()).Replace("{Comments}", Comments.ToString()), null, MediaTypeNames.Text.Plain);
							//}
						}
					}

					if ( hasAddress ) {
						msg.Send();
					}
				} catch ( Exception ex ) {

				} finally {
					if ( msg != null ) {
						msg.Dispose();
						msg = null;
					}
				}
			}
		}

		public struct SurveyAttachmentDetails {
			public string Path { get; set; }
			public string ContentID { get; set; }
		}

		/// <summary>
		/// Gets the count of words that are longer than three characters in the passed strings and returns a dictionary object with the counts of each.
		/// </summary>
		/// <param name="stringToCheck">The string to break and check.</param>
		public static Dictionary<string, int> GetWordCount( params string[] stringsToCheck ) {
			Dictionary<string, int> wordDictionary = new Dictionary<string,int>();
			StringBuilder currentWord = new StringBuilder();
			foreach ( string stringToCheck in stringsToCheck ) {
				for ( int i = 1; i < stringToCheck.Length; i++ ) {
					if ( !Char.IsLetter( stringToCheck[i] )
						&& ( !stringToCheck[i].Equals( '-' ) || ( i != 0 && stringToCheck[i - 1].Equals( '-' ) ) ) //Allow dashes but not multiple
						&& !stringToCheck[i].Equals( '\'' ) ) { //Allow single quotes
						if ( currentWord.Length > 3 ) {
							AddWordToDictionary( wordDictionary, currentWord.ToString().ToLower() );
						}
						currentWord.Clear();
					} else {
						currentWord.Append( stringToCheck[i] );
					}
				}
				if ( currentWord.Length > 3 ) {
					AddWordToDictionary( wordDictionary, currentWord.ToString().ToLower() );
				}
			}
			return wordDictionary;
		}

		private static void AddWordToDictionary( Dictionary<string, int> wordDictionary, string word ) {
			if ( wordDictionary.ContainsKey( word ) ) {
				wordDictionary[word]++;
			} else {
				wordDictionary.Add( word, 1 );
			}

		}

		public static void SaveWordCounts( SurveyType surveyType, int recordID, Dictionary<string, int> wordCounts ) {
			SQLDatabase sql = new SQLDatabase();
			StringBuilder query = new StringBuilder();
			SQLParamList sqlParams = new SQLParamList();
			int i = 1;
			sqlParams.Add( "@RecordID", recordID );
			foreach ( string key in wordCounts.Keys ) {
				i += 2;
				query.AppendFormat( "INSERT INTO [tblSurveyWords] VALUES ({0}, @RecordID, @Word{1}, @Count{1});\n", (int)surveyType, i );
				sqlParams.Add( "@Word" + i, key ).Add( "@Count" + i, wordCounts[key] );
				if ( i >= 2097 ) {
					//Only ~2100 parameters are allowed
					query.Append( "DELETE FROM [tblSurveyWords] WHERE Word IN (SELECT [Word] FROM [tblSurveyWords_Banned]);" );
					sql.NonQuery( query.ToString(), sqlParams );
					i = 1;
					sqlParams = new SQLParamList().Add( "@RecordID", recordID );
				}
			}
			if ( i > 1 ) {
				query.Append( "DELETE FROM [tblSurveyWords] WHERE Word IN (SELECT [Word] FROM [tblSurveyWords_Banned]);" );
				sql.NonQuery( query.ToString(), sqlParams );
			}
			if ( sql.HasError ) {
				ErrorHandler.WriteLog( "SharedClasses.SurveyTools.SaveWordCounts", String.Format( "Unable to save word counts for {0} record ID {1}", surveyType.ToString(), recordID ), ErrorHandler.ErrorEventID.SQLError, sql.ExceptionList[0] );
			}
		}

		public static double GetSentimentScore( string text ) {
			var vals = Classifier.Classify( text );
			double positive = vals["Positive"];
			double negative = vals["Negative"];
			return positive - negative;
		}
	}
}
