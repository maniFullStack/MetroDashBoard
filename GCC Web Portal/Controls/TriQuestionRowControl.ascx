<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TriQuestionRowControl.ascx.cs" Inherits="GCC_Web_Portal.Controls.TriQuestionRowControl" %>
<div class="question-row">
    <sc:MessageManager runat="server" ID="mmMessage"></sc:MessageManager>
    <div class="row grid-row">
        <% if ( HasLabel ) { %>
        <div class="col-md-4"><asp:Label ID="lblRowLabel" runat="server"></asp:Label></div>
        <% } %>
        <div class="col-md-2 col-xs-3 option"><asp:RadioButton ID="radAnswer3" runat="server" GroupName="QRC" Text="" /></div>
        <div class="col-md-2 col-xs-3 option"><asp:RadioButton ID="radAnswer2" runat="server" GroupName="QRC" Text="" /></div>
        <div class="col-md-2 col-xs-3 option"><asp:RadioButton ID="radAnswer1" runat="server" GroupName="QRC" Text="" /></div>
        <% if (ShowNAColumn) { %>
        <div class="col-md-2 col-xs-3 option"><asp:RadioButton ID="radAnswer0" runat="server" GroupName="QRC" Text="" /></div>
        <% } %>
    </div>
</div>