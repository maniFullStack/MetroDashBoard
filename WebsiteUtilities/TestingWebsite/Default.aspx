<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestingWebsite._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
    	table {
    		border-collapse:collapse;
    	}
    	td {
    		padding:3px 20px;
    		border:1px solid #000;
    	}
		.success {
			color:green;
		}
		.fail {
			color:red;
		}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <% string output; %>
    <table>
		<tr>
			<td>SQL SMI Connection Test</td>
			<%= GetCell(SQLSMIConnectionTest(out output), output)%>
		</tr>
		<tr>
			<td>Excel File Connection Test</td>
			<%= GetCell(ExcelExtractTest(out output), output)%>
		</tr>
		<tr>
			<td>User Login Test</td>
			<%= GetCell(UserLoginTest(out output), output)%>
		</tr>
		<tr>
			<td>User Fetch Test</td>
			<%= GetCell(UserFetchTest(out output), output)%>
		</tr>
		<tr>
			<td>CSV To DataTable (File) With Headers Test</td>
			<%= GetCell(CSVToDataTableFileWithHeadersTest(out output), output)%>
		</tr>
		<tr>
			<td>CSV To DataTable (File) Without Headers Test</td>
			<%= GetCell(CSVToDataTableFileWithoutHeadersTest(out output), output)%>
		</tr>
		<tr>
			<td>CSV To DataTable (Stream) With Headers Test</td>
			<%= GetCell(CSVToDataTableStreamWithHeadersTest(out output), output)%>
		</tr>
		<tr>
			<td>CSV To DataTable (Stream) Without Headers Test</td>
			<%= GetCell(CSVToDataTableStreamWithoutHeadersTest(out output), output)%>
		</tr>
		<tr>
			<td>DataTable To CSV Test</td>
			<%= GetCell(DataTableToCSVTest(out output), output)%>
		</tr>
	    <tr>
			<td>Password Reset Generates Guid With Valid User</td>
			<%= GetCell(PasswordResetGeneratesGuidWithValidUser(out output), output)%>
		</tr>
	    <tr>
			<td>Password Reset Returns Error 2 With Uer Not Found</td>
			<%= GetCell(PasswordResetReturns2WithUserNotFound(out output), output)%>
		</tr>
		<tr>
			<td>Password Update By GUID Returns Valid When Using Valid GUID </td>
			<%= GetCell(PasswordUpdateByGUIDReturnsValidWhenUsingValidGUID(out output), output)%>
		</tr>
		<tr>
			<td>Password Update By ID And Password Returns Valid When Using Valid User Info </td>
			<%= GetCell(PasswordUpdateByIDAndPasswordReturnsValidWhenUsingValidUserInfo(out output), output)%>
		</tr>		
        <tr>
			<td>IsInGroup() Tests False When User Is Not In Group </td>
			<%= GetCell(IsInGroupTestsFalseWhenUserIsNotInGroup(out output), output)%>
		</tr>	
		
		<tr>
			<td>IsInGroup() Tests True When User Is In Group </td>
			<%= GetCell(IsInGroupTestsTrueWhenUserIsInGroup(out output), output)%>
		</tr>
		<tr>
			<td>GetUser() By Username Retrieves Valid User </td>
			<%= GetCell(GetUserByUsernameRetrievesValidUser(out output), output)%>
		</tr>		
		
	    <tr>
			<td>GetUser() By Email Retrieves Valid User </td>
			<%= GetCell(GetUserByEmailRetrievesValidUser(out output), output)%>
		</tr>
		
	    <tr>
			<td>Email Validation</td>
			<%= GetCell(EmailValidation(out output), output)%>
		</tr>
		
		
		<!-- More tests to come -->
    </table>
    </div>
    <%
		var template = new WebsiteUtilities.ReplaceTemplate("Template.htm");
		template.AddReplacementValue("ReplaceMe", "Replaced!");
		string test = "2";
		
        
        string currentValue = WebsiteUtilities.RequestVars.Post<string>("Test", "2");
		template.AddReplacementValue("TestOptions", //"{TestOptions}" in the template will be replaced.
				new string[] { "One|1", "Two|2", "Three|3" }, //Pass in string array of items
				(string key, string value, int index) => { //Each item will be passed into this function and appended to the output.
					string[] temp = value.Split('|');//We have two values for each string. I.e. "Two" and "2" from "Two|2"
					//Format the output option and check against the current value from a variable in the outer scope (currentValue).
					return "<option " + (temp[1].Equals(currentValue) ? "selected" : "") + " value=\"" + temp[1] + "\">" + temp[0] + "</option>\n";
				});
    	Response.Write(template.GetTemplate());
	%>
    </form>
</body>
</html>
