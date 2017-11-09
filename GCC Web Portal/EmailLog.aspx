<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailLog.aspx.cs" Inherits="GCC_Web_Portal.EmailLog" %>
<%@ Import Namespace="System.IO" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<link href="/Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <%
        string pd = PickupDirectory;
        if ( !Directory.Exists( pd ) ) { %>
        Invalid pickup directory.
        <% } else { %>
        <table class="table table-striped table-bordered">
            <thead>
                <tr>
                    <th>Subject</th>
                    <th>To</th>
                    <th>CC</th>
                    <th>BCC</th>
                    <th>Date</th>
                    <td></td>
                </tr>
            </thead>
            <tbody>
            <%
            DirectoryInfo di = new DirectoryInfo( pd );
            foreach ( FileInfo file in di.GetFiles( "*.eml" ).OrderByDescending( f => f.LastWriteTime ).ToList() ) {
                string line;
                string to = String.Empty,
                       cc = String.Empty,
                       bcc = String.Empty,
                       subject = String.Empty,
                       date = String.Empty;
                using ( StreamReader fi = new System.IO.StreamReader( file.FullName ) ) {
                    while (( line = fi.ReadLine() ) != null ) {
                        if ( String.IsNullOrWhiteSpace( line ) ) {
                            break;
                        }
                        string[] kvp = line.Split( new char[] { ':' }, 2 );
                        if ( kvp.Length == 2 ) {
                            switch ( kvp[0].ToUpper() ) {
                                case "TO":
                                    to = kvp[1].Trim();
                                    break;
                                case "CC":
                                    cc = kvp[1].Trim();
                                    break;
                                case "BCC":
                                    bcc = kvp[1].Trim();
                                    break;
                                case "SUBJECT":
                                    subject = kvp[1].Trim();
                                    break;
                                case "DATE":
                                    date = kvp[1].Trim();
                                    break;
                            }
                        }
                    }
                    fi.Close();
                } %>
                <tr>
                    <td><%= subject %></td>
                    <td><%= to %></td>
                    <td><%= cc %></td>
                    <td><%= bcc %></td>
                    <td><%= date %></td>
                    <td><a href="/EmailLog.aspx?f=<%= file.Name %>"><%= file.Name %></a></td>
                </tr>
            <% } %>
            </tbody>
        </table>
        <% } %>
    </div>
    </form>
</body>
</html>
