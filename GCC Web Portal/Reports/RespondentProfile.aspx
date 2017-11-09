<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="RespondentProfile.aspx.cs" Inherits="GCC_Web_Portal.RespondentProfile"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,CorporateMarketing,PropertyStaff" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <style>
        .chart {color:#000}
        .jqplot-table-legend { border:none!important}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Respondent Profile</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Respondent Profile</li>
    </ol>
</asp:Content>
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
        <div class="col-xs-12 col-md-6">
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-user"></i>
                    <h3 class="box-title">Respondent Age</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div class="chart" id="chart-AG" style="height:300px;"></div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-6">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-mars"></i>
                    <i class="fa fa-venus"></i>
                    <h3 class="box-title">Respondent Gender</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div class="chart" id="chart-G" style="height:300px;"></div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-md-6">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-retweet"></i>
                    <h3 class="box-title">Visit Frequency</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div class="chart" id="chart-VF" style="height:300px;"></div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-6">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-comment"></i>
                    <h3 class="box-title">Languages</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div class="chart" id="chart-L" style="height:300px;"></div>
                </div>
            </div>
        </div>
    </div>
<% } %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterScripts" runat="server">
    <% if ( Data != null ) { %>
    <script src="//code.highcharts.com/highcharts.js"></script>
    <script src="/Scripts/no-data-to-display.js"></script>
    <script>
        Highcharts.setOptions({
            colors: ["#f8931d", "#8eb021", "#3572b0", "#d04437", "#f6c342", "#654982", "#f691b2", "#0ba3c8", "#ed2025", "#198539", "#68b44c", "#cc3399"]
        });
        var defaultOptions = {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: ''
            },
            tooltip: {
                pointFormat: '<b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: false,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: false
                    },
                    showInLegend: true
                }
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            credits: {
                enabled: false
            },
            series: [{
                colorByPoint: true
            }]
        };

        defaultOptions.series[0].data = [<%
        string lastPrefix = String.Empty;
        bool first = true;
        DataRow dr = Data.Rows[0];
        foreach( DataColumn dc in Data.Columns ) {
            if ( dc.ColumnName.Equals( "TotalRecords" ) ) {
                continue;
            }
            string[] col = dc.ColumnName.Split( '_' );
            bool isNew = !col[0].Equals( lastPrefix );
            if ( isNew && !first ) {
                //Close previous data
                %>];
        $('#chart-<%= lastPrefix %>').highcharts(defaultOptions);
        defaultOptions.series[0].data = [<%
            }
            if ( !isNew ) {
                Response.Write( "," );
            }
            %>{name:'<%= col[1].Replace( "'",@"\'" ) %>',y:<%= dr[dc.ColumnName] %>}<%
            lastPrefix = col[0];
            first = false;
        }
        %>];
        $('#chart-<%= lastPrefix %>').highcharts(defaultOptions);
    </script>
    <% } %>
</asp:Content>
