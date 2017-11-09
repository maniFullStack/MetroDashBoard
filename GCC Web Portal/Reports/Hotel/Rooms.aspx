<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="Rooms.aspx.cs" Inherits="GCC_Web_Portal.Reports.Hotel.Rooms" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Rooms Dashboard: <%= PropertyTools.GetCasinoName( (int)PropertyShortCode ) %></h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/PropertyDashboard/<%= PropertyShortCode.ToString() %>">Property Dashboard</a></li>
        <li><a href="/Reports/Hotel/">Hotel Dashboard</a></li>
        <li class="active">Rooms Dashboard</li>
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
                    <div class="box box-success">
                        <div class="box-header with-border">
                            <i class="fa fa-area-chart"></i>
                            <h3 class="box-title">Rooms</h3>
                        </div>
                        <div class="box-body border-radius-none">
                            <div>
                                <canvas id="chart-RoomsScore" class="chart" height="200" style="height:200px">></canvas>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="box box-warning">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Reservation, Front Desk &ndash; <%= ReportingTools.FormatPercent( propOverall["ReservationScore"].ToString() ) %></h3>
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
                                <th>Friendliness of Reservation Agent</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FriendlinessReservationAgent", "_T2B" ) %></td>
                                <td><div id="FriendlinessReservationAgent" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Helpfulness of Reservation Agent</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "HelpfulnessReservationAgent", "_T2B" ) %></td>
                                <td><div id="HelpfulnessReservationAgent" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Accuracy of reservation information upon check-in</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AccuracyCheckIn", "_T2B" ) %></td>
                                <td><div id="AccuracyCheckIn" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Employee knowledge of the River Rock Casino Resort & Facilities</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "EmployeeKnowledge", "_T2B" ) %></td>
                                <td><div id="EmployeeKnowledge" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Efficiency of check-in</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "EfficiencyCheckIn", "_T2B" ) %></td>
                                <td><div id="EfficiencyCheckIn" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Friendliness of Front Desk staff</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FriendlinessFrontDesk", "_T2B" ) %></td>
                                <td><div id="FriendlinessFrontDesk" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Helpfulness of Front Desk staff</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "HelpfulnessFrontDesk", "_T2B" ) %></td>
                                <td><div id="HelpfulnessFrontDesk" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Employees' 'can-do' attitude</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "EmployeesAttitude", "_T2B" ) %></td>
                                <td><div id="EmployeesAttitude" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Efficiency of check-out</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "EfficiencyCheckOut", "_T2B" ) %></td>
                                <td><div id="EfficiencyCheckOut" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Accuracy of bill at check-out</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AccuracyCheckOut", "_T2B" ) %></td>
                                <td><div id="AccuracyCheckOut" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Valet parking</h3>
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
                                <td><%= ReportingTools.CalculatePercent( propOverall, "GreetingArrivalValet", "_T2B" ) %></td>
                                <td><div id="GreetingArrivalValet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Car returned in timely manner</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "CarReturnTimeValet", "_T2B" ) %></td>
                                <td><div id="CarReturnTimeValet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Original mirror position</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "OriginalMirrorValet", "_T2B" ) %></td>
                                <td><div id="OriginalMirrorValet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Original radio station </th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "OriginalRadioValet", "_T2B" ) %></td>
                                <td><div id="OriginalRadioValet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Original seat position</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "OriginalSeatValet", "_T2B" ) %></td>
                                <td><div id="OriginalSeatValet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Valet driver drove care in respectful manner</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "RespectValet", "_T2B" ) %></td>
                                <td><div id="RespectValet" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Pleasant departure greeting</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "PleasantValet", "_T2B" ) %></td>
                                <td><div id="PleasantValet" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="box box-default">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Bell / Door Service</h3>
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
                                <td><%= ReportingTools.CalculatePercent( propOverall, "GreetingArrivalBellService", "_T2B" ) %></td>
                                <td><div id="GreetingArrivalBellService" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Acknowledgement throughout stay</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AcknowledgementBellService", "_T2B" ) %></td>
                                <td><div id="AcknowledgementBellService" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Friendliness of bell/ door staff</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FriendlinessStaffBellService", "_T2B" ) %></td>
                                <td><div id="FriendlinessStaffBellService" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Employee knowledge of the River Rock Casino Resort &amp; Facilities</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "KnowledgeBellService", "_T2B" ) %></td>
                                <td><div id="KnowledgeBellService" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Staff member went out of way to provide excellent service</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "StaffServiceBellService", "_T2B" ) %></td>
                                <td><div id="StaffServiceBellService" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Pleasant departure greeting</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "DepartureBellService", "_T2B" ) %></td>
                                <td><div id="DepartureBellService" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Housekeeping &amp; Hotel Room</h3>
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
                                <th>Friendliness of Housekeeping staff</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FriendlinessHousekeeping", "_T2B" ) %></td>
                                <td><div id="FriendlinessHousekeeping" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Room cleanliness</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "RoomCleanliness", "_T2B" ) %></td>
                                <td><div id="RoomCleanliness" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Bathroom cleanliness</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "BathroomCleanliness", "_T2B" ) %></td>
                                <td><div id="BathroomCleanliness" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Towels & Linens </th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "TowelsAndLinens", "_T2B" ) %></td>
                                <td><div id="TowelsAndLinens" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Proper functioning of lights, TV, etc.</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "ProperFunctioningElectronics", "_T2B" ) %></td>
                                <td><div id="ProperFunctioningElectronics" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Overall condition of the room</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "OverallRoomCondition", "_T2B" ) %></td>
                                <td><div id="OverallRoomCondition" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Adequate amenities</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AdequateAmenities", "_T2B" ) %></td>
                                <td><div id="AdequateAmenities" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Fitness Centre &amp; Pool / Hot Tub</h3>
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
                                <th>Cleanliness of Fitness Center</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "CleanlinessFitness", "_T2B" ) %></td>
                                <td><div id="CleanlinessFitness" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Quality/ condition of fitness equipment</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "QualityFitness", "_T2B" ) %></td>
                                <td><div id="QualityFitness" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Availability of Fitness Center equipment</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AvailabilityFitness", "_T2B" ) %></td>
                                <td><div id="AvailabilityFitness" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Variety of equipment</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "VarietyFitness", "_T2B" ) %></td>
                                <td><div id="VarietyFitness" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Cleanliness of pool area</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "CleanlinessPool", "_T2B" ) %></td>
                                <td><div id="CleanlinessPool" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Temperature of pool</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "TemperaturePool", "_T2B" ) %></td>
                                <td><div id="TemperaturePool" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Cleanliness of hot tub area</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "CleanlinessHotTub", "_T2B" ) %></td>
                                <td><div id="CleanlinessHotTub" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Temperature of hot tub</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "TemperatureHotTub", "_T2B" ) %></td>
                                <td><div id="TemperatureHotTub" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Cleanliness of changing rooms</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "CleanlinessChangeRooms", "_T2B" ) %></td>
                                <td><div id="CleanlinessChangeRooms" class="mini-chart"></div></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="box box-primary">
                <div class="box-header with-border">
                    <i class="fa fa-check"></i>
                    <h3 class="box-title">Concierge</h3>
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
                                <th>Availability of Concierge</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "AvailabilityConcierge", "_T2B" ) %></td>
                                <td><div id="AvailabilityConcierge" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Friendliness of Concierge</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "FriendlinessConcierge", "_T2B" ) %></td>
                                <td><div id="FriendlinessConcierge" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Employee knowledge of the River Rock Casino Resort &amp; Facilities</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "KnowledgeConcierge", "_T2B" ) %></td>
                                <td><div id="KnowledgeConcierge" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Staff member went out of way to provide excellent service</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "StaffServiceConcierge", "_T2B" ) %></td>
                                <td><div id="StaffServiceConcierge" class="mini-chart"></div></td>
                            </tr>
							<tr>
                                <th>Pleasant departure greeting</th>
                                <td><%= ReportingTools.CalculatePercent( propOverall, "DepartureConcierge", "_T2B" ) %></td>
                                <td><div id="DepartureConcierge" class="mini-chart"></div></td>
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

        $.jqplot('FriendlinessReservationAgent', <%= GetJSData( propOverall, "FriendlinessReservationAgent" ) %>, opts);
        $.jqplot('HelpfulnessReservationAgent', <%= GetJSData( propOverall, "HelpfulnessReservationAgent" ) %>, opts);
        $.jqplot('AccuracyCheckIn', <%= GetJSData( propOverall, "AccuracyCheckIn" ) %>, opts);
        $.jqplot('EmployeeKnowledge', <%= GetJSData( propOverall, "EmployeeKnowledge" ) %>, opts);
        $.jqplot('EfficiencyCheckIn', <%= GetJSData( propOverall, "EfficiencyCheckIn" ) %>, opts);
        $.jqplot('FriendlinessFrontDesk', <%= GetJSData( propOverall, "FriendlinessFrontDesk" ) %>, opts);
        $.jqplot('HelpfulnessFrontDesk', <%= GetJSData( propOverall, "HelpfulnessFrontDesk" ) %>, opts);
        $.jqplot('EmployeesAttitude', <%= GetJSData( propOverall, "EmployeesAttitude" ) %>, opts);
        $.jqplot('EfficiencyCheckOut', <%= GetJSData( propOverall, "EfficiencyCheckOut" ) %>, opts);
        $.jqplot('AccuracyCheckOut', <%= GetJSData( propOverall, "AccuracyCheckOut" ) %>, opts);
        $.jqplot('FriendlinessHousekeeping', <%= GetJSData( propOverall, "FriendlinessHousekeeping" ) %>, opts);
        $.jqplot('RoomCleanliness', <%= GetJSData( propOverall, "RoomCleanliness" ) %>, opts);
        $.jqplot('BathroomCleanliness', <%= GetJSData( propOverall, "BathroomCleanliness" ) %>, opts);
        $.jqplot('TowelsAndLinens', <%= GetJSData( propOverall, "TowelsAndLinens" ) %>, opts);
        $.jqplot('ProperFunctioningElectronics', <%= GetJSData( propOverall, "ProperFunctioningElectronics" ) %>, opts);
        $.jqplot('OverallRoomCondition', <%= GetJSData( propOverall, "OverallRoomCondition" ) %>, opts);
        $.jqplot('AdequateAmenities', <%= GetJSData( propOverall, "AdequateAmenities" ) %>, opts);
        $.jqplot('CleanlinessFitness', <%= GetJSData( propOverall, "CleanlinessFitness" ) %>, opts);
        $.jqplot('QualityFitness', <%= GetJSData( propOverall, "QualityFitness" ) %>, opts);
        $.jqplot('AvailabilityFitness', <%= GetJSData( propOverall, "AvailabilityFitness" ) %>, opts);
        $.jqplot('VarietyFitness', <%= GetJSData( propOverall, "VarietyFitness" ) %>, opts);
        $.jqplot('CleanlinessPool', <%= GetJSData( propOverall, "CleanlinessPool" ) %>, opts);
        $.jqplot('TemperaturePool', <%= GetJSData( propOverall, "TemperaturePool" ) %>, opts);
        $.jqplot('CleanlinessHotTub', <%= GetJSData( propOverall, "CleanlinessHotTub" ) %>, opts);
        $.jqplot('TemperatureHotTub', <%= GetJSData( propOverall, "TemperatureHotTub" ) %>, opts);
        $.jqplot('CleanlinessChangeRooms', <%= GetJSData( propOverall, "CleanlinessChangeRooms" ) %>, opts);
        $.jqplot('GreetingArrivalValet', <%= GetJSData( propOverall, "GreetingArrivalValet" ) %>, opts);
        $.jqplot('CarReturnTimeValet', <%= GetJSData( propOverall, "CarReturnTimeValet" ) %>, opts);
        $.jqplot('OriginalMirrorValet', <%= GetJSData( propOverall, "OriginalMirrorValet" ) %>, opts);
        $.jqplot('OriginalRadioValet', <%= GetJSData( propOverall, "OriginalRadioValet" ) %>, opts);
        $.jqplot('OriginalSeatValet', <%= GetJSData( propOverall, "OriginalSeatValet" ) %>, opts);
        $.jqplot('RespectValet', <%= GetJSData( propOverall, "RespectValet" ) %>, opts);
        $.jqplot('PleasantValet', <%= GetJSData( propOverall, "PleasantValet" ) %>, opts);
        $.jqplot('AvailabilityConcierge', <%= GetJSData( propOverall, "AvailabilityConcierge" ) %>, opts);
        $.jqplot('FriendlinessConcierge', <%= GetJSData( propOverall, "FriendlinessConcierge" ) %>, opts);
        $.jqplot('KnowledgeConcierge', <%= GetJSData( propOverall, "KnowledgeConcierge" ) %>, opts);
        $.jqplot('StaffServiceConcierge', <%= GetJSData( propOverall, "StaffServiceConcierge" ) %>, opts);
        $.jqplot('DepartureConcierge', <%= GetJSData( propOverall, "DepartureConcierge" ) %>, opts);
        $.jqplot('GreetingArrivalBellService', <%= GetJSData( propOverall, "GreetingArrivalBellService" ) %>, opts);
        $.jqplot('AcknowledgementBellService', <%= GetJSData( propOverall, "AcknowledgementBellService" ) %>, opts);
        $.jqplot('FriendlinessStaffBellService', <%= GetJSData( propOverall, "FriendlinessStaffBellService" ) %>, opts);
        $.jqplot('KnowledgeBellService', <%= GetJSData( propOverall, "KnowledgeBellService" ) %>, opts);
        $.jqplot('StaffServiceBellService', <%= GetJSData( propOverall, "StaffServiceBellService" ) %>, opts);
        $.jqplot('DepartureBellService', <%= GetJSData( propOverall, "DepartureBellService" ) %>, opts);
        
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
            double dblVal = dr["RoomsScore"].ToString().StringToDbl( -100000 );
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
        canvas = $('#chart-RoomsScore').get(0).getContext("2d");
        chart = new Chart(canvas);
        chart.Line(chartDataBase, chartOptions);
    </script>
<% } %>
</asp:Content>
