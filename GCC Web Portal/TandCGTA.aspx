<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TandCGTA.aspx.cs" Inherits="GCC_Web_Portal.TandCGTA" %>

<%@ Import Namespace="SharedClasses" %>
<!DOCTYPE html>

<html lang="en" class="no-js">
<head runat="Server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Terms and Conditions</title>
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:BundleReference runat="server" Path="~/Content/bootstrap" />
    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
</head>
<body>
    <form runat="server">
        <div class="container">
            <div class="col-xs-12" id="main-content">
                <h1><%= CasinoName%> Guest Feedback Program</h1>
                <h3>Terms of Use</h3>
                <ul>




                    <li>To enter and be eligible to win, the entrant must answer all mandatory questions on the Guest Experience Survey.</li>
                    <li>No purchase necessary.</li>
                    <li>Selected entrant must have complied with all Contest Conditions and correctly answer a mathematical skill testing question to win a prize. </li>
                    <li>All non-winning, disqualified and/or winning entry forms and associated information will become the property of Great Canadian Gaming Corporation (GCGC).</li>
                    <li>GCGC reserves the right to cancel or suspend this contest should viruses, bugs, or other causes beyond their control corrupt the administration, security, or proper play of this contest. </li>
                    <li>If for any reason the Contest is not capable of running as planned or if the administration, security, fairness, integrity, or proper conduct of this Contest is corrupted or adversely affected by reason infection of computer virus, bugs, tampering, unauthorized intervention, fraud, technical failure, or any other cause beyond its control, GCGC reserves the right to cancel, terminate, modify or suspend the electronic portion of the Contest, including cancelling any method of entry. </li>
                    <li>GCGC may at its sole discretion amend the Contest Conditions at any time. Decisions of GCGC are final.</li>
                    <li>GCGC is not responsible for lost, misdirected, misplaced, incomplete, illegible or damaged entries, garbled or delayed computer transmissions, or entries submitted after the contest deadline. </li>
                    <li>GCGC is not responsible for telephone, technical, network, online, electronic, computer hardware or software failures, or congestion on the corporate website, of any kind.</li>
                    <li>In making the prize available to a Winning Entrant, GCGC makes no representations or warranties whatsoever either expressed or implied, oral or written, in respect of the prize.</li>
                    <li>GCGC reserves the right to substitute the prize or portion thereof with one of equal or greater value.</li>
                    <li>GCGC shall not be liable to the Winning Entrant nor to any other person for loss or damage to person, property, or computer resulting from or connected with participation in this contest, downloading any materials relating to this contest, or acts or omissions of GCGC and/or the prize supplier.</li>
                    <li>GCGC has the right to publish the name and location of the winner without remuneration.</li>


                </ul>



                <h3><%= CasinoName%> Guest Experience Survey Contest Conditions</h3>
                <p>For the month of  <%= DateTime.Now.ToString("MMMM, yyyy") %></p>

                <h4>Qualified Entry and Entrant</h4>
                <ul>
                    <li>A qualified entrant (Entrant) is someone who is 19 years of age or older and a Canadian resident. Employees of Great Canadian Gaming Corporation, its subsidiaries and affiliates, collectively referred to as "GCGC" and British Columbia Lottery Corporation (BCLC), as well as family and household members of same who reside at the same address are not eligible.</li>
                    <li>Employees of Forum Research, its subsidiaries and affiliates, as well as family and household members of the same who reside at the same address are not eligible.</li>
                    <li>Voluntary Self-excluded and Barred persons are not eligible to participate and will not be awarded a prize.</li>
                    <li>Entrant and Entry must meet all Contest Conditions as outlined below. Entry and/or Entrant not meeting Contest Conditions will be disqualified and prize will be forfeited.</li>
                </ul>
                <h4>Contest Period</h4>
                <p>The Contest Period runs from when the Guest Experience Survey's link is released to the public until the end of the same month, or on such other date and time as posted by GCGC. Each calendar month will be considered a different promotional period with its own pool of entrants and prizing. Entrants are assumed to have completed the survey and entered the contest in the same month it was released to the public. Entrants who complete the survey after the end of the month in which the survey was released will be automatically entered into the survey pool for the month in which they actually completed the survey.</p>
                <h4>Prize Draw</h4>
                <ul>
                    <li>To enter and be eligible to win, the entrant must answer all mandatory questions on the <%= CasinoName%> Guest Experience Survey.</li>
                    <li>Entries must be made directly on the Survey site provided via a link in an email sent from GCGC. Photocopies, faxes, emails or phone calls are not considered eligible entries.</li>
                    <li>Entrants must provide a valid e-mail address. First and last name of all online entrants establishes the identity of the entrant.</li>
                    <li>The name and email address of all entrants will be used to conduct the draw.</li>
                    <li>An Entrant may only enter the contest once during the promotional period. Duplicate entries will be deleted. </li>
                    <li>The use of software or other hardware for purposes of making multiple entries is prohibited and will result in disqualification.</li>
                    <li>All entries from all GCGC properties in Greater Toronto Area will be pooled together each month for the purposes of conducting the contest.</li>
                    <li>One (1) winner will be selected randomly from the pool of entrants on the 1st day of the month after the entry was received, at 1:00pm or as soon as practical thereafter. The draw process is as follows:</li>
                    <li>All entries will be collected during the Contest Period.</li>
                    <li>Upon submission, each entry will be assigned a randomly generated, fractional number between 0 and 1. For example, 0.212874.</li>
                    <li>At the end of the month, the entry with the largest randomly generated number will be selected.</li>
                    <li>Numbers are arbitrary, and are generated by a random seed number.</li>
                    <li>Random number generation is in no way influenced by any employee of GCGC.</li>
                    <li>Information collected online will be stored in a secure database only accessible by GCGC Marketing and Forum Research. This information will not be sold to or copied for any other 3rd party.</li>
                    <li>Odds of winning will depend on the number of eligible entries received before the contest deadline.</li>
                    <li>Winning Entrants will be contacted as soon as possible after the draw by GCGC through email. </li>
                    <li>Winning Entrants will have ten (10) days to claim prize, after which prize is forfeited and becomes the property of GCGC.</li>
                    <li>The grand prize winning Entrant will receive one (1) gift card valued at $100 and valid for redemption at any GCGC property in Greater Toronto Area.</li>
                    <li>There is no cash equivalent to any prize. All prizes must be accepted as awarded.</li>
                    <li>Winner must claim prize at the Guest Services desk of any GCGC properties in Greater Toronto Area. Winner is responsible for collection of his/her own prize. </li>
                    <li>Winner will be required to present valid government issued photo identification (including proof of age) and complete a winner's authorization form before prize will be released. Failure to do so will result in disqualification of the winner, and prize will become the property of GCGC.</li>
                    <li>Any intent by the Entrant to misrepresent themselves through the use of aliases and e-mail addresses will be disqualified. In the event of a dispute regarding the identity of the person submitting an electronic entry, the entry will be deemed submitted by the person whose name appears in the database.</li>
                    <li>If an Entrant is found to be ineligible, an alternate winner will be selected from the pool of all eligible entries received. Information collected by GCGC will be stored in a secure database only accessible by GCGC Head Office staff.</li>


                </ul>


                <h3>Personal Information and Privacy policy</h3>
                 <p>Your personal information is collected by Forum Research and used by <%=CasinoName %> on behalf of Great Canadian Gaming Corporation (GCGC) and Ontario Lottery and Gaming Corporation (OLG) in accordance with the Ontario Lottery and Gaming Corporation Act. Your personal information is used for the purposes of: administering this contest, for customer service research, and for Responsible Gaming research. If you require a response to your feedback, your personal information will be used to contact you and may be shared with OLG.
Your information will not be sold, shared with third parties, or used for soliciting purposes. If you have any questions about this, please write to GCGC's Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.
</p>
            </div>
        </div>
        <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see http://go.microsoft.com/fwlink/?LinkID=272931&clcid=0x409 --%>
                <%--Framework Scripts--%>

                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <%--<asp:ScriptReference Name="jquery.ui.combined" />--%>
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>
        <%: Scripts.Render("~/bundles/bootstrap") %>
    </form>
</body>
</html>
