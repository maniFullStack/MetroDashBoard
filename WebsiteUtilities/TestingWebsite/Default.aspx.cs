using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteUtilities;
using System.Data;
using System.IO;

namespace TestingWebsite {
    public partial class _Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected string GetCell(bool success, string extraMessage) {
            if (String.IsNullOrEmpty(extraMessage)) {
                extraMessage = String.Empty;
            } else {
                extraMessage = " &ndash; " + extraMessage;
            }
            return success ? "<td class=\"success\">Succeeded" + extraMessage + "</td>" : "<td class=\"fail\">Failed" + extraMessage + "</td>";
        }

        protected bool SQLSMIConnectionTest(out string exceptionMessage) {
            exceptionMessage = String.Empty;
            try {
                SQLDatabase sql = new SQLDatabase();
                sql.QueryDataTable("SELECT 1");
                if (sql.HasError) {
                    exceptionMessage = sql.ExceptionList[0].ToString();
                } else {
                    return true;
                }
            }  catch (Exception ex) { exceptionMessage = ex.ToString(); }
            return false;
        }

        protected bool ExcelExtractTest(out string exceptionMessage) {
            exceptionMessage = String.Empty;
            try {
                DataTable dt = new DataTable();
                //dt.Columns.Add(@"Test~!@#$%^&*()|}{:?><,./;'[]\=-0987654321`");
                //dt.Columns.Add(@"Test~!@#$%^,./;987654321");
                dt.Columns.Add(@"Test");
                DataRow dr = dt.NewRow();
                dr[0] = "Test Cell 1";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "Test cell 2";
                dt.Rows.Add(dr);
                string lPath = this.MapPath(@"\Files\");
                string lFileName = string.Format("{0}_{1}", "TestFile", DateTime.Now.ToString("yyyyMMdd_HHmmsssfff"));
                string lFilePath = Path.Combine(lPath, lFileName);
                if (Conversion.DataTableToExcel(dt, "TestSheet", lFilePath, false)) {
                    return true;
                } else {
                    return false;
                }
            } catch (Exception ex) { exceptionMessage = ex.ToString(); }
            return false;

            //<add name="Excel" connectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}.xlsx;Extended Properties=Excel 12.0 Xml;HDR=YES;" providerName="System.Data.OleDb.OleDbConnectionStringBuilder"/>
        }

        protected bool UserLoginTest(out string exceptionMessage) {
            exceptionMessage = String.Empty;
            try {
                int outputVal;
                UserInfoDerived ui = UserInformation.LogUserIn<UserInfoDerived>("username", "xxxxx", false, 88, out outputVal);
                if (ui == null || ui.UserID == -1) {
                    exceptionMessage = "Error. OutputVal: " + outputVal;
                } else {
                    return true;
                }
            } catch (Exception ex) { exceptionMessage = ex.ToString(); }
            return false;
        }

        protected bool UserFetchTest(out string exceptionMessage) {
            exceptionMessage = String.Empty;
            try {
                UserInfoDerived ui = UserInformation.GetUser<UserInfoDerived>(1);
                if (ui == null || ui.UserID == -1) {
                    exceptionMessage = "Unable to load user.";
                } else {
                    return true;
                }
            } catch (Exception ex) { exceptionMessage = ex.ToString(); }
            return false;
        }

        protected bool CSVToDataTableFileWithHeadersTest(out string exceptionMessage) {
            exceptionMessage = String.Empty;
            try {
                DataTable dt = Conversion.CSVToDataTable(@"C:\\temp\\Test.csv", true);
                if (dt.Columns.Count == 3 && dt.Rows.Count == 1 && dt.Rows[0][2].Equals("1.025132135")) {
                    return true;
                } else {
                    exceptionMessage = "Test data was not loaded properly.";
                }
            } catch (Exception ex) { exceptionMessage = ex.ToString(); }
            return false;
        }

        protected bool CSVToDataTableFileWithoutHeadersTest(out string exceptionMessage) {
            exceptionMessage = String.Empty;
            try {
                DataTable dt = Conversion.CSVToDataTable("C:\\temp\\Test.csv", false);
                if (dt.Columns.Count == 3 && dt.Rows.Count == 2 && dt.Rows[0][2].Equals("TestCol3")) {
                    return true;
                } else {
                    exceptionMessage = "Test data was not loaded properly.";
                }
            } catch (Exception ex) { exceptionMessage = ex.ToString(); }
            return false;
        }
        protected bool CSVToDataTableStreamWithHeadersTest(out string exceptionMessage) {
            exceptionMessage = String.Empty;
            try {
                using (StreamReader streamR = new StreamReader(@"C:\\temp\\Test.csv")) {
                    DataTable dt = Conversion.CSVToDataTable(streamR.BaseStream, true);
                    if (dt.Columns.Count == 3 && dt.Rows.Count == 1 && dt.Rows[0][2].Equals("1.025132135")) {
                        return true;
                    } else {
                        exceptionMessage = "Test data was not loaded properly.";
                    }
                }
            } catch (Exception ex) { exceptionMessage = ex.ToString(); }
            return false;
        }

        protected bool CSVToDataTableStreamWithoutHeadersTest(out string exceptionMessage) {
            exceptionMessage = String.Empty;
            try {
                using (StreamReader streamR = new StreamReader(@"C:\\temp\\Test.csv")) {
                    DataTable dt = Conversion.CSVToDataTable(streamR.BaseStream, false);
                    if (dt.Columns.Count == 3 && dt.Rows.Count == 2 && dt.Rows[0][2].Equals("TestCol3")) {
                        return true;
                    } else {
                        exceptionMessage = "Test data was not loaded properly.";
                    }
                }
            } catch (Exception ex) { exceptionMessage = ex.ToString(); }
            return false;
        }


        protected bool DataTableToCSVTest(out string exceptionMessage) {
            exceptionMessage = String.Empty;
            try {
                DataTable dt = new DataTable();
                dt.Columns.Add(@"Test~!@#$%^&*()|}{:?><,./;'[]\=-0987654321`");
                dt.Columns.Add(@"Test~!@#$%^,./;987654321");
                dt.Columns.Add(@"Test");
                DataRow dr = dt.NewRow();
                dr[0] = "Test Cell 1";
                dr[1] = "Test c,ell 2";
                dr[2] = "Test c,,,ell 3";
                dt.Rows.Add(dr);
                dr = dt.NewRow();
                dr[0] = "Test Cell 1";
                dr[1] = "Test c,ell 2";
                dr[2] = "Test c,,,ell 3";
                dt.Rows.Add(dr);
                string lPath = this.MapPath(@"\Files\");
                string lFileName = string.Format("{0}_{1}.csv", "TestFile", DateTime.Now.ToString("yyyyMMdd_HHmmsssfff"));
                string lFilePath = Path.Combine(lPath, lFileName);
                if (Conversion.DataTableToCSV(dt, lFilePath, false)) {
                    return true;
                } else {
                    return false;
                }
            } catch (Exception ex) { exceptionMessage = ex.ToString(); }
            return false;

            //<add name="Excel" connectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}.xlsx;Extended Properties=Excel 12.0 Xml;HDR=YES;" providerName="System.Data.OleDb.OleDbConnectionStringBuilder"/>
        }

        protected bool PasswordResetGeneratesGuidWithValidUser(out string exceptionMessage)
        {
            try
            {
                int err;
                string guid = UserInformation.ResetPassword(1, out err);

                if (string.IsNullOrEmpty(guid))
                {
                    exceptionMessage = "Guid not returned";
                    return false;
                }
                if (err != 1)
                {
                    exceptionMessage = string.Format("SP Return Error {0}", err);
                    return false;
                }
                exceptionMessage = string.Format("Passed (GUID: {0})(Response Code: {1})", guid, err);
                return true;
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                return false;
            }
           
        }
        protected bool PasswordResetReturns2WithUserNotFound(out string exceptionMessage)
        {
            try
            {
                int err;
                string guid = UserInformation.ResetPassword(-1, out err);

                if (!string.IsNullOrEmpty(guid))
                {
                    exceptionMessage =  string.Format("GUID Returned With {0}", guid);
                    return false;
                }
                if (err != 2)
                {
                    exceptionMessage = string.Format("SP Return Error {0}", err);
                    return false;
                }
                exceptionMessage = string.Format("Passed (GUID: {0})(Response Code: {1})", guid, err);
                return true;
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                return false;
            }
        }

        protected bool PasswordUpdateByGUIDReturnsValidWhenUsingValidGUID(out string exceptionMessage)
        {
            try
            {
                int outVar;
                string guid = UserInformation.ResetPassword(4, out outVar);

                int err = UserInformation.UpdatePassword<UserInfoDerived>(guid,"xxxxxx");

                if (string.IsNullOrEmpty(guid))
                {
                    exceptionMessage =  string.Format("GUID Returned With {0}, Error Code: {1}", guid, err);
                    return false;
                }
                if (err != 1)
                {
                    exceptionMessage = string.Format("SP Return Error {0}", err);
                    return false;
                }
                exceptionMessage = string.Format("Passed (GUID: {0})(Response Code: {1})", guid, err);
                return true;
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                return false;
            }
        }
        
        protected bool PasswordUpdateByIDAndPasswordReturnsValidWhenUsingValidUserInfo(out string exceptionMessage)
        {
            try
            {
                
                int err = UserInformation.UpdatePassword<UserInfoDerived>(4,"xxxxxx","xxxxxx");


                if (err != 1)
                {
                    exceptionMessage = string.Format("SP Return Error {0}", err);
                    return false;
                }
                exceptionMessage = string.Format("Passed (Response Code: {0})", err);
                return true;
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
                return false;
            }
        }

        protected bool IsInGroupTestsTrueWhenUserIsInGroup(out string exceptionMessage)
        {
            const int userId = 2, groupId = 1;

            var userInGroup = UserInformation.IsInGroup(userId, groupId);
            exceptionMessage = userInGroup ? 
                string.Format("Passed - UserId {0} is in Group {1}.",userId,groupId) :
                string.Format("Failed - UserId {0} is not in Group {1}.",userId,groupId);

            return userInGroup;
        }

        protected bool IsInGroupTestsFalseWhenUserIsNotInGroup(out string exceptionMessage)
        {
            const int userId = 2, groupId = -1;

            var userInGroup = UserInformation.IsInGroup(userId, groupId);
            exceptionMessage = !userInGroup ?
                string.Format("Passed - UserId {0} is not in Group {1}.", userId, groupId) :
                string.Format("Failed - UserId {0} is in Group {1}.", userId, groupId);

            return !userInGroup;
        }

        protected bool GetUserByEmailRetrievesValidUser(out string exceptionMessage)
        {
            var ui = UserInformation.GetUser<UserInfoDerived>("rmarskell@forumresearch.com", 152, true);
            if (ui == null)
            {
                exceptionMessage = "Fail - user not found";
                return false;
            }
            if (ui.UserID != 1)
            {
                exceptionMessage = string.Format("Failed - Expected UserId 2 returned UserId {0}",ui.UserID);
                return false;
            }
            exceptionMessage = string.Format("Passed - returned User Id {0}", ui.UserID);
            return true;

        }

        protected bool GetUserByUsernameRetrievesValidUser(out string exceptionMessage)
        {
            var ui = UserInformation.GetUser<UserInfoDerived>("rmarskell", 152);
            if (ui == null)
            {
                exceptionMessage = "Fail - user not found";
                return false;
            }
            if (ui.UserID != 1)
            {
                exceptionMessage = string.Format("Failed - Expected UserId 2 returned UserId {0}", ui.UserID);
                return false;
            }
            exceptionMessage = string.Format("Passed - returned User Id {0}", ui.UserID);
            return true;
        }

        protected bool EmailValidation(out string exceptionMessage) {
            string emailToTest = "dave@dfmc.on.ca";
            exceptionMessage = "Email Tested: " + emailToTest;
            return Validation.RegExCheck(emailToTest, ValidationType.Email);
        }


    }
    



}
