<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="SurveyList.aspx.cs" Inherits="GCC_Web_Portal.Admin.SurveyList"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,PropertyStaff,HRStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>
<%@ Register Src="~/Admin/MessageTimeline.ascx" TagPrefix="uc1" TagName="MessageTimeline" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style>
        table.details > tbody > tr > td {
            text-align:left;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Survey List</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Survey List</li>
    </ol>
</asp:Content>
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
                    <i class="fa fa-list-alt"></i>
                    <h3 class="box-title">Results</h3>
                </div>
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
                    <div class="clearfix"></div>
                    <div class="table-responsive">
                    <table class="table table-bordered table-striped">
                        <thead>
                            <tr>
                                <th><%= GetSort( "D", "Date Created" ) %></th>
                                <th><%= GetSort( "P", "Property" ) %></th>
                                <th><%= GetSort( "S", "Survey" ) %></th>
                                <th><%= GetSort( "E", "Guest" ) %></th>
                                <th><%= GetSort( "F", "Feedback" ) %></th>
                                <th><%= GetSort( "N", "Sentiment" ) %></th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach( DataRow dr in Data.Rows ) {
                                   SurveyType st = (SurveyType)dr["SurveyTypeID"].ToString().StringToInt(0);
                            %>
                            <tr>
                                <td><%= ReportingTools.AdjustAndDisplayDate( dr["DateCreated"].ToString(), "MMMM dd, yyyy  @  hh:mm tt", User ) %></td>
                                <td><%= ReportingTools.CleanData( dr["PropertyName"] ) %></td>
                                <td>
                                    <%= st %>
                                    <% if ( !dr["StaffMemberID"].Equals( DBNull.Value ) ) { %>
                                    (<abbr title="Submitted by a staff member">S</abbr>)
                                    <% } %>
                                </td>
                                <td><%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( dr["EncoreAndEmail"].ToString() ) %></td>
                                <td>
                                    <%if ( !dr["FeedbackStatusID"].Equals( DBNull.Value ) ) { %>
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
                                    <%= dr["StatusName"] %>
                                    </span>
                                    <br />
                                    <a href="/Admin/Feedback/<%= dr["FeedbackUID"] %>" target="_blank">View Feedback <i class="fa fa-external-link"></i></a>
                                    <% } else if ( dr["IsHistorical"].Equals( true ) && dr["RequestedFeedback"].Equals( 1 ) ) { %>
                                    <span class="label label-default">Followup Requested - Inactive</span>
                                    <% } %>
                                </td>
                                <td>
                                    <%if ( !dr["SentimentScore"].Equals( DBNull.Value ) ) { %>
                                    <% double sentiment = dr["SentimentScore"].ToString().StringToDbl( -1000 );
                                       if ( sentiment >= 75 ) { %>
                                    <span class="label label-success">
                                    <% } else if ( sentiment <= -25 ) { %>
                                    <span class="label label-danger">
                                    <% } else { %>
                                    <span class="label label-default">
                                    <% } %>
                                    <%= String.Format("{0:0}", dr["SentimentScore"] ) %>
                                    </span>
                                    <% } %>
                                </td>
                                <td>
                                    <a href="/Display/<%= st.ToString() %>/<%= dr["RecordID"] %>" class="btn btn-primary" target="_blank">View Responses <i class="fa fa-external-link"></i></a>
                                </td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                    </div>
                    <div class="clearfix"></div>
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
<% } %>
</asp:Content>
<asp:Content ContentPlaceHolderID="FooterScripts" runat="server">

</asp:Content>
