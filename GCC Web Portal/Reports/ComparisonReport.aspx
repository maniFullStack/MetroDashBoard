<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="ComparisonReport.aspx.cs" Inherits="GCC_Web_Portal.Reports.ComparisonReport"
	AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style>
		.wait-message {
			display:none;
			margin-bottom:15px;
		}
	</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
	<h1>Date Range Comparison Report</h1>
	<ol class="breadcrumb">
		<li><a href="/">Home</a></li>
		<li class="active">Date Range Comparison Report</li>
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
	<div class="col-xs-12 col-md-6">
		<div class="box box-info">
			<div class="box-header with-border">
				<i class="fa fa-filter"></i>
				<h3 class="box-title">Filters</h3>
			</div>
			<div class="box-body border-radius-none">
				<p>This page allows you to generate a comparison of the scores between two periods of time. Select the filters below and the click "Export" to generate the file.</p>
				<div class="row">
					<div class="col-sm-4">
						<div class="form-group date-range-filter">
							<label>First Date Range</label><br />
							<uc1:DateRangeFilterControl runat="server" ID="drDateRangeFirst" SessionKey="fltDateRange" Label="Choose..." DefaultBeginDate="2016/01/01" DefaultEndDate="2016/12/31" />
						</div>
					</div>
					<div class="col-sm-4">
						<div class="form-group date-range-filter">
							<label>Second Date Range</label><br />
							<uc1:DateRangeFilterControl runat="server" ID="drDateRangeSecond" SessionKey="fltDateRange2" Label="Choose..." DefaultBeginDate="2016/01/01" DefaultEndDate="2016/12/31"/>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="text-center">
						<asp:Button runat="server" ID="btnExport" Text="Export" OnClick="btnExport_Click" CssClass="btn btn-primary" />
						<br />
						<span class="wait-message label label-waring">Please wait. This may take some time to run...</span>
						<br />
						<% if (hlDownload.Text.Length > 0) { %>
						<asp:HyperLink ID="hlDownload" runat="server" CssClass="btn btn-success"></asp:HyperLink>
						<% } %>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterScripts" runat="server">
	<script>
		$('div.date-range-filter button.drpicker').each(function (i, elem) {
			var $elem = $(elem),
				dateFormat = 'DD/MM/YYYY',
				$beg = $elem.siblings("input[id$='hdnBegin']"),
				$end = $elem.siblings("input[id$='hdnEnd']"),
				s = moment($beg.val(), dateFormat),
				e = moment($end.val(), dateFormat),
				startDate = s.isValid() ? s : moment().subtract(1, 'month').startOf('month'),
				endDate = e.isValid() ? e : moment().endOf('month');
			if (i == 1) {
				startDate = s.isValid() ? s : moment().startOf('month');
				endDate = e.isValid() ? e : moment().endOf('month');
			}
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
		$('#<%= btnExport.ClientID %>').on('click', function (evt) {
			$(this).slideToggle();
			$(this).siblings('.wait-message').slideToggle();
		});
	</script>
</asp:Content>
