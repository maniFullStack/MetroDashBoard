<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScaleQuestionControlFrench.ascx.cs" Inherits="GCC_Web_Portal.Controls.ScaleQuestionControlFrench" %>
<div class="scale-question">
    <sc:MessageManager runat="server" ID="mmMessage_F"></sc:MessageManager>
    <p>
        <sc:SurveyRadioButton ID="radAnswer5_F" runat="server" GroupName="SQC" Text="&nbsp;Extrêmement satisfait " /><br />
        <sc:SurveyRadioButton ID="radAnswer4_F" runat="server" GroupName="SQC" Text="&nbsp;Très satisfait" /><br />
        <sc:SurveyRadioButton ID="radAnswer3_F" runat="server" GroupName="SQC" Text="&nbsp;Satisfait" /><br />
        <sc:SurveyRadioButton ID="radAnswer2_F" runat="server" GroupName="SQC" Text="&nbsp;Insatisfait" /><br />
        <sc:SurveyRadioButton ID="radAnswer1_F" runat="server" GroupName="SQC" Text="&nbsp;Très insatisfait" />
    </p>
</div>