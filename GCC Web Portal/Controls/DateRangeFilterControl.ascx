<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateRangeFilterControl.ascx.cs" Inherits="GCC_Web_Portal.Controls.DateRangeFilterControl" %>
<sc:MessageManager ID="mmMessage" runat="server"></sc:MessageManager>
<button class="btn btn-default drpicker" type="button"><i class="fa fa-calendar"></i> Select Range<i class="fa fa-caret-down"></i></button>
<asp:HiddenField ID="hdnBegin" runat="server" />
<asp:HiddenField ID="hdnEnd" runat="server" />