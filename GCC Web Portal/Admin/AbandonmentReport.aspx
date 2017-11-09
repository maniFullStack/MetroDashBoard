<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="AbandonmentReport.aspx.cs" Inherits="GCC_Web_Portal.Admin.AbandonmentReport"
    AllowedGroups="ForumAdmin,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>GEI Abandonment</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/Admin/">Admin</a></li>
        <li class="active">GEI Abandonment</li>
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
                <i class="fa fa-sign-out"></i>
                <h3 class="box-title">Details</h3>
            </div>
            <div class="box-body border-radius-none">
                <p>The chart below shows the number of people who abandoned the GEI survey at each section.</p>
                <div id="container" style="min-width: 310px; max-width: 800px; height: 800px;"></div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterScripts" runat="server">
    <script src="//code.highcharts.com/highcharts.js"></script>
    <script src="/Scripts/no-data-to-display.js"></script>
    <%
        JSONBuilder jbCategories = new JSONBuilder(true);
        JSONBuilder jbSeriesData = new JSONBuilder( true );
        int total = 0;
        foreach ( DataRow dr in Data.Rows ) {
            jbCategories.AddString( dr["PageName"].ToString() );
            int val = dr["Count"].ToString().StringToInt( 0 );
            jbSeriesData.AddInt( val );
            total += val;
        }
        
    %>
    <script>
        $('#container').highcharts({
            chart: {
                type: 'bar'
            },
            title: {
                enabled:false,
                text: null
            },
            subtitle: {
                enabled:false
            },
            xAxis: {
                categories: <%= jbCategories.ToString() %>,
                title: {
                    text: 'Section'
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Count'
                },
                labels: {
                    overflow: 'justify'
                },
                minTickInterval: 1
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        enabled: true
                    }
                }
            },
            legend: {
                enabled:false,
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                x: -40,
                y: 80,
                floating: true,
                borderWidth: 1,
                backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                shadow: true
            },
            credits: {
                enabled: false
            },
            series: [
                {
                    name: 'Count',
                    data: <%= total == 0 ? "null" : jbSeriesData.ToString() %>
                }
            ]
        });
    </script>
</asp:Content>
