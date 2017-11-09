<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FB.aspx.cs" Inherits="GCC_Web_Portal.Reports.Hotel.FB" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>F&B, Catering Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/PropertyDashboard/<%= PropertyShortCode.ToString() %>">Property Dashboard</a></li>
        <li><a href="/Reports/Hotel/">Hotel Dashboard</a></li>
        <li class="active">F&B, Catering Dashboard</li>
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
        <div class="col-md-6">
            <div class="row">
                <div class="col-xs-12 col-md-6 col-md-offset-3">
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <i class="fa fa-area-chart"></i>
                            <h3 class="box-title">F&B, Catering</h3>
                        </div>
                        <div class="box-body border-radius-none">
                            <div>
                                <canvas id="chart-FoodAndBeverageScore" class="chart" height="200" style="height:200px">></canvas>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Tramonto</h3>
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
                                <th>Greeting upon arrival</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "GreetingsArrivalTramonto", "_T2B" ) %></td>
                                <td><div id="GreetingsArrivalTramonto" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Timeliness of seating</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "TimelinessSeatingTramonto", "_T2B" ) %></td>
                                <td><div id="TimelinessSeatingTramonto" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Attentiveness of server</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AttentivenessServerTramonto", "_T2B" ) %></td>
                                <td><div id="AttentivenessServerTramonto" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Server's knowledge of menu selections</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "ServerKnowledgeTramonto", "_T2B" ) %></td>
                                <td><div id="ServerKnowledgeTramonto" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Timeliness of meal delivery</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "TimelinessMealTramonto", "_T2B" ) %></td>
                                <td><div id="TimelinessMealTramonto" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Quality and taste of food</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "QualityFoodTramonto", "_T2B" ) %></td>
                                <td><div id="QualityFoodTramonto" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Presentation of food</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "PresentationFoodTramonto", "_T2B" ) %></td>
                                <td><div id="PresentationFoodTramonto" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Quality of beverage</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "QualityBeverageTramonto", "_T2B" ) %></td>
                                <td><div id="QualityBeverageTramonto" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Accuracy of bill</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AccuracyBillTramonto", "_T2B" ) %></td>
                                <td><div id="AccuracyBillTramonto" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Curve</h3>
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
                                <th>Greeting upon arrival</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "GreetingArrivalCurve", "_T2B" ) %></td>
                                <td><div id="GreetingArrivalCurve" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Timeliness of seating</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "TimelinessSeatingCurve", "_T2B" ) %></td>
                                <td><div id="TimelinessSeatingCurve" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Attentiveness of server</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AttentivenessServerCurve", "_T2B" ) %></td>
                                <td><div id="AttentivenessServerCurve" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Server's knowledge of menu selections</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "ServerKnowledgeCurve", "_T2B" ) %></td>
                                <td><div id="ServerKnowledgeCurve" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Timeliness of meal delivery</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "TimelinessMealCurve", "_T2B" ) %></td>
                                <td><div id="TimelinessMealCurve" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Quality and taste of food</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "QualityFoodCurve", "_T2B" ) %></td>
                                <td><div id="QualityFoodCurve" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Presentation of food</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "PresentationFoodCurve", "_T2B" ) %></td>
                                <td><div id="PresentationFoodCurve" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Quality of beverage</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "QualityBeveragesCurve", "_T2B" ) %></td>
                                <td><div id="QualityBeveragesCurve" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Accuracy of bill</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AccuracyBillCurve", "_T2B" ) %></td>
                                <td><div id="AccuracyBillCurve" class="mini-chart"></div></td>
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
                    <h3 class="box-title">Buffet</h3>
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
                                <th>Greeting upon arrival</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "GreetingArrivalTheBuffet", "_T2B" ) %></td>
                                <td><div id="GreetingArrivalTheBuffet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Attentiveness of server</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AttentivenessServerTheBuffet", "_T2B" ) %></td>
                                <td><div id="AttentivenessServerTheBuffet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Server's knowledge of menu selections</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "ServerKnowledgeTheBuffet", "_T2B" ) %></td>
                                <td><div id="ServerKnowledgeTheBuffet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Quality and taste of food</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "QualityFoodTheBuffet", "_T2B" ) %></td>
                                <td><div id="QualityFoodTheBuffet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Quality of beverage</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "QualityBeverageTheBuffet", "_T2B" ) %></td>
                                <td><div id="QualityBeverageTheBuffet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Accuracy of bill</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AccuracyBillTheBuffet", "_T2B" ) %></td>
                                <td><div id="AccuracyBillTheBuffet" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">In-Room Dining</h3>
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
                                <th>Phone answered promptly</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "PhonePromptlyInRoomDining", "_T2B" ) %></td>
                                <td><div id="PhonePromptlyInRoomDining" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Friendliness of order taker</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FriendlinessOrderTakerInRoomDining", "_T2B" ) %></td>
                                <td><div id="FriendlinessOrderTakerInRoomDining" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Friendliness of server</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FriendlinessServerInRoomDining", "_T2B" ) %></td>
                                <td><div id="FriendlinessServerInRoomDining" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Order delivered within time period advised</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "OrderDeliveryTimeInRoomDining", "_T2B" ) %></td>
                                <td><div id="OrderDeliveryTimeInRoomDining" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Accuracy of order</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AccuracyOrderInRoomDining", "_T2B" ) %></td>
                                <td><div id="AccuracyOrderInRoomDining" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Presentation of food</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "PresentationFoodInRoomDining", "_T2B" ) %></td>
                                <td><div id="PresentationFoodInRoomDining" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Quality of in-room dining food</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "QualityFoodInRoomDining", "_T2B" ) %></td>
                                <td><div id="QualityFoodInRoomDining" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Delivery staff offered pick-up of empty tray</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "OfferedTrayPickupInRoomDining", "_T2B" ) %></td>
                                <td><div id="OfferedTrayPickupInRoomDining" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="box box-primary">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Meeting &amp; Events</h3>
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
                                <th>Condition and cleanliness of meeting/event room</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "ConditionMeeting", "_T2B" ) %></td>
                                <td><div id="ConditionMeeting" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Proper meeting/event room temperature</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "ProperTemperatureMeeting", "_T2B" ) %></td>
                                <td><div id="ProperTemperatureMeeting" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Quality of meeting/event food and beverage</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "QualityFoodMeeting", "_T2B" ) %></td>
                                <td><div id="QualityFoodMeeting" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Friendliness and efficiency of meeting/event staff</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FriendlinessStaffMeeting", "_T2B" ) %></td>
                                <td><div id="FriendlinessStaffMeeting" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Quality/condition/support of technical equipment</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "QualityTechMeeting", "_T2B" ) %></td>
                                <td><div id="QualityTechMeeting" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Meeting/event facilities (size, design, amenities)</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FacilitySizeMeeting", "_T2B" ) %></td>
                                <td><div id="FacilitySizeMeeting" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Accuracy of meeting/ event signage</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AccuracySignageMeeting", "_T2B" ) %></td>
                                <td><div id="AccuracySignageMeeting" class="mini-chart"></div></td>
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

        $.jqplot('GreetingsArrivalTramonto', <%= GetJSData( propOverall, "GreetingsArrivalTramonto" ) %>, opts);
        $.jqplot('TimelinessSeatingTramonto', <%= GetJSData( propOverall, "TimelinessSeatingTramonto" ) %>, opts);
        $.jqplot('AttentivenessServerTramonto', <%= GetJSData( propOverall, "AttentivenessServerTramonto" ) %>, opts);
        $.jqplot('ServerKnowledgeTramonto', <%= GetJSData( propOverall, "ServerKnowledgeTramonto" ) %>, opts);
        $.jqplot('TimelinessMealTramonto', <%= GetJSData( propOverall, "TimelinessMealTramonto" ) %>, opts);
        $.jqplot('QualityFoodTramonto', <%= GetJSData( propOverall, "QualityFoodTramonto" ) %>, opts);
        $.jqplot('PresentationFoodTramonto', <%= GetJSData( propOverall, "PresentationFoodTramonto" ) %>, opts);
        $.jqplot('QualityBeverageTramonto', <%= GetJSData( propOverall, "QualityBeverageTramonto" ) %>, opts);
        $.jqplot('AccuracyBillTramonto', <%= GetJSData( propOverall, "AccuracyBillTramonto" ) %>, opts);
        $.jqplot('GreetingArrivalTheBuffet', <%= GetJSData( propOverall, "GreetingArrivalTheBuffet" ) %>, opts);
        $.jqplot('AttentivenessServerTheBuffet', <%= GetJSData( propOverall, "AttentivenessServerTheBuffet" ) %>, opts);
        $.jqplot('ServerKnowledgeTheBuffet', <%= GetJSData( propOverall, "ServerKnowledgeTheBuffet" ) %>, opts);
        $.jqplot('QualityFoodTheBuffet', <%= GetJSData( propOverall, "QualityFoodTheBuffet" ) %>, opts);
        $.jqplot('QualityBeverageTheBuffet', <%= GetJSData( propOverall, "QualityBeverageTheBuffet" ) %>, opts);
        $.jqplot('AccuracyBillTheBuffet', <%= GetJSData( propOverall, "AccuracyBillTheBuffet" ) %>, opts);
        $.jqplot('GreetingArrivalCurve', <%= GetJSData( propOverall, "GreetingArrivalCurve" ) %>, opts);
        $.jqplot('TimelinessSeatingCurve', <%= GetJSData( propOverall, "TimelinessSeatingCurve" ) %>, opts);
        $.jqplot('AttentivenessServerCurve', <%= GetJSData( propOverall, "AttentivenessServerCurve" ) %>, opts);
        $.jqplot('ServerKnowledgeCurve', <%= GetJSData( propOverall, "ServerKnowledgeCurve" ) %>, opts);
        $.jqplot('TimelinessMealCurve', <%= GetJSData( propOverall, "TimelinessMealCurve" ) %>, opts);
        $.jqplot('QualityFoodCurve', <%= GetJSData( propOverall, "QualityFoodCurve" ) %>, opts);
        $.jqplot('PresentationFoodCurve', <%= GetJSData( propOverall, "PresentationFoodCurve" ) %>, opts);
        $.jqplot('QualityBeveragesCurve', <%= GetJSData( propOverall, "QualityBeveragesCurve" ) %>, opts);
        $.jqplot('AccuracyBillCurve', <%= GetJSData( propOverall, "AccuracyBillCurve" ) %>, opts);
        $.jqplot('PhonePromptlyInRoomDining', <%= GetJSData( propOverall, "PhonePromptlyInRoomDining" ) %>, opts);
        $.jqplot('FriendlinessOrderTakerInRoomDining', <%= GetJSData( propOverall, "FriendlinessOrderTakerInRoomDining" ) %>, opts);
        $.jqplot('FriendlinessServerInRoomDining', <%= GetJSData( propOverall, "FriendlinessServerInRoomDining" ) %>, opts);
        $.jqplot('OrderDeliveryTimeInRoomDining', <%= GetJSData( propOverall, "OrderDeliveryTimeInRoomDining" ) %>, opts);
        $.jqplot('AccuracyOrderInRoomDining', <%= GetJSData( propOverall, "AccuracyOrderInRoomDining" ) %>, opts);
        $.jqplot('PresentationFoodInRoomDining', <%= GetJSData( propOverall, "PresentationFoodInRoomDining" ) %>, opts);
        $.jqplot('QualityFoodInRoomDining', <%= GetJSData( propOverall, "QualityFoodInRoomDining" ) %>, opts);
        $.jqplot('OfferedTrayPickupInRoomDining', <%= GetJSData( propOverall, "OfferedTrayPickupInRoomDining" ) %>, opts);
        $.jqplot('ConditionMeeting', <%= GetJSData( propOverall, "ConditionMeeting" ) %>, opts);
        $.jqplot('ProperTemperatureMeeting', <%= GetJSData( propOverall, "ProperTemperatureMeeting" ) %>, opts);
        $.jqplot('QualityFoodMeeting', <%= GetJSData( propOverall, "QualityFoodMeeting" ) %>, opts);
        $.jqplot('FriendlinessStaffMeeting', <%= GetJSData( propOverall, "FriendlinessStaffMeeting" ) %>, opts);
        $.jqplot('QualityTechMeeting', <%= GetJSData( propOverall, "QualityTechMeeting" ) %>, opts);
        $.jqplot('FacilitySizeMeeting', <%= GetJSData( propOverall, "FacilitySizeMeeting" ) %>, opts);
        $.jqplot('AccuracySignageMeeting', <%= GetJSData( propOverall, "AccuracySignageMeeting" ) %>, opts);
        
        var chartDataBase = {
            labels: [<%=PropertyGraphs.LoadLabels() %>],
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
            double dblVal = dr["FoodAndBeverageScore"].ToString().StringToDbl( -100000 );
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
        canvas = $('#chart-FoodAndBeverageScore').get(0).getContext("2d");
        chart = new Chart(canvas);
        chart.Line(chartDataBase, chartOptions);
    </script>
<% } %>
</asp:Content>
