<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="EmailPINGenerator.aspx.cs" Inherits="GCC_Web_Portal.Admin.EmailPINGenerator"
    AllowedGroups="ForumAdmin,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
    .table > tbody > tr > td {
        text-align:left;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Email PIN Generator</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/Admin/">Admin</a></li>
        <% if ( BatchID == -1 ) { %>
        <li class="active">Email PIN Batch List</li>
        <% } else { %>
        <li><a href="/Admin/PINGenerator/">Email PIN Batch List</a></li>
        <li class="active"><%= Data.Rows[0]["BatchName"] %></li>
        <% } %>
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
<div class="row">
    <% if ( BatchID == -1 ) {
    // Batch listing page
    %>
    <div class="col-xs-12 col-md-6">
        <div class="box box-info">
            <div class="box-header with-border">
                <i class="fa fa-thumb-tack"></i>
                <h3 class="box-title">Email PIN Generator</h3>
            </div>
            <div class="box-body border-radius-none">
                <p>In order to generate PINs, you must first create a "batch". This allows us to track when the email PINs were generated and what they were for. This page allows you to add and edit the GEI email batches. To download the file with PINs, select the batch below.</p>
                <div class="row" style="margin-bottom:5px;"> 
                    <div class="col-sm-6">
                        <div class="input-group">
                            <asp:TextBox runat="server" ID="txtNewBatch" MaxLength="50" CssClass="form-control" placeholder="Batch Name (i.e. July 2015, Week 2)"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:Button runat="server" ID="btnCreateBatch" Text="Create Batch" CssClass="btn btn-primary" OnClick="btnCreateBatch_Click" />
                            </span>
                        </div>
                    </div>
                </div>
                <% if ( Data == null || Data.Rows.Count == 0 ) { %>
                No batches exist, yet.
                <% } else { %>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Date Created</th>
                            <th>Created By</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        <% foreach( DataRow dr in Data.Rows ) { %>
                        <tr>
                            <td><%= ReportingTools.CleanData( dr["BatchName"] ) %></td>
                            <td><%= ReportingTools.AdjustAndDisplayDate( dr["DateCreated"].ToString(), ConversionDateFormatType.LongDate, User ) %></td>
                            <td><%= ReportingTools.CleanData( dr["FirstName"] ) %> <%= ReportingTools.CleanData( dr["LastName"] ) %></td>
                            <td>
                                <a href="/Admin/PINGenerator/<%= dr["EmailBatchID"] %>" class="btn btn-primary">Details</a>
                            </td>
                        </tr>
                        <% } %>
                    </tbody>
                </table>
                <% } %>
            </div>
        </div>
    </div>
    <% } else if ( Data != null ) {
    // Specific batch
    DataRow dr = Data.Rows[0];
    %>
    <div class="col-xs-12 col-md-6">
        <div class="box box-info">
            <div class="box-header with-border">
                <i class="fa fa-thumb-tack"></i>
                <h3 class="box-title">Batch Details</h3>
            </div>
            <div class="box-body border-radius-none">
                
                <table class="table table-bordered table-striped">
                    <tbody>
                        <tr>
                            <th style="width:40%">Name</th>
                            <td><%= ReportingTools.CleanData( dr["BatchName"] ) %></td>
                        </tr>
                        <tr>
                            <th>Date Created</th>
                            <td><%= ReportingTools.AdjustAndDisplayDate( dr["DateCreated"].ToString(), ConversionDateFormatType.LongDate, User ) %></td>
                        </tr>
                        <tr>
                            <th>Created By</th>
                            <td><%= ReportingTools.CleanData( dr["FirstName"] ) %> <%= ReportingTools.CleanData( dr["LastName"] ) %></td>
                        </tr>
                        <tr>
                            <th>Number of Records</th>
                            <td>
                                <%= dr["PINCount"] %>
                                <% if ( !dr["PINCount"].Equals( 0 ) ) { %>
                                &nbsp;&nbsp;<a href="<%= Config.PINFileDirectory.TrimStart('~') + GetFileName() %>" class="btn btn-primary">Download PIN File</a>
                                <% } %>
                            </td>
                        </tr>
                        <tr>
                            <th><label for="<%= txtBatchNotes.ClientID %>">Notes</label></th>
                            <td>
                                <asp:TextBox runat="server" ID="txtBatchNotes" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
                                <br />
                                <asp:Button runat="server" ID="btnSaveBatchNotes" Text="Save Notes" OnClick="btnSaveBatchNotes_Click" CssClass="btn btn-success"/>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <% if ( dr["PINCount"].Equals( 0 ) ) { %>
    <div class="col-xs-12 col-md-6">
        <div class="box box-warning">
            <div class="box-header with-border">
                <i class="fa fa-cloud-upload"></i>
                <h3 class="box-title">File Management</h3>
            </div>
            <div class="box-body border-radius-none">
                <p>To generate (or update) the email PIN listing for this batch, submit the file below.</p>
                <p>The file must be in Excel format and contain the following three columns:</p>
                <ol>
                    <li><b>Email</b>: The email addresses to generate PINs for.</li>
                    <li><b>Location</b>: The shortcode of the location for this address. This is used to brand the survey appropriately. Valid values are: <abbr title="River Rock Casino Resort">RR</abbr>, <abbr title="Hard Rock Casino Vancouver">HRCV</abbr>, <abbr title="Hastings Racetrack & Casino">HA</abbr>, <abbr title="View Royal Casino">VRL</abbr>, <abbr title="Casino Nanaimo">NAN</abbr>, <abbr title="Chances Chilliwack">CCH</abbr>, <abbr title="Chances Maple Ridge">CMR</abbr>, <abbr title="Chances Dawson Creek">CDC</abbr>, <abbr title="Casino Nova Scotia - Halifax">CNSH</abbr>, <abbr title="Casino Nova Scotia - Sydney">CNSS</abbr>, <abbr title="Great American Casino">GAG</abbr>, <abbr title="Elements Casino">EC</abbr>, <abbr title="Shorelines Slots at Kawartha Downs">SSKD</abbr>, <abbr title="Shorelines Casino Thousand Islands">SCTI</abbr>, <abbr title="Casino New Brunswick">CNB</abbr>, <abbr title="Shorelines Casino Bellevile">SCBE</abbr>, <abbr title="Casino Woodbine">WDB</abbr>, <abbr title="Casino Ajax">AJA</abbr>, <abbr title="Great Blue Heron Casino">GBH</abbr>.</li>
                    <%--<li><b>Location</b>: The shortcode of the location for this address. This is used to brand the survey appropriately. Valid values are: <abbr title="River Rock Casino Resort">RR</abbr>, <abbr title="Hard Rock Casino Vancouver">HRCV</abbr>, <abbr title="Fraser Downs Racetrack & Casino">FD</abbr>, <abbr title="Hastings Racetrack & Casino">HA</abbr>, <abbr title="View Royal Casino">VRL</abbr>, <abbr title="Casino Nanaimo">NAN</abbr>, <abbr title="Chances Chilliwack">CCH</abbr>, <abbr title="Chances Maple Ridge">CMR</abbr>, <abbr title="Chances Dawson Creek">CDC</abbr>, <abbr title="Casino Nova Scotia - Halifax">CNSH</abbr>, <abbr title="Casino Nova Scotia - Sydney">CNSS</abbr>, <abbr title="Great American Casino">GAG</abbr>, <abbr title="Elements Casino">EC</abbr>, <abbr title="Shorelines Slots at Kawartha Downs">SSKD</abbr>, <abbr title="Shorelines Casino Thousand Islands">SCTI</abbr>.</li>--%>
                    <li><b>Encore</b>: The encore number associated with the email address. Only numbers and blanks will be accepted in this column.</li>
                    <li><b>GSEISurvey</b>: (Optional) Only used for GSEI surveys. <strong>Do not include the column for regular GEI surveys.</strong> Should be one of the following values: <abbr title="Any site">BC</abbr>, <abbr title="Horse Racing: FD/EC + HA">HPI</abbr>, <abbr title="Hotel: RR only">Hotel</abbr>, <abbr title="TicketMaster: RR + HRCV">TicketMaster</abbr>, <abbr title="Greate American Casino Only">Great American</abbr>.</li>
                </ol>
                <p>Click <a href="/Files/EmailPINTemplate.xlsx">here</a> to download the template file.</p>
                <div class="form-group">
                    <label for="<%= fuEmailFile.ClientID %>">Select File:</label>
                    <asp:FileUpload runat="server" ID="fuEmailFile" CssClass="form-control" />
                </div>
                <asp:Button Text="Upload & Validate File" runat="server" ID="btnUpload" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
            </div>
        </div>
    </div>
    <% } %>
    <% } %>
</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterScripts" runat="server"></asp:Content>
