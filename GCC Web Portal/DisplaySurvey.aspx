<%@ Page Title="" Language="C#" MasterPageFile="~/Survey.Master" AutoEventWireup="true" CodeBehind="DisplaySurvey.aspx.cs" Inherits="GCC_Web_Portal.DisplaySurvey"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,PropertyStaff,HRStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Survey.Master" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Import Namespace="WebsiteUtilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
        .header-label {
            text-align:center;
            margin-top:20px;
        }
        .answer-text {
            font-weight:bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
<% if ( TopMessage.IsVisible ) { %>
<div class="row">
    <div class="col-md-6 col-md-offset-3">
        <sc:MessageManager runat="server" ID="TopMessage"></sc:MessageManager>
    </div>
</div>
<% } %>
<% if ( SurveyType == SurveyType.None || Data == null ) {
       if ( !TopMessage.IsVisible ) { %>
<div class="row">
    <div class="col-md-6 col-md-offset-3">
        <div class="box box-danger box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">Error</h3>
            </div>
            <div class="box-body">
                  Unable to load the data. Please go back and try again.
            </div>
        </div>
    </div>
</div>
<% }
} else if ( SurveyType == SurveyType.GEI ) { %>
    <p class="question">
        Survey Data:
    </p>
    <p class="answer">
        <strong>Date Submitted: </strong> <%= ReportingTools.AdjustAndDisplayDate( Data["DateCreatedISO"].ToString(), ConversionDateFormatType.FullDateTime, User ) %><br />
        <strong>Encore # (if provided): </strong> <%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( Data["Q4CardNumber"].ToString() ) %>
    </p>
    <p class="question"></p>

    <p class="question">
        Which of the following activities was your <u>primary</u> reason for visiting <%= CasinoName %>?<br />
        Please choose one.
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q1"] ) %>
    </p>

    <p class="question">
        Which of the following <u>other</u> activities did you engage in while visiting <%= CasinoName %>?<br />
        Please choose all that apply.
    </p>
    <div class="answer">
        <ul>
            <% if ( Data["Q2_Slots"].Equals(1) ) { %><li>Playing Slots</li><% } %>
            <% if ( Data["Q2_Tables"].Equals(1) ) { %><li>Playing Tables</li><% } %>
            <% if ( Data["Q2_Poker"].Equals(1) ) { %><li>Playing Poker</li><% } %>
            <% if ( Data["Q2_Food"].Equals(1) ) { %><li>Enjoying Food or Beverages</li><% } %>
            <% if ( Data["Q2_Entertainment"].Equals(1) ) { %><li>Watching Live Entertainment at a show lounge or theatre</li><% } %>
            <% if ( Data["Q2_Hotel"].Equals(1) ) { %><li>Staying at our Hotel</li><% } %>
            <% if ( Data["Q2_LiveRacing"].Equals(1) ) { %><li>Watching Live Racing</li><% } %>
            <% if ( Data["Q2_Racebook"].Equals(1) ) { %><li>Watching Racing at our Racebook</li><% } %>
            <% if ( Data["Q2_Bingo"].Equals(1) ) { %><li>Playing Bingo</li><% } %>
            <% if ( Data["Q2_Lottery"].Equals(1) ) { %><li>Lottery / Pull Tabs</li><% } %>
        </ul>
    </div>

    <% if (PropertyShortCode == GCCPropertyShortCode.GAG ) { %>
    <p class="question">
        Which Great American Casino location did you <u>last</u> visit?
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q3"] ) %>
    </p>
    <% } %>

    <p class="question">
        Are you an <%= PropertyTools.GetPlayersClubName(PropertyID) %> Member?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q4"], ANSWERS_YESNO, true ) %>
        <% if ( Data["Q4"].Equals(1) ) { %>
         - <%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( Data["Q4CardNumber"].ToString() ) %>
        <% } %>
    </p>

    <p class="question">
        Please respond to the following questions and statements based on the experience you had during your <% if ( !IsKioskOrStaffEntry ) { %>most recent <% } %>visit to <%= CasinoName %><% if ( IsKioskOrStaffEntry ) { %> today<% } %>.
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
			    <td class="label-text">Overall, how would you rate the quality of our facility and service on your most recent visit to <%= CasinoName %>?</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q5A"], ANSWERS_EXCELLENT ) %></td>
		    </tr>
		    <tr>
			    <td class="label-text">Taking into account your most recent experience (all the activities and services) at <%= CasinoName %> and your money, time, and effort spent, how would you rate the overall value you received?</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q5B"], ANSWERS_EXCELLENT ) %></td>
		    </tr>
        </table>
    </div>

    <p class="question">
        How likely are you to...
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
		    <tr>
			    <td class="label-text">Recommend <%= CasinoName %> to a friend, family member or business associate who is looking for gaming entertainment.</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q6A"], ANSWERS_WOULD ) %></td>
		    </tr>
		    <tr>
			    <td class="label-text">Visit mostly <%= CasinoName %> for your gaming entertainment.</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q6B"], ANSWERS_WOULD ) %></td>
		    </tr>
		    <tr>
			    <td class="label-text">Visit <%= CasinoName %> for your next gaming entertainment opportunity.</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q6C"], ANSWERS_WOULD ) %></td>
		    </tr>
		    <tr>
			    <td class="label-text">Provide contact information and personal preferences to <%= CasinoName %> so that the casino can serve you better.</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q6D"], ANSWERS_WOULD ) %></td>
		    </tr>
        </table>
    </div>

    <p class="question">
        How satisfied were you with the <u>staff</u> and <u>the level of customer service</u> provided during your <% if ( !IsKioskOrStaffEntry ) { %>last <% } %>visit to <%= CasinoName %><% if ( IsKioskOrStaffEntry ) { %> today<% } %>?<br />
        Please rate the following:
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
		    <tr>
			    <td class="label-text">Ensuring all of your needs were met</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q7A"], ANSWERS_EXCELLENT ) %></td>
		    </tr>
		    <tr>
			    <td class="label-text">Making you feel welcome</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q7B"], ANSWERS_EXCELLENT ) %></td>
		    </tr>
		    <tr>
			    <td class="label-text">Going above & beyond normal service</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q7C"], ANSWERS_EXCELLENT ) %></td>
		    </tr>
		    <tr>
			    <td class="label-text">Speed of service</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q7D"], ANSWERS_EXCELLENT ) %></td>
		    </tr>
		    <tr>
			    <td class="label-text">Encouraging you to visit again</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q7E"], ANSWERS_EXCELLENT ) %></td>
		    </tr>
		    <tr>
			    <td class="label-text">Overall staff availability</td>
			    <td class="answer-text"><%= GetAnswerValue( Data["Q7F"], ANSWERS_EXCELLENT ) %></td>
		    </tr>
        </table>
    </div>

    <p class="question">
        Overall, how satisfied <%= !IsKioskOrStaffEntry ? "were" : "are" %> you with the <u>service provided by the staff</u> at <%= CasinoName %>?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q8"], ANSWERS_SATISFACTION ) %>
    </p>

    <p class="question">
        Specifically, how would you rate each of these staff members that you encountered? Please rate your satisfaction with the staff you interacted with:
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Cashiers</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9A"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Guest Services</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9B"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Slot Attendants</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9C"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Dealers</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9D"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Restaurant Servers</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9E"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Cocktail Servers</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9F"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Coffee Servers</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9G"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Security</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9H"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Managers/Supervisors</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9I"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Hotel Staff</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9J"], ANSWERS_EXCELLENT ) %></td>
            </tr>
        </table>

    </div>

    <p class="question">
        Thinking about all of the  staff members that you dealt with during your visit, how would you rate them on:
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Encouraging you to take part in events or promotions</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10A"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Answering questions you had about the property or promotions</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10B"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Being friendly and welcoming</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10C"], ANSWERS_EXCELLENT ) %></td>
            </tr>
        </table>        
    </div>

    <p class="question">
        Please provide any additional comments or suggestions regarding <%= CasinoName %>'s staff. If you have no comments, please leave the field blank.
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q11"] ) %>
    </p>

    <p class="question">
        How would you rate your satisfaction level with <%= CasinoName %>'s facilities overall?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q12"], ANSWERS_SATISFACTION ) %>
    </p>

    <p class="question">
        Please rate your satisfaction level with the following:
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Ambiance, mood, atmosphere of the environment</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13A"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Cleanliness of general areas</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13B"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Clear signage</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13C"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Washroom cleanliness</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13D"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Adequate  lighting - it is bright enough</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13E"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Safe environment</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13F"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Parking availability</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13G"], ANSWERS_EXCELLENT ) %></td>
            </tr>
        </table>
    </div>

    <p class="question">
        How would you rate your satisfaction with your primary gaming experience overall?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q14"], ANSWERS_SATISFACTION ) %>
    </p>

    <p class="question">
        Based on your primary gaming activity. Please rate your level of satisfaction with <%= CasinoName %>'s gaming in terms of...?
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Variety of games available</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15A"], ANSWERS_EXCELLENT ) %>
            </tr>
            <tr>
                <td class="label-text">Waiting time to play</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15B"], ANSWERS_EXCELLENT ) %>
            </tr>
            <tr>
                <td class="label-text">Availability of specific game at your desired denomination</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15C"], ANSWERS_EXCELLENT ) %>
            </tr>
            <tr>
                <td class="label-text">Contests & monthly promotions</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15D"], ANSWERS_EXCELLENT ) %>
            </tr>
            <tr>
                <td class="label-text">Courtesy & respectfulness of staff</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15E"], ANSWERS_EXCELLENT ) %>
            </tr>
            <tr>
                <td class="label-text">Game Knowledge of Staff</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15F"], ANSWERS_EXCELLENT ) %>
            </tr>
        </table>
    </div>

    <p class="question">
        What is your overall satisfaction with the <%= PropertyTools.GetPlayersClubName( PropertyID ) %> program?
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Rate of earning</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q16A"], ANSWERS_EXCELLENT ) %>
            </tr>
            <tr>
                <td class="label-text">Redemption value</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q16B"], ANSWERS_EXCELLENT ) %>
            </tr>
            <tr>
                <td class="label-text">Choice of rewards</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q16C"], ANSWERS_EXCELLENT ) %>
            </tr>
            <tr>
                <td class="label-text">Slot Free Play</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q16D"], ANSWERS_EXCELLENT ) %>
            </tr>
        </table>
    </div>
    
    <p class="question">
        Did you purchase any food and/or beverages during your most recent visit to <%= CasinoName %>?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q17"], ANSWERS_YESNO, true ) %>
    </p>

    <p class="question">
        Where did you purchase food and/or beverage? Select all that apply:
    </p>
    <div class="answer">
        <ul>
            <% if ( Data["Q18_1"].Equals( 1 ) ) { %><li>Coffee Station / Gaming Floor</li><% } %>
            <% if ( Data["Q18_2"].Equals( 1 ) ) { %><li>Curve</li><% } %>
            <% if ( Data["Q18_3"].Equals( 1 ) ) { %><li>Tramonto Restaurant</li><% } %>
            <% if ( Data["Q18_4"].Equals( 1 ) ) { %><li>Lulu's Lounge</li><% } %>
            <% if ( Data["Q18_5"].Equals( 1 ) ) { %><li>The Buffet</li><% } %>
            <% if ( Data["Q18_6"].Equals( 1 ) ) { %><li>Sea Harbour Seafood Restaurant</li><% } %>
            <% if ( Data["Q18_7"].Equals( 1 ) ) { %><li>Java Jacks Café</li><% } %>
            <% if ( Data["Q18_8"].Equals( 1 ) ) { %><li>International Food Court</li><% } %>
            <% if ( Data["Q18_14"].Equals( 1 ) ) { %><li>Asylum Gastro-Pub and Live Sound Stage</li><% } %>
            <% if ( Data["Q18_15"].Equals( 1 ) ) { %><li>UnListed Buffet and Lounge</li><% } %>
            <% if ( Data["Q18_16"].Equals( 1 ) ) { %><li>Neptune Noodle House</li><% } %>
            <% if ( Data["Q18_17"].Equals( 1 ) ) { %><li>Fu Express Authentic Asian Cuisine</li><% } %>
            <% if ( Data["Q18_18"].Equals( 1 ) ) { %><li>Roadies Burger Bar</li><% } %>
            <% if ( Data["Q18_19"].Equals( 1 ) ) { %><li>Chip's Sandwich Shop</li><% } %>
            <% if ( Data["Q18_20"].Equals( 1 ) ) { %><li>Fuel Café</li><% } %>
            <% if ( Data["Q18_9"].Equals( 1 ) ) { %><li>Poker Room</li><% } %>
            <% if ( Data["Q18_10"].Equals( 1 ) ) { %><li>Salon Privé</li><% } %>
            <% if ( Data["Q18_11"].Equals( 1 ) ) { %><li>Dogwood Room</li><% } %>
            <% if ( Data["Q18_12"].Equals( 1 ) ) { %><li>Jade Room</li><% } %>
            <% if ( Data["Q18_13"].Equals( 1 ) ) { %><li>Phoenix Room</li><% } %>
            <% if ( Data["Q18_21"].Equals( 1 ) ) { %><li>The Homestretch Buffet</li><% } %>
            <% if ( Data["Q18_22"].Equals( 1 ) ) { %><li>The Bridge</li><% } %>
            <% if ( Data["Q18_23"].Equals( 1 ) ) { %><li>The Clubhouse Buffet</li><% } %>
            <% if ( Data["Q18_24"].Equals( 1 ) ) { %><li>Casino Bar</li><% } %>
            <% if ( Data["Q18_25"].Equals( 1 ) ) { %><li>Racebook</li><% } %>
            <% if ( Data["Q18_26"].Equals( 1 ) ) { %><li>Eclipse Lounge</li><% } %>
            <% if ( Data["Q18_27"].Equals( 1 ) ) { %><li>Silks Restaurant</li><% } %>
            <% if ( Data["Q18_28"].Equals( 1 ) ) { %><li>Furlongs Eatery</li><% } %>
            <% if ( Data["Q18_29"].Equals( 1 ) ) { %><li>Jeromes</li><% } %>
            <% if ( Data["Q18_30"].Equals( 1 ) ) { %><li>George Royal Room</li><% } %>
            <% if ( Data["Q18_31"].Equals( 1 ) ) { %><li>Concessions</li><% } %>
            <% if ( Data["Q18_32"].Equals( 1 ) ) { %><li>View Royal Restaurant</li><% } %>
            <% if ( Data["Q18_33"].Equals( 1 ) ) { %><li>View Royal Patio</li><% } %>
            <% if ( Data["Q18_34"].Equals( 1 ) ) { %><li>Black Diamond Bar & Grille</li><% } %>
            <% if ( Data["Q18_35"].Equals( 1 ) ) { %><li>The Well</li><% } %>
            <% if ( Data["Q18_36"].Equals( 1 ) ) { %><li>Prospects Lounge</li><% } %>
            <% if ( Data["Q18_37"].Equals( 1 ) ) { %><li>Trapeze</li><% } %>
            <% if ( Data["Q18_38"].Equals( 1 ) ) { %><li>The Station</li><% } %>
            <% if ( Data["Q18_39"].Equals( 1 ) ) { %><li>Harbourfront Lounge</li><% } %>
            <% if ( Data["Q18_40"].Equals( 1 ) ) { %><li>High Limit Gaming Area</li><% } %>
            <% if ( Data["Q18_41"].Equals( 1 ) ) { %><li>Celtic Junction Bar & Grille</li><% } %>
            <% if ( Data["Q18_42"].Equals( 1 ) ) { %><li>Bar / Restuarant at Great American Casino</li><% } %>
            <% if ( Data["Q18_43"].Equals( 1 ) ) { %><li>Diamond Buffet</li><% } %>
            <% if ( Data["Q18_44"].Equals( 1 ) ) { %><li>Foodies</li><% } %>
            <% if ( Data["Q18_45"].Equals( 1 ) ) { %><li>Molson Lounge</li><% } %>
            <% if ( Data["Q18_46"].Equals( 1 ) ) { %><li>Escape</li><% } %>
            <% if ( Data["Q18_47"].Equals( 1 ) ) { %><li>Player's Lounge and Café</li><% } %>
            <% if ( Data["Q18_48"].Equals( 1 ) ) { %><li>Windward Restaurant & Lounge</li><% } %>
            <% if ( Data["Q18_49"].Equals( 1 ) ) { %><li>Rue 333</li><% } %>
            <% if ( Data["Q18_50"].Equals( 1 ) ) { %><li>Hub City Pub</li><% } %>
            <% if ( Data["Q18_51"].Equals( 1 ) ) { %><li>Buffet</li><% } %>
            <% if ( Data["Q18_52"].Equals( 1 ) ) { %><li>Room Service</li><% } %>
        </ul>
    </div>

    <% for ( int i = 1; i <= 13; i++ ) {
           string fbName = GetFoodAndBevName( i );
           //End the loop if this property has no more F&B locations
           if ( String.IsNullOrEmpty( fbName ) ) {
               break;
           }
           //Skip if it wasn't answered
           if ( Data["Q19_M" + i].Equals( DBNull.Value ) ) {
               continue;
           }
    %>
    <p class="question">
        How would you rate your overall satisfaction level with the food & beverage services at <b><%= ReportingTools.CleanData( fbName ) %></b>?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q19_M" + i], ANSWERS_SATISFACTION ) %>
    </p>
        
    <p class="question">
        More specifically, how would you rate food and beverage services at <b><%= ReportingTools.CleanData( fbName ) %></b> in terms of...?
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Variety of food choices</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q20A_M" + i], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Cleanliness of outlet</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q20B_M" + i], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Courtesy of staff</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q20C_M" + i], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Timely delivery of order</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q20D_M" + i], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Value for the money</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q20E_M" + i], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Pleasant atmosphere</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q20F_M" + i], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality of food</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q20G_M" + i], ANSWERS_EXCELLENT ) %></td>
            </tr>
        </table>
    </div>
    <% } %>

    <p class="question">
        <% if ( !IsKioskOrStaffEntry ) { %>
        Did you visit <%= GetShowLoungeName() %> during your most recent visit for entertainment?
        <% } else { %>
        Have you visited <%= GetShowLoungeName() %> recently for entertainment?
        <% } %>
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q21"], ANSWERS_YESNO, true ) %>
        <% if ( Data["Q21"].Equals( 1 ) ) { %>
         - <%= ReportingTools.CleanData( Data["Q21VisitDate"] ) %>
        <% } %>
    </p>

    <% if ( PropertyShortCode == GCCPropertyShortCode.HRCV ) { %>
    <p class="question">
        Which Show Lounge did you visit at Hard Rock Casino Vancouver?
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q21_HRCV_Lounge"] ) %>
    </p>
    <% } %>

    <p class="question">
        How would you rate your overall satisfaction level with your entertainment experience at <%= GetShowLoungeName( true ) %>?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q22"], ANSWERS_SATISFACTION ) %>
    </p>

    <p class="question">
        Please rate your satisfaction level with the entertainment at <%= GetShowLoungeName( true ) %> in terms of...?
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Sound / quality</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q23A"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Seating availability</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q23B"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Dance floor</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q23C"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Fun and enjoyable atmosphere</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q23D"], ANSWERS_EXCELLENT ) %></td>
            </tr>
        </table>
    </div>

    <p class="question">
        <% if ( !IsKioskOrStaffEntry ) { %>
        Did you attend a show at the <%= CasinoName %> Show Theatre during this visit or in the last 30 days?
        <% } else { %>
        Have you attended a show at the <%= CasinoName %> Show Theatre recently? 
        <% } %>
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q24"], ANSWERS_YESNO, true ) %>
        <% if ( Data["Q21"].Equals( 1 ) ) { %>
         - <%= ReportingTools.CleanData( Data["Q24VisitDate"] ) %>
        <% } %>
    </p>

    <p class="question">
        How would you rate your satisfaction level overall entertainment experience in the <%= CasinoName %> Show Theatre?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q25"], ANSWERS_SATISFACTION ) %>
    </p>

    <p class="question">
        Please rate your satisfaction level with your experience at this event in terms of...?
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">The quality of the show</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q26A"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">The value of the show</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q26B"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Seating choices</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q26C"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Sound quality</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q26D"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Overall customer service of Theatre staff</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q26E"], ANSWERS_EXCELLENT ) %></td>
            </tr>
        </table>
    </div>

    <p class="question">
        Did you experience a problem or issue during your<% if ( !IsKioskOrStaffEntry ) { %> most recent<% } %> visit<% if ( IsKioskOrStaffEntry ) { %> today<% } %>?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q27"], ANSWERS_YESNO, true ) %>
        <% if ( Data["Q27"].ToString().Equals( "1" ) ) { %><a href="/Admin/Feedback/<%= Data["FeedbackUID"] %>" target="_blank"> (Click here to view <i class="fa fa-external-link"></i>)</a><% } %>
    </p>

    <p class="question">
        Where or with whom did the problem occur?
    </p>
    <p class="answer">
        <ul>
            <% if ( Data["Q27A_ArrivalAndParking"].Equals( 1 ) ) { %><li>Arrival and parking</li><% } %>
            <% if ( Data["Q27A_GuestServices"].Equals( 1 ) ) { %><li>Guest Services</li><% } %>
            <% if ( Data["Q27A_Cashiers"].Equals( 1 ) ) { %><li>Cashiers</li><% } %>
            <% if ( Data["Q27A_ManagerSupervisor"].Equals( 1 ) ) { %><li>Manager/Supervisor</li><% } %>
            <% if ( Data["Q27A_Security"].Equals( 1 ) ) { %><li>Security</li><% } %>
            <% if ( Data["Q27A_Slots"].Equals( 1 ) ) { %><li>Slots</li><% } %>
            <% if ( Data["Q27A_Tables"].Equals( 1 ) ) { %><li>Tables</li><% } %>
            <% if ( Data["Q27A_FoodAndBeverage"].Equals( 1 ) ) { %><li>Food & Beverage</li><% } %>
            <% if ( Data["Q27A_Hotel"].Equals( 1 ) ) { %><li>Hotel</li><% } %>
            <% if ( Data["Q27A_Bingo"].Equals( 1 ) ) { %><li>Bingo</li><% } %>
            <% if ( Data["Q27A_Entertainment"].Equals( 1 ) ) { %><li>Entertainment</li><% } %>
            <% if ( Data["Q27A_HorseRacing"].Equals( 1 ) ) { %><li>Horse Racing</li><% } %>
            <% if ( Data["Q27A_Other"].Equals( 1 ) ) { %><li>Other: <%= ReportingTools.CleanData( Data["Q27A_OtherExplanation"] ) %></li><% } %>
        </ul>
    </p>

    <p class="question">
        Briefly describe your problem.
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q27B"] ) %>
    </p>

    <p class="question">
        Has this problem been resolved?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q28"], ANSWERS_YESNO, true ) %>
    </p>

    <p class="question">
        Did you report the problem?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q29"], ANSWERS_YESNO, true ) %>
    </p>

    <p class="question">
        Thinking of this problem, what is your satisfaction level with the <%= CasinoName %>'s ability to fix your problem or issue?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q30"], ANSWERS_SATISFACTION ) %>
    </p>

    <p class="question">
        More specifically, how would you rate your satisfaction level with <%= CasinoName %>'s response to your problem in terms of...?
    </p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">The length of time taken to resolve your problem</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q31A"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">The effort of employees in resolving your problem</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q31B"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">The courteousness of employees while resolving your problem</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q31C"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">The amount of communication with you from employees while resolving your problem</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q31D"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">The fairness of the outcome in resolving your problem</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q31E"], ANSWERS_EXCELLENT ) %></td>
            </tr>
        </table>
    </div>

    <p class="question">
        Please provide any additional comments or suggestions regarding your experience with problem resolution at <%= CasinoName %>.
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q32"] ) %>
    </p>

    <p class="question">
        Would you like someone from <%= CasinoName %> to follow up with you regarding your recent problem?
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q33"], ANSWERS_YESNO, true ) %>
    </p>

    <p class="question">
        Please provide any additional comments on your most recent experience with our services or one suggestion that would make your next visit even more enjoyable.<br />
        <% if ( !IsKioskOrStaffEntry ) { %>If you have no comments, please leave the field blank.<% } %>
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q34"] ) %>
    </p>

    <p class="question">
        We strive to make your experience at <%= CasinoName %> great. If one of our employees made your visit particularly memorable, please share his/her name with us. If you don't know his/her name, please indicate the area within the facility.<br />
        If you have no comments, please leave the field blank.
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q35"] ) %>
    </p>

    <p class="question">
        What is your gender?
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q36"] ) %>
    </p>

    <p class="question">
        What is your age group?
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q37"] ) %>
    </p>

    <p class="question">
        About how often do you come to <%= CasinoName %> for your entertainment or gaming needs?
    </p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q38"] ) %>
    </p>

    <p class="question">
        Other than English, what other languages do you REGULARLY speak at home, if any?
    </p>
    <div class="answer">
        <ul>
            <% if( !Data["Q39_1"].Equals( DBNull.Value ) ) { %><li>Korean</li><% } %>
            <% if( !Data["Q39_2"].Equals( DBNull.Value ) ) { %><li>Punjabi</li><% } %>
            <% if( !Data["Q39_3"].Equals( DBNull.Value ) ) { %><li>Chinese - Mandarin</li><% } %>
            <% if( !Data["Q39_4"].Equals( DBNull.Value ) ) { %><li>Other Western European languages</li><% } %>
            <% if( !Data["Q39_5"].Equals( DBNull.Value ) ) { %><li>Eastern European languages</li><% } %>
            <% if( !Data["Q39_6"].Equals( DBNull.Value ) ) { %><li>Spanish</li><% } %>
            <% if( !Data["Q39_7"].Equals( DBNull.Value ) ) { %><li>French</li><% } %>
            <% if( !Data["Q39_8"].Equals( DBNull.Value ) ) { %><li>Hindi</li><% } %>
            <% if( !Data["Q39_9"].Equals( DBNull.Value ) ) { %><li>Tagalog</li><% } %>
            <% if( !Data["Q39_10"].Equals( DBNull.Value ) ) { %><li>Vietnamese</li><% } %>
            <% if( !Data["Q39_11"].Equals( DBNull.Value ) ) { %><li>Pakistani</li><% } %>
            <% if( !Data["Q39_12"].Equals( DBNull.Value ) ) { %><li>Farsi</li><% } %>
            <% if( !Data["Q39_13"].Equals( DBNull.Value ) ) { %><li>Japanese</li><% } %>
            <% if( !Data["Q39_14"].Equals( DBNull.Value ) ) { %><li>Arabic / Middle Eastern</li><% } %>
            <% if( !Data["Q39_15"].Equals( DBNull.Value ) ) { %><li>Chinese – Cantonese</li><% } %>
            <% if( !Data["Q39_16"].Equals( DBNull.Value ) ) { %><li>Other: <%= ReportingTools.CleanData( Data["Q39_16Explanation"] ) %></li><% } %>
        </ul>
    </div>

    <% if ( !Data["Q33"].Equals( 1 ) ) { %>
    <p class="question">
        <% if ( !IsKioskOrStaffEntry ) { %>
        Would you like casino staff to contact you about your feedback? Only respond “Yes” if you want to be further contacted by staff <u>about your feedback</u>.
        <% } else { %>
        Would you like Management to respond to you regarding this survey? Only if you respond "Yes" will you be contacted by casino staff.
        <% } %>
    </p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q40"], ANSWERS_YESNO, true ) %>
    </p>
    <% } %>
    <% if ( Data["Q40"].Equals( 1 ) || Data["Q33"].Equals( 1 ) ) { %>
    <p class="question">
        <% if ( !IsKioskOrStaffEntry ) { %>
        Would you like casino staff to contact you about your feedback? Only respond “Yes” if you want to be further contacted by staff <u>about your feedback</u>.
        <% } else { %>
        Please confirm your contact information if you wish to be contacted. First and last name are mandatory. All feedback will be responded to via email.  If you would prefer to be contacted via Telephone, please provide it below.
        <% } %>
    </p>
    <p class="answer">
        First Name: <%= ReportingTools.CleanData( Data["FirstName"] ) %><br />
        Last Name: <%= ReportingTools.CleanData( Data["LastName"] ) %><br />
        Contact Email: <%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( Data["ContactEmail"].ToString() ) %><br />
        Telephone Number: <%= ReportingTools.CleanData( Data["TelephoneNumber"] ) %><br />
    </p>
    <% } %>
