<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="CrossTabReport.aspx.cs" Inherits="GCC_Web_Portal.Admin.CrossTabReport"
    AllowedGroups="ForumAdmin,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Cross-Tab Report</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Cross-Tab Report</li>
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
    <div class="col-xs-12">
        <div class="box box-info">
            <div class="box-header with-border">
                <i class="fa fa-filter"></i>
                <h3 class="box-title">Filters</h3>
            </div>
            <div class="box-body border-radius-none">
                <p>This page allows you to run cross tabs between questions on the GEI survey. Select the filters below and the click "Calculate" to generate the data.</p>
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group" id="date-range-filter">
                            <label>Date Range</label><br />
                            <uc1:DateRangeFilterControl runat="server" ID="drDateRange" SessionKey="fltDateRange" Label="Choose..." />
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <label>Export?</label><br />
                            <asp:DropDownList runat="server" ID="ddlExport" CssClass="form-control">
                                <asp:ListItem Text="No" />
                                <asp:ListItem Text="Yes" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-group">
                            <label for="<%= ddlQuestion1.ClientID %>">Header Question</label>
                            <asp:DropDownList runat="server" ID="ddlQuestion1" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-group">
                            <label for="<%= ddlQuestion2.ClientID %>">Row Question</label>
                            <asp:DropDownList runat="server" ID="ddlQuestion2" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="text-center">
                        <asp:Button runat="server" ID="btnExport" Text="Calculate" OnClick="btnExport_Click" CssClass="btn btn-primary" />
                        <br /><br />
			            <% if (hlDownload.Text.Length > 0) { %>
                        <asp:HyperLink ID="hlDownload" runat="server" CssClass="btn btn-success"></asp:HyperLink>
                        <br /><br />
			            <% } %>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<% if ( Data != null ) { %>
<div class="row">
    <div class="col-xs-12 col-md-8 col-md-offset-2">
        <div class="box box-success">
            <div class="box-header with-border">
                <i class="fa fa-th-list"></i>
                <h3 class="box-title">Results</h3>
            </div>
            <div class="box-body border-radius-none">
                <table class="table table-bordered table-striped">
                    <% double totalCount = Data.Rows[0]["RowBase"].ToString().StringToDbl(); %>
                    <thead>
                        <tr>
                            <th rowspan="2"></th>
                            <th rowspan="2">Base</th>
                            <th colspan="<%= HeaderAnswers.Count %>"><%= HeaderRow["LongLabel"].ToString() + ( !HeaderRow["ShortLabel"].Equals( DBNull.Value ) ? "<br />" + HeaderRow["ShortLabel"].ToString() : String.Empty ) %></th>
                        </tr>
                        <tr>
                            <% foreach ( Answer a in HeaderAnswers ) { %>
                            <th><%= a.Label %></th>
                            <% } %>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <th>Base</th>
                            <th style="text-align:center"><%= totalCount %></th>
                            <% DataRow baseRow = Data.Rows[0];
                            foreach ( Answer a in HeaderAnswers ) {
                                Response.Write( GetCellValue( totalCount, baseRow[a.Label] ) );
                            } %>
                        </tr>
                        <tr>
                            <th><%= RowRow["LongLabel"].ToString() + ( !RowRow["ShortLabel"].Equals( DBNull.Value ) ? "<br />" + RowRow["ShortLabel"].ToString() : String.Empty ) %></th>
                            <th colspan="<%= HeaderAnswers.Count + 1 %>"></th>
                        </tr>
                        <% for ( int i = 1; i < Data.Rows.Count; i++ ) {
                               DataRow dr = Data.Rows[i];
                        %>
                        <tr>
                            <th><%= dr["Label"] %></th>
                            <%= GetCellValue( totalCount, dr["RowBase"] ) %>
                            <% foreach ( Answer a in HeaderAnswers ) {
                                Response.Write( GetCellValue( totalCount, dr[a.Label] ) );
                            } %>
                        </tr>
                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>
<% } %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterScripts" runat="server">
    <script>
        $('#date-range-filter button.drpicker').each(function (i, elem) {
            var $elem = $(elem),
                dateFormat = 'DD/MM/YYYY',
                $beg = $elem.siblings("input[id$='hdnBegin']"),
                $end = $elem.siblings("input[id$='hdnEnd']"),
                s = moment($beg.val(), dateFormat),
                e = moment($end.val(), dateFormat),
                startDate = s.isValid() ? s : moment().subtract(1, 'month').startOf('month'),
                endDate = e.isValid() ? e : moment().endOf('month');
            $beg.val(startDate.format(dateFormat));
            $end.val(endDate.format(dateFormat));
            $(elem).daterangepicker({
                "locale": {
                    "format": dateFormat
                },
                "ranges": {
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                    'This Year': [moment().startOf('year'), moment()],
                    'Last Year': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')]
                },
                "startDate": startDate,
                "endDate": endDate,
                "maxDate": moment(),
                "opens": "right",
                "drops": "down"
            }, function (start, end) {
                $(this.element[0]).siblings("input[id$='hdnBegin']").val(start.format('DD/MM/YYYY'));
                $(this.element[0]).siblings("input[id$='hdnEnd']").val(end.format('DD/MM/YYYY'));
            });
        });
    </script>
</asp:Content>
