<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveySnapshot2017.aspx.cs" Inherits="GCC_Web_Portal.SurveySnapshot2017" MasterPageFile="~/Survey.Master" %>
<%@ MasterType VirtualPath="~/Survey.Master" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/QuestionRowControl.ascx" TagPrefix="uc1" TagName="QuestionRowControl" %>
<%@ Register Src="~/Controls/ScaleQuestionControl.ascx" TagPrefix="uc1" TagName="ScaleQuestionControl" %>
<%@ Register Src="~/Controls/SixQuestionRowControl.ascx" TagPrefix="uc1" TagName="SixQuestionRowControl" %>
<%@ Register Src="~/Controls/SixScaleQuestionControl.ascx" TagPrefix="uc1" TagName="SixScaleQuestionControl" %>
<%@ Register Src="~/Controls/YesNoControl.ascx" TagPrefix="uc1" TagName="YesNoControl" %>
<%@ Register Src="~/Controls/TriQuestionRowControl.ascx" TagPrefix="uc1" TagName="TriQuestionRowControl" %>
<%@ Register Src="~/Controls/SurveyProgressBar.ascx" TagPrefix="uc1" TagName="SurveyProgressBar" %>


<asp:Content runat="server" ContentPlaceHolderID="headContent">
    <style>
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="mainContent">
    <h2>Great Canadian's Snapshot Survey&mdash;2017</h2>
    <% if ( DateTime.Now < ReportingTools.AdjustDateTime( new DateTime( 2017, 10, 12, 23, 59, 59 ), true, "Pacific Standard Time" ) ) { %>
    <p>Thank you for visiting. The Great Canadian Snapshot 2017 survey is not yet open.</p>
    <p>It will be available between <strong>12AM PDT on October 12, 2017 and 11:59 PM October 26, 2017.</strong></p>
    <p>Please try back between those times</p>
    <% } else if ( DateTime.Now >= ReportingTools.AdjustDateTime( new DateTime( 2017, 10, 26, 23, 59, 59 ), true, "Pacific Standard Time" ) ) { %>
    <p>Thank you for visiting. <strong>The Great Canadian Snapshot 2017 survey is now closed</strong> and no more entries will be accepted.</p>
    <% } else { %>
    <uc1:SurveyProgressBar runat="server" ID="spbProgress" />
    <%
        //===========================================================================
        //PAGE 1 - Intro
        //===========================================================================
    if ( Master.CurrentPage == 1) {%>
    <p>Welcome to Great Canadian's annual Snapshot Survey. Every employee in each of Great Canadian's operations has the opportunity to respond – whether you have worked with us for one day or one decade or longer!</p>
    <p>The 2017 Snapshot Survey asks for your perceptions and opinions on a variety of topics regarding your work environment. Your opinion is important and your input is of great value. Your feedback helps us to evaluate how well we are meeting our Colleagues & Culture business goal: <em><b>"To build a high performance culture that rewards and recognizes excellence in service delivery, teamwork, individual growth and development, where every employee is passionately engaged in driving the success of the business."</b></em></p>
    <p>Please take a few minutes to complete this survey &ndash; as honestly as you can. All survey responses will be handled with complete confidentiality. A report of results will be prepared for your department manager and executive director/general manager, and the report will only show the summarized results for groups that have ten or more respondents. Following the Snapshot Survey, results will be shared and you can expect that your feedback will assist your site, and more specifically your department, in developing action plans to address opportunities or issues.</p>
    <p>Please respond how you personally feel &ndash; not how you think others feel. Please focus on your own experience at work. There are no wrong answers.</p>
    <p>Thank you for your valued feedback &ndash; and have a GREAT day!</p>
    <p>Please click "Next" to begin.</p>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 2 - Location
        //===========================================================================
        else if ( Master.CurrentPage == 2 ) { %>
    <% if ( QueryPropertyShortCode == GCCPropertyShortCode.GCC || ( QueryPropertyShortCode == GCCPropertyShortCode.GAG && QueryGAGLocation == GLocation.None ) ) { %>
    <p class="question">I work at (name of your location):</p>
    <sc:SurveyDropDown runat="server" ID="fbkProperty" SessionKey="SelectedPropertyID" DBColumn="SelectedPropertyID">
        <asp:ListItem Text="--" Value="" ></asp:ListItem>
        <%--<asp:ListItem Value="21" Text="Bingo Esquimalt"></asp:ListItem>             
        <asp:ListItem Value="7" Text="Casino Nanaimo"></asp:ListItem>
        <asp:ListItem Value="19" Text="Casino New Brunswick"></asp:ListItem>
        <asp:ListItem Value="11" Text="Casino Nova Scotia - Halifax"></asp:ListItem>
        <asp:ListItem Value="12" Text="Casino Nova Scotia - Sydney"></asp:ListItem>
        <asp:ListItem Value="8" Text="Chances Chilliwack"></asp:ListItem>
        <asp:ListItem Value="10" Text="Chances Dawson Creek"></asp:ListItem>
        <asp:ListItem Value="9" Text="Chances Maple Ridge"></asp:ListItem>
        <asp:ListItem Value="14" Text="Elements Casino"></asp:ListItem>
        <asp:ListItem Value="15" Text="Flamboro Downs"></asp:ListItem>
        <asp:ListItem Value="1" Text="GCGC Corporate Office(s) - BC"></asp:ListItem>
        <asp:ListItem Value="16" Text="Georgian Downs"></asp:ListItem>  
        <asp:ListItem Value="13-1" Text="Great American Casino - Everett"></asp:ListItem>
        <asp:ListItem Value="13-2" Text="Great American Casino - Lakewood"></asp:ListItem>
        <asp:ListItem Value="13-3" Text="Great American Casino - Tukwila"></asp:ListItem>     
        <asp:ListItem Value="13-4" Text="Great American Casino - Des Moines"></asp:ListItem>     
        <asp:ListItem Value="3" Text="Hard Rock Casino Vancouver"></asp:ListItem>
        <asp:ListItem Value="5" Text="Hastings Racetrack & Casino"></asp:ListItem>
        --%>
        <asp:ListItem Value="2" Text="River Rock Casino Resort"></asp:ListItem>
<%--        <asp:ListItem Value="17" Text="Shorelines Slots at Kawartha Downs"></asp:ListItem>
        <asp:ListItem Value="18" Text="Shorelines Casino Thousand Islands"></asp:ListItem>
        <asp:ListItem Value="20" Text="Shorelines Casino Belleville"></asp:ListItem>             
        <asp:ListItem Value="6" Text="View Royal Casino"></asp:ListItem>  --%>           
        
        
    </sc:SurveyDropDown>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Continue" />
    </div>
    <% } %>
    <% }
        //===========================================================================
        //PAGE 3 - Basic Form
        //===========================================================================
        else if ( Master.CurrentPage == 3 ) { %>
    <p class="question">My department is:</p>
    <sc:SurveyDropDown runat="server" ID="ddlDepartment" SessionKey="ddlDepartment" DBColumn="Department">
        <asp:ListItem Text="--" Value="" ></asp:ListItem>
        <asp:ListItem Text="All Employees" Value="All Employees"></asp:ListItem>
		<asp:ListItem Text="Accounting / Receiving / Human Resources" Value="Accounting / Receiving / Human Resources"></asp:ListItem>
		<%--<asp:ListItem Text="Administration & Miscellaneous " Value="Administration & Miscellaneous "></asp:ListItem>--%>
		<asp:ListItem Text="Banquets" Value="Banquets"></asp:ListItem>
		<asp:ListItem Text="BC Operations & Development Management" Value="BC Operations & Development Management"></asp:ListItem>
		<asp:ListItem Text="Bingo, Cage & Slots" Value="Bingo, Cage & Slots"></asp:ListItem>
		<asp:ListItem Text="Cage & Countroom" Value="Cage & Countroom"></asp:ListItem>
		<asp:ListItem Text="Cage / Count" Value="Cage / Count"></asp:ListItem>
        <asp:ListItem Text="Cage / Count, Surveillance & Security" Value="Cage / Count, Surveillance & Security"></asp:ListItem>
        <asp:ListItem Text="Cage/Countroom/Guest Services" Value="Cage/Countroom/Guest Services"></asp:ListItem>
        <asp:ListItem Text="Casino: Guest Services" Value="Casino: Guest Services"></asp:ListItem>
        <%--<asp:ListItem Text="Casino Guest Services & Entertainment & Spa" Value="Casino Guest Services & Entertainment & Spa"></asp:ListItem>--%>
        <asp:ListItem Text="Casino Guest Services including GS Manager and Marketing Coordintor" Value="Casino Guest Services including GS Manager and Marketing Coordintor"></asp:ListItem>
        <%--<asp:ListItem Text="Casino Housekeeping" Value="Casino Housekeeping"></asp:ListItem>--%>
        <asp:ListItem Text="Casino Housekeeping including Leads" Value="Casino Housekeeping Including Leads"></asp:ListItem>
		<asp:ListItem Text="Casino Operations" Value="Casino Operations"></asp:ListItem>
		<asp:ListItem Text="Corporate Support Services" Value="Corporate Support Services"></asp:ListItem>
		<asp:ListItem Text="Culinary" Value="Culinary"></asp:ListItem>
		<asp:ListItem Text="Culinary / Food & Beverage Management & Admin" Value="Culinary / Food & Beverage Management & Admin"></asp:ListItem>
		<asp:ListItem Text="Culinary / Stewarding" Value="Culinary / Stewarding"></asp:ListItem>
		<asp:ListItem Text="Executive & Senior Management" Value="Executive & Senior Management"></asp:ListItem>
        <asp:ListItem Text="Exec. Hskper, HGS Mgr, Exec Chef, Fac. Mgr, Buf. Mgr, Pub Mgr, Banq. Mgr, S&C Mgr, S&C Coord, Hot Hskping" Value="Exec. Hskper, HGS Mgr, Exec Chef, Fac. Mgr, Buf. Mgr, Pub Mgr, Banq. Mgr, S&C Mgr, S&C Coord, Hot Hskping"></asp:ListItem>

        <asp:ListItem Text="Facilities" Value="Facilities"></asp:ListItem>
        <asp:ListItem Text="Facilities / Maintenance" Value="Facilities / Maintenance"></asp:ListItem>
		<asp:ListItem Text="Finance & Accounting" Value="Finance & Accounting"></asp:ListItem>
        <asp:ListItem Text="Finance /Database Analyst / Human Resources/IT(TSG)" Value="Finance /Database Analyst / Human Resources/IT(TSG)"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage" Value="Food & Beverage"></asp:ListItem>
        <asp:ListItem Text="Food & Beverage Culinary" Value="Food & Beverage Culinary"></asp:ListItem>
        <asp:ListItem Text="Food & Beverage Buffet including Buffet Sups" Value="Food & Beverage Buffet including Buffet Sups"></asp:ListItem>
        <asp:ListItem Text="Food & Beverage Pub & Beverage including Pub Sups and Bev Hostesses" Value="Food & Beverage Pub & Beverage including Pub Sups and Bev Hostesses"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Outlets" Value="Food & Beverage: Outlets"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Banquets" Value="Food & Beverage: Banquets"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Beverage" Value="Food & Beverage: Beverage"></asp:ListItem>
		<asp:ListItem Text="Food & Beverage: Restaurants" Value="Food & Beverage: Restaurants"></asp:ListItem>
        <asp:ListItem Text="Food & Beverage: Gaming" Value="Food & Beverage: Gaming"></asp:ListItem>
        <asp:ListItem Text="Food & Beverage: Non gaming" Value="Food & Beverage: Non gaming"></asp:ListItem>
		<asp:ListItem Text="Gaming Operations: Bingo, Slots and Cage" Value="Gaming Operations: Bingo, Slots and Cage"></asp:ListItem>
        <asp:ListItem Text="Gaming Operations: Bingo, F&B" Value="Gaming Operations: Bingo, F&B"></asp:ListItem>
		<asp:ListItem Text="Guest Services" Value="Guest Services"></asp:ListItem>
		<asp:ListItem Text="Guest Services & Retail" Value="Guest Services & Retail"></asp:ListItem>
		<asp:ListItem Text="Guest Services & Slots" Value="Guest Services & Slots"></asp:ListItem>
		<asp:ListItem Text="Guest Services/Slots" Value="Guest Services/Slots"></asp:ListItem>
        <%--<asp:ListItem Text="Hotel Ops Mrgs & Sups, Sales & Catering, Fac. Lead, Hotel Hskping Lead" Value="Hotel Ops Mrgs & Sups, Sales & Catering, Fac. Lead, Hotel Hskping Lead"></asp:ListItem>--%>
		<asp:ListItem Text="Hotel: Reservations, Front Office, Concierge and Guest Services" Value="Hotel: Reservations, Front Office, Concierge and Guest Services"></asp:ListItem>
        
        <asp:ListItem Text="Hotel: Res., Concierge and GS, Night Audit, GS Supervisor" Value="Hotel: Res., Concierge and GS, Night Audit, GS Supervisor"></asp:ListItem>
        <asp:ListItem Text="Housekeeping" Value="Housekeeping"></asp:ListItem>		
        <%--<asp:ListItem Text="Hotel Housekeeping" Value="Hotel Housekeeping"></asp:ListItem>--%>
        <asp:ListItem Text="Hotel Housekeeping including Lead and supervisor" Value="Hotel Housekeeping including Lead and supervisor"></asp:ListItem>
		<asp:ListItem Text="Housekeeping/Maintenance/Facilities" Value="Housekeeping/Maintenance/Facilities"></asp:ListItem>
		<asp:ListItem Text="HPI & Customer Service" Value="HPI & Customer Service"></asp:ListItem>
		<asp:ListItem Text="Human Resources & Payroll" Value="Human Resources & Payroll"></asp:ListItem>
        <asp:ListItem Text="Human Resources, Accounting, Maintenance (Property)" Value="Human Resources, Accounting, Maintenance (Property)"></asp:ListItem>
        <asp:ListItem Text="Janitorial & Maintenance" Value="Janitorial & Maintenance"></asp:ListItem>
        
        <%--<asp:ListItem Text="Leadership / Administrative Assistant" Value="Leadership / Administrative Assistant"></asp:ListItem>--%>
        <asp:ListItem Text="Leadership, Admin Assist Mktg Mgr, Cage Mgr, Spa Mgr, Asian Host, Receiving, IT Mgr, Sec. Mgr, db Mgr, Ent." Value="Leadership, Admin Assist Mktg Mgr, Cage Mgr, Spa Mgr, Asian Host, Receiving, IT Mgr, Sec. Mgr, db Mgr, Ent."></asp:ListItem>
        <asp:ListItem Text="Marketing & Player Development" Value="Marketing & Player Development"></asp:ListItem>
		<asp:ListItem Text="Mutuels" Value="Mutuels"></asp:ListItem>
		<asp:ListItem Text="Mutuels, Security & Facilities," Value="Mutuels, Security & Facilities"></asp:ListItem>
        <asp:ListItem Text="Mutuels & Racing" Value="Mutuels & Racing"></asp:ListItem>
		
        <asp:ListItem Text="Operations Management" Value="Operations Management"></asp:ListItem>
        <asp:ListItem Text="Operations Management/Admin" Value="Operations Management/Admin"></asp:ListItem>
        <asp:ListItem Text="Operations Management (HR, Audit, IT, Managers)" Value="Operations Management (HR, Audit, IT, Managers)"></asp:ListItem>
		<asp:ListItem Text="Operations Management & Marketing" Value="Operations Management & Marketing"></asp:ListItem>
        <asp:ListItem Text="Operations Support" Value="Operations Support"></asp:ListItem>
        <asp:ListItem Text="Operations Support (HR/Audit/IT)" Value="Operations Support (HR/Audit/IT)"></asp:ListItem>
		<asp:ListItem Text="Operations Support & Specialists" Value="Operations Support & Specialists"></asp:ListItem>
		<asp:ListItem Text="Ops Management & Department Heads" Value="Ops Management & Department Heads"></asp:ListItem>
		<%--<asp:ListItem Text="Property / Janitorial" Value="Property / Janitorial"></asp:ListItem>--%>
		<asp:ListItem Text="Property Services" Value="Property Services"></asp:ListItem>
		<asp:ListItem Text="Racing and First Aid" Value="Racing and First Aid"></asp:ListItem>
		<%--<asp:ListItem Text="Racing/Race Office" Value="Racing/Race Office"></asp:ListItem>--%>
		<asp:ListItem Text="Resort Management " Value="Resort Management "></asp:ListItem>
        
		<asp:ListItem Text="Sales, Marketing & Player Relations" Value="Sales, Marketing & Player Relations"></asp:ListItem>
        <asp:ListItem Text="Sales, Marketing, Buyer" Value="Sales, Marketing, Buyer"></asp:ListItem>
		<asp:ListItem Text="Security" Value="Security"></asp:ListItem>
		<%--<asp:ListItem Text="Security / Surveillance" Value="Security / Surveillance"></asp:ListItem>--%>
        <asp:ListItem Text="Security (Incl. Event Safety Officers)" Value="Security (Incl. Event Safety Officers)"></asp:ListItem>
		<asp:ListItem Text="Senior Management" Value="Senior Management"></asp:ListItem>
        <asp:ListItem Text="Senior Management (includes DHs)" Value="Senior Management (includes DHs)"></asp:ListItem>
        <asp:ListItem Text="Slot Operations" Value="Slot Operations"></asp:ListItem>
		<asp:ListItem Text="Slots" Value="Slots"></asp:ListItem>
        <asp:ListItem Text="Slots (Includes Slot Techs)" Value="Slots (Includes Slot Techs)"></asp:ListItem>
		<asp:ListItem Text="Slots & Guest Services" Value="Slots & Guest Services"></asp:ListItem>
        <asp:ListItem Text="Spa Aestaticians and Lead" Value="Spa Aestaticians and Lead"></asp:ListItem>
		<asp:ListItem Text="Stewarding" Value="Stewarding"></asp:ListItem>
		<asp:ListItem Text="Surveillance" Value="Surveillance"></asp:ListItem>
		<asp:ListItem Text="Table Games" Value="Table Games"></asp:ListItem>
		<asp:ListItem Text="Table Games & Customer Loyalty" Value="Table Games & Customer Loyalty"></asp:ListItem>
        <asp:ListItem Text="Table Games: Dealers" Value="Table Games: Dealers"></asp:ListItem>
        <asp:ListItem Text="Table Games: Dealers & Dual Supervisors" Value="Table Games: Dealers & Dual Supervisors"></asp:ListItem>
        <asp:ListItem Text="Table Games: Dealer Supervisors" Value="Table Games: Dealer Supervisors"></asp:ListItem>
		<asp:ListItem Text="Table Games Management" Value="Table Games Management"></asp:ListItem>
        <%--<asp:ListItem Text="Table Games Management & Slot Supervisors" Value="Table Games Management & Slot Supervisors"></asp:ListItem>--%>
        <asp:ListItem Text="Table Games Management including CSMs and Pit/Dual Pit Managers, Full Table Sups" Value="Table Games Management including CSMs and Pit/Dual Pit Managers, Full Table Sups"></asp:ListItem>
        <asp:ListItem Text="Table Games Sups/PitMan/CSM" Value="Table Games Sups/PitMan/CSM"></asp:ListItem>
         
		<asp:ListItem Text="Technology Services Group" Value="Technology Services Group"></asp:ListItem>
		<asp:ListItem Text="Theatre" Value="Theatre"></asp:ListItem>
		<asp:ListItem Text="Theatre: Box Office, Ushers" Value="Theatre: Box Office, Ushers"></asp:ListItem>
    </sc:SurveyDropDown>

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 4 - Main Form
        //===========================================================================
        else if ( Master.CurrentPage == 4 ) { %>
    <p class="question">I know what is expected of me at work.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q1" SessionKey="Q1" DBColumn="Q1" />

    <p class="question">I have the materials and equipment to do my job right.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q2" SessionKey="Q2" DBColumn="Q2" />

    <p class="question">In the last 7 days, I have received recognition or praise for doing good work.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q3" SessionKey="Q3" DBColumn="Q3" />

    <p class="question">My supervisor or someone at work seems to care about me as a person.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q4" SessionKey="Q4" DBColumn="Q4" />

    <p class="question">Someone at work encourages my development.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q5" SessionKey="Q5" DBColumn="Q5" />

    <p class="question">At work, my opinions seem to count.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q6" SessionKey="Q6" DBColumn="Q6" />

    <p class="question">Our Vision and Mission makes me feel that my job is important.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q7" SessionKey="Q7" DBColumn="Q7" />

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 5 - Main Form
        //===========================================================================
        else if ( Master.CurrentPage == 5 ) { %>

    <p class="question">My co-workers are committed to doing quality work.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q8" SessionKey="Q8" DBColumn="Q8" />

    <p class="question">I have a trusted [best] friend at work.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q9" SessionKey="Q9" DBColumn="Q9" />

    <p class="question">In the last 12 months, I have received a written Performance Review.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q10" SessionKey="Q10" DBColumn="Q10" />

    <p class="question">This last year, I had opportunities at work to learn and grow.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q11" SessionKey="Q11" DBColumn="Q11" />

    <p class="question">My direct supervisor keeps me informed about matters that affect me.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q12" SessionKey="Q12" DBColumn="Q12" />

    <p class="question">I know who to speak with to have my questions answered.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q13" SessionKey="Q13" DBColumn="Q13" />

    <p class="question">My requests for information or assistance are addressed promptly.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q14" SessionKey="Q14" DBColumn="Q14" />

    <p class="question">I am happy to be working here at <%= Master.CasinoName %>.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q15" SessionKey="Q15" DBColumn="Q15" />

    <p class="question">I would, without hesitation, recommend my workplace to a friend seeking employment.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q16" SessionKey="Q16" DBColumn="Q16" />

    <p class="question">Given the opportunity, I tell others great things about working here.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q17" SessionKey="Q17" DBColumn="Q17" />
    
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 6 - Main Form
        //===========================================================================
        else if ( Master.CurrentPage == 6 ) { %>

    <p class="question">It would take a lot to get me to leave my job.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q18" SessionKey="Q18" DBColumn="Q18" />

    <p class="question">I rarely think about leaving my workplace to work somewhere else.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q19" SessionKey="Q19" DBColumn="Q19" />

    <p class="question">My workplace inspires me to do my best work every day.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q20" SessionKey="Q20" DBColumn="Q20" />

    <p class="question">My workplace motivates me to contribute more than is normally required to complete my work.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q21" SessionKey="Q21" DBColumn="Q21" />

    <p class="question">My management has acted on results from previous surveys.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q22" SessionKey="Q22" DBColumn="Q22" />

    <p class="question">I received the training I need to do my job well.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q23" SessionKey="Q23" DBColumn="Q23" />
    
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 7 - Main Form
        //===========================================================================
        else if ( Master.CurrentPage == 7 ) { %>

    <p class="question">Our GEM Recognition Program acknowledges me and my colleagues in a positive way.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q24" SessionKey="Q24" DBColumn="Q24" />

    <p class="question">I believe in the Company's Values and practice them daily at work.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q25" SessionKey="Q25" DBColumn="Q25" />

    <p class="question">I appreciate the opportunity to have one-on-one discussions with my manager.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q26" SessionKey="Q26" DBColumn="Q26" />

    <p class="question">My manager is effective in providing performance feedback and coaching.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q27" SessionKey="Q27" DBColumn="Q27" />

    <p class="question">We have a respectful workplace that is open, values diversity, and accepts individual differences (e.g. gender, race, ethnicity, sexual orientation, religion, age).</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q28" SessionKey="Q28" DBColumn="Q28" />

    <p class="question">In our organization, we are:</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Strongly Agree</div>
        <div class="col-md-1 col-xs-2 title">Agree</div>
        <div class="col-md-1 col-xs-2 title">Slightly Agree</div>
        <div class="col-md-1 col-xs-2 title">Slightly Disagree</div>
        <div class="col-md-1 col-xs-2 title">Disagree</div>
        <div class="col-md-1 col-xs-2 title">Strongly Disagree</div>
    </div>
    <uc1:SixQuestionRowControl runat="server" ID="Q29A" SessionKey="Q29A" DBColumn="Q29A" Label="Hiring the people we need to be successful today and in the future." />
    <uc1:SixQuestionRowControl runat="server" ID="Q29B" SessionKey="Q29B" DBColumn="Q29B" Label="Keeping the people we need to be successful today and in the future." />
    <uc1:SixQuestionRowControl runat="server" ID="Q29C" SessionKey="Q29C" DBColumn="Q29C" Label="Promoting the people who are best equipped to help us be successful today and in the future." />

    <p class="question">I know what Great <%= Master.PropertyShortCode == GCCPropertyShortCode.GAG ? "American" : "Canadian" %> stands for and what makes our company different and better than the rest.</p>
    <uc1:SixScaleQuestionControl runat="server" ID="Q30" SessionKey="Q30" DBColumn="Q30" />
    
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 8 - Demographics
        //===========================================================================
        else if ( Master.CurrentPage == 8 ) { %>
    <p class="question">I am:</p>
    <sc:MessageManager runat="server" ID="Q36Message"></sc:MessageManager>
    <sc:SurveyRadioButton ID="radQ31_Hourly" runat="server" GroupName="Q31" SessionKey="Q31_Hourly" DBColumn="Q31" DBValue="Hourly" Text="&nbsp;Hourly" /><br />
    <sc:SurveyRadioButton ID="radQ31_Salary" runat="server" GroupName="Q31" SessionKey="Q31_Salary" DBColumn="Q31" DBValue="Salary" Text="&nbsp;Salary" /><br />

    <p class="question">My length of service is:</p>
    <sc:SurveyRadioButton ID="radQ32_1" runat="server" GroupName="Q32" SessionKey="Q32_1" DBColumn="Q32" DBValue="0-1 year" Text="&nbsp;0-1 year" /><br />
    <sc:SurveyRadioButton ID="radQ32_2" runat="server" GroupName="Q32" SessionKey="Q32_2" DBColumn="Q32" DBValue="1-3 years" Text="&nbsp;1-3 years" /><br />
    <sc:SurveyRadioButton ID="radQ32_3" runat="server" GroupName="Q32" SessionKey="Q32_3" DBColumn="Q32" DBValue="3-5 years" Text="&nbsp;3-5 years" /><br />
    <sc:SurveyRadioButton ID="radQ32_4" runat="server" GroupName="Q32" SessionKey="Q32_4" DBColumn="Q32" DBValue="5-9 years" Text="&nbsp;5-9 years" /><br />
    <sc:SurveyRadioButton ID="radQ32_5" runat="server" GroupName="Q32" SessionKey="Q32_5" DBColumn="Q32" DBValue="10 years+" Text="&nbsp;10 years+" /><br />

    <p class="question">In your own words... please take this opportunity to tell us a bit more about your experience at work and provide your comments (either positive or not so positive) that will help to improve your experience at work.</p>
    <div class="text-center">
        <sc:SurveyTextBox ID="Q33" SessionKey="Q33" DBColumn="Q33" runat="server" TextMode="MultiLine" Rows="5" Style="width:95%;" MaxLength="1000"></sc:SurveyTextBox>
    </div>

    <p class="question">To help us group similar comments together, please select the topic area(s) that best fit the comments you made: (Please select all that apply)</p>
    <sc:SurveyCheckBox ID="chkQ34_1" runat="server" SessionKey="Q34_1" DBColumn="Q34_WorkEnvironment" DBValue="1" Text="&nbsp;Your Work Environment" /><br />
    <sc:SurveyCheckBox ID="chkQ34_2" runat="server" SessionKey="Q34_2" DBColumn="Q34_PeopleYouWorkWith" DBValue="1" Text="&nbsp;The People You Work With" /><br />
    <sc:SurveyCheckBox ID="chkQ34_3" runat="server" SessionKey="Q34_3" DBColumn="Q34_YourManager" DBValue="1" Text="&nbsp;Your Manager" /><br />
    <sc:SurveyCheckBox ID="chkQ34_4" runat="server" SessionKey="Q34_4" DBColumn="Q34_Leadership" DBValue="1" Text="&nbsp;Leadership" /><br />
    <sc:SurveyCheckBox ID="chkQ34_5" runat="server" SessionKey="Q34_5" DBColumn="Q34_WorkProcessesResources" DBValue="1" Text="&nbsp;Work Processes/Resources" /><br />
    <sc:SurveyCheckBox ID="chkQ34_6" runat="server" SessionKey="Q34_6" DBColumn="Q34_CorporateSocialResponsibility" DBValue="1" Text="&nbsp;Corporate Social Responsibility" /><br />
    <sc:SurveyCheckBox ID="chkQ34_7" runat="server" SessionKey="Q34_7" DBColumn="Q34_ManagingPerformance" DBValue="1" Text="&nbsp;Managing Performance" /><br />
    <sc:SurveyCheckBox ID="chkQ34_8" runat="server" SessionKey="Q34_8" DBColumn="Q34_Benefits" DBValue="1" Text="&nbsp;Benefits" /><br />
    <sc:SurveyCheckBox ID="chkQ34_9" runat="server" SessionKey="Q34_9" DBColumn="Q34_WorkLifeBalance" DBValue="1" Text="&nbsp;Work/Life Balance" /><br />
    <sc:SurveyCheckBox ID="chkQ34_10" runat="server" SessionKey="Q34_10" DBColumn="Q34_CareerDevelopmentOpportunities" DBValue="1" Text="&nbsp;Career and Development Opportunities" /><br />
    <sc:SurveyCheckBox ID="chkQ34_11" runat="server" SessionKey="Q34_11" DBColumn="Q34_PayRecognition" DBValue="1" Text="&nbsp;Pay/Recognition" /><br />
    <sc:SurveyCheckBox ID="chkQ34_12" runat="server" SessionKey="Q34_12" DBColumn="Q34_EmployerInGeneral" DBValue="1" Text="&nbsp;Your Employer in General" /><br />

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <%--  %><% }
        //===========================================================================
        //PAGE 9 - First Final Confirmation
        //===========================================================================
        else if ( Master.CurrentPage == 9 ) { %>
    <p>Thank you again for responding to the 2017 Snapshot Survey &mdash; we value your input.</p>
    <p>But wait, there's more... if you have 5 more minutes, we would like to ask you some additional questions about community outreach and responsible gaming.</p>
    <sc:SurveyRadioButton ID="radFFNoThanks" runat="server" GroupName="QFF" SessionKey="FFNoThanks" Text="&nbsp;No Thanks" /><br />
    <sc:SurveyRadioButton ID="radFFContinue" runat="server" GroupName="QFF" SessionKey="FFContinue" Text="&nbsp;Continue" /><br />
    
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }--%>


    <% }
        //===========================================================================
        //PAGE 9 - First Final Confirmation
        //===========================================================================
        else if ( Master.CurrentPage == 9 ) { %>
    <p>Thank you for completing the first part of the 2017 Snapshot Survey. &mdash;</p>
    <p>But wait, It’s not over! If you have time, please answer the next set of questions which focus on the PROUD program and our responsible gambling practices.</p>
<sc:SurveyRadioButton ID="radFFContinue" runat="server" GroupName="QFF" SessionKey="FFContinue" Text="&nbsp;Sure! Lets Do it!" /><br />
    <sc:SurveyRadioButton ID="radFFNoThanks" runat="server" GroupName="QFF" SessionKey="FFNoThanks" Text="&nbsp;Sorry, I don't have time" /><br />
    
    
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }


        //===========================================================================
        //PAGE 10 - CSR
        //===========================================================================
        else if ( Master.CurrentPage == 10 ) { %>
    <p>WELCOME to our quick survey about corporate social responsibility.</p>
    <p>Great Canadian Gaming Corporation actively supports the communities in which we live and work.   We're proud of our approach to corporate social responsibility; and we're proud of your involvement in making it happen!</p>
    <p>Two very important elements of our corporate social responsibility are community outreach and responsible gaming. </p>
    <p>COMMUNITY  OUTREACH &ndash; Last year, Great Canadian supported over 1,500 non-profit organizations affiliated with our properties across Canada, and our team members support dozens of local groups through countless volunteering hours.   Through our "We're Proud" Volunteer Recognition Program, Great Canadian recognizes, encourages and most importantly rewards the efforts of our colleagues who volunteer and give back to their communities.  With over 5,000 employees, we're able to connect and support initiatives and programs that are important to many Canadians.  This, in addition to the hundreds of events and fundraisers we support in various ways, allows us to demonstrate our commitment to our corporate value of Citizenship.</p>
    <p>RESPONSIBLE GAMING &ndash; Great Canadian is committed to providing our guests with a positive and safe gaming experience.   While the majority of our guests enjoy gaming as a form of entertainment, a small percentage may experience problems.   Our approach to Responsible Gaming incorporates training of our team members, encouraging customer awareness, as well as working within our local communities and provincial authorities to educate and create awareness around responsible gaming practices.</p>
    <p>THANK YOU for taking a few minutes to respond to these questions.  Your responses will tell us how you feel about our PROUD program and what you know or want to know about Responsible Gaming.  Your responses are valuable and the information you provide will help us to improve and refine these important corporate social responsibility programs.</p>

    <p class="question">I am aware of the PROUD program</p>
    <uc1:YesNoControl ID="CSR_Q1" runat="server" SessionKey="CSR_Q1" DBColumn="CSR_Q1" />
    
    <p class="question">I am aware that I can apply for financial support for my charity through the "We're Proud" Volunteer Recognition Program and for my volunteering efforts</p>
    <uc1:YesNoControl ID="CSR_Q2" runat="server" SessionKey="CSR_Q2" DBColumn="CSR_Q2" />

    <p class="question">I would describe our Company's commitment to community outreach and support as:</p>
    <uc1:ScaleQuestionControl runat="server" ID="CSR_Q3" SessionKey="CSR_Q3" DBColumn="CSR_Q3" />

    <p class="question">The "We're Proud" Volunteer Recognition Program is:</p>
    <div class="row grid-header">
        <div class="col-md-2 col-xs-2 title">Very important to me</div>
        <div class="col-md-2 col-xs-2 title">Somewhat important to me</div>
        <div class="col-md-2 col-xs-2 title">Not important to me</div>
    </div>
    <uc1:TriQuestionRowControl runat="server" ID="CSR_Q4" SessionKey="CSR_Q4" DBColumn="CSR_Q4" />

    <p class="question">I am aware of the following PROUD programs: (Please check all that apply)</p>
    <sc:SurveyCheckBox ID="CSR_Q5_1" runat="server" SessionKey="CSR_Q5_1" DBColumn="CSR_Q5_Challenge" DBValue="1" Text="&nbsp;PROUD Challenge" /><br />
    <sc:SurveyCheckBox ID="CSR_Q5_2" runat="server" SessionKey="CSR_Q5_2" DBColumn="CSR_Q5_Champion" DBValue="1" Text="&nbsp;PROUD Champion" /><br />
    <sc:SurveyCheckBox ID="CSR_Q5_3" runat="server" SessionKey="CSR_Q5_3" DBColumn="CSR_Q5_DayOfCaring" DBValue="1" Text="&nbsp;PROUD Day of Caring" /><br />
    <sc:SurveyCheckBox ID="CSR_Q5_4" runat="server" SessionKey="CSR_Q5_4" DBColumn="CSR_Q5_Scholarship" DBValue="1" Text="&nbsp;PROUD Scholarship" /><br />

    <p class="question">I would describe Great Canadian's commitment to Responsible Gaming as:</p>
    <uc1:ScaleQuestionControl runat="server" ID="CSR_Q6" SessionKey="CSR_Q6" DBColumn="CSR_Q6" />

    <p class="question">How relevant /important is Responsible Gaming to your particular role/position in the organization?</p>
    <div class="row grid-header">
        <div class="col-md-2 col-xs-2 title">Very important to me</div>
        <div class="col-md-2 col-xs-2 title">Somewhat important to me</div>
        <div class="col-md-2 col-xs-2 title">Not important to me</div>
    </div>
    <uc1:TriQuestionRowControl runat="server" ID="CSR_Q7" SessionKey="CSR_Q7" DBColumn="CSR_Q7" />

    <p class="question">I have received information about Responsible Gaming at Great Canadian through: (Please check all that apply)</p>
    <sc:SurveyCheckBox ID="CSR_Q8_1" runat="server" SessionKey="CSR_Q8_1" DBColumn="CSR_Q8_Training" DBValue="1" Text="&nbsp;Training programs" /><br />
    <sc:SurveyCheckBox ID="CSR_Q8_2" runat="server" SessionKey="CSR_Q8_2" DBColumn="CSR_Q8_StaffMeetings" DBValue="1" Text="&nbsp;Staff meetings" /><br />
    <sc:SurveyCheckBox ID="CSR_Q8_3" runat="server" SessionKey="CSR_Q8_3" DBColumn="CSR_Q8_Newsletters" DBValue="1" Text="&nbsp;Newsletters and memos" /><br />
    <sc:SurveyCheckBox ID="CSR_Q8_4" runat="server" SessionKey="CSR_Q8_4" DBColumn="CSR_Q8_Email" DBValue="1" Text="&nbsp;Email" /><br />
    <sc:SurveyCheckBox ID="CSR_Q8_5" runat="server" SessionKey="CSR_Q8_5" DBColumn="CSR_Q8_Intranet" DBValue="1" Text="&nbsp;Intranet" /><br />
    <sc:SurveyCheckBox ID="CSR_Q8_6" runat="server" SessionKey="CSR_Q8_6" DBColumn="CSR_Q8_PostersAndBrochures" DBValue="1" Text="&nbsp;Posters and/or brochures" /><br />
    <sc:SurveyCheckBox ID="CSR_Q8_7" runat="server" SessionKey="CSR_Q8_7" DBColumn="CSR_Q8_ResponsibleGamingKiosk" DBValue="1" Text="&nbsp;On-site responsible gaming kiosk" /><br />
    <sc:SurveyCheckBox ID="CSR_Q8_8" runat="server" SessionKey="CSR_Q8_8" DBColumn="CSR_Q8_PublicAdvertising" DBValue="1" Text="&nbsp;Public advertising materials (TV commercials, newspaper ads, etc.)" /><br />
    <sc:SurveyCheckBox ID="CSR_Q8_9" runat="server" SessionKey="CSR_Q8_9" DBColumn="CSR_Q8_Other" DBValue="1" Text="&nbsp;Other – Please specify" /> <sc:SurveyTextBox ID="CSR_Q8_OtherExplanation" runat="server" SessionKey="CSR_Q8_OtherExplanation" DBColumn="CSR_Q8_OtherExplanation" MaxLength="200" Size="50"></sc:SurveyTextBox><br />

    <p class="question">On a scale of Very Good to Very Poor, how would you rate your understanding of:</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">Very Good</div>
        <div class="col-md-1 col-xs-2 title">Good</div>
        <div class="col-md-1 col-xs-2 title">Moderate</div>
        <div class="col-md-1 col-xs-2 title">Poor</div>
        <div class="col-md-1 col-xs-2 title">Very Poor</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="CSR_Q9A" SessionKey="CSR_Q9A" DBColumn="CSR_Q9A" Label="Risks of gaming" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q9B" SessionKey="CSR_Q9B" DBColumn="CSR_Q9B" Label="Signs of gaming problem" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q9C" SessionKey="CSR_Q9C" DBColumn="CSR_Q9C" Label="Chances of winning and losing" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q9D" SessionKey="CSR_Q9D" DBColumn="CSR_Q9D" Label="Tips for safer gaming" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q9E" SessionKey="CSR_Q9E" DBColumn="CSR_Q9E" Label="Randomness and house advantage" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q9F" SessionKey="CSR_Q9F" DBColumn="CSR_Q9F" Label="Gaming myths" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q9G" SessionKey="CSR_Q9G" DBColumn="CSR_Q9G" Label="Responsible gaming tools" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q9H" SessionKey="CSR_Q9H" DBColumn="CSR_Q9H" Label="Available help resources" />

    <p class="question">How comfortable are you in responding to a guest/player who:</p>
    <div class="row grid-header">
        <div class="col-md-6"></div>
        <div class="col-md-1 col-xs-2 title">5<br />Very<br />Comfortable</div>
        <div class="col-md-1 col-xs-2 title">4</div>
        <div class="col-md-1 col-xs-2 title">3</div>
        <div class="col-md-1 col-xs-2 title">2</div>
        <div class="col-md-1 col-xs-2 title">1<br />Very<br />Uncomfortable</div>
    </div>
    <uc1:QuestionRowControl runat="server" ID="CSR_Q10A" SessionKey="CSR_Q10A" DBColumn="CSR_Q10A" Label="Expresses concerns about their gambling" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q10B" SessionKey="CSR_Q10B" DBColumn="CSR_Q10B" Label="Wants to self-exclude" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q10C" SessionKey="CSR_Q10C" DBColumn="CSR_Q10C" Label="Is angry or upset about how much they lost" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q10D" SessionKey="CSR_Q10D" DBColumn="CSR_Q10D" Label="Is concerned about a friend or a family member's gambling" />
    <uc1:QuestionRowControl runat="server" ID="CSR_Q10E" SessionKey="CSR_Q10E" DBColumn="CSR_Q10E" Label="Expresses their belief in a common gambling myth" />
    
    <p class="question">How informed do you feel about responsible gaming at GCGC?</p>
    <sc:SurveyRadioButton ID="radCSR_Q11_4" runat="server" GroupName="CSR_Q11" SessionKey="CSR_Q11_4" DBColumn="CSR_Q11" DBValue="Very" Text="&nbsp;Very" /><br />
    <sc:SurveyRadioButton ID="radCSR_Q11_3" runat="server" GroupName="CSR_Q11" SessionKey="CSR_Q11_3" DBColumn="CSR_Q11" DBValue="Moderately" Text="&nbsp;Moderately" /><br />
    <sc:SurveyRadioButton ID="radCSR_Q11_2" runat="server" GroupName="CSR_Q11" SessionKey="CSR_Q11_2" DBColumn="CSR_Q11" DBValue="Somewhat" Text="&nbsp;Somewhat" /><br />
    <sc:SurveyRadioButton ID="radCSR_Q11_1" runat="server" GroupName="CSR_Q11" SessionKey="CSR_Q11_1" DBColumn="CSR_Q11" DBValue="Not at all" Text="&nbsp;Not at all" /><br />


    <% if ( !( new GCCPropertyShortCode[] { GCCPropertyShortCode.GAG, GCCPropertyShortCode.FL, GCCPropertyShortCode.GD, GCCPropertyShortCode.CNB } ).Contains( Master.PropertyShortCode ) ) {  %>
    <p class="question">Are you aware of the RG Check Accreditation process?</p>
    <uc1:YesNoControl ID="CSR_Q12" runat="server" SessionKey="CSR_Q12" DBColumn="CSR_Q12" />
    <% } %>
    <p class="question">What would be helpful to enhance your knowledge of responsible gaming?   What areas of responsible gaming would you like to learn more about?</p>
    <div class="text-center">
        <sc:SurveyTextBox ID="CSR_Q13" SessionKey="CSR_Q13" DBColumn="CSR_Q13" runat="server" TextMode="MultiLine" Rows="5" Style="width:95%;" MaxLength="1000"></sc:SurveyTextBox>
    </div>

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
    </div>
    <% }
        //===========================================================================
        //PAGE 99 - Thank You
        //===========================================================================
        else if ( Master.CurrentPage == 99 ) { %>
    <sc:MessageManager runat="server" ID="mmLastPage"></sc:MessageManager>
    <div class="button-container">
    <% if (!String.IsNullOrEmpty( mmLastPage.SuccessMessage)) { %>
        <a href="<%= GetURL( 1, 0) %>" class="btn btn-success">Start Over</a>
        <% object propertySC = Page.RouteData.Values["propertyshortcode"];
           if (propertySC.ToString() == "GCC") { %>
        <a href="http://www.gcgaming.com" class="btn btn-success">Close Survey</a>
        <% } %>
    <% } else { %>
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Previous" />
    <% } %>
    </div>
    <% } %>
    <% } %>
</asp:Content>