<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs" Inherits="GCC_Web_Portal.Admin.UserAdd"
    AllowedGroups="ForumAdmin,CorporateMarketing" %>

<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Create User</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li><a href="/Admin/Users">User List</a></li>
        <li class="active">Create User</li>
    </ol>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <% if ( TopMessage.IsVisible ) { %>
    <div class="row">
        <div class="col-md-6">
            <sc:MessageManager runat="server" ID="TopMessage"></sc:MessageManager>
        </div>
    </div>
    <% } %>
    <div class="row">
        <div class="col-xs-12 col-md-6">
            <div class="box box-success">
                <div class="box-header with-border">
                    <i class="fa fa-user"></i>
                    <h3 class="box-title">User Details</h3>
                </div>
                <div class="box-body border-radius-none">
                    <div class="form-group">
                        <label for="<%= txtFirstName.ClientID %>">First Name</label>
                        <asp:TextBox runat="server" ID="txtFirstName" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="<%= txtLastName.ClientID %>">Last Name</label>
                        <asp:TextBox runat="server" ID="txtLastName" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="<%= txtEmail.ClientID %>">Email</label>
                        <asp:TextBox TextMode="Email" runat="server" ID="txtEmail" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="<%= txtPassword.ClientID %>">Temporary Password</label>
                        <asp:TextBox TextMode="Password" runat="server" ID="txtPassword" CssClass="form-control"></asp:TextBox>
                        <div class="help-block">The user will be required to change their password on first log in.</div>
                    </div>
                    <div class="form-group">
                        <label for="<%= txtPassword2.ClientID %>">Confirm Password</label>
                        <asp:TextBox TextMode="Password" runat="server" ID="txtPassword2" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="<%= ddlProperty.ClientID %>">Property</label>
                        <asp:DropDownList runat="server" ID="ddlProperty" CssClass="form-control">
                            <asp:ListItem Text="None" Value=""></asp:ListItem>
                            <asp:ListItem Text="River Rock Casino Resort" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Hard Rock Casino Vancouver" Value="3"></asp:ListItem>
                            <%--<asp:ListItem Text="Fraser Downs Racetrack & Casino" Value="4"></asp:ListItem>--%>
                            <asp:ListItem Text="Elements Casino" Value="14"></asp:ListItem>
                            <asp:ListItem Text="Hastings Racetrack & Casino" Value="5"></asp:ListItem>
                            <asp:ListItem Text="View Royal Casino" Value="6"></asp:ListItem>
                            <asp:ListItem Text="Casino Nanaimo" Value="7"></asp:ListItem>
                            <asp:ListItem Text="Chances Chilliwack" Value="8"></asp:ListItem>
                            <asp:ListItem Text="Chances Maple Ridge" Value="9"></asp:ListItem>
                            <asp:ListItem Text="Chances Dawson Creek" Value="10"></asp:ListItem>
                            <asp:ListItem Text="Casino Nova Scotia - Halifax" Value="11"></asp:ListItem>
                            <asp:ListItem Text="Casino Nova Scotia - Sydney" Value="12"></asp:ListItem>
                            <asp:ListItem Text="Great American Casino" Value="13"></asp:ListItem>

                            <asp:ListItem Text="Shorelines Slots at Kawartha Downs" Value="17"></asp:ListItem>
                            <asp:ListItem Text="Shorelines Casino Thousand Islands" Value="18"></asp:ListItem>
                            <asp:ListItem Text="Casino New Brunswick" Value="19"></asp:ListItem>
                            <asp:ListItem Text="Shorelines Casino Belleville" Value="20"></asp:ListItem>
                            <asp:ListItem Text="Casino Woodbine" Value="22"></asp:ListItem>
                            <asp:ListItem Text="Casino Ajax" Value="23"></asp:ListItem>
                            <asp:ListItem Text="Great Blue Heron Casino" Value="24"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="<%= ddlGroup.ClientID %>">Group</label>
                        <asp:DropDownList runat="server" ID="ddlGroup" CssClass="form-control">
                            <asp:ListItem Value="3" Text="Corporate Management"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Property Managers"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Property Staff"></asp:ListItem>
                            <asp:ListItem Value="6" Text="HR Staff"></asp:ListItem>
                            <asp:ListItem Value="7" Text="Corporate Marketing"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label for="<%= ddlTimezone.ClientID %>">Timezone</label>
                        <asp:DropDownList runat="server" ID="ddlTimezone" CssClass="form-control">
                            <asp:ListItem Text="(GMT-08:00) Pacific Time (US & Canada)" Value="Pacific Standard Time"></asp:ListItem>
                            <asp:ListItem Text="(GMT-07:00) Mountain Time (US & Canada)" Value="Mountain Standard Time"></asp:ListItem>
                            <asp:ListItem Text="(GMT-06:00) Saskatchewan" Value="Canada Central Standard Time"></asp:ListItem>
                            <asp:ListItem Text="(GMT-06:00) Central Time (US & Canada)" Value="Central Standard Time"></asp:ListItem>
                            <asp:ListItem Text="(GMT-05:00) Eastern Time (US & Canada)" Value="Eastern Standard Time"></asp:ListItem>
                            <asp:ListItem Text="(GMT-04:00) Atlantic Time (Canada)" Value="Atlantic Standard Time"></asp:ListItem>
                            <asp:ListItem Text="(GMT-03:30) Newfoundland" Value="Newfoundland Standard Time"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Button runat="server" ID="btnSave" Text="Create" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="FooterScripts" runat="server"></asp:Content>
