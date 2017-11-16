<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyGSEI.aspx.cs" Inherits="GCC_Web_Portal.SurveyGSEI" MasterPageFile="~/Survey.Master" %>
<%@ MasterType VirtualPath="~/Survey.Master" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/QuestionRowControl.ascx" TagPrefix="uc1" TagName="QuestionRowControl" %>
<%@ Register Src="~/Controls/ScaleQuestionControl.ascx" TagPrefix="uc1" TagName="ScaleQuestionControl" %>
<%@ Register Src="~/Controls/YesNoControl.ascx" TagPrefix="uc1" TagName="YesNoControl" %>
<asp:Content runat="server" ContentPlaceHolderID="headContent">
    <style>
        input[type=text],
        input[type=number] {
            margin-top:5px;
            margin-bottom:5px;
        }
        .icheckbox_flat-blue,.iradio_flat-blue {
            margin-right:5px;
        }
        label.disabled {
            color:#c0c0c0;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="mainContent">
    <h1>How's our Customer Service?</h1>
    <%
        //===========================================================================
        //PAGE 1 - Email
        //===========================================================================
    if ( Master.CurrentPage == 1) {%>
    <h2>Tell us and you could win up to $1,000!</h2>
    <p>Thank you for your interest in completing this quick, 2-minute survey.</p>
    <p>Your enjoyment is important to us, and we would very much appreciate if you would share your thoughts and experiences with us by completing this survey. To show our appreciation for your time, you will be entered into a draw to WIN one of TEN $100 Prizes, or a grand prize of $1,000!</p>
    <sc:MessageManager runat="server" TitleOverride="Please read:" DisplayAs="Info">
        <Message>
            To enter and be eligible to win, an entrant must complete the survey by January 22<sup>nd</sup>, 2017.
        </Message>
    </sc:MessageManager>

    <p>In order to proceed with the survey, we require you to confirm below that we have sent this to the correct email address. Click "Next" to continue.</p>
    <p>All of our surveys are conducted confidentially. If you need us to contact you about your experience, please visit us at www.gcgaming.com/contact.</p>

    <h2>Key Terms & Conditions</h2>
    <% if ( SurveyType != GSEISurveyType.GA ) { %>
    <p>No purchase necessary. An Entrant may only enter the contest once during the promotional period. Duplicate entries will be deleted. Your personal information is collected and used by Great Canadian Gaming Corporation (GCGC) in accordance with British Columbia's Freedom of Information and Protection of Privacy Act. It will be used for GCGC's research purposes and to administer this contest. Your information will not be sold, shared with third parties, or used for soliciting purposes. If you have any questions about this, please write to GCGC's Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
    <% } else { %>
    <p>No purchase necessary. An Entrant may only enter the contest once during the promotional period. Duplicate entries will be deleted. Your personal information is collected and used by Great American Gaming (GAG) in accordance with Freedom of Information and Protection of Privacy Legislation. It will be used for GAG's research purposes and to administer this contest. Your information will not be sold, shared with third parties, or used for soliciting purposes. If you have any questions about this, please write to GAG’s Privacy Officer at 350 - 13775 Commerce Parkway, Richmond, BC V6V 2V4.</p>
    <% } %>
    <p>For terms of use and full contest conditions, please see below.</p>
    <p><a href="/GSEITermsAndConditions/<%= Master.PropertyShortCode.ToString() %>" title="Terms and Conditions" target="_blank">Terms of Use, Full Contest Conditions and Privacy Policy</a></p>
    
    <p class="question">By clicking on "I agree" and providing your email address, you accept the terms and conditions. The survey should only take 2 minutes to complete. </p>
    <sc:MessageManager runat="server" ID="mmAcceptGroup"></sc:MessageManager>
    <p>
        <sc:SurveyRadioButton ID="radAccept" runat="server" GroupName="acceptgrp" SessionKey="radAccept" CssClass="radalign" Text="I agree and want to proceed with the survey" /><br />
        <sc:SurveyRadioButton ID="radDecline" runat="server" GroupName="acceptgrp" SessionKey="radDecline" CssClass="radalign" Text="I decline to complete the survey. If you choose to decline you will not be able to provide your feedback or participate in the contest." />
    </p>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 99 - Declined Message
        //===========================================================================
        else if ( Master.CurrentPage == 99 ) { %>
    <p>We acknowledge that you have chosen not to participate in the survey and contest. Thank you for your recent visit and we look forward to seeing you again soon!</p>

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Undo" />
        <a href="http://gcgaming.com/" class="btn btn-primary">End Survey</a>
    </div>
    
    <% }
        //===========================================================================
        //PAGE 2 - Qualifier
        //===========================================================================
        else if ( Master.CurrentPage == 2 ) {
            if ( SurveyType == GSEISurveyType.HP ) { %>
    <p class="question">Which of the following Great Canadian properties have you visited most often for horse racing in the last 6 months?</p>
    <sc:SurveyRadioButton runat="server" ID="radLocation_EC" SessionKey="radLocation_EC" GroupName="Location" DBColumn="PropertyID" DBValue="14" Text="Fraser Downs Racetrack & Casino / Elements Casino" /><br />
    <sc:SurveyRadioButton runat="server" ID="radLocation_HA" SessionKey="radLocation_HA" GroupName="Location" DBColumn="PropertyID" DBValue="5" Text="Hasting Racecourse & Casino" /><br />
    <sc:SurveyRadioButton runat="server" ID="radLocation_None" SessionKey="radLocation_None" GroupName="Location" DBColumn="PropertyID" DBValue="0" Text="I have not attended either of these properties in the last 6 months" /><br />
        <% } else if ( SurveyType == GSEISurveyType.TM ) { %>
    <p class="question">Which of the following Great Canadian properties have you visited most often to attend a show in the last 6 months?</p>
    <sc:SurveyRadioButton runat="server" ID="radLocation_RR" SessionKey="radLocation_RR" GroupName="Location" DBColumn="PropertyID" DBValue="2" Text="River Rock Casino Resort" /><br />
    <sc:SurveyRadioButton runat="server" ID="radLocation_HRCV" SessionKey="radLocation_HRCV" GroupName="Location" DBColumn="PropertyID" DBValue="3" Text="Hard Rock Casino Vancouver" /><br />
    <sc:SurveyRadioButton runat="server" ID="radLocation_None2" SessionKey="radLocation_None2" GroupName="Location" DBColumn="PropertyID" DBValue="0" Text="I have not attended either of these properties in the last 6 months" /><br />
        <% } else if ( SurveyType == GSEISurveyType.GA ) { %>
    <p class="question">Which of the following Great American properties have you visited most often in the last 3 months?</p>
    <sc:SurveyRadioButton runat="server" ID="radLocation_Lakewood" SessionKey="radLocation_LA" GroupName="Location" DBColumn="Q3" DBValue="Lakewood" Text="Lakewood" /><br />
    <sc:SurveyRadioButton runat="server" ID="radLocation_Tukwila" SessionKey="radLocation_TU" GroupName="Location" DBColumn="Q3" DBValue="Tukwila" Text="Tukwila" /><br />
    <sc:SurveyRadioButton runat="server" ID="radLocation_Everett" SessionKey="radLocation_EV" GroupName="Location" DBColumn="Q3" DBValue="Everett" Text="Everett" /><br />
    <sc:SurveyRadioButton runat="server" ID="radLocation_DeMoines" SessionKey="radLocation_DE" GroupName="Location" DBColumn="Q3" DBValue="DeMoines" Text="DeMoines" /><br />
    <sc:SurveyRadioButton runat="server" ID="radLocation_None3" SessionKey="radLocation_None3" GroupName="Location" DBColumn="Q3" DBValue="None" Text="I have not attended any of these properties in the last 6 months" /><br />
        <% } %>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Continue" />
    </div>
    <% }
        //===========================================================================
        //PAGE 98 - No Location Message
        //===========================================================================
        else if ( Master.CurrentPage == 98 ) { %>
    <p>You have indicated you have not visited one of these properties in the last 6 months. Since we are looking for feedback from guests who have visited recently, this will end the survey.  Thank you for your feedback!</p>

    <div class="button-container">
        <a href="http://gcgaming.com/" class="btn btn-primary">End Survey</a>
    </div>
    
    <% }
        //===========================================================================
        //PAGE 3 - Staff
        //===========================================================================
        else if ( Master.CurrentPage == 3 ) { %>
    <p class="question">
        How satisfied were you with the <u>staff</u> and <u>the level of customer service</u> provided during your last visit to <%= CasinoName %>?<br />
        Please rate the following:
    </p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Excellent</div>
        <div class="col-md-1 col-xs-2 title">Very Good</div>
        <div class="col-md-1 col-xs-2 title">Good</div>
        <div class="col-md-1 col-xs-2 title">Fair</div>
        <div class="col-md-1 col-xs-2 title">Poor</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="Q7A" SessionKey="Q7A" DBColumn="Q7A" Label="Ensuring all of your needs were met" />
    <uc1:QuestionRowControl runat="server" ID="Q7B" SessionKey="Q7B" DBColumn="Q7B" Label="Making you feel welcome" />
    <uc1:QuestionRowControl runat="server" ID="Q7C" SessionKey="Q7C" DBColumn="Q7C" Label="Going above & beyond normal service" />
    <uc1:QuestionRowControl runat="server" ID="Q7D" SessionKey="Q7D" DBColumn="Q7D" Label="Speed of service" />
    <uc1:QuestionRowControl runat="server" ID="Q7E" SessionKey="Q7E" DBColumn="Q7E" Label="Encouraging you to visit again" />
    <uc1:QuestionRowControl runat="server" ID="Q7F" SessionKey="Q7F" DBColumn="Q7F" Label="Overall staff availability" />
    <p class="question">
        Overall, how satisfied were you with the <u>service provided by the staff</u> at <%= CasinoName %>?
    </p>
    <uc1:ScaleQuestionControl runat="server" ID="Q8" SessionKey="Q8" DBColumn="Q8" />

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Continue" />
    </div>
    <% }
        //===========================================================================
        //PAGE 3 - Demographics
        //===========================================================================
        else if ( Master.CurrentPage == 4 ) { %>

    <p class="question">
        What is your age range?
    </p>
    <sc:MessageManager runat="server" ID="Q37Message"></sc:MessageManager>
    <sc:SurveyRadioButton ID="Q37_1" runat="server" GroupName="Q37" SessionKey="Q37_1" DBColumn="Q37" DBValue="19-24" Text="19-24" /><br />
    <sc:SurveyRadioButton ID="Q37_2" runat="server" GroupName="Q37" SessionKey="Q37_2" DBColumn="Q37" DBValue="25-34" Text="25-34" /><br />
    <sc:SurveyRadioButton ID="Q37_3" runat="server" GroupName="Q37" SessionKey="Q37_3" DBColumn="Q37" DBValue="35-44" Text="35-44" /><br />
    <sc:SurveyRadioButton ID="Q37_4" runat="server" GroupName="Q37" SessionKey="Q37_4" DBColumn="Q37" DBValue="45-54" Text="45-54" /><br />
    <sc:SurveyRadioButton ID="Q37_5" runat="server" GroupName="Q37" SessionKey="Q37_5" DBColumn="Q37" DBValue="55-64" Text="55-64" /><br />
    <sc:SurveyRadioButton ID="Q37_6" runat="server" GroupName="Q37" SessionKey="Q37_6" DBColumn="Q37" DBValue="65 or older" Text="65 or older" />
    
    <p class="question">
        What is your gender?
    </p>
    <sc:MessageManager runat="server" ID="Q36Message"></sc:MessageManager>
    <sc:SurveyRadioButton ID="Q36Male" runat="server" GroupName="Q36" SessionKey="Q36Male" DBColumn="Q36" DBValue="Male" Text="Male" /><br />
    <sc:SurveyRadioButton ID="Q36Female" runat="server" GroupName="Q36" SessionKey="Q36Female" DBColumn="Q36" DBValue="Female" Text="Female" />

    <% if ( SurveyType != GSEISurveyType.GA ) { %>
    <p class="question">
        Which of the following activities was your <u>primary</u> reason for visiting <%= CasinoName %>?<br />
        Please choose one.
    </p>
    <sc:MessageManager runat="server" ID="mmQ1"></sc:MessageManager>
    <div id="q1">
         <asp:Panel runat="server">
            <% if ( new[] { "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "WDB","AJA","GBH" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Slots" runat="server" GroupName="Q1" SessionKey="Q1Slots" DBColumn="Q1" DBValue="Slots" Text="Playing Slots" /><br />
            <% } %>
            <% if ( new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "GAG", "EC", "SCTI", "CNB", "WDB","GBH" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Tables" runat="server" GroupName="Q1" SessionKey="Q1Tables" DBColumn="Q1" DBValue="Tables" Text="Playing Tables" /><br />
            <% } %>
            <% if ( new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "EC", "CNB" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Poker" runat="server" GroupName="Q1" SessionKey="Q1Poker" DBColumn="Q1" DBValue="Poker" Text="Playing Poker" /><br />
            <% } %>
            <sc:SurveyRadioButton ID="radQ1_Food" runat="server" GroupName="Q1" SessionKey="Q1Food" DBColumn="Q1" DBValue="Food" Text="Enjoying Food or Beverages" /><br />
            <% if (new[] { "RR", "HRCV", "VRL", "CCH", "CMR", "CDC", "CNSH", "EC", "SCTI", "WDB", "GBH" }.Contains(Master.PropertyShortCode.ToString()))
               { %>
            <sc:SurveyRadioButton ID="radQ1_Entertainment" runat="server" GroupName="Q1" SessionKey="Q1Entertainment" DBColumn="Q1" DBValue="Entertainment" Text="Watching Live Entertainment at a show lounge or theatre" /><br />
            <% } %>
            <% if (Master.PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNB)
               { %>
            <sc:SurveyRadioButton ID="radQ1_Hotel" runat="server" GroupName="Q1" SessionKey="Q1Hotel" DBColumn="Q1" DBValue="Hotel" Text="Staying at our Hotel" /><br />
            <% } %>
            <% if ( new[] { "FD", "HA", "EC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_LiveRacing" runat="server" GroupName="Q1" SessionKey="Q1LiveRacing" DBColumn="Q1" DBValue="LiveRacing" Text="Watching Live Racing" /><br />
            <% } %>
            <% if ( new[] { "RR", "HRCV", "HA", "NAN", "CMR", "EC", "SSKD" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Racebook" runat="server" GroupName="Q1" SessionKey="Q1Racebook" DBColumn="Q1" DBValue="Racebook" Text="Watching Racing at our Racebook" /><br />
            <% } %>
            <% if ( new[] { "CCH", "CMR", "CDC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Bingo" runat="server" GroupName="Q1" SessionKey="Q1Bingo" DBColumn="Q1" DBValue="Bingo" Text="Playing Bingo" /><br />
            <% } %>
            <% if ( !new[] { "CNSH", "CNSS", "EC", "CNB" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Lottery" runat="server" GroupName="Q1" SessionKey="Q1Lottery" DBColumn="Q1" DBValue="Lottery" Text="Lottery / Pull Tabs" /><br />
            <% } %>
            <sc:SurveyRadioButton ID="radQ1_None" runat="server" GroupName="Q1" SessionKey="Q1None" DBColumn="Q1" DBValue="None" Text="None" /><br />
        </asp:Panel>

       <%-- <asp:Panel runat="server">
            <% if ( new[] { "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Slots" runat="server" GroupName="Q1" SessionKey="Q1Slots" DBColumn="Q1" DBValue="Slots" Text="Playing Slots" /><br />
            <% } %>
            <% if ( new[] { "RR", "HRCV", "FD", "VRL", "NAN", "CNSH", "CNSS", "GAG", "EC", "SCTI" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Tables" runat="server" GroupName="Q1" SessionKey="Q1Tables" DBColumn="Q1" DBValue="Tables" Text="Playing Tables" /><br />
            <% } %>
            <% if ( new[] { "RR", "HRCV", "FD", "VRL", "NAN", "CNSH", "CNSS", "EC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Poker" runat="server" GroupName="Q1" SessionKey="Q1Poker" DBColumn="Q1" DBValue="Poker" Text="Playing Poker" /><br />
            <% } %>
            <sc:SurveyRadioButton ID="radQ1_Food" runat="server" GroupName="Q1" SessionKey="Q1Food" DBColumn="Q1" DBValue="Food" Text="Enjoying Food or Beverages" /><br />
            <% if ( new[] { "RR", "HRCV", "VRL", "CCH", "CMR", "CDC", "CNSH", "EC", "SCTI" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Entertainment" runat="server" GroupName="Q1" SessionKey="Q1Entertainment" DBColumn="Q1" DBValue="Entertainment" Text="Watching Live Entertainment at a show lounge or theatre" /><br />
            <% } %>
            <% if ( Master.PropertyShortCode == GCCPropertyShortCode.RR ) { %>
            <sc:SurveyRadioButton ID="radQ1_Hotel" runat="server" GroupName="Q1" SessionKey="Q1Hotel" DBColumn="Q1" DBValue="Hotel" Text="Staying at our Hotel" /><br />
            <% } %>
            <% if ( new[] { "FD", "HA", "EC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_LiveRacing" runat="server" GroupName="Q1" SessionKey="Q1LiveRacing" DBColumn="Q1" DBValue="LiveRacing" Text="Watching Live Racing" /><br />
            <% } %>
            <% if ( new[] { "RR", "HRCV", "FD", "HA", "NAN", "CMR", "EC", "SSKD" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Racebook" runat="server" GroupName="Q1" SessionKey="Q1Racebook" DBColumn="Q1" DBValue="Racebook" Text="Watching Racing at our Racebook" /><br />
            <% } %>
            <% if ( new[] { "CCH", "CMR", "CDC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Bingo" runat="server" GroupName="Q1" SessionKey="Q1Bingo" DBColumn="Q1" DBValue="Bingo" Text="Playing Bingo" /><br />
            <% } %>
            <% if ( !new[] { "CNSH", "CNSS", "EC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
            <sc:SurveyRadioButton ID="radQ1_Lottery" runat="server" GroupName="Q1" SessionKey="Q1Lottery" DBColumn="Q1" DBValue="Lottery" Text="Lottery / Pull Tabs" /><br />
            <% } %>
            <sc:SurveyRadioButton ID="radQ1_None" runat="server" GroupName="Q1" SessionKey="Q1None" DBColumn="Q1" DBValue="None" Text="None" /><br />
        </asp:Panel>--%>
    </div>
    
    <p class="question">
        Which of the following <u>other</u> activities did you engage in while visiting <%= CasinoName %>?<br />
        Please choose all that apply.
    </p>

    <div id="q2">
        <% if (new[] { "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "CNSH", "CNSS", "EC", "SSKD", "SCTI", "CNB", "WDB", "AJA", "GBH" }.Contains(Master.PropertyShortCode.ToString()))
           { %>
        <sc:SurveyCheckBox ID="chkQ2_Slots" runat="server" SessionKey="Q2Slots" DBColumn="Q2_Slots" DBValue="1" Text="Playing Slots" /><br />
        <% } %>
        <% if (new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "GAG", "EC", "SCTI", "CNB", "WDB", "GBH" }.Contains(Master.PropertyShortCode.ToString()))
           { %>
        <sc:SurveyCheckBox ID="chkQ2_Tables" runat="server" SessionKey="Q2Tables" DBColumn="Q2_Tables" DBValue="1" Text="Playing Tables" /><br />
        <% } %>
        <% if ( new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "EC", "CNB" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Poker" runat="server" SessionKey="Q2Poker" DBColumn="Q2_Poker" DBValue="1" Text="Playing Poker" /><br />
        <% } %>
        <% if (  !radQ1_Food.Checked ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Food" runat="server" SessionKey="Q2Food" DBColumn="Q2_Food" DBValue="1" Text="Enjoying Food or Beverages" /><br />
        <% } %>
        <% if (new[] { "RR", "HRCV", "VRL", "CCH", "CMR", "CDC", "CNSH", "EC", "SCTI", "WDB","GBH" }.Contains(Master.PropertyShortCode.ToString()))
           { %>
        <sc:SurveyCheckBox ID="chkQ2_Entertainment" runat="server" SessionKey="Q2Entertainment" DBColumn="Q2_Entertainment" DBValue="1" Text="Watching Live Entertainment at a show lounge or theatre" /><br />
        <% } %>
        <% if (Master.PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNB)
           { %>
        <sc:SurveyCheckBox ID="chkQ2_Hotel" runat="server" SessionKey="Q2Hotel" DBColumn="Q2_Hotel" DBValue="1" Text="Staying at our Hotel" /><br />
        <% } %>
        <% if ( new[] { "FD", "HA", "EC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_LiveRacing" runat="server" SessionKey="Q2LiveRacing" DBColumn="Q2_LiveRacing" DBValue="1" Text="Watching Live Racing" /><br />
        <% } %>
        <% if ( new[] { "RR", "HRCV", "HA", "NAN", "CMR", "EC", "SSKD" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Racebook" runat="server" SessionKey="Q2Racebook" DBColumn="Q2_Racebook" DBValue="1" Text="Watching Racing at our Racebook" /><br />
        <% } %>
        <% if ( new[] { "CCH", "CMR", "CDC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Bingo" runat="server" SessionKey="Q2Bingo" DBColumn="Q2_Bingo" DBValue="1" Text="Playing Bingo" /><br />
        <% } %>
        <% if ( !new[] { "CNSH", "CNSS", "EC", "CNB" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Lottery" runat="server" SessionKey="Q2Lottery" DBColumn="Q2_Lottery" DBValue="1" Text="Lottery / Pull Tabs" /><br />
        <% } %>
    </div>
    
    <%--<div id="q2">
        <% if ( new[] { "RR", "HRCV", "FD", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "CNSH", "CNSS", "EC", "SSKD", "SCTI" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Slots" runat="server" SessionKey="Q2Slots" DBColumn="Q2_Slots" DBValue="1" Text="Playing Slots" /><br />
        <% } %>
        <% if ( new[] { "RR", "HRCV", "FD", "VRL", "NAN", "CNSH", "CNSS", "GAG", "EC", "SCTI" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Tables" runat="server" SessionKey="Q2Tables" DBColumn="Q2_Tables" DBValue="1" Text="Playing Tables" /><br />
        <% } %>
        <% if ( new[] { "RR", "HRCV", "FD", "VRL", "NAN", "CNSH", "CNSS", "EC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Poker" runat="server" SessionKey="Q2Poker" DBColumn="Q2_Poker" DBValue="1" Text="Playing Poker" /><br />
        <% } %>
        <% if (  !radQ1_Food.Checked ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Food" runat="server" SessionKey="Q2Food" DBColumn="Q2_Food" DBValue="1" Text="Enjoying Food or Beverages" /><br />
        <% } %>
        <% if ( new[] { "RR", "HRCV", "VRL", "CCH", "CMR", "CDC", "CNSH", "EC", "SCTI" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Entertainment" runat="server" SessionKey="Q2Entertainment" DBColumn="Q2_Entertainment" DBValue="1" Text="Watching Live Entertainment at a show lounge or theatre" /><br />
        <% } %>
        <% if ( Master.PropertyShortCode == GCCPropertyShortCode.RR ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Hotel" runat="server" SessionKey="Q2Hotel" DBColumn="Q2_Hotel" DBValue="1" Text="Staying at our Hotel" /><br />
        <% } %>
        <% if ( new[] { "FD", "HA", "EC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_LiveRacing" runat="server" SessionKey="Q2LiveRacing" DBColumn="Q2_LiveRacing" DBValue="1" Text="Watching Live Racing" /><br />
        <% } %>
        <% if ( new[] { "RR", "HRCV", "FD", "HA", "NAN", "CMR", "EC", "SSKD" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Racebook" runat="server" SessionKey="Q2Racebook" DBColumn="Q2_Racebook" DBValue="1" Text="Watching Racing at our Racebook" /><br />
        <% } %>
        <% if ( new[] { "CCH", "CMR", "CDC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Bingo" runat="server" SessionKey="Q2Bingo" DBColumn="Q2_Bingo" DBValue="1" Text="Playing Bingo" /><br />
        <% } %>
        <% if ( !new[] { "CNSH", "CNSS", "EC" }.Contains( Master.PropertyShortCode.ToString() ) ) { %>
        <sc:SurveyCheckBox ID="chkQ2_Lottery" runat="server" SessionKey="Q2Lottery" DBColumn="Q2_Lottery" DBValue="1" Text="Lottery / Pull Tabs" /><br />
        <% } %>
    </div>--%>
    <% } %>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Continue" />
    </div>
    <% }
        //===========================================================================
        //PAGE 97 - Thank You
        //===========================================================================
        else if ( Master.CurrentPage == 97 ) { %>
    <sc:MessageManager runat="server" ID="mmLastPage"></sc:MessageManager>
    <% if (!String.IsNullOrEmpty( mmLastPage.SuccessMessage)) { %>
    <div class="button-container">
        <a href="http://gcgaming.com/" class="btn btn-primary">End Survey</a>
    </div>
    <% } else { %>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Go Back" />
    </div>
    <% } %>
    <% } %>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="bottomScripts">
    <script>
        $('#q1 input[type="radio"]').on('ifChecked', function (evt) {
            console.log(evt);
            var idParts = evt.currentTarget.id.split('_'),
                endPart = idParts[idParts.length - 1];
            $('#q2 input').each( function ( i, elem ) {
                if (elem.id.indexOf('_' + endPart) !== -1) {
                    $(elem).iCheck('disable');
                    $(elem).parent().next('label').addClass('disabled');
                } else {
                    $(elem).iCheck('enable');
                    $(elem).parent().next('label').removeClass('disabled');
                }
            }); 
        });
    </script>
</asp:Content>