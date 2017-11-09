<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="QuarterlyReport.aspx.cs" Inherits="GCC_Web_Portal.QuarterlyReport"
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
    <h1><i class="fa fa-pie-chart"></i> Quarterly Report</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Quarterly Report</li>
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
    <div class="col-xs-12 col-sm-6">
        <div class="box box-info">
            <div class="box-body border-radius-none">
                <p>This page allows you to export the latest quarterly report for all locations. Click the button below and the file will be generated. This may take some time so please be patient.</p>
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterScripts" runat="server">
    <script>
        $('#<%= btnExport.ClientID %>').on('click', function (evt) {
            $(this).slideToggle();
            $(this).siblings('.wait-message').slideToggle();
        });
    </script>
</asp:Content>
