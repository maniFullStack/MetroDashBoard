<%@ Page Language="C#" MasterPageFile="~/Survey.Master" AutoEventWireup="true" CodeBehind="HastingsSurvey.aspx.cs" Inherits="GCC_Web_Portal.HastingsSurvey" %>
<%@ MasterType VirtualPath="~/Survey.Master" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/QuestionRowControl.ascx" TagPrefix="uc1" TagName="QuestionRowControl" %>
<%@ Register Src="~/Controls/ScaleQuestionControl.ascx" TagPrefix="uc1" TagName="ScaleQuestionControl" %>
<%@ Register Src="~/Controls/SixQuestionRowControl.ascx" TagPrefix="uc1" TagName="SixQuestionRowControl" %>
<%@ Register Src="~/Controls/SixScaleQuestionControl.ascx" TagPrefix="uc1" TagName="SixScaleQuestionControl" %>
<%@ Register Src="~/Controls/YesNoControl.ascx" TagPrefix="uc1" TagName="YesNoControl" %>
<%@ Register Src="~/Controls/TriQuestionRowControl.ascx" TagPrefix="uc1" TagName="TriQuestionRowControl" %>
<%@ Register Src="~/Controls/SurveyProgressBar.ascx" TagPrefix="uc1" TagName="SurveyProgressBar" %>





