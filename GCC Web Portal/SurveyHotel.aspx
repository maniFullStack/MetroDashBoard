<%@ Page Title="" Language="C#" MasterPageFile="~/Survey.Master" AutoEventWireup="true" CodeBehind="SurveyHotel.aspx.cs" Inherits="GCC_Web_Portal.SurveyHotel" %>
<%@ MasterType VirtualPath="~/Survey.Master" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/QuestionRowControl.ascx" TagPrefix="uc1" TagName="QuestionRowControl" %>
<%@ Register Src="~/Controls/ScaleQuestionControl.ascx" TagPrefix="uc1" TagName="ScaleQuestionControl" %>
<%@ Register Src="~/Controls/YesNoControl.ascx" TagPrefix="uc1" TagName="YesNoControl" %>
<%@ Register Src="~/Controls/TriQuestionRowControl.ascx" TagPrefix="uc1" TagName="TriQuestionRowControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
        .header-label {
            text-align:center;
            margin-top:20px;
        }
        span.radalign {
            display: table;
        }
        span.radalign > input{
            display: table-cell;
        }
        span.radalign > label {
            display: table-cell;
            vertical-align: top;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <%
        //===========================================================================
        //PAGE 1 - Intro
        //===========================================================================
    if ( Master.CurrentPage == 1 ) {%>
    <h1><%= Master.CasinoName %> Hotel Survey</h1>
    <p>Thank you for your recent visit to <%= Master.CasinoName %> & Hotel!</p>
    <p>Your enjoyment is important to us, and we would be delighted if you would share your thoughts and experiences with us by completing this survey.</p>
    <p>All of our surveys are conducted confidentially. Should you wish for us to respond to you regarding your comments, please indicate so and provide your contact information at the end of this survey.</p>
    <p>To start, please confirm your email address and click "next" below.</p>
    <sc:MessageManager runat="server" ID="mmTxtEmail"></sc:MessageManager>
    <p>
        Email address:
            <sc:SurveyTextBox ID="txtEmail" runat="server" SessionKey="txtEmail" DBColumn="Email" MaxLength="150" Size="50"></sc:SurveyTextBox>
    </p>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 2 - Privacy Policy
        //===========================================================================
    else if ( Master.CurrentPage == 2 ) {%>
    <h2>Personal Information and Privacy policy</h2>
    <p>Your personal information is collected and used by Great Canadian Gaming Corporation (GCGC) on behalf of the British Columbia Lottery Corporation in accordance with British Columbia’s Freedom of Information and Protection of Privacy Act. It will be used for GCGC’s research purposes only. Your information will not be sold, shared with third parties, or used for soliciting purposes. If you have any questions about this, please write to GCGC’s Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
    <p class="question">
        By clicking on "I agree" and providing your email address, you accept the Personal Information and Privacy policy. The survey should take approximately 5 minutes to complete depending on your comments.
    </p>
    <sc:MessageManager runat="server" ID="mmAcceptGroup"></sc:MessageManager>
    <p>
        <sc:SurveyRadioButton ID="radAccept" runat="server" GroupName="acceptgrp" SessionKey="radAccept" CssClass="radalign" Text="&nbsp;I agree and want to proceed with the survey" /><br />
        <sc:SurveyRadioButton ID="radDecline" runat="server" GroupName="acceptgrp" SessionKey="radDecline" CssClass="radalign" Text="&nbsp;I decline to complete the survey" />
    </p>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 99 - Declined Message
        //===========================================================================
        else if ( Master.CurrentPage == 99 ) { %>
    <p>We acknowledge that you have chosen not to participate in the survey. Thank you for your recent visit and we look forward to seeing you again soon!</p>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Undo" />
        <a href="<%= PropertyTools.GetCasinoURL( Master.PropertyShortCode ) %>" class="btn btn-primary">End Survey</a>
    </div>
    <% }
        //===========================================================================
        //PAGE 1 - During your stay
        //===========================================================================
    else if ( Master.CurrentPage == 3) {%>
    <p class="question">How satisfied were you with your stay at the River Rock Casino Hotel overall?</p>
    <uc1:ScaleQuestionControl runat="server" ID="Q1Overall" SessionKey="Q1Overall" DBColumn="Q1Overall" />

    <p class="question">How satisfied were you with the <u>staff</u> and <u>the level of customer service</u> provided during your last visit to River Rock Casino Resort? Please rate on the following scale:</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Excellent</div>
        <div class="col-md-1 col-xs-2 title">Very Good</div>
        <div class="col-md-1 col-xs-2 title">Good</div>
        <div class="col-md-1 col-xs-2 title">Fair</div>
        <div class="col-md-1 col-xs-2 title">Poor</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q1A" SessionKey="Q1A" DBColumn="Q1A" Label="Ensuring all of your needs were met" />
    <uc1:QuestionRowControl runat="server" ID="Q1B" SessionKey="Q1B" DBColumn="Q1B" Label="Making you feel welcome" />
    <uc1:QuestionRowControl runat="server" ID="Q1C" SessionKey="Q1C" DBColumn="Q1C" Label="Going above & beyond normal service" />
    <uc1:QuestionRowControl runat="server" ID="Q1D" SessionKey="Q1D" DBColumn="Q1D" Label="Speed of service" />
    <uc1:QuestionRowControl runat="server" ID="Q1E" SessionKey="Q1E" DBColumn="Q1E" Label="Encouraging you to visit again" />
    <uc1:QuestionRowControl runat="server" ID="Q1F" SessionKey="Q1F" DBColumn="Q1F" Label="Overall staff availability" />

    <p class="question">Overall, how satisfied were you with the <u>service provided by the staff</u> at River Rock Casino Resort?</p>
    <div class="row grid-header">
        <div class="col-md-2 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-2 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-2 col-xs-2 title">Satisfied</div>
        <div class="col-md-2 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-2 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q2" SessionKey="Q2" DBColumn="Q2" Label="" />
    
    <p class="question">Please rate your satisfaction with our <u>Reservation, Check-In & Check-Out</u> services using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q3A" SessionKey="Q3A" DBColumn="Q3A" Label="Friendliness of Reservation Agent" />
    <uc1:QuestionRowControl runat="server" ID="Q3B" SessionKey="Q3B" DBColumn="Q3B" Label="Helpfulness of Reservation Agent" />
    <uc1:QuestionRowControl runat="server" ID="Q3C" SessionKey="Q3C" DBColumn="Q3C" Label="Accuracy of reservation information upon check-in" />
    <uc1:QuestionRowControl runat="server" ID="Q3D" SessionKey="Q3D" DBColumn="Q3D" Label="Employee knowledge of the River Rock Casino Resort & Facilities" />
    <uc1:QuestionRowControl runat="server" ID="Q3E" SessionKey="Q3E" DBColumn="Q3E" Label="Efficiency of check-in" />
    <uc1:QuestionRowControl runat="server" ID="Q3F" SessionKey="Q3F" DBColumn="Q3F" Label="Friendliness of Front Desk staff" />
    <uc1:QuestionRowControl runat="server" ID="Q3G" SessionKey="Q3G" DBColumn="Q3G" Label="Helpfulness of Front Desk staff" />
    <uc1:QuestionRowControl runat="server" ID="Q3H" SessionKey="Q3H" DBColumn="Q3H" Label="Employees' 'can-do' attitude" />
    <uc1:QuestionRowControl runat="server" ID="Q3I" SessionKey="Q3I" DBColumn="Q3I" Label="Efficiency of check-out" />
    <uc1:QuestionRowControl runat="server" ID="Q3J" SessionKey="Q3J" DBColumn="Q3J" Label="Accuracy of bill at check-out" />
    
    <p class="question">Please rate your satisfaction with our <u>Housekeeping</u> services using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q4A" SessionKey="Q4A" DBColumn="Q4A" Label="Friendliness of Housekeeping staff" />
    <uc1:QuestionRowControl runat="server" ID="Q4B" SessionKey="Q4B" DBColumn="Q4B" Label="Room cleanliness" />
    <uc1:QuestionRowControl runat="server" ID="Q4C" SessionKey="Q4C" DBColumn="Q4C" Label="Bathroom cleanliness" />

    <p class="question">Please rate your satisfaction with your <u>Hotel Room</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q5A" SessionKey="Q5A" DBColumn="Q5A" Label="Towels & Linens" />
    <uc1:QuestionRowControl runat="server" ID="Q5B" SessionKey="Q5B" DBColumn="Q5B" Label="Proper functioning of lights, TV, etc." />
    <uc1:QuestionRowControl runat="server" ID="Q5C" SessionKey="Q5C" DBColumn="Q5C" Label="Overall condition of the room" />
    <uc1:QuestionRowControl runat="server" ID="Q5D" SessionKey="Q5D" DBColumn="Q5D" Label="Adequate amenities (Soap, Shampoo, Conditioner, Hair Dryer, etc.)" />

    <p class="question">
        Please specify if there are any amenities you would like to see in your hotel room:
    </p>
    <sc:SurveyTextBox runat="server" ID="txtQ5Amenities" SessionKey="Q5Amenities" DBColumn="Q5Amenities" Rows="4" style="width:50%" MaxLength="1000" TextMode="MultiLine"></sc:SurveyTextBox>

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 4 - Dining & Facilities
        //===========================================================================
        else if ( Master.CurrentPage == 4 ) { %>
    <p class="header-label">
        Please select "yes" or "no" to the following questions.
    </p>
    <h2>Dining Experience</h2>
    <p class="question">Did you visit Tramonto Restaurant during your stay?</p>
    <uc1:YesNoControl runat="server" ID="Q6Tramonto" SessionKey="Q6Tramonto" DBColumn="Q6Tramonto" />
    <p class="question">Did you visit The Buffet during your stay?</p>
    <uc1:YesNoControl runat="server" ID="Q6TheBuffet" SessionKey="Q6TheBuffet" DBColumn="Q6TheBuffet" />
    <p class="question">Did you visit Curve during your stay?</p>
    <uc1:YesNoControl runat="server" ID="Q6Curve" SessionKey="Q6Curve" DBColumn="Q6Curve" />
    <p class="question">Did you use In-Room Dining during your stay?</p>
    <uc1:YesNoControl runat="server" ID="Q6InRoomDining" SessionKey="Q6InRoomDining" DBColumn="Q6InRoomDining" />

    <h2>Facilities &amp; Amenities</h2>
    <p class="question">Did you visit the Fitness Center during your stay?</p>
    <uc1:YesNoControl runat="server" ID="Q6FitnessCenter" SessionKey="Q6FitnessCenter" DBColumn="Q6FitnessCenter" />
    <p class="question">Did you use the Pool and/or Hot Tub?</p>
    <uc1:YesNoControl runat="server" ID="Q6PoolHotTub" SessionKey="Q6PoolHotTub" DBColumn="Q6PoolHotTub" />
    <p class="question">Did you attend a meeting or event during your stay?</p>
    <uc1:YesNoControl runat="server" ID="Q6Meeting" SessionKey="Q6Meeting" DBColumn="Q6Meeting" />
    <p class="question">Did you use our Valet Parking during your stay?</p>
    <uc1:YesNoControl runat="server" ID="Q6ValetParking" SessionKey="Q6ValetParking" DBColumn="Q6ValetParking" />
    <p class="question">Did you use our Concierge during your stay?</p>
    <uc1:YesNoControl runat="server" ID="Q6Concierge" SessionKey="Q6Concierge" DBColumn="Q6Concierge" />
    <p class="question">Did you use our Bell/Door Service during your stay?</p>
    <uc1:YesNoControl runat="server" ID="Q6BellDoorService" SessionKey="Q6BellDoorService" DBColumn="Q6BellDoorService" />

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 5 - Q7-Q16
        //===========================================================================
        else if ( Master.CurrentPage == 5 ) { %>
    <% if ( Q6Tramonto.SelectedValue == 1 ) { %>
    <p class="question">Please rate your satisfaction with <u>Tramonto Restaurant</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q7A" SessionKey="Q7A" DBColumn="Q7A" Label="Greeting upon arrival" />
    <uc1:QuestionRowControl runat="server" ID="Q7B" SessionKey="Q7B" DBColumn="Q7B" Label="Timeliness of seating " />
    <uc1:QuestionRowControl runat="server" ID="Q7C" SessionKey="Q7C" DBColumn="Q7C" Label="Attentiveness of server " />
    <uc1:QuestionRowControl runat="server" ID="Q7D" SessionKey="Q7D" DBColumn="Q7D" Label="Server’s knowledge of menu selections " />
    <uc1:QuestionRowControl runat="server" ID="Q7E" SessionKey="Q7E" DBColumn="Q7E" Label="Timeliness of meal delivery" />
    <uc1:QuestionRowControl runat="server" ID="Q7F" SessionKey="Q7F" DBColumn="Q7F" Label="Quality and taste of food " />
    <uc1:QuestionRowControl runat="server" ID="Q7G" SessionKey="Q7G" DBColumn="Q7G" Label="Presentation of food " />
    <uc1:QuestionRowControl runat="server" ID="Q7H" SessionKey="Q7H" DBColumn="Q7H" Label="Quality of beverage" />
    <uc1:QuestionRowControl runat="server" ID="Q7I" SessionKey="Q7I" DBColumn="Q7I" Label="Accuracy of bill" />
    <% }
    
       if ( Q6TheBuffet.SelectedValue == 1 ) { %>
    <p class="question">Please rate your satisfaction with <u>The Buffet</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q8A" SessionKey="Q8A" DBColumn="Q8A" Label="Greeting upon arrival" />
    <uc1:QuestionRowControl runat="server" ID="Q8B" SessionKey="Q8B" DBColumn="Q8B" Label="Attentiveness of server " />
    <uc1:QuestionRowControl runat="server" ID="Q8C" SessionKey="Q8C" DBColumn="Q8C" Label="Server’s knowledge of menu selections " />
    <uc1:QuestionRowControl runat="server" ID="Q8D" SessionKey="Q8D" DBColumn="Q8D" Label="Quality and taste of food " />
    <uc1:QuestionRowControl runat="server" ID="Q8E" SessionKey="Q8E" DBColumn="Q8E" Label="Quality of beverage" />
    <uc1:QuestionRowControl runat="server" ID="Q8F" SessionKey="Q8F" DBColumn="Q8F" Label="Accuracy of bill" />
    <% }
    
       if ( Q6Curve.SelectedValue == 1 ) { %>
    <p class="question">Please rate your satisfaction with <u>Curve</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q9A" SessionKey="Q9A" DBColumn="Q9A" Label="Greeting upon arrival" />
    <uc1:QuestionRowControl runat="server" ID="Q9B" SessionKey="Q9B" DBColumn="Q9B" Label="Timeliness of seating " />
    <uc1:QuestionRowControl runat="server" ID="Q9C" SessionKey="Q9C" DBColumn="Q9C" Label="Attentiveness of server " />
    <uc1:QuestionRowControl runat="server" ID="Q9D" SessionKey="Q9D" DBColumn="Q9D" Label="Server’s knowledge of menu selections " />
    <uc1:QuestionRowControl runat="server" ID="Q9E" SessionKey="Q9E" DBColumn="Q9E" Label="Timeliness of meal delivery" />
    <uc1:QuestionRowControl runat="server" ID="Q9F" SessionKey="Q9F" DBColumn="Q9F" Label="Quality and taste of food " />
    <uc1:QuestionRowControl runat="server" ID="Q9G" SessionKey="Q9G" DBColumn="Q9G" Label="Presentation of food " />
    <uc1:QuestionRowControl runat="server" ID="Q9H" SessionKey="Q9H" DBColumn="Q9H" Label="Quality of beverage" />
    <uc1:QuestionRowControl runat="server" ID="Q9I" SessionKey="Q9I" DBColumn="Q9I" Label="Accuracy of bill" />
    <% }
    
       if ( Q6InRoomDining.SelectedValue == 1 ) { %>
    <p class="question">Please rate your satisfaction with <u>In-Room Dining</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q10A" SessionKey="Q10A" DBColumn="Q10A" Label="Phone answered promptly" />
    <uc1:QuestionRowControl runat="server" ID="Q10B" SessionKey="Q10B" DBColumn="Q10B" Label="Friendliness of order taker" />
    <uc1:QuestionRowControl runat="server" ID="Q10C" SessionKey="Q10C" DBColumn="Q10C" Label="Friendliness of server" />
    <uc1:QuestionRowControl runat="server" ID="Q10D" SessionKey="Q10D" DBColumn="Q10D" Label="Order delivered within time period advised" />
    <uc1:QuestionRowControl runat="server" ID="Q10E" SessionKey="Q10E" DBColumn="Q10E" Label="Accuracy of order" />
    <uc1:QuestionRowControl runat="server" ID="Q10F" SessionKey="Q10F" DBColumn="Q10F" Label="Presentation of food" />
    <uc1:QuestionRowControl runat="server" ID="Q10G" SessionKey="Q10G" DBColumn="Q10G" Label="Quality of in-room dining food" />
    <uc1:QuestionRowControl runat="server" ID="Q10H" SessionKey="Q10H" DBColumn="Q10H" Label="Delivery staff offered pick-up of empty tray" />
    <% }
    
       if ( Q6FitnessCenter.SelectedValue == 1 ) { %>
    <p class="question">Please rate your satisfaction with the <u>Fitness Centre</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q11A" SessionKey="Q11A" DBColumn="Q11A" Label="Cleanliness of Fitness Center" />
    <uc1:QuestionRowControl runat="server" ID="Q11B" SessionKey="Q11B" DBColumn="Q11B" Label="Quality/ condition of fitness equipment" />
    <uc1:QuestionRowControl runat="server" ID="Q11C" SessionKey="Q11C" DBColumn="Q11C" Label="Availability of Fitness Center equipment" />
    <uc1:QuestionRowControl runat="server" ID="Q11D" SessionKey="Q11D" DBColumn="Q11D" Label="Variety of equipment" />
    <% }
    
       if ( Q6PoolHotTub.SelectedValue == 1 ) { %>
    <p class="question">Please rate your satisfaction with the <u>Pool and/or Hot Tub</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q12A" SessionKey="Q12A" DBColumn="Q12A" Label="Cleanliness of pool area" />
    <uc1:QuestionRowControl runat="server" ID="Q12B" SessionKey="Q12B" DBColumn="Q12B" Label="Temperature of pool" />
    <uc1:QuestionRowControl runat="server" ID="Q12C" SessionKey="Q12C" DBColumn="Q12C" Label="Cleanliness of hot tub area" />
    <uc1:QuestionRowControl runat="server" ID="Q12D" SessionKey="Q12D" DBColumn="Q12D" Label="Temperature of hot tub" />
    <uc1:QuestionRowControl runat="server" ID="Q12E" SessionKey="Q12E" DBColumn="Q12E" Label="Cleanliness of changing rooms" />
    <% }
    
       if ( Q6Meeting.SelectedValue == 1 ) { %>
    <p class="question">What meeting or event were you attending at River Rock Casino Resort?</p>
    <sc:SurveyTextBox runat="server" ID="txtQ13_MeetingDescription" SessionKey="Q13_MeetingDescription" DBColumn="Q13_MeetingDescription" MaxLength="150" Size="50"></sc:SurveyTextBox>

    <p class="question">Please rate your satisfaction with our <u>Meeting and Event services</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q13A" SessionKey="Q13A" DBColumn="Q13A" Label="Condition and cleanliness of meeting/event room" />
    <uc1:QuestionRowControl runat="server" ID="Q13B" SessionKey="Q13B" DBColumn="Q13B" Label="Proper meeting/event room temperature" />
    <uc1:QuestionRowControl runat="server" ID="Q13C" SessionKey="Q13C" DBColumn="Q13C" Label="Quality of meeting/event food and beverage" />
    <uc1:QuestionRowControl runat="server" ID="Q13D" SessionKey="Q13D" DBColumn="Q13D" Label="Friendliness and efficiency of meeting/event staff" />
    <uc1:QuestionRowControl runat="server" ID="Q13E" SessionKey="Q13E" DBColumn="Q13E" Label="Quality/condition/support of technical equipment" />
    <uc1:QuestionRowControl runat="server" ID="Q13F" SessionKey="Q13F" DBColumn="Q13F" Label="Meeting/event facilities (size, design, amenities)" />
    <uc1:QuestionRowControl runat="server" ID="Q13G" SessionKey="Q13G" DBColumn="Q13G" Label="Accuracy of meeting/ event signage" />
    <% }
    
       if ( Q6ValetParking.SelectedValue == 1 ) { %>
    <p class="question">Please rate your satisfaction with the <u>Valet Parking services</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q14A" SessionKey="Q14A" DBColumn="Q14A" Label="Greeting upon arrival" />
    <uc1:QuestionRowControl runat="server" ID="Q14B" SessionKey="Q14B" DBColumn="Q14B" Label="Car returned in timely manner" />
    <uc1:QuestionRowControl runat="server" ID="Q14C" SessionKey="Q14C" DBColumn="Q14C" Label="Original mirror position" />
    <uc1:QuestionRowControl runat="server" ID="Q14D" SessionKey="Q14D" DBColumn="Q14D" Label="Original radio station " />
    <uc1:QuestionRowControl runat="server" ID="Q14E" SessionKey="Q14E" DBColumn="Q14E" Label="Original seat position" />
    <uc1:QuestionRowControl runat="server" ID="Q14F" SessionKey="Q14F" DBColumn="Q14F" Label="Valet driver drove care in respectful manner" />
    <uc1:QuestionRowControl runat="server" ID="Q14G" SessionKey="Q14G" DBColumn="Q14G" Label="Pleasant departure greeting" />
    <% }
    
       if ( Q6Concierge.SelectedValue == 1 ) { %>
    <p class="question">Please rate your satisfaction with our <u>Concierge services</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q15A" SessionKey="Q15A" DBColumn="Q15A" Label="Availability of Concierge" />
    <uc1:QuestionRowControl runat="server" ID="Q15B" SessionKey="Q15B" DBColumn="Q15B" Label="Friendliness of Concierge" />
    <uc1:QuestionRowControl runat="server" ID="Q15C" SessionKey="Q15C" DBColumn="Q15C" Label="Employee knowledge of the River Rock Casino Resort & Facilities" />
    <uc1:QuestionRowControl runat="server" ID="Q15D" SessionKey="Q15D" DBColumn="Q15D" Label="Staff member went out of way to provide excellent service" />
    <uc1:QuestionRowControl runat="server" ID="Q15E" SessionKey="Q15E" DBColumn="Q15E" Label="Pleasant departure greeting" />
    <% }
    
       if ( Q6BellDoorService.SelectedValue == 1 ) { %>
    <p class="question">Please rate your satisfaction with our <u>Bell/Door Service</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q16A" SessionKey="Q16A" DBColumn="Q16A" Label="Greeting upon arrival" />
    <uc1:QuestionRowControl runat="server" ID="Q16B" SessionKey="Q16B" DBColumn="Q16B" Label="Acknowledgement throughout stay" />
    <uc1:QuestionRowControl runat="server" ID="Q16C" SessionKey="Q16C" DBColumn="Q16C" Label="Friendliness of bell/ door staff" />
    <uc1:QuestionRowControl runat="server" ID="Q16D" SessionKey="Q16D" DBColumn="Q16D" Label="Employee knowledge of the River Rock Casino Resort & Facilities" />
    <uc1:QuestionRowControl runat="server" ID="Q16E" SessionKey="Q16E" DBColumn="Q16E" Label="Staff member went out of way to provide excellent service" />
    <uc1:QuestionRowControl runat="server" ID="Q16F" SessionKey="Q16F" DBColumn="Q16F" Label="Pleasant departure greeting" />
    <% } %>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 6 - Q17-23
        //===========================================================================
        else if ( Master.CurrentPage == 6 ) { %>
    <p class="question">Please rate your satisfaction with <u>how we made you feel</u> during your stay using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q17A" SessionKey="Q17A" DBColumn="Q17A" Label="Welcome" />
    <uc1:QuestionRowControl runat="server" ID="Q17B" SessionKey="Q17B" DBColumn="Q17B" Label="Comfortable" />
    <uc1:QuestionRowControl runat="server" ID="Q17C" SessionKey="Q17C" DBColumn="Q17C" Label="Important" />

    <p class="question">Please rate your satisfaction with <u>your overall stay at River Rock Casino Resort</u> using the scale below.</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Extremely satisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Satisfied</div>
        <div class="col-md-1 col-xs-2 title">Dissatisfied</div>
        <div class="col-md-1 col-xs-2 title">Very Dissatisfied</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q18A" SessionKey="Q18A" DBColumn="Q18A" Label="Overall condition of the River Rock Casino Resort" />
    <uc1:QuestionRowControl runat="server" ID="Q18B" SessionKey="Q18B" DBColumn="Q18B" Label="Value for price" />

    <p class="question">If you return to this area, how likely is it that you will return to this resort?</p>
    <div class="row grid-header">
        <div class="col-md-2 col-xs-2 title">Very likely</div>
        <div class="col-md-2 col-xs-2 title">Possibly</div>
        <div class="col-md-2 col-xs-2 title">Not likely</div>
    </div>
    <uc1:TriQuestionRowControl runat="server" ID="Q19" SessionKey="Q19" DBColumn="Q19" />

    <p class="question">How likely is it that you will recommend this resort to others?</p>
    <div class="row grid-header">
        <div class="col-md-2 col-xs-2 title">Very likely</div>
        <div class="col-md-2 col-xs-2 title">Possibly</div>
        <div class="col-md-2 col-xs-2 title">Not likely</div>
    </div>
    <uc1:TriQuestionRowControl runat="server" ID="Q20" SessionKey="Q20" DBColumn="Q20" />

    <p class="question">During your stay, did the staff provide exceptional service which exceeded your expectations?</p>
    <uc1:YesNoControl runat="server" ID="Q21" SessionKey="Q21" DBColumn="Q21" />
    <p>If yes, please provide the name & department of the staff member who provided exceptional service:</p>
    <sc:SurveyTextBox ID="txtQ21Explanation" runat="server" SessionKey="Q21Explanation" DBColumn="Q21Explanation" MaxLength="200" Size="50"></sc:SurveyTextBox>

    <p class="question">When selecting a hotel, how important are eco-friendly or "green" initiatives?</p>
    <div class="row grid-header">
        <div class="col-md-2 col-xs-2 title">Very important</div>
        <div class="col-md-2 col-xs-2 title">Somewhat important</div>
        <div class="col-md-2 col-xs-2 title">Not important</div>
    </div>
    <uc1:TriQuestionRowControl runat="server" ID="Q22" SessionKey="Q22" DBColumn="Q22" />

    <p class="question">Did you experience a problem during your stay with us?</p>
    <uc1:YesNoControl runat="server" ID="Q23" SessionKey="Q23" DBColumn="Q23" />
    
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 7 - Q24 - Issue Details
        //===========================================================================
        else if ( Master.CurrentPage == 7 ) { %>
    <p class="question">Please categorize the nature of your problem during your stay:</p>
    <sc:MessageManager runat="server" ID="mmQ24A"></sc:MessageManager>
    <sc:SurveyCheckBox ID="chkQ24A_1" runat="server" SessionKey="Q24A_1" DBColumn="Q24A_Arrival" DBValue="1" Text="&nbsp;Arrival" /><br />
    <sc:SurveyCheckBox ID="chkQ24A_2" runat="server" SessionKey="Q24A_2" DBColumn="Q24A_Staff" DBValue="1" Text="&nbsp;Staff" /><br />
    <sc:SurveyCheckBox ID="chkQ24A_3" runat="server" SessionKey="Q24A_3" DBColumn="Q24A_GuestRoom" DBValue="1" Text="&nbsp;Guest Room" /><br />
    <sc:SurveyCheckBox ID="chkQ24A_4" runat="server" SessionKey="Q24A_4" DBColumn="Q24A_FoodBeverage" DBValue="1" Text="&nbsp;Food & Beverage" /><br />
    <sc:SurveyCheckBox ID="chkQ24A_5" runat="server" SessionKey="Q24A_5" DBColumn="Q24A_FacilitiesService" DBValue="1" Text="&nbsp;Facilities & Service" /><br />
    <sc:SurveyCheckBox ID="chkQ24A_6" runat="server" SessionKey="Q24A_6" DBColumn="Q24A_BillingDeparture" DBValue="1" Text="&nbsp;Billing/Departure" /><br />
    <sc:SurveyCheckBox ID="chkQ24A_7" runat="server" SessionKey="Q24A_7" DBColumn="Q24A_MeetingsEvents" DBValue="1" Text="&nbsp;Meetings & Events" /><br />
    <sc:SurveyCheckBox ID="chkQ24A_8" runat="server" SessionKey="Q24A_8" DBColumn="Q24A_Other" DBValue="1" Text="&nbsp;Other" /> <sc:SurveyTextBox ID="txtQ24A_OtherExplanation" runat="server" SessionKey="Q24A_OtherExplanation" DBColumn="Q24A_OtherExplanation" MaxLength="150" Size="50"></sc:SurveyTextBox><br />
    
    <p class="question">Please briefly describe your problem:</p>
    <sc:SurveyTextBox ID="Q24B" runat="server" SessionKey="Q24B" DBColumn="Q24B" MaxLength="1000" Rows="5" TextMode="MultiLine" style="width:50%"></sc:SurveyTextBox>

    <p class="question">Did you report the problem?</p>
    <uc1:YesNoControl runat="server" ID="Q24C" SessionKey="Q24C" DBColumn="Q24C" />
    
    <p class="question">Thinking of this problem, what is your satisfaction level with River Rock Hotel's ability to fix your problem or issue?</p>
    <uc1:ScaleQuestionControl runat="server" ID="Q24D" SessionKey="24D" DBColumn="Q24D" />

    <p class="question">More specifically, how would you rate your satisfaction level with River Rock Hotel's response to your problem in terms of...?</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Excellent</div>
        <div class="col-md-1 col-xs-2 title">Very Good</div>
        <div class="col-md-1 col-xs-2 title">Good</div>
        <div class="col-md-1 col-xs-2 title">Fair</div>
        <div class="col-md-1 col-xs-2 title">Poor</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q24E_1" SessionKey="Q24E_1" DBColumn="Q24E_1" Label="The length of time taken to resolve your problem" />
    <uc1:QuestionRowControl runat="server" ID="Q24E_2" SessionKey="Q24E_2" DBColumn="Q24E_2" Label="The effort of employees in resolving your problem" />
    <uc1:QuestionRowControl runat="server" ID="Q24E_3" SessionKey="Q24E_3" DBColumn="Q24E_3" Label="The courteousness of employees while resolving your problem" />
    <uc1:QuestionRowControl runat="server" ID="Q24E_4" SessionKey="Q24E_4" DBColumn="Q24E_4" Label="The amount of communication with you from employees while resolving your problem" />
    <uc1:QuestionRowControl runat="server" ID="Q24E_5" SessionKey="Q24E_5" DBColumn="Q24E_5" Label="The fairness of the outcome in resolving your problem" />

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 8 - Q25-28 - Last questions
        //===========================================================================
        else if ( Master.CurrentPage == 8 ) { %>
    <p class="question">Please select your primary reason for choosing River Rock Casino Resort.</p>
    <sc:MessageManager runat="server" ID="mmQ25"></sc:MessageManager>
    <sc:SurveyRadioButton ID="Q25_1" runat="server" GroupName="Q25" SessionKey="Q25_1" DBColumn="Q25" DBValue="Special package/rate" Text="&nbsp;Special package/rate" /><br />
    <sc:SurveyRadioButton ID="Q25_2" runat="server" GroupName="Q25" SessionKey="Q25_2" DBColumn="Q25" DBValue="Facilities/Amenities" Text="&nbsp;Facilities/Amenities" /><br />
    <sc:SurveyRadioButton ID="Q25_3" runat="server" GroupName="Q25" SessionKey="Q25_3" DBColumn="Q25" DBValue="Website" Text="&nbsp;Website" /><br />
    <sc:SurveyRadioButton ID="Q25_4" runat="server" GroupName="Q25" SessionKey="Q25_4" DBColumn="Q25" DBValue="Business meeting/Conference venue" Text="&nbsp;Business meeting/Conference venue" /><br />
    <sc:SurveyRadioButton ID="Q25_5" runat="server" GroupName="Q25" SessionKey="Q25_5" DBColumn="Q25" DBValue="Articles/Advertisements" Text="&nbsp;Articles/Advertisements" /><br />
    <sc:SurveyRadioButton ID="Q25_6" runat="server" GroupName="Q25" SessionKey="Q25_6" DBColumn="Q25" DBValue="Previous visit" Text="&nbsp;Previous visit" /><br />
    <sc:SurveyRadioButton ID="Q25_7" runat="server" GroupName="Q25" SessionKey="Q25_7" DBColumn="Q25" DBValue="Personal recommendation" Text="&nbsp;Personal recommendation" /><br />
    <sc:SurveyRadioButton ID="Q25_8" runat="server" GroupName="Q25" SessionKey="Q25_8" DBColumn="Q25" DBValue="Location" Text="&nbsp;Location" /><br />
    <sc:SurveyRadioButton ID="Q25_9" runat="server" GroupName="Q25" SessionKey="Q25_9" DBColumn="Q25" DBValue="Travel Agent" Text="&nbsp;Travel Agent" /><br />
    <sc:SurveyRadioButton ID="Q25_10" runat="server" GroupName="Q25" SessionKey="Q25_10" DBColumn="Q25" DBValue="Other, please specify" Text="&nbsp;Other, please specify" /> <sc:SurveyTextBox ID="Q25_OtherExplanation" runat="server" SessionKey="Q25_OtherExplanation" DBColumn="Q25_OtherExplanation" MaxLength="150" Size="50"></sc:SurveyTextBox><br />


    <p class="question">What was the primary purpose of your visit?</p>
    <sc:MessageManager runat="server" ID="mmQ26"></sc:MessageManager>
    <sc:SurveyCheckBox ID="Q26_1" runat="server" SessionKey="Q26_1" DBColumn="Q26Business" DBValue="1" Text="&nbsp;Business" /><br />
    <sc:SurveyCheckBox ID="Q26_2" runat="server" SessionKey="Q26_2" DBColumn="Q26Pleasure" DBValue="1" Text="&nbsp;Pleasure" /><br />
    <sc:SurveyCheckBox ID="Q26_3" runat="server" SessionKey="Q26_3" DBColumn="Q26MeetingEvent" DBValue="1" Text="&nbsp;Meeting / Event" /><br />
    <sc:SurveyCheckBox ID="Q26_4" runat="server" SessionKey="Q26_4" DBColumn="Q26Other" DBValue="1" Text="&nbsp;Other, please specify" /> <sc:SurveyTextBox ID="Q26_OtherExplanation" runat="server" SessionKey="Q26_OtherExplanation" DBColumn="Q26_OtherExplanation" MaxLength="150" Size="50"></sc:SurveyTextBox><br />

    <p class="question">Have you visited River Rock Casino Resort before?</p>
    <uc1:YesNoControl runat="server" ID="Q27" SessionKey="Q27" DBColumn="Q27" />

    <p class="question">Do you have any other comments or suggestions for the management and staff of River Rock Casino Resort?</p>
    <sc:SurveyTextBox ID="Q28" runat="server" SessionKey="Q28" DBColumn="Q28" MaxLength="1000" Rows="5" TextMode="MultiLine" style="width:50%"></sc:SurveyTextBox>

    <p class="question">Would you like casino staff to contact you about your feedback? Only respond “Yes” if you want to be further contacted by staff <u>about your feedback.</u></p>
    <uc1:YesNoControl runat="server" ID="Q29" SessionKey="Q29" DBColumn="Q29" />
    
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 9 - Contact Info
        //===========================================================================
        else if ( Master.CurrentPage == 9 ) { %>
    <% if ( Q29.SelectedValue == 1 ) { %>
    <p class="question">
        Please confirm your contact information if you wish to be contacted. First and last name are mandatory. <u>You will be contacted using the email address provided at the start of this survey.</u> If you would prefer to be contacted via Telephone, please provide it below.
    </p>
    <div class="row grid-row">
        <div class="col-md-4">First Name</div>
        <div class="col-md-6"><sc:SurveyTextBox ID="txtFirstName" SessionKey="txtFirstName" DBColumn="FirstName" runat="server" MaxLength="100"></sc:SurveyTextBox></div>
    </div>
    <div class="row grid-row">
        <div class="col-md-4">Last Name</div>
        <div class="col-md-6"><sc:SurveyTextBox ID="txtLastName" SessionKey="txtLastName" DBColumn="LastName" runat="server" MaxLength="100"></sc:SurveyTextBox></div>
    </div>
    <div class="row grid-row">
        <div class="col-md-4">Email address</div>
        <div class="col-md-6"><sc:SurveyTextBox ID="txtEmail2" runat="server" SessionKey="ContactEmail" DBColumn="ContactEmail" MaxLength="150" Size="50"></sc:SurveyTextBox></div>
    </div>
    <div class="row grid-row">
        <div class="col-md-4">Tel # (numbers only please. Example: 555 234 5678):</div>
        <div class="col-md-6"><sc:SurveyTextBox ID="txtTelephoneNumber" SessionKey="txtTelephoneNumber" DBColumn="TelephoneNumber" runat="server" MaxLength="30"></sc:SurveyTextBox></div>
    </div>
    <p class="question">Please briefly describe the reason you're requesting a follow up.</p>
    <sc:SurveyTextBox ID="txtFollowupReason" runat="server" SessionKey="FollowupReason" DBColumn="FollowupReason" MaxLength="1000" Rows="5" TextMode="MultiLine" style="width:50%"></sc:SurveyTextBox>
    <% } %>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 10 - Final Confirmation
        //===========================================================================
        else if ( Master.CurrentPage == 10 ) { %>
    <p>Your feedback is important to us and we look forward to receiving your comments and will endeavor to answer all questions within 12-24 hours. If your question is more urgent, please contact Guest Services at 604.273.1895.</p>
    <p>By clicking "Done" below, you acknowledge that you are aware that your personal information (contact information and feedback) is being collected by Forum Research and used by Great Canadian Casino Corporation (GCGC) in accordance with applicable Freedom of Information and Protection of Privacy laws. It will only be used for GCGC's reporting &amp; research purposes and to administer the guest feedback process. </p>
    <p>Your information will NOT be sold, shared with third parties, or used for soliciting purposes.</p>
    <p>The information you provide will be stored by our survey partner on a server located outside of Canada. The data collected by GCGC is private, confidential, and protected by Secure Sockets Layer (SSL). The servers are also physically and electronically protected by a number of security measures and no customer data can be accessed without written authorization from GCGC. If you have any questions about this, please write to GCGC's Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
    
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Done" />
    </div>
    <% }
        //===========================================================================
        //PAGE 97 - Thank You
        //===========================================================================
        else if ( Master.CurrentPage == 97 ) { %>
    <sc:MessageManager runat="server" ID="mmLastPage"></sc:MessageManager>
    <% if (!String.IsNullOrEmpty( mmLastPage.SuccessMessage)) { %>
    <div class="button-container">
        <a href="<%= PropertyTools.GetCasinoURL( Master.PropertyShortCode ) %>" class="btn btn-success">Return to Website</a>
    </div>
    <% } %>
    <% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="server">
</asp:Content>
