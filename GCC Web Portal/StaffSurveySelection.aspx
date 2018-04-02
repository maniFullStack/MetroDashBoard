<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="StaffSurveySelection.aspx.cs" Inherits="GCC_Web_Portal.StaffSurveySelection"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,PropertyStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .wait-message {
            display:none;
            margin-bottom:15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1><i class="fa fa-pencil-square-o"></i> Manual Survey Entry</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Manual Survey Entry</li>
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
    <div class="col-xs-12 col-sm-6 col-md-4">
        <div class="box box-info">
            <div class="box-body border-radius-none">
                <p>Enter survey details for which property:</p>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-group">
                            <label>Property</label>
                            <% if ( Master.IsPropertyUser ) { %>

                              <span class="form-control"><%= PropertyTools.GetCasinoName( (int)User.PropertyShortCode ) %></span>
                            <% } 
                                else { %>
                            <asp:DropDownList runat="server" ID="ddlProperty" CssClass="form-control" OnSelectedIndexChanged="ddlProperty_SelectedIndexChanged" AutoPostBack="true">
                                
                                <asp:ListItem Text="Casino Nanaimo" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Casino New Brunswick" Value="19"></asp:ListItem>
                                 <asp:ListItem Text="Casino Nova Scotia - Halifax" Value="11"></asp:ListItem>
                                <asp:ListItem Text="Casino Nova Scotia - Sydney" Value="12"></asp:ListItem>
                                <asp:ListItem Text="Chances Chilliwack" Value="8"></asp:ListItem>
                                <asp:ListItem Text="Chances Maple Ridge" Value="9"></asp:ListItem>
                                <asp:ListItem Text="Chances Dawson Creek" Value="10"></asp:ListItem>
                                <asp:ListItem Text="Elements Casino" Value="14"></asp:ListItem>
                                <asp:ListItem Text="Great American Casino" Value="13"></asp:ListItem>
                              
                                  <%--<asp:ListItem Text="Fraser Downs Racetrack & Casino" Value="4"></asp:ListItem>--%>
                                <asp:ListItem Text="Hastings Racetrack & Casino" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Hard Rock Casino Vancouver" Value="3"></asp:ListItem>
                               
                                <asp:ListItem Text="River Rock Casino Resort" Value="2"></asp:ListItem>
                                
                                <asp:ListItem Text="Shorelines Casino Belleville" Value="20"></asp:ListItem>
                                   <asp:ListItem Text="Shorelines Casino Thousand Islands" Value="18"></asp:ListItem>
                              <asp:ListItem Text="Shorelines Slots at Kawartha Downs" Value="17"></asp:ListItem>
                                  <asp:ListItem Text="View Royal Casino" Value="6"></asp:ListItem>

                                 <asp:ListItem Text="Casino Woodbine" Value="22"></asp:ListItem>
                                 <asp:ListItem Text="Casino Ajax" Value="23"></asp:ListItem>
                                 <asp:ListItem Text="Great Blue Heron Casino" Value="24"></asp:ListItem>
                                <asp:ListItem Text="Elements Casino Brantford" Value="25"></asp:ListItem>
                                <asp:ListItem Text="Elements Casino Flamboro" Value="26"></asp:ListItem>
                                <asp:ListItem Text="Elements Casino Grand River" Value="27"></asp:ListItem>
                                <asp:ListItem Text="Elements Casino Mohawk" Value="28"></asp:ListItem>
                                
                             
                            </asp:DropDownList>
                            <% } %>
                            

                        </div>

                        <div>

                         <asp:DropDownList runat="server" ID ="ddlSurveyLang" CssClass ="form-control">
                                                                          
                              
                                 <asp:ListItem Text="English" Value="1"> </asp:ListItem>
                                 
                         
                         </asp:DropDownList>

                        </div>
                        
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-xs-12 text-center">
                        <asp:Button runat="server" ID="btnContinue" Text="Continue" OnClick="btnContinue_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterScripts" runat="server"></asp:Content>
