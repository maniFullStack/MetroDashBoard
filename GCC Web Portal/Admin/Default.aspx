<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GCC_Web_Portal.Admin.Default"
    AllowedGroups="ForumAdmin,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Survey List</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Administration</li>
    </ol>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<div class="row">
    <div class="col-xs-12">
        <div class="box box-info">
            <div class="box-header with-border">
                <i class="fa fa-user"></i>
                <h3 class="box-title">Admin Pages</h3>
            </div>
            <div class="box-body border-radius-none">
                <p>The following are pages available to the administrators of the website. Click the links to view the reports.</p>
                <ul>
                    <li><a href="/Admin/Abandonment/">Abandonment Report</a></li>
                    <li><a href="/Admin/CrossTabs">Cross Tab Report</a></li>
                    <li><a href="/Admin/WinnerSelection/">Monthly Winner Selection</a></li>
                    <li><a href="/Admin/PINGenerator/">Email PIN / Batch Management</a></li>
                    <li><a href="/Admin/NotificationManagement">Notification Management</a></li>
                    <li><a href="/Admin/Users/">User Management</a></li>
                    <li><a href="/Admin/DataExport">Data Export</a></li>
                </ul>
            </div>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterScripts" runat="server">
</asp:Content>
