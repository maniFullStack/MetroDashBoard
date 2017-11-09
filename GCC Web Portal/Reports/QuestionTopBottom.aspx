<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="QuestionTopBottom.aspx.cs" Inherits="GCC_Web_Portal.QuestionTopBottom"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Question Top / Bottom Report</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Question Top / Bottom</li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<% if ( Data == null || Data.Rows.Count == 0 ) { %>
<div class="row">
    <div class="col-md-6 col-md-offset-3">
        <div class="box box-danger box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">Error</h3>
            </div>
            <div class="box-body">
                <% if ( Data == null ) { %>
                Unable to load the data. Please try again.
                <% } else { %>
                No data was found for these filters.
                <% } %>
            </div>
        </div>
    </div>
</div>
<% } else { %>
    <div class="row">
        <div class="col-xs-12 col-md-6">
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-arrow-up"></i>
                    <h3 class="box-title">Top 10 Questions</h3>
                </div>
                <div class="box-body border-radius-none">
                    <table class="table table-bordered table-striped" style="width:100%">
                        <thead>
                            <tr>
                                <th style="width:60%">Question</th>
                                <th style="width:20%">Score</th>
                                <th style="width:20%">Count</th>
                            </tr>
                        </thead>
                        <tbody>
                            <% for( int i = 0; i < 10; i++ ) {
                                   if ( Data.Rows.Count <= i ) {
                                       break;
                                   }
                                   DataRow dr = Data.Rows[i];
                            %>
                            <tr>
                                <th><%= ReportingTools.CleanData( dr["Question"] ) %>. <%= ReportingTools.CleanData( dr["QuestionText"] ) %></th>
                                <td><%= ReportingTools.FormatPercent( dr["Score"].ToString() ) %></td>
                                <td><%= dr["Count"] %></td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-6">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-arrow-down"></i>
                    <h3 class="box-title">Bottom 10 Questions</h3>
                </div>
                <div class="box-body border-radius-none">
                    <table class="table table-bordered table-striped" style="width:100%">
                        <thead>
                            <tr>
                                <th style="width:60%">Question</th>
                                <th style="width:20%">Score</th>
                                <th style="width:20%">Count</th>
                            </tr>
                        </thead>
                        <tbody>
                            <% for ( int i = 10; i < 20; i++ ) {
                                   if ( Data.Rows.Count <= i ) {
                                       break;
                                   }
                                   DataRow dr = Data.Rows[i];
                            %>
                            <tr>
                                <th><%= ReportingTools.CleanData( dr["Question"] ) %>. <%= ReportingTools.CleanData( dr["QuestionText"] ) %></th>
                                <td><%= ReportingTools.FormatPercent( dr["Score"].ToString() ) %></td>
                                <td><%= dr["Count"] %></td>
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
<asp:Content ID="Content4" ContentPlaceHolderID="FooterScripts" runat="server"></asp:Content>
