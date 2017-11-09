<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TenScaleQuestionControl.ascx.cs" Inherits="GCC_Web_Portal.Controls.TenScaleQuestionControl" %>
<div class="scale-question">
    <sc:MessageManager runat="server" ID="mmMessage"></sc:MessageManager>
    <p>
        <% if ( !HideZero ) { %><sc:SurveyRadioButton ID="OLG1" runat="server" GroupName="OLG" Text="" /><% } %>
        <sc:SurveyRadioButton ID="OLG2" runat="server" GroupName="OLG" Text="" />
        <sc:SurveyRadioButton ID="OLG3" runat="server" GroupName="OLG" Text="" />
        <sc:SurveyRadioButton ID="OLG4" runat="server" GroupName="OLG" Text="" />
        <sc:SurveyRadioButton ID="OLG5" runat="server" GroupName="OLG" Text="" />
        <sc:SurveyRadioButton ID="OLG6" runat="server" GroupName="OLG" Text="" />
        <sc:SurveyRadioButton ID="OLG7" runat="server" GroupName="OLG" Text="" />
        <sc:SurveyRadioButton ID="OLG8" runat="server" GroupName="OLG" Text="" />
        <sc:SurveyRadioButton ID="OLG9" runat="server" GroupName="OLG" Text="" />
        <sc:SurveyRadioButton ID="OLG10" runat="server" GroupName="OLG" Text="" />
        <sc:SurveyRadioButton ID="OLG11" runat="server" GroupName="OLG" Text="" />
    </p>
</div>