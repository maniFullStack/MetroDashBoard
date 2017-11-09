<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="PRS.aspx.cs" Inherits="GCC_Web_Portal.Reports.Hotel.PRS" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Hotel PRS Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/PropertyDashboard/<%= PropertyShortCode.ToString() %>">Property Dashboard</a></li>
        <li><a href="/Reports/Hotel/">Hotel Dashboard</a></li>
        <li class="active">PRS Dashboard</li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<% if ( ChartData == null ) { %>
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
       DataRow propOverall = ChartData.Rows[0];
%>
    <div class="row">
        <div class="col-xs-12 col-md-4">
            <div class="row">
                <div class="col-xs-12">
                  <div class="info-box">
                    <span class="info-box-icon bg-aqua"><i class="fa fa-star-o"></i></span>
                    <div class="info-box-content">
                      <span class="info-box-text">PRS Score (T2B)</span>
                      <span class="info-box-number"><%= ReportingTools.FormatPercent(propOverall["PRS"].ToString()) %></span>
                    </div>
                  </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="info-box bg-red">
                        <span class="info-box-icon"><i class="fa fa-exclamation-triangle"></i></span>
                        <div class="info-box-content">
                            <%double pct = ( propOverall["ExperiencedProblemYesCount"].ToString().StringToDbl() / ( propOverall["ExperiencedProblemYesCount"].ToString().StringToDbl() + propOverall["ExperiencedProblemNoCount"].ToString().StringToDbl() ) ); %>
                            <span class="info-box-text">Experienced Problem?</span>
                            <span class="info-box-number"><%= propOverall["ExperiencedProblemYesCount"].ToString() %> / <%= propOverall["ExperiencedProblemYesCount"].ToString().StringToInt() + propOverall["ExperiencedProblemNoCount"].ToString().StringToInt() %></span>
                            <div class="progress">
                                <div class="progress-bar" style="width: <%= String.Format("{0}", pct * 100.0) %>%"></div>
                            </div>
                            <span class="progress-description"><%= ReportingTools.FormatPercent( pct.ToString() ) %></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                  <div class="info-box bg-yellow">
                    <span class="info-box-icon"><i class="fa fa-flag"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">Reported problem?</span>
                            <span class="info-box-number"><%= propOverall["ReportedProblemYesCount"].ToString() %> / <%= propOverall["ReportedProblemYesCount"].ToString().StringToInt() + propOverall["ReportedProblemNoCount"].ToString().StringToInt() %></span>
                        <% pct = ( propOverall["ReportedProblemYesCount"].ToString().StringToDbl() / ( propOverall["ReportedProblemYesCount"].ToString().StringToDbl() + propOverall["ReportedProblemNoCount"].ToString().StringToDbl() ) );
                         if ( pct != -1000 ) {
                        %>
                        <div class="progress">
                            <div class="progress-bar" style="width: <%= String.Format("{0}", pct * 100.0) %>%"></div>
                        </div>
                        <span class="progress-description"><%= ReportingTools.FormatPercent( pct.ToString() ) %></span>
                        <% } %>
                      
                    </div>
                  </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-4">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title">PRS</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-PRS" class="chart" height="230" style="height:230px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-4">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-building-o"></i>
                    <h3 class="box-title">Problem Locations</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div class="chart" id="chart-PR" style="height:240px;"></div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Overall Ability to Fix Problem</h3>
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
                                <th>The length of time taken to resolve your problem</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "TimeTaken", "_T2B" ) %></td>
                                <td><div id="TimeTaken" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>The effort of employees in resolving your problem</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "EmployeeEffort", "_T2B" ) %></td>
                                <td><div id="EmployeeEffort" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>The courteousness of employees while resolving your problem</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Courteousness", "_T2B" ) %></td>
                                <td><div id="Courteousness" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>The amount of communication with you from employees while resolving your problem</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Communication", "_T2B" ) %></td>
                                <td><div id="Communication" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>The fairness of the outcome in resolving your problem</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Fairness", "_T2B" ) %></td>
                                <td><div id="Fairness" class="mini-chart"></div></td>
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
    <% if ( ChartData != null ) {
        DataRow propOverall = ChartData.Rows[0];
    %>
    <script src="//code.highcharts.com/highcharts.js"></script>
    <script src="/Scripts/no-data-to-display.js"></script>
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

        $.jqplot('TimeTaken', <%= GetJSData( propOverall, "TimeTaken" ) %>, opts);
        $.jqplot('EmployeeEffort', <%= GetJSData( propOverall, "EmployeeEffort" ) %>, opts);
        $.jqplot('Courteousness', <%= GetJSData( propOverall, "Courteousness" ) %>, opts);
        $.jqplot('Communication', <%= GetJSData( propOverall, "Communication" ) %>, opts);
        $.jqplot('Fairness', <%= GetJSData( propOverall, "Fairness" ) %>, opts);

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
        for (int i = 24; i < 32; i++) {
            DataColumn dc = ChartData.Columns[i];
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
            %>{name:'<%= col[1].Replace( "'",@"\'" ) %>',y:<%= propOverall[dc.ColumnName] %>}<%
            lastPrefix = col[0];
            first = false;
        }
        %>];
        $('#chart-<%= lastPrefix %>').highcharts(defaultOptions);

        
        
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
                  pointHighlightStroke: "rgba(60,141,188,1)"
              }
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
        <%
        //Grab the last 5 months and place them in a string array
        DateTime startDate = DateTime.Now.AddMonths(-5).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
        var months = Enumerable.Range(0, 6).Select(startDate.AddMonths).Select(m => m.ToString("yyyy-MM")).ToList();

        //Placeholder variable to temporarily store the selected index month for comparison
        var selectedMonth = months[0];
        
        StringBuilder dataProp = new StringBuilder();
        int rowStart = 1;

        //Initiate Case Switch and restart at 1 for each graph
        int caseSwitch = 1;
        int monthsLength = 0;
        for ( int j = rowStart; j < ChartData.Rows.Count; j++ ) 
        {
            DataRow dr = ChartData.Rows[j];
            double dblVal = dr["PRS"].ToString().StringToDbl( -100000 );
            string val = dblVal == -100000 ? "null" : String.Format( "{0:0.0}", dblVal * 100.0 );

            //Initiate the while loop condition and restart for each row in the Data Table
            var retry = true;

            //Store the month from the Database row in a temp string
            string datemonth = ChartData.Rows[j]["DateMonth"].ToString();

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
                        continue;
                    }
                    dataProp.AppendFormat(",{0}", "0.0");
                    caseSwitch++;
                    retry = true;
                }
            } 
        }
        if (monthsLength != 6)
        {
            dataProp.AppendFormat(",{0}", "0.0");
        } 
        if ( dataProp.Length > 0 ) 
        {
            dataProp.Remove( 0, 1 );
        }
        %>
        chartDataBase.datasets[0].data = [<%= dataProp.ToString() %>];
        canvas = $('#chart-PRS').get(0).getContext("2d");
        chart = new Chart(canvas);
        chart.Line(chartDataBase, chartOptions);
    </script>
<% } %>
</asp:Content>
