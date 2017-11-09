<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Staff.aspx.cs" Inherits="GCC_Web_Portal.Staff" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>GSEI Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/PropertyDashboard/<%= PropertyShortCode.ToString() %>">Property Dashboard</a></li>
        <li class="active">GSEI Dashboard</li>
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
                    <h3 class="box-title">GSEI</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-StaffScore" class="chart" height="230" style="height:230px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <% if ( Master.DateFilter.BeginDate.Value < new DateTime( 2015, 9, 30 )
             || Master.DateFilter.EndDate.Value < new DateTime( 2015, 9, 30 ) ) { %>
        <div class="col-xs-12 col-md-4">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title">Historical Staff Score</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-StaffScoreHistorical" class="chart" height="230" style="height:230px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <% } %>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-user"></i>
                    <h3 class="box-title">Staff Ratings</h3>
                </div>
                <div class="box-body border-radius-none">
                    <table class="table table-bordered table-striped" style="width:100%">
                        <thead>
                            <tr>
                                <th style="width:40%"></th>
                                <th>Responses Counted</th>
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
                                <th>Cashiers</th>
                                <td><%= Data.Rows[1]["Cashiers"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Cashiers", "_T2B" ) %></td>
                                <td><div id="Cashiers" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Guest Services</th>
                                <td><%= Data.Rows[1]["Guest Services"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "GuestServices", "_T2B" ) %></td>
                                <td><div id="GuestServices" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Slot Attendants</th>
                                <td><%= Data.Rows[1]["Slot Attendants"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "SlotAttendants", "_T2B" ) %></td>
                                <td><div id="SlotAttendants" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Dealers</th>
                                <td><%= Data.Rows[1]["Dealers"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Dealers", "_T2B" ) %></td>
                                <td><div id="Dealers" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Restaurant Servers</th>
                                <td><%= Data.Rows[1]["Restaurant Servers"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "RestaurantServers", "_T2B" ) %></td>
                                <td><div id="RestaurantServers" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Cocktail Servers</th>
                                <td><%= Data.Rows[1]["Cocktail Servers"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "CocktailServers", "_T2B" ) %></td>
                                <td><div id="CocktailServers" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Coffee Servers</th>
                                <td><%= Data.Rows[1]["Coffee Servers"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "CoffeeServers", "_T2B" ) %></td>
                                <td><div id="CoffeeServers" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Security</th>
                                <td><%= Data.Rows[1]["Security"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Security", "_T2B" ) %></td>
                                <td><div id="Security" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Managers/Supervisors</th>
                                <td><%= Data.Rows[1]["Managers/Supervisors"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "ManagersSupervisors", "_T2B" ) %></td>
                                <td><div id="ManagersSupervisors" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Hotel Staff</th>
                                <td><%= Data.Rows[1]["Hotel Staff"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "HotelStaff", "_T2B" ) %></td>
                                <td><div id="HotelStaff" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-list"></i>
                    <h3 class="box-title">Attribute Ratings</h3>
                </div>
                <div class="box-body border-radius-none">
                    <table class="table table-bordered table-striped" style="width:100%">
                        <thead>
                            <tr>
                                <th style="width:40%"></th>
                                <th>Surveys Counted</th>
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
                                <td><%= Data.Rows[1]["Ensuring"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Ensuring", "_T2B" ) %></td>
                                <td><div id="Ensuring" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Making you feel welcome</th>
                                <td><%= Data.Rows[1]["Welcome"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Welcome", "_T2B" ) %></td>
                                <td><div id="Welcome" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Going above & beyond normal service</th>
                                <td><%= Data.Rows[1]["Service"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Service", "_T2B" ) %></td>
                                <td><div id="Service" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Speed of service</th>
                                <td><%= Data.Rows[1]["Speed"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Speed", "_T2B" ) %></td>
                                <td><div id="Speed" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Encouraging you to visit again</th>
                                <td><%= Data.Rows[1]["EncouragingVisit"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "EncouragingVisit", "_T2B" ) %></td>
                                <td><div id="EncouragingVisit" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Overall staff availability</th>
                                <td><%= Data.Rows[1]["Availability"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Availability", "_T2B" ) %></td>
                                <td><div id="Availability" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Encouraging you to take part in events or promotions</th>
                                <td><%= Data.Rows[1]["Encouraging"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Encouraging", "_T2B" ) %></td>
                                <td><div id="Encouraging" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Answering questions you had about the property or promotions</th>
                                <td><%= Data.Rows[1]["Answering"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Answering", "_T2B" ) %></td>
                                <td><div id="Answering" class="mini-chart"></div></td>
                            </tr>
                            <tr>
                                <th>Being friendly and welcoming</th>
                                <td><%= Data.Rows[1]["Friendly"].ToString() %></td>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "Friendly", "_T2B" ) %></td>
                                <td><div id="Friendly" class="mini-chart"></div></td>
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

        $.jqplot('Cashiers', <%= GetJSData( propOverall, "Cashiers" ) %>, opts);
        $.jqplot('GuestServices', <%= GetJSData( propOverall, "GuestServices" ) %>, opts);
        $.jqplot('SlotAttendants', <%= GetJSData( propOverall, "SlotAttendants" ) %>, opts);
        $.jqplot('Dealers', <%= GetJSData( propOverall, "Dealers" ) %>, opts);
        $.jqplot('RestaurantServers', <%= GetJSData( propOverall, "RestaurantServers" ) %>, opts);
        $.jqplot('CocktailServers', <%= GetJSData( propOverall, "CocktailServers" ) %>, opts);
        $.jqplot('CoffeeServers', <%= GetJSData( propOverall, "CoffeeServers" ) %>, opts);
        $.jqplot('Security', <%= GetJSData( propOverall, "Security" ) %>, opts);
        $.jqplot('ManagersSupervisors', <%= GetJSData( propOverall, "ManagersSupervisors" ) %>, opts);
        $.jqplot('HotelStaff', <%= GetJSData( propOverall, "HotelStaff" ) %>, opts);
        
        $.jqplot('Ensuring', <%= GetJSData( propOverall, "Ensuring" ) %>, opts);
        $.jqplot('Welcome', <%= GetJSData( propOverall, "Welcome" ) %>, opts);
        $.jqplot('Service', <%= GetJSData( propOverall, "Service" ) %>, opts);
        $.jqplot('Speed', <%= GetJSData( propOverall, "Speed" ) %>, opts);
        $.jqplot('EncouragingVisit', <%= GetJSData( propOverall, "EncouragingVisit" ) %>, opts);
        $.jqplot('Availability', <%= GetJSData( propOverall, "Availability" ) %>, opts);

        $.jqplot('Encouraging', <%= GetJSData( propOverall, "Encouraging" ) %>, opts);
        $.jqplot('Answering', <%= GetJSData( propOverall, "Answering" ) %>, opts);
        $.jqplot('Friendly', <%= GetJSData( propOverall, "Friendly" ) %>, opts);
        

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
        
        int end = 5;
        if ( Master.DateFilter.BeginDate.Value < new DateTime( 2015, 9, 30 )
             || Master.DateFilter.EndDate.Value < new DateTime( 2015, 9, 30 ) ) {
            end = 6;
        }
        for ( int i = 4; i < end; i++ ) 
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
        for ( int j = rowStart; j < Data.Rows.Count; j++ ) {
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
