using SharedClasses;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin
{
    public partial class UserEdit : AuthenticatedPage
    {
        protected const int ROWS_PER_PAGE = 20;
        protected DataTable Data = null;

        /// <summary>
        /// Gets the current listing page.
        /// </summary>
        public int UserID
        {
            get
            {
                object userid = Page.RouteData.Values["userid"];
                if (userid != null)
                {
                    return Conversion.StringToInt(userid.ToString(), 1);
                }
                else
                {
                    return 1;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC User Editor";
            Master.HideAllFilters = true;
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            SQLParamList sqlParams = new SQLParamList().Add("@UserID", UserID);
            DataTable dt = sql.ExecStoredProcedureDataTable("spAdmin_User_Get", sqlParams);
            if (!sql.HasError)
            {
                Data = dt;
                if (dt.Rows.Count > 0 && !IsPostBack)
                {
                    DataRow dr = dt.Rows[0];
                    txtFirstName.Text = dr["FirstName"].ToString();
                    txtLastName.Text = dr["LastName"].ToString();
                    txtEmail.Text = dr["Email"].ToString();
                    ddlProperty.SelectedValue = dr["PropertyID"].ToString();
                    ddlStatus.SelectedValue = String.IsNullOrWhiteSpace(dr["Active"].ToString()) ? "True" : dr["Active"].ToString();
                    ddlGroup.SelectedValue = dr["GroupID"].ToString();
                    ddlTimezone.SelectedValue = dr["Timezone"].ToString();
                }
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtFirstName.Text) || String.IsNullOrWhiteSpace(txtLastName.Text))
            {
                TopMessage.ErrorMessage = "Please fill in both the first and last name fields.";
                return;
            }
            if (!Validation.RegExCheck(txtEmail.Text, ValidationType.Email))
            {
                TopMessage.ErrorMessage = "Please enter a valid email address.";
                return;
            }

            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            DataTable dtEmail = sql.QueryDataTable("SELECT UserID FROM [tblCOM_Users] WHERE UserID != @UserID AND Email = @Email",
                                                    new SQLParamList().Add("@UserID", UserID).Add("@Email", txtEmail.Text));
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "There was an error trying to validate the email address. Please try again.";
                return;
            }
            else if (dtEmail.Rows.Count > 0)
            {
                TopMessage.ErrorMessage = "This email address is already used for another user. Please use a unique email address.";
                return;
            }

            StringBuilder changes = new StringBuilder();
            if (Data != null && Data.Rows.Count > 0)
            {
                DataRow dr = Data.Rows[0];
                if (!txtFirstName.Text.Equals(dr["FirstName"].ToString()))
                {
                    changes.AppendFormat("First Name: '{0}' > '{1}', ", dr["FirstName"].ToString(), txtFirstName.Text);
                }
                if (!txtLastName.Text.Equals(dr["LastName"].ToString()))
                {
                    changes.AppendFormat("Last Name: '{0}' > '{1}', ", dr["LastName"].ToString(), txtLastName.Text);
                }
                if (!txtEmail.Text.Equals(dr["Email"].ToString()))
                {
                    changes.AppendFormat("Email: '{0}' > '{1}', ", dr["Email"].ToString(), txtEmail.Text);
                }
                if (!ddlProperty.SelectedValue.Equals(dr["PropertyID"].ToString()))
                {
                    changes.AppendFormat("Property ID: '{0}' > '{1}', ", dr["PropertyID"].ToString(), ddlProperty.SelectedValue);
                }
                if (!ddlStatus.SelectedValue.Equals(String.IsNullOrWhiteSpace(dr["Active"].ToString()) ? "True" : dr["Active"].ToString()))
                {
                    changes.AppendFormat("Active: '{0}' > '{1}', ", String.IsNullOrWhiteSpace(dr["Active"].ToString()) ? "True" : dr["Active"].ToString(), ddlStatus.SelectedValue);
                }
                if (!ddlGroup.SelectedValue.Equals(dr["GroupID"].ToString()))
                {
                    changes.AppendFormat("Group ID: '{0}' > '{1}', ", dr["GroupID"].ToString(), ddlGroup.SelectedValue);
                }
                if (!ddlTimezone.SelectedValue.Equals(dr["Timezone"].ToString()))
                {
                    changes.AppendFormat("Timezone: '{0}' > '{1}', ", dr["Timezone"].ToString(), ddlTimezone.SelectedValue);
                }
                if (changes.Length > 0)
                {
                    changes.Remove(changes.Length - 2, 2);
                }
                else
                {
                    changes.Append("No changes made.");
                }
            }

            SQLParamList sqlParams = new SQLParamList()
                    .Add("@UserID", UserID)
                    .Add("@FirstName", txtFirstName.Text)
                    .Add("@LastName", txtLastName.Text)
                    .Add("@Email", txtEmail.Text)
                    .Add("@Active", ddlStatus.SelectedValue.Equals("True"))
                    .Add("@ModifiedUserID", User.UserID)
                    .Add("@Timezone", ddlTimezone.SelectedValue)
                    .Add("@GroupID", ddlGroup.SelectedValue)
                    .Add("@LastChanges", changes.ToString());
            if (ddlProperty.SelectedIndex == 0)
            {
                sqlParams.Add("@PropertyID", DBNull.Value);
            }
            else
            {
                sqlParams.Add("@PropertyID", ddlProperty.SelectedValue);
            }
            int rows = sql.NonQuery(@"
UPDATE [tblCOM_Users]
SET FirstName = @FirstName,
    LastName = @LastName,
    Email = @Email,
    PropertyID = @PropertyID,
    Active = @Active,
    Timezone = @Timezone,
    TSModified = GETDATE(),
    ModifiedUserID = @ModifiedUserID,
    LastChanges = @LastChanges
WHERE UserID = @UserID;

UPDATE [tblCOM_UserToGroups]
SET [GroupID] = @GroupID
WHERE UserID = @UserID;

INSERT INTO [tblCOM_UserChangeEvents] ( UserID, DateCreated, ChangesMade )
VALUES ( @UserID, GETDATE(), @LastChanges );
",
                         sqlParams);
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Unable to save the user. Something went wrong when connecting to the database. Please try again. (EUE100)";
            }
            else if (rows == 0)
            {
                TopMessage.ErrorMessage = "Unable to find this user. Please try again. (EUE101)";
            }
            else
            {
                TopMessage.SuccessMessage = "Successfully updated the user.";
            }
        }

        protected void btnSendPasswordReset_Click(object sender, EventArgs e)
        {
            if (Data.Rows.Count > 0)
            {
                PasswordResetCode result = ResetPassword.SendResetEmail(Data.Rows[0]["Email"].ToString());
                switch (result)
                {
                    case PasswordResetCode.Success: //Success
                        TopMessage.SuccessMessage = "A message was sent to the email address containing a link to reset the user's password.";
                        break;

                    case PasswordResetCode.InvalidEmail: //Invalid email
                        TopMessage.ErrorMessage = "Invalid email address.";
                        break;

                    case PasswordResetCode.GuidUpdateFailure: //Failed updating GUID
                        TopMessage.ErrorMessage = "Invalid user information.";
                        break;

                    case PasswordResetCode.CriticalError: //Critical error
                    default:
                        TopMessage.ErrorMessage = "It looks like there was an error sending the password reset email. Please try again.";
                        break;
                }
            }
            else
            {
                TopMessage.ErrorMessage = "Unable to verify the user's email address. Please try again. (EUE102)";
            }
        }
    }
}