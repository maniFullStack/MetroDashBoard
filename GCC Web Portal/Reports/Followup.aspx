<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Followup.aspx.cs" Inherits="GCC_Web_Portal.FollowupReport" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Follow-up Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/PropertyDashboard/<%= PropertyShortCode.ToString() %>">Property Dashboard</a></li>
        <li class="active">Follow-up Dashboard</li>
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
    <% if ( TopMessage.IsVisible ) { %>
    <div class="row">
        <div class="col-md-6">
            <sc:MessageManager runat="server" ID="TopMessage"></sc:MessageManager>
        </div>
    </div>
    <% } %>
    <div class="row">
        <div class="col-xs-12 col-md-3">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title">Request Count</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-FeedbackCount" class="chart" height="230" style="height:230px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3">
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title"># < 24 Hrs</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-FeedbackLessThan24Hrs" class="chart" height="230" style="height:230px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title"># 24-48 Hrs</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-Feedback24HrsTo48Hrs" class="chart" height="230" style="height:230px"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-3">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title"># > 48 Hrs</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-FeedbackGreaterThan48Hrs" class="chart" height="230" style="height:230px"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-md-6">
            <div class="box box-primary">
                <div class="box-header with-border">
                    <i class="fa fa-reply"></i>
                    <h3 class="box-title">Follow-up</h3>
                </div>
                <div class="box-body border-radius-none">
                    <table class="table table-bordered table-striped" style="width:100%">
                        <thead>
                            <tr>
                                <th style="width:60%"></th>
                                <th style="width:20%">#</th>
                                <th style="width:20%">%</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th># Follow-up requests</th>
                                <td><%= propOverall["FeedbackCount"] %></td>
                                <td></td>
                            </tr>
                            <tr>
                                <th>Average First Response Time</th>
                                <td><%= ReportingTools.GetNiceHours( propOverall["AverageFeedbackResponse"].ToString() ) %></td>
                                <td></td>
                            </tr>
                            <tr>
                                <th>Average Close Time</th>
                                <td><%= ReportingTools.GetNiceHours( propOverall["AverageFeedbackCloseTime"].ToString() ) %></td>
                                <td></td>
                            </tr>
                            <tr>
                                <th><a href="/Admin/Feedback/List?sf=1<%= PropertyShortCode == GCCPropertyShortCode.GCC ? String.Empty : "&fltProperty=" + (int)PropertyShortCode %>&fltFeedbackAge=1" title="Show Feedback Under 24 Hours"># Follow-up under 24 hours</a></th>
                                <td><%= propOverall["FeedbackLessThan24Hrs"] %></td>
                                <td><%= GetPercentage( propOverall["FeedbackLessThan24Hrs"].ToString(), propOverall["FeedbackCount"].ToString() ) %></td>
                            </tr>
                            <tr>
                                <th><a href="/Admin/Feedback/List?sf=1<%= PropertyShortCode == GCCPropertyShortCode.GCC ? String.Empty : "&fltProperty=" + (int)PropertyShortCode %>&fltFeedbackAge=2" title="Show Feedback Under 24 Hours"># Follow-up 24 to 48 hours</a></th>
                                <td><%= propOverall["Feedback24HrsTo48Hrs"] %></td>
                                <td><%= GetPercentage( propOverall["Feedback24HrsTo48Hrs"].ToString(), propOverall["FeedbackCount"].ToString() ) %></td>
                            </tr>
                            <tr>
                                <th><a href="/Admin/Feedback/List?sf=1<%= PropertyShortCode == GCCPropertyShortCode.GCC ? String.Empty : "&fltProperty=" + (int)PropertyShortCode %>&fltFeedbackAge=3" title="Show Feedback Under 24 Hours"># Follow-up over 48 hours</a></th>
                                <td><%= propOverall["FeedbackGreaterThan48Hrs"] %></td>
                                <td><%= GetPercentage( propOverall["FeedbackGreaterThan48Hrs"].ToString(), propOverall["FeedbackCount"].ToString() ) %></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-6">
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-question-circle"></i>
                    <h3 class="box-title">Status</h3>
                </div>
                <div class="box-body border-radius-none">
                    <table class="table table-bordered table-striped" style="width:100%">
                        <thead>
                            <tr>
                                <th style="width:60%"></th>
                                <th style="width:20%">#</th>
                                <th style="width:20%">%</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>Open</th>
                                <td><%= propOverall["FS_Open"] %></td>
                                <td><%= GetPercentage( propOverall["FS_Open"].ToString(), propOverall["FeedbackCount"].ToString() ) %></td>
                            </tr>
                            <tr>
                                <th>Awaiting Guest Response</th>
                                <td><%= propOverall["FS_Awaiting Guest Response"] %></td>
                                <td><%= GetPercentage( propOverall["FS_Awaiting Guest Response"].ToString(), propOverall["FeedbackCount"].ToString() ) %></td>
                            </tr>
                            <tr>
                                <th>Closed - Guest Response Complete</th>
                                <td><%= propOverall["FS_Closed - Guest Response Complete"] %></td>
                                <td><%= GetPercentage( propOverall["FS_Closed - Guest Response Complete"].ToString(), propOverall["FeedbackCount"].ToString() ) %></td>
                            </tr>
                            <tr>
                                <th>Closed - No Further Action Required</th>
                                <td><%= propOverall["FS_Closed - No Further Action Required"] %></td>
                                <td><%= GetPercentage( propOverall["FS_Closed - No Further Action Required"].ToString(), propOverall["FeedbackCount"].ToString() ) %></td>
                            </tr>
                            <tr>
                                <th>Closed - Unable to Satisfy Guest</th>
                                <td><%= propOverall["FS_Closed - Unable to Satisfy Guest"] %></td>
                                <td><%= GetPercentage( propOverall["FS_Closed - Unable to Satisfy Guest"].ToString(), propOverall["FeedbackCount"].ToString() ) %></td>
                            </tr>
                            <tr>
                                <th>Closed - No Response</th>
                                <td><%= propOverall["FS_Closed - No Response"] %></td>
                                <td><%= GetPercentage( propOverall["FS_Closed - No Response"].ToString(), propOverall["FeedbackCount"].ToString() ) %></td>
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
        bool havePropertyRow = false;
        if ( PropertyShortCode == GCCPropertyShortCode.GCC ) {
            propOverall = Data.Rows[0];
        } else {
            propOverall = Data.Rows[1];
            if ( !propOverall["FeedbackCount"].Equals( 0 ) ) {
                havePropertyRow = true;
            }
        }
        %>
    <script>
        var chartDataBase = {
            labels: [<%= PropertyGraphs.LoadLabels() %>],
            datasets: [
              <% if ( havePropertyRow || PropertyShortCode == GCCPropertyShortCode.GCC ) { %>
              {
                  label: "<%= PropertyShortCode.ToString() %>",
                  fillColor: "rgba(60,141,188,0.9)",
                  strokeColor: "rgba(60,141,188,0.8)",
                  pointColor: "#3b8bba",
                  pointStrokeColor: "rgba(60,141,188,1)",
                  pointHighlightFill: "#fff",
                  pointHighlightStroke: "rgba(60,141,188,1)"
              }<% } %>
              <% if ( PropertyShortCode != GCCPropertyShortCode.GCC ) { %>
              <% if ( havePropertyRow ) { %>,<% } %>
              {
                  label: "GCC",
                  fillColor: "rgba(255, 255, 255, 0)",
                  strokeColor: "rgb(255, 50, 50)"
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
            tooltipTemplate: "<" + "%= datasetLabel %" + ">: <" + "%= value %" + ">",
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
                string val = dblVal == -100000 ? "null" : String.Format( "{0:0.0}", dblVal );

                //Initiate the while loop condition and restart for each row in the Data Table
                var retry = true;

                //Store the month from the Database row in a temp string
                string datemonth = Data.Rows[j]["DateMonth"].ToString();

                //This loop will cover two sets of data: first, the overall GCC scores, and second, the property scores
                if ( dr["PropertyID"].Equals( DBNull.Value ) ) 
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
            if ( PropertyShortCode == GCCPropertyShortCode.GCC || !havePropertyRow ) 
            {
            %>
            chartDataBase.datasets[0].data = [<%= dataGCC.ToString() %>];
            <%} else { %>
            chartDataBase.datasets[0].data = [<%= dataProp.ToString() %>];
            
         <% } %>
        canvas = $('#chart-<%= Data.Columns[i].ColumnName %>').get(0).getContext("2d");
        chart = new Chart(canvas);
        chart.Line(chartDataBase, chartOptions);
        <% } %>
    </script>
<% } %>
</asp:Content>
