using GCC_Web_Portal.Admin;
using SharedClasses;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class GuestFeedback : BasePage
    {
        protected DataSet Data { get; set; }

        /// <summary>
        /// Gets the GUID for the current request.
        /// </summary>
        public string GUID
        {
            get
            {
                object property = Page.RouteData.Values["guid"];
                if (property != null)
                {
                    return property.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// Gets whether or not the current issue is closed. Should only be referenced after the LoadComplete event.
        /// </summary>
        public bool IssueIsClosed
        {
            get
            {
                int feedbackStatus = Conversion.StringToInt(Data.Tables[0].Rows[0]["FeedbackStatusID"].ToString());
                return (feedbackStatus == (int)FeedbackStatus.ClosedGuestResponseComplete
                        || feedbackStatus == (int)FeedbackStatus.ClosedNoFurtherActionRequired
                        || feedbackStatus == (int)FeedbackStatus.ClosedNoResponse
                        || feedbackStatus == (int)FeedbackStatus.ClosedUnabletoSatisfyGuest);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Title = "Feedback";
            MessageTimeline.OnReply += MessageTimeline_OnReply;
            MessageTimeline.IsGuestVersion = true;
            if (IsPostBack)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#end-of-messages';", true);
            }
            else
            {
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }

        private void MessageTimeline_OnReply(TextBox textBox, MessageManager mm)
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
                textBox.Text = String.Empty;
                return;
            }
            if (!String.IsNullOrEmpty(textBox.Text.Trim()))
            {
                SQLDatabase sql = new SQLDatabase();
                int rows = sql.NonQuery(@"	INSERT INTO [tblFeedbackEvents] ( [FeedbackID], [FeedbackEventTypeID], [IsFromPlayer], [DateCreated], [Message], [NewStatusValue] )
											VALUES ( ( SELECT FeedbackID FROM [tblFeedbackRequests] WHERE [UID] = @UID ), 3, 1, GETDATE(), @Message, NULL );",
                                        new SQLParamList()
                                            .Add("@Message", textBox.Text)
                                            .Add("@UID", GUID));
                if (!sql.HasError && rows == 1)
                {
                    SurveyTools.SendFeedbackNotifications(Server, GUID, false);
                    mm.SuccessMessage = "Thank you. Your reply was sent successfully.";
                    textBox.Text = String.Empty;
                    int feedbackStatus = sql.QueryScalarValue<int>("SELECT [FeedbackStatusID] FROM [tblFeedbackRequests] WHERE [UID] = @UID", new SQLParamList().Add("@UID", GUID));
                    if (feedbackStatus != (int)FeedbackStatus.Open)
                    {
                        sql.ExecStoredProcedureDataTable("spFeedback_ChangeStatus",
                                                            new SQLParamList()
                                                                .Add("@UID", GUID)
                                                                .Add("@FeedbackStatusID", (int)FeedbackStatus.Open)
                                                                .Add("@IsFromPlayer", true));
                    }
                }
                else
                {
                    mm.ErrorMessage = "Oops! It looks like something went wrong. Please check the timeline and verify that your message was added.";
                }
            }
            else
            {
                mm.ErrorMessage = "Please enter something into the message box.";
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(GUID))
            {
                SQLDatabase sql = new SQLDatabase();
                SQLParamList sqlParams = new SQLParamList();
                UserInfo ui = SessionWrapper.Get<UserInfo>("UserInfo");
                sqlParams.Add("@GUID", GUID)
                         .Add("@UpdateLastViewedTime", !(ui != null || RequestVars.Get<string>("a", null) != null));
                DataSet ds = sql.ExecStoredProcedureDataSet("[spFeedback_GetGuestItem]", sqlParams);
                if (!sql.HasError)
                {
                    Data = ds;

                    GCCPropertyShortCode sc = (GCCPropertyShortCode)Conversion.StringToInt(ds.Tables[0].Rows[0]["PropertyID"].ToString(), 1);

                    Master.ForceSpecificProperty = sc;
                    MessageTimeline.PropertyShortCode = sc;

                    MessageTimeline.Messages = ds.Tables[1];
                    int feedbackStatus = Conversion.StringToInt(ds.Tables[0].Rows[0]["FeedbackStatusID"].ToString());
                    MessageTimeline.HideReplyBox = ui != null || RequestVars.Get<string>("a", null) != null;

                    Title = PropertyTools.GetCasinoName((int)sc) + " Feedback";
                }
            }
        }
    }
}