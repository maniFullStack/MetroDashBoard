<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FoodAndBev.aspx.cs" Inherits="GCC_Web_Portal.FoodAndBev" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Food &amp; Beverage Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/PropertyDashboard/<%= PropertyShortCode.ToString() %>">Property Dashboard</a></li>
        <li class="active">Food &amp; Beverage Dashboard</li>
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
       DataRow propOverall = null;
       if ( Data.Rows.Count > 0 ) {
           propOverall = Data.Rows[0];
       }
       string suffix = PropertyShortCode == GCCPropertyShortCode.GCC ? String.Empty : "_M" + SelectedMention;
%>
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-cutlery"></i>
                    <h3 class="box-title">F&amp;B Location T2B Scores</h3>
                </div>
                <div class="box-body border-radius-none">
                    <% if (DataFull == null
                           || ( DataFull.Tables.Count == 1 && Data.Rows.Count == 0 ) // Site specific
                           || ( DataFull.Tables.Count == 2 && DataFull.Tables[0].Rows.Count == 0 ) // All locations
                          ) { %>
                    No data found.
                    <% } else { %>
                    <div class="row-fluid">
                        <%
                        if ( PropertyShortCode == GCCPropertyShortCode.GCC ) {
                            //Code for all locations
                            foreach ( DataRow dr in DataFull.Tables[0].Rows ) { %>
                        <div class="col-xs-12 col-md-4">
                            <div class="row-fluid clearfix" style="border-bottom:1px solid #c0c0c0;">
                                <div class="col-xs-8"><%= dr["Name"] %></div>
                                <div class="col-xs-2"><%= dr["Count"] %></div>
                                <div class="col-xs-2"><%= ReportingTools.FormatPercent( dr["Score"].ToString() ) %></div>
                            </div>
                        </div>
                            <%
                            }
                        } else {
                            //Code for indivudal sites
                            for ( int i = 1; i <= 13; i++ ) {
                                string name;
                                if ( PropertyTools.HasFoodAndBev( PropertyShortCode, i, out name ) ) { %>
                        <div class="col-xs-12 col-md-4">
                            <div class="row-fluid clearfix" style="border-bottom:1px solid #c0c0c0;">
                                <div class="col-xs-8"><%= name %></div>
                                <div class="col-xs-2"><%= propOverall["Count_M" + i].ToString() %></div>
                                <div class="col-xs-2"><%= ReportingTools.FormatPercent( propOverall["Score_M" + i].ToString() ) %></div>
                            </div>
                        </div>
                            <%  }
                            }
                        } %>
                    </div>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-success">
                        <div class="box-header with-border">
                            <i class="fa fa-building-o"></i>
                            <h3 class="box-title">Select F&amp;B Location</h3>
                        </div>
                        <div class="box-body border-radius-none">
                            <asp:DropDownList runat="server" ID="ddlSelectedLocation" AutoPostBack="true">
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                <asp:ListItem Text="13" Value="13"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-warning">
                        <div class="box-header with-border">
                            <i class="fa fa-check"></i>
                            <h3 class="box-title"><%= ddlSelectedLocation.SelectedItem.Text %> Attribute Ratings</h3>
                        </div>
                        <div class="box-body border-radius-none">
                            <% if ( propOverall == null ) { %>
                            <p>No results found for this location.</p>
                            <% } else { %>
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
                                        <th>Overall dining experience</th>
                                        <td><%= ReportingTools.CalculatePercent( propOverall, "Q19" + suffix, "_T2B" ) %></td>
                                        <td><div id="Q19_Chart" class="mini-chart"></div></td>
                                    </tr>
                                    <tr>
                                        <th>Variety of food choices</th>
                                        <td><%= ReportingTools.CalculatePercent( propOverall, "Q20A" + suffix, "_T2B" ) %></td>
                                        <td><div id="Q20A_Chart" class="mini-chart"></div></td>
                                    </tr>
                                    <tr>
                                        <th>Cleanliness of outlet</th>
                                        <td><%= ReportingTools.CalculatePercent( propOverall, "Q20B" + suffix, "_T2B" ) %></td>
                                        <td><div id="Q20B_Chart" class="mini-chart"></div></td>
                                    </tr>
                                    <tr>
                                        <th>Courtesy of staff</th>
                                        <td><%= ReportingTools.CalculatePercent( propOverall, "Q20C" + suffix, "_T2B" ) %></td>
                                        <td><div id="Q20C_Chart" class="mini-chart"></div></td>
                                    </tr>
                                    <tr>
                                        <th>Timely delivery of order</th>
                                        <td><%= ReportingTools.CalculatePercent( propOverall, "Q20D" + suffix, "_T2B" ) %></td>
                                        <td><div id="Q20D_Chart" class="mini-chart"></div></td>
                                    </tr>
                                    <tr>
                                        <th>Value for the money</th>
                                        <td><%= ReportingTools.CalculatePercent( propOverall, "Q20E" + suffix, "_T2B" ) %></td>
                                        <td><div id="Q20E_Chart" class="mini-chart"></div></td>
                                    </tr>
                                    <tr>
                                        <th>Pleasant atmosphere</th>
                                        <td><%= ReportingTools.CalculatePercent( propOverall, "Q20F" + suffix, "_T2B" ) %></td>
                                        <td><div id="Q20F_Chart" class="mini-chart"></div></td>
                                    </tr>
                                    <tr>
                                        <th>Quality of food</th>
                                        <td><%= ReportingTools.CalculatePercent( propOverall, "Q20G" + suffix, "_T2B" ) %></td>
                                        <td><div id="Q20G_Chart" class="mini-chart"></div></td>
                                    </tr>
                                </tbody>
                            </table>
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <% if ( propOverall != null ) { %>
        <div class="col-md-4">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title"><%= ddlSelectedLocation.SelectedItem.Text %> F&amp;B Score</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-FoodAndBevScore" class="chart" height="230" style="height:230px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <% } %>
    </div>
<% } %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterScripts" runat="server">
    <% if ( Data != null && Data.Rows.Count > 0 ) {
        DataRow propOverall = Data.Rows[0];
        string suffix = PropertyShortCode == GCCPropertyShortCode.GCC ? String.Empty : "_M" + SelectedMention;
           %>
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
        
        $.jqplot('Q19_Chart', <%= GetJSData( propOverall, "Q19" + suffix ) %>, opts);
        $.jqplot('Q20A_Chart', <%= GetJSData( propOverall, "Q20A" + suffix ) %>, opts);
        $.jqplot('Q20B_Chart', <%= GetJSData( propOverall, "Q20B" + suffix ) %>, opts);
        $.jqplot('Q20C_Chart', <%= GetJSData( propOverall, "Q20C" + suffix ) %>, opts);
        $.jqplot('Q20D_Chart', <%= GetJSData( propOverall, "Q20D" + suffix ) %>, opts);
        $.jqplot('Q20E_Chart', <%= GetJSData( propOverall, "Q20E" + suffix ) %>, opts);
        $.jqplot('Q20F_Chart', <%= GetJSData( propOverall, "Q20F" + suffix ) %>, opts);
        $.jqplot('Q20G_Chart', <%= GetJSData( propOverall, "Q20G" + suffix ) %>, opts);
        

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

        //Initiate Case Switch and restart at 1 for each graph
        int caseSwitch = 1;
        int monthsLength = 0;
        
        StringBuilder dataProp = new StringBuilder();
        for ( int j = 1; j < Data.Rows.Count; j++ ) 
        {
            DataRow dr = Data.Rows[j];
            double dblVal = dr["Score" + suffix].ToString().StringToDbl() * 100.0;
            string val = String.Format( "{0:0.0}", dblVal );
            
            //Initiate the while loop condition and restart for each row in the Data Table
            var retry = true;

            //Store the month from the Database row in a temp string
            string datemonth = Data.Rows[j]["DateMonth"].ToString();

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
        //Add trailing 0.0 if there is no data for the current month
        if (monthsLength != 6)
        {
            dataProp.AppendFormat(",{0}", "0.0");
        }
        //Remove leading commas
        if ( dataProp.Length > 0 ) {
            dataProp.Remove( 0, 1 );
        }
        %>
        chartDataBase.datasets[0].data = [<%= dataProp.ToString() %>];
        canvas = $('#chart-FoodAndBevScore').get(0).getContext("2d");
        chart = new Chart(canvas);
        chart.Line(chartDataBase, chartOptions);
    </script>
<% } %>
</asp:Content>
