<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SixScaleQuestionControl.ascx.cs" Inherits="GCC_Web_Portal.Controls.SixScaleQuestionControl" %>
<div class="scale-question">
    <sc:MessageManager runat="server" ID="mmMessage"></sc:MessageManager>
    <p>
        <sc:SurveyRadioButton ID="radAnswer6" runat="server" GroupName="SQC" Text="&nbsp;Strongly Disagree" /><br />
        <sc:SurveyRadioButton ID="radAnswer5" runat="server" GroupName="SQC" Text="&nbsp;Disagree" /><br />
        <sc:SurveyRadioButton ID="radAnswer4" runat="server" GroupName="SQC" Text="&nbsp;Slightly Disagree" /><br />
        <sc:SurveyRadioButton ID="radAnswer3" runat="server" GroupName="SQC" Text="&nbsp;Slightly Agree" /><br />
        <sc:SurveyRadioButton ID="radAnswer2" runat="server" GroupName="SQC" Text="&nbsp;Agree" /><br />
        <sc:SurveyRadioButton ID="radAnswer1" runat="server" GroupName="SQC" Text="&nbsp;Strongly Agree" />
    </p>
</div>