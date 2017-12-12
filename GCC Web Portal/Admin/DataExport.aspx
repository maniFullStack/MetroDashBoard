<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="DataExport.aspx.cs" Inherits="GCC_Web_Portal.Admin.DataExport"
	AllowedGroups="ForumAdmin,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
	<h1>Data Export</h1>
	<ol class="breadcrumb">
		<li><a href="/">Home</a></li>
		<li class="active">Data Export</li>
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
				<p>This page allows you to export the data for a specific survey. Select the filters below and the click "Export" to generate the file.</p>
				<div class="row">
					<div class="col-sm-4">
						<div class="form-group" id="date-range-filter">
							<label>Date Range</label><br />
							<uc1:DateRangeFilterControl runat="server" ID="drDateRange" SessionKey="fltDateRange" Label="Choose..." />
						</div>
					</div>
					<div class="col-sm-4">
						<div class="form-group">
							<label>Survey</label>
							<asp:DropDownList runat="server" ID="ddlSurvey" CssClass="form-control" AutoPostBack="true">
								<asp:ListItem Text="GEI" Value="1"></asp:ListItem>
								<asp:ListItem Text="Hotel" Value="2"></asp:ListItem>
								<asp:ListItem Text="Feedback" Value="3"></asp:ListItem>
								<asp:ListItem Text="Donation - Sponsorship" Value="4"></asp:ListItem>
								<asp:ListItem Text="Snapshot" Value="5"></asp:ListItem>
								<asp:ListItem Text="Hastings Racecourse" Value="7"></asp:ListItem>
							</asp:DropDownList>
						</div>
					</div>
					<% if (ddlSurvey.SelectedValue != "7") { %>
					<div class="col-sm-4">
						<div class="form-group">
							<label>Property</label>
							<asp:DropDownList runat="server" ID="ddlProperty" CssClass="form-control">
								<asp:ListItem Text="All" Value=""></asp:ListItem>
								<asp:ListItem Text="River Rock Casino Resort" Value="2"></asp:ListItem>
								<asp:ListItem Text="Hard Rock Casino Vancouver" Value="3"></asp:ListItem>
								<asp:ListItem Text="Elements Casino" Value="14"></asp:ListItem>
							   <%-- <asp:ListItem Text="Fraser Downs Racetrack & Casino" Value="4"></asp:ListItem>--%>
								<asp:ListItem Text="Hastings Racetrack & Casino" Value="5"></asp:ListItem>
								<asp:ListItem Text="View Royal Casino" Value="6"></asp:ListItem>
								<asp:ListItem Text="Casino Nanaimo" Value="7"></asp:ListItem>
								<asp:ListItem Text="Chances Chilliwack" Value="8"></asp:ListItem>
								<asp:ListItem Text="Chances Maple Ridge" Value="9"></asp:ListItem>
								<asp:ListItem Text="Chances Dawson Creek" Value="10"></asp:ListItem>
								<asp:ListItem Text="Casino Nova Scotia - Halifax" Value="11"></asp:ListItem>
								<asp:ListItem Text="Casino Nova Scotia - Sydney" Value="12"></asp:ListItem>
								<asp:ListItem Text="Great American Casino" Value="13"></asp:ListItem>
								<asp:ListItem Text="Flamboro Downs" Value="15"></asp:ListItem>                                
								<asp:ListItem Text="Shorelines Slots at Kawartha Downs" Value="17"></asp:ListItem>
								<asp:ListItem Text="Shorelines Casino Thousand Islands" Value="18"></asp:ListItem>
								<asp:ListItem Text="Casino New Brunswick" Value="19"></asp:ListItem>
								<asp:ListItem Text="Shorelines Casino Belleville" Value="20"></asp:ListItem>
								<asp:ListItem Text="Casino Woodbine" Value="22"></asp:ListItem>
								<asp:ListItem Text="Casino Ajax" Value="23"></asp:ListItem>
								<asp:ListItem Text="Great Blue Heron Casino" Value="24"></asp:ListItem>
							</asp:DropDownList>
						</div>
					</div>
					<% } %>
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
