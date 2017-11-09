<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FeedbackReport.aspx.cs" Inherits="GCC_Web_Portal.Reports.FeedbackReport" 
   %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Feedback Export</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Feedback Export</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
                 <div class="box-body border-radius-none">
                <p>This page allows you to export the data for Feedback Items. Select the filters in the sidebar and the click "Export" to generate the file.</p>
                <div class="row">
                   <%-- <div class="col-sm-4">
                        <div class="form-group" id="date-range-filter">
                            <label>Date Range</label><br />
                            <uc1:DateRangeFilterControl runat="server" ID="drDateRange" SessionKey="fltDateRange" Label="Choose..." />
                        </div>
                    </div> --%>                   
                </div>
                <div class="row">
                    <div class="text-center">
                        <asp:Button runat="server" ID="btnExport" Text="Export" OnClick="btnExport_Click" CssClass="btn btn-primary" />
                        <br /><br />
			            <% if (hlDownload.Text.Length > 0) { %>
                        <asp:HyperLink ID="hlDownload" runat="server" CssClass="btn btn-success"></asp:HyperLink>
			            <% } %>
                    </div>
                </div>
            </div>
        </div>
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
