#region Using...

using System;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace WebsiteUtilities
{
    /// <summary>
    ///     This class is used as a base for UserInformation across websites.
    /// </summary>
    public class UserInformation
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the user's username.
        /// </summary>
        public string Username { get; set; }

        public int GroupID {get;set;}

        /// <summary>
        ///     Gets or sets the user ID value from the database.
        /// </summary>
        public int UserID { get; protected set; }

        /// <summary>
        ///     Gets or sets the user's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Gets or sets the user's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Gets the full name of the user in the format "&lt;FirstName&gt; &lt;LastName&gt;".
        /// </summary>
        public virtual string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        /// <summary>
        ///     Gets the full name of the user in the format "&lt;LastName&gt;, &lt;FirstName&gt;".
        /// </summary>
        public virtual string FullNameReverse
        {
            get { return LastName + ", " + FirstName; }
        }

        /// <summary>
        ///     Gets or sets the user's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     The last time the user was logged in.
        /// </summary>
        public DateTime LastLoginDate { get; set; }

        /// <summary>
        ///     The date the user's password expires.
        /// </summary>
        public DateTime PasswordExpireDate { get; set; }

        /// <summary>
        ///     The date the user reset their password.
        /// </summary>
        public DateTime PasswordResetDate { get; set; }

        /// <summary>
        ///     The amount of times the user has logged in.
        /// </summary>
        public int LoginCount { get; protected set; }

        /// <summary>
        ///     The date the user was created.
        /// </summary>
        public DateTime CreationDate { get; protected set; }

        /// <summary>
        ///     The last time this user was modified.
        /// </summary>
        public DateTime ModificationDate { get; set; }

        /// <summary>
        ///     The user who last modified this user.
        /// </summary>
        public int ModifiedUserID { get; set; }

        /// <summary>
        ///     The GUID of the user from the database.
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// Is The User In a Deleted / Inactive 
        /// </summary>
        public bool IsActive {get; set;}

        /// <summary>
        /// The first custom flag associated with this user/group
        /// </summary>
        public int CustomFlag1 { get; set; }
        /// <summary>
        /// The second custom flag associated with this user/group
        /// </summary>
        public int CustomFlag2 { get; set; }
        /// <summary>
        /// The first custom character string associated with this user/group
        /// </summary>
        public string CustomChar1 { get; set; }
        /// <summary>
        /// The second custom character string associated with this user/group
        /// </summary>
        public string CustomChar2 { get; set; }
        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Private constructor to prevent class from being initialized externally.
        /// </summary>
        protected UserInformation()
        {
        }

        #endregion Constructors

        #region Protected Methods

        #region Load Methods

        /// <summary>
        ///     Load method for user information class. If UserID = -1, the user failed to be loaded. Override this method to handle user logins.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email to check. Which one to pass depends on the useEmailForLogin parameter.</param>
        /// <param name="password">The password to try.</param>
        /// <param name="useEmailForLogin">If true, the usernameOrEmail value will be assumed to be an email.</param>
        /// <param name="clientID">The client ID of the login attempt.</param>
        /// <param name="outputValue">
        ///     <para>The value output from the query. Use this to generate response messages. Values are:</para>
        ///     <para>-1 - Unknown error. Something went wrong with the query.</para>
        ///     <para>0 - Success</para>
        ///     <para>1 - User is locked out.</para>
        ///     <para>2 - Login failed.</para>
        ///     <para>3 - No user or email was specified.</para>
        ///     <para>4 - User doesn't exist.</para>
        /// </param>
        protected virtual void Load(string usernameOrEmail, string password, bool useEmailForLogin, int clientID,
                                    out int outputValue)
        {
            outputValue = -1;
            //Set up the sql request
            SQLDatabaseReporting sql = new SQLDatabaseReporting();
            SQLParamList sqlParams = new SQLParamList();
            sqlParams.Add(useEmailForLogin ? "@Email" : "@Username", usernameOrEmail);

            SqlParameter outParam;

            sqlParams.Add("@Password", password)
                     .Add("@ClientID", clientID)
                     .Add("@IP", RequestVars.GetRequestIPv4Address())
                     .AddOutputParam("@OutputValue", 4, out outParam);

            //Try and get the user's info
            DataTable dt = sql.ExecStoredProcedureDataTable("spCOM_WebReportingLogon", sqlParams);
            
            if (!sql.HasError)
            {
                outputValue = Conversion.StringToInt(outParam.Value.ToString(), -1);
                if (outputValue == 0 && dt.Rows.Count > 0)
                {
                    //Success!
                    SetUserDataFromDataRow(dt.Rows[0]);
                    return;
                }
            }
            UserID = -1;
        }


        /// <summary>
        ///     Loads a user based on their ID. Override this method to handle extra user information.
        /// </summary>
        /// <param name="userID">The ID of the user to load.</param>
        protected virtual void Load(int userID)
        {
            SQLDatabaseReporting sql = new SQLDatabaseReporting();
            //Try and get the user's info
            DataTable dt = sql.ExecStoredProcedureDataTable("spCOM_LoadUserData", new SqlParameter("@UserID", userID));
            if (!sql.HasError)
            {
                if (dt.Rows.Count > 0)
                {
                    //Success!
                    SetUserDataFromDataRow(dt.Rows[0]);
                    return;
                }
            }
            UserID = -1;
        }

        /// <summary>
        ///     Loads a user based on their GUID string.
        /// </summary>
        /// <param name="guid">The GUID of the user to look up.</param>
        protected virtual void Load(string guid)
        {
            SQLDatabaseReporting sql = new SQLDatabaseReporting();
            //Try and get the user's info
            DataTable dt = sql.ExecStoredProcedureDataTable("spCOM_LoadUserData", new SqlParameter("@GUID", guid));
            if (!sql.HasError)
            {
                if (dt.Rows.Count > 0)
                {
                    //Success!
                    SetUserDataFromDataRow(dt.Rows[0]);
                    return;
                }
            }
            UserID = -1;
        }
       
        #endregion Load Methods

        #endregion Protected Methods

        #region Static Methods

        #region Load Methods

        /// <summary>
        ///     Virtual method used to attempt a user login. If the user could not be logged in, null will be returned. If the user is logged in, the object will be added to the session with the key "UserInfo". Requires "DatabaseReporting" connection string to be defined in web.config unless it's overridden.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email to check. Which one to pass depends on the useEmailForLogin parameter.</param>
        /// <param name="password">The password to try.</param>
        /// <param name="useEmailForLogin">If true, the usernameOrEmail value will be assumed to be an email.</param>
        /// <param name="clientID">The client ID of the login attempt.</param>
        /// <param name="outputValue">
        ///     <para>The value output from the query. Use this to generate response messages. Values are:</para>
        ///     <para>-1 - Unknown error. Something went wrong with the query.</para>
        ///     <para>0 - Success</para>
        ///     <para>1 - User is locked out.</para>
        ///     <para>2 - Login failed.</para>
        ///     <para>3 - No user or email was specified.</para>
        ///     <para>4 - User doesn't exist.</para>
        /// </param>
        /// <returns>Null if the user could not be logged in or the UserInformation object </returns>
        public static T LogUserIn<T>(string usernameOrEmail, string password, bool useEmailForLogin, int clientID,
                                     out int outputValue) where T : UserInformation, new()
        {
            T ui = new T();
            ui.Load(usernameOrEmail, password, useEmailForLogin, clientID, out outputValue);
            if (ui.UserID != -1)
            {
                SessionWrapper.Add<UserInformation>("UserInfo", ui);
                return ui;
            }
            return null;
        }

        /// <summary>
        ///     Gets a user's information from the database. Requires "DatabaseReporting" connection string to be defined in web.config unless it's overridden.
        /// </summary>
        /// <typeparam name="T">A type of UserInformation class (or, simply, UserInformation). Override this class to add customizations to the user.</typeparam>
        /// <param name="userID">The ID of the user in the database.</param>
        /// <returns>A UserInformation object with data from the passed user ID. If the ID is invalid or there was an error, returns null.</returns>
        public static T GetUser<T>(int userID) where T : UserInformation, new()
        {
            T ui = new T();
            ui.Load(userID);
            if (ui.UserID != -1)
            {
                return ui;
            }
            return null;
        }

        /// <summary>
        ///     Gets a user's information from the database. Requires "DatabaseReporting" connection string to be defined in web.config unless it's overridden.
        /// </summary>
        /// <typeparam name="T">A derived UserInformation class (or, simply, UserInformation). Override this class to add customizations to the user.</typeparam>
        /// <param name="guid">The user's GUID value.</param>
        /// <returns>A UserInformation object with data from the passed GUID. If the ID is invalid or there was an error, returns null.</returns>
        public static T GetUser<T>(string guid) where T : UserInformation, new()
        {
            T ui = new T();
            ui.Load(guid);
            if (ui.UserID != -1)
            {
                return ui;
            }
            return null;
        }

        /// <summary>
        /// Get User by username or email address 
        /// </summary>
        /// <param name="userNameOrEmail">Username or Email for user</param>
        /// <param name="clientId">Client Id user is associated to</param>
        /// <param name="isEmailSearch">(Optional: defaults to false if not specified) Search by email instead of username</param>
        /// <typeparam name="T">Child of UserInformation</typeparam>
        /// <returns>T</returns>
        public static T GetUser<T>(string userNameOrEmail, int clientId, bool isEmailSearch) where T : UserInformation, new()
        {

            string query = string.Format("SELECT [UserID],[Username],[ClientID],[Email] FROM [tblCOM_Users] WHERE {0} = @UserNameOrEmail AND ClientID = @ClientID", 

                isEmailSearch ? "[Email]" : "[Username]");
            
            SQLParamList paramList = new SQLParamList();
            paramList
                .Add("@ClientID", clientId)
                .Add("@UserNameOrEmail", userNameOrEmail);
          SQLDatabaseReporting sql = new SQLDatabaseReporting();
            
            var dt = sql.QueryDataTable(query, paramList);


            if (sql.HasError)
            {
                foreach (var ex in sql.ExceptionList)
                {
                    ErrorHandler.WriteLog(
                        typeof (UserInformation).FullName,
                        "Database Error Encountered",
                        ErrorHandler.ErrorEventID.SQLError, ex);
                }

                return null;
            }

            return dt.Rows.Count < 1 ? 
                null : 
                GetUser<T>(Conversion.StringToInt(dt.Rows[0]["UserID"].ToString(), -1));
        }

        /// <summary>
        /// Get User by username or email address 
        /// </summary>
        /// <param name="userNameOrEmail">Username or Email for user</param>
        /// <param name="clientId">Client Id user is associated to</param>
        /// <typeparam name="T">Child of UserInformation</typeparam>
        /// <returns>T</returns>
        public static T GetUser<T>(string userNameOrEmail, int clientId)
            where T : UserInformation, new()
        {
            return GetUser<T>(userNameOrEmail, clientId, false);
        }


        /// <summary>
        ///     Assgn new reset GUID to user and update password reset date
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="errorCode">
        ///     Action Response Code
        ///     Error Codes:
        ///     0 -- Unknown Error
        ///     1 -- Success
        ///     2 -- UserId not found
        ///     3 -- Something went horribly wrong(Multiple records affected)
        ///     4 -- UserId not provided
        ///     5 -- Critical Error
        /// </param>
        /// <returns></returns>
        public static string ResetPassword(int userId, out int errorCode)
        {
            SQLDatabaseReporting sql = new SQLDatabaseReporting();

            DataTable dt = sql.ExecStoredProcedureDataTable("spCOM_PasswordReset", new SqlParameter("@UserId", userId));
            if (sql.HasError)
            {
                errorCode = 5; //database error
                return null;
            }

            if (!sql.HasError && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                errorCode = Conversion.StringToInt(dr["OutputCode"].ToString(), -1);
                return dr["GUID"].ToString();
            }

            // no sql error but no row something went really really wrong ಠ_ಠ 
            errorCode = 5; // critical error
            return string.Empty;
        }

        #endregion Load Methods

        #region User CUD
        #region Update Password
        /// <summary>
        /// Update Existing User's password by reset code 
        /// </summary>
        /// <param name="guid">Reset code sent to users email.</param>
        /// <param name="newPass">User's new chosen password.</param>
        /// <typeparam name="T">Child of UserInformation</typeparam>
        /// <returns>
        /// /////Error Codes/////
        /// 1 - Success
        /// 2 - User not found 
        /// 3 - Reset code expired
        /// 4 - Invalid password
        /// 5 - Database error
        /// 6 - No rows updated 
        /// 7 - Provided Password is incorrect
        /// </returns>
        public static int UpdatePassword<T>(string guid, string newPass) where T : UserInformation, new()
        {
            T ui = GetUser<T>(guid);

            return ui == null ? 2 : UpdatePassword(ui, null, newPass);
        }
        /// <summary>
        /// Perform Password Change for regular password change 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="newPass"></param>
        /// <param name="oldPass"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// 1 - Success
        /// 2 - User not found 
        /// 3 - Reset code expired
        /// 4 - Invalid password
        /// 5 - Database error
        /// 6 - No rows updated 
        /// 7 - Provided Password is incorrect
        /// </returns>
        public static int UpdatePassword<T>(int userid, string newPass, string oldPass) where T : UserInformation, new()
        {
            T ui = GetUser<T>(userid);

            return ui == null ? 2 : UpdatePassword(ui, oldPass, newPass);
        }
        /// <summary>
        /// Perform Password Update
        /// </summary>
        /// <param name="ui">Retrieved User</param>
        /// <param name="oldPass">User's current password</param>
        /// <param name="newPass">User's new password </param>
        /// <returns>
        /// /////Error Codes//////
        /// 1 - Success
        /// 2 - User not found 
        /// 3 - Reset code expired
        /// 4 - Invalid password
        /// 5 - Database error
        /// 6 - No rows updated 
        /// 7 - Provided Password is incorrect
        /// </returns>
        public static int UpdatePassword<T>(T ui, string oldPass, string newPass)
            where T : UserInformation, new()
        {
            //oldPass == null only when it's a GUID reset, so check if the reset date is within the limits
            if (oldPass == null && DateTime.Now > ui.PasswordResetDate.AddHours(12))
                return 3; //reset code expired

            if (newPass.Length > 20 || newPass.Length < 6)
                return 4; //invalid password length

            SQLDatabaseReporting sql = new SQLDatabaseReporting();

            SqlParameter outParam;

            SQLParamList sqlParams = new SQLParamList();

            sqlParams.Add("@UserId", ui.UserID)
                     .Add("@NewPassword", newPass)
                     .AddOutputParam("@AffectedRows", 4, out outParam);

            if (oldPass != null)
            {
                sqlParams.Add("@OldPassword", oldPass);
            }

            sql.ExecStoredProcedureDataTable("spCOM_UpdatePassword", sqlParams);

            if (sql.HasError)
                return 5;

            int affectedRows = Conversion.StringToInt(outParam.Value.ToString(), -1);

            if (affectedRows > 1)
                return 5; // Database Error
            // Database Error

            if (oldPass != null && affectedRows == 0)
                return 7; // assume if the old password was provided and no rows are returned that the password is incorrect 

            if (affectedRows == 0)
                return 6; // information is valid but no rows updated



            return affectedRows == 1 ? 1 : 0; // if not successful by this point return unknown error (1 = success, 0, unknown)
        }

        #endregion

        #region Add To Group
        /// <summary>
        /// Add a user to a security group for a given client
        /// </summary>
        /// <param name="userId">Id of User to add to group</param>
        /// <param name="groupId">Id of Group to add user to</param>
        /// <param name="clientId">Client Id Group belongs to </param>
        /// <returns>
        /// 0 - Unknown Error
        /// 1 - Success
        /// 2 - Group Not Found
        /// 3 - User already in group
        /// 4 - Database Error
        /// </returns>
        public static int AddUserToGroup(int userId, int groupId, int clientId)
        {
            return AddUserToGroup(userId, groupId, clientId, null, null, null, null);
        }
        /// <summary>
        /// Add a user to a security group for a given client
        /// </summary>
        /// <param name="userId">Id of User to add to group</param>
        /// <param name="groupId">Id of Group to add user to</param>
        /// <param name="clientId">Client Id Group belongs to </param>
        /// <param name="customFlag1">Custom Int Flag (Application specific)</param>
        /// <param name="customFlag2">Custom Int Flag (Application specific)</param>
        /// <param name="customChar1">Custom String Flag (Application specific)</param>
        /// <param name="customChar2">Custom String Flag (Application specific)</param>
        /// <returns>
        /// /////Error Codes/////
        /// 0 - Unknown Error
        /// 1 - Success
        /// 2 - Group Not Found
        /// 3 - User already in group
        /// 4 - Database Error
        /// </returns>
        public static int AddUserToGroup(int userId, int groupId, int clientId, int? customFlag1, int? customFlag2, string customChar1, string customChar2)
        {

            //sanitize empty string as null
            customChar1 = customChar1 == "" ? null : customChar1;
            customChar2 = customChar2 == "" ? null : customChar2;

            SQLDatabaseReporting sql = new SQLDatabaseReporting();

            var dt = sql.QueryDataTable(
                @"SELECT Count([GroupID]) as GroupCount
                              FROM [dbo].[tblCOM_Groups]
                              Where GroupID = @GroupId AND ClientID = @ClientId",
                new SqlParameter("@GroupId", groupId),
                new SqlParameter("@ClientId", clientId));
            int count = Conversion.StringToInt(dt.Rows[0]["GroupCount"].ToString(), -1);
            if (count < 0)
                return 4; // database error 


            bool groupExists = count > 0;

            if (!groupExists)
                return 2;// group doesn't exist

            dt = sql.QueryDataTable(
                        @"SELECT Count([UserID]) as GroupCount
                          FROM [dbo].[tblCOM_UserToGroups]
                          Where UserID = 2 AND GroupID = 1",
                          new SqlParameter("@UserId", userId),
                          new SqlParameter("@GroupId", groupId));
            count = Conversion.StringToInt(dt.Rows[0]["GroupId"].ToString(), -1);

            if (count < 0)
                return 4; // database error 


            bool userInGroup = count > 0;

            if (userInGroup)
                return 3;//user already in group;

            SQLParamList paramList = new SQLParamList();
            paramList
                .Add("@UserID", userId)
                .Add("@GroupID", groupId)
                .Add("@IntFlag1", customFlag1)
                .Add("@IntFlag2", customFlag2)
                .Add("@CharFlag1", customChar1)
                .Add("@CharFlag2", customChar2);

            int rows = sql.NonQuery(
                @"INSERT INTO [dbo].[tblCOM_UserToGroups]
                           ([UserID],[GroupID]
                           ,[CustomFlag1],[CustomFlag2]
                           ,[CustomChar1],[CustomChar2])
                     VALUES
                           (@UserId,@GroupId
                           ,@IntFlag1,@IntFlag2
                           ,@CharFlag1,@CharFlag2)",
                paramList);


            if (rows < 1 || sql.HasError)
                return 4; //database error 

            return 1; // Success!  

        }

        #endregion

        /// <summary>
        /// Checks if user is currently registered in a given group 
        /// </summary>
        /// <param name="userId">User's ID </param>
        /// <param name="groupId">Group to check if is User is member in</param>
        /// <returns>True if user and group match false if either user or group not found or user not in group</returns>
        public static bool IsInGroup(int userId, int groupId)
        {

            SQLDatabaseReporting sql = new SQLDatabaseReporting();
            var dt = sql.QueryDataTable(@"
                    SELECT [UserID],[GroupID]
                      FROM [dbo].[tblCOM_UserToGroups]
                      WHERE UserID = @UserId AND GroupID = @GroupId",
                               new SqlParameter("@UserId", userId),
                               new SqlParameter("@GroupId", groupId));
            
            return dt.Rows.Count > 0;
        }


        /// <summary>
        /// Create New User Login in database
        /// </summary>
        /// <param name="username">requested username, must be unique to client id</param>
        /// <param name="password">Users's password - must be between 6-20 characters</param>
        /// <param name="clientId">Client Id client id for application</param>
        /// <param name="firstName">User's First Name</param>
        /// <param name="lastName">User's Last Name</param>
        /// <param name="email">User's Email - Must be globalally unique</param>
        /// <param name="error">
        /// Error output - 
        /// //////Codes//////
        /// 0- Unknown Error
        /// 1- Success
        /// 2- Username or Email Exists
        /// 3- Invalid Email address
        /// 4- Invalid Characters in first or last name 
        /// 5- Password invalid 
        /// 6- Database Error
        /// 
        /// </param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateNewUser<T>(string username, string password, int clientId, string firstName, string lastName, string email, out int error)
            where T : UserInformation, new()
        {

            SQLDatabaseReporting sql = new SQLDatabaseReporting();
            error = 1;

            if (!Validation.RegExCheck(email, ValidationType.Email))
            {
                error = 3; // Invalid email address 
            }
            if (!Validation.RegExCheck(firstName, ValidationType.Name) || !Validation.RegExCheck(lastName, ValidationType.Name))
            {
                error = 4; // Invalid characters in name 
            }

            if (password.Length < 6 || password.Length > 20)
            {
                error = 5;// invalid password
            }

            if (error != 1) // if not successful up to this point return null to prevent unnecessary queries 
                return null;


            bool usernameExists = sql.NonQuery("SELECT * AS [UserCount] FROM [tblCOM_Users] WHERE [Username] = @Username AND [ClientId] = @ClientId",
                new SqlParameter("@Username", username), new SqlParameter("@ClientId", clientId)) > 0;

            bool emailExists = sql.NonQuery("SELECT * AS [UserCount] FROM [tblCOM_Users] WHERE [Email] = @Email  AND [ClientId] = @ClientId",
                new SqlParameter("@Email", email)) > 0;

            if (usernameExists || emailExists)
            {
                error = 2; // user exists with same email or (username and clientId)  
            }

            if (error != 1)
                return null;

            //if you made it this far you should be good to create the user 
            SqlParameter userId;
            var sqlParams = new SQLParamList()
                .Add("@Username", username)
                .Add("@Password", password)
                .Add("@Password", clientId)
                .Add("@Email", email)
                .Add("@FirstName", firstName)
                .Add("@LastName", lastName)
                .Add("@ClientId", clientId)
                .AddOutputParam("@UserID", 4, out userId);

            sql.ExecStoredProcedureDataTable("spCOM_CreateNewUser", sqlParams);

            if (sql.HasError)
            {
                error = 6;
            } //Database Error

            if (error == 1)
            {
                var cuser = GetUser<T>(Conversion.StringToInt(userId.Value.ToString(), -1));
                if (cuser == null)
                {
                    error = 6;
                    return null;
                }
                return cuser; // return created user here 
            }


            error = 0;
            return null;
        }

        /// <summary>
        /// Toggles the user's active status 
        /// </summary>
        /// <typeparam name="T">Child of UserInformation </typeparam>
        /// <param name="id">UserId</param>
        /// <param name="active">Set to true or false explicitly define what the active state should be </param>
        /// <param name="error">Error Code Output
        /// /////Error Codes//////
        /// 0 - Unknown Error
        /// 1 - Success
        /// 2 - User not found
        /// 3 - Database Error
        /// </param>
        /// <returns>T of UserInformation</returns>
        public static T ToggleActiveUserStatus<T>(int id, bool? active, out int error) where T : UserInformation, new()
        {
            error = 0;
            var ui = GetUser<T>(id);
            if (ui == null)
            {
                error = 2; // user not found 
                return null;
            }

            SQLDatabaseReporting sql = new SQLDatabaseReporting();


            int affected = sql.NonQuery("UPDATE [tblCOM_Users] SET [Active] = @IsActive, [TSModified] = GETDATE() WHERE UserID = 2", new SqlParameter("@IsActive", (active ?? !ui.IsActive)));
            if (affected != 1)
                error = 3;  //Database Error 

            error = 1; // Success! 
            return GetUser<T>(id);
        }
        #endregion

        #endregion Static Methods

        #region Private Methods

        /// <summary>
        ///     Sets the UserInformation's properties based on a DataRow.
        /// </summary>
        /// <param name="dr">The data row to pull user information from.</param>
        private void SetUserDataFromDataRow(DataRow dr)
        {
            if (dr == null)
            {
                UserID = -1;
                return;
            }
            UserID = Conversion.StringToInt(dr["UserID"].ToString());
            Username = dr["Username"].ToString();
            FirstName = dr["FirstName"].ToString();
            LastName = dr["LastName"].ToString();
            Email = dr["Email"].ToString();
            GUID = dr["GUID"].ToString();
            LoginCount = Conversion.StringToInt(dr["LoginCount"].ToString(), 0);
            LastLoginDate = (dr["LastLoginDate"] != DBNull.Value)
                                ? Conversion.XMLDateToDateTime(dr["LastLogindate"].ToString())
                                : DateTime.MinValue;
            PasswordResetDate = (dr["PasswordResetDate"] != DBNull.Value)
                                    ? Conversion.XMLDateToDateTime(dr["PasswordResetDate"].ToString())
                                    : DateTime.MinValue;
            PasswordExpireDate = (dr["PasswordExpireDate"] != DBNull.Value)
                                     ? Conversion.XMLDateToDateTime(dr["PasswordExpireDate"].ToString())
                                     : DateTime.MinValue;
            CreationDate = (dr["TSCreated"] != DBNull.Value)
                               ? Conversion.XMLDateToDateTime(dr["TSCreated"].ToString())
                               : DateTime.MinValue;
            ModificationDate = (dr["TSModified"] != DBNull.Value)
                                   ? Conversion.XMLDateToDateTime(dr["TSModified"].ToString())
                                   : DateTime.MinValue;
            ModifiedUserID = Conversion.StringToInt(dr["ModifiedUserID"].ToString(), -1);

            IsActive = Conversion.StringToBool(dr["Active"].ToString());

            GroupID = Conversion.StringToInt(dr["GroupID"].ToString(), -1);
            CustomFlag1 = Conversion.StringToInt(dr["CustomFlag1"].ToString(), -1);
            CustomFlag2 = Conversion.StringToInt(dr["CustomFlag2"].ToString(), -1);
            CustomChar1 = dr["CustomChar1"].ToString();
            CustomChar2 = dr["CustomChar2"].ToString();
        }

        #endregion Private Methods
    }
}
