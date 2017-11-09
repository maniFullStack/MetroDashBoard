<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageTimeline.ascx.cs" Inherits="GCC_Web_Portal.Admin.MessageTimeline" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<% if ( Messages != null ) { %>
<asp:HiddenField runat="server" ID="hdnLastTab" />
<ul class="timeline">
    <%
    DateTime lastDate = DateTime.MinValue.Date;
    foreach( DataRow dr in Messages.Rows ) {
        DateTime curDate = Conversion.XMLDateToDateTime( dr["DateCreated"].ToString() );
        curDate = ReportingTools.AdjustDateTime( curDate, false, PropertyTools.GetCasinoTimezone( PropertyShortCode ) );
        FeedbackEventType ft = (FeedbackEventType)dr["FeedbackEventTypeID"];
        bool isStatusChange = ( ft == FeedbackEventType.StatusChange );

        //Make double sure we can't see staff notes in the guest version (also handled in the db)
        if ( IsGuestVersion && ( ft == FeedbackEventType.StaffNote || ft == FeedbackEventType.TierChange ) ) {
            continue;
        }
        
        if ( curDate.Date != lastDate.Date ) { %>
    <li class="time-label">
        <span class="bg-green"><%= curDate.ToString("MMMM dd, yyyy") %></span>
    </li>
    <%      lastDate = curDate.Date;
        } %>
    <li>
        <% string iconClass = "fa-comments " + ( ft == FeedbackEventType.StaffMessage ? "bg-green" : "bg-blue" );
           if ( ft == FeedbackEventType.StaffNote ) {
               iconClass = "fa-sticky-note bg-aqua";
           } else if ( ft == FeedbackEventType.TierChange ) {
               iconClass = "fa-exclamation-circle bg-yellow";
           } else if ( isStatusChange ) {
               iconClass = "fa-exclamation bg-yellow";
           }
        %>
        <i class="fa <%= iconClass %>"></i>
        <div class="timeline-item">
            <span class="time">
                <i class="fa fa-clock-o"></i> <%= curDate.ToString( "hh:mm tt" ) %>
                <% if ( !IsGuestVersion && !dr["StaffMemberName"].Equals( DBNull.Value ) ) {%>
                by <%= dr["StaffMemberName"] %>
                    <% if ( ft == FeedbackEventType.StaffNote && !dr["StaffContact"].Equals( DBNull.Value ) ) { %>
                    (<%= dr["StaffContact"] %>)
                    <% } %>
                <% } %>
            </span>
            <h3 class="timeline-header">
                <% if ( ft == FeedbackEventType.StatusChange || ft == FeedbackEventType.TierChange ) { %>
                <%= ReportingTools.CleanData( dr["Message"] ) %>
                <% } else if ( ft == FeedbackEventType.StaffMessage ) { %>
                Staff Message
                <% } else if ( ft == FeedbackEventType.PlayerMessage ) { %>
                <%= IsGuestVersion ? "Your" : "Guest" %> Message
                <% } else if ( ft == FeedbackEventType.StaffNote ) { %>
                Staff Note
                <% } %>
            </h3>
            <% if ( !isStatusChange && ft != FeedbackEventType.TierChange ) { %>
            <div class="timeline-body">
                <%= ReportingTools.CleanData( dr["Message"] ) %>
            </div>
            <% } %>
            <%--<div class='timeline-footer'></div>--%>
        </div>
    </li>
    <% } %>
    <% if ( !HideReplyBox ) {
        if ( !lastDate.Equals( DateTime.Today ) ) { %>
    <li class="time-label">
        <span class="bg-red"><%= DateTime.Now.ToString("MMMM dd, yyyy") %></span>
    </li>
    <%  } %>
    <li>
        <i class="fa fa-bullhorn bg-yellow"></i>
        <div class="timeline-item">
            <% if ( IsGuestVersion ) { %>
            <span class="time"></span>
            <h3 class="timeline-header">
                Send Reply
            </h3>
            <% } %>
            <div class="timeline-body">
                <sc:MessageManager runat="server" ID="MessageManager"></sc:MessageManager>
                <% if ( !IsGuestVersion ) { %>
                <div class="nav-tabs-custom">
                    <ul class="nav nav-tabs">
                        <li class="<%= !hdnLastTab.Value.Equals("note") ? "active" : "" %>"><a href="#tab_1" data-toggle="tab"><i class="fa fa-comments"></i> Send Reply</a></li>
                        <li class="<%= hdnLastTab.Value.Equals("note") ? "active" : "" %>"><a href="#tab_2" data-toggle="tab"><i class="fa fa-sticky-note"></i> Add Note</a></li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane<%= !hdnLastTab.Value.Equals("note") ? " active" : "" %>" id="tab_1">
                            <p>Enter text in the box below to send a reply to the guest. They will be notified via email.</p>
                <% } %>
                            <asp:TextBox runat="server" ID="txtReplyMessage" style="width:100%" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            <br />
                            <asp:Button runat="server" ID="btnSendReply" Text="Send reply to guest" CssClass="btn btn-success" OnClick="btnSendReply_Click" />
                <% if ( !IsGuestVersion ) { %>
                        </div>
                        <div class="tab-pane<%= hdnLastTab.Value.Equals("note") ? " active" : "" %>" id="tab_2">
                            <p>Enter text in the box below to add a note to this feedback item. This will not be shown to the guest.</p>
                            <asp:TextBox runat="server" ID="txtStaffNote" style="width:100%" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            <p>
                                Interaction: <asp:DropDownList runat="server" ID="ddlGuestInteraction">
                                                <asp:ListItem Text="--" Value=""></asp:ListItem>
                                                <asp:ListItem Text="No interaction" Value="No interaction"></asp:ListItem>
                                                <asp:ListItem Text="In-person" Value="In-person"></asp:ListItem>
                                                <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>
                                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                                             </asp:DropDownList>
                            </p>
                            <asp:Button runat="server" ID="btnAddNote" Text="Add note for staff" CssClass="btn btn-success" OnClick="btnStaffNote_Click" />
                        </div>
                    </div>
                </div>
                <% } %>
            </div>
        </div>
    </li>
    <% } %>
    <li>
        <a name="end-of-messages"></a>
        <i class="fa fa-clock-o bg-gray"></i>
    </li>
</ul>
<% } %>