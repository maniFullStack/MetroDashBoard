<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FeedbackList.aspx.cs" Inherits="GCC_Web_Portal.Admin.FeedbackList"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,PropertyStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>
<asp:Content ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="MainContentHeader" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
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
                    <i class="fa fa-question"></i>
                    <h3 class="box-title">Feedback Requests</h3>
                    <span class="pull-right">
                        <a href="/SFeedback/" class="btn btn-success">Manual Feedback Entry</a>
                    </span>
                    <div class="input-group" style="max-width: 300px;">
                        <asp:TextBox runat="server" ID="txtRecordIDSearch" MaxLength="10" CssClass="form-control" placeholder="Record ID"></asp:TextBox>
                        <span class="input-group-btn">
                            <asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="btn btn-primary" />
                        </span>
                    </div>
                </div>
                <div class="table-responsive">
                    <div class="box-body border-radius-none">
                        <% if ( Data.Rows.Count == 0 ) { %>
                        No results found!
                        <% } else {
                               int currentPageStart = ( ( CurrentPage - 1 ) * ROWS_PER_PAGE ) + 1;
                               int currentPageEnd = CurrentPage * ROWS_PER_PAGE;
                               int prevPage = CurrentPage == 1 ? -1 : CurrentPage - 1;
                               int totalRecords = Conversion.StringToInt( Data.Rows[0]["TotalRows"].ToString() );
                               int nextPage = CurrentPage * ROWS_PER_PAGE < totalRecords ? CurrentPage + 1 : -1;
                               if ( currentPageEnd > totalRecords ) {
                                   currentPageEnd = totalRecords;
                               }
                        %>
                        <div class="pull-right">
                            <% if ( CurrentPage == -1 ) { %>
                            <%= totalRecords %> / <%= totalRecords %>
                            <a href="<%= GetPaginationURL( 1 ) %>" class="btn btn-default btn-sm" role="button">Show Pages</a>
                            <% } else { %>
                            <%= currentPageStart %>-<%= currentPageEnd %> / <%= totalRecords %>
                            <div class="btn-group">
                                <a href="<%= GetPaginationURL( prevPage ) %>" class="btn btn-default btn-sm<%= prevPage == -1 ? " disabled" : String.Empty %>" role="button"><i class="fa fa-chevron-left"></i></a>
                                <a href="<%= GetPaginationURL( -1 ) %>" class="btn btn-default btn-sm<%= currentPageStart == 1 && currentPageEnd == totalRecords ? " disabled" : String.Empty %>" role="button">Show All</a>
                                <a href="<%= GetPaginationURL( nextPage ) %>" class="btn btn-default btn-sm<%= nextPage == -1 ? " disabled" : String.Empty %>" role="button"><i class="fa fa-chevron-right"></i></a>
                            </div>
                            <%} %>
                        </div>
                    
                        <table class="table table-bordered table-striped">
                            <thead>
                                <tr>
                                    <th><%= GetSort( "D", "Date" ) %></th>
                                    <th><%= GetSort( "S", "Survey" ) %></th>
                                    <th><%= GetSort( "P", "Property" ) %></th>
                                    <th><%= GetSort( "R", "Reason / Area" ) %></th>
                                    <th><%= GetSort( "F", "Status" ) %></th>
                                    <th><%= GetSort( "T", "Tier" ) %></th>
                                    
                                    <th><%= GetSort( "M", "Message(s)" ) %></th>
                                    <th><%= GetSort( "A", "Age" ) %></th>
                                    <th><%= GetSort( "N", "Name" ) %></th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                <% foreach( DataRow dr in Data.Rows ) { %>
                                <tr>
                                    <td><%= ReportingTools.AdjustAndDisplayDate( dr["DateCreated"].ToString(), "MMMM dd, yyyy  @  hh:mm tt", User ) %></td>
                                    <td>
                                        <%= dr["SurveyName"] %>
                                        <% if ( !dr["StaffMemberID"].Equals( DBNull.Value ) ) { %>
                                        (<abbr title="Submitted by a staff member">S</abbr>)
                                        <% } %>
                                    </td>
                                    <td><%= ReportingTools.CleanData( dr["CasinoName"] ) %></td>
                                    <td><%= ReportingTools.CleanData( dr["ReasonDescription"] ) %></td>
                                    <td>
                                        <% int statusID = (int)dr["FeedbackStatusID"];
                                            if ( statusID == 1 ) { %>
                                        <span class="label label-danger">
                                        <% } else if ( statusID == 2 ) { %>
                                        <span class="label label-primary">
                                        <% } else if ( statusID == 3 || statusID == 4 ) { %>
                                        <span class="label label-success">
                                        <% } else if ( statusID >= 5 ) { %>
                                        <span class="label label-warning">
                                        <% } else { %>
                                        <span>
                                        <% } %>
                                        <%= ReportingTools.CleanData( dr["StatusName"] ) %>
                                        </span>
                                    </td>
                                    <td>Tier <%= dr["FeedbackTier"] %></td>
                                    
                                    <td><%= dr["MessageCount"] %></td>
	                                <td><%= String.Format("{0:0.0}h", dr["Hours"].ToString()) %></td>
                                    <td><%= ReportingTools.GetNiceName( dr["Name"].ToString() ) %></td>
                                    <td><a href="/Admin/Feedback/<%= dr["UID"] %>" class="btn btn-primary" target="_blank">View <i class="fa fa-external-link"></i></a></td>
                                </tr>
                                <% } %>
                            </tbody>
                        </table>
                    
                        <div class="pull-right">
                            <% if ( CurrentPage == -1 ) { %>
                            <%= totalRecords %> / <%= totalRecords %>
                            <a href="<%= GetPaginationURL( 1 ) %>" class="btn btn-default btn-sm" role="button">Show Pages</a>
                            <% } else { %>
                            <%= currentPageStart %>-<%= currentPageEnd %> / <%= totalRecords %>
                            <div class="btn-group">
                                <a href="<%= GetPaginationURL( prevPage ) %>" class="btn btn-default btn-sm<%= prevPage == -1 ? " disabled" : String.Empty %>" role="button"><i class="fa fa-chevron-left"></i></a>
                                <a href="<%= GetPaginationURL( -1 ) %>" class="btn btn-default btn-sm<%= currentPageStart == 1 && currentPageEnd == totalRecords ? " disabled" : String.Empty %>" role="button">Show All</a>
                                <a href="<%= GetPaginationURL( nextPage ) %>" class="btn btn-default btn-sm<%= nextPage == -1 ? " disabled" : String.Empty %>" role="button"><i class="fa fa-chevron-right"></i></a>
                            </div>
                            <%} %>
                        </div>
                        <% } %>
                    </div>
                </div>
            </div>
        </div>
    </div>
<% } %>
</asp:Content>
<asp:Content ContentPlaceHolderID="FooterScripts" runat="server">
</asp:Content>
