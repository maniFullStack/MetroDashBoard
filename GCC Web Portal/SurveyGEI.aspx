<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyGEI.aspx.cs" Inherits="GCC_Web_Portal.SurveyGEI" %>

<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/QuestionRowControl.ascx" TagPrefix="uc1" TagName="QuestionRowControl" %>
<%@ Register Src="~/Controls/ScaleQuestionControl.ascx" TagPrefix="uc1" TagName="ScaleQuestionControl" %>
<%@ Register Src="~/Controls/ScaleQuestionControlFrench.ascx" TagPrefix="uc1" TagName="ScaleQuestionControlFrench" %>
<%@ Register Src="~/Controls/YesNoControl.ascx" TagPrefix="uc1" TagName="YesNoControl" %>
<%@ Register Src="~/Controls/YesNoControlFrench.ascx" TagPrefix="uc1" TagName="YesNoControlFrench" %>
<%@ Register Src="~/Controls/SurveyProgressBar.ascx" TagPrefix="uc1" TagName="SurveyProgressBar" %>
<%@ Register Src="~/Controls/TenScaleQuestionControl.ascx" TagPrefix="uc1" TagName="TenScaleQuestionControl" %>

<!DOCTYPE html>
<html lang="en" class="no-js">
<head runat="Server">
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1">

	<title><% if (strSurveyLang != "French")
			  { %>
		<%= SharedClasses.PropertyTools.GetCasinoName(PropertyID) %> - Guest Experience Survey
	<%}
			  else
			  { %>
		<%= SharedClasses.PropertyTools.GetCasinoName(PropertyID) %> - Sondage sur l'expérience des clients
	<%} %>
	</title>


	<asp:PlaceHolder runat="server">
		<%: Scripts.Render("~/bundles/modernizr") %>
	</asp:PlaceHolder>
	<webopt:BundleReference runat="server" Path="~/Content/bootstrap" />
	<webopt:BundleReference runat="server" Path="~/Content/css" />
	<link href="/Content/skins/icheck-flat/blue.css" rel="stylesheet" />
	<link href="/Content/themes/property<%= "" + PropertyID %>.css" rel="stylesheet" />
	<!--[if lt IE 9]>
	  <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
	  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
	<style>
		h1, h2, h3 {
			text-align: center;
		}




		.page {
			/*border-top:2px dotted #2c2c2c;*/
		}

		.satisfied {
			font-size: 1.2em;
			font-weight: bold;
		}

		.question {
			font-size: 1.2em;
			font-weight: bold;
			margin-top: 20px;
			border-bottom: 1px dashed #b7b7b7;
		}

		.button-container {
			text-align: center;
			margin-top: 20px;
		}

		.grid-header .title {
			font-weight: bold;
			text-align: center;
		}

		.grid-row .option {
			text-align: center;
		}

		.grid-row {
			/*border-bottom:1px dashed #b7b7b7;*/
			margin-top: 15px;
		}

		.option label {
			display: none;
		}

		.skipped {
			color: #666;
			background: #CCC;
			border: 1px solid #999;
			border-radius: 5px;
			padding: 10px 5px;
			font-size: 0.9em;
		}

		div.alert {
			text-align: left;
		}

		span.radalign {
			display: table;
		}

			span.radalign > input {
				display: table-cell;
			}

			span.radalign > label {
				display: table-cell;
				vertical-align: top;
			}

		@media (max-width:991px) {
			#header-logo {
				width: 100%;
				background-size: 100% auto;
				background-repeat: no-repeat;
			}

			.grid-header {
				display: none;
			}

			.option label {
				display: block;
			}
		}
	</style>
