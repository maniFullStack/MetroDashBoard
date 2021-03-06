﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Theatre.aspx.cs" Inherits="GCC_Web_Portal.Theatre" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Theatre Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/PropertyDashboard/<%= PropertyShortCode.ToString() %>">Property Dashboard</a></li>
        <li class="active">Theatre Dashboard</li>
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
<% } else {
       DataRow propOverall;
       if ( PropertyShortCode == GCCPropertyShortCode.GCC ) {
           propOverall = Data.Rows[0];
       } else {
           propOverall = Data.Rows[1];
       }
       
%>
    <div class="row">
        <div class="col-xs-12 col-md-4">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title">Theatre Score</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-TheatreScore" class="chart" height="230" style="height:230px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-4">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title">Theatre Attendance</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-VisitedTheatreScore" class="chart" height="230" style="height:230px"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-building-o"></i>
                    <h3 class="box-title">Theatre</h3>
                </div>
                <div class="box-body border-radius-none">
                    <table class="table table-bordered table-striped" style="width:100%">
                        <thead>
                            <tr>
                                <th style="width:40%"></th>
                                <th style="width:20%">Top 2 Box %</th>
                                <th style="width:40%">
                                    <span class="label label-danger">B2B</span>
                                    <span class="label label-primary">Mid</span>
                                    <span class="label label-success">T2B</span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>Overall entertainment experience</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "OverallTheatre", "_T2B" ) %></td>
                                <td><div id="OverallTheatre" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>The quality of the show</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Quality", "_T2B" ) %></td>
                                <td><div id="Quality" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>The value of the show</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Value", "_T2B" ) %></td>
                                <td><div id="Value" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Seating choices</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Seating", "_T2B" ) %></td>
                                <td><div id="Seating" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Sound quality</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Sound", "_T2B" ) %></td>
                                <td><div id="Sound" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Overall customer service of Theatre staff</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "OverallStaff", "_T2B" ) %></td>
                                <td><div id="OverallStaff" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
