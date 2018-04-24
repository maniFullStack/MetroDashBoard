<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="SnapshotExport.aspx.cs" Inherits="GCC_Web_Portal.SnapshotExport"
	AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,HRStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
	<h1>Snapshot Report</h1>
	<ol class="breadcrumb">
		<li><a href="/">Home</a></li>
		<li class="active">Snapshot Report</li>
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
				<p>This page allows you to export the 2017 Snapshot report. Select the filters below and the click "Generate Report" to generate the report.</p>
				<div class="row">
					<div class="col-sm-4">
						<div class="form-group">
							<label for="<%= ddlRegion.ClientID %>">Region</label>
							<% if ( Master.IsPropertyUser ) { %>
							<span class="form-control"><%= PropertyTools.GetCasinoRegion( User.PropertyShortCode ) %></span>
							<% } else { %>
							<asp:DropDownList runat="server" ID="ddlRegion" CssClass="form-control" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged" AutoPostBack="true">
								<asp:ListItem Text="Overall" Value="" />
								<asp:ListItem Text="BC" Value="BC" />
								<asp:ListItem Text="NS" Value="NS" />
								<asp:ListItem Text="ON" Value="ON" />
								<asp:ListItem Text="WA" Value="WA" />
								<asp:ListItem Text="NB" Value="NB" />
							</asp:DropDownList>
							<% } %>
						</div>
					</div>
					<% if ( ddlProperty.Visible ) { %>
					<div class="col-sm-4">
						<div class="form-group">
							<label for="<%= ddlProperty.ClientID %>">Property</label>
							
							<% if ( Master.IsPropertyUser && User.PropertyShortCode != GCCPropertyShortCode.GAG ) { %>
							<span class="form-control"><%= User.PropertyShortCode %></span>
							<% } else { %>
							<asp:DropDownList runat="server" ID="ddlProperty" CssClass="form-control" OnSelectedIndexChanged="ddlProperty_SelectedIndexChanged" AutoPostBack="true">
								<asp:ListItem Value="" Text="All Properties"></asp:ListItem>
						 
		<asp:ListItem Value="21" Text="Bingo Esquimalt"></asp:ListItem>             
		<asp:ListItem Value="7" Text="Casino Nanaimo"></asp:ListItem>
		<asp:ListItem Value="19" Text="Casino New Brunswick"></asp:ListItem>
		<asp:ListItem Value="11" Text="Casino Nova Scotia - Halifax"></asp:ListItem>
		<asp:ListItem Value="12" Text="Casino Nova Scotia - Sydney"></asp:ListItem>
		<asp:ListItem Value="8" Text="Chances Chilliwack"></asp:ListItem>
		<asp:ListItem Value="10" Text="Chances Dawson Creek"></asp:ListItem>
		<asp:ListItem Value="9" Text="Chances Maple Ridge"></asp:ListItem>
		<asp:ListItem Value="14" Text="Elements Casino Surrey"></asp:ListItem>
		<asp:ListItem Value="15" Text="Flamboro Downs"></asp:ListItem>
		<asp:ListItem Value="1" Text="GCGC Corporate Office(s) - BC"></asp:ListItem>
		<asp:ListItem Value="16" Text="Georgian Downs"></asp:ListItem>  
		<asp:ListItem Value="13-1" Text="Great American Casino - Everett"></asp:ListItem>
		<asp:ListItem Value="13-2" Text="Great American Casino - Lakewood"></asp:ListItem>
		<asp:ListItem Value="13-3" Text="Great American Casino - Tukwila"></asp:ListItem>     
		<asp:ListItem Value="13-4" Text="Great American Casino - DesMoines"></asp:ListItem>     
		<asp:ListItem Value="3" Text="Hard Rock Casino Vancouver"></asp:ListItem>
		<asp:ListItem Value="5" Text="Hastings Racetrack & Casino"></asp:ListItem>
		
		<asp:ListItem Value="2" Text="River Rock Casino Resort"></asp:ListItem>
		<asp:ListItem Value="17" Text="Shoreline Slot at Kawartha Downs"></asp:ListItem>
		<asp:ListItem Value="18" Text="Shorlines Casino Thousand Islands"></asp:ListItem>
		<asp:ListItem Value="20" Text="Shorelines Casino Belleville"></asp:ListItem>      
		<asp:ListItem Value="6" Text="Elements Casino Victoria"></asp:ListItem>             

		
							</asp:DropDownList>
							<% } %>
						</div>
					</div>
					<% }
						if ( ddlDepartment.Visible ) { %>
					<div class="col-sm-4">
						<div class="form-group">
							<label>Department</label>
							<asp:DropDownList runat="server" ID="ddlDepartment" CssClass="form-control">

										<asp:ListItem Text="--" Value="" ></asp:ListItem>
		<asp:ListItem Text="All Employees" Value="All Employees"></asp:ListItem>
		<asp:ListItem Text="Accounting / Receiving / Human Resources" Value="Accounting / Receiving / Human Resources"></asp:ListItem>
		<%--<asp:ListItem Text="Administration & Miscellaneous " Value="Administration & Miscellaneous "></asp:ListItem>--%>
		<asp:ListItem Text="Banquets" Value="Banquets"></asp:ListItem>
		<asp:ListItem Text="BC Operations & Development Management" Value="BC Operations & Development Management"></asp:ListItem>
		<asp:ListItem Text="Bingo, Cage & Slots" Value="Bingo, Cage & Slots"></asp:ListItem>
		<asp:ListItem Text="Cage & Countroom" Value="Cage & Countroom"></asp:ListItem>
		<asp:ListItem Text="Cage / Count" Value="Cage / Count"></asp:ListItem>
		<asp:ListItem Text="Cage / Count, Surveillance & Security" Value="Cage / Count, Surveillance & Security"></asp:ListItem>
		<asp:ListItem Text="Cage/Countroom/Guest Services" Value="Cage/Countroom/Guest Services"></asp:ListItem>
		<asp:ListItem Text="Casino: Guest Services" Value="Casino: Guest Services"></asp:ListItem>
		<%--<asp:ListItem Text="Casino Guest Services & Entertainment & Spa" Value="Casino Guest Services & Entertainment & Spa"></asp:ListItem>--%>
		<asp:ListItem Text="Casino Guest Services including GS Manager and Marketing Coordintor" Value="Casino Guest Services including GS Manager and Marketing Coordintor"></asp:ListItem>
		<%--<asp:ListItem Text="Casino Housekeeping" Value="Casino Housekeeping"></asp:ListItem>--%>
		<asp:ListItem Text="Casino Housekeeping including Leads" Value="Casino Housekeeping Including Leads"></asp:ListItem>
		<asp:ListItem Text="Casino Operations" Value="Casino Operations"></asp:ListItem>
		<asp:ListItem Text="Corporate Support Services" Value="Corporate Support Services"></asp:ListItem>
		<asp:ListItem Text="Culinary" Value="Culinary"></asp:ListItem>
		<asp:ListItem Text="Culinary / Food & Beverage Management & Admin" Value="Culinary / Food & Beverage Management & Admin"></asp:ListItem>
		<asp:ListItem Text="Culinary / Stewarding" Value="Culinary / Stewarding"></asp:ListItem>
		<asp:ListItem Text="Executive & Senior Management" Value="Executive & Senior Management"></asp:ListItem>
		<asp:ListItem Text="Exec. Hskper, HGS Mgr, Exec Chef, Fac. Mgr, Buf. Mgr, Pub Mgr, Banq. Mgr, S&C Mgr, S&C Coord, Hot Hskping" Value="Exec. Hskper, HGS Mgr, Exec Chef, Fac. Mgr, Buf. Mgr, Pub Mgr, Banq. Mgr, S&C Mgr, S&C Coord, Hot Hskping"></asp:ListItem>

		<asp:ListItem Text="Facilities" Value="Facilities"></asp:ListItem>
		<asp:ListItem Text="Facilities / Maintenance" Value="Facilities / Maintenance"></asp:ListItem>
		<asp:ListItem Text="Finance & Accounting" Value="Finance & Accounting"></asp:ListItem>
		<asp:ListItem Text="Finance /Database Analyst / Human Resources/IT(TSG)" Value="Finance /Database Analyst / Human Resources/IT(TSG)"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage" Value="Food & Beverage"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage Culinary" Value="Food & Beverage Culinary"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage Buffet including Buffet Sups" Value="Food & Beverage Buffet including Buffet Sups"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage Pub & Beverage including Pub Sups and Bev Hostesses" Value="Food & Beverage Pub & Beverage including Pub Sups and Bev Hostesses"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Outlets" Value="Food & Beverage: Outlets"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Banquets" Value="Food & Beverage: Banquets"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Beverage" Value="Food & Beverage: Beverage"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Restaurants" Value="Food & Beverage: Restaurants"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Gaming" Value="Food & Beverage: Gaming"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Non gaming" Value="Food & Beverage: Non gaming"></asp:ListItem>
		<asp:ListItem Text="Gaming Operations: Bingo, Slots and Cage" Value="Gaming Operations: Bingo, Slots and Cage"></asp:ListItem>
		<asp:ListItem Text="Gaming Operations: Bingo, F&B" Value="Gaming Operations: Bingo, F&B"></asp:ListItem>
		<asp:ListItem Text="Guest Services" Value="Guest Services"></asp:ListItem>
		<asp:ListItem Text="Guest Services & Retail" Value="Guest Services & Retail"></asp:ListItem>
		<asp:ListItem Text="Guest Services & Slots" Value="Guest Services & Slots"></asp:ListItem>
		<asp:ListItem Text="Guest Services/Slots" Value="Guest Services/Slots"></asp:ListItem>
		<%--<asp:ListItem Text="Hotel Ops Mrgs & Sups, Sales & Catering, Fac. Lead, Hotel Hskping Lead" Value="Hotel Ops Mrgs & Sups, Sales & Catering, Fac. Lead, Hotel Hskping Lead"></asp:ListItem>--%>
		<asp:ListItem Text="Hotel: Reservations, Front Office, Concierge and Guest Services" Value="Hotel: Reservations, Front Office, Concierge and Guest Services"></asp:ListItem>
		
		<asp:ListItem Text="Hotel: Res., Concierge and GS, Night Audit, GS Supervisor" Value="Hotel: Res., Concierge and GS, Night Audit, GS Supervisor"></asp:ListItem>
		<asp:ListItem Text="Housekeeping" Value="Housekeeping"></asp:ListItem>		
		<%--<asp:ListItem Text="Hotel Housekeeping" Value="Hotel Housekeeping"></asp:ListItem>--%>
		<asp:ListItem Text="Hotel Housekeeping including Lead and supervisor" Value="Hotel Housekeeping including Lead and supervisor"></asp:ListItem>
		<asp:ListItem Text="Housekeeping/Maintenance/Facilities" Value="Housekeeping/Maintenance/Facilities"></asp:ListItem>
		<asp:ListItem Text="HPI & Customer Service" Value="HPI & Customer Service"></asp:ListItem>
		<asp:ListItem Text="Human Resources & Payroll" Value="Human Resources & Payroll"></asp:ListItem>
		<asp:ListItem Text="Human Resources, Accounting, Maintenance (Property)" Value="Human Resources, Accounting, Maintenance (Property)"></asp:ListItem>
		<asp:ListItem Text="Janitorial & Maintenance" Value="Janitorial & Maintenance"></asp:ListItem>
		
		<%--<asp:ListItem Text="Leadership / Administrative Assistant" Value="Leadership / Administrative Assistant"></asp:ListItem>--%>
		<asp:ListItem Text="Leadership, Admin Assist Mktg Mgr, Cage Mgr, Spa Mgr, Asian Host, Receiving, IT Mgr, Sec. Mgr, db Mgr, Ent." Value="Leadership, Admin Assist Mktg Mgr, Cage Mgr, Spa Mgr, Asian Host, Receiving, IT Mgr, Sec. Mgr, db Mgr, Ent."></asp:ListItem>
		<asp:ListItem Text="Marketing & Player Development" Value="Marketing & Player Development"></asp:ListItem>
		<asp:ListItem Text="Mutuels" Value="Mutuels"></asp:ListItem>
		<asp:ListItem Text="Mutuels, Security & Facilities," Value="Mutuels, Security & Facilities"></asp:ListItem>
		<asp:ListItem Text="Mutuels & Racing" Value="Mutuels & Racing"></asp:ListItem>
		
		<asp:ListItem Text="Operations Management" Value="Operations Management"></asp:ListItem>
		<asp:ListItem Text="Operations Management/Admin" Value="Operations Management/Admin"></asp:ListItem>
		<asp:ListItem Text="Operations Management (HR, Audit, IT, Managers)" Value="Operations Management (HR, Audit, IT, Managers)"></asp:ListItem>
		<asp:ListItem Text="Operations Management & Marketing" Value="Operations Management & Marketing"></asp:ListItem>
		<asp:ListItem Text="Operations Support" Value="Operations Support"></asp:ListItem>
		<asp:ListItem Text="Operations Support (HR/Audit/IT)" Value="Operations Support (HR/Audit/IT)"></asp:ListItem>
		<asp:ListItem Text="Operations Support & Specialists" Value="Operations Support & Specialists"></asp:ListItem>
		<asp:ListItem Text="Ops Management & Department Heads" Value="Ops Management & Department Heads"></asp:ListItem>
		<%--<asp:ListItem Text="Property / Janitorial" Value="Property / Janitorial"></asp:ListItem>--%>
		<asp:ListItem Text="Property Services" Value="Property Services"></asp:ListItem>
		<asp:ListItem Text="Racing and First Aid" Value="Racing and First Aid"></asp:ListItem>
		<%--<asp:ListItem Text="Racing/Race Office" Value="Racing/Race Office"></asp:ListItem>--%>
		<asp:ListItem Text="Resort Management " Value="Resort Management "></asp:ListItem>
		
		<asp:ListItem Text="Sales, Marketing & Player Relations" Value="Sales, Marketing & Player Relations"></asp:ListItem>
		<asp:ListItem Text="Sales, Marketing, Buyer" Value="Sales, Marketing, Buyer"></asp:ListItem>
		<asp:ListItem Text="Security" Value="Security"></asp:ListItem>
		<%--<asp:ListItem Text="Security / Surveillance" Value="Security / Surveillance"></asp:ListItem>--%>
		<asp:ListItem Text="Security (Incl. Event Safety Officers)" Value="Security (Incl. Event Safety Officers)"></asp:ListItem>
		<asp:ListItem Text="Senior Management" Value="Senior Management"></asp:ListItem>
		<asp:ListItem Text="Senior Management (includes DHs)" Value="Senior Management (includes DHs)"></asp:ListItem>
		<asp:ListItem Text="Slot Operations" Value="Slot Operations"></asp:ListItem>
		<asp:ListItem Text="Slots" Value="Slots"></asp:ListItem>
		<asp:ListItem Text="Slots (Includes Slot Techs)" Value="Slots (Includes Slot Techs)"></asp:ListItem>
		<asp:ListItem Text="Slots & Guest Services" Value="Slots & Guest Services"></asp:ListItem>
		<asp:ListItem Text="Spa Aestaticians and Lead" Value="Spa Aestaticians and Lead"></asp:ListItem>
		<asp:ListItem Text="Stewarding" Value="Stewarding"></asp:ListItem>
		<asp:ListItem Text="Surveillance" Value="Surveillance"></asp:ListItem>
		<asp:ListItem Text="Table Games" Value="Table Games"></asp:ListItem>
		<asp:ListItem Text="Table Games & Customer Loyalty" Value="Table Games & Customer Loyalty"></asp:ListItem>
		<asp:ListItem Text="Table Games: Dealers" Value="Table Games: Dealers"></asp:ListItem>
		<asp:ListItem Text="Table Games: Dealers & Dual Supervisors" Value="Table Games: Dealers & Dual Supervisors"></asp:ListItem>
		<asp:ListItem Text="Table Games: Dealer Supervisors" Value="Table Games: Dealer Supervisors"></asp:ListItem>
		<asp:ListItem Text="Table Games Management" Value="Table Games Management"></asp:ListItem>
		<%--<asp:ListItem Text="Table Games Management & Slot Supervisors" Value="Table Games Management & Slot Supervisors"></asp:ListItem>--%>
		<asp:ListItem Text="Table Games Management including CSMs and Pit/Dual Pit Managers, Full Table Sups" Value="Table Games Management including CSMs and Pit/Dual Pit Managers, Full Table Sups"></asp:ListItem>
		<asp:ListItem Text="Table Games Sups/PitMan/CSM" Value="Table Games Sups/PitMan/CSM"></asp:ListItem>
		 
		<asp:ListItem Text="Technology Services Group" Value="Technology Services Group"></asp:ListItem>
		<asp:ListItem Text="Theatre" Value="Theatre"></asp:ListItem>
		<asp:ListItem Text="Theatre: Box Office, Ushers" Value="Theatre: Box Office, Ushers"></asp:ListItem>




								<%--<asp:ListItem Text="All Departments" Value="" ></asp:ListItem>
								<%--<asp:ListItem Text="--" Value="" ></asp:ListItem>--%>
		<%--<asp:ListItem Text="Accounting / Receiving / Human Resources" Value="Accounting / Receiving / Human Resources"></asp:ListItem>
		<asp:ListItem Text="Administration & Miscellaneous " Value="Administration & Miscellaneous "></asp:ListItem>
		<asp:ListItem Text="Banquets" Value="Banquets"></asp:ListItem>
		<asp:ListItem Text="BC Operations & Development Management" Value="BC Operations & Development Management"></asp:ListItem>
		<asp:ListItem Text="Bingo, Cage & Slots" Value="Bingo, Cage & Slots"></asp:ListItem>
		<asp:ListItem Text="Cage & Countroom" Value="Cage & Countroom"></asp:ListItem>
		<asp:ListItem Text="Cage / Count" Value="Cage / Count"></asp:ListItem>
		<asp:ListItem Text="Cage / Countroom / Guest Services" Value="Cage / Countroom / Guest Services"></asp:ListItem>
		<asp:ListItem Text="Casino: Guest Services" Value="Casino: Guest Services"></asp:ListItem>
		<asp:ListItem Text="Casino Guest Services & Entertainment & Spa" Value="Casino Guest Services & Entertainment & Spa"></asp:ListItem>
		<asp:ListItem Text="Casino Housekeeping" Value="Casino Housekeeping"></asp:ListItem>
		<asp:ListItem Text="Casino Operations" Value="Casino Operations"></asp:ListItem>
		<asp:ListItem Text="Corporate Support Services" Value="Corporate Support Services"></asp:ListItem>
		<asp:ListItem Text="Culinary" Value="Culinary"></asp:ListItem>
		<asp:ListItem Text="Culinary / Food & Beverage Management & Admin" Value="Culinary / Food & Beverage Management & Admin"></asp:ListItem>
		<asp:ListItem Text="Culinary / Stewarding" Value="Culinary / Stewarding"></asp:ListItem>
		<asp:ListItem Text="Executive & Senior Management" Value="Executive & Senior Management"></asp:ListItem>
		<asp:ListItem Text="Facilities" Value="Facilities"></asp:ListItem>
		<asp:ListItem Text="Facilities / Maintenance" Value="Facilities / Maintenance"></asp:ListItem>
		<asp:ListItem Text="Finance & Accounting" Value="Finance & Accounting"></asp:ListItem>
		<asp:ListItem Text="Finance / Receiving / Human Resources / IT(TSG)" Value="Finance / Receiving / Human Resources / IT(TSG)"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage" Value="Food & Beverage"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage Management (Including Culinary)" Value="Food & Beverage Management (Including Culinary)"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Outlets" Value="Food & Beverage: Outlets"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Banquets" Value="Food & Beverage: Banquets"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Beverage" Value="Food & Beverage: Beverage"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Restaurants" Value="Food & Beverage: Restaurants"></asp:ListItem>
		<asp:ListItem Text="Gaming Operations: Bingo, Slots and Cage" Value="Gaming Operations: Bingo, Slots and Cage"></asp:ListItem>
		<asp:ListItem Text="Guest Services" Value="Guest Services"></asp:ListItem>
		<asp:ListItem Text="Guest Services & Retail" Value="Guest Services & Retail"></asp:ListItem>
		<asp:ListItem Text="Guest Services & Slots" Value="Guest Services & Slots"></asp:ListItem>
		<asp:ListItem Text="Guest Services/Slots" Value="Guest Services/Slots"></asp:ListItem>
		<asp:ListItem Text="Hotel Ops Mrgs & Sups, Sales & Catering, Fac. Lead, Hotel Hskping Lead" Value="Hotel Ops Mrgs & Sups, Sales & Catering, Fac. Lead, Hotel Hskping Lead"></asp:ListItem>
		<asp:ListItem Text="Hotel: Reservations, Front Office, Concierge and Guest Services" Value="Hotel: Reservations, Front Office, Concierge and Guest Services"></asp:ListItem>
		<asp:ListItem Text="Housekeeping" Value="Housekeeping"></asp:ListItem>		
		<asp:ListItem Text="Hotel Housekeeping" Value="Hotel Housekeeping"></asp:ListItem>
		<asp:ListItem Text="Housekeeping/Maintenance/Facilities" Value="Housekeeping/Maintenance/Facilities"></asp:ListItem>
		<asp:ListItem Text="HPI & Customer Service" Value="HPI & Customer Service"></asp:ListItem>
		<asp:ListItem Text="Human Resources & Payroll" Value="Human Resources & Payroll"></asp:ListItem>
		<asp:ListItem Text="Leadership / Administrative Assistant" Value="Leadership / Administrative Assistant"></asp:ListItem>
		<asp:ListItem Text="Mktg & Plyr Dev/Spa Mgr & Sup/CSG Mgr/Cage & Count Mgr & Sups/DB Mgr" Value="Mktg & Plyr Dev/Spa Mgr & Sup/CSG Mgr/Cage & Count Mgr & Sups/DB Mgr"></asp:ListItem>
		<asp:ListItem Text="Mutuels" Value="Mutuels"></asp:ListItem>
		<asp:ListItem Text="Mutuels & Facilities" Value="Mutuels & Facilities"></asp:ListItem>
		<asp:ListItem Text="Mutuels & Racing" Value="Mutuels & Racing"></asp:ListItem>
		<asp:ListItem Text="Operations Management" Value="Operations Management"></asp:ListItem>
		<asp:ListItem Text="Operations Management (HR, Audit, IT, Managers)" Value="Operations Management (HR, Audit, IT, Managers)"></asp:ListItem>
		<asp:ListItem Text="Operations Management & Marketing" Value="Operations Management & Marketing"></asp:ListItem>
		<asp:ListItem Text="Operations Support" Value="Operations Support"></asp:ListItem>
		<asp:ListItem Text="Operations Support & Specialists" Value="Operations Support & Specialists"></asp:ListItem>
		<asp:ListItem Text="Ops Management & Department Heads" Value="Ops Management & Department Heads"></asp:ListItem>--%>
		<%-- old<asp:ListItem Text="Property / Janitorial" Value="Property / Janitorial"></asp:ListItem>--%>
		<%--<asp:ListItem Text="Property Services" Value="Property Services"></asp:ListItem>
		<asp:ListItem Text="Racing and First Aid" Value="Racing and First Aid"></asp:ListItem>--%>
		<%--old<asp:ListItem Text="Racing/Race Office" Value="Racing/Race Office"></asp:ListItem>--%>
		<%--<asp:ListItem Text="Resort Management" Value="Resort Management"></asp:ListItem>
		<asp:ListItem Text="Sales, Marketing & Player Relations" Value="Sales, Marketing & Player Relations"></asp:ListItem>
		<asp:ListItem Text="Security" Value="Security"></asp:ListItem>
		<asp:ListItem Text="Security / Surveillance" Value="Security / Surveillance"></asp:ListItem>
		<asp:ListItem Text="Senior Management" Value="Senior Management"></asp:ListItem>
		<asp:ListItem Text="Slot Operations" Value="Slot Operations"></asp:ListItem>
		<asp:ListItem Text="Slots" Value="Slots"></asp:ListItem>
		<asp:ListItem Text="Slots (Includes Slot Techs)" Value="Slots (Includes Slot Techs)"></asp:ListItem>
		<asp:ListItem Text="Slots & Guest Services" Value="Slots & Guest Services"></asp:ListItem>
		<asp:ListItem Text="Stewarding" Value="Stewarding"></asp:ListItem>
		<asp:ListItem Text="Surveillance" Value="Surveillance"></asp:ListItem>
		<asp:ListItem Text="Table Games" Value="Table Games"></asp:ListItem>
		<asp:ListItem Text="Table Games & Customer Loyalty" Value="Table Games & Customer Loyalty"></asp:ListItem>
		<asp:ListItem Text="Table Games: Dealers" Value="Table Games: Dealers"></asp:ListItem>
		<asp:ListItem Text="Table Games: Dealer Supervisors" Value="Table Games: Dealer Supervisors"></asp:ListItem>
		<asp:ListItem Text="Table Games Management" Value="Table Games Management"></asp:ListItem>
		<asp:ListItem Text="Table Games Management & Slot Supervisors" Value="Table Games Management & Slot Supervisors"></asp:ListItem>
		<asp:ListItem Text="Technology Services Group" Value="Technology Services Group"></asp:ListItem>
		<asp:ListItem Text="Theatre" Value="Theatre"></asp:ListItem>
		<asp:ListItem Text="Theatre: Box Office, Ushers" Value="Theatre: Box Office, Ushers"></asp:ListItem>--%>
						   
							
							
							
							
							
							</asp:DropDownList>
						</div>
					</div>
					<% } %>
				</div>
				<div class="row">
					<div class="col-sm-offset-4 col-sm-4 text-center">
						<asp:Button runat="server" ID="btnGenerateReport" Text="Generate Report" CssClass="btn btn-primary" OnClick="btnGenerateReport_Click" />
						<% if ( ddlRegion.SelectedValue.Equals( "BC" ) && String.IsNullOrWhiteSpace( ddlProperty.SelectedValue ) ) { %>
						<asp:Button runat="server" ID="btnGenerateSurveillanceReport" Text="Generate Surveillance Report" CssClass="btn btn-success" OnClick="btnGenerateSurveillanceReport_Click" />
						<% } %>
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
<asp:Content ID="Content5" ContentPlaceHolderID="FooterScripts" runat="server"></asp:Content>
