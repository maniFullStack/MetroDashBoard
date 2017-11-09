<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScaleQuestionControl.ascx.cs" Inherits="GCC_Web_Portal.Controls.ScaleQuestionControl" %>
<div class="scale-question">
    <sc:MessageManager runat="server" ID="mmMessage"></sc:MessageManager>
    <p>
        <sc:SurveyRadioButton ID="radAnswer5" runat="server" GroupName="SQC" Text="&nbsp;Extremely Satisfied" /><br />
        <sc:SurveyRadioButton ID="radAnswer4" runat="server" GroupName="SQC" Text="&nbsp;Very Satisfied" /><br />
        <sc:SurveyRadioButton ID="radAnswer3" runat="server" GroupName="SQC" Text="&nbsp;Satisfied" /><br />
        <sc:SurveyRadioButton ID="radAnswer2" runat="server" GroupName="SQC" Text="&nbsp;Dissatisfied" /><br />
        <sc:SurveyRadioButton ID="radAnswer1" runat="server" GroupName="SQC" Text="&nbsp;Very Dissatisfied" />
    </p>
</div>