<% } %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterScripts" runat="server">
    <% if ( Data != null ) {
        DataRow propOverall;
        if ( PropertyShortCode == GCCPropertyShortCode.GCC ) {
            propOverall = Data.Rows[0];
        } else {
            propOverall = Data.Rows[1];
        }%>
    <script>
        var opts = {
            seriesColors: ['#dd4b39', '#3c8dbc', '#00a65a'],
            stackSeries: true,
            captureRightClick: true,
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: {
                    barMargin: 15,
                    padding:0,
                    fillToZero:true,
                    highlightMouseDown: true,
                    barDirection: 'horizontal'
                },
                shadow: true,
                pointLabels: { show: true, stackedValue:false }
            },
            axes: {
                yaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    showTicks: false,
                    tickOptions: {
                        show:false,
                        showGridline: false,
                        showMark: false
                    },
                    rendererOptions: {
                        drawBaseline: false
                    }
                },
                xaxis: {
                    showTicks: false,
                    tickOptions: {
                        show: false,
                        showGridline: false,
                        showMark: false
                    },
                    rendererOptions: {
                        drawBaseline: false
                    }
                }
            },
            series: [
                { label: 'B2B' },
                { label: 'MID' },
                { label: 'T2B' }
            ],
            grid: { drawBorder: false, drawGridlines: false, shadow:false, background:'transparent' },
            legend: { show: false },
            title: { show: false }
        };
        
        $.jqplot('OverallTheatre', <%= GetJSData( propOverall, "OverallTheatre" ) %>, opts);
        $.jqplot('Quality', <%= GetJSData( propOverall, "Quality" ) %>, opts);
        $.jqplot('Value', <%= GetJSData( propOverall, "Value" ) %>, opts);
        $.jqplot('Seating', <%= GetJSData( propOverall, "Seating" ) %>, opts);
        $.jqplot('Sound', <%= GetJSData( propOverall, "Sound" ) %>, opts);
        $.jqplot('OverallStaff', <%= GetJSData( propOverall, "OverallStaff" ) %>, opts);
        

        var chartDataBase = {
            labels: [<%= PropertyGraphs.LoadLabels() %>],
            datasets: [
              {
                  label: "<%= PropertyShortCode.ToString() %>",
                  fillColor: "rgba(60,141,188,0.9)",
                  strokeColor: "rgba(60,141,188,0.8)",
                  pointColor: "#3b8bba",
                  pointStrokeColor: "rgba(60,141,188,1)",
                  pointHighlightFill: "#fff",
                  pointHighlightStroke: "rgba(60,141,188,1)",
                  //data: [28, 48, 40, 19, 86, 27]
              }<% if ( PropertyShortCode != GCCPropertyShortCode.GCC ) { %>,
              {
                  label: "GCC",
                  fillColor: "rgba(255, 255, 255, 0)",
                  strokeColor: "rgb(255, 50, 50)",
                  //pointColor: "rgb(210, 214, 222)",
                  //pointStrokeColor: "#c1c7d1",
                  //pointHighlightFill: "#fff",
                  //pointHighlightStroke: "rgb(220,220,220)",
                  //data: [65, 59, 80, 81, 56, 55]
              }
              <% } %>
            ]
        };
        
        var chartOptions = {
            showScale: true,
            scaleShowGridLines: false,
            scaleGridLineColor: "rgba(0,0,0,.05)",
            scaleGridLineWidth: 1,
            scaleShowHorizontalLines: true,
            scaleShowVerticalLines: true,
            scaleOverride:true,
            scaleSteps:10,
            scaleStepWidth:10,
            scaleStartValue:0,
            bezierCurve: true,
            bezierCurveTension: 0.3,
            pointDot: false,
            pointDotRadius: 4,
            pointDotStrokeWidth: 1,
            pointHitDetectionRadius: 20,
            datasetStroke: true,
            datasetStrokeWidth: 3,
            datasetFill: true,
            legendTemplate: "<ul class=\"<" + "%=name.toLowerCase()%" + ">-legend\"><" + "% for (var i=0; i<datasets.length; i++){%" + "><li><span style=\"background-color:<" + "%=datasets[i].lineColor%" + ">\"></span><" + "%=datasets[i].label%" + "></li><" + "%}%" + "></ul>",
            maintainAspectRatio: false,
            responsive: true,
            multiTooltipTemplate: "<" + "%= datasetLabel %" + ">: <" + "%= value %" + ">"
        };
        var canvas, chart;
        <%
        //Grab the last 5 months and place them in a string array
        DateTime startDate = DateTime.Now.AddMonths(-5).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
        var months = Enumerable.Range(0, 6).Select(startDate.AddMonths).Select(m => m.ToString("yyyy-MM")).ToList();

        //Placeholder variable to temporarily store the selected index month for comparison
        var selectedMonth = months[0];
        
        for ( int i = 4; i < 6; i++ ) 
        {
        StringBuilder dataGCC = new StringBuilder();
        StringBuilder dataProp = new StringBuilder();
        int rowStart = 2;

        //Initiate Case Switch and restart at 1 for each graph
        int caseSwitch = 1;
        int monthsLength = 0;
            
        //Start on second row if we're only showing overall scores
        if ( PropertyShortCode == GCCPropertyShortCode.GCC ) 
        {
            rowStart--;
        }
        for ( int j = rowStart; j < Data.Rows.Count; j++ ) 
        {
            DataRow dr = Data.Rows[j];
            double dblVal = dr[i].ToString().StringToDbl( -100000 );
            string val = dblVal == -100000 ? "null" : String.Format( "{0:0.0}", dblVal * 100.0 );

            //Initiate the while loop condition and restart for each row in the Data Table
            var retry = true;

            //Store the month from the Database row in a temp string
            string datemonth = Data.Rows[j]["DateMonth"].ToString();

            //This loop will cover two sets of data: first, the overall GCC scores, and second, the property scores
            if ( dr["PropertyID"].Equals( 1 ) ) 
            {
                dataGCC.AppendFormat( ",{0}", val );
            } 
            else
            {
                //Loop through the Date Array and compare against the Database current row month
                //If the month matches, enter the Database value, if not enter 0.0 and retry with the next Array Index                     
                while (retry)
                {
                    retry = false;
                    switch (caseSwitch)
                    {
                        case 1:
                            selectedMonth = months[0];
                            monthsLength++;
                            break;
                        case 2:
                            selectedMonth = months[1];
                            monthsLength++;
                            break;
                        case 3:
                            selectedMonth = months[2];
                            monthsLength++;
                            break;
                        case 4:
                            selectedMonth = months[3];
                            monthsLength++;
                            break;
                        case 5:
                            selectedMonth = months[4];
                            monthsLength++;
                            break;
                        case 6:
                            selectedMonth = months[5];
                            monthsLength++;
                            break;
                    }
                    if (selectedMonth == datemonth)
                    {
                        dataProp.AppendFormat(",{0}", val);
                        caseSwitch++;
                        retry = false;
                    }
                    else
                    {
                        //If there is no data, remove all 0.0 instances to generate blank graphs
                        if (dataProp.Length >= 30)
                        {
                            dataProp.Remove(0, dataProp.Length);
                            dataGCC.Remove(0, dataGCC.Length);
                            continue;
                        }
                        dataProp.AppendFormat(",{0}", "0.0");
                        caseSwitch++;
                        retry = true;

                    }
                }
            }
        }
        //Add trailing 0.0 if there is no data for the current month
        if (monthsLength != 6)
        {
            dataProp.AppendFormat(",{0}", "0.0");
            dataGCC.AppendFormat(",{0}", "0.0");
        }
        //Remove leading commas
        if ( dataGCC.Length > 0 ) {
            dataGCC.Remove( 0, 1 );
        }
        if ( dataProp.Length > 0 ) {
            dataProp.Remove( 0, 1 );
        }
        if ( PropertyShortCode == GCCPropertyShortCode.GCC ) {
        %>
        chartDataBase.datasets[0].data = [<%= dataGCC.ToString() %>];
        <%} else { %>
        chartDataBase.datasets[0].data = [<%= dataProp.ToString() %>];
        chartDataBase.datasets[1].data = [<%= dataGCC.ToString() %>];
        <% } %>
        canvas = $('#chart-<%= Data.Columns[i].ColumnName %>').get(0).getContext("2d");
        chart = new Chart(canvas);
        chart.Line(chartDataBase, chartOptions);
        <% } %>
    </script>
<% } %>
</asp:Content>
