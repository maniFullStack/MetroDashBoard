<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Gaming.aspx.cs" Inherits="GCC_Web_Portal.GamingReport" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Gaming Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/PropertyDashboard/<%= PropertyShortCode.ToString() %>">Property Dashboard</a></li>
        <li class="active">Gaming Dashboard</li>
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
            <div class="row" style="margin-top:42px;">
                <div class="col-xs-12">
                  <div class="info-box">
                    <span class="info-box-icon bg-aqua"><i class="fa fa-star-o"></i></span>
                    <div class="info-box-content">
                      <span class="info-box-text">Gaming Score (T2B)</span>
                      <span class="info-box-number"><%= ReportingTools.FormatPercent(propOverall["Gaming"].ToString()) %></span>
                    </div>
                  </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-4">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title">Gaming</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-Gaming" class="chart" height="250" style="height:250px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-4">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-building-o"></i>
                    <h3 class="box-title">Primary Gaming Types</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div class="col-xs-8">
                        <canvas id="chartGamingTypes" height="250" style="height:250px;"></canvas>
                    </div>
                    <div id="legendGamingTypes" class="col-xs-4">
                        <ul class="doughnut-legend clear-fix">
                            <li>
                                <span style="background-color:#f8931d"></span>Slots
                            </li>
                            <li>
                                <span style="background-color:#8eb021"></span>Tables
                            </li>
                            <li>
                                <span style="background-color:#3572b0"></span>Poker
                            </li>
                            <li>
                                <span style="background-color:#d04437"></span>Food&nbsp;&&nbsp;Bev
                            </li>
                            <li>
                                <span style="background-color:#f6c342"></span>Live Ent
                            </li>
                            <li>
                                <span style="background-color:#654982"></span>Hotel
                            </li>
                            <li>
                                <span style="background-color:#f691b2"></span>Live&nbsp;Racing
                            </li>
                            <li>
                                <span style="background-color:#0ba3c8"></span>Racebook
                            </li>
                            <li>
                                <span style="background-color:#ed2025"></span>Bingo
                            </li>
                            <li>
                                <span style="background-color:#198539"></span>Lottery
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Attribute Ratings</h3>
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
                                <th>Variety of games available</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Variety", "_T2B" ) %></td>
                                <td><div id="Variety" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Waiting time to play</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "WaitingTime", "_T2B" ) %></td>
                                <td><div id="WaitingTime" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Availability of specific game at your desired denomination</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Availability", "_T2B" ) %></td>
                                <td><div id="Availability" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Contests & monthly promotions</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Contest", "_T2B" ) %></td>
                                <td><div id="Contest" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Courtesy & respectfulness of staff</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Courtesy", "_T2B" ) %></td>
                                <td><div id="Courtesy" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Game Knowledge of Staff</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Knowledge", "_T2B" ) %></td>
                                <td><div id="Knowledge" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Rate of earning</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Earning", "_T2B" ) %></td>
                                <td><div id="Earning" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Redemption value</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Value", "_T2B" ) %></td>
                                <td><div id="Value" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Choice of rewards</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Rewards", "_T2B" ) %></td>
                                <td><div id="Rewards" class="mini-chart"></div></td>
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

        $.jqplot('Variety', <%= GetJSData( propOverall, "Variety" ) %>, opts);
        $.jqplot('WaitingTime', <%= GetJSData( propOverall, "WaitingTime" ) %>, opts);
        $.jqplot('Availability', <%= GetJSData( propOverall, "Availability" ) %>, opts);
        $.jqplot('Contest', <%= GetJSData( propOverall, "Contest" ) %>, opts);
        $.jqplot('Courtesy', <%= GetJSData( propOverall, "Courtesy" ) %>, opts);
        $.jqplot('Knowledge', <%= GetJSData( propOverall, "Knowledge" ) %>, opts);

        $.jqplot('Earning', <%= GetJSData( propOverall, "Earning" ) %>, opts);
        $.jqplot('Value', <%= GetJSData( propOverall, "Value" ) %>, opts);
        $.jqplot('Rewards', <%= GetJSData( propOverall, "Rewards" ) %>, opts);
        

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

        
        var pieData = [
            {
                value:<%= propOverall["Slots"].ToString() %>,
                color:'#f8931d',
                label: 'Slots'
            },
            {
                value:<%= propOverall["Tables"].ToString() %>,
                color:'#8eb021',
                label: 'Tables'
            },
            {
                value:<%= propOverall["Poker"].ToString() %>,
                color:'#3572b0',
                label: 'Poker'
            },
            {
                value:<%= propOverall["Food"].ToString() %>,
                color:'#d04437',
                label: 'Food & Beverages'
            },
            {
                value:<%= propOverall["Entertainment"].ToString() %>,
                color:'#f6c342',
                label: 'Live Entertainment'
            },
            {
                value:<%= propOverall["Hotel"].ToString() %>,
                color:'#654982',
                label: 'Hotel'
            },
            {
                value:<%= propOverall["LiveRacing"].ToString() %>,
                color:'#f691b2',
                label: 'Live Racing'
            },
            {
                value:<%= propOverall["Racebook"].ToString() %>,
                color:'#0ba3c8',
                label: 'Racebook'
            },
            {
                value:<%= propOverall["Bingo"].ToString() %>,
                color:'#ed2025',
                label: 'Bingo'
            },
            {
                value:<%= propOverall["Lottery"].ToString() %>,
                color:'#198539',
                label: 'Lottery / Pull Tabs'
            }
        ];

        
        var canvas = $('#chartGamingTypes').get(0).getContext("2d");
        var chart = new Chart(canvas).Pie(pieData, chartOptions);


        chartOptions.scaleOverride = true;
        chartOptions.scaleSteps = 10;
        chartOptions.scaleStepWidth = 10;
        chartOptions.scaleStartValue = 0;

        <%
        //Grab the last 5 months and place them in a string array
        DateTime startDate = DateTime.Now.AddMonths(-5).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
        var months = Enumerable.Range(0, 6).Select(startDate.AddMonths).Select(m => m.ToString("yyyy-MM")).ToList();

        //Placeholder variable to temporarily store the selected index month for comparison
        var selectedMonth = months[0];
        
        int i = 4;
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
    </script>
<% } %>
</asp:Content>
