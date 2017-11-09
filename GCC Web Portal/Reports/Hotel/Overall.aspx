<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Overall.aspx.cs" Inherits="GCC_Web_Portal.Reports.Hotel.Overall" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Hotel Overall & GSEI Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/PropertyDashboard/<%= PropertyShortCode.ToString() %>">Property Dashboard</a></li>
        <li><a href="/Reports/Hotel/">Hotel Dashboard</a></li>
        <li class="active">Overall & GSEI Dashboard</li>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<% if ( TableData == null ) { %>
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
       DataRow propOverall = TableData.Rows[0];
       
%>
    <div class="row">
        <div class="col-xs-12 col-md-3">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Overall Stay</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-OverallStay" class="chart" height="290" style="height:290px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3">
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">GSEI</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-GSEI" class="chart" height="290" style="height:290px">></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-pie-chart"></i>
                    <h3 class="box-title">Amenities Used During Visit</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div class="chart" id="chart-PV" style="height:300px;"></div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-pie-chart"></i>
                    <h3 class="box-title">Reason for Choosing</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div class="chart" id="chart-RC" style="height:300px;"></div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">GSEI</h3>
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
                                <th>Ensuring all of your needs were met</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "EnsuringNeeds", "_T2B" ) %></td>
                                <td><div id="EnsuringNeeds" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Making you feel welcome</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "MakingWelcome", "_T2B" ) %></td>
                                <td><div id="MakingWelcome" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Going above & beyond normal service</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "GoingAbove", "_T2B" ) %></td>
                                <td><div id="GoingAbove" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Speed of service</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "SpeedService", "_T2B" ) %></td>
                                <td><div id="SpeedService" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Encouraging you to visit again</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "EncouragingVisit", "_T2B" ) %></td>
                                <td><div id="EncouragingVisit" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Overall staff availability</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "StaffAvailability", "_T2B" ) %></td>
                                <td><div id="StaffAvailability" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Overall service provided by staff (Index)</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "StaffService", "_T2B" ) %></td>
                                <td><div id="StaffService" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Overall Stay</h3>
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
                                <th>Overall condition of the River Rock Casino Resort</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "OverallCondition", "_T2B" ) %></td>
                                <td><div id="OverallCondition" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Value for price</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "ValueForPrice", "_T2B" ) %></td>
                                <td><div id="ValueForPrice" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Made You Feel</h3>
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
                                <th>Welcome</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FeltWelcome", "_T2B" ) %></td>
                                <td><div id="FeltWelcome" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Comfortable</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FeltComfortable", "_T2B" ) %></td>
                                <td><div id="FeltComfortable" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Important</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FeltImportant", "_T2B" ) %></td>
                                <td><div id="FeltImportant" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Expectations</h3>
                </div>
                <div class="box-body border-radius-none">
                    <table class="table table-bordered table-striped" style="width:100%">
                        <thead>
                            <tr>
                                <th style="width:40%"></th>
                                <th style="width:20%">Top Box %</th>
                                <th style="width:40%">
                                    <span class="label label-danger">BB</span>
                                    <span class="label label-primary">Mid</span>
                                    <span class="label label-success">TB</span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>If you return to this area, how likely is it that you will return to this resort?</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "LikelihoodToReturn", "_T2B" ) %></td>
                                <td><div id="LikelihoodToReturn" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>How likely is it that you will recommend this resort to others?</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "LikelihoodToRecommend", "_T2B" ) %></td>
                                <td><div id="LikelihoodToRecommend" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>During your stay, did the staff provide exceptional service which exceeded your expectations? (% Yes)</th>
                                <td><%= ReportingTools.FormatPercent( propOverall["ExceptionServiceScore"].ToString() ) %></td>
                                <td></td>
                            </tr>
                            <tr>
                                <th>When selecting a hotel, how important are eco-friendly or "green" initiatives?</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "GreenImportance", "_T2B" ) %></td>
                                <td><div id="GreenImportance" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Have you visited River Rock Casino Resort before? (% Yes)</th>
                                <td><%= ReportingTools.FormatPercent( propOverall["VisitedBeforeScore"].ToString() ) %></td>
                                <td></td>
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
    <% if ( TableData != null ) {
        DataRow propOverall = TableData.Rows[0];
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

        $.jqplot('EnsuringNeeds', <%= GetJSData( propOverall, "EnsuringNeeds" ) %>, opts);
        $.jqplot('MakingWelcome', <%= GetJSData( propOverall, "MakingWelcome" ) %>, opts);
        $.jqplot('GoingAbove', <%= GetJSData( propOverall, "GoingAbove" ) %>, opts);
        $.jqplot('SpeedService', <%= GetJSData( propOverall, "SpeedService" ) %>, opts);
        $.jqplot('EncouragingVisit', <%= GetJSData( propOverall, "EncouragingVisit" ) %>, opts);
        $.jqplot('StaffAvailability', <%= GetJSData( propOverall, "StaffAvailability" ) %>, opts);
        $.jqplot('StaffService', <%= GetJSData( propOverall, "StaffService" ) %>, opts);
        $.jqplot('OverallCondition', <%= GetJSData( propOverall, "OverallCondition" ) %>, opts);
        $.jqplot('ValueForPrice', <%= GetJSData( propOverall, "ValueForPrice" ) %>, opts);
        $.jqplot('FeltWelcome', <%= GetJSData( propOverall, "FeltWelcome" ) %>, opts);
        $.jqplot('FeltComfortable', <%= GetJSData( propOverall, "FeltComfortable" ) %>, opts);
        $.jqplot('FeltImportant', <%= GetJSData( propOverall, "FeltImportant" ) %>, opts);
        $.jqplot('LikelihoodToReturn', <%= GetJSData( propOverall, "LikelihoodToReturn" ) %>, opts);
        $.jqplot('LikelihoodToRecommend', <%= GetJSData( propOverall, "LikelihoodToRecommend" ) %>, opts);
        $.jqplot('GreenImportance', <%= GetJSData( propOverall, "GreenImportance" ) %>, opts);
        

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
        for (int i = 3; i < 23; i++) {
            DataColumn dc = TableData.Columns[i];
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

        <%--
        
        var pieData = [
            {
                value:<%= propOverall["ArrivalParking"].ToString() %>,
                color:'#f8931d',
                label: 'Arrival and parking'
            },
            {
                value:<%= propOverall["GuestServices"].ToString() %>,
                color:'#8eb021',
                label: 'Guest Services'
            },
            {
                value:<%= propOverall["Cashiers"].ToString() %>,
                color:'#3572b0',
                label: 'Cashiers'
            },
            {
                value:<%= propOverall["ManagerSupervisor"].ToString() %>,
                color:'#d04437',
                label: 'Manager/Supervisor'
            },
            {
                value:<%= propOverall["Security"].ToString() %>,
                color:'#f6c342',
                label: 'Security'
            },
            {
                value:<%= propOverall["Slots"].ToString() %>,
                color:'#654982',
                label: 'Slots'
            },
            {
                value:<%= propOverall["Tables"].ToString() %>,
                color:'#f691b2',
                label: 'Tables'
            },
            {
                value:<%= propOverall["FoodBeverage"].ToString() %>,
                color:'#0ba3c8',
                label: 'Food & Beverage'
            },
            {
                value:<%= propOverall["Hotel"].ToString() %>,
                color:'#ed2025',
                label: 'Hotel'
            },
            {
                value:<%= propOverall["Other"].ToString() %>,
                color:'#198539',
                label: 'Other'
            }
        ];

        
        var canvas = $('#chartProblemLocations').get(0).getContext("2d");
        var chart = new Chart(canvas).Pie(pieData, chartOptions);
        --%>
        
        <%
        //Grab the last 5 months and place them in a string array
        DateTime startDate = DateTime.Now.AddMonths(-5).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
        var months = Enumerable.Range(0, 6).Select(startDate.AddMonths).Select(m => m.ToString("yyyy-MM")).ToList();

        //Placeholder variable to temporarily store the selected index month for comparison
        var selectedMonth = months[0];
        
        for ( int i = 4; i < 6; i++ ) 
        {
            StringBuilder dataProp = new StringBuilder();
            int rowStart = 1;

            //Initiate Case Switch and restart at 1 for each graph
            int caseSwitch = 1;
            int monthsLength = 0;
            for ( int j = rowStart; j < ChartData.Rows.Count; j++ ) 
            {
                DataRow dr = ChartData.Rows[j];
                double dblVal = dr[i].ToString().StringToDbl( -100000 );
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
        canvas = $('#chart-<%= ChartData.Columns[i].ColumnName %>').get(0).getContext("2d");
        chart = new Chart(canvas);
        chart.Line(chartDataBase, chartOptions);
        <%
    } %>
    </script>
<% } %>
</asp:Content>
