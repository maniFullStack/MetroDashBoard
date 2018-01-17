using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsiteUtilities;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace SharedClasses {
    public class UserInfo : UserInformation
    {

        public GCCProperty Property { get; set; }
        /// <summary>
        /// The user's timezone (from TimeZoneInfo.GetSystemTimeZones())
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// The user's group.
        /// </summary>
        public UserGroups Group { get; set; }
            

        public GCCPropertyShortCode PropertyShortCode {
            get {
                return (GCCPropertyShortCode)( (int)Property );
            }
        }

        protected override void Load( int userID ) {
            base.Load( userID );
            LoadExtraData();
        }

        protected override void Load( string guid ) {
            base.Load( guid );
            LoadExtraData();
        }

        protected override void Load( string usernameOrEmail, string password, bool useEmailForLogin, int clientID, out int outputValue ) {
            base.Load( usernameOrEmail, password, useEmailForLogin, clientID, out outputValue );
            LoadExtraData();
        }

        private void LoadExtraData() {
            if ( UserID > -1 ) {
                SQLDatabase sql = new SQLDatabase();
                DataTable dt = sql.QueryDataTable( "SELECT PropertyID, Timezone, (SELECT TOP 1 GroupID FROM [tblCOM_UserToGroups] WHERE UserID = @UserID) AS [GroupID] FROM tblCOM_Users WHERE UserID = @UserID", new SqlParameter( "@UserID", UserID ) );
                Property = (GCCProperty)dt.Rows[0]["PropertyID"].ToString().StringToInt();

                

                
                Group = (UserGroups)dt.Rows[0]["GroupID"].ToString().StringToInt();
                Timezone = dt.Rows[0]["Timezone"].ToString();
                if ( String.IsNullOrWhiteSpace( Timezone ) ) {
                    Timezone = "Pacific Standard Time";
                }
            }
        }

        public bool UpdatePassword(string newPassword, out string errorMessage)
        {
            if (!IsPasswordValid(newPassword, out errorMessage))
            {
                return false;
            }
            if (!ValidateUser(out errorMessage))
            {
                return false;
            }

            SQLDatabaseReporting sql = new SQLDatabaseReporting();
            SqlParameter affectedRows;
            SQLParamList sqlParams = new SQLParamList()
                .Add("@UserID", UserID)
                .Add("@NewPassword", newPassword)
                .AddOutputParam("@AffectedRows", 4, out affectedRows);

            sql.ExecStoredProcedureDataTable("spCOM_UpdatePassword", sqlParams);

            if (sql.HasError)
            {
                errorMessage = "There was a database level error while attempting to save this user. Please contact the administrator if this error persists.";
                return false;
            }
            else if (Conversion.StringToInt(affectedRows.Value.ToString(), 0) == 0)
            {
                errorMessage = "Invalid user specified. Unable to change password.";
                return false;
            }

            return true;
        }   
        
        public static string ResetPassword(string email)
        {
            SqlParameter rowsUpdated;
            SqlParameter output;
            SQLDatabaseReporting sql = new SQLDatabaseReporting();
            SQLParamList sqlParams = new SQLParamList()
                .Add("@Email", email)
                .Add("@ClientID", Config.ClientID)
                .AddOutputParam("@OutputCode", 4, out output)
                .AddOutputParam("@RowCount", 4, out rowsUpdated);

            DataTable dt = sql.ExecStoredProcedureDataTable("spCOM_PasswordReset", sqlParams);
            if (sql.HasError || Int32.Parse(rowsUpdated.Value.ToString()) == 0)
            {
                return null;
            }
            return dt.Rows[0]["GUID"].ToString();
        }

        private bool IsPasswordValid(string password, out string message)
        {
            message = "";

            if (string.IsNullOrEmpty(password))
            {
                message = "No password was entered.";
                return false;
            }

            var isValid = (!String.IsNullOrEmpty(password) && password.Length >= 6 && password.Length <= 20);

            if (!isValid)
            {
                message = "Password must be between 6 and 20 characters in length.";
                return false;
            }

            int numbers = (Regex.Matches(password, @"[0-9]")).Count;
            int uppers = (Regex.Matches(password, @"[A-Z]")).Count;
            int lowers = (Regex.Matches(password, @"[a-z]")).Count;

            bool complexityMet =
                numbers > 0 &&
                uppers > 0 &&
                lowers > 0;

            if (!complexityMet)
            {
                message = "Password complexity requirements not met. Password must have at least one uppercase letter, one lowercase letter and one number.";
                return false;
            }

            return isValid;
        }

        private bool ValidateUser(out string message)
        {
            message = null;

            if (UserID < -1 || UserID == 0) message = "Invalid user information.";
            if (String.IsNullOrEmpty(Username)) message = "No username was specified. ";
            if (Username.Length < 4 || Username.Length > 150)
                message = "The email must be between 4 and 150 characters.";
            if (!Validation.RegExCheck(Email, ValidationType.Email))
                message = "Invalid email address.";
            return (message == null);
        }

        /// <summary>
        ///     Clears Password History for specified userId
        /// </summary>
        /// <returns></returns>
        public int ClearLoginAttempts() {
            SQLDatabaseReporting sql = new SQLDatabaseReporting();
            int rows = sql.NonQuery(
                "DELETE FROM dbo.tblCOM_UserLoginAttempts WHERE UserID = @UserID",
                new SqlParameter( "@UserID", UserID ) );
            return rows;
        }
    }
}
