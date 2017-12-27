<%@ Page Title="Home" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GCC_Web_Portal.Default"
	AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,PropertyStaff,HRStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
	<style>
		tr.sub-total {
			font-weight:bold;
			background:#f4f4f4;
			border-top:1px solid #d8d8d8;
			border-bottom:2px solid #d8d8d8;
		}
		table.snapshot > tbody > tr.ss-row > td:first-child {
			padding-left:40px;
			text-align:left;
		}
	</style>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="MainContentHeader">
	<h1>Home</h1>
	<ol class="breadcrumb">
		<li class="active">Home</li>
	</ol>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
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
		<div class="col-md-12 col-lg-6">
			<div class="box box-success">
				<div class="box-header with-border">
					<i class="fa fa-line-chart"></i>
					<h3 class="box-title">Overall</h3>
				</div>
				<div class="box-body border-radius-none">
					<table class="table table-bordered results">
						<thead>
							<tr>
								<th style="width:20%"></th>
								<th style="width:20%">GEI</th>
								<th style="width:20%">NPS</th>
								<th style="width:20%">PRS</th>
								<th style="width:20%">GSEI</th>
							</tr>
						</thead>
						<tbody>
							<%  ReportFilterListBox fltProperty = Master.GetFilter<ReportFilterListBox>( "fltProperty" );
								foreach(DataRow dr in Data.Tables[DATA_SCORES].Rows) {
									GCCPropertyShortCode sc;
									if (!Enum.TryParse(dr["ShortCode"].ToString(), out sc)) {
										sc = GCCPropertyShortCode.None;
									}
									if ( dr["ShortCode"].Equals( "GCC" )
										|| Master.IsPropertyUser
										|| fltProperty.SelectedIndex == -1
										|| fltProperty.Items.FindByValue( ( (int)sc ).ToString() ) == null
										|| fltProperty.Items.FindByValue( ( (int)sc ).ToString() ).Selected ) { %>
							<tr>
								<th>
									<% if ( !Master.IsPropertyUser || dr["ShortCode"].Equals( User.PropertyShortCode.ToString() ) ) {  %>
									<a href="/PropertyDashboard/<%= dr["ShortCode"].ToString() %>"><%= dr["ShortCode"].ToString() %></a>
									<% } else { %>
									<%= dr["ShortCode"].ToString() %>
									<% } %>
								</th>
								<td><%= ReportingTools.FormatIndex( dr["GEI"].ToString() ) %></td>
								<td><%= ReportingTools.FormatPercent( dr["NPS"].ToString() ) %></td>
								<td><%= ReportingTools.FormatPercent( dr["PRS"].ToString() ) %></td>
								<td><%= ReportingTools.FormatPercent( dr["GSEI"].ToString() ) %></td>
							</tr>
							<%      }
								} %>
						</tbody>
					</table>
				</div>
			</div>
		</div>
		<div class="col-md-12 col-lg-6">
			<div class="box box-danger">
				<div class="box-header with-border">
					<i class="fa fa-envelope-o"></i>
					<h3 class="box-title">Followup Stats</h3>
				</div>
				<div class="box-body border-radius-none">
					<table class="table table-bordered results">
						<thead>
							<tr>
								<th style="width:20%"></th>
								<th style="width:16%">#&nbsp;Followup</th>
								<th style="width:16%">%&nbsp;Followup</th>
								<th style="width:16%">#&nbsp;<&nbsp;24h</th>
								<th style="width:16%">#&nbsp;24-48h</th>
								<th style="width:16%">#&nbsp;>&nbsp;48h</th>
								<th style="width:16%">Avg&nbsp;Resp.</th>
							</tr>
						</thead>
						<tbody>
							<% foreach ( DataRow dr in Data.Tables[DATA_FEEDBACK].Rows ) {
								GCCPropertyShortCode sc;
								if ( !Enum.TryParse( dr["ShortCode"].ToString(), out sc ) ) {
									sc = GCCPropertyShortCode.None;
								}
								if ( dr["ShortCode"].Equals( "GCC" )
									|| Master.IsPropertyUser
									|| fltProperty.SelectedIndex == -1
									|| fltProperty.Items.FindByValue( ( (int)sc ).ToString() ) == null
									|| fltProperty.Items.FindByValue( ( (int)sc ).ToString() ).Selected ) { %>
							<tr>
								<% if ( dr["Name"].Equals( "Overall" ) && !Master.IsPropertyUser ) { %>
								<th><a href="/Followup/GCC">Overall</a></th>
								<% } else if ( dr["Name"].Equals( "Overall" ) && Master.IsPropertyUser ) { %>
								<th>Overall</th>
								<% } else if ( dr["ShortCode"].Equals( "GCC" ) ) { %>
								<th>GCC</th>
								<% } else if ( !Master.IsPropertyUser || dr["ShortCode"].Equals( User.PropertyShortCode.ToString() ) ) { %>
								<th><a href="/Followup/<%= dr["ShortCode"].ToString() %>"><%= dr["ShortCode"].ToString() %></a></th>
								<% } else { %>
								<th><%= dr["ShortCode"].ToString() %></th>
								<% } %>
								<td><%= dr["FeedbackCompleteCount"].ToString() %> / <%= dr["FeedbackCount"].ToString() %></td>
								<td><%= ReportingTools.FormatPercent( dr["FeedbackCompletePercent"].ToString() ) %></td>
								<td><%= dr["FeedbackLessThan24Hrs"].ToString() %></td>
								<td><%= dr["Feedback24HrsTo48Hrs"].ToString() %></td>
								<td><%= dr["FeedbackGreaterThan48Hrs"].ToString() %></td>
								<td><%= ReportingTools.GetNiceHours( dr["AverageFeedbackResponse"].ToString() ) %></td>
							</tr>
							<%  }
							} %>
						</tbody>
					</table>
				</div>
			</div>
		</div>
	</div>
	<div class="row">
		<div class="col-md-12 col-lg-6">
			<div class="row">
				<div class="col-xs-12">
					<div class="box box-warning">
						<div class="box-header with-border">
							<i class="fa fa-building-o"></i>
							<h3 class="box-title">Facilities</h3>
						</div>
						<div class="box-body border-radius-none">
							<table class="table table-bordered results">
								<thead>
									<tr>
										<th style="width:20%"></th>
										<th style="width:14%">Facilities</th>
										<th style="width:13%">Staff</th>
										<th style="width:13%">Gaming</th>
										<th style="width:13%">F&B</th>
										<th style="width:13%">Lounge</th>
										<th style="width:14%">Theatre</th>
										<%--<th>Entertain</th>--%>
									</tr>
								</thead>
								<tbody>
									<% foreach(DataRow dr in Data.Tables[DATA_SCORES].Rows) {
										GCCPropertyShortCode sc;
										if ( !Enum.TryParse( dr["ShortCode"].ToString(), out sc ) ) {
											sc = GCCPropertyShortCode.None;
										}
										if ( dr["ShortCode"].Equals( "GCC" )
											|| Master.IsPropertyUser
											|| fltProperty.SelectedIndex == -1
											|| fltProperty.Items.FindByValue( ( (int)sc ).ToString() ) == null
											|| fltProperty.Items.FindByValue( ( (int)sc ).ToString() ).Selected ) { %>
									<tr>
										<th>
											<% if ( !Master.IsPropertyUser || dr["ShortCode"].Equals( User.PropertyShortCode.ToString() ) ) {  %>
											<a href="/PropertyDashboard/<%= dr["ShortCode"].ToString() %>"><%= dr["ShortCode"].ToString() %></a>
											<% } else { %>
											<%= dr["ShortCode"].ToString() %>
											<% } %>
										</th>
										<td><%= ReportingTools.FormatPercent( dr["FacilitiesScore"].ToString() ) %></td>
										<td><%= ReportingTools.FormatPercent( dr["StaffScore"].ToString() ) %></td>
										<td><%= ReportingTools.FormatPercent( dr["GamingScore"].ToString() ) %></td>
										<td><%= ReportingTools.FormatPercent( dr["FoodAndBeverageScore"].ToString() ) %></td>
										<td><%= ReportingTools.FormatPercent( dr["LoungeScore"].ToString() ) %></td>
										<td><%= ReportingTools.FormatPercent( dr["TheatreScore"].ToString() ) %></td>
										<%--<td><%= ReportingTools.FormatPercent( dr["EntertainScore"].ToString() ) %></td>--%>
									</tr>
									<%  }
									} %>
								</tbody>
							</table>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="col-md-12 col-lg-6">
			<div class="box box-primary">
				<div class="box-header with-border">
					<i class="fa fa-users"></i>
					<h3 class="box-title">Employee Snapshot Status</h3>
					<div class="box-tools pull-right">
						<a href="/SnapshotStatus" class="small-box-footer">Details <i class="fa fa-arrow-circle-right"></i></a>
					</div>
				</div>
				<div class="box-body border-radius-none">
					<small>Note: Only the Property filter is applied to this data.</small>
					<table class="table table-bordered results snapshot">
						<thead>
							<tr>
								<th style="width:60%">Location</th>
								<th style="width:20%">Salary</th>
								<th style="width:20%">Hourly</th>
							</tr>
						</thead>
						<tbody>
							<%
							bool isFirstRow = true;
							foreach ( DataRow dr in Data.Tables[DATA_SNAPSHOT].Rows ) { %>
							<tr class="<%= isFirstRow ? "sub-total" : "ss-row" %>">
								<th><%= dr["Location"] %></th>
								<td><%= dr["ActualSalaryCount"] %>&nbsp;/&nbsp;<%= dr["TargetSalaryCount"] %> =&nbsp;<%= ReportingTools.FormatPercent( dr["SalaryCompletionRate"].ToString(), 0 ) %></td>
								<td><%= dr["ActualHourlyCount"] %>&nbsp;/&nbsp;<%= dr["TargetHourlyCount"] %> =&nbsp;<%= ReportingTools.FormatPercent( dr["HourlyCompletionRate"].ToString(), 0 ) %></td>
							</tr>
							<%
								isFirstRow = false;
							} %>
						</tbody>
					</table>
				</div>
			</div>
		</div>
	</div>
<% } %>
   <%-- <div class="row">
		<div class="col-md-12 col-lg-6 col-lg-offset-3">
			<div class="box box-primary">
				<div class="box-header with-border">
					<i class="fa fa-link"></i>
					<h3 class="box-title">Other Reports</h3>
				</div>
				<div class="box-body border-radius-none">
					<div class="row">
						<div class="col-sm-6 col-md-3">
							<a href="/PropertyDashboard/">Property Dashboard</a>
						</div>
						<div class="col-sm-6 col-md-3">
							<a href="/RespondentProfile/">Respondent Profile</a>
						</div>
						<div class="col-sm-6 col-md-3">
							<a href="/QuestionTopBottom/">Top / Bottom Questions</a>
						</div>
						<div class="col-sm-6 col-md-3">
							<a href="/PropertyDashboard/">Text Analytics</a>
						</div>
						<div class="col-sm-6 col-md-3">
							<a href="/PropertyDashboard/">Respondent History</a>
						</div>
						<div class="col-sm-6 col-md-3">
							<a href="/PropertyDashboard/">Employee Survey Results</a>
						</div>
						<div class="col-sm-6 col-md-3">
							<a href="/Admin/">Admin Portal</a>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>--%>
</asp:Content>