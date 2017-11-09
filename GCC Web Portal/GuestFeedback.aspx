<%@ Page Title="" Language="C#" MasterPageFile="~/Survey.Master" AutoEventWireup="true" CodeBehind="GuestFeedback.aspx.cs" Inherits="GCC_Web_Portal.GuestFeedback" %>
<%@ MasterType VirtualPath="~/Survey.Master" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Register Src="~/Controls/QuestionRowControl.ascx" TagPrefix="uc1" TagName="QuestionRowControl" %>
<%@ Register Src="~/Controls/ScaleQuestionControl.ascx" TagPrefix="uc1" TagName="ScaleQuestionControl" %>
<%@ Register Src="~/Controls/YesNoControl.ascx" TagPrefix="uc1" TagName="YesNoControl" %>
<%@ Register Src="~/Controls/TriQuestionRowControl.ascx" TagPrefix="uc1" TagName="TriQuestionRowControl" %>
<%@ Register Src="~/Admin/MessageTimeline.ascx" TagPrefix="uc1" TagName="MessageTimeline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <style>
        .timeline {
          position: relative;
          margin: 0 0 30px 0;
          padding: 0;
          list-style: none;
        }
        .timeline:before {
          content: '';
          position: absolute;
          top: 0px;
          bottom: 0;
          width: 4px;
          background: #ddd;
          left: 31px;
          margin: 0;
          border-radius: 2px;
        }
        .timeline > li {
          position: relative;
          margin-right: 10px;
          margin-bottom: 15px;
        }
        .timeline > li:before,
        .timeline > li:after {
          content: " ";
          display: table;
        }
        .timeline > li:after {
          clear: both;
        }
        .timeline > li > .timeline-item {
          -webkit-box-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);
          box-shadow: 0 1px 1px rgba(0, 0, 0, 0.1);
          border-radius: 3px;
          margin-top: 0px;
          background: #fff;
          color: #444;
          margin-left: 60px;
          margin-right: 15px;
          padding: 0;
          position: relative;
        }
        .timeline > li > .timeline-item > .time {
          color: #999;
          float: right;
          padding: 10px;
          font-size: 12px;
        }
        .timeline > li > .timeline-item > .timeline-header {
          margin: 0;
          color: #555;
          border-bottom: 1px solid #f4f4f4;
          padding: 10px;
          font-size: 16px;
          line-height: 1.1;
          text-align:left;
        }
        .timeline > li > .timeline-item > .timeline-header > a {
          font-weight: 600;
        }
        .timeline > li > .timeline-item > .timeline-body,
        .timeline > li > .timeline-item > .timeline-footer {
          padding: 10px;
        }
        .timeline > li.time-label > span {
          font-weight: 600;
          padding: 5px;
          display: inline-block;
          background-color: #fff;
          border-radius: 4px;
        }
        .timeline > li > .fa,
        .timeline > li > .glyphicon,
        .timeline > li > .ion {
          width: 30px;
          height: 30px;
          font-size: 15px;
          line-height: 30px;
          position: absolute;
          color: #666;
          background: #d2d6de;
          border-radius: 50%;
          text-align: center;
          left: 18px;
          top: 0;
        }

        .bg-red,
        .bg-yellow,
        .bg-aqua,
        .bg-blue,
        .bg-light-blue,
        .bg-green,
        .bg-navy,
        .bg-teal,
        .bg-olive,
        .bg-lime,
        .bg-orange,
        .bg-fuchsia,
        .bg-purple,
        .bg-maroon,
        .bg-black,
        .bg-red-active,
        .bg-yellow-active,
        .bg-aqua-active,
        .bg-blue-active,
        .bg-light-blue-active,
        .bg-green-active,
        .bg-navy-active,
        .bg-teal-active,
        .bg-olive-active,
        .bg-lime-active,
        .bg-orange-active,
        .bg-fuchsia-active,
        .bg-purple-active,
        .bg-maroon-active,
        .bg-black-active,
        .callout.callout-danger,
        .callout.callout-warning,
        .callout.callout-info,
        .callout.callout-success,
        .alert-success,
        .alert-danger,
        .alert-error,
        .alert-warning,
        .alert-info,
        .label-danger,
        .label-info,
        .label-waring,
        .label-primary,
        .label-success,
        .modal-primary .modal-body,
        .modal-primary .modal-header,
        .modal-primary .modal-footer,
        .modal-warning .modal-body,
        .modal-warning .modal-header,
        .modal-warning .modal-footer,
        .modal-info .modal-body,
        .modal-info .modal-header,
        .modal-info .modal-footer,
        .modal-success .modal-body,
        .modal-success .modal-header,
        .modal-success .modal-footer,
        .modal-danger .modal-body,
        .modal-danger .modal-header,
        .modal-danger .modal-footer {
            color: #fff !important;
        }
        .bg-gray {
            color: #000;
            background-color: #d2d6de !important;
        }
        .bg-black {
            background-color: #111111 !important;
        }
        .bg-red,
        .callout.callout-danger,
        .alert-danger,
        .alert-error,
        .label-danger,
        .modal-danger .modal-body {
            background-color: #dd4b39 !important;
        }
        .bg-yellow,
        .callout.callout-warning,
        .alert-warning,
        .label-waring,
        .modal-warning .modal-body {
            background-color: #f39c12 !important;
        }
        .bg-aqua,
        .callout.callout-info,
        .alert-info,
        .label-info,
        .modal-info .modal-body {
            background-color: #00c0ef !important;
        }
        .bg-blue {
            background-color: #0073b7 !important;
        }
        .bg-light-blue,
        .label-primary,
        .modal-primary .modal-body {
            background-color: #3c8dbc !important;
        }
        .bg-green,
        .callout.callout-success,
        .alert-success,
        .label-success,
        .modal-success .modal-body {
            background-color: #00a65a !important;
        }
        .bg-navy {
            background-color: #001f3f !important;
        }
        .bg-teal {
            background-color: #39cccc !important;
        }
        .bg-olive {
            background-color: #3d9970 !important;
        }
        .bg-lime {
            background-color: #01ff70 !important;
        }
        .bg-orange {
            background-color: #ff851b !important;
        }
        .bg-fuchsia {
            background-color: #f012be !important;
        }
        .bg-purple {
            background-color: #605ca8 !important;
        }
        .bg-maroon {
            background-color: #d81b60 !important;
        }
        .bg-gray-active {
            color: #000;
            background-color: #b5bbc8 !important;
        }
        .bg-black-active {
            background-color: #000000 !important;
        }
        .bg-red-active,
        .modal-danger .modal-header,
        .modal-danger .modal-footer {
            background-color: #d33724 !important;
        }
        .bg-yellow-active,
        .modal-warning .modal-header,
        .modal-warning .modal-footer {
            background-color: #db8b0b !important;
        }
        .bg-aqua-active,
        .modal-info .modal-header,
        .modal-info .modal-footer {
            background-color: #00a7d0 !important;
        }
        .bg-blue-active {
            background-color: #005384 !important;
        }
        .bg-light-blue-active,
        .modal-primary .modal-header,
        .modal-primary .modal-footer {
            background-color: #357ca5 !important;
        }
        .bg-green-active,
        .modal-success .modal-header,
        .modal-success .modal-footer {
            background-color: #008d4c !important;
        }
        .bg-navy-active {
            background-color: #001a35 !important;
        }
        .bg-teal-active {
            background-color: #30bbbb !important;
        }
        .bg-olive-active {
            background-color: #368763 !important;
        }
        .bg-lime-active {
            background-color: #00e765 !important;
        }
        .bg-orange-active {
            background-color: #ff7701 !important;
        }
        .bg-fuchsia-active {
            background-color: #db0ead !important;
        }
        .bg-purple-active {
            background-color: #555299 !important;
        }
        .bg-maroon-active {
            background-color: #ca195a !important;
        }
        [class^="bg-"].disabled {
            opacity: 0.65;
            filter: alpha(opacity=65);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
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
    DataRow feedbackDetailRow = Data.Tables[0].Rows[0];
    DataTable feedbackMessages = Data.Tables[1];
    DataRow associatedSurveyRow = null;
    if ( Data.Tables.Count > 2 && Data.Tables[2].Rows.Count > 0 ) {
        associatedSurveyRow = Data.Tables[2].Rows[0];
    }
    int propertyID = feedbackDetailRow["PropertyID"].ToString().StringToInt();
    int surveyTypeID = feedbackDetailRow["SurveyTypeID"].ToString().StringToInt();
 %>
    <div class="row">
        <div class="col-xs-12">
            <% if ( surveyTypeID.Equals( (int)SurveyType.GEI ) ) { %>
            <table class="table table-bordered table-striped details">
                <tbody>
                    <tr>
                        <th style="width:30%;">Date&nbsp;Entered:</th>
                        <td><%= ReportingTools.AdjustDateTime( Conversion.XMLDateToDateTime( associatedSurveyRow["DateCreated"].ToString() ), false, PropertyTools.GetCasinoTimezone( (GCCPropertyShortCode)propertyID ) ).ToString( @"MMMM&nb\sp;dd,&nb\sp;yyyy  hh:mm:ss&nb\sp;tt" ) %></td>
                    </tr>
                    <tr>
                        <th>Name:</th>
                        <td>
                            <% if ( String.IsNullOrEmpty( associatedSurveyRow["FirstName"].ToString() ) ) { %>
                            Not Given
                            <% } else { %>
                            <%= ReportingTools.CleanData( associatedSurveyRow["FirstName"] ) %> <%= ReportingTools.CleanData( associatedSurveyRow["LastName"] ) %></td>
                            <% } %>
                    </tr>
                    <tr>
                        <th>Telephone:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["TelephoneNumber"] ) %></td>
                    </tr>
                    <tr>
                        <th>Email:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Email"] ) %></td>
                    </tr>
                    <% if ( associatedSurveyRow["Q40"].Equals( 1 ) || associatedSurveyRow["Q40"].Equals( 0 ) ) { %>
                    <tr>
                        <th>Would you like someone to contact you?</th>
                        <td><%= associatedSurveyRow["Q40"].Equals( 1 ) ? "Yes" : "No" %></td>
                    </tr>
                    <% } %>
                    <tr>
                        <th>Did you experience any problems?</th>
                        <td><%= associatedSurveyRow["Q27"].Equals( 1 ) ? "Yes - " : "No" %> <%= ReportingTools.CleanData( associatedSurveyRow["Q27B"] ) %></td>
                    </tr>
                    <% if ( !String.IsNullOrWhiteSpace( associatedSurveyRow["Q11"].ToString() ) ) { %>
                    <tr>
                        <th>Comments about staff:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q11"] ) %></td>
                    </tr>
                    <% } %>
                    <% if ( !String.IsNullOrWhiteSpace( associatedSurveyRow["Q34"].ToString() ) ) { %>
                    <tr>
                        <th>General comments or suggestions:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q34"] ) %></td>
                    </tr>
                    <% } %>
                </tbody>
            </table>
            <% } else if ( surveyTypeID.Equals( (int)SurveyType.Hotel ) ) { %>
            <table class="table table-bordered table-striped details">
                <tbody>
                    <tr>
                        <th>Name:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["FirstName"] ) %> <%= ReportingTools.CleanData( associatedSurveyRow["LastName"] ) %></td>
                    </tr>
                    <tr>
                        <th>Telephone:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["TelephoneNumber"] ) %></td>
                    </tr>
                    <tr>
                        <th>Email:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["ContactEmail"] ) %></td>
                    </tr>
                    <tr>
                        <th>Follow up reason:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["FollowupReason"] ) %></td>
                    </tr>
                </tbody>
            </table>
            <% } else if ( surveyTypeID.Equals( (int)SurveyType.Feedback ) ) { %>
            <table class="table table-bordered table-striped details">
                <tbody>
                    <tr>
                        <th style="width:35%;">Date&nbsp;Entered:</th>
                        <td><%= ReportingTools.AdjustDateTime( Conversion.XMLDateToDateTime( associatedSurveyRow["DateCreated"].ToString() ), false, PropertyTools.GetCasinoTimezone( (GCCPropertyShortCode)propertyID ) ).ToString( @"MMMM&nb\sp;dd,&nb\sp;yyyy  hh:mm:ss&nb\sp;tt" ) %></td>
                    </tr>
                    <tr>
                        <th>Name:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Name"] ) %></td>
                    </tr>
                    <tr>
                        <th>Telephone:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["TelephoneNumber"] ) %></td>
                    </tr>
                    <tr>
                        <th>Email:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["ContactEmail"] ) %></td>
                    </tr>
                    <tr>
                        <th>1. What type of feedback do you want to give?</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q1"] ) %></td>
                    </tr>
                    <tr>
                        <th>2. What part of our business does this relate to?</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q2Description"] ) %></td>
                    </tr>
                    <tr>
                        <th>3. Please provide your feedback or the details of your request below.</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q3"] ) %></td>
                    </tr>
                    <tr>
                        <th>4. Do you have an Encore Rewards card?</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q4"] ) %></td>
                    </tr>
                    <tr>
                        <th>5. Would you like someone to contact you about your feedback?</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q5"] ) %></td>
                    </tr>
                </tbody>
            </table>
            <% } else if ( feedbackDetailRow["SurveyTypeID"].Equals( (int)SurveyType.Donation ) ) { %>
            <table class="table table-bordered table-striped details">
                <tbody>
                    <tr>
                        <th style="width:35%;">Date&nbsp;Entered:</th>
                        <td><%= ReportingTools.AdjustDateTime( Conversion.XMLDateToDateTime( associatedSurveyRow["DateCreated"].ToString() ), false, PropertyTools.GetCasinoTimezone( (GCCPropertyShortCode)propertyID ) ).ToString( @"MMMM&nb\sp;dd,&nb\sp;yyyy  hh:mm:ss&nb\sp;tt" ) %></td>
                    </tr>
                    <tr>
                        <th>Organization Name:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q1"] ) %></td>
                    </tr>
                    <tr>
                        <th>Contact:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q5Name"] ) %> - <%= ReportingTools.CleanData( associatedSurveyRow["Q5Title"] ) %></td>
                    </tr>
                    <tr>
                        <th>Contact Telephone:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q5Telephone"] ) %></td>
                    </tr>
                    <tr>
                        <th>Contact Email:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q5Email"] ) %></td>
                    </tr>
                    <tr>
                        <th>Address:</th>
                        <td>
                            <%= ReportingTools.CleanData( associatedSurveyRow["Q6Street"] ) %><br />
                            <%= ReportingTools.CleanData( associatedSurveyRow["Q6City"] ) %>, <%= ReportingTools.CleanData( associatedSurveyRow["Q6Province"] ) %>  <%= ReportingTools.CleanData( associatedSurveyRow["Q6PostalCode"] ) %></td>
                    </tr>
                    <tr>
                        <th>Seeking:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q9"] ) + ( associatedSurveyRow["Q9"].Equals("OTHER") ? " - " + ReportingTools.CleanData( associatedSurveyRow["Q9C_Explanation"] ) : String.Empty  ) %></td>
                    </tr>
                    <tr>
                        <th>Amount / Value of Request:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q10"] ) %></td>
                    </tr>
                    <tr>
                        <th>Request Details:</th>
                        <td><%= ReportingTools.CleanData( associatedSurveyRow["Q14"] ) %></td>
                    </tr>
                </tbody>
            </table>
            <% } %>
        </div>
    </div>
    <uc1:MessageTimeline runat="server" id="MessageTimeline" />
<% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="server"></asp:Content>
