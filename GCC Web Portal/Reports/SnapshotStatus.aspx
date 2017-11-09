<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="SnapshotStatus.aspx.cs" Inherits="GCC_Web_Portal.SnapshotStatus"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,PropertyStaff,HRStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <style>
        tr.sub-total {
            font-weight:bold;
            background:#f4f4f4;
            border-top:1px solid #d8d8d8;
            border-bottom:2px solid #d8d8d8;
        }
        table.snapshot > tbody > tr.ss-row > td:first-child {
            padding-left:40px;
            text-align:left;
        }
        table.snapshot > tbody > tr.extra-title > th {
            border:2px solid #d8d8d8;
            text-align:center;
            border-right:none;
        }
        table.snapshot > tbody > tr.extra-title > th:first-child {
            text-align:left;
            border-left:none;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Snapshot Status Report</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Snapshot Status</li>
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
        <div class="col-xs-12">
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-list"></i>
                    <h3 class="box-title">Snapshot Status</h3>
                </div>
                <div class="box-body border-radius-none">
                    <asp:Button Text="Export" runat="server" ID="btnExport" OnClick="btnExport_Click" CssClass="btn btn-primary pull-right" style="margin-bottom:5px;" />
                    <table class="table table-bordered results snapshot">
                        <thead>
						    <tr>
							    <th style="width:40%">Location / Department</th>
							    <th style="width:10%">Salary</th>
                                <th style="width:10%">Salary Completion</th>
							    <th style="width:10%">Hourly</th>
							    <th style="width:10%">Hourly Completion</th>
							    <th style="width:10%">All</th>
							    <th style="width:10%">Overall Completion</th>
						    </tr>
                        </thead>
                        <tbody>
                            <%
                            string lastLocation = String.Empty;
                            foreach ( DataRow dr in Data.Rows ) {
                                bool isNewLoc = !lastLocation.Equals( dr["Location"].ToString() );
                                %>
                            <% if ( isNewLoc && lastLocation.Length > 0 && lastLocation != "Overall" ) { %>
						    <tr class="extra-title">
							    <th>Location / Department</th>
							    <th colspan="2">Salary</th>
							    <th colspan="2">Hourly</th>
							    <th colspan="2">Overall</th>
						    </tr>
                            <% } %>
                            <tr class="<%= isNewLoc ? "sub-total" : "ss-row" %>">
                                <% if ( isNewLoc ) { %>
                                <th><%= ReportingTools.CleanData( dr["Location"] ) %></th>
                                <% } else { %>
                                <td><%= ReportingTools.CleanData( dr["Department"] ) %></td>
                                <% } %>
	                            <td><%= dr["ActualSalaryCount"] %>&nbsp;/&nbsp;<%= dr["TargetSalaryCount"] %></td>
                                <td><%= ReportingTools.FormatPercent( dr["SalaryCompletionRate"].ToString(), 0 ) %></td>
                                <td><%= dr["ActualHourlyCount"] %>&nbsp;/&nbsp;<%= dr["TargetHourlyCount"] %></td>
                                <td><%= ReportingTools.FormatPercent( dr["HourlyCompletionRate"].ToString(), 0 ) %></td>
                                <td><%= dr["RecordCount"] %>&nbsp;/&nbsp;<%= dr["TotalCount"] %></td>
                                <td><%= ReportingTools.FormatPercent( dr["CompletionRate"].ToString(), 0 ) %></td>
                            </tr>
                            <% 
                                lastLocation = dr["Location"].ToString();
                            } %>
                        </tbody>
					</table>
                </div>
            </div>
        </div>
    </div>
<% } %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterScripts" runat="server"></asp:Content>
