<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GCC_Web_Portal.Reports.Hotel.Default" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Hotel Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/PropertyDashboard/<%= PropertyShortCode.ToString() %>">Property Dashboard</a></li>
        <li class="active">Hotel Dashboard</li>
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
%>
    <div class="row">
        <div class="col-xs-12 col-md-3">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Overall Stay</h3>
                    <div class="box-tools pull-right">
                        <a href="/Reports/Hotel/Overall" class="small-box-footer" title="Hotel Overall Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-OverallStayScore" class="chart" height="200" style="height:200px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">GSEI</h3>
                    <div class="box-tools pull-right">
                        <a href="/Reports/Hotel/Overall" class="small-box-footer" title="Hotel Overall Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-GSEIScore" class="chart" height="200" style="height:200px">></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3">
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Rooms</h3>
                    <div class="box-tools pull-right">
                        <a href="/Reports/Hotel/Rooms" class="small-box-footer" title="Hotel Rooms Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-RoomsScore" class="chart" height="200" style="height:200px">></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">F&B, Catering</h3>
                    <div class="box-tools pull-right">
                        <a href="/Reports/Hotel/FB" class="small-box-footer" title="Hotel F&B Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-FoodAndBeverageScore" class="chart" height="200" style="height:200px">></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%
       DataRow currentRow = TableData.Rows[0];
       DataRow lastMonthRow = TableData.Rows[1];
       DataRow last12MonthsRow = TableData.Rows[2];
    %>
    <div class="row">
        <div class="col-md-6">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-building-o"></i>
                    <h3 class="box-title">Rooms</h3>
                    <div class="box-tools pull-right">
                        <a href="/Reports/Hotel/Rooms" class="small-box-footer" title="Hotel Rooms Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <table class="table table-bordered table-striped" style="width:100%">
                        <thead>
                            <tr>
                                <th style="width:40%"></th>
                                <th style="width:12%">Filter Period</th>
                                <th style="width:12%">Last Month</th>
                                <th style="width:12%">Change LM</th>
                                <th style="width:12%">Last 12 Months</th>
                                <th style="width:12%">Change L12M</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%= GetDataRow( "Reservation, Front Desk", "Reservation", currentRow, lastMonthRow, last12MonthsRow ) %>
                            <%= GetDataRow( "Housekeeping", "Housekeeping", currentRow, lastMonthRow, last12MonthsRow ) %>
                            <%= GetDataRow( "Hotel Room", "HotelRoom", currentRow, lastMonthRow, last12MonthsRow ) %>
                            <%= GetDataRow( "Fitness Centre", "FitnessClub", currentRow, lastMonthRow, last12MonthsRow ) %>
                            <%= GetDataRow( "Pool / Hot Tub", "PoolHotTub", currentRow, lastMonthRow, last12MonthsRow ) %>
                            <%= GetDataRow( "Valet Parking", "ValetParking", currentRow, lastMonthRow, last12MonthsRow ) %>
                            <%= GetDataRow( "Concierge", "Concierge", currentRow, lastMonthRow, last12MonthsRow ) %>
                            <%= GetDataRow( "Bell / Door Service", "BellDoorService", currentRow, lastMonthRow, last12MonthsRow ) %>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <i class="fa fa-cutlery"></i>
                            <h3 class="box-title">F&amp;B, Catering</h3>
                            <div class="box-tools pull-right">
                                <a href="/Reports/Hotel/FB" class="small-box-footer" title="Hotel F&B Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                            </div>
                        </div>
                        <div class="box-body border-radius-none">
                            <table class="table table-bordered table-striped" style="width:100%">
                                <thead>
                                    <tr>
                                        <th style="width:40%"></th>
                                        <th style="width:12%">Current Period</th>
                                        <th style="width:12%">Last Month</th>
                                        <th style="width:12%">Change LM</th>
                                        <th style="width:12%">Last 12 Months</th>
                                        <th style="width:12%">Change L12M</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%= GetDataRow( "Tramonto", "Tramonto", currentRow, lastMonthRow, last12MonthsRow ) %>
                                    <%= GetDataRow( "Buffet", "Buffet", currentRow, lastMonthRow, last12MonthsRow ) %>
                                    <%= GetDataRow( "Curve", "Curve", currentRow, lastMonthRow, last12MonthsRow ) %>
                                    <%= GetDataRow( "InRoomDining", "InRoomDining", currentRow, lastMonthRow, last12MonthsRow ) %>
                                    <%= GetDataRow( "MeetingAndEvents", "MeetingAndEvents", currentRow, lastMonthRow, last12MonthsRow ) %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-success">
                        <div class="box-header with-border">
                            <i class="fa fa-flag-o"></i>
                            <h3 class="box-title">L2R, Problems</h3>
                            <div class="box-tools pull-right">
                                <a href="/Reports/Hotel/PRS" class="small-box-footer" title="Hotel PRS Dashboard">PRS Details <i class="fa fa-arrow-circle-right"></i></a>
                            </div>
                        </div>
                        <div class="box-body border-radius-none">
                            <table class="table table-bordered table-striped" style="width:100%">
                                <thead>
                                    <tr>
                                        <th style="width:40%"></th>
                                        <th style="width:12%">Current Period</th>
                                        <th style="width:12%">Last Month</th>
                                        <th style="width:12%">Change LM</th>
                                        <th style="width:12%">Last 12 Months</th>
                                        <th style="width:12%">Change L12M</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%= GetDataRow( "Likely to Recommend", "LiklihoodToRecommend", currentRow, lastMonthRow, last12MonthsRow ) %>
                                    <tr>
                                        <%
                                        double currentVal = currentRow["ProblemCount"].ToString().StringToDbl( -1000 );
                                        double lastMonthVal = lastMonthRow["ProblemCount"].ToString().StringToDbl( -1000 );
                                        double last12MonthsVal = last12MonthsRow["ProblemCount"].ToString().StringToDbl( -1000 );
                                        %>
                                        <th><a href="/Reports/Hotel/PRS" title="Hotel PRS Dashboard">Problem?</a></th>
                                        <td><%= currentVal == -1000 ? String.Empty : String.Format( "{0:#,###}", currentVal ) %></td>
                                        <td><%= lastMonthVal == -1000 ? String.Empty : String.Format( "{0:#,###}", lastMonthVal ) %></td>
                                        <td><%= currentVal == -1000 || lastMonthVal == -1000 ? String.Empty : String.Format( "{0:#,###}", currentVal - lastMonthVal ) %></td>
                                        <td><%= last12MonthsVal == -1000 ? String.Empty : String.Format( "{0:#,###}", last12MonthsVal ) %></td>
                                        <td><%= currentVal == -1000 || last12MonthsVal == -1000 ? String.Empty : String.Format( "{0:#,###}", currentVal - last12MonthsVal ) %></td>
                                    </tr>
                                    <%= GetDataRow( "<a href=\"/Reports/Hotel/PRS\" title=\"Hotel PRS Dashboard\">PRS</a>", "PRS", currentRow, lastMonthRow, last12MonthsRow ) %>
                                </tbody>
                            </table>
                        </div>
                    </div>
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
    <script>
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

        var canvas, chart;

        <%
        //Grab the last 5 months and place them in a string array
        DateTime startDate = DateTime.Now.AddMonths(-5).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
        var months = Enumerable.Range(0, 6).Select(startDate.AddMonths).Select(m => m.ToString("yyyy-MM")).ToList();

        //Placeholder variable to temporarily store the selected index month for comparison
        var selectedMonth = months[0];
        
        for ( int i = 4; i < 8; i++ ) 
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
