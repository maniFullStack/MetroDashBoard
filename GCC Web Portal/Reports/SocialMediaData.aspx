<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="SocialMediaData.aspx.cs" Inherits="GCC_Web_Portal.Reports.SocialMediaData"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,HRStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<% if ( Data == null ) { %>
    <div class="row">
        <div class="col-md-6 col-md-offset-3">
            <div class="box box-danger box-solid">
                <div class="box-header with-border">
                    <h3 class="box-title">Error</h3>
                </div>
                <div class="box-body">
                      Unable to load the data. Please try again.
                </div>
            </div>
        </div>
    </div>
<% } else { %>
    <div class="row">
        <div class="col-md-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-bullhorn"></i>
                    <h3 class="box-title">Social Media & Text Analytics - Data View</h3>
                </div>
                <div class="box-body border-radius-none">
                    <sc:MessageManager ID="mmMessage" runat="server"></sc:MessageManager>
                    <asp:Button Text="Reload Data" runat="server" OnClick="btnReloadData_Click" ID="btnReloadData" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">        
        <div class="col-md-12">
            <div class="box box-info">
                <div class="box-body border-radius-none" style="">
                    <div class="table-responsive">
                    <table class="table">
                        <thead>
                            <tr>
                                <th>Property</th>
                                <th>Keyword</th>
                                <th>Source</th>
                                <th>Sentiment</th>
                                <th>Published</th>
                                <th>URL</th>
                                <th>Title</th>
                                <th>Content</th>
                            </tr>
                        </thead>
                        <tbody>
<% if ( Data.Tables.Count > 0 ) { %>
<% foreach ( DataRow dr in Data.Tables[0].Rows ) { %>
                            <tr>
                                <td><%= ReportingTools.CleanData( dr["property"] ) %></td>
                                <td><%= ReportingTools.CleanData( dr["keyword"] ) %></td>
                                <td><%= ReportingTools.CleanData( dr["source"] ) %></td>
                                <td><%= dr["sentiment"].ToString() %></td>
                                <td><%= dr["published"].ToString() %></td>
                                <td><%= ReportingTools.CleanData( dr["url"] ) %></td>
                                <td><%= ReportingTools.CleanData( dr["title"] ) %></td>
                                <td><%= ReportingTools.CleanData( dr["content"] ) %></td>
                            </tr>
<% } %>
<% } %>
                        </tbody>
                    </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
<% } %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterScripts" runat="server">
</asp:Content>
