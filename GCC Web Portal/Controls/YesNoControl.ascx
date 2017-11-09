<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="YesNoControl.ascx.cs" Inherits="GCC_Web_Portal.Controls.YesNoControl" %>
<sc:MessageManager runat="server" ID="mmMessage"></sc:MessageManager>
<asp:RadioButton ID="radYes" runat="server" GroupName="YN" Text="&nbsp;Yes" /><br />
<asp:RadioButton ID="radNo" runat="server" GroupName="YN" Text="&nbsp;No" />