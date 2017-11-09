using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace GCGC_Web_Portal
{
	public static class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			//routes.EnableFriendlyUrls();
			
			// Main pages
			routes.MapPageRoute( "Default", "Default", "~/Default.aspx" );
			routes.MapPageRoute( "Login", "Login", "~/Login.aspx" );
			routes.MapPageRoute( "PasswordChange", "PasswordChange", "~/PasswordChange.aspx" );
			routes.MapPageRoute( "ResetPassword", "ResetPassword", "~/ResetPassword.aspx" );
			
			// Surveys
			routes.MapPageRoute( "GEISurvey", "Survey/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveyGEI.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "GEISurveyEmail", "SE/{propertyshortcode}/{pin}/{page}/{redirectdirection}", "~/SurveyGEI.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "{pin}", "none" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "GEISurveyKiosk", "SurveyK/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveyGEI.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "GEISurveyStaff", "SurveyS/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveyGEI.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "TermsAndConditions", "TermsAndConditions/{propertyshortcode}", "~/TAndCs.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute("TermsAndConditions_French", "TermsAndConditions_French/{propertyshortcode}", "~/TAndCs_French.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } });

			//Seperate Terms and Conditions for CNB
			routes.MapPageRoute("TermsAndConditions_CNB", "TermsAndConditions_CNB/{propertyshortcode}", "~/TAndCCNB.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } });
			routes.MapPageRoute("TermsAndConditions_French_CNB", "TermsAndConditions_French_CNB/{propertyshortcode}", "~/TAndCCNB_French.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } });

			//Seperate Terms and Conditions for SCTI
			routes.MapPageRoute("TermsAndConditions_SCTI", "TermsAndConditions_SCTI/{propertyshortcode}", "~/TAndCSCTI.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } });
			routes.MapPageRoute("TermsAndConditions_French_SCTI", "TermsAndConditions_French_SCTI/{propertyshortcode}", "~/TandSCTI_French.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } });




			routes.MapPageRoute( "SurveyFeedback", "Feedback/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveyFeedback.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "SurveyFeedbackStaff", "SFeedback/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveyFeedback.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "SurveyHotel", "HotelSurvey/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveyHotel.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "SurveyDonation", "DonationRequest/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveyDonation.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "page", "1" }, { "redirectdirection", "1" } } );
			//routes.MapPageRoute( "SurveySnapshot2017", "Snapshot/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveySnapshot2017.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "page", "1" }, { "redirectdirection", "1" }, { "surveytype", "DirectAccess" } } );
			
			//routes.MapPageRoute( "SurveySnapshot2017Kiosk", "SnapshotK/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveySnapshot2017.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "page", "1" }, { "redirectdirection", "1" }, { "surveytype", "Kiosk" } } );



			routes.MapPageRoute("SurveySnapshot2017", "Snapshot/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveySnapshot2017.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "RR" }, { "page", "1" }, { "redirectdirection", "1" }, { "surveytype", "DirectAccess" } });

			routes.MapPageRoute("SurveySnapshot2017Kiosk", "SnapshotK/{propertyshortcode}/{page}/{redirectdirection}", "~/SurveySnapshot2017.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "RR" }, { "page", "1" }, { "redirectdirection", "1" }, { "surveytype", "Kiosk" } });


			routes.MapPageRoute( "HastingsSurvey", "HastingsSurvey/{page}/{redirectdirection}", "~/HastingsSurvey.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "HA" }, { "page", "1" }, { "redirectdirection", "1" } });
			routes.MapPageRoute( "TandCHastings", "TandCHastings", "~/TandCHastings.aspx" );

			routes.MapPageRoute( "StaffSurveySelection", "StaffSurveySelection", "~/StaffSurveySelection.aspx" );

			//GSEI Surveys
			routes.MapPageRoute( "GSEITermsAndConditions", "GSEITermsAndConditions/{propertyshortcode}", "~/Surveys/GSEI/GSEITAndCs.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute("GSEITermsAndConditions_French", "GSEITermsAndConditions_French/{propertyshortcode}", "~/Surveys/GSEI/GSEITAndCs_French.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } });
			routes.MapPageRoute( "GSEI_BC", "GSEIBC/{propertyshortcode}/{pin}/{page}/{redirectdirection}", "~/Surveys/GSEI/SurveyGSEI.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "{pin}", "none" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "GSEI_HP", "GSEIHP/{propertyshortcode}/{pin}/{page}/{redirectdirection}", "~/Surveys/GSEI/SurveyGSEI.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "{pin}", "none" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "GSEI_HO", "GSEIHO/{propertyshortcode}/{pin}/{page}/{redirectdirection}", "~/Surveys/GSEI/SurveyGSEI.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "{pin}", "none" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "GSEI_TM", "GSEITM/{propertyshortcode}/{pin}/{page}/{redirectdirection}", "~/Surveys/GSEI/SurveyGSEI.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "{pin}", "none" }, { "page", "1" }, { "redirectdirection", "1" } } );
			routes.MapPageRoute( "GSEI_GA", "GSEIGA/{propertyshortcode}/{pin}/{page}/{redirectdirection}", "~/Surveys/GSEI/SurveyGSEI.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" }, { "{pin}", "none" }, { "page", "1" }, { "redirectdirection", "1" } } );

			//Reports
			routes.MapPageRoute( "PropertyDashboard", "PropertyDashboard/{propertyshortcode}", "~/PropertyDashboard.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "GEINPS", "GEINPS/{propertyshortcode}", "~/Reports/GEINPS.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "PRS", "PRS/{propertyshortcode}", "~/Reports/PRS.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "Followup", "Followup/{propertyshortcode}", "~/Reports/Followup.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "Facilities", "Facilities/{propertyshortcode}", "~/Reports/Facilities.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "Staff", "Staff/{propertyshortcode}", "~/Reports/Staff.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "Gaming", "Gaming/{propertyshortcode}", "~/Reports/Gaming.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "FoodAndBev", "FoodAndBev/{propertyshortcode}", "~/Reports/FoodAndBev.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "Lounge", "Lounge/{propertyshortcode}", "~/Reports/Lounge.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "Theatre", "Theatre/{propertyshortcode}", "~/Reports/Theatre.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "SnapshotStatus", "SnapshotStatus", "~/Reports/SnapshotStatus.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } } );
			routes.MapPageRoute( "SocialMediaDashboard", "SocialMediaDashboard/{propertyshortcode}", "~/Reports/SocialMediaDashboard.aspx", false, new RouteValueDictionary() { { "propertyshortcode", "GCC" } });

			routes.MapPageRoute( "QuestionTopBottom", "QuestionTopBottom", "~/Reports/QuestionTopBottom.aspx" );
			routes.MapPageRoute( "RespondentProfile", "RespondentProfile", "~/Reports/RespondentProfile.aspx" );

			routes.MapPageRoute( "FeedbackExport", "Reports/FeedbackExport", "~/Reports/FeedbackExport.aspx");

			routes.MapPageRoute( "RespondentDetails", "GuestDetails/{respid}", "~/Reports/RespondentDetails.aspx", false, new RouteValueDictionary() { { "respid", null } } );
			routes.MapPageRoute( "SnapshotExport", "SnapshotExport", "~/Reports/SnapshotExport.aspx" );

			routes.MapPageRoute( "AbandonmentReport", "Admin/Abandonment", "~/Admin/AbandonmentReport.aspx" );
			routes.MapPageRoute( "WinnerSelection", "Admin/WinnerSelection", "~/Admin/WinnerSelection.aspx" );
			routes.MapPageRoute( "CrossTabs", "Admin/CrossTabs", "~/Admin/CrossTabReport.aspx" );

			routes.MapPageRoute( "QuarterlyReport", "Reports/Quarterly", "~/Reports/QuarterlyReport.aspx" );
			routes.MapPageRoute( "MonthlyReport", "Reports/Monthly", "~/Reports/MonthlyReport.aspx" );
			routes.MapPageRoute( "MonthlyReportHotel", "Reports/MonthlyHotel", "~/Reports/Hotel/MonthlyReport.aspx" );
			routes.MapPageRoute( "KeyDriver", "Reports/KeyDriver", "~/Reports/KeyDriver.aspx" );
			routes.MapPageRoute( "ComparisonReport", "Reports/Comparison", "~/Reports/ComparisonReport.aspx" );
			routes.MapPageRoute( "WordCloud", "Reports/WordCloud", "~/Reports/WordCloud.aspx" );


			routes.MapPageRoute( "HotelOverall", "Reports/Hotel/Overall", "~/Reports/Hotel/Overall.aspx" );
			routes.MapPageRoute( "HotelRooms", "Reports/Hotel/Rooms", "~/Reports/Hotel/Rooms.aspx" );
			routes.MapPageRoute( "HotelFB", "Reports/Hotel/FB", "~/Reports/Hotel/FB.aspx" );
			routes.MapPageRoute( "HotelPRS", "Reports/Hotel/PRS", "~/Reports/Hotel/PRS.aspx" );
			

			//Admin
			routes.MapPageRoute( "FeedbackList", "Admin/Feedback/List/{page}", "~/Admin/FeedbackList.aspx", false, new RouteValueDictionary() { { "page", "1" } } );
			routes.MapPageRoute( "AdminFeedback", "Admin/Feedback/{guid}", "~/Admin/FeedbackItem.aspx" );
			routes.MapPageRoute( "FBK", "F/{guid}", "~/GuestFeedback.aspx" );
			routes.MapPageRoute( "SurveyList", "Admin/Surveys/{page}", "~/Admin/SurveyList.aspx", false, new RouteValueDictionary() { { "page", "1" } } );
			routes.MapPageRoute( "SurveyDisplay", "Display/{surveytype}/{recordid}", "~/DisplaySurvey.aspx", false, new RouteValueDictionary() { { "surveytype", "None" } } );

			routes.MapPageRoute( "PINGenerator", "Admin/PINGenerator/{batchid}", "~/Admin/EmailPINGenerator.aspx", false, new RouteValueDictionary() { { "batchid", "-1" } } );

			routes.MapPageRoute( "NotificationManagement", "Admin/NotificationManagement", "~/Admin/NotificationManagement.aspx" );
			routes.MapPageRoute( "UserList", "Admin/Users/{page}", "~/Admin/UserList.aspx", false, new RouteValueDictionary() { { "page", "1" } } );
			routes.MapPageRoute( "UserEdit", "Admin/User/{userid}", "~/Admin/UserEdit.aspx" );
			routes.MapPageRoute( "UserAdd", "Admin/AddUser", "~/Admin/UserAdd.aspx" );
			routes.MapPageRoute( "DataExport", "Admin/DataExport", "~/Admin/DataExport.aspx" );


			routes.MapPageRoute( "EmailLog", "EmailLog", "~/EmailLog.aspx" );
			
		}
	}
}