</head>
<body class="survey">
	<form runat="server">
		<div id="top-break"></div>
		<%--<nav class="navbar navbar-inverse navbar-fixed-top" id="survey-navigation">
			  <div class="container">
				<div class="navbar-header">
				  <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
					<span class="sr-only">Toggle navigation</span>
					<span class="icon-bar"></span>
					<span class="icon-bar"></span>
					<span class="icon-bar"></span>
				  </button>
				  <a class="navbar-brand" href="#">Project name</a>
				</div>
				<div id="navbar" class="collapse navbar-collapse">
				  <ul class="nav navbar-nav">
					<li class="active"><a href="#">Home</a></li>
					<li><a href="#about">About</a></li>
					<li><a href="#contact">Contact</a></li>
				  </ul>
				</div><!--/.nav-collapse -->
			  </div>
		  </nav>--%>
		<div class="container">
			<div class="col-xs-12" id="main-content">
				<div id="header-logo"></div>



				<h1><%= HeaderTitle %></h1>


				<%-- <% if(strSurveyLang != "French") { %>
				<h1>Guest Experience Survey</h1>
				<%} else if(strSurveyLang == "French" && PINSurveyLang.SelectedValue == "French") { %>
				<h1>Enquête sur l'expérience des clients</h1>
				<%} else { %>
				<h1>Guest Experience Survey</h1>
				<%} %>--%>
				<sc:MessageManager runat="server" ID="TopMessage"></sc:MessageManager>

				<% if (String.IsNullOrWhiteSpace(TopMessage.ErrorMessage))
				   { %>
				<uc1:SurveyProgressBar runat="server" ID="spbProgress" />
				<%
					   //===========================================================================
					   //PAGE 1 - Not Kiosk or Staff Entry
					   //===========================================================================
					   if (CurrentPage == 1 && !IsKioskOrStaffEntry)
					   {%>





				<%if (PropertyShortCode == GCCPropertyShortCode.CNB || PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.WDB)
					  { %>
				<p>Please Select a Language for Survey : </p>
				<br />
				<p>Veuillez choisir une langue pour le sondage:</p>
				<br />
				<asp:RadioButtonList runat="server" ID="PINSurveyLang" RepeatDirection="vertical" RepeatLayout="Flow" AutoPostBack="true">
					<asp:ListItem Text="English" Value="English" Selected="True"></asp:ListItem>
					<asp:ListItem Text="Français" Value="French"></asp:ListItem>
				</asp:RadioButtonList>
				<br />
				<br />
				<asp:Button ID="btnLang" Text="Translate / Traduire" runat="server" CssClass="btn btn-primary" />

				<br />
				<br />
				<%} %>

				<div runat="server" id="English">


					<% if (PropertyShortCode == GCCPropertyShortCode.GAG || PropertyShortCode == GCCPropertyShortCode.SSKD ||PropertyShortCode == GCCPropertyShortCode.AJA || PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.SCBE || PropertyShortCode == GCCPropertyShortCode.WDB || PropertyShortCode == GCCPropertyShortCode.AJA || PropertyShortCode == GCCPropertyShortCode.GBH)
					   { %>
					<h3>You could WIN $100!</h3>
					<% } else if (PropertyShortCode == GCCPropertyShortCode.CNSH) { %>
					<h3>Dinner for four at 3Sixty Buffet</h3>
					<% } else if (PropertyShortCode == GCCPropertyShortCode.CNSS) { %>
					<h3>Dinner for four at All Star Grille</h3>
					<% } else if (PropertyShortCode == GCCPropertyShortCode.CNB) { %>
					<h3>A Dinner buffet for 4!</h3>

					<% } else { %>
					<h3>You could WIN up to $500!</h3>
					<% } %>
					<p>Thank you for your recent visit to <%= CasinoName %>! </p>
					<p>
						Your enjoyment is important to us, and we would be delighted if you would share your thoughts and experiences with us by completing this survey.
						<br />
						To show our appreciation for your time, you will be entered into a draw to win <b><%= PropertyTools.GetSurveyPrize( PropertyShortCode ) %></b>!
					</p>
					<sc:MessageManager runat="server" TitleOverride="Please read:" DisplayAs="Info">
						<Message>
							<% DateTime closeDate = DateTime.Now.AddMonths( 1 ).AddDays( -DateTime.Now.AddMonths( 1 ).Day );
						   //closeDate = new DateTime( closeDate.Year, closeDate.Month, 15 ); %>
							  To enter and be eligible to win, an entrant must complete the survey by <%= closeDate.ToString( "MMMM dd, yyyy" ) %>.
						</Message>
					</sc:MessageManager>
					<p>All of our surveys are conducted confidentially. Should you wish for us to respond to you regarding your comments, please indicate so and provide your contact information at the end of this survey.</p>
					<p>In order to be eligible to win a gift card, please give us your email address. Winners will be notified via email only. Click "Next" to continue.</p>
					<sc:MessageManager runat="server" ID="mmTxtEmail"></sc:MessageManager>
					<p>
						Email address:
					  <sc:SurveyTextBox ID="txtEmail" runat="server" SessionKey="txtEmail" DBColumn="Email" MaxLength="150" Size="50" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
					</p>
					<div class="button-container">
						<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
					</div>


				</div>




				<div runat="server" id="French">


					<% if (PropertyShortCode == GCCPropertyShortCode.GAG || PropertyShortCode == GCCPropertyShortCode.SSKD || PropertyShortCode == GCCPropertyShortCode.AJA || PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.WDB)
		{ %>
					<h3>Vous pourriez GAGNER 100 $!</h3>
					<% } else if (PropertyShortCode == GCCPropertyShortCode.CNSH) { %>
					<h3>Dinner for four at 3Sixty Buffet</h3>
					<% } else if (PropertyShortCode == GCCPropertyShortCode.CNSS) { %>
					<h3>Dinner for four at All Star Grille</h3>

					<% } else if (PropertyShortCode == GCCPropertyShortCode.CNB) { %>
					<h3>Vous pourriez gagner un souper-buffet pour 4!</h3>


					<% } else { %>
					<h3>Vous pourriez GAGNER jusqu'à 500 $!</h3>
					<% } %>
					<p>Merci de votre récente visite au <%= CasinoName %>! </p>
					<p>
						La qualité de votre expérience nous tient à cœur et nous serions très reconnaissants si vous partagiez vos impressions et votre expérience avec nous en répondant à ce sondage.<br />
						Pour vous remercier de votre temps, vous serez admissible au tirage pour gagner <b><%= PropertyTools.GetSurveyPrize_French( PropertyShortCode ) %></b>!
					</p>
					<sc:MessageManager runat="server" TitleOverride="Veuillez lire:" DisplayAs="Info">
						<Message>
							<% DateTime closeDate = DateTime.Now.AddMonths( 1 ).AddDays( -DateTime.Now.AddMonths( 1 ).Day );
						   //closeDate = new DateTime( closeDate.Year, closeDate.Month, 15 ); %>
							  Pour participer et être admissible au tirage, un participant doit remplir le sondage d'ici le <%= closeDate.ToString( "dd MMMM yyyy",System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR") ) %>.
						</Message>
					</sc:MessageManager>
					<p>Tous nos sondages sont menés de façon confidentielle. Si vous souhaitez que nous vous répondions au sujet de vos commentaires, veuillez l'indiquer et fournir vos coordonnées à la fin de ce sondage.</p>
					<p>Pour être admissible au tirage d’une carte-cadeau, veuillez nous indiquer votre adresse électronique. Les gagnants seront avisés par courriel seulement. Cliquez sur « Suivant » pour continuer.</p>
					<sc:MessageManager runat="server" ID="MessageManager3"></sc:MessageManager>
					<p>
						Adresse électronique :
					  <sc:SurveyTextBox ID="txtEmail_F" runat="server" SessionKey="txtEmail" DBColumn="Email" MaxLength="150" Size="50" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
					</p>
					<div class="button-container">
						<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
					</div>


				</div>










				<% }
				   
			   



					   //===========================================================================
					   //PAGE 1 - Kiosk
					   //===========================================================================
					   else if (CurrentPage == 1 && SurveyType == SharedClasses.GEISurveyType.Kiosk)
					   { %>
				<p>Thank you for your feedback! </p>
				<p>Your enjoyment is important to us, and we appreciate your willingness to share your thoughts &amp; experiences with us so that we can continue to improve upon and exceed your expectations.</p>
				<h3>Key Terms and Conditions</h3>
				<% if (PropertyShortCode == GCCPropertyShortCode.GAG)
				   { %>
				<p>Your personal information is collected and used by Great American Casino only for research purposes only. Your information will not be sold, shared with third parties, or used for soliciting purposes.</p>
				<% }
				   else if (PropertyShortCode == GCCPropertyShortCode.CNSH || PropertyShortCode == GCCPropertyShortCode.CNSS)
				   { %>
				<p>Your personal information is only being collected for Great Canadian Gaming Corporation's research purposes only. Your information will not be sold, shared with third parties, or used for soliciting purposes. If you have any questions about this, please write to GCGC's Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
				<p>Please click "Next" to continue.</p>
				<% }
				   else if (PropertyShortCode == GCCPropertyShortCode.CNB)
				   { %>
				<p>Your personal information is collected and used by Great Canadian Gaming Corporation (GCGC) for GCGC's research purposes only. Any personal information provided is managed according to the Right to Information and Protection of Privacy Act of New Brunswick (RTIPPA NB) and other applicable legislation. Your information is kept confidential and secure and is not disclosed to anyone outside of the company or other third parties without your consent, unless required by law or regulation. If you have any questions about this, please write to GCGC's Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
				<p>Please click "Next" to continue.</p>
				<% }
				   else
				   { %>
				<p>Your personal information is collected and used by Great Canadian Gaming Corporation (GCGC) on behalf of the British Columbia Lottery Corporation in accordance with British Columbia's Freedom of Information and Protection of Privacy Act. It will be used for GCGC's research purposes only. Your information will not be sold, shared with third parties, or used for soliciting purposes. If you have any questions about this, please write to GCGC's Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
				<p>Please click "Next" to continue.</p>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>
				<% }
					   //===========================================================================
					   //PAGE 1 - Staff Entry
					   //===========================================================================
					   else if (CurrentPage == 1 && SurveyType == SharedClasses.GEISurveyType.StaffSurvey)
					   { %>


				<% if (strSurveyLang != "French")
				   { %>

				<p>Please enter the date of the visit.</p>
				<p>Staff Name: <%= ReportingTools.CleanData( User.FullName ) %> <%--<sc:SurveyTextBox ID="txtStaffMember" runat="server" SessionKey="StaffMember" DBColumn="StaffMember" MaxLength="50" Size="50"></sc:SurveyTextBox>--%></p>
				<p>
					Visit Date:
					<sc:SurveyTextBox ID="txtVisitDate" runat="server" SessionKey="VisitDate" DBColumn="VisitDate" MaxLength="10" Size="10" CssClass="date-picker"></sc:SurveyTextBox>
				</p>
				<% if(strSurveyLang != null) { %>
				<p>Selected Survey Language : <%= strSurveyLang.ToString()%></p>
				<%} else { %>
				<p>Selected Survey Language : English</p>
				<%} %>
				<br />
				<div class="button-container">
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>

				<%}
				   else if (strSurveyLang == "French")
				   { %>


				<p>Veuillez entrer la date de la visite.</p>
				<p>Nom du personnel: <%= ReportingTools.CleanData( User.FullName ) %> <%--<sc:SurveyTextBox ID="txtStaffMember" runat="server" SessionKey="StaffMember" DBColumn="StaffMember" MaxLength="50" Size="50"></sc:SurveyTextBox>--%></p>
				<p>
					Date de visite:

					<sc:SurveyTextBox ID="txtVisitDate_F" runat="server" SessionKey="VisitDate" DBColumn="VisitDate" MaxLength="10" Size="10" CssClass="date-picker"></sc:SurveyTextBox>
				</p>
				<p>Langue d'enquête choisie: <%= strSurveyLang.ToString()%></p>
				<br />
				<div class="button-container">
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>


				<%} %>
				<% }
					   //===========================================================================
					   //PAGE 2 - Terms and Conditions
					   //===========================================================================
					   else if (CurrentPage == 2)
					   { %>

				<% if(strSurveyLang != "French") { %>

				<h2>Key Terms and Conditions</h2>
				<% if (PropertyShortCode == GCCPropertyShortCode.CNSH
								  || PropertyShortCode == GCCPropertyShortCode.CNSS)
				   { %>
				<p>No purchase necessary. An Entrant may only enter the contest once during the promotional period. The contest promotional period will close at the end of the month in which this survey was completed. Duplicate entries will be deleted. Your personal information is only being collected for Great Canadian Gaming Corporation's research purposes and to administer this contest. Your information will not be sold, shared with third parties, or used for soliciting purposes. If you have any questions about this, please write to GCGC's Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
				<% }
				   else if (PropertyShortCode == GCCPropertyShortCode.GAG)
				   { %>
				<p>No purchase necessary. An Entrant may only enter the contest once during the promotional period. The contest promotional period will close at the end of the month in which this survey was completed. Duplicate entries will be deleted. Your personal information is collected and used by Great American Casino only for research purposes and to administer this contest. Your information will not be sold, shared with third parties, or used for soliciting purposes.</p>
				<% }
	   else if (PropertyShortCode == GCCPropertyShortCode.SSKD || PropertyShortCode == GCCPropertyShortCode.AJA || PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.SCBE || PropertyShortCode == GCCPropertyShortCode.WDB || PropertyShortCode == GCCPropertyShortCode.GBH)
				   { %>
				<p>No purchase necessary. An Entrant may only enter the contest once during the promotional period. The contest promotional period will close at the end of the month in which this survey was completed. Duplicate entries will be deleted. Your personal information is collected and used by Shorelines Casino on behalf of Great Canadian Gaming Corporation (GCGC) and Ontario Lottery and Gaming Corporation (OLG) in accordance with Ontario’s Freedom of Information and Protection of Privacy Act. Your personal information is used for the purposes of: administering this contest, for customer service research, and for Responsible Gaming research.  It will be used for GCGC's research purposes and to administer this contest. Your information will not be sold, shared with third parties, or used for soliciting purposes. If you have any questions about this, please write to GCGC's Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
				<% }
				   else if (PropertyShortCode == GCCPropertyShortCode.CNB)
				   { %>
				<p>No purchase necessary. An Entrant may only enter the contest once during the promotional period. The contest promotional period will close at the end of the month in which this survey was completed. Duplicate entries will be deleted. Your personal information is collected and used by Great Canadian Gaming Corporation (GCGC) for GCGC's research purposes only. Any personal information provided is managed according to the Right to Information and Protection of Privacy Act of New Brunswick (RTIPPA NB) and other applicable legislation. Your information is kept confidential and secure and is not disclosed to anyone outside of the company or other third parties without your consent, unless required by law or regulation. If you have any questions about this, please write to GCGC's Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
				<% } else { %>
				<p>No purchase necessary. An Entrant may only enter the contest once during the promotional period. The contest promotional period will close at the end of the month in which this survey was completed. Duplicate entries will be deleted. Your personal information is collected and used by Great Canadian Gaming Corporation (GCGC) on behalf of the British Columbia Lottery Corporation in accordance with British Columbia's Freedom of Information and Protection of Privacy Act. It will be used for GCGC's research purposes and to administer this contest. Your information will not be sold, shared with third parties, or used for soliciting purposes. If you have any questions about this, please write to GCGC's Privacy Officer at 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
				<% } %>
				<p>For terms of use and full terms and conditions, please see below.</p>

				<% if(PropertyShortCode == GCCPropertyShortCode.CNB) { %>
				<p><a href="/TermsAndConditions_CNB/<%= PropertyShortCode.ToString()%>" title="Terms and Conditions" target="_blank">Terms of Use, Full Contest Conditions and Privacy Policy</a></p>
				<%}
	   else if (PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.WDB || PropertyShortCode == GCCPropertyShortCode.GBH)
	   { %>
				<p><a href="/TermsAndConditions_SCTI/<%= PropertyShortCode.ToString()%>" title="Terms and Conditions" target="_blank">Terms of Use, Full Contest Conditions and Privacy Policy</a></p>
				<% } else { %>
				<p><a href="/TermsAndConditions/<%= PropertyShortCode.ToString()%>" title="Terms and Conditions" target="_blank">Terms of Use, Full Contest Conditions and Privacy Policy</a></p>
				<%} %>

				<p class="question">
					By selecting "I agree" and providing your email address, you accept the terms and conditions. The survey should take approximately 10 minutes to complete depending on your comments.
				</p>
				<sc:MessageManager runat="server" ID="mmAcceptGroup"></sc:MessageManager>
				<p>
					<sc:SurveyRadioButton ID="radAccept" runat="server" GroupName="acceptgrp" SessionKey="radAccept" CssClass="radalign" Text="&nbsp;I agree and want to proceed with the survey" /><br />
					<sc:SurveyRadioButton ID="radDecline" runat="server" GroupName="acceptgrp" SessionKey="radDecline" CssClass="radalign" Text="&nbsp;I decline to complete the survey. If you choose to decline you will not be able to provide your feedback or participate in the contest." />
				</p>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Continue" />
				</div>

				<%} else { %>


				<h2>Conditions générales</h2>
				<% if (PropertyShortCode == GCCPropertyShortCode.CNSH
								  || PropertyShortCode == GCCPropertyShortCode.CNSS)
				   { %>
				<p>Pas d'achat nécessaire. Un participant ne peut participer au concours qu'une seule fois pendant la période promotionnelle. La période promotionnelle du concours se terminera à la fin du mois au cours duquel le sondage a été Terminer. Les entrées en double seront supprimées. Vos renseignements personnels ne sont recueillis que pour la recherche de Great Canadian Gaming Corporation et pour administrer ce concours. Vos informations ne seront pas vendues, partagées avec des tiers ou utilisé à des fins de sollicitation. Si vous avez des questions à ce sujet, veuillez écrire à l'agent de confidentialité de GCGC au 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
				<% }
				   else if (PropertyShortCode == GCCPropertyShortCode.GAG)
				   { %>
				<p>Pas d'achat nécessaire. Un participant ne peut participer au concours qu'une seule fois pendant la période promotionnelle. La période promotionnelle du concours se terminera à la fin du mois au cours duquel le sondage a été Terminer. Les entrées en double seront supprimées. Vos renseignements personnels sont recueillis et utilisés par Great American Casino uniquement à des fins de recherche et d'administrer ce concours. Vos informations ne seront pas vendues, partagées avec des tiers ou utilisé à des fins de sollicitation.</p>
				<% }
	   else if (PropertyShortCode == GCCPropertyShortCode.SSKD || PropertyShortCode == GCCPropertyShortCode.AJA || PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.SCBE || PropertyShortCode == GCCPropertyShortCode.WDB || PropertyShortCode == GCCPropertyShortCode.GBH)
				   { %>
				<p>Aucun achat nécessaire. Un participant ne peut participer au concours qu'une seule fois durant la période promotionnelle. La période promotionnelle du concours se terminera à la fin du mois au cours duquel le sondage a été complété. Les entrées en double seront supprimées. Vos renseignements personnels sont recueillis et utilisés par le Casino Shorelines au nom de la Great Canadian Gaming Corporation (GCGC) et de la Société des loteries et des jeux de l'Ontario (OLG) conformément à la Loi sur l'accès à l'information et la protection de la vie privée. Vos renseignements personnels sont utilisés aux fins d’administrer ce concours, à la recherche du service à la clientèle et pour la recherche du jeu responsable. Ils seront utilisés à des fins de recherche pour la GCGC et pour administrer ce concours. Vos informations ne seront pas vendues, partagées avec des tiers ou utilisées à des fins de sollicitation. Si vous aviez des questions à ce sujet, veuillez écrire à l'agent de confidentialité de la GCGC au 95 Schooner Street, Coquitlam, C.B. V3K 7A8.</p>
				<% }
				   else if (PropertyShortCode == GCCPropertyShortCode.CNB)
				   { %>
				<p>Aucun achat nécessaire. Un participant ne peut participer au concours qu'une seule fois pendant la période promotionnelle. La période promotionnelle du concours se terminera à la fin du mois au cours duquel le sondage a été Terminer. Les entrées en double seront supprimées. Vos renseignements personnels sont recueillis et utilisés par la Great Canadian Gaming Corporation (GCGC) aux fins de recherche de la GCGC uniquement. Tous les renseignements personnels fournis sont gérés conformément à la Loi sur l'accès à l'information et à la protection de la vie privée du Nouveau-Brunswick (RTIPPA NB) et à d'autres lois applicables. Vos renseignements sont gardés confidentiels et sécurisés et ne sont divulgués à quiconque en dehors de la société ou d'autres tiers sans votre consentement, sauf si la loi ou le règlement l'exige. Si vous avez des questions à ce sujet, veuillez écrire à l'agent de confidentialité de GCGC au 95 rue Schooner, Coquitlam, C.-B. V3K 7A8.</p>
				<% } else { %>
				<p>Pas Aucun achat nécessaire. Un participant ne peut participer au concours qu'une seule fois pendant la période promotionnelle. La période promotionnelle du concours se terminera à la fin du mois au cours duquel le sondage a été Terminer. Les entrées en double seront supprimées. Vos renseignements personnels sont recueillis et utilisés par la Great Canadian Gaming Corporation (GCGC) au nom de la British Columbia Lottery Corporation conformément à la Loi sur l'accès à l'information et la protection de la vie privée de la Colombie-Britannique. Il sera utilisé à des fins de recherche GCGC et d'administrer ce concours. Vos informations ne seront pas vendues, partagées avec des tiers ou utilisé à des fins de sollicitation. Si vous avez des questions à ce sujet, veuillez écrire à l'agent de confidentialité de GCGC au 95 Schooner Street, Coquitlam, BC V3K 7A8.</p>
				<% } %>
				<p>Pour les conditions d'utilisation et l’ensemble des modalités, veuillez voir ci-dessous.</p>



				<% if(PropertyShortCode == GCCPropertyShortCode.CNB) { %>
				<p><a href="/TermsAndConditions_French_CNB/<%= PropertyShortCode.ToString()%>" title="Terms and Conditions" target="_blank">Conditions d'utilisation, conditions complètes du concours et politique de confidentialité</a></p>


				<% }
	   else if (PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.WDB || PropertyShortCode == GCCPropertyShortCode.GBH)
	   { %>
				<p><a href="/TermsAndConditions_French_SCTI/<%= PropertyShortCode.ToString()%>" title="Terms and Conditions" target="_blank">Conditions d'utilisation, conditions complètes du concours et politique de confidentialité</a></p>



				<%} else{ %>
				<p><a href="/TermsAndConditions_French/<%= PropertyShortCode.ToString()%>" title="Terms and Conditions" target="_blank">Conditions d'utilisation, conditions complètes du concours et politique de confidentialité</a></p>

				<%} %>


				<p class="question">
					En sélectionnant « J'accepte » et en fournissant votre adresse électronique, vous acceptez les modalités. Le sondage devrait prendre environ 10 minutes, selon vos commentaires.
				</p>
				<sc:MessageManager runat="server" ID="MessageManager2"></sc:MessageManager>
				<p>
					<sc:SurveyRadioButton ID="radAccept_F" runat="server" GroupName="acceptgrp" SessionKey="radAccept" CssClass="radalign" Text="&nbsp;J’accepte et je souhaite répondre au sondage." /><br />
					<sc:SurveyRadioButton ID="radDecline_F" runat="server" GroupName="acceptgrp" SessionKey="radDecline" CssClass="radalign" Text="&nbsp;Je ne veux pas répondre au sondage. Si vous choisissez de refuser, vous ne serez pas en mesure de fournir vos commentaires ou de participer au concours." />
				</p>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Continuer" />
				</div>


				<% } %>
				<% }
					   //===========================================================================
					   //PAGE 99 - Declined Message
					   //===========================================================================
					   else if (CurrentPage == 99)
					   { %>


				<% if(strSurveyLang != "French") { %>


				<p>
					We acknowledge that you have chosen not to participate in the survey<% if (!IsKioskOrStaffEntry)
																						   { %> and contest<% } %>. Thank you for your <% if (!IsKioskOrStaffEntry)
																																		  { %>recent<% } %> visit and we look forward to seeing you again soon!
				</p>

				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Undo" />
					<% if (SurveyType != SharedClasses.GEISurveyType.Kiosk)
					   { %>
					<a href="<%= GetCasinoURL() %>" class="btn btn-primary">End Survey</a>
					<% }
					   else
					   { %>
					<a href="<%= GetSurveyURL(-1) %>" class="btn btn-primary">Start Over</a>
					<% } %>
				</div>


				<% } else if(strSurveyLang == "French" ) { %>

				<p>
					Nous reconnaissons que vous avez choisi de ne pas participer au sondage et au concours. Merci de votre récente visite et nous espérons vous revoir bientôt!

				</p>

				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Défaire" />
					<% if (SurveyType != SharedClasses.GEISurveyType.Kiosk)
					   { %>
					<a href="<%= GetCasinoURL() %>" class="btn btn-primary">Fin du sondage</a>
					<% }
					   else
					   { %>
					<a href="<%= GetSurveyURL(-1) %>" class="btn btn-primary">Recommencer</a>
					<% } %>
				</div>

				<% } %>
				<% }
				//<%--<%--trying removing extra 
				
					   //===========================================================================
					   //PAGE 3 - Q1
					   //===========================================================================
					  else if (CurrentPage == 3)
					   { %>


				<% if (strSurveyLang != "French")
				   { %>


				<p class="question">
					Which of the following activities was your <u>primary</u> reason for visiting <%= CasinoName %>?<br />
					Please choose one.
				</p>
				<sc:MessageManager runat="server" ID="mmQ1"></sc:MessageManager>
				<p>
					<asp:Panel runat="server">
						<% if (new[] { "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "CNSH", "CNSS", "EC", "SSKD","AJA", "SCTI", "CNB","SCBE", "WDB", "GBH" }.Contains(PropertyShortCode.ToString()))
						   { %>

						<sc:SurveyRadioButton ID="radQ1_Slots" runat="server" GroupName="Q1" SessionKey="Q1Slots" DBColumn="Q1" DBValue="Slots" Text="&nbsp;Playing Slots" /><br />


						<% } %>
						<% if (new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "GAG", "EC", "SCTI", "CNB","SCBE" ,"WDB","GBH"}.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Tables" runat="server" GroupName="Q1" SessionKey="Q1Tables" DBColumn="Q1" DBValue="Tables" Text="&nbsp;Playing Tables" /><br />
						<% } %>
						<% if (new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "EC", "CNB","SCBE" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Poker" runat="server" GroupName="Q1" SessionKey="Q1Poker" DBColumn="Q1" DBValue="Poker" Text="&nbsp;Playing Poker" /><br />
						<% } %>
						<sc:SurveyRadioButton ID="radQ1_Food" runat="server" GroupName="Q1" SessionKey="Q1Food" DBColumn="Q1" DBValue="Food" Text="&nbsp;Enjoying Food or Beverages" /><br />
						<% if (new[] { "RR", "HRCV", "VRL", "CCH", "CMR", "CDC", "CNSH", "EC", "SCTI", "CNB","WDB","GBH" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Entertainment" runat="server" GroupName="Q1" SessionKey="Q1Entertainment" DBColumn="Q1" DBValue="Entertainment" Text="&nbsp;Watching Live Entertainment at a show lounge or theatre" /><br />
						<% } %>
						<% if (PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNB)
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Hotel" runat="server" GroupName="Q1" SessionKey="Q1Hotel" DBColumn="Q1" DBValue="Hotel" Text="&nbsp;Staying at our Hotel" /><br />
						<% } %>
						<% if (new[] { "FD", "HA", "EC" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_LiveRacing" runat="server" GroupName="Q1" SessionKey="Q1LiveRacing" DBColumn="Q1" DBValue="LiveRacing" Text="&nbsp;Watching Live Racing" /><br />
						<% } %>
						<% if (new[] { "RR", "HRCV", "HA", "NAN", "CMR", "EC", "SSKD", "AJA", "SCBE" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Racebook" runat="server" GroupName="Q1" SessionKey="Q1Racebook" DBColumn="Q1" DBValue="Racebook" Text="&nbsp;Watching Racing at our Racebook" /><br />
						<% } %>
						<% if (new[] { "CCH", "CMR", "CDC" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Bingo" runat="server" GroupName="Q1" SessionKey="Q1Bingo" DBColumn="Q1" DBValue="Bingo" Text="&nbsp;Playing Bingo" /><br />
						<% } %>
						<% if (!new[] { "CNSH", "CNSS", "EC","CNB" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Lottery" runat="server" GroupName="Q1" SessionKey="Q1Lottery" DBColumn="Q1" DBValue="Lottery" Text="&nbsp;Lottery / Pull Tabs" /><br />
						<% } %>
						<sc:SurveyRadioButton ID="radQ1_None" runat="server" GroupName="Q1" SessionKey="Q1None" DBColumn="Q1" DBValue="None" Text="&nbsp;None" /><br />
					</asp:Panel>
				</p>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>

				<% }
				   else if (strSurveyLang == "French")
				   { %>


				<p class="question">
					Laquelle des activités suivantes constituait la raison <u>principale</u> de votre visite au  <% = CasinoName%>?
					<br />
					Veuillez en choisir une:
				</p>
				<sc:MessageManager runat="server" ID="MessageManager1"></sc:MessageManager>
				<p>
					<asp:Panel runat="server">
						<% if (new[] { "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "CNSH", "CNSS", "EC", "SSKD", "AJA", "SCTI", "CNB", "SCBE", "WDB", "GBH" }.Contains(PropertyShortCode.ToString()))
						   { %>

						<sc:SurveyRadioButton ID="radQ1_Slots_F" runat="server" GroupName="Q1" SessionKey="Q1Slots" DBColumn="Q1" DBValue="Slots" Text="&nbsp;Jouer aux machines à sous" /><br />


						<% } %>
						<% if (new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "GAG", "EC", "SCTI", "CNB", "SCBE", "WDB", "GBH" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Tables_F" runat="server" GroupName="Q1" SessionKey="Q1Tables" DBColumn="Q1" DBValue="Tables" Text="&nbsp;Jouer aux tables" /><br />
						<% } %>
						<% if (new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "EC", "CNB","SCBE" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Poker_F" runat="server" GroupName="Q1" SessionKey="Q1Poker" DBColumn="Q1" DBValue="Poker" Text="&nbsp;Jouer au poker" /><br />
						<% } %>
						<sc:SurveyRadioButton ID="radQ1_Food_F" runat="server" GroupName="Q1" SessionKey="Q1Food" DBColumn="Q1" DBValue="Food" Text="&nbsp;Profiter de la gastronomie" /><br />
						<% if (new[] { "RR", "HRCV", "VRL", "CCH", "CMR", "CDC", "CNSH", "EC", "SCTI", "CNB", "WDB", "GBH" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Entertainment_F" runat="server" GroupName="Q1" SessionKey="Q1Entertainment" DBColumn="Q1" DBValue="Entertainment" Text="&nbsp;Profiter du divertissement à notre Lounge " /><br />
						<% } %>
						<% if (PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNB)
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Hotel_F" runat="server" GroupName="Q1" SessionKey="Q1Hotel" DBColumn="Q1" DBValue="Hotel" Text="&nbsp;Séjour à l’hôtel" /><br />
						<% } %>
						<% if (new[] { "FD", "HA", "EC" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_LiveRacing_F" runat="server" GroupName="Q1" SessionKey="Q1LiveRacing" DBColumn="Q1" DBValue="LiveRacing" Text="&nbsp;Regarder la course en direct" /><br />
						<% } %>
						<% if (new[] { "RR", "HRCV", "HA", "NAN", "CMR", "EC", "SSKD", "AJA", "SCBE" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Racebook_F" runat="server" GroupName="Q1" SessionKey="Q1Racebook" DBColumn="Q1" DBValue="Racebook" Text="&nbsp;Regarder Racing sur votre Facebook" /><br />
						<% } %>
						<% if (new[] { "CCH", "CMR", "CDC" }.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Bingo_F" runat="server" GroupName="Q1" SessionKey="Q1Bingo" DBColumn="Q1" DBValue="Bingo" Text="&nbsp;Jouer au bingo" /><br />
						<% } %>
						<% if (!new[] { "CNSH", "CNSS", "EC","CNB"}.Contains(PropertyShortCode.ToString()))
						   { %>
						<sc:SurveyRadioButton ID="radQ1_Lottery_F" runat="server" GroupName="Q1" SessionKey="Q1Lottery" DBColumn="Q1" DBValue="Lottery" Text="&nbsp;Billets de loterie/billets à languette" /><br />
						<% } %>
						<sc:SurveyRadioButton ID="radQ1_None_F" runat="server" GroupName="Q1" SessionKey="Q1None" DBColumn="Q1" DBValue="None" Text="&nbsp;Aucune" /><br />
					</asp:Panel>
				</p>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>



				<% } %>
				<% }
					   //===========================================================================
					   //PAGE 4 - Q2-Q4
					   //===========================================================================
					   else if (CurrentPage == 4)
					   { %>
				<%  if (!IsKioskOrStaffEntry)
					{ %>



				<% if (strSurveyLang != "French")
				   { %>



				<p class="question">
					Which of the following <u>other</u> activities did you engage in while visiting <%= CasinoName %>?<br />
					Please choose all that apply.
				</p>
				<p>
					<% if (new[] { "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "CNSH", "CNSS", "EC", "SSKD", "AJA", "SCTI", "CNB", "SCBE", "WDB", "GBH" }.Contains(PropertyShortCode.ToString()) && !radQ1_Slots.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Slots" runat="server" SessionKey="Q2Slots" DBColumn="Q2_Slots" DBValue="1" Text="&nbsp;Playing Slots" /><br />
					<% } %>
					<% if (new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "GAG", "EC", "SCTI", "CNB", "SCBE", "WDB", "GBH" }.Contains(PropertyShortCode.ToString()) && !radQ1_Tables.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Tables" runat="server" SessionKey="Q2Tables" DBColumn="Q2_Tables" DBValue="1" Text="&nbsp;Playing Tables" /><br />
					<% } %>
					<% if (new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "EC", "CNB","SCBE" }.Contains(PropertyShortCode.ToString()) && !radQ1_Poker.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Poker" runat="server" SessionKey="Q2Poker" DBColumn="Q2_Poker" DBValue="1" Text="&nbsp;Playing Poker" /><br />
					<% } %>
					<% if (!radQ1_Food.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Food" runat="server" SessionKey="Q2Food" DBColumn="Q2_Food" DBValue="1" Text="&nbsp;Enjoying Food or Beverages" /><br />
					<% } %>
					<% if (new[] { "RR", "HRCV", "VRL", "CCH", "CMR", "CDC", "CNSH", "EC", "SCTI", "CNB", "WDB", "GBH" }.Contains(PropertyShortCode.ToString()) && !radQ1_Entertainment.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Entertainment" runat="server" SessionKey="Q2Entertainment" DBColumn="Q2_Entertainment" DBValue="1" Text="&nbsp;Watching Live Entertainment at a show lounge or theatre" /><br />
					<% } %>
					<% if ((PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNB) && !radQ1_Hotel.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Hotel" runat="server" SessionKey="Q2Hotel" DBColumn="Q2_Hotel" DBValue="1" Text="&nbsp;Staying at our Hotel" /><br />
					<% } %>
					<% if (new[] { "FD", "HA", "EC" }.Contains(PropertyShortCode.ToString()) && !radQ1_LiveRacing.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_LiveRacing" runat="server" SessionKey="Q2LiveRacing" DBColumn="Q2_LiveRacing" DBValue="1" Text="&nbsp;Watching Live Racing" /><br />
					<% } %>
					<% if (new[] { "RR", "HRCV", "HA", "NAN", "CMR", "EC", "SSKD", "AJA", "SCBE" }.Contains(PropertyShortCode.ToString()) && !radQ1_Racebook.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Racebook" runat="server" SessionKey="Q2Racebook" DBColumn="Q2_Racebook" DBValue="1" Text="&nbsp;Watching Racing at our Racebook" /><br />
					<% } %>
					<% if (new[] { "CCH", "CMR", "CDC" }.Contains(PropertyShortCode.ToString()) && !radQ1_Bingo.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Bingo" runat="server" SessionKey="Q2Bingo" DBColumn="Q2_Bingo" DBValue="1" Text="&nbsp;Playing Bingo" /><br />
					<% } %>
					<% if (!new[] { "CNSH", "CNSS", "EC","CNB"}.Contains(PropertyShortCode.ToString()) && !radQ1_Lottery.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Lottery" runat="server" SessionKey="Q2Lottery" DBColumn="Q2_Lottery" DBValue="1" Text="&nbsp;Lottery / Pull Tabs" /><br />
					<% } %>
				</p>

				<% }
				   else
				   { %>

				<p class="question">
					Parmi les autres activités suivantes, lesquelles avez-vous effectuées lors de votre visite au <%= CasinoName %>?
					<br />
					Veuillez choisir toutes les réponses qui s'appliquent.

				</p>
				<p>
					<% if (new[] { "RR", "HRCV", "HA", "VRL", "NAN", "CCH", "CMR", "CDC", "CNSH", "CNSS", "EC", "SSKD", "AJA", "SCTI", "CNB", "SCBE", "WDB", "GBH" }.Contains(PropertyShortCode.ToString()) && !radQ1_Slots.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Slots_F" runat="server" SessionKey="Q2Slots" DBColumn="Q2_Slots" DBValue="1" Text="&nbsp;Jouer aux machines à sous" /><br />
					<% } %>
					<% if (new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "GAG", "EC", "SCTI", "CNB", "SCBE", "WDB", "GBH" }.Contains(PropertyShortCode.ToString()) && !radQ1_Tables.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Tables_F" runat="server" SessionKey="Q2Tables" DBColumn="Q2_Tables" DBValue="1" Text="&nbsp;Jouer aux tables" /><br />
					<% } %>
					<% if (new[] { "RR", "HRCV", "VRL", "NAN", "CNSH", "CNSS", "EC", "CNB","SCBE" }.Contains(PropertyShortCode.ToString()) && !radQ1_Poker.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Poker_F" runat="server" SessionKey="Q2Poker" DBColumn="Q2_Poker" DBValue="1" Text="&nbsp;Jouer au poker" /><br />
					<% } %>
					<% if (!radQ1_Food.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Food_F" runat="server" SessionKey="Q2Food" DBColumn="Q2_Food" DBValue="1" Text="&nbsp;Profiter de la gastronomie" /><br />
					<% } %>
					<% if (new[] { "RR", "HRCV", "VRL", "CCH", "CMR", "CDC", "CNSH", "EC", "SCTI", "CNB", "WDB", "GBH" }.Contains(PropertyShortCode.ToString()) && !radQ1_Entertainment.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Entertainment_F" runat="server" SessionKey="Q2Entertainment" DBColumn="Q2_Entertainment" DBValue="1" Text="&nbsp;Profiter du divertissement à notre Lounge " /><br />
					<%--"&nbsp;Assister à un spectacle à notre salle de spectacles ou à notre pub"--%>
					<% } %>
					<% if ((PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNB) && !radQ1_Hotel.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Hotel_F" runat="server" SessionKey="Q2Hotel" DBColumn="Q2_Hotel" DBValue="1" Text="&nbsp;Séjour à l’hôtel" /><br />
					<% } %>
					<% if (new[] { "FD", "HA", "EC" }.Contains(PropertyShortCode.ToString()) && !radQ1_LiveRacing.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_LiveRacing_F" runat="server" SessionKey="Q2LiveRacing" DBColumn="Q2_LiveRacing" DBValue="1" Text="&nbsp;Regarder la course en direct" /><br />
					<% } %>
					<% if (new[] { "RR", "HRCV", "HA", "NAN", "CMR", "EC", "SSKD", "AJA", "SCBE" }.Contains(PropertyShortCode.ToString()) && !radQ1_Racebook.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Racebook_F" runat="server" SessionKey="Q2Racebook" DBColumn="Q2_Racebook" DBValue="1" Text="&nbsp;Regarder Racing sur votre Facebook" /><br />
					<% } %>
					<% if (new[] { "CCH", "CMR", "CDC" }.Contains(PropertyShortCode.ToString()) && !radQ1_Bingo.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Bingo_F" runat="server" SessionKey="Q2Bingo" DBColumn="Q2_Bingo" DBValue="1" Text="&nbsp;Jouer au bingo" /><br />
					<% } %>
					<% if (!new[] { "CNSH", "CNSS", "EC","CNB"}.Contains(PropertyShortCode.ToString()) && !radQ1_Lottery.Checked)
					   { %>
					<sc:SurveyCheckBox ID="chkQ2_Lottery_F" runat="server" SessionKey="Q2Lottery" DBColumn="Q2_Lottery" DBValue="1" Text="&nbsp;Billets de loterie/billets à languette" /><br />
					<% } %>
				</p>

				<%} %>






				<% if (PropertyShortCode == GCCPropertyShortCode.GAG)
				   { %>



				<% if (strSurveyLang != "French")
				   { %>


				<p class="question">
					Which Great American Casino location did you <u>last</u> visit?
				</p>
				<sc:MessageManager runat="server" ID="mmQ3"></sc:MessageManager>
				<p>
					<sc:SurveyRadioButton ID="radQ3_Everett" runat="server" GroupName="Q3" SessionKey="Q3Everett" DBColumn="Q3" DBValue="Everett" Text="&nbsp;Everett" /><br />
					<sc:SurveyRadioButton ID="radQ3_Lakewood" runat="server" GroupName="Q3" SessionKey="Q3Lakewood" DBColumn="Q3" DBValue="Lakewood" Text="&nbsp;Lakewood" /><br />
					<sc:SurveyRadioButton ID="radQ3_Tukwila" runat="server" GroupName="Q3" SessionKey="Q3Tukwila" DBColumn="Q3" DBValue="Tukwila" Text="&nbsp;Tukwila" /><br />
					<sc:SurveyRadioButton ID="radQ3_DeMoines" runat="server" GroupName="Q3" SessionKey="Q3DeMoines" DBColumn="Q3" DBValue="DeMoines" Text="&nbsp;DeMoines" /><br />
				</p>


				<%}
				   else
				   { %>

				<p class="question">
					Quelle est l'adresse du Great American Casino où vous avez visité <u>dernier </u>?
				</p>
				<sc:MessageManager runat="server" ID="mmQ3_F"></sc:MessageManager>
				<p>
					<sc:SurveyRadioButton ID="radQ3_Everett_F" runat="server" GroupName="Q3" SessionKey="Q3Everett" DBColumn="Q3" DBValue="Everett" Text="&nbsp;Everett" /><br />
					<sc:SurveyRadioButton ID="radQ3_Lakewood_F" runat="server" GroupName="Q3" SessionKey="Q3Lakewood" DBColumn="Q3" DBValue="Lakewood" Text="&nbsp;Lakewood" /><br />
					<sc:SurveyRadioButton ID="radQ3_Tukwila_F" runat="server" GroupName="Q3" SessionKey="Q3Tukwila" DBColumn="Q3" DBValue="Tukwila" Text="&nbsp;Tukwila" /><br />
					<sc:SurveyRadioButton ID="radQ3_DeMoines_F" runat="server" GroupName="Q3" SessionKey="Q3DeMoines" DBColumn="Q3" DBValue="DeMoines" Text="&nbsp;DeMoines" /><br />
				</p>



				<%} %>

				<% } %>
				<% } %>







				<% if (strSurveyLang != "French")
				   { %>

				<p class="question">
					Are you a <%= PropertyTools.GetPlayersClubName(PropertyID) %> Member?
				</p>
				<uc1:YesNoControl ID="Q4" runat="server" SessionKey="Q4" DBColumn="Q4" />
				<p>
					If Yes, please provide your member number / player card number:
					<sc:SurveyTextBox ID="txtQ4_CardNumber" runat="server" SessionKey="Q4CardNumber" DBColumn="Q4CardNumber" MaxLength="15" Size="15" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
				</p>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>



				<% }
				   else
				   { %>


				<p class="question">
					Êtes-vous membre du  <% = PropertyTools.GetPlayersClubName_French (PropertyID)%> ?
				</p>
				<uc1:YesNoControlFrench ID="Q4_F" runat="server" SessionKey="Q4" DBColumn="Q4" />
				<p>
					Si «Oui», veuillez fournir votre numéro de membre/numéro de carte de joueur :
					<sc:SurveyTextBox ID="txtQ4_CardNumber_F" runat="server" SessionKey="Q4CardNumber" DBColumn="Q4CardNumber" MaxLength="15" Size="15" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
				</p>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>


				<%} %>
				<% }
					   //===========================================================================
					   //PAGE 5 - Q5-Q6
					   //===========================================================================
					   else if (CurrentPage == 5)
					   { %>




				<% if (strSurveyLang != "French")
				   { %>





				<p class="question">
					Please respond to the following questions and statements based on the experience you had during your <% if (!IsKioskOrStaffEntry)
																															{ %>most recent <% } %>visit to <%= CasinoName %><% if (IsKioskOrStaffEntry)
																																												{ %> today<% } %>.
				</p>
				<sc:MessageManager runat="server" ID="mmQ5"></sc:MessageManager>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q5A" SessionKey="Q5A" DBColumn="Q5A" Label="Overall, how would you rate the quality of our facility and service on your most recent visit to {CasinoName}?" />
				<uc1:QuestionRowControl runat="server" ID="Q5B" SessionKey="Q5B" DBColumn="Q5B" Label="Taking into account your most recent experience (all the activities and services) at {CasinoName} and your money, time, and effort spent, how would you rate the overall value you received?" />
				<p class="question">
					How likely are you to...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Definitely Would</div>
					<div class="col-md-1 col-xs-2 title">Probably Would</div>
					<div class="col-md-1 col-xs-2 title">Might or Might Not</div>
					<div class="col-md-1 col-xs-2 title">Probably Would Not</div>
					<div class="col-md-1 col-xs-2 title">Definitely Would Not</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q6A" SessionKey="Q6A" DBColumn="Q6A" Label="Recommend {CasinoName} to a friend, family member or business associate who is looking for gaming entertainment." />
				<uc1:QuestionRowControl runat="server" ID="Q6B" SessionKey="Q6B" DBColumn="Q6B" Label="Visit mostly {CasinoName} for your gaming entertainment." />
				<uc1:QuestionRowControl runat="server" ID="Q6C" SessionKey="Q6C" DBColumn="Q6C" Label="Visit {CasinoName} for your next gaming entertainment opportunity." />
				<uc1:QuestionRowControl runat="server" ID="Q6D" SessionKey="Q6D" DBColumn="Q6D" Label="Provide contact information and personal preferences to {CasinoName} so that the casino can serve you better." />
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>



				<%}
				   else
				   { %>

				<p class="question">
					Veuillez répondre aux questions et énoncés suivants selon votre expérience au cours de votre plus récente visite au <% = CasinoName%> <% if (IsKioskOrStaffEntry)
																																																	{%> aujourd'hui <%}%>.
				</p>
				<sc:MessageManager runat="server" ID="mmQ5_F"></sc:MessageManager>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellente</div>
					<div class="col-md-1 col-xs-2 title">Très bonne</div>
					<div class="col-md-1 col-xs-2 title">Bonne</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvaise</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q5A_F" SessionKey="Q5A" DBColumn="Q5A" Label="Dans l’ensemble, comment avez-vous trouvé la qualité de nos installations et du service lors de votre plus récente visite au {CasinoName}?" />
				<uc1:QuestionRowControl runat="server" ID="Q5B_F" SessionKey="Q5B" DBColumn="Q5B" Label="En tenant compte de votre plus récente expérience (tous les services et activités) au {CasinoName} ainsi qu’à l’argent que vous avez dépensé, au temps que vous avez passé et à l’effort que vous avez mis, comment avez-vous trouvé la valeur de votre visite?" />
				<p class="question">
					Dans quelle mesure êtes-vous susceptible de...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 ">&nbsp Certainement &nbsp</div>
					<div class="col-md-1 col-xs-2 ">&nbsp Probablement &nbsp</div>
					<div class="col-md-1 col-xs-2 ">&nbsp Possiblement &nbsp</div>
					<div class="col-md-1 col-xs-2 ">&nbsp Probablement pas &nbsp</div>
					<div class="col-md-1 col-xs-2 ">&nbsp Certainement pas &nbsp</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q6A_F" SessionKey="Q6A" DBColumn="Q6A" Label="Recommander {CasinoName} à un ami, un membre de votre famille ou un collègue qui cherche à se divertir avec des jeux?" />
				<uc1:QuestionRowControl runat="server" ID="Q6B_F" SessionKey="Q6B" DBColumn="Q6B" Label="Visiter {CasinoName} pour vous divertir avec des jeux?" />
				<uc1:QuestionRowControl runat="server" ID="Q6C_F" SessionKey="Q6C" DBColumn="Q6C" Label="Visiter {CasinoName} la prochaine fois que vous voulez vous divertir avec des jeux?" />
				<uc1:QuestionRowControl runat="server" ID="Q6D_F" SessionKey="Q6D" DBColumn="Q6D" Label="Fournir vos coordonnées et vos préférences personnelles au {CasinoName} afin que nous puissions mieux vous servir?" />
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>


				<%} %>


				<% }
					   //===========================================================================
					   //PAGE 6 - Q7-Q11 Staff
					   //===========================================================================
					   else if (CurrentPage == 6)
					   { %>






				<% if (strSurveyLang != "French")
				   { %>




				<h2>Staff</h2>
				<p class="question">
					How satisfied were you with the <u>staff</u> and <u>the level of customer service</u> provided during your <% if (!IsKioskOrStaffEntry)
																																  { %>last <% } %>visit to <%= CasinoName %><% if (IsKioskOrStaffEntry)
																																											   { %> today<% } %>?<br />
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
					Overall, how satisfied <%= !IsKioskOrStaffEntry ? "were" : "are" %> you with the <u>service provided by the staff</u> at <%= CasinoName %>?
				</p>
				<uc1:ScaleQuestionControl runat="server" ID="Q8" SessionKey="Q8" DBColumn="Q8" />
				<%  if (!IsKioskOrStaffEntry)
					{ %>
				<p class="question">
					Specifically, how would you rate each of these staff members that you encountered? Please rate your satisfaction with the staff you interacted with:
				</p>
				<sc:MessageManager runat="server" ID="mmQ9"></sc:MessageManager>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">No Interaction</div>
				</div>
				<div class="randomize">
					<% bool isChances = PropertyShortCode == GCCPropertyShortCode.CCH || PropertyShortCode == GCCPropertyShortCode.CMR || PropertyShortCode == GCCPropertyShortCode.CDC; %>
					<uc1:QuestionRowControl runat="server" ID="Q9A" SessionKey="Q9A" DBColumn="Q9A" ShowNAColumn="True" Label="Cashiers" />
					<% if (PropertyShortCode != GCCPropertyShortCode.GAG && !isChances)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9B" SessionKey="Q9B" DBColumn="Q9B" ShowNAColumn="True" Label="Guest Services" />
					<% } %>
					<% if (PropertyShortCode != GCCPropertyShortCode.GAG)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9C" SessionKey="Q9C" DBColumn="Q9C" ShowNAColumn="True" Label="Slot Attendants" />
					<% } %>
					<% if (PropertyShortCode != GCCPropertyShortCode.HA && !isChances)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9D" SessionKey="Q9D" DBColumn="Q9D" ShowNAColumn="True" Label="Dealers" />
					<% } %>
					<uc1:QuestionRowControl runat="server" ID="Q9E" SessionKey="Q9E" DBColumn="Q9E" ShowNAColumn="True" Label="Restaurant Servers" />
					<% if (!isChances)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9F" SessionKey="Q9F" DBColumn="Q9F" ShowNAColumn="True" Label="Cocktail Servers" />
					<% } %>
					<% if (PropertyShortCode != GCCPropertyShortCode.GAG)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9G" SessionKey="Q9G" DBColumn="Q9G" ShowNAColumn="True" Label="Coffee Servers" />
					<% } %>
					<uc1:QuestionRowControl runat="server" ID="Q9H" SessionKey="Q9H" DBColumn="Q9H" ShowNAColumn="True" Label="Security" />
					<uc1:QuestionRowControl runat="server" ID="Q9I" SessionKey="Q9I" DBColumn="Q9I" ShowNAColumn="True" Label="Managers/Supervisors" />
					<% if (PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNB)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9J" SessionKey="Q9J" DBColumn="Q9J" ShowNAColumn="True" Label="Hotel Staff" />
					<% } %>

					<% if (PropertyShortCode == GCCPropertyShortCode.CNSH)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9K" SessionKey="Q9K" DBColumn="Q9K" ShowNAColumn="True" Label="Buffet Servers" />
					<% } %>
				</div>
				<% } %>
				<p class="question">
					Thinking about all of the staff members that you dealt with during your visit, how would you rate them on:
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q10A" SessionKey="Q10A" DBColumn="Q10A" ShowNAColumn="True" Label="Encouraging you to take part in events or promotions" />
				<uc1:QuestionRowControl runat="server" ID="Q10B" SessionKey="Q10B" DBColumn="Q10B" ShowNAColumn="True" Label="Answering questions you had about the property or promotions" />
				<uc1:QuestionRowControl runat="server" ID="Q10C" SessionKey="Q10C" DBColumn="Q10C" Label="Being friendly and welcoming" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Please provide any additional comments or suggestions regarding <%= CasinoName %>'s staff. If you have no comments, please leave the field blank.
				</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="Q11" SessionKey="Q11" DBColumn="Q11" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="2000"></sc:SurveyTextBox>
				</div>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>





				<% }
				   else
				   { %>


				<h2>Personnel</h2>
				<p class="question">
					Quel a été votre degré de satisfaction du personnel et du niveau de service à la clientèle offert lors de votre dernière <% if (!IsKioskOrStaffEntry) %> visite au <% = CasinoName%>?
					<br />
					Veuillez évaluer les énoncés suivants :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q7A_F" SessionKey="Q7A" DBColumn="Q7A" Label="Faire en sorte que vos besoins soient comblés" />
				<uc1:QuestionRowControl runat="server" ID="Q7B_F" SessionKey="Q7B" DBColumn="Q7B" Label="Accuell" />
				<uc1:QuestionRowControl runat="server" ID="Q7C_F" SessionKey="Q7C" DBColumn="Q7C" Label="Surpasser la qualité de service attendu" />
				<uc1:QuestionRowControl runat="server" ID="Q7D_F" SessionKey="Q7D" DBColumn="Q7D" Label="Rapidité du service" />
				<uc1:QuestionRowControl runat="server" ID="Q7E_F" SessionKey="Q7E" DBColumn="Q7E" Label="Encouragement à visitez à nouveau" />
				<uc1:QuestionRowControl runat="server" ID="Q7F_F" SessionKey="Q7F" DBColumn="Q7F" Label="Disponibilité globale du personnel" />
				<p class="question">
					Dans l'ensemble, quel est votre degré de satisfaction <u>du service offert par le personnel du</u> <% = CasinoName%>?
				</p>
				<%--  <uc1:ScaleQuestionControlFrench runat="server" ID="Q8_F" SessionKey="Q8" DBColumn="Q8" />--%>

				<uc1:ScaleQuestionControlFrench runat="server" ID="Q8_FN" SessionKey="Q8" DBColumn="Q8" />


				<%  if (!IsKioskOrStaffEntry)
					{ %>
				<p class="question">
					Plus précisément, comment qualifieriez-vous chacun de ces membres du personnel que vous avez rencontrés? Veuillez évaluer votre degré de satisfaction à l'égard du personnel avec lequel vous avez communiqué :
				</p>
				<sc:MessageManager runat="server" ID="mmQ9_F"></sc:MessageManager>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Pas d'interaction</div>
				</div>
				<div class="randomize">
					<% bool isChances = PropertyShortCode == GCCPropertyShortCode.CCH || PropertyShortCode == GCCPropertyShortCode.CMR || PropertyShortCode == GCCPropertyShortCode.CDC; %>
					<uc1:QuestionRowControl runat="server" ID="Q9A_F" SessionKey="Q9A" DBColumn="Q9A" ShowNAColumn="True" Label="Caissiers" />
					<% if (PropertyShortCode != GCCPropertyShortCode.GAG && !isChances)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9B_F" SessionKey="Q9B" DBColumn="Q9B" ShowNAColumn="True" Label="Service à la clientèle" />
					<% } %>
					<% if (PropertyShortCode != GCCPropertyShortCode.GAG)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9C_F" SessionKey="Q9C" DBColumn="Q9C" ShowNAColumn="True" Label="Opérateurs de machines à sous" />
					<% } %>
					<% if (PropertyShortCode != GCCPropertyShortCode.HA && !isChances)
					   { %>
					<%--<uc1:QuestionRowControl runat="server" ID="Q9D_F" SessionKey="Q9D" DBColumn="Q9D" ShowNAColumn="True" Label="Opérateurs de machines à sous" />--%>
					<uc1:QuestionRowControl runat="server" ID="Q9D_F" SessionKey="Q9D" DBColumn="Q9D" ShowNAColumn="True" Label="Concessionnaires" />
					<% } %>
					<uc1:QuestionRowControl runat="server" ID="Q9E_F" SessionKey="Q9E" DBColumn="Q9E" ShowNAColumn="True" Label="Serveurs de restaurant" />
					<% if (!isChances)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9F_F" SessionKey="Q9F" DBColumn="Q9F" ShowNAColumn="True" Label="Serveurs de cocktails" />
					<% } %>
					<% if (PropertyShortCode != GCCPropertyShortCode.GAG)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9G_F" SessionKey="Q9G" DBColumn="Q9G" ShowNAColumn="True" Label="Serveurs de café" />
					<% } %>
					<uc1:QuestionRowControl runat="server" ID="Q9H_F" SessionKey="Q9H" DBColumn="Q9H" ShowNAColumn="True" Label="Sécurité" />
					<uc1:QuestionRowControl runat="server" ID="Q9I_F" SessionKey="Q9I" DBColumn="Q9I" ShowNAColumn="True" Label="Gérants/superviseurs" />
					<% if (PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNB)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9J_F" SessionKey="Q9J" DBColumn="Q9J" ShowNAColumn="True" Label="Personnel d'hôtel" />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.CNSH)
					   { %>
					<uc1:QuestionRowControl runat="server" ID="Q9K_F" SessionKey="Q9K" DBColumn="Q9K" ShowNAColumn="True" Label="Serveurs de buffet" />
					<% } %>
				</div>
				<% } %>
				<p class="question">
					En tenant compte de tous les membres du personnel avec qui vous avez interagi pendant votre visite, quel est votre degré de satisfaction envers les énoncés suivants :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q10A_F" SessionKey="Q10A" DBColumn="Q10A" ShowNAColumn="True" Label="Encouragement à participer à des évènements et promotions" />
				<uc1:QuestionRowControl runat="server" ID="Q10B_F" SessionKey="Q10B" DBColumn="Q10B" ShowNAColumn="True" Label="Répondre à vos questions au sujet de la propriété ou des promotions" />
				<uc1:QuestionRowControl runat="server" ID="Q10C_F" SessionKey="Q10C" DBColumn="Q10C" Label="Être chaleureux et accueillant" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Veuillez fournir tout autre commentaire ou suggestion supplémentaire concernant le personnel du <% = CasinoName%>. Si vous n'avez pas de commentaires, laissez le champ vide.
				</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="Q11_F" SessionKey="Q11" DBColumn="Q11" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="2000"></sc:SurveyTextBox>
				</div>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>


				<%} %>


				<% }
					   //===========================================================================
					   //PAGE 7 - Q12-Q13 Facilities
					   //===========================================================================
					   else if (CurrentPage == 7)
					   { %>





				<% if (strSurveyLang != "French")
				   { %>





				<h2>Facilities</h2>
				<p class="question">
					How would you rate your satisfaction level with <%= CasinoName %>'s facilities overall?
				</p>
				<uc1:ScaleQuestionControl runat="server" ID="Q12" SessionKey="Q12" DBColumn="Q12" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Please rate your satisfaction level with the following:
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q13A" ShowNAColumn="True" SessionKey="Q13A" DBColumn="Q13A" Label="Ambiance, mood, atmosphere of the environment" />
				<uc1:QuestionRowControl runat="server" ID="Q13B" ShowNAColumn="True" SessionKey="Q13B" DBColumn="Q13B" Label="Cleanliness of general areas" />
				<uc1:QuestionRowControl runat="server" ID="Q13C" ShowNAColumn="True" SessionKey="Q13C" DBColumn="Q13C" Label="Clear signage" />
				<uc1:QuestionRowControl runat="server" ID="Q13D" ShowNAColumn="True" SessionKey="Q13D" DBColumn="Q13D" Label="Washroom cleanliness" />
				<uc1:QuestionRowControl runat="server" ID="Q13E" ShowNAColumn="True" SessionKey="Q13E" DBColumn="Q13E" Label="Adequate  lighting - it is bright enough" />
				<uc1:QuestionRowControl runat="server" ID="Q13F" ShowNAColumn="True" SessionKey="Q13F" DBColumn="Q13F" Label="Safe environment" />
				<uc1:QuestionRowControl runat="server" ID="Q13G" ShowNAColumn="True" SessionKey="Q13G" DBColumn="Q13G" Label="Parking availability" />
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>


				<% }
				   else
				   { %>


				<h2>Installations</h2>
				<p class="question">
					Dans l’ensemble, quel est votre degré de satisfaction des installations du <% = CasinoName%>?
				</p>
				<uc1:ScaleQuestionControlFrench runat="server" ID="Q12_F" SessionKey="Q12" DBColumn="Q12" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Veuillez évaluer votre niveau de satisfaction avec ce qui suit:
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q13A_F" ShowNAColumn="True" SessionKey="Q13A" DBColumn="Q13A" Label="Ambiance, climat, atmosphère de l'environnement" />
				<uc1:QuestionRowControl runat="server" ID="Q13B_F" ShowNAColumn="True" SessionKey="Q13B" DBColumn="Q13B" Label="Propreté des aires générales" />
				<uc1:QuestionRowControl runat="server" ID="Q13C_F" ShowNAColumn="True" SessionKey="Q13C" DBColumn="Q13C" Label="Signalisation claire" />
				<uc1:QuestionRowControl runat="server" ID="Q13D_F" ShowNAColumn="True" SessionKey="Q13D" DBColumn="Q13D" Label="Propreté des salles de bain" />
				<uc1:QuestionRowControl runat="server" ID="Q13E_F" ShowNAColumn="True" SessionKey="Q13E" DBColumn="Q13E" Label="Éclairage adéquat – il est suffisamment lumineux" />
				<uc1:QuestionRowControl runat="server" ID="Q13F_F" ShowNAColumn="True" SessionKey="Q13F" DBColumn="Q13F" Label="Environnement sécurisé" />
				<uc1:QuestionRowControl runat="server" ID="Q13G_F" ShowNAColumn="True" SessionKey="Q13G" DBColumn="Q13G" Label="Disponibilité du stationnement" />
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>




				<%} %>


				<% }
					   //===========================================================================
					   //PAGE 8 - Q14-Q18 Gaming Experience and Food & Beverage
					   //===========================================================================
					   else if (CurrentPage == 8)
					   { %>



				<% if (strSurveyLang != "French")
				   { %>




				<h2>Gaming and Food & Beverage Experience</h2>
				<% if (radQ1_Slots.Checked || chkQ2_Slots.Checked
					   || radQ1_Tables.Checked || chkQ2_Tables.Checked
					   || radQ1_Poker.Checked || chkQ2_Poker.Checked
					   || radQ1_LiveRacing.Checked || chkQ2_LiveRacing.Checked
					   || radQ1_Racebook.Checked || chkQ2_Racebook.Checked
					   || radQ1_Bingo.Checked || chkQ2_Bingo.Checked
					   || radQ1_Lottery.Checked || chkQ2_Lottery.Checked)
				   { %>
				<p class="question">
					How would you rate your satisfaction with your primary gaming experience overall?
				</p>
				<uc1:ScaleQuestionControl runat="server" ID="Q14" SessionKey="Q14" DBColumn="Q14" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Based on your primary gaming activity. Please rate your level of satisfaction with <%= CasinoName %>'s gaming in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q15A" SessionKey="Q15A" DBColumn="Q15A" ShowNAColumn="True" Label="Variety of games available" />
				<uc1:QuestionRowControl runat="server" ID="Q15B" SessionKey="Q15B" DBColumn="Q15B" ShowNAColumn="True" Label="Waiting time to play" />
				<uc1:QuestionRowControl runat="server" ID="Q15C" SessionKey="Q15C" DBColumn="Q15C" ShowNAColumn="True" Label="Availability of specific game at your desired denomination" />
				<uc1:QuestionRowControl runat="server" ID="Q15D" SessionKey="Q15D" DBColumn="Q15D" ShowNAColumn="True" Label="Contests & monthly promotions" />
				<uc1:QuestionRowControl runat="server" ID="Q15E" SessionKey="Q15E" DBColumn="Q15E" ShowNAColumn="True" Label="Courtesy & respectfulness of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q15F" SessionKey="Q15F" DBColumn="Q15F" ShowNAColumn="True" Label="Game Knowledge of Staff" />
				<% } %>
				<% } %>
				<% if (Q4.SelectedValue == 1 && !IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					What is your overall satisfaction with the <%= PropertyTools.GetPlayersClubName( PropertyID ) %> program?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q16A" SessionKey="Q16A" DBColumn="Q16A" ShowNAColumn="True" Label="Rate of earning" />
				<uc1:QuestionRowControl runat="server" ID="Q16B" SessionKey="Q16B" DBColumn="Q16B" ShowNAColumn="True" Label="Redemption value" />
				<uc1:QuestionRowControl runat="server" ID="Q16C" SessionKey="Q16C" DBColumn="Q16C" ShowNAColumn="True" Label="Choice of rewards" />
				<% if (PropertyShortCode != GCCPropertyShortCode.GAG)
				   { %>
				<uc1:QuestionRowControl runat="server" ID="Q16D" SessionKey="Q16D" DBColumn="Q16D" ShowNAColumn="True" Label="Slot Free Play" />
				<% } %>
				<% } %>
				<p class="question">
					Did you purchase any food and/or beverages during your most recent visit to <%= CasinoName %>?
				</p>
				<uc1:YesNoControl runat="server" ID="Q17" SessionKey="Q17" DBColumn="Q17" />
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>





				<% }
				   else
				   { %>




				<h2>Expérience – jeux, nourriture et boissons</h2>
				<% if (radQ1_Slots_F.Checked || chkQ2_Slots_F.Checked
					   || radQ1_Tables_F.Checked || chkQ2_Tables_F.Checked
					   || radQ1_Poker_F.Checked || chkQ2_Poker_F.Checked
					   || radQ1_LiveRacing_F.Checked || chkQ2_LiveRacing_F.Checked
					   || radQ1_Racebook_F.Checked || chkQ2_Racebook_F.Checked
					   || radQ1_Bingo_F.Checked || chkQ2_Bingo_F.Checked
					   || radQ1_Lottery_F.Checked || chkQ2_Lottery_F.Checked)
				   { %>
				<p class="question">
					Dans l’ensemble, quel est votre degré de satisfaction de votre expérience de jeu initiale?
				</p>
				<uc1:ScaleQuestionControlFrench runat="server" ID="Q14_F" SessionKey="Q14" DBColumn="Q14" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					En fonction de votre activité de jeu principale, veuillez évaluer votre niveau de satisfaction avec les jeux du <% = CasinoName%> pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q15A_F" SessionKey="Q15A" DBColumn="Q15A" ShowNAColumn="True" Label="Variété de jeux disponibles" />
				<uc1:QuestionRowControl runat="server" ID="Q15B_F" SessionKey="Q15B" DBColumn="Q15B" ShowNAColumn="True" Label="Temps d'attente pour jouer" />
				<uc1:QuestionRowControl runat="server" ID="Q15C_F" SessionKey="Q15C" DBColumn="Q15C" ShowNAColumn="True" Label="Disponibilité du jeu spécifique à votre dénomination souhaitée" />
				<uc1:QuestionRowControl runat="server" ID="Q15D_F" SessionKey="Q15D" DBColumn="Q15D" ShowNAColumn="True" Label="Concours et promotions mensuelles" />
				<uc1:QuestionRowControl runat="server" ID="Q15E_F" SessionKey="Q15E" DBColumn="Q15E" ShowNAColumn="True" Label="Courtoisie et respect du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q15F_F" SessionKey="Q15F" DBColumn="Q15F" ShowNAColumn="True" Label="Niveau de connaissance du personnel" />
				<% } %>
				<% } %>
				<% if (Q4.SelectedValue == 1 && !IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Quelle est votre satisfaction globale avec le programme du <% = PropertyTools.GetPlayersClubName_French (PropertyID)%>?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q16A_F" SessionKey="Q16A" DBColumn="Q16A" ShowNAColumn="True" Label="Vitesse d’accumulation de récompenses" />
				<uc1:QuestionRowControl runat="server" ID="Q16B_F" SessionKey="Q16B" DBColumn="Q16B" ShowNAColumn="True" Label="Valeur d’échange" />
				<uc1:QuestionRowControl runat="server" ID="Q16C_F" SessionKey="Q16C" DBColumn="Q16C" ShowNAColumn="True" Label="Choix de récompenses" />
				<% if (PropertyShortCode != GCCPropertyShortCode.GAG)
				   { %>
				<uc1:QuestionRowControl runat="server" ID="Q16D_F" SessionKey="Q16D" DBColumn="Q16D" ShowNAColumn="True" Label="Crédit pour machines à sous" />
				<% } %>
				<% } %>
				<p class="question">
					Avez-vous acheté de la nourriture ou des boissons pendant votre plus récente visite au  <% = CasinoName%>?
				</p>
				<uc1:YesNoControlFrench runat="server" ID="Q17_F" SessionKey="Q17" DBColumn="Q17" />
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>


				<%} %>


				<% }
					   //===========================================================================
					   //PAGE 9 - Q18
					   //===========================================================================
					   else if (CurrentPage == 9)
					   { %>






				<% if (strSurveyLang != "French")
				   { %>






				<% if (Q17.SelectedValue == 1)
				   { %>
				<p class="question">
					Where did you purchase food and/or beverage? Select all that apply:
				</p>
				<p>
					<% if (PropertyShortCode != GCCPropertyShortCode.GAG)
					   { %>
					<sc:SurveyCheckBox ID="Q18_1" runat="server" SessionKey="Q18_1" DBColumn="Q18_1" DBValue="1" Text="&nbsp;Coffee Station / Gaming Floor" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.RR)
					   { %>
					<sc:SurveyCheckBox ID="Q18_2" runat="server" SessionKey="Q18_2" DBColumn="Q18_2" DBValue="1" Text="&nbsp;Curve" /><br />
					<sc:SurveyCheckBox ID="Q18_3" runat="server" SessionKey="Q18_3" DBColumn="Q18_3" DBValue="1" Text="&nbsp;Tramonto Restaurant" /><br />
					<!-- <sc:SurveyCheckBox ID="Q18_4" runat="server" SessionKey="Q18_4" DBColumn="Q18_4" DBValue="1" Text="&nbsp;Lulu's Lounge" /><br /> -->
					<sc:SurveyCheckBox ID="Q18_5" runat="server" SessionKey="Q18_5" DBColumn="Q18_5" DBValue="1" Text="&nbsp;The Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_6" runat="server" SessionKey="Q18_6" DBColumn="Q18_6" DBValue="1" Text="&nbsp;Sea Harbour Seafood Restaurant" /><br />
					<sc:SurveyCheckBox ID="Q18_7" runat="server" SessionKey="Q18_7" DBColumn="Q18_7" DBValue="1" Text="&nbsp;Java Jacks Café" /><br />
					<sc:SurveyCheckBox ID="Q18_8" runat="server" SessionKey="Q18_8" DBColumn="Q18_8" DBValue="1" Text="&nbsp;International Food Court" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.HRCV)
					   { %>
					<sc:SurveyCheckBox ID="Q18_14" runat="server" SessionKey="Q18_14" DBColumn="Q18_14" DBValue="1" Text="&nbsp;Asylum Gastro-Pub and Live Sound Stage" /><br />
					<sc:SurveyCheckBox ID="Q18_15" runat="server" SessionKey="Q18_15" DBColumn="Q18_15" DBValue="1" Text="&nbsp;Unlisted Buffet and Lounge" /><br />
					<sc:SurveyCheckBox ID="Q18_16" runat="server" SessionKey="Q18_16" DBColumn="Q18_16" DBValue="1" Text="&nbsp;Neptune Noodle House" /><br />
					<sc:SurveyCheckBox ID="Q18_17" runat="server" SessionKey="Q18_17" DBColumn="Q18_17" DBValue="1" Text="&nbsp;Fu Express Authentic Asian Cuisine" /><br />
					<sc:SurveyCheckBox ID="Q18_18" runat="server" SessionKey="Q18_18" DBColumn="Q18_18" DBValue="1" Text="&nbsp;Roadies Burger Bar" /><br />
					<sc:SurveyCheckBox ID="Q18_19" runat="server" SessionKey="Q18_19" DBColumn="Q18_19" DBValue="1" Text="&nbsp;Chip's Sandwich Shop" /><br />
					<sc:SurveyCheckBox ID="Q18_20" runat="server" SessionKey="Q18_20" DBColumn="Q18_20" DBValue="1" Text="&nbsp;Fuel Café" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.HRCV || PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNSH)
					   { %>
					<sc:SurveyCheckBox ID="Q18_9" runat="server" SessionKey="Q18_9" DBColumn="Q18_9" DBValue="1" Text="&nbsp;Poker & Ponies Room" /><br />
					<% if (PropertyShortCode != GCCPropertyShortCode.CNSH)
					   { %>
					<sc:SurveyCheckBox ID="Q18_10" runat="server" SessionKey="Q18_10" DBColumn="Q18_10" DBValue="1" Text="&nbsp;Salon Privé" /><br />
					<% } %>
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.RR)
					   { %>
					<sc:SurveyCheckBox ID="Q18_11" runat="server" SessionKey="Q18_11" DBColumn="Q18_11" DBValue="1" Text="&nbsp;Dogwood Room" /><br />
					<sc:SurveyCheckBox ID="Q18_12" runat="server" SessionKey="Q18_12" DBColumn="Q18_12" DBValue="1" Text="&nbsp;Jade Room" /><br />
					<sc:SurveyCheckBox ID="Q18_13" runat="server" SessionKey="Q18_13" DBColumn="Q18_13" DBValue="1" Text="&nbsp;Phoenix Room" /><br />
					<% } %>
					<%--<% if (PropertyShortCode == GCCPropertyShortCode.FD || PropertyShortCode == GCCPropertyShortCode.EC) { %>--%>
					<% if (PropertyShortCode == GCCPropertyShortCode.EC)
					   { %>
					<sc:SurveyCheckBox ID="Q18_21" runat="server" SessionKey="Q18_21" DBColumn="Q18_21" DBValue="1" Text="&nbsp;The Homestretch Buffet" /><br />
					<% } %>
					<%--<% if (PropertyShortCode == GCCPropertyShortCode.FD) { %>
					<sc:SurveyCheckBox ID="Q18_22" runat="server" SessionKey="Q18_22" DBColumn="Q18_22" DBValue="1" Text="&nbsp;The Bridge" /><br />
					<sc:SurveyCheckBox ID="Q18_23" runat="server" SessionKey="Q18_23" DBColumn="Q18_23" DBValue="1" Text="&nbsp;The Clubhouse Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_24" runat="server" SessionKey="Q18_24" DBColumn="Q18_24" DBValue="1" Text="&nbsp;Casino Bar" /><br />
					<% } %>--%>
					<%--<% if (PropertyShortCode == GCCPropertyShortCode.FD || PropertyShortCode == GCCPropertyShortCode.EC) { %>--%>
					<% if (PropertyShortCode == GCCPropertyShortCode.EC)
					   { %>
					<sc:SurveyCheckBox ID="Q18_25" runat="server" SessionKey="Q18_25" DBColumn="Q18_25" DBValue="1" Text="&nbsp;Racebook" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.HA)
					   { %>
					<sc:SurveyCheckBox ID="Q18_26" runat="server" SessionKey="Q18_26" DBColumn="Q18_26" DBValue="1" Text="&nbsp;Eclipse Lounge" /><br />
					<sc:SurveyCheckBox ID="Q18_27" runat="server" SessionKey="Q18_27" DBColumn="Q18_27" DBValue="1" Text="&nbsp;Silks Restaurant" /><br />
					<sc:SurveyCheckBox ID="Q18_28" runat="server" SessionKey="Q18_28" DBColumn="Q18_28" DBValue="1" Text="&nbsp;Furlongs Eatery" /><br />
					<sc:SurveyCheckBox ID="Q18_29" runat="server" SessionKey="Q18_29" DBColumn="Q18_29" DBValue="1" Text="&nbsp;Jeromes" /><br />
					<sc:SurveyCheckBox ID="Q18_30" runat="server" SessionKey="Q18_30" DBColumn="Q18_30" DBValue="1" Text="&nbsp;George Royal Room" /><br />
					<sc:SurveyCheckBox ID="Q18_31" runat="server" SessionKey="Q18_31" DBColumn="Q18_31" DBValue="1" Text="&nbsp;Concessions" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.VRL)
					   { %>
					<sc:SurveyCheckBox ID="Q18_32" runat="server" SessionKey="Q18_32" DBColumn="Q18_32" DBValue="1" Text="&nbsp;View Royal Restaurant" /><br />
					<sc:SurveyCheckBox ID="Q18_33" runat="server" SessionKey="Q18_33" DBColumn="Q18_33" DBValue="1" Text="&nbsp;View Royal Patio" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.NAN)
					   { %>
					<sc:SurveyCheckBox ID="Q18_34" runat="server" SessionKey="Q18_34" DBColumn="Q18_34" DBValue="1" Text="&nbsp;Black Diamond Bar & Grille" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.CCH || PropertyShortCode == GCCPropertyShortCode.CMR)
					   { %>
					<sc:SurveyCheckBox ID="Q18_35" runat="server" SessionKey="Q18_35" DBColumn="Q18_35" DBValue="1" Text="&nbsp;The Well" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.CDC)
					   { %>
					<sc:SurveyCheckBox ID="Q18_36" runat="server" SessionKey="Q18_36" DBColumn="Q18_36" DBValue="1" Text="&nbsp;Prospects Lounge" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.CNSH)
					   { %>

					<sc:SurveyCheckBox ID="Q18_40" runat="server" SessionKey="Q18_40" DBColumn="Q18_40" DBValue="1" Text="&nbsp;High Limit Gaming Area" /><br />
					<sc:SurveyCheckBox ID="Q18_37" runat="server" SessionKey="Q18_37" DBColumn="Q18_37" DBValue="1" Text="&nbsp;3Sixty Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_38" runat="server" SessionKey="Q18_38" DBColumn="Q18_38" DBValue="1" Text="&nbsp;3Sixty Restaurant" /><br />
					<% } %>


					<%if(PropertyShortCode == GCCPropertyShortCode.None){ %>

					<sc:SurveyCheckBox ID="Q18_39" runat="server" SessionKey="Q18_39" DBColumn="Q18_39" DBValue="1" Text="&nbsp;Harbourfront Lounge" /><br />
					<%} %>


					<% if (PropertyShortCode == GCCPropertyShortCode.CNSS)
					   { %>
					<sc:SurveyCheckBox ID="Q18_41" runat="server" SessionKey="Q18_41" DBColumn="Q18_41" DBValue="1" Text="&nbsp;Celtic Junction Bar & Grille" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.GAG)
					   { %>
					<sc:SurveyCheckBox ID="Q18_42" runat="server" SessionKey="Q18_42" DBColumn="Q18_42" DBValue="1" Text="&nbsp;Bar / Restuarant at Great American Casino" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.EC)
					   { %>
					<sc:SurveyCheckBox ID="Q18_43" runat="server" SessionKey="Q18_43" DBColumn="Q18_43" DBValue="1" Text="&nbsp;Diamond Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_44" runat="server" SessionKey="Q18_44" DBColumn="Q18_44" DBValue="1" Text="&nbsp;Foodies" /><br />
					<sc:SurveyCheckBox ID="Q18_45" runat="server" SessionKey="Q18_45" DBColumn="Q18_45" DBValue="1" Text="&nbsp;Molson Lounge" /><br />
					<sc:SurveyCheckBox ID="Q18_46" runat="server" SessionKey="Q18_46" DBColumn="Q18_46" DBValue="1" Text="&nbsp;Escape" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.SSKD)
					   { %>
					<sc:SurveyCheckBox ID="Q18_47" runat="server" SessionKey="Q18_47" DBColumn="Q18_47" DBValue="1" Text="&nbsp;Player's Lounge and Café" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.SCTI)
					   { %>
					<sc:SurveyCheckBox ID="Q18_48" runat="server" SessionKey="Q18_48" DBColumn="Q18_48" DBValue="1" Text="&nbsp;Windward Restaurant & Lounge" /><br />
					<% } %>
					<%if (PropertyShortCode == GCCPropertyShortCode.CNB)
					  { %>
					<sc:SurveyCheckBox ID="Q18_49" runat="server" SessionKey="Q18_49" DBColumn="Q18_49" DBValue="1" Text="&nbsp;Rue 333" /><br />
					<sc:SurveyCheckBox ID="Q18_50" runat="server" SessionKey="Q18_50" DBColumn="Q18_50" DBValue="1" Text="&nbsp;Hub City Pub" /><br />
					<sc:SurveyCheckBox ID="Q18_51" runat="server" SessionKey="Q18_51" DBColumn="Q18_51" DBValue="1" Text="&nbsp;Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_52" runat="server" SessionKey="Q18_52" DBColumn="Q18_52" DBValue="1" Text="&nbsp;Room Service" /><br />
					<% } %>
					<%if (PropertyShortCode == GCCPropertyShortCode.SCBE)
					  { %>
					<sc:SurveyCheckBox ID="Q18_53" runat="server" SessionKey="Q18_53" DBColumn="Q18_53" DBValue="1" Text="&nbsp;Windward Restaurant and Lounge" /><br />
					<sc:SurveyCheckBox ID="Q18_54" runat="server" SessionKey="Q18_54" DBColumn="Q18_54" DBValue="1" Text="&nbsp;The Buffet" /><br />
					<% } %>
					<%if (PropertyShortCode == GCCPropertyShortCode.WDB)
					  { %>
					<sc:SurveyCheckBox ID="Q18_55" runat="server" SessionKey="Q18_55" DBColumn="Q18_55" DBValue="1" Text="&nbsp;Willows Dining Room" /><br />
					<sc:SurveyCheckBox ID="Q18_56" runat="server" SessionKey="Q18_56" DBColumn="Q18_56" DBValue="1" Text="&nbsp;Willows Noodle Bar" /><br />
					<sc:SurveyCheckBox ID="Q18_57" runat="server" SessionKey="Q18_57" DBColumn="Q18_57" DBValue="1" Text="&nbsp;Hoofbeats Lounge" /><br />
					<% } %>
					<%if (PropertyShortCode == GCCPropertyShortCode.AJA)
					  { %>
					<sc:SurveyCheckBox ID="Q18_58" runat="server" SessionKey="Q18_58" DBColumn="Q18_58" DBValue="1" Text="&nbsp;Getaway Restaurant" /><br />

					<% } %>
					<%if (PropertyShortCode == GCCPropertyShortCode.GBH)
					  { %>
					<sc:SurveyCheckBox ID="Q18_59" runat="server" SessionKey="Q18_59" DBColumn="Q18_59" DBValue="1" Text="&nbsp;Waters Edge Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_60" runat="server" SessionKey="Q18_60" DBColumn="Q18_60" DBValue="1" Text="&nbsp;Lucky Stone Bar & Grill" /><br />
					<sc:SurveyCheckBox ID="Q18_61" runat="server" SessionKey="Q18_61" DBColumn="Q18_61" DBValue="1" Text="&nbsp;Heron Bar" /><br />
					<sc:SurveyCheckBox ID="Q18_62" runat="server" SessionKey="Q18_62" DBColumn="Q18_62" DBValue="1" Text="&nbsp;Game Side Dinning" /><br />


					<% } %>
				</p>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>


				<% }
				   else
				   { %>


				<% if (Q17_F.SelectedValue_F == 1)
				   { %>
				<p class="question">
					Où avez-vous acheté de la nourriture ou des boissons? Sélectionnez les choix qui s’appliquent:
				</p>
				<p>
					<% if (PropertyShortCode != GCCPropertyShortCode.GAG)
					   { %>
					<sc:SurveyCheckBox ID="Q18_1_F" runat="server" SessionKey="Q18_1" DBColumn="Q18_1" DBValue="1" Text="&nbsp;Station de café/salle de jeu" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.RR)
					   { %>
					<sc:SurveyCheckBox ID="Q18_2_F" runat="server" SessionKey="Q18_2" DBColumn="Q18_2" DBValue="1" Text="&nbsp;Courbe" /><br />
					<sc:SurveyCheckBox ID="Q18_3_F" runat="server" SessionKey="Q18_3" DBColumn="Q18_3" DBValue="1" Text="&nbsp;Tramonto Restaurant" /><br />
					<sc:SurveyCheckBox ID="Q18_4_F" runat="server" SessionKey="Q18_4" DBColumn="Q18_4" DBValue="1" Text="&nbsp;Lulu's Lounge" /><br />
					<sc:SurveyCheckBox ID="Q18_5_F" runat="server" SessionKey="Q18_5" DBColumn="Q18_5" DBValue="1" Text="&nbsp;Le Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_6_F" runat="server" SessionKey="Q18_6" DBColumn="Q18_6" DBValue="1" Text="&nbsp;Sea Harbour Fruits de mer" /><br />
					<sc:SurveyCheckBox ID="Q18_7_F" runat="server" SessionKey="Q18_7" DBColumn="Q18_7" DBValue="1" Text="&nbsp;Java Jacks Café" /><br />
					<sc:SurveyCheckBox ID="Q18_8_F" runat="server" SessionKey="Q18_8" DBColumn="Q18_8" DBValue="1" Text="&nbsp;Cour internationale de l'alimentation" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.HRCV)
					   { %>
					<sc:SurveyCheckBox ID="Q18_14_F" runat="server" SessionKey="Q18_14" DBColumn="Q18_14" DBValue="1" Text="&nbsp;Asylum Gastro-Pub et scène sonore en direct" /><br />
					<sc:SurveyCheckBox ID="Q18_15_F" runat="server" SessionKey="Q18_15" DBColumn="Q18_15" DBValue="1" Text="&nbsp;Buffet et salon non cotés" /><br />
					<sc:SurveyCheckBox ID="Q18_16_F" runat="server" SessionKey="Q18_16" DBColumn="Q18_16" DBValue="1" Text="&nbsp;Neptune Noodle House" /><br />
					<sc:SurveyCheckBox ID="Q18_17_F" runat="server" SessionKey="Q18_17" DBColumn="Q18_17" DBValue="1" Text="&nbsp;FFu Express cuisine asiatique authentique" /><br />
					<sc:SurveyCheckBox ID="Q18_18_F" runat="server" SessionKey="Q18_18" DBColumn="Q18_18" DBValue="1" Text="&nbsp;Roadies Burger Bar" /><br />
					<sc:SurveyCheckBox ID="Q18_19_F" runat="server" SessionKey="Q18_19" DBColumn="Q18_19" DBValue="1" Text="&nbsp;Boutique Sandwich" /><br />
					<sc:SurveyCheckBox ID="Q18_20_F" runat="server" SessionKey="Q18_20" DBColumn="Q18_20" DBValue="1" Text="&nbsp;Carburant, café" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.HRCV || PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.CNSH)
					   { %>
					<sc:SurveyCheckBox ID="Q18_9_F" runat="server" SessionKey="Q18_9" DBColumn="Q18_9" DBValue="1" Text="&nbsp;Salle de Poker & Poneys" /><br />
					<% if (PropertyShortCode != GCCPropertyShortCode.CNSH)
					   { %>
					<sc:SurveyCheckBox ID="Q18_10_F" runat="server" SessionKey="Q18_10" DBColumn="Q18_10" DBValue="1" Text="&nbsp;Salon Privé" /><br />
					<% } %>
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.RR)
					   { %>
					<sc:SurveyCheckBox ID="Q18_11_F" runat="server" SessionKey="Q18_11" DBColumn="Q18_11" DBValue="1" Text="&nbsp;Chambre Dogwood" /><br />
					<sc:SurveyCheckBox ID="Q18_12_F" runat="server" SessionKey="Q18_12" DBColumn="Q18_12" DBValue="1" Text="&nbsp;Chambre Jade" /><br />
					<sc:SurveyCheckBox ID="Q18_13_F" runat="server" SessionKey="Q18_13" DBColumn="Q18_13" DBValue="1" Text="&nbsp;Chambre Phoenix" /><br />
					<% } %>
					<%--<% if (PropertyShortCode == GCCPropertyShortCode.FD || PropertyShortCode == GCCPropertyShortCode.EC) { %>--%>
					<% if (PropertyShortCode == GCCPropertyShortCode.EC)
					   { %>
					<sc:SurveyCheckBox ID="Q18_21_F" runat="server" SessionKey="Q18_21" DBColumn="Q18_21" DBValue="1" Text="&nbsp;Le Buffet Homestretch" /><br />
					<% } %>
					<%--<% if (PropertyShortCode == GCCPropertyShortCode.FD) { %>
					<sc:SurveyCheckBox ID="Q18_22" runat="server" SessionKey="Q18_22" DBColumn="Q18_22" DBValue="1" Text="&nbsp;The Bridge" /><br />
					<sc:SurveyCheckBox ID="Q18_23" runat="server" SessionKey="Q18_23" DBColumn="Q18_23" DBValue="1" Text="&nbsp;The Clubhouse Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_24" runat="server" SessionKey="Q18_24" DBColumn="Q18_24" DBValue="1" Text="&nbsp;Casino Bar" /><br />
					<% } %>--%>
					<%--<% if (PropertyShortCode == GCCPropertyShortCode.FD || PropertyShortCode == GCCPropertyShortCode.EC) { %>--%>
					<% if (PropertyShortCode == GCCPropertyShortCode.EC)
					   { %>
					<sc:SurveyCheckBox ID="Q18_25_F" runat="server" SessionKey="Q18_25" DBColumn="Q18_25" DBValue="1" Text="&nbsp;Carnet de course" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.HA)
					   { %>
					<sc:SurveyCheckBox ID="Q18_26_F" runat="server" SessionKey="Q18_26" DBColumn="Q18_26" DBValue="1" Text="&nbsp;Eclipse Lounge" /><br />
					<sc:SurveyCheckBox ID="Q18_27_F" runat="server" SessionKey="Q18_27" DBColumn="Q18_27" DBValue="1" Text="&nbsp;Silks Restaurant" /><br />
					<sc:SurveyCheckBox ID="Q18_28_F" runat="server" SessionKey="Q18_28" DBColumn="Q18_28" DBValue="1" Text="&nbsp;Furlongs Eatery" /><br />
					<sc:SurveyCheckBox ID="Q18_29_F" runat="server" SessionKey="Q18_29" DBColumn="Q18_29" DBValue="1" Text="&nbsp;Jeromes" /><br />
					<sc:SurveyCheckBox ID="Q18_30_F" runat="server" SessionKey="Q18_30" DBColumn="Q18_30" DBValue="1" Text="&nbsp;Chambre George Royal" /><br />
					<sc:SurveyCheckBox ID="Q18_31_F" runat="server" SessionKey="Q18_31" DBColumn="Q18_31" DBValue="1" Text="&nbsp;Concessions" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.VRL)
					   { %>
					<sc:SurveyCheckBox ID="Q18_32_F" runat="server" SessionKey="Q18_32" DBColumn="Q18_32" DBValue="1" Text="&nbsp;Voir Royal Restaurant" /><br />
					<sc:SurveyCheckBox ID="Q18_33_F" runat="server" SessionKey="Q18_33" DBColumn="Q18_33" DBValue="1" Text="&nbsp;Voir Royal Patio" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.NAN)
					   { %>
					<sc:SurveyCheckBox ID="Q18_34_F" runat="server" SessionKey="Q18_34" DBColumn="Q18_34" DBValue="1" Text="&nbsp;Black Diamond Bar & Grille" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.CCH || PropertyShortCode == GCCPropertyShortCode.CMR)
					   { %>
					<sc:SurveyCheckBox ID="Q18_35_F" runat="server" SessionKey="Q18_35" DBColumn="Q18_35" DBValue="1" Text="&nbsp;Le puits" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.CDC)
					   { %>
					<sc:SurveyCheckBox ID="Q18_36_F" runat="server" SessionKey="Q18_36" DBColumn="Q18_36" DBValue="1" Text="&nbsp;Salon Perspectives" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.CNSH)
					   { %>

					<sc:SurveyCheckBox ID="Q18_40_F" runat="server" SessionKey="Q18_40" DBColumn="Q18_40" DBValue="1" Text="&nbsp;Zone de jeu à limites élevées" /><br />
					<sc:SurveyCheckBox ID="Q18_37_F" runat="server" SessionKey="Q18_37" DBColumn="Q18_37" DBValue="1" Text="&nbsp;3Sixty Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_38_F" runat="server" SessionKey="Q18_38" DBColumn="Q18_38" DBValue="1" Text="&nbsp;3Sixty Restaurant" /><br />
					<% } %>

					<%if(PropertyShortCode == GCCPropertyShortCode.None) { %>

					<sc:SurveyCheckBox ID="Q18_39_F" runat="server" SessionKey="Q18_39" DBColumn="Q18_39" DBValue="1" Text="&nbsp;Salon du Harbourfront" /><br />
					<%} %>

					<% if (PropertyShortCode == GCCPropertyShortCode.CNSS)
					   { %>
					<sc:SurveyCheckBox ID="Q18_41_F" runat="server" SessionKey="Q18_41" DBColumn="Q18_41" DBValue="1" Text="&nbsp;Celtic Junction Bar & Grille" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.GAG)
					   { %>
					<sc:SurveyCheckBox ID="Q18_42_F" runat="server" SessionKey="Q18_42" DBColumn="Q18_42" DBValue="1" Text="&nbsp;Bar / Restaurant au Great American Casino" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.EC)
					   { %>
					<sc:SurveyCheckBox ID="Q18_43_F" runat="server" SessionKey="Q18_43" DBColumn="Q18_43" DBValue="1" Text="&nbsp;Buffet de diamants" /><br />
					<sc:SurveyCheckBox ID="Q18_44_F" runat="server" SessionKey="Q18_44" DBColumn="Q18_44" DBValue="1" Text="&nbsp;Gourmandises" /><br />
					<sc:SurveyCheckBox ID="Q18_45_F" runat="server" SessionKey="Q18_45" DBColumn="Q18_45" DBValue="1" Text="&nbsp;Molson Lounge" /><br />
					<sc:SurveyCheckBox ID="Q18_46_F" runat="server" SessionKey="Q18_46" DBColumn="Q18_46" DBValue="1" Text="&nbsp;Échapper" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.SSKD)
					   { %>
					<sc:SurveyCheckBox ID="Q18_47_F" runat="server" SessionKey="Q18_47" DBColumn="Q18_47" DBValue="1" Text="&nbsp;Salon des joueurs et Café" /><br />
					<% } %>
					<% if (PropertyShortCode == GCCPropertyShortCode.SCTI)
					   { %>
					<sc:SurveyCheckBox ID="Q18_48_F" runat="server" SessionKey="Q18_48" DBColumn="Q18_48" DBValue="1" Text="&nbsp;Restaurant et Lounge Windward" /><br />
					<% } %>
					<%if (PropertyShortCode == GCCPropertyShortCode.CNB)
					  { %>
					<sc:SurveyCheckBox ID="Q18_49_F" runat="server" SessionKey="Q18_49" DBColumn="Q18_49" DBValue="1" Text="&nbsp;Rue 333" /><br />
					<sc:SurveyCheckBox ID="Q18_50_F" runat="server" SessionKey="Q18_50" DBColumn="Q18_50" DBValue="1" Text="&nbsp;Pub Hub City" /><br />
					<sc:SurveyCheckBox ID="Q18_51_F" runat="server" SessionKey="Q18_51" DBColumn="Q18_51" DBValue="1" Text="&nbsp;Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_52_F" runat="server" SessionKey="Q18_52" DBColumn="Q18_52" DBValue="1" Text="&nbsp;Service à la chambre" /><br />
					<% } %>

					<%if (PropertyShortCode == GCCPropertyShortCode.SCBE)
					  { %>
					<sc:SurveyCheckBox ID="Q18_53_F" runat="server" SessionKey="Q18_53" DBColumn="Q18_53" DBValue="1" Text="&nbsp;Restaurant et Lounge Windward" /><br />
					<sc:SurveyCheckBox ID="Q18_54_F" runat="server" SessionKey="Q18_54" DBColumn="Q18_54" DBValue="1" Text="&nbsp;The Buffet" /><br />
					<% } %>

					<%if (PropertyShortCode == GCCPropertyShortCode.WDB)
					  { %>
					<sc:SurveyCheckBox ID="Q18_55_F" runat="server" SessionKey="Q18_55" DBColumn="Q18_55" DBValue="1" Text="&nbsp;Willows Dining Room" /><br />
					<sc:SurveyCheckBox ID="Q18_56_F" runat="server" SessionKey="Q18_56" DBColumn="Q18_56" DBValue="1" Text="&nbsp;Willows Noodle Bar" /><br />
					<sc:SurveyCheckBox ID="Q18_57_F" runat="server" SessionKey="Q18_57" DBColumn="Q18_57" DBValue="1" Text="&nbsp;Hoofbeats Lounge" /><br />
					<% } %>
					<%if (PropertyShortCode == GCCPropertyShortCode.AJA)
					  { %>
					<sc:SurveyCheckBox ID="Q18_58_F" runat="server" SessionKey="Q18_58" DBColumn="Q18_58" DBValue="1" Text="&nbsp;Getaway Restaurant" /><br />

					<% } %>
					<%if (PropertyShortCode == GCCPropertyShortCode.GBH)
					  { %>
					<sc:SurveyCheckBox ID="Q18_59_F" runat="server" SessionKey="Q18_59" DBColumn="Q18_59" DBValue="1" Text="&nbsp;Waters Edge Buffet" /><br />
					<sc:SurveyCheckBox ID="Q18_60_F" runat="server" SessionKey="Q18_60" DBColumn="Q18_60" DBValue="1" Text="&nbsp;Lucky Stone Bar & Grill" /><br />
					<sc:SurveyCheckBox ID="Q18_61_F" runat="server" SessionKey="Q18_61" DBColumn="Q18_61" DBValue="1" Text="&nbsp;Heron Bar" /><br />
					<sc:SurveyCheckBox ID="Q18_62_F" runat="server" SessionKey="Q18_62" DBColumn="Q18_62" DBValue="1" Text="&nbsp;Game Side Dinning" /><br />


					<% } %>
				</p>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>




				<%} %>


				<% }
					   //===========================================================================
					   //PAGE 10 - Q19-Q20 * 13 - F&B Multi-mention Questions
					   //===========================================================================
					   else if (CurrentPage == 10)
					   { %>



				<% if (strSurveyLang != "French")
				   { %>



				<% if (Q18_1.Checked || Q18_42.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(1) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(1) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M1" runat="server" SessionKey="Q19_M1" DBColumn="Q19_M1" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(1) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M1" SessionKey="Q20A_M1" DBColumn="Q20A_M1" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M1" SessionKey="Q20B_M1" DBColumn="Q20B_M1" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M1" SessionKey="Q20C_M1" DBColumn="Q20C_M1" ShowNAColumn="True" Label="Courtesy of staff" />


				<uc1:QuestionRowControl runat="server" ID="Q20D_M1" SessionKey="Q20D_M1" DBColumn="Q20D_M1" ShowNAColumn="True" Label="Timely delivery of order" />

				<uc1:QuestionRowControl runat="server" ID="Q20E_M1" SessionKey="Q20E_M1" DBColumn="Q20E_M1" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M1" SessionKey="Q20F_M1" DBColumn="Q20F_M1" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M1" SessionKey="Q20G_M1" DBColumn="Q20G_M1" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<% if (Q18_2.Checked || Q18_14.Checked || Q18_21.Checked || Q18_26.Checked || Q18_32.Checked || Q18_34.Checked || Q18_35.Checked || Q18_36.Checked || Q18_37.Checked || Q18_41.Checked || Q18_47.Checked || Q18_48.Checked || Q18_49.Checked || Q18_53.Checked || Q18_55.Checked || Q18_58.Checked || Q18_59.Checked )
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(2) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(2) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M2" runat="server" SessionKey="Q19_M2" DBColumn="Q19_M2" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(2) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M2" SessionKey="Q20A_M2" DBColumn="Q20A_M2" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M2" SessionKey="Q20B_M2" DBColumn="Q20B_M2" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M2" SessionKey="Q20C_M2" DBColumn="Q20C_M2" ShowNAColumn="True" Label="Courtesy of staff" />

				<% if(PropertyShortCode != GCCPropertyShortCode.CNSH) {  %>
				<uc1:QuestionRowControl runat="server" ID="Q20D_M2" SessionKey="Q20D_M2" DBColumn="Q20D_M2" ShowNAColumn="True" Label="Timely delivery of order" />
				<%} %>
				<uc1:QuestionRowControl runat="server" ID="Q20E_M2" SessionKey="Q20E_M2" DBColumn="Q20E_M2" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M2" SessionKey="Q20F_M2" DBColumn="Q20F_M2" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M2" SessionKey="Q20G_M2" DBColumn="Q20G_M2" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<%--<% if (Q18_3.Checked || Q18_15.Checked || Q18_22.Checked || ( Q18_25.Checked && PropertyShortCode == GCCPropertyShortCode.EC ) || Q18_27.Checked || Q18_33.Checked || Q18_38.Checked) { %>--%>
				<% if (Q18_3.Checked || Q18_15.Checked || (Q18_25.Checked && PropertyShortCode == GCCPropertyShortCode.EC) || Q18_27.Checked || Q18_33.Checked || Q18_38.Checked || Q18_50.Checked || Q18_54.Checked || Q18_56.Checked || Q18_60.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(3) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(3) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M3" runat="server" SessionKey="Q19_M3" DBColumn="Q19_M3" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(3) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M3" SessionKey="Q20A_M3" DBColumn="Q20A_M3" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M3" SessionKey="Q20B_M3" DBColumn="Q20B_M3" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M3" SessionKey="Q20C_M3" DBColumn="Q20C_M3" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M3" SessionKey="Q20D_M3" DBColumn="Q20D_M3" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M3" SessionKey="Q20E_M3" DBColumn="Q20E_M3" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M3" SessionKey="Q20F_M3" DBColumn="Q20F_M3" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M3" SessionKey="Q20G_M3" DBColumn="Q20G_M3" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<%-- <% if (Q18_4.Checked || Q18_16.Checked || Q18_23.Checked || Q18_28.Checked || Q18_39.Checked || Q18_43.Checked) { %>--%>
				<% if (Q18_4.Checked || Q18_16.Checked || Q18_28.Checked || Q18_39.Checked || Q18_43.Checked || Q18_51.Checked || Q18_57.Checked || Q18_61.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(4) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(4) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M4" runat="server" SessionKey="Q19_M4" DBColumn="Q19_M4" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(4) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M4" SessionKey="Q20A_M4" DBColumn="Q20A_M4" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M4" SessionKey="Q20B_M4" DBColumn="Q20B_M4" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M4" SessionKey="Q20C_M4" DBColumn="Q20C_M4" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M4" SessionKey="Q20D_M4" DBColumn="Q20D_M4" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M4" SessionKey="Q20E_M4" DBColumn="Q20E_M4" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M4" SessionKey="Q20F_M4" DBColumn="Q20F_M4" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M4" SessionKey="Q20G_M4" DBColumn="Q20G_M4" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<%--  <% if (Q18_5.Checked || Q18_17.Checked || Q18_24.Checked || Q18_29.Checked || Q18_40.Checked || Q18_44.Checked) { %>--%>
				<% if (Q18_5.Checked || Q18_17.Checked || Q18_29.Checked || Q18_40.Checked || Q18_44.Checked || Q18_52.Checked  || Q18_62.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(5) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(5) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M5" runat="server" SessionKey="Q19_M5" DBColumn="Q19_M5" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(5) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M5" SessionKey="Q20A_M5" DBColumn="Q20A_M5" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M5" SessionKey="Q20B_M5" DBColumn="Q20B_M5" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M5" SessionKey="Q20C_M5" DBColumn="Q20C_M5" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M5" SessionKey="Q20D_M5" DBColumn="Q20D_M5" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M5" SessionKey="Q20E_M5" DBColumn="Q20E_M5" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M5" SessionKey="Q20F_M5" DBColumn="Q20F_M5" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M5" SessionKey="Q20G_M5" DBColumn="Q20G_M5" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<% if (Q18_6.Checked || Q18_18.Checked || (Q18_25.Checked && PropertyShortCode != GCCPropertyShortCode.EC) || Q18_30.Checked || (Q18_9.Checked && PropertyShortCode == GCCPropertyShortCode.CNSH) || Q18_45.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(6) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(6) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M6" runat="server" SessionKey="Q19_M6" DBColumn="Q19_M6" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(6) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M6" SessionKey="Q20A_M6" DBColumn="Q20A_M6" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M6" SessionKey="Q20B_M6" DBColumn="Q20B_M6" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M6" SessionKey="Q20C_M6" DBColumn="Q20C_M6" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M6" SessionKey="Q20D_M6" DBColumn="Q20D_M6" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M6" SessionKey="Q20E_M6" DBColumn="Q20E_M6" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M6" SessionKey="Q20F_M6" DBColumn="Q20F_M6" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M6" SessionKey="Q20G_M6" DBColumn="Q20G_M6" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<% if (Q18_7.Checked || Q18_19.Checked || Q18_31.Checked || Q18_46.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(7) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(7) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M7" runat="server" SessionKey="Q19_M7" DBColumn="Q19_M7" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(7) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M7" SessionKey="Q20A_M7" DBColumn="Q20A_M7" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M7" SessionKey="Q20B_M7" DBColumn="Q20B_M7" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M7" SessionKey="Q20C_M7" DBColumn="Q20C_M7" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M7" SessionKey="Q20D_M7" DBColumn="Q20D_M7" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M7" SessionKey="Q20E_M7" DBColumn="Q20E_M7" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M7" SessionKey="Q20F_M7" DBColumn="Q20F_M7" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M7" SessionKey="Q20G_M7" DBColumn="Q20G_M7" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<% if (Q18_8.Checked || Q18_20.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(8) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(8) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M8" runat="server" SessionKey="Q19_M8" DBColumn="Q19_M8" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(8) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M8" SessionKey="Q20A_M8" DBColumn="Q20A_M8" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M8" SessionKey="Q20B_M8" DBColumn="Q20B_M8" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M8" SessionKey="Q20C_M8" DBColumn="Q20C_M8" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M8" SessionKey="Q20D_M8" DBColumn="Q20D_M8" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M8" SessionKey="Q20E_M8" DBColumn="Q20E_M8" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M8" SessionKey="Q20F_M8" DBColumn="Q20F_M8" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M8" SessionKey="Q20G_M8" DBColumn="Q20G_M8" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<% if (Q18_9.Checked && PropertyShortCode != GCCPropertyShortCode.CNSH)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(9) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(9) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M9" runat="server" SessionKey="Q19_M9" DBColumn="Q19_M9" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(9) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M9" SessionKey="Q20A_M9" DBColumn="Q20A_M9" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M9" SessionKey="Q20B_M9" DBColumn="Q20B_M9" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M9" SessionKey="Q20C_M9" DBColumn="Q20C_M9" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M9" SessionKey="Q20D_M9" DBColumn="Q20D_M9" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M9" SessionKey="Q20E_M9" DBColumn="Q20E_M9" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M9" SessionKey="Q20F_M9" DBColumn="Q20F_M9" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M9" SessionKey="Q20G_M9" DBColumn="Q20G_M9" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<% if (Q18_10.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(10) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(10) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M10" runat="server" SessionKey="Q19_M10" DBColumn="Q19_M10" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(10) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M10" SessionKey="Q20A_M10" DBColumn="Q20A_M10" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M10" SessionKey="Q20B_M10" DBColumn="Q20B_M10" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M10" SessionKey="Q20C_M10" DBColumn="Q20C_M10" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M10" SessionKey="Q20D_M10" DBColumn="Q20D_M10" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M10" SessionKey="Q20E_M10" DBColumn="Q20E_M10" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M10" SessionKey="Q20F_M10" DBColumn="Q20F_M10" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M10" SessionKey="Q20G_M10" DBColumn="Q20G_M10" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<% if (Q18_11.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(11) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(11) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M11" runat="server" SessionKey="Q19_M11" DBColumn="Q19_M11" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(11) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M11" SessionKey="Q20A_M11" DBColumn="Q20A_M11" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M11" SessionKey="Q20B_M11" DBColumn="Q20B_M11" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M11" SessionKey="Q20C_M11" DBColumn="Q20C_M11" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M11" SessionKey="Q20D_M11" DBColumn="Q20D_M11" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M11" SessionKey="Q20E_M11" DBColumn="Q20E_M11" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M11" SessionKey="Q20F_M11" DBColumn="Q20F_M11" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M11" SessionKey="Q20G_M11" DBColumn="Q20G_M11" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<% if (Q18_12.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(12) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(12) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M12" runat="server" SessionKey="Q19_M12" DBColumn="Q19_M12" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(12) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M12" SessionKey="Q20A_M12" DBColumn="Q20A_M12" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M12" SessionKey="Q20B_M12" DBColumn="Q20B_M12" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M12" SessionKey="Q20C_M12" DBColumn="Q20C_M12" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M12" SessionKey="Q20D_M12" DBColumn="Q20D_M12" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M12" SessionKey="Q20E_M12" DBColumn="Q20E_M12" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M12" SessionKey="Q20F_M12" DBColumn="Q20F_M12" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M12" SessionKey="Q20G_M12" DBColumn="Q20G_M12" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<% if (Q18_13.Checked)
				   { %>
				<h2>Food & Beverage Experience - <%= GetFoodAndBevName(13) %></h2>
				<p class="question">
					How would you rate your overall satisfaction level with the food & beverage services at <b><%= GetFoodAndBevName(13) %></b>?
				</p>
				<uc1:ScaleQuestionControl ID="Q19_M13" runat="server" SessionKey="Q19_M13" DBColumn="Q19_M13" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate food and beverage services at <b><%= GetFoodAndBevName(13) %></b> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M13" SessionKey="Q20A_M13" DBColumn="Q20A_M13" ShowNAColumn="True" Label="Variety of food choices" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M13" SessionKey="Q20B_M13" DBColumn="Q20B_M13" ShowNAColumn="True" Label="Cleanliness of outlet" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M13" SessionKey="Q20C_M13" DBColumn="Q20C_M13" ShowNAColumn="True" Label="Courtesy of staff" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M13" SessionKey="Q20D_M13" DBColumn="Q20D_M13" ShowNAColumn="True" Label="Timely delivery of order" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M13" SessionKey="Q20E_M13" DBColumn="Q20E_M13" ShowNAColumn="True" Label="Value for the money" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M13" SessionKey="Q20F_M13" DBColumn="Q20F_M13" ShowNAColumn="True" Label="Pleasant atmosphere" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M13" SessionKey="Q20G_M13" DBColumn="Q20G_M13" ShowNAColumn="True" Label="Quality of food" />
				<% } %>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>





				<% }
				   else
				   { %>



				<% if (Q18_1_F.Checked || Q18_42_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(1) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à la <b><% = GetFoodAndBevName_French(1)%> </b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M1_F" runat="server" SessionKey="Q19_M1" DBColumn="Q19_M1" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à la <b><% = GetFoodAndBevName_French(1)%> </b>pour ce qui est de ...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M1_F" SessionKey="Q20A_M1" DBColumn="Q20A_M1" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M1_F" SessionKey="Q20B_M1" DBColumn="Q20B_M1" ShowNAColumn="True" Label="Propreté" />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M1_F" SessionKey="Q20C_M1" DBColumn="Q20C_M1" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M1_F" SessionKey="Q20D_M1" DBColumn="Q20D_M1" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M1_F" SessionKey="Q20E_M1" DBColumn="Q20E_M1" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M1_F" SessionKey="Q20F_M1" DBColumn="Q20F_M1" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M1_F" SessionKey="Q20G_M1" DBColumn="Q20G_M1" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<% if (Q18_2_F.Checked || Q18_14_F.Checked || Q18_21_F.Checked || Q18_26_F.Checked || Q18_32_F.Checked || Q18_34_F.Checked || Q18_35_F.Checked || Q18_36_F.Checked || Q18_37_F.Checked || Q18_41_F.Checked || Q18_47_F.Checked || Q18_48_F.Checked || Q18_49_F.Checked || Q18_53_F.Checked || Q18_55_F.Checked || Q18_58_F.Checked || Q18_59_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(2) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><% = GetFoodAndBevName_French(2)%> </b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M2_F" runat="server" SessionKey="Q19_M2" DBColumn="Q19_M2" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(2)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M2_F" SessionKey="Q20A_M2" DBColumn="Q20A_M2" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M2_F" SessionKey="Q20B_M2" DBColumn="Q20B_M2" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M2_F" SessionKey="Q20C_M2" DBColumn="Q20C_M2" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<% if(PropertyShortCode == GCCPropertyShortCode.CNSH) {  %>
				<uc1:QuestionRowControl runat="server" ID="Q20D_M2_F" SessionKey="Q20D_M2" DBColumn="Q20D_M2" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<%} %>
				<uc1:QuestionRowControl runat="server" ID="Q20E_M2_F" SessionKey="Q20E_M2" DBColumn="Q20E_M2" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M2_F" SessionKey="Q20F_M2" DBColumn="Q20F_M2" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M2_F" SessionKey="Q20G_M2" DBColumn="Q20G_M2" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<%--<% if (Q18_3.Checked || Q18_15.Checked || Q18_22.Checked || ( Q18_25.Checked && PropertyShortCode == GCCPropertyShortCode.EC ) || Q18_27.Checked || Q18_33.Checked || Q18_38.Checked) { %>--%>
				<% if (Q18_3_F.Checked || Q18_15_F.Checked || (Q18_25_F.Checked && PropertyShortCode == GCCPropertyShortCode.EC) || Q18_27_F.Checked || Q18_33_F.Checked || Q18_38_F.Checked || Q18_50_F.Checked || Q18_54_F.Checked || Q18_56_F.Checked || Q18_60_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(3) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(3) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M3_F" runat="server" SessionKey="Q19_M3" DBColumn="Q19_M3" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(3)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M3_F" SessionKey="Q20A_M3" DBColumn="Q20A_M3" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M3_F" SessionKey="Q20B_M3" DBColumn="Q20B_M3" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M3_F" SessionKey="Q20C_M3" DBColumn="Q20C_M3" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M3_F" SessionKey="Q20D_M3" DBColumn="Q20D_M3" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M3_F" SessionKey="Q20E_M3" DBColumn="Q20E_M3" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M3_F" SessionKey="Q20F_M3" DBColumn="Q20F_M3" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M3_F" SessionKey="Q20G_M3" DBColumn="Q20G_M3" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<%-- <% if (Q18_4.Checked || Q18_16.Checked || Q18_23.Checked || Q18_28.Checked || Q18_39.Checked || Q18_43.Checked) { %>--%>
				<% if (Q18_4_F.Checked || Q18_16_F.Checked || Q18_28_F.Checked || Q18_39_F.Checked || Q18_43_F.Checked || Q18_51_F.Checked || Q18_57_F.Checked || Q18_61_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(4) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(4) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M4_F" runat="server" SessionKey="Q19_M4" DBColumn="Q19_M4" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(4)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M4_F" SessionKey="Q20A_M4" DBColumn="Q20A_M4" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M4_F" SessionKey="Q20B_M4" DBColumn="Q20B_M4" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M4_F" SessionKey="Q20C_M4" DBColumn="Q20C_M4" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M4_F" SessionKey="Q20D_M4" DBColumn="Q20D_M4" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M4_F" SessionKey="Q20E_M4" DBColumn="Q20E_M4" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M4_F" SessionKey="Q20F_M4" DBColumn="Q20F_M4" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M4_F" SessionKey="Q20G_M4" DBColumn="Q20G_M4" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<%--  <% if (Q18_5.Checked || Q18_17.Checked || Q18_24.Checked || Q18_29.Checked || Q18_40.Checked || Q18_44.Checked) { %>--%>
				<% if (Q18_5_F.Checked || Q18_17_F.Checked || Q18_29_F.Checked || Q18_40_F.Checked || Q18_44_F.Checked || Q18_52_F.Checked || Q18_62_F.Checked )
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(5) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(5) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M5_F" runat="server" SessionKey="Q19_M5" DBColumn="Q19_M5" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(5)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>



				<uc1:QuestionRowControl runat="server" ID="Q20A_M5_F" SessionKey="Q20A_M5" DBColumn="Q20A_M5" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M5_F" SessionKey="Q20B_M5" DBColumn="Q20B_M5" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M5_F" SessionKey="Q20C_M5" DBColumn="Q20C_M5" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M5_F" SessionKey="Q20D_M5" DBColumn="Q20D_M5" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M5_F" SessionKey="Q20E_M5" DBColumn="Q20E_M5" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M5_F" SessionKey="Q20F_M5" DBColumn="Q20F_M5" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M5_F" SessionKey="Q20G_M5" DBColumn="Q20G_M5" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<% if (Q18_6_F.Checked || Q18_18_F.Checked || (Q18_25_F.Checked && PropertyShortCode != GCCPropertyShortCode.EC) || Q18_30_F.Checked || (Q18_9_F.Checked && PropertyShortCode == GCCPropertyShortCode.CNSH) || Q18_45_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(6) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(6) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M6_F" runat="server" SessionKey="Q19_M6" DBColumn="Q19_M6" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(6)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M6_F" SessionKey="Q20A_M6" DBColumn="Q20A_M6" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M6_F" SessionKey="Q20B_M6" DBColumn="Q20B_M6" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M6_F" SessionKey="Q20C_M6" DBColumn="Q20C_M6" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M6_F" SessionKey="Q20D_M6" DBColumn="Q20D_M6" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M6_F" SessionKey="Q20E_M6" DBColumn="Q20E_M6" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M6_F" SessionKey="Q20F_M6" DBColumn="Q20F_M6" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M6_F" SessionKey="Q20G_M6" DBColumn="Q20G_M6" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<% if (Q18_7_F.Checked || Q18_19_F.Checked || Q18_31_F.Checked || Q18_46_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(7) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(7) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M7_F" runat="server" SessionKey="Q19_M7" DBColumn="Q19_M7" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(7)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M7_F" SessionKey="Q20A_M7" DBColumn="Q20A_M7" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M7_F" SessionKey="Q20B_M7" DBColumn="Q20B_M7" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M7_F" SessionKey="Q20C_M7" DBColumn="Q20C_M7" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M7_F" SessionKey="Q20D_M7" DBColumn="Q20D_M7" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M7_F" SessionKey="Q20E_M7" DBColumn="Q20E_M7" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M7_F" SessionKey="Q20F_M7" DBColumn="Q20F_M7" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M7_F" SessionKey="Q20G_M7" DBColumn="Q20G_M7" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<% if (Q18_8_F.Checked || Q18_20_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(8) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(8) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M8_F" runat="server" SessionKey="Q19_M8" DBColumn="Q19_M8" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(8)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M8_F" SessionKey="Q20A_M8" DBColumn="Q20A_M8" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M8_F" SessionKey="Q20B_M8" DBColumn="Q20B_M8" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M8_F" SessionKey="Q20C_M8" DBColumn="Q20C_M8" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M8_F" SessionKey="Q20D_M8" DBColumn="Q20D_M8" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M8_F" SessionKey="Q20E_M8" DBColumn="Q20E_M8" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M8_F" SessionKey="Q20F_M8" DBColumn="Q20F_M8" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M8_F" SessionKey="Q20G_M8" DBColumn="Q20G_M8" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<% if (Q18_9_F.Checked && PropertyShortCode != GCCPropertyShortCode.CNSH)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(9) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(9) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M9_F" runat="server" SessionKey="Q19_M9" DBColumn="Q19_M9" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(9)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M9_F" SessionKey="Q20A_M9" DBColumn="Q20A_M9" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M9_F" SessionKey="Q20B_M9" DBColumn="Q20B_M9" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M9_F" SessionKey="Q20C_M9" DBColumn="Q20C_M9" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M9_F" SessionKey="Q20D_M9" DBColumn="Q20D_M9" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M9_F" SessionKey="Q20E_M9" DBColumn="Q20E_M9" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M9_F" SessionKey="Q20F_M9" DBColumn="Q20F_M9" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M9_F" SessionKey="Q20G_M9" DBColumn="Q20G_M9" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<% if (Q18_10_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(10) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(10) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M10_F" runat="server" SessionKey="Q19_M10" DBColumn="Q19_M10" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(10)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M10_F" SessionKey="Q20A_M10" DBColumn="Q20A_M10" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M10_F" SessionKey="Q20B_M10" DBColumn="Q20B_M10" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M10_F" SessionKey="Q20C_M10" DBColumn="Q20C_M10" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M10_F" SessionKey="Q20D_M10" DBColumn="Q20D_M10" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M10_F" SessionKey="Q20E_M10" DBColumn="Q20E_M10" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M10_F" SessionKey="Q20F_M10" DBColumn="Q20F_M10" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M10_F" SessionKey="Q20G_M10" DBColumn="Q20G_M10" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<% if (Q18_11_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(11) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(11) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M11_F" runat="server" SessionKey="Q19_M11" DBColumn="Q19_M11" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(11)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M11_F" SessionKey="Q20A_M11" DBColumn="Q20A_M11" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M11_F" SessionKey="Q20B_M11" DBColumn="Q20B_M11" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M11_F" SessionKey="Q20C_M11" DBColumn="Q20C_M11" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M11_F" SessionKey="Q20D_M11" DBColumn="Q20D_M11" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M11_F" SessionKey="Q20E_M11" DBColumn="Q20E_M11" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M11_F" SessionKey="Q20F_M11" DBColumn="Q20F_M11" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M11_F" SessionKey="Q20G_M11" DBColumn="Q20G_M11" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<% if (Q18_12_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(12) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(12) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M12_F" runat="server" SessionKey="Q19_M12" DBColumn="Q19_M12" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(12)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M12_F" SessionKey="Q20A_M12" DBColumn="Q20A_M12" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M12_F" SessionKey="Q20B_M12" DBColumn="Q20B_M12" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M12_F" SessionKey="Q20C_M12" DBColumn="Q20C_M12" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M12_F" SessionKey="Q20D_M12" DBColumn="Q20D_M12" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M12_F" SessionKey="Q20E_M12" DBColumn="Q20E_M12" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M12_F" SessionKey="Q20F_M12" DBColumn="Q20F_M12" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M12_F" SessionKey="Q20G_M12" DBColumn="Q20G_M12" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<% if (Q18_13_F.Checked)
				   { %>
				<h2>Expérience restauration - <%= GetFoodAndBevName_French(13) %></h2>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec les services de restauration à <b><%= GetFoodAndBevName_French(13) %></b>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q19_M13_F" runat="server" SessionKey="Q19_M13" DBColumn="Q19_M13" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, comment évalueriez-vous les services de restauration à <b><% = GetFoodAndBevName_French(13)%> </b>pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q20A_M13_F" SessionKey="Q20A_M13" DBColumn="Q20A_M13" ShowNAColumn="True" Label="Variété de plats" />
				<uc1:QuestionRowControl runat="server" ID="Q20B_M13_F" SessionKey="Q20B_M13" DBColumn="Q20B_M13" ShowNAColumn="True" Label="Propreté " />
				<uc1:QuestionRowControl runat="server" ID="Q20C_M13_F" SessionKey="Q20C_M13" DBColumn="Q20C_M13" ShowNAColumn="True" Label="Courtoisie du personnel" />
				<uc1:QuestionRowControl runat="server" ID="Q20D_M13_F" SessionKey="Q20D_M13" DBColumn="Q20D_M13" ShowNAColumn="True" Label="Livraison rapide de la commande" />
				<uc1:QuestionRowControl runat="server" ID="Q20E_M13_F" SessionKey="Q20E_M13" DBColumn="Q20E_M13" ShowNAColumn="True" Label="Rapport qualité / prix" />
				<uc1:QuestionRowControl runat="server" ID="Q20F_M13_F" SessionKey="Q20F_M13" DBColumn="Q20F_M13" ShowNAColumn="True" Label="Ambiance agréable" />
				<uc1:QuestionRowControl runat="server" ID="Q20G_M13_F" SessionKey="Q20G_M13" DBColumn="Q20G_M13" ShowNAColumn="True" Label="Qualité de la nourriture" />
				<% } %>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>






				<%} %>



				<% }
					   //===========================================================================
					   //PAGE 11 - Q21 Entertainment
					   //===========================================================================
					   else if (CurrentPage == 11)
					   { %>




				<% if (strSurveyLang != "French")
				   { %>




				<h2>Entertainment</h2>
				<% if (PropertyShortCode == GCCPropertyShortCode.RR
					   || PropertyShortCode == GCCPropertyShortCode.HRCV
					   || PropertyShortCode == GCCPropertyShortCode.VRL
					   || PropertyShortCode == GCCPropertyShortCode.CCH
					   || PropertyShortCode == GCCPropertyShortCode.CMR
					   || PropertyShortCode == GCCPropertyShortCode.CDC
					   || PropertyShortCode == GCCPropertyShortCode.CNSH
					   || PropertyShortCode == GCCPropertyShortCode.EC
					   || PropertyShortCode == GCCPropertyShortCode.SCTI
					 
					   || PropertyShortCode == GCCPropertyShortCode.CNB
					   || PropertyShortCode == GCCPropertyShortCode.SCBE)
				   { %>
				<p class="question">
					<% if (!IsKioskOrStaffEntry)
					   { %>
					Did you visit <%= GetShowLoungeName() %> during your most recent visit for entertainment?
					<% }
					   else
					   { %>
					Have you visited <%= GetShowLoungeName() %> recently for live entertainment?
					<% } %>
				</p>
				<uc1:YesNoControl runat="server" ID="Q21" SessionKey="Q21" DBColumn="Q21" />
				<sc:MessageManager runat="server" ID="mmQ21A"></sc:MessageManager>
				<p>
					If "Yes", what date did you visit <%= GetShowLoungeName() %>?
					<sc:SurveyTextBox ID="txtShowVisitDate" runat="server" SessionKey="Q21_VisitDate" DBColumn="Q21VisitDate" MaxLength="20" CssClass="date-picker"></sc:SurveyTextBox>
				</p>
				<% if (PropertyShortCode == GCCPropertyShortCode.HRCV)
				   { %>
				<p class="question">
					Which Show Lounge did you visit at Hard Rock Casino Vancouver?
				</p>
				<sc:SurveyRadioButton ID="radQ21_HRCV_LoungeA" runat="server" GroupName="Q21_HRCV_Lounge" SessionKey="Q21_HRCV_LoungeA" DBColumn="Q21_HRCV_Lounge" DBValue="Asylum" Text="&nbsp;Asylum" /><br />
				<sc:SurveyRadioButton ID="radQ21_HRCV_LoungeU" runat="server" GroupName="Q21_HRCV_Lounge" SessionKey="Q21_HRCV_LoungeB" DBColumn="Q21_HRCV_Lounge" DBValue="UnListed" Text="&nbsp;UnListed" /><br />
				<% } %>





				<% if (PropertyShortCode == GCCPropertyShortCode.EC)
				   { %>
				<p class="question">
					Which Show Lounge did you visit at Elements Casino?
				</p>
				<sc:SurveyRadioButton ID="radQ21_EC_LoungeM" runat="server" GroupName="Q21_EC_Lounge" SessionKey="Q21_EC_LoungeM" DBColumn="Q21_EC_Lounge" DBValue="Molson Lounge" Text="&nbsp;Molson Lounge" /><br />
				<sc:SurveyRadioButton ID="radQ21_EC_LoungeE" runat="server" GroupName="Q21_EC_Lounge" SessionKey="Q21_EC_LoungeE" DBColumn="Q21_EC_Lounge" DBValue="Escape" Text="&nbsp;Escape" /><br />
				<% } %>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>



				<% }
				   else
				   { %>


				<h2>Divertissement</h2>
				<% if (PropertyShortCode == GCCPropertyShortCode.RR
					   || PropertyShortCode == GCCPropertyShortCode.HRCV
					   || PropertyShortCode == GCCPropertyShortCode.VRL
					   || PropertyShortCode == GCCPropertyShortCode.CCH
					   || PropertyShortCode == GCCPropertyShortCode.CMR
					   || PropertyShortCode == GCCPropertyShortCode.CDC
					   || PropertyShortCode == GCCPropertyShortCode.CNSH
					   || PropertyShortCode == GCCPropertyShortCode.EC
					   || PropertyShortCode == GCCPropertyShortCode.SCTI
					 
					   || PropertyShortCode == GCCPropertyShortCode.CNB
					   || PropertyShortCode == GCCPropertyShortCode.SCBE)
				   { %>
				<p class="question">
					<% if (!IsKioskOrStaffEntry)
					   { %>
					Lors de votre dernière visite, avez-vous assisté à un spectacle au <% = GetShowLoungeName ()%> pour vous divertir? 
					<% }
					   else
					   { %>
					Avez-vous visité <% = GetShowLoungeName ()%> récemment pour le divertissement en direct?
					<% } %>
				</p>
				<uc1:YesNoControlFrench runat="server" ID="Q21_F" SessionKey="Q21" DBColumn="Q21" />
				<sc:MessageManager runat="server" ID="MessageManager4"></sc:MessageManager>
				<p>
					Si «Oui», veuillez fournir la date de votre visite?
					<sc:SurveyTextBox ID="txtShowVisitDate_F" runat="server" SessionKey="Q21_VisitDate" DBColumn="Q21VisitDate" MaxLength="20" CssClass="date-picker"></sc:SurveyTextBox>
				</p>
				<% if (PropertyShortCode == GCCPropertyShortCode.HRCV)
				   { %>
				<p class="question">
					Quel salon avez-vous visité au Hard Rock Casino Vancouver?
				</p>
				<sc:SurveyRadioButton ID="radQ21_HRCV_LoungeA_F" runat="server" GroupName="Q21_HRCV_Lounge" SessionKey="Q21_HRCV_LoungeA" DBColumn="Q21_HRCV_Lounge" DBValue="Asylum" Text="&nbsp;Asile" /><br />
				<sc:SurveyRadioButton ID="radQ21_HRCV_LoungeU_F" runat="server" GroupName="Q21_HRCV_Lounge" SessionKey="Q21_HRCV_LoungeB" DBColumn="Q21_HRCV_Lounge" DBValue="UnListed" Text="&nbsp;Non répertorié" /><br />
				<% } %>
				<% if (PropertyShortCode == GCCPropertyShortCode.EC)
				   { %>
				<p class="question">
					Quel Salon avez-vous visité au Casino Elements?
				</p>
				<sc:SurveyRadioButton ID="radQ21_EC_LoungeM_F" runat="server" GroupName="Q21_EC_Lounge" SessionKey="Q21_EC_LoungeM" DBColumn="Q21_EC_Lounge" DBValue="Molson Lounge" Text="&nbsp;Molson Lounge" /><br />
				<sc:SurveyRadioButton ID="radQ21_EC_LoungeE_F" runat="server" GroupName="Q21_EC_Lounge" SessionKey="Q21_EC_LoungeE" DBColumn="Q21_EC_Lounge" DBValue="Escape" Text="&nbsp;Échapper" /><br />
				<% } %>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>





				<%} %>


				<% }
					   //===========================================================================
					   //PAGE 12 - Q22-Q23 Show Lounge
					   //===========================================================================
					   else if (CurrentPage == 12)
					   { %>



				<% if (strSurveyLang != "French")
				   { %>






				<h2>Show Lounge</h2>
				<% if (Q21.SelectedValue == 1)
				   { %>
				<p class="question">
					How would you rate your overall satisfaction level with your entertainment experience at <%= GetShowLoungeName( true ) %>?
				</p>
				<uc1:ScaleQuestionControl ID="Q22" runat="server" SessionKey="Q22" DBColumn="Q22" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Please rate your satisfaction level with the entertainment at <%= GetShowLoungeName( true ) %> in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q23A" SessionKey="Q23A" DBColumn="Q23A" ShowNAColumn="True" Label="Sound / quality" />
				<uc1:QuestionRowControl runat="server" ID="Q23B" SessionKey="Q23B" DBColumn="Q23B" ShowNAColumn="True" Label="Seating availability" />
				<uc1:QuestionRowControl runat="server" ID="Q23C" SessionKey="Q23C" DBColumn="Q23C" ShowNAColumn="True" Label="Dance floor" />
				<uc1:QuestionRowControl runat="server" ID="Q23D" SessionKey="Q23D" DBColumn="Q23D" ShowNAColumn="True" Label="Fun and enjoyable atmosphere" />
				<% } %>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>




				<% }
				   else
				   { %>


				<h2>Divertissement</h2>
				<% if (Q21_F.SelectedValue_F == 1)
				   { %>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction globale avec votre expérience de divertissement au <% = GetShowLoungeName (true)%>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q22_F" runat="server" SessionKey="Q22" DBColumn="Q22" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Veuillez évaluer votre niveau de satisfaction avec le divertissement au <% = GetShowLoungeName (true)%> pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q23A_F" SessionKey="Q23A" DBColumn="Q23A" ShowNAColumn="True" Label="Qualité /sonore" />
				<uc1:QuestionRowControl runat="server" ID="Q23B_F" SessionKey="Q23B" DBColumn="Q23B" ShowNAColumn="True" Label="Disponibilité des places" />
				<uc1:QuestionRowControl runat="server" ID="Q23C_F" SessionKey="Q23C" DBColumn="Q23C" ShowNAColumn="True" Label="Piste de dance" />
				<uc1:QuestionRowControl runat="server" ID="Q23D_F" SessionKey="Q23D" DBColumn="Q23D" ShowNAColumn="True" Label="Ambiance amusante et agréable" />
				<% } %>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>



				<%} %>


				<% }
					   //===========================================================================
					   //PAGE 13 - Q24 Theatre
					   //===========================================================================
					   else if (CurrentPage == 13)
					   { %>


				<% if (strSurveyLang != "French")
				   { %>


				<h2>Theatre</h2>
				<% if (PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.HRCV || PropertyShortCode == GCCPropertyShortCode.CNSH || PropertyShortCode == GCCPropertyShortCode.CNB)
				   { %>
				<p class="question">
					<% if (PropertyShortCode == GCCPropertyShortCode.CNSH)
					   { %>
					Have you attended a show at the Schooner Showroom <%= !IsKioskOrStaffEntry ? "recently" : "during this visit or in the last 30 days" %>? 
					<% }
					   else if (!IsKioskOrStaffEntry)
					   { %>
					Did you attend a show at the <%= CasinoName %> Show Theatre during this visit or in the last 30 days?
					<% }
					   else
					   { %>
					Have you attended a show at the <%= CasinoName %> Show Theatre recently? 
					<% } %>
				</p>
				<uc1:YesNoControl runat="server" ID="Q24" SessionKey="Q24" DBColumn="Q24" />
				<sc:MessageManager runat="server" ID="mmQ24A"></sc:MessageManager>
				<p>
					If "Yes", what date did you visit <%= CasinoName %> Show Theatre?
					<sc:SurveyTextBox ID="Q24_VisitDate" runat="server" SessionKey="Q24_VisitDate" DBColumn="Q24VisitDate" MaxLength="20" CssClass="date-picker"></sc:SurveyTextBox>
				</p>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>



				<%}
				   else
				   { %>

				<h2>Salle de spectacle</h2>
				<% if (PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.HRCV || PropertyShortCode == GCCPropertyShortCode.CNSH || PropertyShortCode == GCCPropertyShortCode.CNB)
				   { %>
				<p class="question">
					<% if (PropertyShortCode == GCCPropertyShortCode.CNSH)
					   { %>
					Avez-vous assisté à un spectacle à la Showroom Schooner <% =! IsKioskOrStaffEntry? "Récemment": "au cours de cette visite ou au cours des 30 derniers jours"%>?
					<% }
					   else if (!IsKioskOrStaffEntry)
					   { %>
					Avez-vous assisté à un spectacle à la salle de spectacle de <% = CasinoName%> durant cette visite ou au cours des 30 derniers jours?
					<% }
					   else
					   { %>
					Avez-vous assisté à un spectacle au <% = CasinoName%> Show Theatre récemment?
					<% } %>
				</p>
				<uc1:YesNoControlFrench runat="server" ID="Q24_F" SessionKey="Q24" DBColumn="Q24" />
				<sc:MessageManager runat="server" ID="MessageManager5"></sc:MessageManager>
				<p>
					Si «Oui», à quelle date avez-vous visité la salle de spectacle de <% = CasinoName%>?
					<sc:SurveyTextBox ID="Q24_VisitDate_F" runat="server" SessionKey="Q24_VisitDate" DBColumn="Q24VisitDate" MaxLength="20" CssClass="date-picker"></sc:SurveyTextBox>
				</p>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>



				<%} %>
				<% }
					   //===========================================================================
					   //PAGE 14 - Q25-Q26 Theatre Experience
					   //===========================================================================
					   else if (CurrentPage == 14)
					   { %>

				<% if (strSurveyLang != "French")
				   { %>


				<h2>Theatre</h2>
				<% if (Q24.SelectedValue == 1 && (PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.HRCV || PropertyShortCode == GCCPropertyShortCode.CNSH || PropertyShortCode == GCCPropertyShortCode.CNB))
				   { %>
				<p class="question">
					How would you rate your satisfaction level overall entertainment experience in the <%= CasinoName %> Show Theatre?
				</p>
				<uc1:ScaleQuestionControl ID="Q25" runat="server" SessionKey="Q25" DBColumn="Q25" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Please rate your satisfaction level with your experience at this event in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q26A" SessionKey="Q26A" DBColumn="Q26A" ShowNAColumn="True" Label="The quality of the show" />
				<uc1:QuestionRowControl runat="server" ID="Q26B" SessionKey="Q26B" DBColumn="Q26B" ShowNAColumn="True" Label="The value of the show" />
				<uc1:QuestionRowControl runat="server" ID="Q26C" SessionKey="Q26C" DBColumn="Q26C" ShowNAColumn="True" Label="Seating choices" />
				<uc1:QuestionRowControl runat="server" ID="Q26D" SessionKey="Q26D" DBColumn="Q26D" ShowNAColumn="True" Label="Sound quality" />
				<uc1:QuestionRowControl runat="server" ID="Q26E" SessionKey="Q26E" DBColumn="Q26E" ShowNAColumn="True" Label="Overall customer service of Theatre staff" />
				<% } %>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>



				<%}
				   else
				   { %>



				<h2>Salle de spectacle</h2>
				<% if (Q24_F.SelectedValue_F == 1 && (PropertyShortCode == GCCPropertyShortCode.RR || PropertyShortCode == GCCPropertyShortCode.HRCV || PropertyShortCode == GCCPropertyShortCode.CNSH || PropertyShortCode == GCCPropertyShortCode.CNB))
				   { %>
				<p class="question">
					Comment qualifieriez-vous votre niveau de satisfaction de l'expérience globale de divertissement à la salle de spectacle de <% = CasinoName%>?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q25_F" runat="server" SessionKey="Q25" DBColumn="Q25" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Veuillez évaluer votre niveau de satisfaction avec votre expérience à cet évènement pour ce qui est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q26A_F" SessionKey="Q26A" DBColumn="Q26A" ShowNAColumn="True" Label="La qualité du spectacle" />
				<uc1:QuestionRowControl runat="server" ID="Q26B_F" SessionKey="Q26B" DBColumn="Q26B" ShowNAColumn="True" Label="La valeur du spectacle" />
				<uc1:QuestionRowControl runat="server" ID="Q26C_F" SessionKey="Q26C" DBColumn="Q26C" ShowNAColumn="True" Label="Choix de sièges" />
				<uc1:QuestionRowControl runat="server" ID="Q26D_F" SessionKey="Q26D" DBColumn="Q26D" ShowNAColumn="True" Label="Qualité du son" />
				<uc1:QuestionRowControl runat="server" ID="Q26E_F" SessionKey="Q26E" DBColumn="Q26E" ShowNAColumn="True" Label="Service à la clientèle en général du personnel de la salle de spectacle" />
				<% } %>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>





				<%} %>

				<% }
					   //===========================================================================
					   //PAGE 15 - Q27 Issue Qualifier
					   //===========================================================================
					   else if (CurrentPage == 15)
					   { %>

				<% if (strSurveyLang != "French")
				   { %>




				<p class="question">
					Did you experience a problem or issue during your<% if (!IsKioskOrStaffEntry)
																		{ %> most recent<% } %> visit<% if (IsKioskOrStaffEntry)
																										{ %> today<% } %>?
				</p>
				<sc:MessageManager runat="server" ID="mmQ27"></sc:MessageManager>
				<uc1:YesNoControl runat="server" ID="Q27" SessionKey="Q27" DBColumn="Q27" />
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>


				<%}
				   else
				   { %>

				<p class="question">
					Avez-vous éprouvé un problème lors de votre <% if (!IsKioskOrStaffEntry)
																					{%> dernière <%}%> visite <% if (IsKioskOrStaffEntry)
																													 {%> aujourd'hui <%}%>?
				</p>
				<sc:MessageManager runat="server" ID="MessageManager6"></sc:MessageManager>
				<uc1:YesNoControlFrench runat="server" ID="Q27_F" SessionKey="Q27" DBColumn="Q27" />
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>



				<%} %>

				<% }
					   //===========================================================================
					   //PAGE 16 - Q27A-Q29 Issue Details
					   //===========================================================================
					   else if (CurrentPage == 16)
					   { %>



				<% if (strSurveyLang != "French")
				   { %>







				<% if (Q27.SelectedValue == 1)
				   { %>
				<p class="question">
					<% if (!IsKioskOrStaffEntry)
					   { %>
						Where or with whom did the problem occur?
					<% }
					   else
					   { %>
						Based on the guest’s responses, please categorize this feedback so that the right property staff can be notified for follow-up:
					<% } %>
				</p>
				<sc:MessageManager runat="server" ID="mmQ27A"></sc:MessageManager>
				<sc:SurveyRadioButton ID="radQ27A_1" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_1" DBColumn="Q27A_ArrivalAndParking" DBValue="1" Text="&nbsp;Arrival and parking" /><br />
				<sc:SurveyRadioButton ID="radQ27A_2" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_2" DBColumn="Q27A_GuestServices" DBValue="1" Text="&nbsp;Guest Services" /><br />
				<sc:SurveyRadioButton ID="radQ27A_3" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_3" DBColumn="Q27A_Cashiers" DBValue="1" Text="&nbsp;Cashiers" /><br />
				<sc:SurveyRadioButton ID="radQ27A_4" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_4" DBColumn="Q27A_ManagerSupervisor" DBValue="1" Text="&nbsp;Manager/Supervisor" /><br />
				<sc:SurveyRadioButton ID="radQ27A_5" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_5" DBColumn="Q27A_Security" DBValue="1" Text="&nbsp;Security" /><br />
				<sc:SurveyRadioButton ID="radQ27A_6" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_6" DBColumn="Q27A_Slots" DBValue="1" Text="&nbsp;Slots" /><% if (radQ27A_6.Visible)
																																													  { %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_7" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_7" DBColumn="Q27A_Tables" DBValue="1" Text="&nbsp;Tables" /><% if (radQ27A_7.Visible)
																																														{ %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_8" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_8" DBColumn="Q27A_FoodAndBeverage" DBValue="1" Text="&nbsp;Food & Beverage" /><br />
				<sc:SurveyRadioButton ID="radQ27A_9" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_9" DBColumn="Q27A_Hotel" DBValue="1" Text="&nbsp;Hotel" /><% if (radQ27A_9.Visible)
																																													  { %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_11" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_11" DBColumn="Q27A_Bingo" DBValue="1" Text="&nbsp;Bingo" /><% if (radQ27A_11.Visible)
																																														{ %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_12" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_12" DBColumn="Q27A_Entertainment" DBValue="1" Text="&nbsp;Entertainment" /><% if (radQ27A_12.Visible)
																																																		{ %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_13" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_13" DBColumn="Q27A_HorseRacing" DBValue="1" Text="&nbsp;Horse&nbsp;Racing" /><% if (radQ27A_13.Visible)
																																																		  { %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_10" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_10" DBColumn="Q27A_Other" DBValue="1" Text="&nbsp;Other" />
				<sc:SurveyTextBox ID="txtQ27A_OtherExplanation" runat="server" SessionKey="Q27A_OtherExplanation" DBColumn="Q27A_OtherExplanation" MaxLength="500" Size="50"></sc:SurveyTextBox><br />
				<p class="question">
					Briefly describe your problem.
				</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="txtQ27B" SessionKey="Q27B" DBColumn="Q27B" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="2000"></sc:SurveyTextBox>
				</div>
				<p class="question">
					Has this problem been resolved?
				</p>
				<uc1:YesNoControl runat="server" ID="Q28" SessionKey="Q28" DBColumn="Q28" />
				<p class="question">
					Did you report the problem?
				</p>
				<uc1:YesNoControl runat="server" ID="Q29" SessionKey="Q29" DBColumn="Q29" />
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>



				<% }
				   else
				   { %>






				<% if (Q27_F.SelectedValue_F == 1)
				   { %>
				<p class="question">
					<% if (!IsKioskOrStaffEntry)
					   { %>
					   Où et avec qui le problème s'est-il produit?
					<% }
					   else
					   { %>
						Sur la base des réponses des invités, veuillez classer ces commentaires afin que le personnel de la propriété droit puisse être averti pour le suivi:
					<% } %>
				</p>
				<sc:MessageManager runat="server" ID="mmQ27A_F"></sc:MessageManager>
				<sc:SurveyRadioButton ID="radQ27A_1_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_1" DBColumn="Q27A_ArrivalAndParking" DBValue="1" Text="&nbsp;Arrivée et stationnement" /><br />
				<sc:SurveyRadioButton ID="radQ27A_2_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_2" DBColumn="Q27A_GuestServices" DBValue="1" Text="&nbsp;Service à la clientèle" /><br />
				<sc:SurveyRadioButton ID="radQ27A_3_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_3" DBColumn="Q27A_Cashiers" DBValue="1" Text="&nbsp;Caissiers" /><br />
				<sc:SurveyRadioButton ID="radQ27A_4_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_4" DBColumn="Q27A_ManagerSupervisor" DBValue="1" Text="&nbsp;Gestionnaire / Superviseur" /><br />
				<sc:SurveyRadioButton ID="radQ27A_5_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_5" DBColumn="Q27A_Security" DBValue="1" Text="&nbsp;La Sécurité" /><br />
				<sc:SurveyRadioButton ID="radQ27A_6_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_6" DBColumn="Q27A_Slots" DBValue="1" Text="&nbsp;Machines à sous" /><% if (radQ27A_6_F.Visible)
																																																  { %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_7_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_7" DBColumn="Q27A_Tables" DBValue="1" Text="&nbsp;Les Tables" /><% if (radQ27A_7_F.Visible)
																																															  { %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_8_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_8" DBColumn="Q27A_FoodAndBeverage" DBValue="1" Text="&nbsp;Nourriture et Boissons" /><br />
				<sc:SurveyRadioButton ID="radQ27A_9_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_9" DBColumn="Q27A_Hotel" DBValue="1" Text="&nbsp;Un Hôtel" /><% if (radQ27A_9_F.Visible)
																																														   { %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_11_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_11" DBColumn="Q27A_Bingo" DBValue="1" Text="&nbsp;Bingo" /><% if (radQ27A_11_F.Visible)
																																														  { %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_12_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_12" DBColumn="Q27A_Entertainment" DBValue="1" Text="&nbsp;Divertissement" /><% if (radQ27A_12_F.Visible)
																																																		   { %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_13_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_13" DBColumn="Q27A_HorseRacing" DBValue="1" Text="&nbsp;Horse&nbsp;Courses" /><% if (radQ27A_13_F.Visible)
																																																			 { %><br />
				<% } %>
				<sc:SurveyRadioButton ID="radQ27A_10_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_10" DBColumn="Q27A_Other" DBValue="1" Text="&nbsp;Autre" />
				<sc:SurveyTextBox ID="txtQ27A_OtherExplanation_F" runat="server" SessionKey="Q27A_OtherExplanation" DBColumn="Q27A_OtherExplanation" MaxLength="500" Size="50"></sc:SurveyTextBox><br />
				<p class="question">
					Décrivez brièvement votre problème.
				</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="txtQ27B_F" SessionKey="Q27B" DBColumn="Q27B" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="2000"></sc:SurveyTextBox>
				</div>
				<p class="question">
					Ce problème at-il été résolu?
				</p>
				<uc1:YesNoControlFrench runat="server" ID="Q28_F" SessionKey="Q28" DBColumn="Q28" />
				<p class="question">
					Avez-vous signalé le problème?
				</p>
				<uc1:YesNoControlFrench runat="server" ID="Q29_F" SessionKey="Q29" DBColumn="Q29" />
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>



				<%} %>


				<% }
					   //===========================================================================
					   //PAGE 17 - Q27 Satisfaction with Fixing Problem
					   //===========================================================================
					   else if (CurrentPage == 17)
					   { %>





				<% if (strSurveyLang != "French")
				   { %>





				<% if (Q29.SelectedValue == 1)
				   { %>
				<p class="question">
					Thinking of this problem, what is your satisfaction level with the <%= CasinoName %>'s ability to fix your problem or issue?
				</p>
				<uc1:ScaleQuestionControl ID="Q30" runat="server" SessionKey="Q30" DBColumn="Q30" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					More specifically, how would you rate your satisfaction level with <%= CasinoName %>'s response to your problem in terms of...?
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Very Good</div>
					<div class="col-md-1 col-xs-2 title">Good</div>
					<div class="col-md-1 col-xs-2 title">Fair</div>
					<div class="col-md-1 col-xs-2 title">Poor</div>
					<div class="col-md-1 col-xs-2 title">Don't Know / N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q31A" SessionKey="Q31A" DBColumn="Q31A" ShowNAColumn="True" Label="The length of time taken to resolve your problem" />
				<uc1:QuestionRowControl runat="server" ID="Q31B" SessionKey="Q31B" DBColumn="Q31B" ShowNAColumn="True" Label="The effort of employees in resolving your problem" />
				<uc1:QuestionRowControl runat="server" ID="Q31C" SessionKey="Q31C" DBColumn="Q31C" ShowNAColumn="True" Label="The courteousness of employees while resolving your problem" />
				<uc1:QuestionRowControl runat="server" ID="Q31D" SessionKey="Q31D" DBColumn="Q31D" ShowNAColumn="True" Label="The amount of communication with you from employees while resolving your problem" />
				<uc1:QuestionRowControl runat="server" ID="Q31E" SessionKey="Q31E" DBColumn="Q31E" ShowNAColumn="True" Label="The fairness of the outcome in resolving your problem" />
				<p class="question">
					Please provide any additional comments or suggestions regarding your experience with problem resolution at <%= CasinoName %>.
				</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="Q32" SessionKey="Q32" DBColumn="Q32" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="1500"></sc:SurveyTextBox>
				</div>
				<% } %>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>






				<%}
				   else
				   { %>





				<% if (Q29_F.SelectedValue_F == 1)
				   { %>
				<p class="question">
					En y tenant compte, veuillez évaluer votre niveau de satisfaction de la capacité du <% = CasinoName%> à résoudre votre problème?
				</p>
				<uc1:ScaleQuestionControlFrench ID="Q30_F" runat="server" SessionKey="Q30" DBColumn="Q30" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Plus précisément, veuillez évaluer votre niveau de satisfaction de la réponse du <% = CasinoName%> à votre problème pour ce «Oui» est de :
				</p>
				<div class="row grid-header">
					<div class="col-md-6"></div>
					<div class="col-md-1 col-xs-2 title">Excellent</div>
					<div class="col-md-1 col-xs-2 title">Très bon</div>
					<div class="col-md-1 col-xs-2 title">Bon</div>
					<div class="col-md-1 col-xs-2 title">Médiocre</div>
					<div class="col-md-1 col-xs-2 title">Mauvais</div>
					<div class="col-md-1 col-xs-2 title">Ne sais pas/ N/A</div>
				</div>
				<uc1:QuestionRowControl runat="server" ID="Q31A_F" SessionKey="Q31A" DBColumn="Q31A" ShowNAColumn="True" Label="Le temps nécessaire pour résoudre votre problème" />
				<uc1:QuestionRowControl runat="server" ID="Q31B_F" SessionKey="Q31B" DBColumn="Q31B" ShowNAColumn="True" Label="L'effort des employés pour résoudre votre problème" />
				<uc1:QuestionRowControl runat="server" ID="Q31C_F" SessionKey="Q31C" DBColumn="Q31C" ShowNAColumn="True" Label="La courtoisie des employés pendant la résolution de votre problème" />
				<uc1:QuestionRowControl runat="server" ID="Q31D_F" SessionKey="Q31D" DBColumn="Q31D" ShowNAColumn="True" Label="La quantité de communication entre vous et les employés pendant la résolution de votre problème" />
				<uc1:QuestionRowControl runat="server" ID="Q31E_F" SessionKey="Q31E" DBColumn="Q31E" ShowNAColumn="True" Label="L'équité du résultat dans la résolution de votre problème" />
				<p class="question">
					Veuillez fournir tout autre commentaire ou suggestion supplémentaire concernant votre expérience de résolution de problèmes au <% = CasinoName%>.
				</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="Q32_F" SessionKey="Q32" DBColumn="Q32" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="1500"></sc:SurveyTextBox>
				</div>
				<% } %>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>



				<%} %>


				<% }
					   //===========================================================================
					   //PAGE 18 - Q33 Request Follow-up
					   //===========================================================================
					   else if (CurrentPage == 18)
					   { %>



				<% if (strSurveyLang != "French")
				   { %>



				<% if (!IsKioskOrStaffEntry
						&& (Q29.SelectedValue == 0
						|| Q30.SelectedValue == 1 || Q30.SelectedValue == 2
						|| Q31A.SelectedValue == 1 || Q31A.SelectedValue == 2
						|| Q31B.SelectedValue == 1 || Q31B.SelectedValue == 2
						|| Q31C.SelectedValue == 1 || Q31C.SelectedValue == 2
						|| Q31D.SelectedValue == 1 || Q31D.SelectedValue == 2
						|| Q31E.SelectedValue == 1 || Q31E.SelectedValue == 2))
				   { %>
				<p class="question">
					Would you like someone from <%= CasinoName %> to follow up with you regarding your recent problem?
				</p>
				<uc1:YesNoControl runat="server" ID="Q33" SessionKey="Q33" DBColumn="Q33" />
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>


				<%}
				   else
				   { %>

				<% if (!IsKioskOrStaffEntry
						&& (Q29_F.SelectedValue_F == 0
						|| Q30_F.SelectedValue_F == 1 || Q30_F.SelectedValue_F == 2
						|| Q31A_F.SelectedValue == 1 || Q31A_F.SelectedValue == 2
						|| Q31B_F.SelectedValue == 1 || Q31B_F.SelectedValue == 2
						|| Q31C_F.SelectedValue == 1 || Q31C_F.SelectedValue == 2
						|| Q31D_F.SelectedValue == 1 || Q31D_F.SelectedValue == 2
						|| Q31E_F.SelectedValue == 1 || Q31E_F.SelectedValue == 2))
				   { %>
				<p class="question">
					Voulez-vous que quelqu'un de <% = CasinoName%> suive avec vous concernant votre problème récent?
				</p>
				<uc1:YesNoControlFrench runat="server" ID="Q33_F" SessionKey="Q33" DBColumn="Q33" />
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>



				<%} %>
				<% }
					   //===========================================================================
					   //PAGE 19 - Q34-Q35 Additional Comments
					   //===========================================================================
					   else if (CurrentPage == 19)
					   { %>



				<% if (strSurveyLang != "French")
				   { %>


				<p class="question">
					Please provide any additional comments on your most recent experience with our services or one suggestion that would make your next visit even more enjoyable.<br />
					<% if (!IsKioskOrStaffEntry)
					   { %>If you have no comments, please leave the field blank.<% } %>
				</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="Q34" SessionKey="Q34" DBColumn="Q34" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="1000"></sc:SurveyTextBox>
				</div>
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					We strive to make your experience at <%= CasinoName %> great. If one of our employees made your visit particularly memorable, please share his/her name with us. If you don't know his/her name, please indicate the area within the facility.<br />
					If you have no comments, please leave the field blank.
				</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="Q35" SessionKey="Q35" DBColumn="Q35" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="1000"></sc:SurveyTextBox>
				</div>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>


				<%}
				   else
				   { %>


				<p class="question">
					Veuillez fournir tout commentaire supplémentaire au sujet de votre plus récente expérience de nos services ou une suggestion qui pourrait rendre votre prochaine visite encore plus agréable. Si vous n'avez pas de commentaires, laissez le champ vide.
				</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="Q34_F" SessionKey="Q34" DBColumn="Q34" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="1000"></sc:SurveyTextBox>
				</div>
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Nous nous efforçons de rendre votre expérience au <% = CasinoName%> la plus agréable possible. Si l'un de nos employés a rendu votre visite particulièrement mémorable, veuillez partager son nom avec nous. Si vous ne connaissez pas son nom, veuillez indiquer la zone de l'établissement où l’employé travaille.
					<br />
					Si vous n'avez pas de commentaires, laissez le champ vide.
				</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="Q35_F" SessionKey="Q35" DBColumn="Q35" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="1000"></sc:SurveyTextBox>
				</div>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>




				<%} %>

				<% }
					   //===========================================================================
					   //PAGE 20 - Q36-Q40 Demographics
					   //===========================================================================
					   else if (CurrentPage == 20)
					   { %>



				<% if (strSurveyLang != "French")
				   { %>




				<p class="question">
					These final questions are for classification purposes only and will allow us to serve you better. All answers are optional.
				</p>
				<p class="question">
					What is your gender?
				</p>
				<sc:MessageManager runat="server" ID="Q36Message"></sc:MessageManager>
				<sc:SurveyRadioButton ID="Q36Male" runat="server" GroupName="Q36" SessionKey="Q36Male" DBColumn="Q36" DBValue="Male" Text="&nbsp;Male" /><br />
				<sc:SurveyRadioButton ID="Q36Female" runat="server" GroupName="Q36" SessionKey="Q36Female" DBColumn="Q36" DBValue="Female" Text="&nbsp;Female" />
				<p class="question">
					What is your age group?
				</p>
				<sc:MessageManager runat="server" ID="Q37Message"></sc:MessageManager>
				<sc:SurveyRadioButton ID="Q37_1" runat="server" GroupName="Q37" SessionKey="Q37_1" DBColumn="Q37" DBValue="19-24" Text="&nbsp;19-24" /><br />
				<sc:SurveyRadioButton ID="Q37_2" runat="server" GroupName="Q37" SessionKey="Q37_2" DBColumn="Q37" DBValue="25-34" Text="&nbsp;25-34" /><br />
				<sc:SurveyRadioButton ID="Q37_3" runat="server" GroupName="Q37" SessionKey="Q37_3" DBColumn="Q37" DBValue="35-44" Text="&nbsp;35-44" /><br />
				<sc:SurveyRadioButton ID="Q37_4" runat="server" GroupName="Q37" SessionKey="Q37_4" DBColumn="Q37" DBValue="45-54" Text="&nbsp;45-54" /><br />
				<sc:SurveyRadioButton ID="Q37_5" runat="server" GroupName="Q37" SessionKey="Q37_5" DBColumn="Q37" DBValue="55-64" Text="&nbsp;55-64" /><br />
				<sc:SurveyRadioButton ID="Q37_6" runat="server" GroupName="Q37" SessionKey="Q37_6" DBColumn="Q37" DBValue="65 or older" Text="&nbsp;65 or older" />
				<p class="question">
					About how often do you come to <%= CasinoName %> for your entertainment or gaming needs?
				</p>
				<sc:MessageManager runat="server" ID="Q38Message"></sc:MessageManager>
				<sc:SurveyRadioButton ID="Q38_1" runat="server" GroupName="Q38" SessionKey="Q38_1" DBColumn="Q38" DBValue="This was my first visit" Text="&nbsp;This was my first visit" /><br />
				<sc:SurveyRadioButton ID="Q38_2" runat="server" GroupName="Q38" SessionKey="Q38_2" DBColumn="Q38" DBValue="2-7 times per week" Text="&nbsp;2-7 times per week" /><br />
				<sc:SurveyRadioButton ID="Q38_3" runat="server" GroupName="Q38" SessionKey="Q38_3" DBColumn="Q38" DBValue="Once per week" Text="&nbsp;Once per week" /><br />
				<sc:SurveyRadioButton ID="Q38_4" runat="server" GroupName="Q38" SessionKey="Q38_4" DBColumn="Q38" DBValue="2-3 times per month" Text="&nbsp;2-3 times per month" /><br />
				<sc:SurveyRadioButton ID="Q38_5" runat="server" GroupName="Q38" SessionKey="Q38_5" DBColumn="Q38" DBValue="Once per month" Text="&nbsp;Once per month" /><br />
				<sc:SurveyRadioButton ID="Q38_6" runat="server" GroupName="Q38" SessionKey="Q38_6" DBColumn="Q38" DBValue="Several times a year" Text="&nbsp;Several times a year" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					Other than English, what other languages do you REGULARLY speak at home, if any?
				</p>
				<div class="randomize">
					<div>
						<sc:SurveyCheckBox ID="Q39_1" runat="server" SessionKey="Q39_1" DBColumn="Q39_1" DBValue="Korean" Text="&nbsp;Korean" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_2" runat="server" SessionKey="Q39_2" DBColumn="Q39_2" DBValue="Punjabi" Text="&nbsp;Punjabi" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_3" runat="server" SessionKey="Q39_3" DBColumn="Q39_3" DBValue="Chinese - Mandarin" Text="&nbsp;Chinese - Mandarin" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_4" runat="server" SessionKey="Q39_4" DBColumn="Q39_4" DBValue="Other Western European languages" Text="&nbsp;Other Western European languages" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_5" runat="server" SessionKey="Q39_5" DBColumn="Q39_5" DBValue="Eastern European languages" Text="&nbsp;Eastern European languages" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_6" runat="server" SessionKey="Q39_6" DBColumn="Q39_6" DBValue="Spanish" Text="&nbsp;Spanish" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_7" runat="server" SessionKey="Q39_7" DBColumn="Q39_7" DBValue="French" Text="&nbsp;French" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_8" runat="server" SessionKey="Q39_8" DBColumn="Q39_8" DBValue="Hindi" Text="&nbsp;Hindi" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_9" runat="server" SessionKey="Q39_9" DBColumn="Q39_9" DBValue="Tagalog" Text="&nbsp;Tagalog" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_10" runat="server" SessionKey="Q39_10" DBColumn="Q39_10" DBValue="Vietnamese" Text="&nbsp;Vietnamese" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_11" runat="server" SessionKey="Q39_11" DBColumn="Q39_11" DBValue="Pakistani" Text="&nbsp;Pakistani" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_12" runat="server" SessionKey="Q39_12" DBColumn="Q39_12" DBValue="Farsi" Text="&nbsp;Farsi" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_13" runat="server" SessionKey="Q39_13" DBColumn="Q39_13" DBValue="Japanese" Text="&nbsp;Japanese" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_14" runat="server" SessionKey="Q39_14" DBColumn="Q39_14" DBValue="Arabic / Middle Eastern" Text="&nbsp;Arabic / Middle Eastern" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_15" runat="server" SessionKey="Q39_15" DBColumn="Q39_15" DBValue="Chinese – Cantonese" Text="&nbsp;Chinese – Cantonese" />
					</div>
				</div>
				<sc:SurveyCheckBox ID="Q39_16" runat="server" SessionKey="Q39_16" DBColumn="Q39_16" DBValue="Other (Specify Below)" Text="&nbsp;Other (Specify Below)" />
				<sc:MessageManager runat="server" ID="Q39_16ExpMessage"></sc:MessageManager>
				<p>Language not included above:</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="Q39_16Explanation" SessionKey="Q39_16Explanation" DBColumn="Q39_16Explanation" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="1000"></sc:SurveyTextBox>
				</div>
				<% } %>
				<% if (Q33.SelectedValue != 1)
				   { %>
				<p class="question">
					<% if (!IsKioskOrStaffEntry)
					   { %>
					Would you like casino staff to contact you about your feedback? Only respond “Yes” if you want to be further contacted by staff <u>about your feedback</u>.
					<% }
					   else
					   { %>
					Would you like Management to respond to you regarding this survey? Only if you respond "Yes" will you be contacted by casino staff.
					<% } %>
				</p>
				<uc1:YesNoControl runat="server" ID="Q40" SessionKey="Q40" DBColumn="Q40" />
				<% } %>
				<% // OLG QUESTIONS (ONTARIO ONLY) %>
				<% if (PropertyShortCode == GCCPropertyShortCode.SSKD || PropertyShortCode == GCCPropertyShortCode.AJA || PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.SCBE || PropertyShortCode == GCCPropertyShortCode.WDB || PropertyShortCode == GCCPropertyShortCode.GBH)
				   { %>
				<p class="question">
					On behalf of the Ontario Lottery & Gaming Corporation (OLG), we would like to ask you an additional 7 questions about your recent experience at <%= CasinoName %>.
					Would you like to provide your feedback to OLG?
				</p>
				<uc1:YesNoControl runat="server" ID="OLGYesNo" SessionKey="OLGYesNo" DBColumn="OLGYesNo" />

				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>



				<%}
				   else if (strSurveyLang == "French")
				   { %>



				<p class="question">
					Ces dernières questions sont uniquement à des fins de classification et nous permettront de mieux vous servir. Toutes les réponses sont facultatives.
				</p>
				<p class="question">
					Quel est votre sexe?
				</p>
				<sc:MessageManager runat="server" ID="MessageManager8"></sc:MessageManager>
				<sc:SurveyRadioButton ID="Q36Male_F" runat="server" GroupName="Q36" SessionKey="Q36Male" DBColumn="Q36" DBValue="Male" Text="&nbsp;Homme" /><br />
				<sc:SurveyRadioButton ID="Q36Female_F" runat="server" GroupName="Q36" SessionKey="Q36Female" DBColumn="Q36" DBValue="Female" Text="&nbsp;Femme" />
				<p class="question">
					Quel est votre groupe d'âge?
				</p>
				<sc:MessageManager runat="server" ID="MessageManager9"></sc:MessageManager>
				<sc:SurveyRadioButton ID="Q37_1_F" runat="server" GroupName="Q37" SessionKey="Q37_1" DBColumn="Q37" DBValue="19-24" Text="&nbsp;19-24" /><br />
				<sc:SurveyRadioButton ID="Q37_2_F" runat="server" GroupName="Q37" SessionKey="Q37_2" DBColumn="Q37" DBValue="25-34" Text="&nbsp;25-34" /><br />
				<sc:SurveyRadioButton ID="Q37_3_F" runat="server" GroupName="Q37" SessionKey="Q37_3" DBColumn="Q37" DBValue="35-44" Text="&nbsp;35-44" /><br />
				<sc:SurveyRadioButton ID="Q37_4_F" runat="server" GroupName="Q37" SessionKey="Q37_4" DBColumn="Q37" DBValue="45-54" Text="&nbsp;45-54" /><br />
				<sc:SurveyRadioButton ID="Q37_5_F" runat="server" GroupName="Q37" SessionKey="Q37_5" DBColumn="Q37" DBValue="55-64" Text="&nbsp;55-64" /><br />
				<sc:SurveyRadioButton ID="Q37_6_F" runat="server" GroupName="Q37" SessionKey="Q37_6" DBColumn="Q37" DBValue="65 or older" Text="&nbsp;65 ans ou plus" />
				<p class="question">
					À quelle fréquence venez-vous au <% = CasinoName%> pour combler vos besoins de jeu ou de divertissement? 
				</p>
				<sc:MessageManager runat="server" ID="MessageManager10"></sc:MessageManager>
				<sc:SurveyRadioButton ID="Q38_1_F" runat="server" GroupName="Q38" SessionKey="Q38_1" DBColumn="Q38" DBValue="This was my first visit" Text="&nbsp;C'était ma première visite" /><br />
				<sc:SurveyRadioButton ID="Q38_2_F" runat="server" GroupName="Q38" SessionKey="Q38_2" DBColumn="Q38" DBValue="2-7 times per week" Text="&nbsp;De 2 à 7 fois par semaine" /><br />
				<sc:SurveyRadioButton ID="Q38_3_F" runat="server" GroupName="Q38" SessionKey="Q38_3" DBColumn="Q38" DBValue="Once per week" Text="&nbsp;Une fois par semaine" /><br />
				<sc:SurveyRadioButton ID="Q38_4_F" runat="server" GroupName="Q38" SessionKey="Q38_4" DBColumn="Q38" DBValue="2-3 times per month" Text="&nbsp;De 2 à 3 fois par mois" /><br />
				<sc:SurveyRadioButton ID="Q38_5_F" runat="server" GroupName="Q38" SessionKey="Q38_5" DBColumn="Q38" DBValue="Once per month" Text="&nbsp;Une fois par mois" /><br />
				<sc:SurveyRadioButton ID="Q38_6_F" runat="server" GroupName="Q38" SessionKey="Q38_6" DBColumn="Q38" DBValue="Several times a year" Text="&nbsp;Plusieurs fois par année" />
				<% if (!IsKioskOrStaffEntry)
				   { %>
				<p class="question">
					À l’exception du français, quelles autres langues parlez-vous RÉGULIÈREMENT à la maison, s’il y a lieu?
				</p>
				<div class="randomize">
					<div>
						<sc:SurveyCheckBox ID="Q39_1_F" runat="server" SessionKey="Q39_1" DBColumn="Q39_1" DBValue="Korean" Text="&nbsp;Coréen" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_2_F" runat="server" SessionKey="Q39_2" DBColumn="Q39_2" DBValue="Punjabi" Text="&nbsp;Punjabi" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_3_F" runat="server" SessionKey="Q39_3" DBColumn="Q39_3" DBValue="Chinese - Mandarin" Text="&nbsp;Chinois Mandarin" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_4_F" runat="server" SessionKey="Q39_4" DBColumn="Q39_4" DBValue="Other Western European languages" Text="&nbsp;Autres langues d'Europe occidentale" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_5_F" runat="server" SessionKey="Q39_5" DBColumn="Q39_5" DBValue="Eastern European languages" Text="&nbsp;Langues de l'Europe de l'Est" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_6_F" runat="server" SessionKey="Q39_6" DBColumn="Q39_6" DBValue="Spanish" Text="&nbsp;Espagnol" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_7_F" runat="server" SessionKey="Q39_7" DBColumn="Q39_7" DBValue="English" Text="&nbsp;Anglais" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_8_F" runat="server" SessionKey="Q39_8" DBColumn="Q39_8" DBValue="Hindi" Text="&nbsp;Hindi" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_9_F" runat="server" SessionKey="Q39_9" DBColumn="Q39_9" DBValue="Tagalog" Text="&nbsp;Tagalog" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_10_F" runat="server" SessionKey="Q39_10" DBColumn="Q39_10" DBValue="Vietnamese" Text="&nbsp;Vietnamien" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_11_F" runat="server" SessionKey="Q39_11" DBColumn="Q39_11" DBValue="Pakistani" Text="&nbsp;Pakistanais" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_12_F" runat="server" SessionKey="Q39_12" DBColumn="Q39_12" DBValue="Farsi" Text="&nbsp;Farsi" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_13_F" runat="server" SessionKey="Q39_13" DBColumn="Q39_13" DBValue="Japanese" Text="&nbsp;Japonais" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_14_F" runat="server" SessionKey="Q39_14" DBColumn="Q39_14" DBValue="Arabic / Middle Eastern" Text="&nbsp;Arabe / Moyen-Orient" />
					</div>
					<div>
						<sc:SurveyCheckBox ID="Q39_15_F" runat="server" SessionKey="Q39_15" DBColumn="Q39_15" DBValue="Chinese – Cantonese" Text="&nbsp;Chinois - Cantonais" />
					</div>
				</div>
				<sc:SurveyCheckBox ID="Q39_16_F" runat="server" SessionKey="Q39_16" DBColumn="Q39_16" DBValue="Other (Specify Below)" Text="&nbsp;Autre (précisez ci-dessous)" />
				<sc:MessageManager runat="server" ID="MessageManager11"></sc:MessageManager>
				<p>Langue non mentionnée ci-dessus :</p>
				<div class="text-center">
					<sc:SurveyTextBox ID="Q39_16Explanation_F" SessionKey="Q39_16Explanation" DBColumn="Q39_16Explanation" runat="server" TextMode="MultiLine" Rows="5" Style="width: 95%;" MaxLength="1000"></sc:SurveyTextBox>
				</div>
				<% } %>
				<% if (Q33_F.SelectedValue_F != 1)
				   { %>
				<p class="question">
					<% if (!IsKioskOrStaffEntry)
					   { %>
					Souhaitez-vous recevoir une réponse du personnel concernant vos commentaires? Le personnel ne communiquera avec vous que si vous cochez «Oui».
					<% }
					   else
					   { %>
					Souhaitez-vous que la direction vous réponde à propos de ce sondage? Seulement si vous répondez «Oui» vous sera contacté par le personnel du casino.
					<% } %>
				</p>
				<uc1:YesNoControlFrench runat="server" ID="Q40_F" SessionKey="Q40" DBColumn="Q40" />
				<% } %>
				<% // OLG QUESTIONS (ONTARIO ONLY) %>
				<% if (PropertyShortCode == GCCPropertyShortCode.SSKD || PropertyShortCode == GCCPropertyShortCode.AJA || PropertyShortCode == GCCPropertyShortCode.SCTI || PropertyShortCode == GCCPropertyShortCode.SCBE || PropertyShortCode == GCCPropertyShortCode.WDB || PropertyShortCode == GCCPropertyShortCode.GBH)
				   { %>
				<p class="question">
					Au nom de la Société des loteries et des jeux de l'Ontario (OLG), nous aimerions vous poser 7 autres questions au sujet de votre expérience récente au <% = CasinoName%>. Pouvons-nous transmettre vos commentaires à l’OLG?
				</p>
				<uc1:YesNoControlFrench runat="server" ID="OLGYesNo_F" SessionKey="OLGYesNo" DBColumn="OLGYesNo" />

				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>



				<%} %>






				<% }
					   //===========================================================================
					   //PAGE 21 - Q41 Contact Info
					   //===========================================================================
					   else if (CurrentPage == 21)
					   { %>


				<% if (strSurveyLang != "French")
				   { %>

				<% if (Q33.SelectedValue == 1 || Q40.SelectedValue == 1)
				   { %>
				<p class="question">
					<% if (Q27.SelectedValue != 1)
					   { %>
					<p class="question">
						<% if (!IsKioskOrStaffEntry)
						   { %>
								You have indicated that you want a staff member to follow-up with you about your feedback. To help us direct your comments, please categorize your feedback below:
							<% }
						   else
						   { %>
								Based on the guest’s responses, please categorize this feedback so that the right property staff can be notified for follow-up:
							<% } %>
					</p>
					<sc:MessageManager runat="server" ID="Q40Message"></sc:MessageManager>
					<sc:SurveyRadioButton ID="radQ40A_1" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_1" DBColumn="Q27A_ArrivalAndParking" DBValue="1" Text="&nbsp;Arrival and parking" /><br />
					<sc:SurveyRadioButton ID="radQ40A_2" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_2" DBColumn="Q27A_GuestServices" DBValue="1" Text="&nbsp;Guest Services" /><br />
					<sc:SurveyRadioButton ID="radQ40A_3" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_3" DBColumn="Q27A_Cashiers" DBValue="1" Text="&nbsp;Cashiers" /><br />
					<sc:SurveyRadioButton ID="radQ40A_4" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_4" DBColumn="Q27A_ManagerSupervisor" DBValue="1" Text="&nbsp;Manager/Supervisor" /><br />
					<sc:SurveyRadioButton ID="radQ40A_5" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_5" DBColumn="Q27A_Security" DBValue="1" Text="&nbsp;Security" /><br />
					<sc:SurveyRadioButton ID="radQ40A_6" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_6" DBColumn="Q27A_Slots" DBValue="1" Text="&nbsp;Slots" /><% if (radQ40A_6.Visible)
																																														  { %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_7" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_7" DBColumn="Q27A_Tables" DBValue="1" Text="&nbsp;Tables" /><% if (radQ40A_7.Visible)
																																															{ %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_8" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_8" DBColumn="Q27A_FoodAndBeverage" DBValue="1" Text="&nbsp;Food & Beverage" /><br />
					<sc:SurveyRadioButton ID="radQ40A_9" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_9" DBColumn="Q27A_Hotel" DBValue="1" Text="&nbsp;Hotel" /><% if (radQ40A_9.Visible)
																																														  { %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_11" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_11" DBColumn="Q27A_Bingo" DBValue="1" Text="&nbsp;Bingo" /><% if (radQ40A_11.Visible)
																																															{ %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_12" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_12" DBColumn="Q27A_Entertainment" DBValue="1" Text="&nbsp;Entertainment" /><% if (radQ40A_12.Visible)
																																																			{ %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_13" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_13" DBColumn="Q27A_HorseRacing" DBValue="1" Text="&nbsp;Horse&nbsp;Racing" /><% if (radQ40A_13.Visible)
																																																			  { %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_10" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_10" DBColumn="Q27A_Other" DBValue="1" Text="&nbsp;Other" />
					<sc:SurveyTextBox ID="txtQ40OtherExplanation" runat="server" SessionKey="Q27A_OtherExplanation" DBColumn="Q27A_OtherExplanation" MaxLength="500" Size="50"></sc:SurveyTextBox><br />
					<% } %>
				</p>
				<p class="question">
					<% if (!IsKioskOrStaffEntry)
					   { %>
					Please confirm your contact information if you wish to be contacted. First and last name are mandatory. <u>You will be contacted using the email address provided at the start of this survey.</u> If you would prefer to be contacted via Telephone, please provide it below.
					<% }
					   else
					   { %>
					Please confirm your contact information if you wish to be contacted. First and last name are mandatory. All feedback will be responded to via email.  If you would prefer to be contacted via Telephone, please provide it below.
					<% } %>
				</p>
				<div class="row grid-row">
					<div class="col-md-4">First Name</div>
					<div class="col-md-6">
						<sc:SurveyTextBox ID="txtFirstName" SessionKey="txtFirstName" DBColumn="FirstName" runat="server" MaxLength="100" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
					</div>
				</div>
				<div class="row grid-row">
					<div class="col-md-4">Last Name</div>
					<div class="col-md-6">
						<sc:SurveyTextBox ID="txtLastName" SessionKey="txtLastName" DBColumn="LastName" runat="server" MaxLength="100" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
					</div>
				</div>
				<% if (IsKioskOrStaffEntry)
				   { %>
				<div class="row grid-row">
					<div class="col-md-4">Email address</div>
					<div class="col-md-6">
						<sc:SurveyTextBox ID="txtEmail2" runat="server" SessionKey="ContactEmail" DBColumn="ContactEmail" MaxLength="150" Size="50" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
					</div>
				</div>
				<% } %>
				<div class="row grid-row">
					<div class="col-md-4">Tel # (numbers only please. Example: 555 234 5678):</div>
					<div class="col-md-6">
						<sc:SurveyTextBox ID="txtTelephoneNumber" SessionKey="txtTelephoneNumber" DBColumn="TelephoneNumber" runat="server" MaxLength="30" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
					</div>
				</div>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>


				<% }
				   else if (strSurveyLang == "French")
				   { %>


				<% if (Q33_F.SelectedValue_F == 1 || Q40_F.SelectedValue_F == 1)
				   { %>
				<p class="question">
					<% if (Q27_F.SelectedValue_F != 1)
					   { %>
					<p class="question">
						<% if (!IsKioskOrStaffEntry)
						   { %>
								Vous avez indiqué que vous vouliez qu'un membre du personnel fasse le suivi de vos commentaires. Pour nous aider à diriger vos commentaires, veuillez classer vos commentaires ci-dessous:
							<% }
						   else
						   { %>
								Sur la base des réponses des invités, veuillez classer ces commentaires afin que le personnel de la propriété droit puisse être averti pour le suivi:
							<% } %>
					</p>
					<sc:MessageManager runat="server" ID="MessageManager12"></sc:MessageManager>
					<sc:SurveyRadioButton ID="radQ40A_1_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_1" DBColumn="Q27A_ArrivalAndParking" DBValue="1" Text="&nbsp;Arrivée et stationnement" /><br />
					<sc:SurveyRadioButton ID="radQ40A_2_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_2" DBColumn="Q27A_GuestServices" DBValue="1" Text="&nbsp;Service à la clientèle" /><br />
					<sc:SurveyRadioButton ID="radQ40A_3_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_3" DBColumn="Q27A_Cashiers" DBValue="1" Text="&nbsp;caissiers" /><br />
					<sc:SurveyRadioButton ID="radQ40A_4_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_4" DBColumn="Q27A_ManagerSupervisor" DBValue="1" Text="&nbsp;Gestionnaire / Superviseur" /><br />
					<sc:SurveyRadioButton ID="radQ40A_5_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_5" DBColumn="Q27A_Security" DBValue="1" Text="&nbsp;la sécurité" /><br />
					<sc:SurveyRadioButton ID="radQ40A_6_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_6" DBColumn="Q27A_Slots" DBValue="1" Text="&nbsp;Machines à sous" /><% if (radQ40A_6_F.Visible)
																																																	  { %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_7_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_7" DBColumn="Q27A_Tables" DBValue="1" Text="&nbsp;les tables" /><% if (radQ40A_7_F.Visible)
																																																  { %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_8_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_8" DBColumn="Q27A_FoodAndBeverage" DBValue="1" Text="&nbsp;nourriture et boissons" /><br />
					<sc:SurveyRadioButton ID="radQ40A_9_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_9" DBColumn="Q27A_Hotel" DBValue="1" Text="&nbsp;Un hôtel" /><% if (radQ40A_9_F.Visible)
																																															   { %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_11_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_11" DBColumn="Q27A_Bingo" DBValue="1" Text="&nbsp;Bingo" /><% if (radQ40A_11_F.Visible)
																																															  { %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_12_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_12" DBColumn="Q27A_Entertainment" DBValue="1" Text="&nbsp;Divertissement" /><% if (radQ40A_12_F.Visible)
																																																			   { %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_13_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_13" DBColumn="Q27A_HorseRacing" DBValue="1" Text="&nbsp;Cheval&nbsp;Courses" /><% if (radQ40A_13_F.Visible)
																																																				  { %><br />
					<% } %>
					<sc:SurveyRadioButton ID="radQ40A_10_F" runat="server" GroupName="Q27A_ProblemDescription" SessionKey="Q27A_10" DBColumn="Q27A_Other" DBValue="1" Text="&nbsp;Autre" />
					<sc:SurveyTextBox ID="txtQ40OtherExplanation_F" runat="server" SessionKey="Q27A_OtherExplanation" DBColumn="Q27A_OtherExplanation" MaxLength="500" Size="50"></sc:SurveyTextBox><br />
					<% } %>
				</p>
				<p class="question">
					<% if (!IsKioskOrStaffEntry)
					   { %>
					Veuillez fournir vos coordonnées si vous souhaitez que nous communiquions avec vous. Votre prénom et nom de famille sont obligatoires. Nous vous communiquerons à l’adresse électronique indiquée au début de ce sondage. Si vous préférez que l’on vous appelle, veuillez nous fournir votre numéro de téléphone.
					<% }
					   else
					   { %>
					Veuillez confirmer vos coordonnées si vous souhaitez être contacté. Prénom et nom sont obligatoires. Tous les commentaires seront répondu par courrier électronique. Si vous préférez être contacté par téléphone, veuillez le fournir ci-dessous.
					<% } %>
				</p>
				<div class="row grid-row">
					<div class="col-md-4">Prénom</div>
					<div class="col-md-6">
						<sc:SurveyTextBox ID="txtFirstName_F" SessionKey="txtFirstName" DBColumn="FirstName" runat="server" MaxLength="100" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
					</div>
				</div>
				<div class="row grid-row">
					<div class="col-md-4">Nom de famille</div>
					<div class="col-md-6">
						<sc:SurveyTextBox ID="txtLastName_F" SessionKey="txtLastName" DBColumn="LastName" runat="server" MaxLength="100" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
					</div>
				</div>
				<% if (IsKioskOrStaffEntry)
				   { %>
				<div class="row grid-row">
					<div class="col-md-4">Adresse courriel</div>
					<div class="col-md-6">
						<sc:SurveyTextBox ID="txtEmail2_F" runat="server" SessionKey="ContactEmail" DBColumn="ContactEmail" MaxLength="150" Size="50" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
					</div>
				</div>
				<% } %>
				<div class="row grid-row">
					<div class="col-md-4">Tel # (Numéro de tél. Exemple: 555 234 5678):</div>
					<div class="col-md-6">
						<sc:SurveyTextBox ID="txtTelephoneNumber_F" SessionKey="txtTelephoneNumber" DBColumn="TelephoneNumber" runat="server" MaxLength="30" autocomplete="off" aria-autocomplete="none"></sc:SurveyTextBox>
					</div>
				</div>
				<% } %>
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>
				<% } %>



				<%}%>






				<%  //===========================================================================
					   //PAGE 22 - OLG Questions
					   //===========================================================================


					   else if (CurrentPage == 22)
					   { %>



				<% if (strSurveyLang != "French")
				   { %>





				<% if (OLGYesNo.SelectedValue == 1)
				   { %>
				<p class="question">1. On a scale of 1 to 10 where 1 equals 'not at all satisfied' and 10 equals 'completely satisfied', how satisfied were you with <%= CasinoName %> overall?</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Not at all Satisfied</p>
				</div>
				<div class="col-sm-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ1" SessionKey="OLGQ1" DBColumn="OLGQ1" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Completely Satisfied</p>
				</div>
				<br />
				<br />
				<p class="question">2. On a scale of 1 to 10 where 1 equals 'not at all satisfied' and 10 equals 'completely satisfied', how strongly do you agree that your visit to <%= CasinoName %> met your entertainment expectations?</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Not at all Satisfied</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ2" SessionKey="OLGQ2" DBColumn="OLGQ2" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Completely Satisfied</p>
				</div>
				<br />
				<br />
				<p class="question">3. On a scale of 1 to 10 where 1 equals 'definitely would not recommend' and 10 equals 'definitely would recommend', how likely would you be to recommend <%= CasinoName %> to your family & friends?</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Definitely would not recommend</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ3" SessionKey="OLGQ3" DBColumn="OLGQ3" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Definitely would recommend</p>
				</div>
				<br />
				<br />
				<p class="question">4. On a scale of 1 to 10 where 1 equals 'not at all satisfied' and 10 equals 'completely satisfied', I am satisfied with the quality of the resources available at <%= CasinoName %> for getting help with gambling related issues.</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Not at all Satisfied</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ4" SessionKey="OLGQ4" DBColumn="OLGQ4" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Completely Satisfied</p>
				</div>
				<br />
				<br />
				<p class="question">5. On a scale of 1 to 10 where 1 equals 'Do not agree' and 10 equals 'completely agree', I agree that the Responsible Gaming Resource Centre at <%= CasinoName %> provides useful / usable information.</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Do Not Agree</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ5" SessionKey="OLGQ5" DBColumn="OLGQ5" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Completely Agree</p>
				</div>
				<br />
				<br />
				<p class="question">6. On a scale of 1 to 10 where 1 equals 'Do not agree' and 10 equals 'completely agree', I agree that Information about how games work is readily available at <%= CasinoName %>.</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Do Not Agree</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ6" SessionKey="OLGQ6" DBColumn="OLGQ6" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Completely Agree</p>
				</div>
				<br />
				<br />
				<p class="question">7. On a scale of 1 to 10 where 1 equals 'Do not agree' and 10 equals 'completely agree', I agree that Information about smart play habits is readily available at <%= CasinoName %>.</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Do Not Agree</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ7" SessionKey="OLGQ7" DBColumn="OLGQ7" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Completely Agree</p>
				</div>
				<br />
				<br />
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Next" />
				</div>
				<% } %>

				<% }

				   else if (strSurveyLang == "French")
				   { %>


				<% if (OLGYesNo_F.SelectedValue_F == 1)
				   { %>
				<p class="question">1. Sur une échelle de 1 à 10, ou 1 correspond à « pas du tout satisfait » et 10 à « pleinement satisfait », à quel point êtes-vous satisfait du <% = CasinoName%> dans l'ensemble?</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Pas du tout satisfait</p>
				</div>
				<div class="col-sm-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ1_F" SessionKey="OLGQ1" DBColumn="OLGQ1" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Pleinement satisfait</p>
				</div>
				<br />
				<br />
				<p class="question">2. Sur une échelle de 1 à 10, ou 1 correspond à « pas du tout satisfait » et 10 à « pleinement satisfait », à quel point êtes-vous d'accord que votre visite au <% = CasinoName%> a répondu à vos attentes de divertissement?</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Pas du tout satisfait</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ2_F" SessionKey="OLGQ2" DBColumn="OLGQ2" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Pleinement satisfait</p>
				</div>
				<br />
				<br />
				<p class="question">3. Sur une échelle de 1 à 10, ou 1 correspond à « ne recommanderait définitivement pas » et 10 à « recommanderait définitivement », quelle est la probabilité que vous recommandiez <% = CasinoName%> à votre famille et vos amis?</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Je ne le recommande pas</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ3_F" SessionKey="OLGQ3" DBColumn="OLGQ3" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Je le recommande sans hésiter</p>
				</div>
				<br />
				<br />
				<p class="question">4. Sur une échelle de 1 à 10, ou 1 correspond à « pas du tout satisfait » et 10 à « pleinement satisfait », je suis satisfait de la qualité des ressources disponibles au <% = CasinoName%> pour obtenir de l'aide sur les questions liées au jeu.</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Pas du tout satisfait</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ4_F" SessionKey="OLGQ4" DBColumn="OLGQ4" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Pleinement satisfait</p>
				</div>
				<br />
				<br />
				<p class="question">5. Sur une échelle de 1 à 10, ou 1 correspond à « pas du tout d’accord » et 10 à « pleinement d’accord », je suis d'accord que le centre de ressources de jeu responsable du <% = CasinoName%> fournit de l’information utile et pertinente.</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Pas du tout d’accord</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ5_F" SessionKey="OLGQ5" DBColumn="OLGQ5" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Pleinement d’accord </p>
				</div>
				<br />
				<br />
				<p class="question">6. Sur une échelle de 1 à 10, ou 1 correspond à « pas du tout d’accord » et 10 à « pleinement d’accord », je suis d'accord que les informations sur le déroulement des jeux sont facilement accessibles au <% = CasinoName%>.</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Pas du tout d’accord</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ6_F" SessionKey="OLGQ6" DBColumn="OLGQ6" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Pleinement d’accord</p>
				</div>
				<br />
				<br />
				<p class="question">7. Sur une échelle de 1 à 10, ou 1 correspond à « pas du tout d’accord » et 10 à « pleinement d’accord », je suis d'accord que les informations sur les habitudes de jeu responsable sont facilement accessibles au <% = CasinoName%>.</p>
				<br />
				<br />
				<div class="col-sm-3">
					<p class="satisfied">Pas du tout d’accord</p>
				</div>
				<div class="col-md-6">
					<uc1:TenScaleQuestionControl runat="server" ID="OLGQ7_F" SessionKey="OLGQ7" DBColumn="OLGQ7" Label="" HideZero="true" />
				</div>
				<div class="col-sm-3">
					<p class="satisfied">Pleinement d’accord</p>
				</div>
				<br />
				<br />
				<div class="button-container">
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Arrière" />
					<asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="Suivant" />
				</div>




				<% } %>

				<%} %>


				<% } //===========================================================================
					   //PAGE 97 - Final Step
					   //===========================================================================
					   else if (CurrentPage == 97)
					   { %>
				<sc:MessageManager runat="server" ID="mmLastPage"></sc:MessageManager>
				<div class="button-container">
					<% if (SurveyComplete)
					   {
						   if (SurveyType == SharedClasses.GEISurveyType.StaffSurvey)
						   { %>

					<% }
						   else if (SurveyType != SharedClasses.GEISurveyType.Kiosk)
						   { %>
					<%if (strSurveyLang != "French")
					  { %>
					<a href="<%= GetCasinoURL() %>" class="btn btn-primary">Continue</a>
					<%}
					  else if (strSurveyLang == "French")
					  { %>
					<a href="<%= GetCasinoURL() %>" class="btn btn-primary">Continuer</a>
					<%} %>
					<% }
						   else
						   { %>
					<%if (strSurveyLang == "French")
					  { %>
					<a href="<%= GetSurveyURL(-1) %>" class="btn btn-primary">Start Over</a>
					<%}
					  else if (strSurveyLang != "French")
					  { %>
					<a href="<%= GetSurveyURL(-1) %>" class="btn btn-primary">Recommencer</a>
					<%} %>
					<% }
					   }
					   else
					   { %>
					<asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="Back" />
					<% } %>
				</div>

				<% } %>

				<%} %>

				<%-- //if ( String.IsNullOrWhiteSpace( TopMessage.ErrorMessage ) { %>--%>
				<%--<sc:MessageManager runat="server" TitleOverride="Debug Info" DisplayAs="Info" CSSStyle="margin-top:10px;">
					<Message>
						Survey Type: <%= SurveyType %><br />
						Property ID: <%= PropertyID %><br />
						Property: <%= ThemeProperty %><br />
						Property Short Code: <%= PropertyShortCode %><br />
						Page: <%= CurrentPage %><br />
						Email PIN: <%= EmailPIN.ToString() %>
					</Message>
				</sc:MessageManager>
					
					
					
					<% // OLG QUESTIONS (ONTARIO ONLY) %>
				<% if ( PropertyShortCode == GCCPropertyShortCode.SSKD || PropertyShortCode == GCCPropertyShortCode.SCTI ) { %>
				<p class="question">
					On behalf of the Ontario Lottery & Gaming Corporation (OLG), we would like to ask you an additional 7 questions about your recent experience at <%= CasinoName %>.
					Would you like to provide your feedback to OLG?
				</p>                
				<uc1:YesNoControl runat="server" ID="OLGYesNo" SessionKey="OLGYesNo" DBColumn="OLGYesNo" />

				<% } %>
	 
				--%>
			</div>
		</div>
		<!-- /.container -->
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
		<script src="/Scripts/icheck.min.js"></script>
		<script>
			$(".randomize")
				.children()
				.sort(function () {
					return Math.random() * 10 > 5 ? 1 : -1;
				}).each(function () {
					var $t = $(this);
					$t.appendTo($t.parent());
				});
			$('input.date-picker').datepicker({ language: '<%= strSurveyLang == "French" ? "fr" : "en" %>' });


			$('input').iCheck({
				labelHover: false,
				cursor: true,
				checkboxClass: 'icheckbox_flat-blue',
				radioClass: 'iradio_flat-blue'
			});




			//20171128 -  commented out this section since on Page 10 multiple options are getting selected at the same time
		<%--	<% if ( SurveyType == GEISurveyType.StaffSurvey && CurrentPage == 10 ) { %>
			$(".scale-question ").on("ifClicked", "input[type=radio]", function (evt) {
				var $this = $(this);
				$(".scale-question input[value=" + $this.val() + "]").not($this).iCheck('check');
			});
			<% } %>--%>





		</script>

	</form>
</body>
</html>
