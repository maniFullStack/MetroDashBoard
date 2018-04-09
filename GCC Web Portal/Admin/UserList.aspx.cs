using SharedClasses;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using WebsiteUtilities;

namespace GCC_Web_Portal.Admin
{
    public partial class UserList : AuthenticatedPage
    {
        protected const int ROWS_PER_PAGE = 20;
        protected DataTable Data = null;
		protected LoginErrorCode LoginResponse = LoginErrorCode.UnknownError;

		/// <summary>
		/// Gets the current listing page.
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

        protected void Page_Load(object sender, EventArgs e)
        {
			int loginUserID = RequestVars.Get<int>( "luid" , -1 );
			if ( loginUserID >= 0 ) {
				LoginResponse = TryLogUserIn( loginUserID );
				switch ( LoginResponse ) {
					case LoginErrorCode.Success:
						Response.Redirect( "/Director.ashx" );
						break;

					case LoginErrorCode.PasswordExpired:
						Response.Redirect( "/Director.ashx" ); //This will redirect to password change
						break;

					default:
						TopMessage.ErrorMessage = LoginResponse.ToString();
						break;
				}
			}

            Title = "GCC Users";
            Master.HideAllFilters = true;
            SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
            SQLParamList sqlParams = new SQLParamList();
            if (CurrentPage == -1)
            {
                sqlParams.Add("@ShowAllRows", true);
            }
            else
            {
                sqlParams.Add("@Page", CurrentPage)
                         .Add("@RowsPerPage", ROWS_PER_PAGE);
            }
            if (!String.IsNullOrEmpty(txtNameSearch.Text.Trim()))
            {
                sqlParams.Add("@TextSearch", txtNameSearch.Text.Trim());
            }
            DataTable dt = sql.ExecStoredProcedureDataTable("spAdmin_User_List", sqlParams);
            if (!sql.HasError)
            {
                Data = dt;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
        }

        protected string GetGroupName(object groupID)
        {
            switch (groupID.ToString().StringToInt(-1))
            {
                case 1:
                    return "Forum Research";

                case 3:
                    return "Corporate Management";

                case 4:
                    return "Property Managers";

                case 5:
                    return "Property Staff";

                case 6:
                    return "HR Staff";

                case 7:
                    return "Corporate Marketing";
            }
            return "Unknown";
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (Data != null)
            {
                string fileName = String.Format("UserExport-{0}.csv", ReportingTools.AdjustAndDisplayDate(DateTime.Now, "yyyy-MM-dd-hh-mm-ss", User));
                string fullPath = Server.MapPath(Path.Combine(Config.CacheFileDirectory, fileName));
                if (!Data.DataTableToCSV(fullPath, true, 16))
                {
                    TopMessage.ErrorMessage = "Unable to export the user list. Please try again.";
                }
                else
                {
                    Response.Clear();
                    Response.ContentType = "text/csv";
                    Response.AddHeader("content-disposition", String.Format(@"attachment;filename=""{0}""", fileName));
                    Response.WriteFile(fullPath);
                    Response.End();
                }
            }
        }

		/// <summary>
		///     Checks for valid login credentials. Returns 0 if successful, 1 if the username or password is empty, 2 if there was a SQL error, 3 if the user is locked out or 4 if the username or password is invalid.
		/// </summary>
		private static LoginErrorCode TryLogUserIn(int userID) {
			SQLDatabase sql = new SQLDatabase();    sql.CommandTimeout = 120;
			DataTable dt = sql.QueryDataTable( "SELECT email FROM tblCOM_Users WHERE UserID = " + userID );
			if ( !sql.HasError && dt.Rows.Count > 0 ) {
				int outputVal;

				UserInfo ui = UserInformation.LogUserIn<UserInfo>(
					dt.Rows[0]["email"].ToString(),
					"ASBNNLKW@nknslafans@$#^@*AjksN",
					true, //email logins allowed
					Config.ClientID, out outputVal );

				LoginErrorCode loginCode = (LoginErrorCode)outputVal;

				return loginCode;
			} else {
				return LoginErrorCode.LoginFailed;
			}
		}		
	}
}