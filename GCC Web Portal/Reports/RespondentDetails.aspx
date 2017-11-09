<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="RespondentDetails.aspx.cs" Inherits="GCC_Web_Portal.RespondentDetails"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,CorporateMarketing,PropertyStaff" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <style>
        .chart {color:#000}
        .jqplot-table-legend { border:none!important}
        #gender-icon {
            font-size:24px;
            text-align:center;
            line-height:1;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
<%
    bool isSearchPage = ( Data == null || Data.Tables.Count != 4 );
%>
    <h1>Respondent Details<%= !isSearchPage ? " - " + ReportingTools.CleanData( RespondentEncoreNumber ) + ReportingTools.CleanData( RespondentEmail ) : String.Empty %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <% if ( isSearchPage ) { %>
        <li class="active">Respondent Search</li>
        <% } else { %>
        <li><a href="/GuestDetails">Respondent Search</a></li>
        <li class="active">Respondent Details</li>
        <% } %>
    </ol>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<%
    bool isSearchPage = ( Data == null || Data.Tables.Count != 4 );
%>
<% if ( TopMessage.IsVisible ) { %>
<div class="row">
    <div class="col-md-6">
        <sc:MessageManager runat="server" ID="TopMessage"></sc:MessageManager>
    </div>
</div>
<% } %>
<% if ( isSearchPage ) { %>
<div class="row">
    <div class="col-md-6 col-md-offset-3">
        <div class="box box-info">
            <div class="box-header with-border">
                <h3 class="box-title">Respondent Search</h3>
            </div>
            <div class="box-body">
                <p>Enter an encore number or email address into the box below to search for a respondent.</p>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="form-group">
                            <label>Encore # / Email</label><br />
                            <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-center">
                        <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<% } else {
       DataRow primaryRow = null;
       string encoreID = null;
       if ( Data.Tables[DATA_PRIMARY].Rows.Count > 0 ) {
           primaryRow = Data.Tables[DATA_PRIMARY].Rows[0];
       }

       DataTable alternates = Data.Tables[DATA_ALTERNATES];
       if ( alternates.Rows.Count > 0 && primaryRow == null ) {
           primaryRow = alternates.Rows[0];
       }

       if ( primaryRow != null ) {
           encoreID = primaryRow["EncoreNumber"].ToString();
       }
%>
    <div class="row">
        <div class="col-xs-12 col-md-6">
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-user"></i>
                    <h3 class="box-title">Respondent Profile</h3>
                </div>
                <div class="box-body border-radius-none">
                    <% if ( primaryRow == null ) { %>
                    No respondent details found.
                    <% } else { %>
                    <div class="">
                        <div class="clearfix">
                            <div id="gender-icon" style="width:30%;float:left;">
                                <i class="fa fa-<%= primaryRow["Gender"].ToString().ToLower() %>"></i>
                            </div>
                            <div style="width:70%;float:left;">
                                <table class="table table-striped table-bordered">
                                    <tr>
                                        <th>Encore #:</th>
                                        <td><%= ReportingTools.CleanData( primaryRow["EncoreNumber"] ) %></td>
                                    </tr>
                                    <tr>
                                        <th>Email:</th>
                                        <td><%= ReportingTools.CleanData( primaryRow["Email"] ) %></td>
                                    </tr>
                                    <tr>
                                        <th>Tenure:</th>
                                        <td><%= ReportingTools.CleanData( primaryRow["Tenure"] ) %></td>
                                    </tr>
                                    <tr>
                                        <th>Tier:</th>
                                        <td><%= ReportingTools.CleanData( primaryRow["CustomerTier"] ) %></td>
                                    </tr>
                                    <tr>
                                        <th>Value Tier:</th>
                                        <td><%= ReportingTools.CleanData( primaryRow["ValueTier"] ) %></td>
                                    </tr>
                                    <tr>
                                        <th>Visit Frequency:</th>
                                        <td><%= ReportingTools.CleanData( primaryRow["VisitFrequency"] ) %></td>
                                    </tr>
                                    <tr>
                                        <th>Market Share:</th>
                                        <td><%= ReportingTools.CleanData( primaryRow["MarketShare"] ) %></td>
                                    </tr>
                                    <tr>
                                        <th>Allowed to Contact:</th>
                                        <td><%= ReportingTools.CleanData( primaryRow["AllowedToContact"] ) %></td>
                                    </tr>
                                    <tr>
                                        <th>Segments:</th>
                                        <td>
                                            <%= primaryRow["Loyalty"].Equals(1) ? "<span class='label label-default'>Loyalty</span>" : String.Empty %>
                                            <%= primaryRow["Prive"].Equals(1) ? "<span class='label label-default'>Prive</span>" : String.Empty %>
                                            <%= primaryRow["Winback"].Equals(1) ? "<span class='label label-default'>Winback</span>" : String.Empty %>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <% } %>
                </div>
            </div>
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-edit"></i>
                    <h3 class="box-title">Surveys</h3>
                </div>
                <div class="box-body border-radius-none">
                    <% if ( Data.Tables[DATA_SURVEYS].Rows.Count == 0 ) { %>
                    No results found.
                    <% } else { %>
                    
                    <div class="table-responsive">
                    <table class="table table-bordered table-striped">
                        <thead>
                            <tr>
                                <th>Date Created</th>
                                <th>Property</th>
                                <th>Survey</th>
                                <th>Encore ID</th>
                                <th>Feedback</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach ( DataRow dr in Data.Tables[DATA_SURVEYS].Rows ) {
                                    SurveyType st = (SurveyType)dr["SurveyTypeID"].ToString().StringToInt(0);
                            %>
                            <tr>
                                <td><%= ReportingTools.AdjustAndDisplayDate( dr["DateCreated"].ToString(), "MMMM dd, yyyy  @  hh:mm tt", User ) %></td>
                                <td><%= ReportingTools.CleanData( dr["PropertyName"] ) %></td>
                                <td><%= st %></td>
                                <td><%= ReportingTools.CleanData( dr["EncoreNumber"] ) %></td>
                                <td>
                                    <%if ( !dr["FeedbackStatusID"].Equals( DBNull.Value ) ) { %>
                                    <% int statusID = (int)dr["FeedbackStatusID"];
                                    if ( statusID == 1 ) { %>
                                    <span class="label label-danger">
                                    <% } else if ( statusID == 2 ) { %>
                                    <span class="label label-primary">
                                    <% } else if ( statusID == 3 || statusID == 4 ) { %>
                                    <span class="label label-success">
                                    <% } else if ( statusID >= 5 ) { %>
                                    <span class="label label-warning">
                                    <% } else { %>
                                    <span>
                                    <% } %>
                                    <%= ReportingTools.CleanData( dr["StatusName"] ) %>
                                    </span>
                                    <br />
                                    <a href="/Admin/Feedback/<%= dr["FeedbackUID"] %>" target="_blank">View Feedback <i class="fa fa-external-link"></i></a>
                                    <% } else if ( dr["IsHistorical"].Equals( true ) && dr["RequestedFeedback"].Equals( 1 ) ) { %>
                                    <span class="label label-default">Followup Requested - Inactive</span>
                                    <% } %>
                                </td>
                                <td>
                                    <a href="/Display/<%= st.ToString() %>/<%= dr["RecordID"] %>" class="btn btn-primary" target="_blank">View Responses <i class="fa fa-external-link"></i></a>
                                </td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                    </div>
                    <% } %>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-6">
            <% if ( alternates.Rows.Count > 1
                   || ( alternates.Rows.Count == 1
                        && !alternates.Rows[0]["EncoreNumber"].Equals( encoreID ) )
                   ) { %>
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-users"></i>
                    <h3 class="box-title">Alternate Accounts</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div class="table-responsive">
                        <table class="table table-striped table-bordered">
						    <tr>
							    <th>Encore #</th>
							    <th>Email</th>
							    <th>Gender</th>
							    <th>Tenure</th>
							    <th>Tier</th>
							    <th>Value Tier</th>
							    <th>Visit Frequency</th>
							    <th>Market Share</th>
							    <th>Allowed to Contact</th>
							    <th>Segments</th>
						    </tr>
                            <%
                            foreach ( DataRow ar in alternates.Rows ) {
                                if ( ar["EncoreNumber"].Equals( encoreID ) ) {
                                    continue;
                                }
                            %>
						    <tr>
							    <td><%= ReportingTools.CleanData( ar["EncoreNumber"] ) %></td>
							    <td><%= ReportingTools.CleanData( ar["Email"] ) %></td>
							    <td><%= ReportingTools.CleanData( ar["Gender"] ) %></td>
							    <td><%= ReportingTools.CleanData( ar["Tenure"] ) %></td>
							    <td><%= ReportingTools.CleanData( ar["CustomerTier"] ) %></td>
							    <td><%= ReportingTools.CleanData( ar["ValueTier"] ) %></td>
							    <td><%= ReportingTools.CleanData( ar["VisitFrequency"] ) %></td>
							    <td><%= ReportingTools.CleanData( ar["MarketShare"] ) %></td>
							    <td><%= ReportingTools.CleanData( ar["AllowedToContact"] ) %></td>
							    <td>
								    <%= ar["Loyalty"].Equals(1) ? "<span class='label label-default'>Loyalty</span>" : String.Empty %>
								    <%= ar["Prive"].Equals(1) ? "<span class='label label-default'>Prive</span>" : String.Empty %>
								    <%= ar["Winback"].Equals(1) ? "<span class='label label-default'>Winback</span>" : String.Empty %>
							    </td>
						    </tr>
                            <% } %>
					    </table>
                    </div>
                </div>
            </div>
            <% } %>
        
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-line-chart"></i>
                    <h3 class="box-title">Response Summary</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div>
                        <h3>GEI</h3>
                        <canvas id="chart-GEI" class="chart" style="height: 250px;"></canvas>
                    </div>
                    <div>
                        <h3>NPS</h3>
                        <canvas id="chart-NPS" class="chart" style="height: 250px;"></canvas>
                    </div>
                    <div>
                        <h3>PRS</h3>
                        <canvas id="chart-PRS" class="chart" style="height: 250px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>
<% } %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterScripts" runat="server">
<script type="text/javascript">
//FitText
    !function(t){t.fn.fitText=function(n,i){var e=n||1,o=t.extend({minFontSize:Number.NEGATIVE_INFINITY,maxFontSize:Number.POSITIVE_INFINITY},i);return this.each(function(){var n=t(this),i=function(){n.css("font-size",Math.max(Math.min(n.width()/(10*e),parseFloat(o.maxFontSize)),parseFloat(o.minFontSize)))};i(),t(window).on("resize.fittext orientationchange.fittext",i)})}}(jQuery);
    $('#gender-icon').fitText(0.075);
</script>
    <% if ( Data != null && Data.Tables.Count == 4 ) { %>
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
                  //data: [28, 48, 40, 19, 86, 27]
              }
            ]
        };

        var chartOptions = {
            showScale: true,
            scaleShowGridLines: true,
            scaleGridLineColor: "rgba(0,0,0,.05)",
            scaleGridLineWidth: 1,
            scaleShowHorizontalLines: true,
            scaleShowVerticalLines: false,
            scaleOverride: true,
            scaleSteps: 5,
            scaleStepWidth: 20,
            scaleStartValue: 0,
            bezierCurve: true,
            bezierCurveTension: 0.3,
            pointDot: true,
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
        //Feedback results
        DateTime startDate = DateTime.Now.AddMonths(-5).AddDays(-DateTime.Now.AddMonths(-1).Day + 1).Date;
        var months = Enumerable.Range(0, 6).Select(startDate.AddMonths).Select(m => m.ToString("yyyy-MM")).ToList();

        //Placeholder variable to temporarily store the selected index month for comparison
        var selectedMonth = months[0];        
        
        DataTable scrDT = Data.Tables[3];
        for ( int i = 2; i < scrDT.Columns.Count; i++ ) 
        {
            StringBuilder dataProp = new StringBuilder();
            
            //Initiate Case Switch and restart at 1 for each graph
            int caseSwitch = 1;
            int monthsLength = 0;
            
            foreach ( DataRow dr in scrDT.Rows ) 
            {
                double dblVal = dr[i].ToString().StringToDbl( -100000 );
                string val = dblVal == -100000 ? "null" : String.Format( "{0:0.0}", dblVal );               

                //Initiate the while loop condition and restart for each row in the Data Table
                var retry = true;

                //Store the month from the Database row in a temp string
                string datemonth = dr["DateMonth"].ToString();

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
            //Remove leading comma            
            if ( dataProp.Length > 0 ) {
                dataProp.Remove( 0, 1 );
            }
            %>
        <% if ( scrDT.Columns[i].ColumnName.Equals( "NPS" ) ) { %>
        chartOptions.scaleSteps = 10;
        chartOptions.scaleStartValue = -100;
        chartOptions.datasetFill = false;
        <% } else { %>
        chartOptions.scaleSteps = 5;
        chartOptions.scaleStartValue = 0;
        chartOptions.datasetFill = true;
        <% } %>
        chartDataBase.datasets[0].data = [<%= dataProp.ToString() %>];
        canvas = $('#chart-<%= scrDT.Columns[i].ColumnName %>').get(0).getContext("2d");
        chart = new Chart(canvas);
        chart.Line(chartDataBase, chartOptions);
        <% } %>
    </script>
    <% } %>
</asp:Content>
