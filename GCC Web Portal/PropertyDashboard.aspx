<%@ Page Title="Property Dashboard" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="PropertyDashboard.aspx.cs" Inherits="GCC_Web_Portal.PropertyDashboard"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,PropertyStaff,HRStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>

<asp:Content runat="server" ContentPlaceHolderID="MainContentHeader">
    <h1>Property Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Property Dashboard</li>
    </ol>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
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
    <% if ( StatRow != null ) { %>
    <div class="row">
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="small-box bg-green">
                <div class="inner">
                    <h3><%= StatRow["ThisMonth"].ToString() %></h3>
                    <p>Surveys in <%= DateTime.Now.ToString( "MMM yyyy" ) %></p>
                </div>
                <div class="icon">
                    <i class="fa fa-check-circle-o"></i>
                </div>
                <%--<a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>--%>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="small-box bg-red">
                <div class="inner">
                    <h3><%= StatRow["FeedbackOpenCount"].ToString() %></h3>
                    <p>Open Feedback in <%= DateTime.Now.ToString( "MMM yyyy" ) %></p>
                </div>
                <div class="icon">
                    <i class="fa fa-flag"></i>
                </div>
                <%--<a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>--%>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="small-box bg-yellow">
                <div class="inner">
                    <h3><%= StatRow["FeedbackCount"].ToString() %></h3>
                    <p>Total Feedback in <%= DateTime.Now.ToString( "MMM yyyy" ) %></p>
                </div>
                <div class="icon">
                    <i class="fa fa-comments-o"></i>
                </div>
                <%--<a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>--%>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="small-box bg-blue">
                <div class="inner">
                    <h3><%= StatRow["ThisYear"].ToString() %></h3>
                    <p>Surveys in <%= DateTime.Now.ToString( "yyyy" ) %></p>
                </div>
                <div class="icon">
                    <i class="fa fa-file-text-o"></i>
                </div>
                <%--<a href="#" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>--%>
            </div>
        </div>
    </div>
    <% } %>
    <% if ( TopMessage.IsVisible ) { %>
    <div class="row">
        <div class="col-md-6">
            <sc:MessageManager runat="server" ID="TopMessage"></sc:MessageManager>
        </div>
    </div>
    <% } %>
    <div class="row">
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">GEI</h3>
                    <div class="box-tools pull-right">
                        <a href="/GEINPS/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="GEI / NPS Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-GEI" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">GSEI</h3>
                    <div class="box-tools pull-right">
                        <a href="/Staff/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="GSEI Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-StaffScore" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">NPS</h3>
                    <div class="box-tools pull-right">
                        <a href="/GEINPS/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="GEI / NPS Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-NPS" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">PRS</h3>
                    <div class="box-tools pull-right">
                        <a href="/PRS/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="PRS Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-PRS" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Total Feedback</h3>
                    <div class="box-tools pull-right">
                        <a href="/Followup/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="Follow-up Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-FeedbackCount" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">#&nbsp;&lt;&nbsp;24h</h3>
                    <div class="box-tools pull-right">
                        <a href="/Followup/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="Follow-up Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-FeedbackLessThan24Hrs" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">#&nbsp;24-48h</h3>
                    <div class="box-tools pull-right">
                        <a href="/Followup/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="Follow-up Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-Feedback24To48Hrs" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">#&nbsp;&gt;&nbsp;48h</h3>
                    <div class="box-tools pull-right">
                        <a href="/Followup/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="Follow-up Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-Feedback48Hrs" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Average Response Time (Hrs)</h3>
                    <div class="box-tools pull-right">
                        <a href="/Followup/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="Follow-up Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>      
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-AverageFeedbackResponse" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Facilities</h3>
                    <div class="box-tools pull-right">
                        <a href="/Facilities/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="Facilities Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-FacilitiesScore" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Gaming</h3>
                    <div class="box-tools pull-right">
                        <a href="/Gaming/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="Gaming Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-GamingScore" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Food &amp; Beverage</h3>
                    <div class="box-tools pull-right">
                        <a href="/FoodAndBev/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="Food &amp; Beverage Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-FoodAndBeverageScore" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Lounge</h3>
                    <div class="box-tools pull-right">
                        <a href="/Lounge/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="Lounge Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-LoungeScore" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Theatre</h3>
                    <div class="box-tools pull-right">
                        <a href="/Theatre/<%= PropertyShortCode.ToString() %>" class="small-box-footer" title="Theatre Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-TheatreScore" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <% if ( PropertyShortCode == GCCPropertyShortCode.RR ) { %>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Hotel</h3>
                    <div class="box-tools pull-right">
                        <a href="/Reports/Hotel" class="small-box-footer" title="Hotel Dashboard">Details <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-HotelScore" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
        <% } %>
        <div class="col-lg-3 col-sm-6 col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-area-chart"></i>
                    <h3 class="box-title">Overall Sentiment Score</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <canvas id="chart-SentimentScore" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
