<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuestionRowControl.ascx.cs" Inherits="GCC_Web_Portal.Controls.QuestionRowControl" %>
<div class="question-row">
    <sc:MessageManager runat="server" ID="mmMessage"></sc:MessageManager>
    <div class="row grid-row">
        <% int colSpan = 2;
        if ( HasLabel ) {
            colSpan = 1; %>
        <div class="col-md-6"><asp:Label ID="lblRowLabel" runat="server"></asp:Label></div>
        <% } %>
        <div class="col-md-<%= colSpan %> col-xs-2 option"><asp:RadioButton ID="radAnswer5" runat="server" GroupName="QRC" Text="" /></div>
        <div class="col-md-<%= colSpan %> col-xs-2 option"><asp:RadioButton ID="radAnswer4" runat="server" GroupName="QRC" Text="" /></div>
        <div class="col-md-<%= colSpan %> col-xs-2 option"><asp:RadioButton ID="radAnswer3" runat="server" GroupName="QRC" Text="" /></div>
        <div class="col-md-<%= colSpan %> col-xs-2 option"><asp:RadioButton ID="radAnswer2" runat="server" GroupName="QRC" Text="" /></div>
        <div class="col-md-<%= colSpan %> col-xs-2 option"><asp:RadioButton ID="radAnswer1" runat="server" GroupName="QRC" Text="" /></div>
        <% if (ShowNAColumn) { %>
        <div class="col-md-<%= colSpan %> col-xs-2 option"><asp:RadioButton ID="radAnswer0" runat="server" GroupName="QRC" Text="" /></div>
        <% } %>
    </div>
</div>