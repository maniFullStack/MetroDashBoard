<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="MonthlyReport.aspx.cs" Inherits="GCC_Web_Portal.Reports.Hotel.MonthlyReport"
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
    <h1><i class="fa fa-calendar"></i> Monthly Hotel Report</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Monthly Hotel Report</li>
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
                <p>This page allows you to export the monthly report for a location. Select the filters below and the click "Export" to generate the file. This may take some time so please be patient.</p>
                <div class="row">
                    <div class="col-sm-6 col-md-4">
                        <div class="form-group">
                            <label>Month</label><br />
                            <asp:DropDownList runat="server" ID="ddlMonth" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-4">
                        <div class="form-group">
                            <label>Property</label>
                            <% if ( Master.IsPropertyUser ) { %>
                            <span class="form-control"><%= PropertyTools.GetCasinoName( (int)User.PropertyShortCode ) %></span>
                            <% } else { %>
                            <asp:DropDownList runat="server" ID="ddlProperty" CssClass="form-control">
                                <asp:ListItem Text="River Rock Casino Resort" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Hard Rock Casino Vancouver" Value="3"></asp:ListItem>
                                 <asp:ListItem Text="Elements Casino Surrey" Value="14"></asp:ListItem>
                                <asp:ListItem Text="Hastings Racetrack & Casino" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Elements Casino Victoria" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Casino Nanaimo" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Chances Chilliwack" Value="8"></asp:ListItem>
                                <asp:ListItem Text="Chances Maple Ridge" Value="9"></asp:ListItem>
                                <asp:ListItem Text="Chances Dawson Creek" Value="10"></asp:ListItem>
                                <asp:ListItem Text="Casino Nova Scotia - Halifax" Value="11"></asp:ListItem>
                                <asp:ListItem Text="Casino Nova Scotia - Sydney" Value="12"></asp:ListItem>
                                <asp:ListItem Text="Great American Casino" Value="13"></asp:ListItem>                               
                                <asp:ListItem Text="Shorelines Slots at Kawartha Downs" Value="17"></asp:ListItem>
                                <asp:ListItem Text="Shorelines Casino Thousand Islands" Value="18"></asp:ListItem>
                                <asp:ListItem Text="Casino New Brunswick" Value="19"></asp:ListItem>
                                <asp:ListItem Text="Shorelines Casino Belleville" Value="20"></asp:ListItem>
                            </asp:DropDownList>
                            <% } %>
                        </div>
                    </div>
                    <div class="col-sm-12 col-md-4 text-center">
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
        $('#<%= btnExport.ClientID %>').on('click', function (evt) {
            $(this).slideToggle();
            $(this).siblings('.wait-message').slideToggle();
        });
    </script>
</asp:Content>
