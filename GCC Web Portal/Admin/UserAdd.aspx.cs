using SharedClasses;
using System;
using System.Data;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin
{
    public partial class UserAdd : AuthenticatedPage
    {
        protected const int ROWS_PER_PAGE = 20;
        protected DataTable Data = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "GCC Create User";
            Master.HideAllFilters = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
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

            if (String.IsNullOrWhiteSpace(txtPassword.Text) || String.IsNullOrWhiteSpace(txtPassword2.Text))
            {
                TopMessage.ErrorMessage = "Please fill in both password fields.";
                return;
            }

            if (txtPassword.Text != txtPassword2.Text)
            {
                TopMessage.ErrorMessage = "The password fields must match.";
                return;
            }

            if ((ddlGroup.SelectedValue.Equals("4") || ddlGroup.SelectedValue.Equals("5"))
                && ddlProperty.SelectedValue.Equals(String.Empty)
                )
            {
                TopMessage.ErrorMessage = "Please select a property for this user or change the group they're in to a group that is not property specific.";
                return;
            }

            SQLDatabase sql = new SQLDatabase();
            DataTable dtEmail = sql.QueryDataTable("SELECT UserID FROM [tblCOM_Users] WHERE Email = @Email",
                                                    new SQLParamList().Add("@Email", txtEmail.Text));
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

            System.Data.SqlClient.SqlParameter userIDParam = new System.Data.SqlClient.SqlParameter("@UserID", SqlDbType.Int);
            userIDParam.Direction = ParameterDirection.Output;
            SQLParamList sqlParams = new SQLParamList()
                    .Add("@Username", txtEmail.Text)
                    .Add("@Password", txtPassword2.Text)
                    .Add("@ClientID", 1)
                    .Add("@Email", txtEmail.Text)
                    .Add("@FirstName", txtFirstName.Text)
                    .Add("@LastName", txtLastName.Text)
                    .Add("@ExpirePassword", true)
                    .Add("@AddToGroup", ddlGroup.SelectedValue)
                    .Add("@Timezone", ddlTimezone.SelectedValue)
                    .Add("@CreateUserID", User.UserID)
                    .Add(userIDParam);
            if (ddlProperty.SelectedIndex != 0)
            {
                sqlParams.Add("@PropertyID", ddlProperty.SelectedValue);
            }
            sql.ExecStoredProcedureDataTable("spCOM_CreateNewUser", sqlParams);
            if (sql.HasError)
            {
                TopMessage.ErrorMessage = "Unable to add the user. Something went wrong when connecting to the database. Please try again. (EUE100)";
            }
            else
            {
                TopMessage.SuccessMessage = "Successfully added the user.";
                txtEmail.Text = String.Empty;
                txtFirstName.Text = String.Empty;
                txtLastName.Text = String.Empty;
                txtPassword.Text = String.Empty;
                txtPassword2.Text = String.Empty;
                ddlProperty.SelectedIndex = 0;
                ddlGroup.SelectedIndex = 0;
                ddlTimezone.SelectedIndex = 0;
            }
        }
    }
}