<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="mainContent" runat="server">
    <% if (Master.CurrentPage == 1 ) { %>
    <h2>Hastings Racecourse Customer Survey</h2>
    <h3>Win a Hastings VIP Race Experience!</h3>
    <p>Thank you for your recent visit to Hastings Racecourse & Casino!</p>
    <p>In order to serve you better, we would like to ask you a few questions to help us better understand our guests and their needs. To show our appreciation for your time, you will be entered into a draw to win <strong>VIP Race experience</strong> (Reserved Box for 6 people, $100 in Food & Beverage credits and an opportunity to present a race) or a <strong>weekly prize draw</strong> for a Reserved Box for 6 people!</p>
    <p>You are receiving this survey because you are a member of Hastings Racecourse Insiders Club and have agreed to be contacted.  All of our surveys are conducted confidentially. Should you wish unsubscribe from our list, or want someone to follow-up with you about your experience, please visit our web site at hastingsracecourse.com, click on the “Contact” tab and then click the “Give us your Feedback” button.</p>
    <p>To start, please confirm your email address and click 'Next' below.</p>
    <sc:MessageManager runat="server" ID="mmTxtEmail"></sc:MessageManager>
    <p>
        Email address:
            <sc:SurveyTextBox ID="txtEmail" runat="server" SessionKey="txtEmail" DBColumn="Email" MaxLength="150" Size="50" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
    </p>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% } 
       
       //======================================================
       //PAGE 2 - Terms and Conditions
       //======================================================
       else if (Master.CurrentPage == 2) { %>
       <h2>Key Terms and Conditions</h2>
       <p>Your personal information is collected and used by Great Canadian Gaming Corporation (GCGC) on behalf of the British Columbia Lottery Corporation in accordance with British Columbia’s Freedom of Information and Protection of Privacy Act. It will be used for GCGC’s research purposes only. Your information will not be sold, shared with third parties, or used for soliciting purposes. If you have any questions about this, please write to GCGC’s Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
       <p>For terms of use and full terms and conditions, please see below.</p>
       <p><a href="/TandCHastings.aspx" title="Terms and Conditions" target="_blank">Terms of Use, Full Contest Conditions and Privacy Policy</a></p>
       <p class="question">
            By clicking on "I agree" and providing your email address, you accept the Personal Information and Privacy policy. The survey should take approximately 5 minutes to complete depending on your comments.
       </p>
       <sc:MessageManager runat="server" ID="mmAcceptGroup"></sc:MessageManager>
       <p>
           <sc:SurveyRadioButton ID="radAccept" runat="server" GroupName="acceptgrp" SessionKey="radAccept" CssClass="radalign" Text="&nbsp;I agree and want to proceed with the survey" /><br />
           <sc:SurveyRadioButton ID="radDecline" runat="server" GroupName="acceptgrp" SessionKey="radDecline" CssClass="radalign" Text="&nbsp;I decline to complete the survey." />
       </p>
       <div class="button-container">
           <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
           <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Continue" />
       </div>
    <% } 
       
       //======================================================
       //PAGE 99 - Declined Survey
       //======================================================
       else if (Master.CurrentPage == 99) { %>
       <p>We acknowledge that you have chosen not to participate in the survey. Thank you for your recent visit and we look forward to seeing you again soon!</p>
       <div class="button-container">
            <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Undo" /> <a href="http://www.hastingsracecourse.com" class="btn btn-primary">End Survey</a>           
       </div>
    <% } 
       
       //======================================================
       //PAGE 3 - Q1 - Q4
       //======================================================
       else if (Master.CurrentPage == 3) { %>

       <p class="question">
           How often do you typically visit Hastings Racecourse each racing season?
       </p>
       <p>
           <asp:Panel runat="server">
               <sc:MessageManager runat="server" ID="Q1Message"></sc:MessageManager>
               <sc:SurveyRadioButton ID="Q1_1" runat="server" GroupName="Q1" SessionKey="Q1_1" DBColumn="Q1" DBValue="1 time per racing season" Text="&nbsp;1 time per racing season" /><br />
               <sc:SurveyRadioButton ID="Q1_2to4" GroupName="Q1" runat="server" SessionKey="Q1_2to4" DBColumn="Q1" DBValue="2-4 times per racing season" Text="&nbsp;2-4 times per racing season" /><br />
               <sc:SurveyRadioButton ID="Q1_5to9" GroupName="Q1" runat="server" SessionKey="Q1_5to9" DBColumn="Q1" DBValue="5-9 times per racing season" Text="&nbsp;5-9 times per racing season" /><br />
               <sc:SurveyRadioButton ID="Q1_10" GroupName="Q1" runat="server" SessionKey="Q1_10" DBColumn="Q1" DBValue="More than 10 times per racing season" Text="&nbsp;More than 10 times per racing season" /><br />
           </asp:Panel>
       </p>

       <p class="question">
           Do you usually play the Racebook when you visit?
       </p>
       <p>
           <asp:Panel runat="server">
               <sc:MessageManager runat="server" ID="Q2Message"></sc:MessageManager>
               <sc:SurveyRadioButton ID="Q2_Yes" runat="server" GroupName="Q2" SessionKey="Q2_Yes" DBColumn="Q2" DBValue="Yes" Text="&nbsp;Yes" /><br />
               <sc:SurveyRadioButton ID="Q2_No" runat="server" GroupName="Q2" SessionKey="Q2_No" DBColumn="Q2" DBValue="No" Text="&nbsp;No" /><br />
           </asp:Panel>
       </p>

       <p class="question">
           Do you usually purchase Food & Beverages when you visit?
       </p>
       <p>
           <asp:Panel runat="server">
               <sc:MessageManager runat="server" ID="Q3Message"></sc:MessageManager>
               <sc:SurveyRadioButton ID="Q3_Yes" runat="server" GroupName="Q3" SessionKey="Q3_Yes" DBColumn="Q3" DBValue="Yes" Text="&nbsp;Yes" /><br />
               <sc:SurveyRadioButton ID="Q3_No" runat="server" GroupName="Q3" SessionKey="Q3_No" DBColumn="Q3" DBValue="No" Text="&nbsp;No" /><br />
           </asp:Panel>
       </p>

       <p class="question">
           Do you usually play the slots when you visit?
       </p>
           <asp:Panel runat="server">
               <sc:MessageManager runat="server" ID="Q4Message"></sc:MessageManager>
               <sc:SurveyRadioButton ID="Q4_Yes" runat="server" GroupName="Q4" SessionKey="Q4_Yes" DBColumn="Q4" DBValue="Yes" Text="&nbsp;Yes" /><br />
               <sc:SurveyRadioButton ID="Q4_No" runat="server" GroupName="Q4" SessionKey="Q4_No" DBColumn="Q4" DBValue="No" Text="&nbsp;No" /><br />              
               <p>
                   If yes, Do you have an Encore Rewards card number? Please share it here: <sc:SurveyTextBox ID="Q4_EncoreNumber" runat="server" SessionKey="Q4EncoreNumber" DBColumn="Q4EncoreNumber" MaxLength="15" Size="15" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
               </p>
           </asp:Panel>           
       <div class="button-container">
           <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
           <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
       </div>  
    <% }

       //======================================================
       //PAGE 4 - Q5 - Q8
       //======================================================
       else if (Master.CurrentPage == 4) { %>
       
       <p class="question">
           Where do you usually prefer to watch the race from? Please select the area you normally view from:
       </p>
       <p>
           <asp:Panel runat="server">
               <sc:MessageManager runat="server" ID="Q5Message"></sc:MessageManager>
               <sc:SurveyRadioButton ID="Q5_Tarmac" runat="server" GroupName="Q5" SessionKey="Q5_Tarmac" DBColumn="Q5" DBValue="Tarmac" Text="&nbsp;Tarmac" /><br />
               <sc:SurveyRadioButton ID="Q5_BoxSeats" runat="server" GroupName="Q5" SessionKey="Q5_BoxSeats" DBColumn="Q5" DBValue="Box Seats" Text="&nbsp;Box Seats" /><br />
               <sc:SurveyRadioButton ID="Q5_GroupPatio" runat="server" GroupName="Q5" SessionKey="Q5_GroupPatio" DBColumn="Q5" DBValue="Group Patio" Text="&nbsp;Group Patio (Group Patios, Sol VIP Lounge and Molson Canadian Trackside Patio)" /><br />
               <sc:SurveyRadioButton ID="Q5_MarqueeTent" runat="server" GroupName="Q5" SessionKey="Q5_MarqueeTent" DBColumn="Q5" DBValue="Marquee Tent" Text="&nbsp;Marquee Tent" /><br />
               <sc:SurveyRadioButton ID="Q5_SilksBuffet" runat="server" GroupName="Q5" SessionKey="Q5_SilksBuffet" DBColumn="Q5" DBValue="Silks Buffet" Text="&nbsp;Silks Buffet" /><br />
               <sc:SurveyRadioButton ID="Q5_DiamondClub" runat="server" GroupName="Q5" SessionKey="Q5_DiamondClub" DBColumn="Q5" DBValue="Diamond Club" Text="&nbsp;Diamond Club" /><br />
           </asp:Panel>
       </p>

       <p class="question">
           Overall, how would you rate the quality of our facility and service on your most recent visit to Hastings Racecourse?
       </p>
       <p>
           <asp:Panel runat="server">
               <sc:MessageManager runat="server" ID="Q6Message"></sc:MessageManager>
               <sc:SurveyRadioButton ID="Q6_Excellent" runat="server" GroupName="Q6" SessionKey="Q6_Excellent" DBColumn="Q6" DBValue="Excellent" Text="&nbsp;Excellent" /><br />
               <sc:SurveyRadioButton ID="Q6_VeryGood" runat="server" GroupName="Q6" SessionKey="Q6_VeryGood" DBColumn="Q6" DBValue="Very Good" Text="&nbsp;Very Good" /><br />
               <sc:SurveyRadioButton ID="Q6_Good" runat="server" GroupName="Q6" SessionKey="Q6_Good" DBColumn="Q6" DBValue="Good" Text="&nbsp;Good" /><br />
               <sc:SurveyRadioButton ID="Q6_Fair" runat="server" GroupName="Q6" SessionKey="Q6_Fair" DBColumn="Q6" DBValue="Fair" Text="&nbsp;Fair" /><br />
               <sc:SurveyRadioButton ID="Q6_Poor" runat="server" GroupName="Q6" SessionKey="Q6_Poor" DBColumn="Q6" DBValue="Poor" Text="&nbsp;Poor" /><br />
           </asp:Panel>
       </p>

       <p class="question">
           Taking into account your most recent experience (all the activities and services) at Hastings Racecourse and your money, time and effort spent, how would you rate the overall value you received?
       </p>
       <p>
           <asp:Panel runat="server">
               <sc:MessageManager runat="server" ID="Q7Message"></sc:MessageManager>
               <sc:SurveyRadioButton ID="Q7_Excellent" runat="server" GroupName="Q7" SessionKey="Q7_Excellent" DBColumn="Q7" DBValue="Excellent" Text="&nbsp;Excellent" /><br />
               <sc:SurveyRadioButton ID="Q7_VeryGood" runat="server" GroupName="Q7" SessionKey="Q7_VeryGood" DBColumn="Q7" DBValue="Very Good" Text="&nbsp;Very Good" /><br />
               <sc:SurveyRadioButton ID="Q7_Good" runat="server" GroupName="Q7" SessionKey="Q7_Good" DBColumn="Q7" DBValue="Good" Text="&nbsp;Good" /><br />
               <sc:SurveyRadioButton ID="Q7_Fair" runat="server" GroupName="Q7" SessionKey="Q7_Fair" DBColumn="Q7" DBValue="Fair" Text="&nbsp;Fair" /><br />
               <sc:SurveyRadioButton ID="Q7_Poor" runat="server" GroupName="Q7" SessionKey="Q7_Poor" DBColumn="Q7" DBValue="Poor" Text="&nbsp;Poor" /><br />
           </asp:Panel>
       </p>

        <p class="question">
            How likely would you be to recommend Hastings Racecourse to friends, family or a business associate looking for racing entertainment?
        </p>
        <p>
            <asp:Panel runat="server">
                <sc:MessageManager runat="server" ID="Q8Message"></sc:MessageManager>
                <sc:SurveyRadioButton ID="Q8_DefinitelyWould" runat="server" GroupName="Q8" SessionKey="Q8_DefinitelyWould" DBColumn="Q8" DBValue="Definitely Would" Text="&nbsp;Definitely Would" /><br />
                <sc:SurveyRadioButton ID="Q8_ProbablyWould" runat="server" GroupName="Q8" SessionKey="Q8_ProbablyWould" DBColumn="Q8" DBValue="Probably Would" Text="&nbsp;Probably Would" /><br />
                <sc:SurveyRadioButton ID="Q8_MightMightNot" runat="server" GroupName="Q8" SessionKey="Q8_MightMightNot" DBColumn="Q8" DBValue="Might or Might Not" Text="&nbsp;Might or Might Not" /><br />
                <sc:SurveyRadioButton ID="Q8_ProbablyWouldNot" runat="server" GroupName="Q8" SessionKey="Q8_ProbablyWouldNot" DBColumn="Q8" DBValue="Probably Would Not" Text="&nbsp;Probably Would Not" /><br />
                <sc:SurveyRadioButton ID="Q8_DefinitelyWouldNot" runat="server" GroupName="Q8" SessionKey="Q8_DefinitelyWouldNot" DBColumn="Q8" DBValue="Definitely Would Not" Text="&nbsp;Definitely Would Not" /><br />              
            </asp:Panel>
        </p>
       <div class="button-container">
           <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
           <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
       </div>
    <% }

       //======================================================
       //PAGE 5 - Q9 - Q13
       //======================================================
       else if (Master.CurrentPage == 5) { %> 
       
       <h3>In order to better understand your responses, please tell us a little about yourself:</h3>
       <p class="question">
           What is your gender?
       </p> 
       <p>
           <asp:Panel runat="server">
               <sc:MessageManager runat="server" ID="Q9Message"></sc:MessageManager>
               <sc:SurveyRadioButton ID="Q9_Male" runat="server" GroupName="Q9" SessionKey="Q9_Male" DBColumn="Q9" DBValue="Male" Text="&nbsp;Male" /><br />
               <sc:SurveyRadioButton ID="Q9_Female" runat="server" GroupName="Q9" SessionKey="Q9_Female" DBColumn="Q9" DBValue="Female" Text="&nbsp;Female" /><br />
           </asp:Panel>
       </p>
       
       <p class="question">
           Age Range?
       </p>
       <p>
           <asp:Panel runat="server">
               <sc:MessageManager runat="server" ID="Q10Message"></sc:MessageManager>
               <sc:SurveyRadioButton ID="Q10_19to24" runat="server" GroupName="Q10" SessionKey="Q10_19to24" DBColumn="Q10" DBValue="19-24" Text="&nbsp;19-24" /><br />
               <sc:SurveyRadioButton ID="Q10_25to34" runat="server" GroupName="Q10" SessionKey="Q10_25to34" DBColumn="Q10" DBValue="25-34" Text="&nbsp;25-34" /><br />
               <sc:SurveyRadioButton ID="Q10_35to44" runat="server" GroupName="Q10" SessionKey="Q10_35to44" DBColumn="Q10" DBValue="35-44" Text="&nbsp;35-44" /><br />
               <sc:SurveyRadioButton ID="Q10_45to54" runat="server" GroupName="Q10" SessionKey="Q10_45to54" DBColumn="Q10" DBValue="45-54" Text="&nbsp;45-54" /><br />
               <sc:SurveyRadioButton ID="Q10_55to64" runat="server" GroupName="Q10" SessionKey="Q10_55to64" DBColumn="Q10" DBValue="55-64" Text="&nbsp;55-64" /><br />
               <sc:SurveyRadioButton ID="Q10_65orOlder" runat="server" GroupName="Q10" SessionKey="Q10_65orOlder" DBColumn="Q10" DBValue="65 or older" Text="&nbsp;65 or older" /><br />
           </asp:Panel>
       </p>
    
       <p class="question">
           Gross Annual Household Income?
       </p>
       <p>
           <asp:Panel runat="server">
               <sc:MessageManager runat="server" ID="Q11Message"></sc:MessageManager>
               <sc:SurveyRadioButton ID="Q11_35000" runat="server" GroupName="Q11" SessionKey="Q11_35000" DBColumn="Q11" DBValue="Under $35,000" Text="&nbsp;Under $35,000" /><br />
               <sc:SurveyRadioButton ID="Q11_35000to59999" runat="server" GroupName="Q11" SessionKey="Q11_35000to59999" DBColumn="Q11" DBValue="$35,000 - $59,999" Text="&nbsp;$35,000 - $59,999" /><br />
               <sc:SurveyRadioButton ID="Q11_60000to89999" runat="server" GroupName="Q11" SessionKey="Q11_60000to89999" DBColumn="Q11" DBValue="$60,000 - $89,999" Text="&nbsp;$60,000 - $89,999" /><br />
               <sc:SurveyRadioButton ID="Q11_90000" runat="server" GroupName="Q11" SessionKey="Q11_90000" DBColumn="Q11" DBValue="Over $90,000" Text="&nbsp;Over $90,000" /><br />
               <sc:SurveyRadioButton ID="Q11_NoSay" runat="server" GroupName="Q11" SessionKey="Q11_NoSay" DBColumn="Q11" DBValue="Prefer not to say" Text="&nbsp;Prefer not to say" /><br />
           </asp:Panel>
       </p>
       
       <p class="question">
           What are the first 3 letters / number of your postal code? This will help us identify what neighbourhood you come from.
       </p>
       <p>
           <sc:SurveyTextBox ID="Q12_PostalCode" runat="server" SessionKey="Q12_PostalCode" DBColumn="Q12_PostalCode" MaxLength="3" Size="15" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
       </p>

       <p class="question">
           Please provide your contact information. This informtion will only be used to contact contest winners.
       </p>
           <asp:Panel runat="server">
               <p>First Name: <sc:SurveyTextBox ID="Q13_FirstName" runat="server" SessionKey="Q13_FirstName" DBColumn="FirstName" MaxLength="30" Size="30" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox></p>
               <p>Last Name: <sc:SurveyTextBox ID="Q13_LastName" runat="server" SessionKey="Q13_LastName" DBColumn="LastName" MaxLength="30" Size="30" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox></p>
               <p>E-mail address (please confirm): <sc:SurveyTextBox ID="Q13_Email" runat="server" SessionKey="Q13_Email" DBColumn="Q13_Email" MaxLength="30" Size="30" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox></p>
           </asp:Panel>
       <div class="button-container">
           <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
           <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
       </div>       
    <% }

       //======================================================
       //PAGE 97 - Survey Complete
       //====================================================== 
       else if (Master.CurrentPage == 97) { %>
       <sc:MessageManager runat="server" ID="SurveyComplete"></sc:MessageManager>
            <div class="button-container">
                <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
                <a href="http://www.hastingsracecourse.com" class="btn btn-primary">Finish</a>
            </div>
        
    <% } %> 
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="server">
</asp:Content>
