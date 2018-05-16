<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyFeedback.aspx.cs" Inherits="GCC_Web_Portal.SurveyFeedback" MasterPageFile="~/Survey.Master" Culture="Auto" UICulture="Auto"%>
<%@ MasterType VirtualPath="~/Survey.Master" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Register Src="~/Controls/QuestionRowControl.ascx" TagPrefix="uc1" TagName="QuestionRowControl" %>
<%@ Register Src="~/Controls/ScaleQuestionControl.ascx" TagPrefix="uc1" TagName="ScaleQuestionControl" %>
<%@ Register Src="~/Controls/YesNoControl.ascx" TagPrefix="uc1" TagName="YesNoControl" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
    <h1><%=Lang.SFeedbackPageTitle %></h1>
    
   <% if (Session["CurrentUI"] == "fr-CA") { %>
            <asp:Button ID="btnEnglish" CssClass="btn btn-primary" runat="server" Text="English" OnClick="EnglishLinkButton_Click" />
            <br /><br />
       <% } else { %>
            <asp:Button ID="btnFrench" CssClass="btn btn-primary" runat="server" Text="Français" OnClick="FrenchLinkButton_Click" />
            <br /><br />
      <%  }  %>
    <%
        //===========================================================================
        //PAGE 1 - Email
        //===========================================================================
    if ( Master.CurrentPage == 1) {%>
    <% if ( IsStaffSurvey ) { %>
    <p><%= Lang.Page1_StaffName%>: <%= ReportingTools.CleanData( User.FullName ) %></p>
    <p><%= Lang.Page1_Question1%></p>
    <sc:SurveyDropDown runat="server" ID="ddlStaffContact" SessionKey="StaffContact" DBColumn="StaffContact">
        <asp:ListItem Value="" Text="--"></asp:ListItem>
    </sc:SurveyDropDown>
    <br /><br />
    <p><% =Lang.Page1_EnterEmail %></p>
    <% } else { %>


     <% if (Session["CurrentUI"] == "fr-CA") { %>
    <p><% =Lang.Page1_Thankyou %> <%= Master.CasinoNameFrench %>! </p>
     <% } else { %>
     <p><% =Lang.Page1_Thankyou %> <%= Master.CasinoName %>! </p>
     <%  }  %>
    
    
    
    
    <p><% =Lang.Page1_StartFeedback %></p>
    <% } %>
    <sc:MessageManager runat="server" ID="mmTrackingEmail"></sc:MessageManager>
    <p>
        <%=Lang.Page1_EmailAddress %>: <sc:SurveyTextBox ID="fbkEmail" runat="server" SessionKey="fbkEmail" DBColumn="TrackingEmail" MaxLength="150" Size="50"></sc:SurveyTextBox>
    </p>
    <% if ( Master.PropertyShortCode == GCCPropertyShortCode.GCC ) { %>
    <p><% =Lang.Page1_SelectProperty %>:</p>
    <sc:SurveyDropDown runat="server" ID="fbkProperty" SessionKey="SelectedPropertyID" DBColumn="SelectedPropertyID">
        <asp:ListItem Text="--" Value="" ></asp:ListItem>
    </sc:SurveyDropDown>
    <br />
    <div id="direct-question-container">
        <p><% =Lang.Page1_Question2B %>:</p>
        <sc:SurveyDropDown runat="server" ID="fbkDirectQuestion" SessionKey="DirectQuestionOperations" DBColumn="DirectQuestionOperations">
            <asp:ListItem Text="--" Value="" ></asp:ListItem>
        </sc:SurveyDropDown>
    </div>
    <% } %>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="<%$Resources:Lang, SFeedbackNextButton%>" />
    </div>
    <% }
        //===========================================================================
        //PAGE 2 - Basic Form
        //===========================================================================
        else if ( Master.CurrentPage == 2 ) { %>
    <% if ( AlignedPropertyShortCode == GCCPropertyShortCode.GAG ) { %>
    <p class="question">
        <%=Lang.Page2_Question1%>
    </p>
    <sc:MessageManager runat="server" ID="mmGAG"></sc:MessageManager>
    <p>
        <sc:SurveyRadioButton ID="radGAG_Everett" runat="server"  /><br />
        <sc:SurveyRadioButton ID="radGAG_Lakewood" runat="server"  /><br />
        <sc:SurveyRadioButton ID="radGAG_Tukwila" runat="server"  /><br />
        <sc:SurveyRadioButton ID="radGAG_DeMoines" runat="server"  /><br />
    </p>
    <% } %>
    <p class="question"><%=Lang.Page2_Question2 %></p>
    <sc:SurveyDropDown runat="server" ID="fbkQ1" SessionKey="fbkQ1" DBColumn="Q1">
        <asp:ListItem Text="--" Value="" ></asp:ListItem>
    </sc:SurveyDropDown>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="<%$Resources:Lang, SFeedbackPreviousButton %>" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="<%$Resources:Lang, SFeedbackContinueButton %>" />
    </div>
    <% }
        //===========================================================================
        //PAGE 3 - Final Confirmation
        //===========================================================================
        else if ( Master.CurrentPage == 3 ) { %>
    <p class="question"><%=Lang.Page3_Question1 %></p>
    <sc:SurveyDropDown runat="server" ID="fbkQ2" SessionKey="fbkQ2" DBColumn="Q2">
        <asp:ListItem Text="--" Value="" ></asp:ListItem>
    </sc:SurveyDropDown>

    <p class="question"><%=Lang.Page3_Question2 %></p>
    <sc:SurveyTextBox ID="fbkQ3" runat="server" SessionKey="fbkQ3" DBColumn="Q3" TextMode="MultiLine" Rows="10" style="width:90%" MaxLength="3000"></sc:SurveyTextBox>

    <%--<p class="question">4. Do you have an Encore Rewards card? Please provide your Encore card number so that we may better serve you.<br />(Optional)</p>--%>
  <%--  <p class="question"><%=Lang.Page3_Question3_Part1 %> <%= PropertyTools.GetPlayersClubName(PropertyID) %> <%=Lang.Page3_Question3_Part2 %> <%= PropertyTools.GetPlayersClubName(PropertyID) %> <%=Lang.Page3_Question3_Part3 %><br /><%=Lang.SFeedbackOptional %></p>--%>
     <% if (Session["CurrentUI"] == "fr-CA") { %>
     <p class="question"><%=Lang.Page3_Question3_Part1 %> <%=Lang.Page3_Question3_Part2 %> <%=Lang.Page3_Question3_Part3 %><br /><%=Lang.SFeedbackOptional %></p>

    <%} else { %>
     <p class="question"><%=Lang.Page3_Question3_Part1 %> <%= PropertyTools.GetPlayersClubName(PropertyID) %> <%=Lang.Page3_Question3_Part2 %> <%=Lang.Page3_Question3_Part3 %><br /><%=Lang.SFeedbackOptional %></p>

    <%} %>
    <sc:SurveyTextBox ID="fbkQ4" runat="server" SessionKey="fbkQ4" DBColumn="Q4" MaxLength="20"></sc:SurveyTextBox>

    <p class="question"><%=Lang.Page3_Question4 %></p>
    <sc:SurveyDropDown runat="server" ID="fbkQ5" SessionKey="fbkQ5" DBColumn="Q5" >
        <asp:ListItem Text="--" Value="" ></asp:ListItem>
    </sc:SurveyDropDown>

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="<%$Resources:Lang , SFeedbackPreviousButton %>" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="<%$Resources:Lang , SFeedbackContinueButton %>" />
    </div>
    <% }
        //===========================================================================
        //PAGE 4 - Contact Details
        //===========================================================================
        else if ( Master.CurrentPage == 4 ) { %>
    <p class="question"><%=Lang.Page4_Question1 %></p>
    <div class="row grid-row">
        <div class="col-md-4"><%=Lang.Page4_Name %></div>
        <div class="col-md-6"><sc:SurveyTextBox ID="txtName" SessionKey="txtName" DBColumn="Name" runat="server"></sc:SurveyTextBox></div>
    </div>
    <div class="row grid-row">
        <div class="col-md-4"><%=Lang.Page4_Telephone %></div>
        <div class="col-md-6"><sc:SurveyTextBox ID="txtTelephoneNumber" SessionKey="txtTelephoneNumber" DBColumn="TelephoneNumber" runat="server"></sc:SurveyTextBox></div>
    </div>
    <div class="row grid-row">
        <div class="col-md-4"><%=Lang.Page4_Email %></div>
        <div class="col-md-6"><sc:SurveyTextBox ID="txtEmailContact" SessionKey="txtEmailContact" DBColumn="ContactEmail" runat="server"></sc:SurveyTextBox></div>
    </div>
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="<%$Resources:Lang , SFeedbackPreviousButton %>" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="<%$Resources:Lang , SFeedbackDoneButton %>" />
    </div>
    <% }
        //===========================================================================
        //PAGE 5 - Final Confirmation
        //===========================================================================
        else if ( Master.CurrentPage == 5 ) { %>
    <% if ( AlignedPropertyShortCode == GCCPropertyShortCode.GAG ) { %>

    <p><%=Lang.Page5_Response1%> <%= PropertyTools.GetPhoneNumber( Master.PropertyShortCode, radGAG_DeMoines.Checked ? 4 : radGAG_Tukwila.Checked ? 3 : radGAG_Lakewood.Checked ? 2 : radGAG_Everett.Checked ? 1 : 0 ) %></p>
    <p><%=Lang.Page5_Response1_2%></p>
    <p><%=Lang.Page5_Response1_3%></p>

    <% } else if ( AlignedPropertyShortCode == GCCPropertyShortCode.CNSH && Master.PropertyShortCode == GCCPropertyShortCode.CNSS ) { %>
    
    <p><%=Lang.Page5_Response2%> <%= PropertyTools.GetPhoneNumber( AlignedPropertyShortCode, -1 ) %></p>
    <p><%=Lang.Page5_Response2_2%></p>
    <p><%=Lang.Page5_Response2_3%></p>
     <% } else if (AlignedPropertyShortCode == GCCPropertyShortCode.WDB || AlignedPropertyShortCode == GCCPropertyShortCode.AJA || AlignedPropertyShortCode == GCCPropertyShortCode.GBH || AlignedPropertyShortCode == GCCPropertyShortCode.SCTI || AlignedPropertyShortCode == GCCPropertyShortCode.SCBE || AlignedPropertyShortCode == GCCPropertyShortCode.SSKD || AlignedPropertyShortCode == GCCPropertyShortCode.ECB || AlignedPropertyShortCode == GCCPropertyShortCode.ECF || AlignedPropertyShortCode == GCCPropertyShortCode.ECGR || AlignedPropertyShortCode == GCCPropertyShortCode.ECM) { %>
    
    <p><%=Lang.Page5_Response2%> <%= PropertyTools.GetPhoneNumber( AlignedPropertyShortCode, -1 ) %></p>
    <p><%=Lang.Page5_Response2_2_Ontario%></p>
    <p><%=Lang.Page5_Response2_3%></p>

    <% } else { %>

    <p><%=Lang.Page5_Response3%> <%= PropertyTools.GetPhoneNumber( Master.PropertyShortCode, -1 ) %></p>
    <p><%=Lang.Page5_Response3_2%></p>
    <p><%=Lang.Page5_Response3_3%></p>

    <% } %>

    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="<%$Resources:Lang , SFeedbackPreviousButton %>" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="<%$Resources:Lang , SFeedbackDoneButton %>" />
    </div>
    <% }
        //===========================================================================
        //PAGE 99 - Thank You
        //===========================================================================
        else if ( Master.CurrentPage == 99 ) { %>
    <sc:MessageManager runat="server" ID="mmLastPage"></sc:MessageManager>
    <% if (!String.IsNullOrEmpty( mmLastPage.SuccessMessage)) { %>
    <div class="button-container">
        <% if ( !IsStaffSurvey ) { %>
        <a href="<%= PropertyTools.GetCasinoURL( Master.PropertyShortCode ) %>" class="btn btn-success"><%=Lang.SFeedbackReturnButton %></a>
        <% } else { %>
        <% if ( !String.IsNullOrWhiteSpace( FeedbackUID ) ) { %>
        <a href="/Admin/Feedback/List" class="btn btn-success"><%=Lang.SFeedbackGoToListButton %></a>
        <a href="/Admin/Feedback/<%= FeedbackUID %>" class="btn btn-primary"><%=Lang.SFeedbackGoToItemButton %></a>
        <% } else { %>
        <a href="/Admin/Surveys" class="btn btn-success"><%=Lang.SFeedbackGoToSurveyButton %></a>
        <% } %>
        <% } %>
    </div>
    <% } %>
    <% } %>
    <script>
        $(function () {
            if ($('#<%= fbkProperty.ClientID %>').val() != 1) {
                $("#direct-question-container").hide();
            }
            $('#<%= fbkProperty.ClientID %>').on('change', function (evt) {
                if ($(this).val() == 1) { // 1 is "No Specific Property"
                    $("#direct-question-container").slideDown();
                } else {
                    $("#direct-question-container").slideUp();
                    $("#direct-question-container").val("");
                }
            });
        });
    </script>
</asp:Content>