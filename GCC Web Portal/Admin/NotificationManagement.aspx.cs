using SharedClasses;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin
{
    public partial class NotificationManagement : AuthenticatedPage
    {
        protected DataTable Data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.HideAllFilters = true;
            if (RequestVars.Get("a", 0) == 1)
            {
                Response.Clear();
                switch (RequestVars.Get("t", 0))
                {
                    case 1: //Remove
                        int propSurvReaID = RequestVars.Post("p", -1);
                        int userID = RequestVars.Post("u", -1);
                        int sendType = RequestVars.Post("s", -1);
                        if (propSurvReaID != -1 && userID != -1 && sendType != -1)
                        {
                            SQLDatabase sql = new SQLDatabase();
                            int rows = sql.NonQuery(
                                @"DELETE FROM [tblNotificationUsers] WHERE [PropertySurveyReasonID] = @PropertySurveyReasonID AND [UserID] = @UserID AND [SendType] = @SendType",
                                new SQLParamList().Add("@PropertySurveyReasonID", propSurvReaID)
                                                  .Add("@UserID", userID)
                                                  .Add("@SendType", sendType)
                                );
                            if (!sql.HasError)
                            {
                                if (rows != 0)
                                {
                                    Response.Write(new JSONBuilder().AddInt("s", 0));
                                }
                                else
                                {
                                    Response.Write(new JSONBuilder().AddInt("s", 4).AddString("msg", "No matching records found. Refresh the page to see the most up to date information."));
                                }
                            }
                            else
                            {
                                Response.Write(new JSONBuilder().AddInt("s", 3).AddString("msg", "There was a problem contacting the database. Please try again. (ENM105)"));
                            }
                        }
                        else
                        {
                            Response.Write(new JSONBuilder().AddInt("s", 2).AddString("msg", "Invalid values specified."));
                        }
                        break;

                    case 2: //Get user list
                        propSurvReaID = RequestVars.Get("p", -1);
                        if (propSurvReaID != -1)
                        {
                            SQLDatabase sql = new SQLDatabase();
                            DataTable dt = sql.QueryDataTable(@"
SELECT [UserID],[FirstName],[LastName]
FROM [tblCOM_Users]
WHERE [UserID] NOT IN (SELECT [UserID] FROM [tblNotificationUsers] WHERE [PropertySurveyReasonID] = @PSRID )
    AND [Active] = 1
ORDER BY [FirstName], [LastName]",
                                    new SqlParameter("@PSRID", propSurvReaID));

                            if (!sql.HasError)
                            {
                                JSONBuilder jb = new JSONBuilder().AddInt("s", 0);
                                jb.AddArray("ubh");
                                foreach (DataRow dr in dt.Rows)
                                {
                                    jb.AddObject()
                                      .AddInt("i", (int)dr["UserID"])
                                      .AddString("n", dr["FirstName"].ToString() + " " + dr["LastName"].ToString())
                                      .CloseObject();
                                }
                                jb.CloseArray();
                                Response.Write(jb);
                            }
                            else
                            {
                                Response.Write(new JSONBuilder().AddInt("s", 3).AddString("msg", "There was a problem contacting the database. Please try again. (ENM106)"));
                            }
                        }
                        else
                        {
                            Response.Write(new JSONBuilder().AddInt("s", 2).AddString("msg", "Invalid values specified."));
                        }
                        break;

                    default:
                        Response.Write(new JSONBuilder().AddInt("s", 1).AddString("msg", "Invalid type specified."));
                        break;
                }
                Response.End();
                return;
            }

            if (!IsPostBack)
            {
                //Hide reason by default. Can only be shown if Property and Survey are changed.
                ddlReason.Visible = false;
            }
            else
            {
                lblReasonError.Text = String.Empty;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            SQLDatabase sql = new SQLDatabase();

            SQLParamList sqlParams = new SQLParamList();
            if (ddlProperty.SelectedIndex == 0)
            {
                sqlParams.Add("@PropertyID", DBNull.Value);
            }
            else
            {
                sqlParams.Add("@PropertyID", ddlProperty.SelectedValue);
            }
            if (ddlSurvey.SelectedIndex == 0)
            {
                sqlParams.Add("@SurveyTypeID", DBNull.Value);
            }
            else
            {
                sqlParams.Add("@SurveyTypeID", ddlSurvey.SelectedValue);
            }
            if (ddlReason.SelectedIndex == 0)
            {
                sqlParams.Add("@ReasonID", DBNull.Value);
            }
            else
            {
                sqlParams.Add("@ReasonID", ddlReason.SelectedValue);
            }

            DataTable dt = sql.QueryDataTable(@"
SELECT psr.[PropertySurveyReasonID]
	  ,psr.PropertyID
	  ,psr.SurveyTypeID
	  ,psr.ReasonID
	  ,p.Name AS [PropertyName]
	  ,st.SurveyName
	  ,nr.ReasonDescription
      ,nu.[UserID]
	  ,u.FirstName
	  ,u.LastName
      ,nu.[SendType]
FROM [tblNotificationPropertySurveyReason] psr
	LEFT JOIN [tblProperties] p
		ON psr.PropertyID = p.PropertyID
	LEFT JOIN [tblSurveyTypes] st
		ON psr.SurveyTypeID = st.SurveyTypeID
	LEFT JOIN [tblNotificationReasons] nr
		ON psr.ReasonID = nr.ReasonID
	LEFT JOIN [tblNotificationUsers] nu
		ON nu.PropertySurveyReasonID = psr.PropertySurveyReasonID
	LEFT JOIN [tblCOM_Users] u
		ON nu.UserID = u.UserID
WHERE   (psr.PropertyID = @PropertyID OR @PropertyID IS NULL)
	AND (psr.SurveyTypeID = @SurveyTypeID OR @SurveyTypeID IS NULL)
	AND (psr.ReasonID = @ReasonID OR @ReasonID IS NULL)
ORDER BY [PropertyName], psr.[SurveyTypeID], [ReasonDescription], [SendType], u.[FirstName]",
                sqlParams
            );
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Oops. Something went wrong when loading the data. Please try again. (ENM101)";
            }
            else
            {
                Data = dt;
            }
        }

        protected void PropertySurveyFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProperty.SelectedIndex > 0 && ddlSurvey.SelectedIndex > 0)
            {
                string selectedValue = ddlReason.SelectedValue;
                SQLDatabase sql = new SQLDatabase();
                DataTable dt = sql.QueryDataTable(@"
SELECT nr.[ReasonID], nr.[ReasonDescription]
FROM [tblNotificationPropertySurveyReason] psr
    LEFT JOIN [tblNotificationReasons] nr
        ON psr.[ReasonID] = nr.[ReasonID]
WHERE psr.[PropertyID] = @PropertyID
    AND psr.[SurveyTypeID] = @SurveyTypeID
ORDER BY nr.[ReasonDescription]",
                    new SQLParamList()
                            .Add("@PropertyID", ddlProperty.SelectedValue)
                            .Add("@SurveyTypeID", ddlSurvey.SelectedValue)
                );
                if (sql.HasError)
                {
                    TopMessage.ErrorMessage = "Oops! There was an error loading the filters for this Property / Survey combination. Please try again. (ENM100)";
                    ddlReason.Visible = false;
                }
                else
                {
                    if (dt.Rows.Count == 0)
                    {
                        lblReasonError.Text = "No associated reasons found. This combination is not possible. (ENM102)";
                        ddlReason.Visible = false;
                    }
                    else
                    {
                        ddlReason.Items.Clear();
                        ddlReason.Items.Add(new ListItem("All", String.Empty));
                        foreach (DataRow dr in dt.Rows)
                        {
                            ListItem li = new ListItem(dr["ReasonDescription"].ToString(), dr["ReasonID"].ToString());
                            li.Selected = li.Value.Equals(selectedValue);
                            ddlReason.Items.Add(li);
                        }
                        ddlReason.Visible = true;
                    }
                }
            }
            else
            {
                ddlReason.Visible = false;
            }
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            //Do stuff
            int selectedUserID = Request.Form[ddlUserSearch.UniqueID].StringToInt(-1);
            int selectedType = ddlSendType.SelectedValue.StringToInt(1, 3, -1);
            int propSurvReaID = hdnPSRID.Value.StringToInt(-1);
            if (selectedUserID != -1 && selectedType != -1 && propSurvReaID != -1)
            {
                SQLDatabase sql = new SQLDatabase();
                int rows = sql.NonQuery(@"INSERT INTO [tblNotificationUsers] VALUES (@PSRID, @UserID, @SendType);",
                                         new SQLParamList()
                                                .Add("@PSRID", propSurvReaID)
                                                .Add("@UserID", selectedUserID)
                                                .Add("@SendType", selectedType)
                                       );
                if (sql.HasError || rows != 1)
                {
                    TopMessage.ErrorMessage = "Unable to add the user. There was an issue connecting to the database. Please try again. (ENM103)";
                }
                else
                {
                    TopMessage.SuccessMessage = "Notification updated successfully!";
                    hdnPSRID.Value = "";
                    ddlSendType.SelectedIndex = 0;
                }
            }
            else
            {
                TopMessage.ErrorMessage = "Unable to add the user. Invalid parameters were sent. Please try again. (ENM104)";
            }
        }
    }
}