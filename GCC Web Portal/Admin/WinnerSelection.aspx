<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="WinnerSelection.aspx.cs" Inherits="GCC_Web_Portal.Admin.WinnerSelection"
    AllowedGroups="ForumAdmin,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Monthly Winner Selection</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/Admin/">Admin</a></li>
        <li class="active">Monthly Winner Selection</li>
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
                <i class="fa fa-trophy"></i>
                <h3 class="box-title">Winner Export</h3>
            </div>
            <div class="box-body border-radius-none">
                <p>This page allows you to export the monthly winner report. Select the month below and the click "Export" to generate the file.</p>
                <div class="row">
                    <div class="col-xs-6">
                        <div class="form-group">
                            <label for="<%= ddlMonthYear.ClientID %>">Month</label>
                            <asp:DropDownList runat="server" ID="ddlMonthYear" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xs-6">
                        <asp:Button Text="Export" runat="server" ID="btnExport" OnClick="btnExport_Click" CssClass="btn btn-primary" style="margin-top:25px;" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterScripts" runat="server"></asp:Content>
