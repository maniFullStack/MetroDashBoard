<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="GCC_Web_Portal.Admin.UserList"
	AllowedGroups="ForumAdmin,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContentHeader" runat="server">
	<h1>User List</h1>
	<ol class="breadcrumb">
		<li><a href="/">Home</a></li>
		<li class="active">User List</li>
	</ol>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<% if ( TopMessage.IsVisible ) { %>
<div class="row">
	<div class="col-md-6">
		<sc:MessageManager runat="server" ID="TopMessage"></sc:MessageManager>
	</div>
</div>
<% } %>
<% if ( Data == null ) { %>
	<div class="row">
		<div class="col-md-6 col-md-offset-3">
			<div class="box box-danger box-solid">
				<div class="box-header with-border">
					<h3 class="box-title">Error</h3>
				</div>
				<div class="box-body">
					  Unable to load the data. Please try again.
				</div>
			</div>
		</div>
	</div>
<% } else { %>
	<div class="row">
		<div class="col-xs-12">
			<div class="box box-info">
				<div class="box-header with-border">
					<i class="fa fa-users"></i>
					<h3 class="box-title">Users</h3>
					<span class="pull-right">
						<a href="/Admin/AddUser" title="Add a New User" class="btn btn-success">Add New</a>&nbsp;
						<asp:Button Text="Export" runat="server" ID="btnExport" OnClick="btnExport_Click" CssClass="btn btn-primary pull-right" UseSubmitBehavior = "false"/>
						
					</span>
				</div>
				<div class="box-body border-radius-none">
					<% int currentPageStart = -1, currentPageEnd = -1, prevPage = -1, totalRecords = -1, nextPage = -1; %>
					<div class="row" style="margin-bottom:5px;"> 
						<div class="col-sm-6">
							<div class="input-group">
								<asp:TextBox runat="server" ID="txtNameSearch" MaxLength="150" CssClass="form-control" placeholder="Name or Email"></asp:TextBox>
								<span class="input-group-btn">
									<asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="btn btn-primary" />
								</span>
							</div>
						</div>
						<% if ( Data.Rows.Count != 0 ) {
							currentPageStart = ( ( CurrentPage - 1 ) * ROWS_PER_PAGE ) + 1;
							currentPageEnd = CurrentPage * ROWS_PER_PAGE;
							prevPage = CurrentPage == 1 ? -1 : CurrentPage - 1;
							totalRecords = Conversion.StringToInt( Data.Rows[0]["TotalRows"].ToString() );
							nextPage = CurrentPage * ROWS_PER_PAGE < totalRecords ? CurrentPage + 1 : -1;
							if ( currentPageEnd > totalRecords ) {
								currentPageEnd = totalRecords;
							} %>
						<div class="col-sm-6">
							<div class="pull-right">
								<% if ( CurrentPage == -1 ) { %>
								<%= totalRecords %> / <%= totalRecords %>
								<a href="/Admin/Users/1" class="btn btn-default btn-sm" role="button">Show Pages</a>
								<% } else { %>
								<%= currentPageStart %>-<%= currentPageEnd %> / <%= totalRecords %>
								<div class="btn-group">
									<a href="/Admin/Users/<%= prevPage %>" class="btn btn-default btn-sm<%= prevPage == -1 ? " disabled" : String.Empty %>" role="button"><i class="fa fa-chevron-left"></i></a>
									<a href="/Admin/Users/-1" class="btn btn-default btn-sm<%= currentPageStart == 1 && currentPageEnd == totalRecords ? " disabled" : String.Empty %>" role="button">Show All</a>
									<a href="/Admin/Users/<%= nextPage %>" class="btn btn-default btn-sm<%= nextPage == -1 ? " disabled" : String.Empty %>" role="button"><i class="fa fa-chevron-right"></i></a>
								</div>
								<% } %>
							</div>
						</div>
						<% } %>
					</div>
					<% if ( Data.Rows.Count == 0 ) { %>
					No results found!
					<% } else { %>
					<table class="table table-bordered table-striped">
						<thead>
							<tr>
								<th>Name</th>
								<th>Email</th>
								<th>Status</th>
								<th>Property</th>
								<th>Team</th>
								<th>Last Login</th>
								<th></th>
							</tr>
						</thead>
						<tbody>
							<% foreach( DataRow dr in Data.Rows ) {
							%>
							<tr>
								<td><%= ReportingTools.CleanData( dr["FirstName"] ) %> <%= ReportingTools.CleanData( dr["LastName"] ) %></td>
								<td><%= ReportingTools.CleanData( dr["Email"] ) %></td>
								<td><%= dr["Active"].Equals( true ) ? "Active" : "Disabled" %></td>
								<td><%= ReportingTools.CleanData( dr["PropertyName"] ) %></td>
								<td><%= ReportingTools.CleanData( GetGroupName( dr["GroupID"] ) ) %></td>
								<td><%= dr["LastLogin"].Equals(DBNull.Value) ? "Never" : Conversion.XMLDateToDateTime( dr["LastLogin"].ToString() ).ToString( "yyyy-MM-dd" ) %></td>
								<td>
									<a href="/Admin/User/<%= dr["UserID"] %>" class="btn btn-primary">Edit</a>
									<a href="/Admin/Users/?luid=<%= dr["UserID"] %>" class="btn btn-info">Log In As</a>
								</td>
							</tr>
							<% } %>
						</tbody>
					</table>
					<div class="pull-right">
						<% if ( CurrentPage == -1 ) { %>
						<%= totalRecords %> / <%= totalRecords %>
						<a href="/Admin/Users/1" class="btn btn-default btn-sm" role="button">Show Pages</a>
						<% } else { %>
						<%= currentPageStart %>-<%= currentPageEnd %> / <%= totalRecords %>
						<div class="btn-group">
							<a href="/Admin/Users/<%= prevPage %>" class="btn btn-default btn-sm<%= prevPage == -1 ? " disabled" : String.Empty %>" role="button"><i class="fa fa-chevron-left"></i></a>
							<a href="/Admin/Users/-1" class="btn btn-default btn-sm<%= currentPageStart == 1 && currentPageEnd == totalRecords ? " disabled" : String.Empty %>" role="button">Show All</a>
							<a href="/Admin/Users/<%= nextPage %>" class="btn btn-default btn-sm<%= nextPage == -1 ? " disabled" : String.Empty %>" role="button"><i class="fa fa-chevron-right"></i></a>
						</div>
						<%} %>
					</div>
					<% } %>
				</div>
			</div>
		</div>
	</div>    
<% } %>
</asp:Content>
<asp:Content ContentPlaceHolderID="FooterScripts" runat="server">

</asp:Content>
