<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="FeedbackItem.aspx.cs" Inherits="GCC_Web_Portal.Admin.FeedbackItem"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,PropertyStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>
<%@ Register Src="~/Admin/MessageTimeline.ascx" TagPrefix="uc1" TagName="MessageTimeline" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style>
        table.details > tbody > tr > td {
            text-align:left;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Conten>t
<asp:Content ContentPlaceHolderID="MainContentHeader" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<% if ( TopMessage.IsVisible ) { %>
<div class="row">
    <div class="col-md-6 col-md-offset-3">
        <sc:MessageManager runat="server" ID="TopMessage"></sc:MessageManager>
    </div>
</div>
<% } %>
<% if ( Data == null || Data.Tables[0].Rows.Count == 0 ) {
       if ( !TopMessage.IsVisible ) { %>
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
<% }
} else {
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
        <div class="col-xs-12 col-md-4">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <i class="fa fa-question"></i>
                            <h3 class="box-title">Feedback Request Details</h3>
                        </div>
                        <div class="box-body border-radius-none">
                            <table class="table table-bordered table-striped">
                                <tbody>
                                    <tr>
                                        <th>Property:</th>
                                        <td><%= feedbackDetailRow["CasinoName"] %><%= surveyTypeID == 1 && propertyID == 13 ? " - " + associatedSurveyRow["Q3"].ToString() :
                                                                                      surveyTypeID == 3 && propertyID == 13 ? " - " + associatedSurveyRow["GAGProperty"].ToString() : String.Empty %></td>
                                    </tr>
                                    <% if ( ( (SurveyType)feedbackDetailRow["SurveyTypeID"] ) == SurveyType.Feedback && !associatedSurveyRow["StaffMemberID"].Equals( DBNull.Value ) ) { %>
                                    <tr>
                                        <th>Entered By:</th>
                                        <td><%= ReportingTools.CleanData( associatedSurveyRow["EntryStaffMemberName"] ) %></td>
                                    </tr>
                                    <tr>
                                        <th>Contact Type:</th>
                                        <td><%= ReportingTools.CleanData( associatedSurveyRow["StaffContact"] ) %></td>
                                    </tr>
                                    <% } %>
                                    <tr>
                                        <th>Date Created:</th>
                                        <td><%= ReportingTools.AdjustAndDisplayDate( feedbackDetailRow["DateCreated"].ToString(), "MMMM dd, yyyy  @  hh:mm tt", User ) %></td>
                                    </tr>
                                    <tr>
                                        <th>Current Status:</th>
                                        <td>
                                            <% int statusID = (int)feedbackDetailRow["FeedbackStatusID"];
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
                                            <%= feedbackDetailRow["StatusName"] %>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Feedback Source:</th>
                                        <td><%= feedbackDetailRow["SurveyName"] %></td>
                                    </tr>
                                    <tr>
                                        <th>Feedback Reason / Area:</th>
                                        <td><%= feedbackDetailRow["ReasonDescription"] %></td>
                                    </tr>
                                    <tr>
                                        <th>Last Viewed by Guest:</th>
                                        <td>
                                            <% if ( feedbackDetailRow["DateLastViewedByGuest"].Equals( DBNull.Value ) ) { %>
                                            Never
                                            <% } else { %>
                                            <%= Conversion.XMLDateToDateTime( feedbackDetailRow["DateLastViewedByGuest"].ToString() ).ToString("MMMM dd, yyyy  @  hh:mm tt") %>
                                            <% } %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Player Tier:</th>
                                        <% if ( feedbackDetailRow["SurveyTypeID"].Equals( (int)SurveyType.GEI ) ) { %>
                                        <td><%= associatedSurveyRow["PlayerTier"] %></td>
                                        <% } %>
                                    </tr>
                                    <tr>
                                        <th>Market Share:</th>
                                        <td>
                                             <% if ( feedbackDetailRow["SurveyTypeID"].Equals( (int)SurveyType.GEI ) ) { %>
                                            <% if (associatedSurveyRow["MarketShare"].ToString() == "0.0%") { %>
                                            --
                                            <% } else { %>
                                            <%= associatedSurveyRow["MarketShare"] %>
                                            <% } %>
                                            <% } %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>Feedback Tier:</th>
                                        <td>
                                            <div class="form-inline">
                                                <asp:HiddenField runat="server" ID="hdnOldTier" />
                                                <asp:DropDownList runat="server" ID="ddlTier" CssClass="form-control">
                                                    <asp:ListItem Text="Tier 1" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Tier 2" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Tier 3" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Button runat="server" ID="btnSaveTier" CssClass="btn btn-warning" Text="Save" OnClick="btnSaveTier_Click" UseSubmitBehavior="false"/>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-danger">
                        <div class="box-header with-border">
                            <i class="fa fa-cog"></i>
                            <h3 class="box-title">Options</h3>
                        </div>
                        <div class="box-body border-radius-none">
                            <%if ( IssueIsClosed ) { %>
                            <asp:LinkButton runat="server" ID="btnReopen" CssClass="btn btn-warning" OnClick="btnReopen_OnClick" Text="Reopen this Request" />
                            <% } else { %>
                            <a href="#" id="btnShowCloseModal" class="btn btn-danger" data-toggle="modal" data-target="#close-modal">Close this Request...  <i class='fa fa-external-link'></i></a>
                            <% } %>
                            <a href="/F/<%= GUID %>?a=1" title="Guest Version" class="btn btn-primary" target="_blank">View as Guest <i class='fa fa-external-link'></i></a>
                            <a href="/Display/<%= ((SurveyType)feedbackDetailRow["SurveyTypeID"]).ToString() %>/<%= feedbackDetailRow["RecordID"] %>" title="View Full Survey" class="btn btn-success" target="_blank">View Full Survey <i class='fa fa-external-link'></i></a>
                            <%--Work in process, Adding the ability to change feedback status to invalid and then exclude that case from any reports--%>
                            <%--<a href="#" runat="server" id="btnShowInvalidModal" class="btn btn-warning" style="margin-top:5px" data-toggle="modal" data-target="#invalid-modal">Mark Invalid <i class='fa fa-external-link'></i></a>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-md-8">
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-list-ol"></i>
                    <h3 class="box-title"><%= feedbackDetailRow["SurveyName"] %> Survey Details</h3>
                </div>
                <div class="box-body border-radius-none">
                    <% if ( feedbackDetailRow["SurveyTypeID"].Equals( (int)SurveyType.GEI ) ) { %>
                    <table class="table table-bordered table-striped details">
                        <tbody>
                            <tr>
                                <th style="width:15%;">Date&nbsp;Entered:</th>
                                <td colspan="3"><%= ReportingTools.AdjustAndDisplayDate( associatedSurveyRow["DateCreated"].ToString(), @"MMMM&nb\sp;dd,&nb\sp;yyyy  hh:mm:ss&nb\sp;tt", User ) %></td>
                            </tr>
                            <tr>
                                <th style="width:15%;">Player Card&nbsp;#:</th>
                                <td style="width:15%;"><%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( associatedSurveyRow["Q4CardNumber"].ToString() ) %></td>
                                <th style="width:10%;">Name:</th>
                                <td>
                                    <% if ( String.IsNullOrEmpty( associatedSurveyRow["FirstName"].ToString() ) ) { %>
                                    Not Given
                                    <% } else { %>
                                    <%= ReportingTools.CleanData( associatedSurveyRow["FirstName"] ) %> <%= ReportingTools.CleanData( associatedSurveyRow["LastName"] ) %></td>
                                    <% } %>
                            </tr>
                            <tr>
                                <th>Telephone:</th>
                                <td colspan="3"><%= ReportingTools.CleanData( associatedSurveyRow["TelephoneNumber"] ) %></td>
                            </tr>
                            <tr>
                                <th>Email:</th>
                                <td colspan="3"><%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( associatedSurveyRow["Email"].ToString() ) %></td>
                            </tr>
                            <% if ( associatedSurveyRow["Q40"].Equals( 1 ) || associatedSurveyRow["Q40"].Equals( 0 ) ) { %>
                            <tr>
                                <th colspan="2">Would you like someone to contact you?</th>
                                <td colspan="2"><%= associatedSurveyRow["Q40"].Equals( 1 ) ? "Yes" : "No" %></td>
                            </tr>
                            <% } %>
                            <tr>
                                <th colspan="2">Did the guest experience any problems?</th>
                                <td colspan="2"><%= associatedSurveyRow["Q27"].Equals( 1 ) ? "Yes - " : "No" %> <%= ReportingTools.CleanData( associatedSurveyRow["Q27B"] ) %></td>
                            </tr>
                            <% if ( !String.IsNullOrWhiteSpace( associatedSurveyRow["Q11"].ToString() ) ) { %>
                            <tr>
                                <th colspan="2">Comments about staff:</th>
                                <td colspan="2"><%= ReportingTools.CleanData( associatedSurveyRow["Q11"] ) %></td>
                            </tr>
                            <% } %>
                            <% if ( !String.IsNullOrWhiteSpace( associatedSurveyRow["Q34"].ToString() ) ) { %>
                            <tr>
                                <th colspan="2">General comments or suggestions:</th>
                                <td colspan="2"><%= ReportingTools.CleanData( associatedSurveyRow["Q34"] ) %></td>
                            </tr>
                            <% } %>
                            <% if ( !String.IsNullOrWhiteSpace( associatedSurveyRow["Q35"].ToString() ) ) { %>
                            <tr>
                                <th colspan="2">Memorable employee or area:</th>
                                <td colspan="2"><%= ReportingTools.CleanData( associatedSurveyRow["Q35"] ) %></td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                    <% } else if ( feedbackDetailRow["SurveyTypeID"].Equals( (int)SurveyType.Hotel ) ) { %>
                    <table class="table table-bordered table-striped details">
                        <tbody>
                            <tr>
                                <th style="width:25%;">Name:</th>
                                <td><%= ReportingTools.CleanData( associatedSurveyRow["FirstName"] ) %> <%= ReportingTools.CleanData( associatedSurveyRow["LastName"] ) %></td>
                            </tr>
                            <tr>
                                <th>Telephone:</th>
                                <td><%= ReportingTools.CleanData( associatedSurveyRow["TelephoneNumber"] ) %></td>
                            </tr>
                            <tr>
                                <th>Email:</th>
                                <td><%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( ReportingTools.CleanData( associatedSurveyRow["ContactEmail"] ) ) %></td>
                            </tr>
                            <tr>
                                <th>Follow up reason:</th>
                                <td><%= ReportingTools.CleanData( associatedSurveyRow["FollowupReason"] ) %></td>
                            </tr>
                            <tr>
                                <th>Please briefly describe your problem:</th>
                                <td><%= ReportingTools.CleanData( associatedSurveyRow["Q24B"] ) %></td>
                            </tr>
                        </tbody>
                    </table>
                    <% } else if ( feedbackDetailRow["SurveyTypeID"].Equals( (int)SurveyType.Feedback ) ) { %>
                    <table class="table table-bordered table-striped details">
                        <tbody>
                            <tr>
                                <th style="width:35%;">Date&nbsp;Entered:</th>
                                <td><%= ReportingTools.AdjustAndDisplayDate( associatedSurveyRow["DateCreated"].ToString(), @"MMMM&nb\sp;dd,&nb\sp;yyyy  hh:mm:ss&nb\sp;tt", User ) %></td>
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
                                <td><%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( ReportingTools.CleanData( associatedSurveyRow["ContactEmail"] ) ) %></td>
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
                                <th>4. Do you have a(n) <%= PropertyTools.GetPlayersClubName(associatedSurveyRow["PropertyID"].ToString().StringToInt()) %> card?</th>
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
                                <td><%= ReportingTools.AdjustAndDisplayDate( associatedSurveyRow["DateCreated"].ToString(), @"MMMM&nb\sp;dd,&nb\sp;yyyy  hh:mm:ss&nb\sp;tt", User ) %></td>
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
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <uc1:MessageTimeline runat="server" id="MessageTimeline" />
        </div>
    </div>
    <div class="modal fade" id="close-modal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Close Reason</h4>
                </div>
                <div class="modal-body">
                    <p>Please select the reason you are closing this case. After closing, replies will be disabled unless it is re-opened.
                        <br />Note: This reason will not be shown to the user. They will just see that it was "Closed".</p>
                    <asp:DropDownList runat="server" ID="ddlCloseReason">
                        <asp:ListItem Text="Guest Response Complete" Value="3"></asp:ListItem>
                        <asp:ListItem Text="No Further Action Required" Value="4"></asp:ListItem>
                        <asp:ListItem Text="Unable to Satisfy Guest" Value="5"></asp:ListItem>
                        <asp:ListItem Text="No Response" Value="6"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" ID="btnClose" CssClass="btn btn-danger" OnClick="btnClose_OnClick" Text="Close and Save" />
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->
    <div class="modal fade" id="invalid-modal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Invalid Response Confirmation</h4>
                </div>
                <div class="modal-body">
                    <p>Are you sure that you want to mark this case as invalid?
                        <br />Note: If marked invalid, the user will only see that it was "Closed".</p>
                   <%-- <asp:DropDownList runat="server" ID="ddlInvalidReason">
                        <asp:ListItem Text="Reason 1" Value="7"></asp:ListItem>
                        <asp:ListItem Text="Reason 2" Value="8"></asp:ListItem>
                        <asp:ListItem Text="Reason 3" Value="9"></asp:ListItem>                       
                    </asp:DropDownList>--%>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton runat="server" ID="btnInvalid" CssClass="btn btn-danger" OnClick="btnInvalid_OnClick" Text="Mark Invalid and Close" />
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <!-- /.modal -->
<% } %>
</asp:Content>
<asp:Content ContentPlaceHolderID="FooterScripts" runat="server">
</asp:Content>