<% } else 
   ////////////////////////////////////////////////////////////////
   //                        HOTEL SURVEY                        //
   ////////////////////////////////////////////////////////////////    
   if ( SurveyType == SurveyType.Hotel ) { %>
    <p>Email Given: <%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( Data["Email"].ToString() ) %></p>

    <p class="question">How satisfied were you with your stay at the River Rock Casino Hotel overall?</p>
    <p class="answer">
	    <%= GetAnswerValue( Data["Q1Overall"], ANSWERS_SATISFACTION ) %>
    </p>

    <p class="question">How satisfied were you with the <u>staff</u> and <u>the level of customer service</u> provided during your last visit to River Rock Casino Resort? Please rate on the following scale:</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Ensuring all of your needs were met</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q1A"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Making you feel welcome</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q1B"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Going above & beyond normal service</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q1C"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Speed of service</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q1D"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Encouraging you to visit again</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q1E"], ANSWERS_EXCELLENT ) %></td>
            </tr>
            <tr>
                <td class="label-text">Overall staff availability</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q1F"], ANSWERS_EXCELLENT ) %></td>
            </tr>

        </table>
    </div>

    <p class="question">Overall, how satisfied were you with the <u>service provided by the staff</u> at River Rock Casino Resort?</p>
    <div class="answer">
        <%= GetAnswerValue( Data["Q2"], ANSWERS_SATISFACTION ) %>
    </div>

    <p class="question">Please rate your satisfaction with our <u>Reservation, Check-In & Check-Out</u> services using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Friendliness of Reservation Agent</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q3A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Helpfulness of Reservation Agent</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q3B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Accuracy of reservation information upon check-in</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q3C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Employee knowledge of the River Rock Casino Resort &amp; Facilities</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q3D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Efficiency of check-in</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q3E"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Friendliness of Front Desk staff</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q3F"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Helpfulness of Front Desk staff</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q3G"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Employees' 'can-do' attitude</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q3H"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Efficiency of check-out</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q3I"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Accuracy of bill at check-out</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q3J"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>

    <p class="question">Please rate your satisfaction with our <u>Housekeeping</u> services using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Friendliness of Housekeeping staff</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q4A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Room cleanliness</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q4B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Bathroom cleanliness</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q4C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>

    <p class="question">Please rate your satisfaction with your <u>Hotel Room</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Comfortable bed and furniture</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q5A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Proper functioning of lights, TV, etc.</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q5B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Overall condition of the room</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q5C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Adequate amenities</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q5D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>

    <p class="question">Please specify if there are any amenities you would like to see in your hotel room:</p>
    <p class="answer">
	    <%= ReportingTools.CleanData( Data["Q5Amenities"] ) %>
    </p>
    <p class="question">Amenities Used During Visit:</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Did you visit Tramonto Restaurant during your stay?</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q6Tramonto"], ANSWERS_YESNO, true ) %></td>
            </tr>
            <tr>
                <td class="label-text">Did you visit The Buffet during your stay?</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q6TheBuffet"], ANSWERS_YESNO, true ) %></td>
            </tr>
            <tr>
                <td class="label-text">Did you visit Curve during your stay?</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q6Curve"], ANSWERS_YESNO, true ) %></td>
            </tr>
            <tr>
                <td class="label-text">Did you use In-Room Dining during your stay?</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q6InRoomDining"], ANSWERS_YESNO, true ) %></td>
            </tr>
            <tr>
                <td class="label-text">Did you visit the Fitness Center during your stay?</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q6FitnessCenter"], ANSWERS_YESNO, true ) %></td>
            </tr>
            <tr>
                <td class="label-text">Did you use the Pool and/or Hot Tub?</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q6PoolHotTub"], ANSWERS_YESNO, true ) %></td>
            </tr>
            <tr>
                <td class="label-text">Did you attend a meeting or event during your stay?</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q6Meeting"], ANSWERS_YESNO, true ) %></td>
            </tr>
            <tr>
                <td class="label-text">Did you use our Valet Parking during your stay?</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q6ValetParking"], ANSWERS_YESNO, true ) %></td>
            </tr>
            <tr>
                <td class="label-text">Did you use our Concierge during your stay?</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q6Concierge"], ANSWERS_YESNO, true ) %></td>
            </tr>
            <tr>
                <td class="label-text">Did you use our Bell/Door Service during your stay?</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q6BellDoorService"], ANSWERS_YESNO, true ) %></td>
            </tr>
        </table>
    </div>
    <% if ( Data["Q6Tramonto"].Equals( 1 ) ) { %>
    <p class="question">Please rate your satisfaction with <u>Tramonto Restaurant</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Greeting upon arrival</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q7A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Timeliness of seating </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q7B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Attentiveness of server </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q7C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Server’s knowledge of menu selections </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q7D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Timeliness of meal delivery</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q7E"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality and taste of food </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q7F"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Presentation of food </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q7G"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality of beverage</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q7H"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Accuracy of bill</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q7I"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>
    <% }


       if ( Data["Q6TheBuffet"].Equals( 1 ) ) { %>
    <p class="question">Please rate your satisfaction with <u>The Buffet</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Greeting upon arrival</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q8A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Attentiveness of server </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q8B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Server’s knowledge of menu selections </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q8C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality and taste of food </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q8D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality of beverage</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q8E"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Accuracy of bill</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q8F"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>
    <% }


       if ( Data["Q6Curve"].Equals( 1 ) ) { %>
    <p class="question">Please rate your satisfaction with <u>Curve</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Greeting upon arrival</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Timeliness of seating </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Attentiveness of server </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Server’s knowledge of menu selections </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Timeliness of meal delivery</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9E"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality and taste of food </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9F"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Presentation of food </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9G"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality of beverage</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9H"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Accuracy of bill</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q9I"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>
    <% }


       if ( Data["Q6InRoomDining"].Equals( 1 ) ) { %>
    <p class="question">Please rate your satisfaction with <u>In-Room Dining</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Phone answered promptly</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Friendliness of order taker</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Friendliness of server</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Order delivered within time period advised</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Accuracy of order</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10E"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Presentation of food</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10F"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality of in-room dining food</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10G"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Delivery staff offered pick-up of empty tray</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q10H"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>
    <% }


       if ( Data["Q6FitnessCenter"].Equals( 1 ) ) { %>
    <p class="question">Please rate your satisfaction with the <u>Fitness Centre</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Cleanliness of Fitness Center</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q11A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality/ condition of fitness equipment</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q11B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Availability of Fitness Center equipment</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q11C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Variety of equipment</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q11D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>
    <% }


       if ( Data["Q6PoolHotTub"].Equals( 1 ) ) { %>
    <p class="question">Please rate your satisfaction with the <u>Pool and/or Hot Tub</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Cleanliness of pool area</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q12A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Temperature of pool</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q12B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Cleanliness of hot tub area</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q12C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Temperature of hot tub</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q12D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Cleanliness of changing rooms</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q12E"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>
    <% }


       if ( Data["Q6Meeting"].Equals( 1 ) ) { %>
    <p class="question">What meeting or event were you attending at River Rock Casino Resort?</p>
    <p class="answer">
	    <%= ReportingTools.CleanData( Data["Q13_MeetingDescription"] ) %>
    </p>

    <p class="question">Please rate your satisfaction with our <u>Meeting and Event services</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Condition and cleanliness of meeting/event room</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Proper meeting/event room temperature</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality of meeting/event food and beverage</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Friendliness and efficiency of meeting/event staff</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Quality/condition/support of technical equipment</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13E"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Meeting/event facilities (size, design, amenities)</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13F"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Accuracy of meeting/ event signage</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q13G"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>
    <% }


       if ( Data["Q6ValetParking"].Equals( 1 ) ) { %>
    <p class="question">Please rate your satisfaction with the <u>Valet Parking services</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Greeting upon arrival</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q14A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Car returned in timely manner</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q14B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Original mirror position</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q14C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Original radio station </td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q14D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Original seat position</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q14E"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Valet driver drove care in respectful manner</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q14F"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Pleasant departure greeting</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q14G"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>
    <% }


       if ( Data["Q6Concierge"].Equals( 1 ) ) { %>
    <p class="question">Please rate your satisfaction with our <u>Concierge services</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Availability of Concierge</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Friendliness of Concierge</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Level of knowledge</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Staff member went out of way to provide excellent service</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Pleasant departure greeting</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q15E"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>
    <% }


       if ( Data["Q6BellDoorService"].Equals( 1 ) ) { %>
    <p class="question">Please rate your satisfaction with our <u>Bell/Door Service</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Greeting upon arrival</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q16A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Acknowledgement throughout stay</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q16B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Friendliness of bell/ door staff</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q16C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Level of knowledge</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q16D"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Staff member went out of way to provide excellent service</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q16E"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Pleasant departure greeting</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q16F"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>
    <% } %>
    <p class="question">Please rate your satisfaction with <u>how we made you feel</u> during your stay using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Welcome</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q17A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Comfortable</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q17B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Important</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q17C"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>

    <p class="question">Please rate your satisfaction with <u>your overall stay at River Rock Casino Resort</u> using the scale below.</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">Overall condition of the River Rock Casino Resort</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q18A"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">Value for price</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q18B"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>

    <p class="question">If you return to this area, how likely is it that you will return to this resort?</p>
    <p class="answer">
	    <%= GetAnswerValue( Data["Q19"], ANSWERS_TRI_LIKELY ) %>
    </p>

    <p class="question">How likely is it that you will recommend this resort to others?</p>
    <p class="answer">
	    <%= GetAnswerValue( Data["Q20"], ANSWERS_TRI_LIKELY ) %>
    </p>

    <p class="question">During your stay, did the staff provide exceptional service which exceeded your expectations?</p>
    <p class="answer">
	    <%= GetAnswerValue( Data["Q21"], ANSWERS_YESNO, true ) %>
    </p>
    <% if ( Data["Q21"].Equals(1) ) { %>
    <p>If yes, please provide the name & department of the staff member who provided exceptional service:</p>
    <p><%= ReportingTools.CleanData( Data["Q21Explanation"] ) %></p>
    <% } %>

    <p class="question">When selecting a hotel, how important are eco-friendly or "green" initiatives?</p>
    <p class="answer">
	    <%= GetAnswerValue( Data["Q22"], ANSWERS_TRI_IMPORTANT ) %>
    </p>

    <p class="question">Did you experience a problem during your stay with us?</p>
    <p class="answer">
	    <%= GetAnswerValue( Data["Q23"], ANSWERS_YESNO, true ) %>
    </p>

    <% if ( Data["Q23"].Equals( 1 ) ) { %>
    <p class="question">Please categorize the nature of your problem during your stay:</p>
    <div class="answer">
        <ul>
	        <% if ( Data["Q24A_Arrival"].Equals( 1 ) ) { %><li>Arrival</li><% } %>
            <% if ( Data["Q24A_Staff"].Equals( 1 ) ) { %><li>Staff</li><% } %>
            <% if ( Data["Q24A_GuestRoom"].Equals( 1 ) ) { %><li>Guest Room</li><% } %>
            <% if ( Data["Q24A_FoodBeverage"].Equals( 1 ) ) { %><li>Food & Beverage</li><% } %>
            <% if ( Data["Q24A_FacilitiesService"].Equals( 1 ) ) { %><li>Facilities & Service</li><% } %>
            <% if ( Data["Q24A_BillingDeparture"].Equals( 1 ) ) { %><li>Billing/Departure</li><% } %>
            <% if ( Data["Q24A_MeetingsEvents"].Equals( 1 ) ) { %><li>Meetings & Events</li><% } %>
            <% if ( Data["Q24A_Other"].Equals( 1 ) ) { %><li>Other: <%= ReportingTools.CleanData( Data["Q24A_OtherExplanation"] ) %></li><% } %>
        </ul>
    </div>

    <p class="question">Please briefly describe your problem:</p>
    <p class="answer">
	    <%= ReportingTools.CleanData( Data["Q24B"] ) %>
    </p>
        
    <p class="question">Did you report the problem?</p>
    <p class="answer">
	    <%= GetAnswerValue( Data["Q24C"], ANSWERS_YESNO, true ) %>
    </p>

    <p class="question">Thinking of this problem, what is your satisfaction level with River Rock Hotel's ability to fix your problem or issue?</p>
    <p class="answer">
	    <%= GetAnswerValue( Data["Q24D"], ANSWERS_SATISFACTION ) %>
    </p>

    <p class="question">More specifically, how would you rate your satisfaction level with River Rock Hotel's response to your problem in terms of...?</p>
    <div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:80%" />
            </colgroup>
            <tr>
                <td class="label-text">The length of time taken to resolve your problem</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q24E_1"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">The effort of employees in resolving your problem</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q24E_2"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">The courteousness of employees while resolving your problem</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q24E_3"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">The amount of communication with you from employees while resolving your problem</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q24E_4"], ANSWERS_SATISFACTION ) %></td>
            </tr>
            <tr>
                <td class="label-text">The fairness of the outcome in resolving your problem</td>
                <td class="answer-text"><%= GetAnswerValue( Data["Q24E_5"], ANSWERS_SATISFACTION ) %></td>
            </tr>
        </table>
    </div>

    <% } %>

    <p class="question">Please select your primary reason for choosing River Rock Casino Resort.</p>
    <p class="answer">
	    <%= ReportingTools.CleanData( Data["Q25"] ) %><% if ( Data["Q25"].Equals( "Other, please specify" ) ) { %>:
        <%= ReportingTools.CleanData( Data["Q25_OtherExplanation"] ) %>
        <% } %>
    </p>

    <p class="question">What was the primary purpose of your visit?</p>
    <div class="answer">
        <ul>
	        <% if ( Data["Q26Business"].Equals( 1 ) ) { %><li>Business</li><% } %>
            <% if ( Data["Q26Pleasure"].Equals( 1 ) ) { %><li>Pleasure</li><% } %>
            <% if ( Data["Q26MeetingEvent"].Equals( 1 ) ) { %><li>Meeting / Event</li><% } %>
            <% if ( Data["Q26Other"].Equals( 1 ) ) { %><li>Other, please specify: <%= ReportingTools.CleanData( Data["Q26_OtherExplanation"] ) %></li><% } %>
        </ul>
    </div>

    <p class="question">Have you visited River Rock Casino Resort before?</p>
    <p class="answer">
	    <%= GetAnswerValue( Data["Q27"], ANSWERS_YESNO, true ) %>
    </p>

    <p class="question">Do you have any other comments or suggestions for the management and staff of River Rock Casino Resort?</p>
    <p class="answer">
	    <%= ReportingTools.CleanData( Data["Q28"] ) %>
    </p>

    
    <p class="question">Would you like casino staff to contact you about your feedback? Only respond “Yes” if you want to be further contacted by staff <u>about your feedback.</u></p>
    <p class="answer">
        <%= GetAnswerValue( Data["Q29"], ANSWERS_YESNO, true ) %>
    </p>

    <% if ( Data["Q29"].Equals( 1 ) ) { %>
    <p class="question">
        Please confirm your contact information if you wish to be contacted. First and last name are mandatory. <u>You will be contacted using the email address provided at the start of this survey.</u> If you would prefer to be contacted via Telephone, please provide it below.
    </p>
    <p class="answer">
        First Name: <%= ReportingTools.CleanData( Data["FirstName"] ) %><br />
        Last Name: <%= ReportingTools.CleanData( Data["LastName"] ) %><br />
        Contact Email: <%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( Data["ContactEmail"].ToString() ) %><br />
        Telephone Number: <%= ReportingTools.CleanData( Data["TelephoneNumber"] ) %><br />
        Please briefly describe the reason you're requesting a follow up.<br />
        <%= ReportingTools.CleanData( Data["FollowupReason"] ) %>
    </p>
    <% } %>
<% } else 
   ////////////////////////////////////////////////////////////////
   //                      FEEDBACK SURVEY                       //
   ////////////////////////////////////////////////////////////////    
   if ( SurveyType == SurveyType.Feedback ) { %>
    <% if ( !Data["Q5"].Equals("I do not want to be contacted") ) { %>
    <p>Email Given: <%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( Data["TrackingEmail"].ToString() ) %></p>
    <% } %>
    <% if ( !Data["StaffContact"].Equals( DBNull.Value ) ) { %>
    <p>Staff Contact: <%= Data["StaffContact"] %></p>
    <% } %>
    <p>Please select which property you would like to provide feedback for: <%= ReportingTools.CleanData( Data["PropertyName"] ) %></p>
    
    <p class="question">1. What type of feedback do you want to give?</p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q1"] ) %>
    </p>
    
    <p class="question">2. What part of our business does this relate to?</p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q2"] ) %>
    </p>
    
    <p class="question">3. Please provide your feedback or the details of your request below.</p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q3"] ) %>
    </p>
    
    <p class="question">4. Do you have a(n) <%= PropertyTools.GetPlayersClubName(PropertyID) %> card? Please provide your <%= PropertyTools.GetPlayersClubName(PropertyID) %> number so that we may better serve you.</p>
    <p class="answer">
        <%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( Data["Q4"].ToString() ) %>
    </p>

    <p class="question">5. Would you like someone to contact you about your feedback? If so, please tell us your preferred method of contact:</p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q5"] ) %>
    </p>

    <% if ( !Data["Q5"].Equals("I do not want to be contacted") ) { %>
    <p class="question">Please provide us your contact details. You will only be contacted with respect to your feedback.</p>
    <p class="answer">
        Name: <%= ReportingTools.CleanData( Data["Name"] ) %><br />
        Contact Email: <%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( Data["ContactEmail"].ToString() ) %><br />
        Telephone Number: <%= ReportingTools.CleanData( Data["TelephoneNumber"] ) %><br />
    </p>
    <% } %>
<% } else 
   ////////////////////////////////////////////////////////////////
   //                      DONATION SURVEY                       //
   ////////////////////////////////////////////////////////////////    
   if ( SurveyType == SurveyType.Donation ) { %>
    <p class="question">1. Name of organization/charity:</p>
    <p class="answer">
        <%= ReportingTools.CleanData( Data["Q1"] ) %>
    </p>

	<p class="question">2. Is your organization a registered charity?</p>
	<p class="answer">
		<%= GetAnswerValue( Data["Q2"], ANSWERS_YESNO, true ) %>
	</p>
	<p class="question">3. If Yes, please provide the charity license number:</p>
	<p class="answer">
		<%= ReportingTools.CleanData( Data["Q3"] ) %>
	</p>
	<p class="question">4. If No, what kind of organization are you? (School PAC, Nor-for-profit Org, Community Group, etc.)</p>
	<p class="answer">
		<%= ReportingTools.CleanData( Data["Q4"] ) %>
	</p>
	<p class="question">5. Contact Information:</p>
	<p class="answer">
		Contact Name: <%= ReportingTools.CleanData( Data["Q5Name"] ) %><br />
        Contact Title: <%= ReportingTools.CleanData( Data["Q5Title"] ) %><br />
        Contact Telephone Number: <%= ReportingTools.CleanData( Data["Q5Telephone"] ) %><br />
        Contact Email Address: <%= GCC_Web_Portal.RespondentDetails.GenerateRespondentDetailsLink( Data["Q5Email"].ToString() ) %><br />
	</p>
	<p class="question">6. Mailing address of organization / charity:</p>
	<p class="answer">
		Street address: <%= ReportingTools.CleanData( Data["Q6Street"] ) %><br />
        City / Town: <%= ReportingTools.CleanData( Data["Q6City"] ) %><br />
        Province: <%= ReportingTools.CleanData( Data["Q6Province"] ) %><br />
        Postal Code: <%= ReportingTools.CleanData( Data["Q6PostalCode"] ) %><br />
	</p>
	<p class="question">7. Main telephone number for organization / charity</p>
	<p class="answer">
		<%= ReportingTools.CleanData( Data["Q7"] ) %>
	</p>
	<p class="question">8. Website link (if applicable):</p>
	<p class="answer">
		<%= ReportingTools.CleanData( Data["Q8"] ) %>
	</p>
	<p class="question">9. Are you seeking:</p>
	<p class="answer">
		<%= ReportingTools.CleanData( Data["Q9"] ) %>
	</p>
	<p class="question">10. Please specify the amount or value of your request:</p>
	<p class="answer">
		<%= ReportingTools.CleanData( Data["Q10"] ) %>
	</p>
	<p class="question">11. Have you received a donation from Great Canadian Gaming Corporation or any of the following properties in the past? Do you have a current request in at other properties?<br />Please indicate which ones:</p>
	<div class="answer">
        <table class="table table-bordered table-striped">
            <colgroup>
                <col style="width:60%" />
            </colgroup>
			<thead>
				<tr>
					<th></th>
					<th class="text-center">Past Support</th>
					<th class="text-center">Current Request</th>
				</tr>
			</thead>
			<tbody>
				<tr>
					<th>River Rock Casino Resort (Richmond, BC)</th>
					<td class="text-center"><%= Data["Q11A_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11A_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
				</tr>
				<tr>
					<th>Hard Rock Casino Vancouver (Coquitlam, BC)</th>
					<td class="text-center"><%= Data["Q11B_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11B_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
				</tr>
				<tr>
					<th>Hastings Racecourse and Slots (Vancouver, BC)</th>
					<td class="text-center"><%= Data["Q11C_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11C_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
				</tr>
				<%--<tr>
					<th>Fraser Downs Racetrack and Casino (Surrey, BC)</th>
					<td class="text-center"><%= Data["Q11D_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11D_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
				</tr>--%>
				<tr>
					<th>View Royal Casino (Victoria, BC)</th>
					<td class="text-center"><%= Data["Q11E_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11E_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
				</tr>
				<tr>
					<th>Nanaimo Casino (Nanaimo, BC)</th>
					<td class="text-center"><%= Data["Q11F_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11F_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
				</tr>
				<tr>
					<th>Chances Dawson Creek (Dawson Creek, BC)</th>
					<td class="text-center"><%= Data["Q11G_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11G_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
				</tr>
				<tr>
					<th>Maple Ridge Community Gaming Centre (Maple Ridge, BC)</th>
					<td class="text-center"><%= Data["Q11H_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11H_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
				</tr>
				<tr>
					<th>Chilliwack Bingo (Chilliwack, BC)</th>
					<td class="text-center"><%= Data["Q11I_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11I_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
				</tr>
				<tr>
					<th>Corporate Donation – Head Office (Richmond, BC)</th>
					<td class="text-center"><%= Data["Q11J_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11J_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
				</tr>
                <tr>
                    <th>Shorelines Slots at Kawartha Downs (Fraserville, ON)</th>
					<td class="text-center"><%= Data["Q11K_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11K_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
                </tr>
                <tr>
                    <th>Shorelines Casino Thousand Islands (Gananoque, ON)</th>
					<td class="text-center"><%= Data["Q11L_PastSupport"].Equals( 1 ) ? "Yes" : "No" %></td>
					<td class="text-center"><%= Data["Q11L_CurrentRequest"].Equals( 1 ) ? "Yes" : "No" %></td>
                </tr>
			</tbody>
        </table>
    </div>


	<p class="question">12. When & what type of support did you receive in the past or what support request do you already have under consideration, if any?</p>
	<p class="answer">
		<%= ReportingTools.CleanData( Data["Q12"] ) %>
	</p>
	<p class="question">13. Please provide a brief description of your organization:</p>
	<p class="answer">
		<%= ReportingTools.CleanData( Data["Q13"] ) %>
	</p>
	<p class="question">14. Please provide a brief description of the event/program for which you are seeking a donation/sponsorship:</p>
	<p class="answer">
		<%= ReportingTools.CleanData( Data["Q14"] ) %>
	</p>
<% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bottomScripts" runat="server">
</asp:Content>