<% } %>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="FooterScripts">
<% if ( Data != null ) { %>
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
                  pointHighlightStroke: "rgba(60,141,188,1)",
                  //data: [0]
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
            scaleOverride: true,
            scaleSteps: 5,
            scaleStepWidth: 20,
            scaleStartValue: 0,
            bezierCurve: true,
            bezierCurveTension: 0.3,
            pointDot: false,
            pointDotRadius: 4,
            min: 0.0, //Added to make sure that the minumum plot point is at least 0.0
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
        var feedbackCanvas, feedbackChart;

        <%
        //GEI results
        DataTable mnthDT = Data.Tables[DATA_MONTHLY_STATS];        

        //Grab the last 5 months and place them in a string array
        DateTime startDate = DateTime.Now.AddMonths(-5).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
        var months = Enumerable.Range(0, 6).Select(startDate.AddMonths).Select(m => m.ToString("yyyy-MM")).ToList();

        //Placeholder variable to temporarily store the selected index month for comparison
        var selectedMonth = months[0];
        
        if ( mnthDT.Rows.Count > 0 ) {
        for (int i = 4;  i < mnthDT.Columns.Count; i++) {
            StringBuilder dataGCCGEI = new StringBuilder();
            StringBuilder dataPropGEI = new StringBuilder();
            
            //Initiate Case Switch and restart at 1 for each graph
            int caseSwitch = 1;
            int monthsLength = 0;
            for ( int j = 0; j < mnthDT.Rows.Count; j++ ) 
            {
                DataRow dr = mnthDT.Rows[j];
                double dblVal = dr[i].ToString().StringToDbl(-100000);
                string val = dblVal == -100000 ? "0" : String.Format( "{0:0.0}", dblVal );

                //Initiate the while loop condition and restart for each row in the Data Table
                var retry = true;
                
                //Store the month from the Database row in a temp string
                string datemonth = mnthDT.Rows[j]["DateMonth"].ToString();
                
                //This loop will cover two sets of data: first, the overall GCC scores, and second, the property scores
                if (dr["PropertyID"].Equals(1))
                {                     
                    dataGCCGEI.AppendFormat(",{0}", val);
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
                            dataPropGEI.AppendFormat(",{0}", val);
                            caseSwitch++;                           
                            retry = false;
                        }
                        else
                        {
                            //If there is no data, remove all 0.0 instances to generate blank graphs
                            if (dataPropGEI.Length >= 30)
                            {
                                dataPropGEI.Remove(0,dataPropGEI.Length);
                                dataGCCGEI.Remove(0, dataGCCGEI.Length);                             
                                continue;
                            }                            
                            dataPropGEI.AppendFormat(",{0}", "0.0");
                            caseSwitch++;                          
                            retry = true;                            
                        }
                    }                                                             
                }
            }
            //Add trailing 0.0 if there is no data for the current month
            if (monthsLength != 6)
            {
                dataPropGEI.AppendFormat(",{0}", "0.0");
                dataGCCGEI.AppendFormat(",{0}", "0.0");
            }
            //Remove leading commas
            if ( dataGCCGEI.Length > 0 ) {
                dataGCCGEI.Remove( 0, 1 );
            }
            if ( dataPropGEI.Length > 0 ) {
                dataPropGEI.Remove( 0, 1 );
            }
            if ( dataGCCGEI.Length == 0 || ( PropertyShortCode != GCCPropertyShortCode.GCC && dataPropGEI.Length == 0 ) ) {
                continue;
            }
            if ( PropertyShortCode == GCCPropertyShortCode.GCC ) {
        %>
        chartDataBase.datasets[0].data = [<%= dataGCCGEI.ToString() %>];
        <% } else { %>
        chartDataBase.datasets[0].data = [<%= dataPropGEI.ToString() %>];
        chartDataBase.datasets[1].data = [<%= dataGCCGEI.ToString() %>];
        <% } %>
        canvas = $('#chart-<%= mnthDT.Columns[i].ColumnName %>').get(0).getContext("2d");
        chart = new Chart(canvas);
        chart.Line(chartDataBase, chartOptions);
        
        <% if ( i == 12 ) { %>
        chartOptions.scaleOverride = false;
     
        <% } 
        }
        } %>
        <%
        
        //Feedback results
        DataTable fbkDT = Data.Tables[DATA_FEEDBACK];
        
        //Grab the last 6 months and place them in a string array
        startDate = DateTime.Now.AddMonths(-5).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
        months = Enumerable.Range(0, 6).Select(startDate.AddMonths).Select(m => m.ToString("yyyy-MM")).ToList();
        
        if ( fbkDT.Rows.Count > 0 )
        {
        for (int i = 4;  i < fbkDT.Columns.Count; i++)
        {
            StringBuilder dataGCC = new StringBuilder();
            StringBuilder dataProp = new StringBuilder();
            int rowStart = 1;
            int caseSwitch = 1;
            int monthsLength = 0;
            
            for ( int j = rowStart; j < fbkDT.Rows.Count; j++ ) 
            {
                DataRow dr = fbkDT.Rows[j];
                double dblVal = dr[i].ToString().StringToDbl( -100000 );
                string val = dblVal == -100000 ? "null" : String.Format( "{0:0.0}", dblVal );
                
                var retry = true;
                string dateMonth = fbkDT.Rows[j]["DateMonth"].ToString();
                
                //This loop will cover two sets of data: first, the overall GCC scores, and second, the property scores
                if ( dr["PropertyID"].Equals( DBNull.Value ) ) 
                {
                    dataGCC.AppendFormat(",{0}", val);                                                             
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
                        if (selectedMonth == dateMonth)
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
            if (dataGCC.Length > 0)
            {
                dataGCC.Remove(0, 1);
            }
            if ( dataProp.Length > 0 ) 
            {
                dataProp.Remove( 0, 1 );
            }
            if (dataGCC.Length == 0 || (PropertyShortCode != GCCPropertyShortCode.GCC && dataProp.Length == 0))
            {
                continue;
            }
            if ( PropertyShortCode == GCCPropertyShortCode.GCC )
            {
         %>         
            chartDataBase.datasets[0].data = [<%= dataGCC.ToString() %>];
         <% }
            else
            { %>
            chartDataBase.datasets[0].data = [<%= dataProp.ToString() %>];        
        <% } %>
            //Remove the GCC data set from the previous graphs in order to remove the Red Line from the Feedback Graphs
        <%  if (PropertyShortCode != GCCPropertyShortCode.GCC) { %>
            chartDataBase.datasets[1].data = [];
        <% } %>
            feedbackCanvas = $('#chart-<%= fbkDT.Columns[i].ColumnName %>').get(0).getContext("2d");        
            feedbackChart = new Chart(feedbackCanvas);
            feedbackChart.Line(chartDataBase, chartOptions);
     <% }
        } %>
        //Hotel
        chartOptions.scaleOverride = true;
        <%
        //Placeholder variable to temporarily store the selected index month for comparison
        selectedMonth = months[0];
        if ( PropertyShortCode == GCCPropertyShortCode.RR ) 
        {
            DataTable hotelDT = Data.Tables[DATA_HOTEL];
            
            StringBuilder dataGCCHotel = new StringBuilder();
            StringBuilder dataPropHotel = new StringBuilder();
            //Initiate Case Switch and restart at 1 for each graph
            int caseSwitch = 1;
            int monthsLength = 0;
            
            if ( hotelDT.Rows.Count > 0 ) {
            for ( int j = 0; j < hotelDT.Rows.Count; j++ ) 
            {
                DataRow dr = hotelDT.Rows[j];
                double dblVal = dr[4].ToString().StringToDbl();
                string val = String.Format( "{0:0.0}", dblVal );

                //Initiate the while loop condition and restart for each row in the Data Table
                var retry = true;

                //Store the month from the Database row in a temp string
                string datemonth = mnthDT.Rows[j]["DateMonth"].ToString();

                //This loop will cover two sets of data: first, the overall GCC scores, and second, the property scores
                if ( dr["PropertyID"].Equals( 1 ) ) 
                {                   
                    dataGCCHotel.AppendFormat( ",{0}", val );
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
                            dataPropHotel.AppendFormat(",{0}", val);
                            caseSwitch++;
                            retry = false;
                        }
                        else
                        {
                            //If there is no data, remove all 0.0 instances to generate blank graphs
                            if (dataPropHotel.Length >= 30)
                            {
                                dataPropHotel.Remove(0, dataPropHotel.Length);
                                dataGCCHotel.Remove(0, dataGCCHotel.Length);
                                continue;
                            }
                            dataPropHotel.AppendFormat(",{0}", "0.0");
                            caseSwitch++;
                            retry = true;
                        }
                    }
                }
            }
            //Add trailing 0.0 if there is no data for the current month
            if (monthsLength != 6)
            {
                dataPropHotel.AppendFormat(",{0}", "0.0");
                dataGCCHotel.AppendFormat(",{0}", "0.0");
            } 
            //Remove leading commas
            if ( dataGCCHotel.Length > 0 ) {
                dataGCCHotel.Remove( 0, 1 );
            }
            if ( dataPropHotel.Length > 0 ) {
                dataPropHotel.Remove( 0, 1 );
            }
            if ( dataGCCHotel.Length != 0 && dataPropHotel.Length != 0 ) {
            %>
            chartDataBase.datasets[0].data = [<%= dataPropHotel.ToString() %>];
            chartDataBase.datasets[1].data = [<%= dataGCCHotel.ToString() %>];
            canvas = $('#chart-HotelScore').get(0).getContext("2d");
            chart = new Chart(canvas);
            chart.Line(chartDataBase, chartOptions);
        <% }
        }
        } %>
    </script>
<% } %>
</asp:Content>
