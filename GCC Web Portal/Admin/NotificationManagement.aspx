<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="NotificationManagement.aspx.cs" Inherits="GCC_Web_Portal.Admin.NotificationManagement"
    AllowedGroups="ForumAdmin,CorporateMarketing" %>

<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .error {
            display: block;
            margin-top: 20px;
            color: crimson;
        }

        .results > tbody > tr > td:last-child {
            text-align: left;
        }

        .results ul {
            list-style-type: none;
            padding-left: 0;
        }

            .results ul li.send-type-title {
                font-weight: bold;
                font-size: large;
            }

            .results ul li {
                padding-left: 20px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Notification Management</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Notification Management</li>
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
        <div class="col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-filter"></i>
                    <h3 class="box-title">Filters</h3>
                </div>
                <div class="box-body border-radius-none">
                    <p>This page allows you to manage the users that will get notifications for a particular survey response. You can filter it by Property, Survey and Reason by selecting the filters.</p>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label>Property</label>
                                <asp:DropDownList runat="server" ID="ddlProperty" AutoPostBack="true" OnSelectedIndexChanged="PropertySurveyFilter_SelectedIndexChanged" CssClass="form-control">
                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                    <asp:ListItem Text="GCGC" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="River Rock Casino Resort" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Hard Rock Casino Vancouver" Value="3"></asp:ListItem>
                                    <%-- <asp:ListItem Text="Fraser Downs Racetrack & Casino" Value="4"></asp:ListItem>--%>
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
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label>Survey</label>
                                <asp:DropDownList runat="server" ID="ddlSurvey" AutoPostBack="true" OnSelectedIndexChanged="PropertySurveyFilter_SelectedIndexChanged" CssClass="form-control">
                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                    <asp:ListItem Text="GEI" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="GEI – Problem Resolution" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Hotel" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Feedback" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Donation / Sponsorship" Value="4"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <% if ( lblReasonError.Text.Length > 0 ) { %>
                            <asp:Label runat="server" ID="lblReasonError" CssClass="error"></asp:Label>
                            <% } %>
                            <% if ( ddlReason.Visible ) { %>
                            <div class="form-group">
                                <label>Reason&nbsp;/&nbsp;Area</label>
                                <asp:DropDownList runat="server" ID="ddlReason" AutoPostBack="true" CssClass="form-control">
                                    <asp:ListItem Text="All" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <% } %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-exclamation-circle"></i>
                    <h3 class="box-title">Notifications</h3>
                </div>
                <div class="box-body border-radius-none">
                    <% if ( Data != null ) { %>
                    <table id="results" class="table table-striped table-bordered results">
                        <thead>
                            <tr>
                                <th>Property</th>
                                <th>Survey</th>
                                <th>Reason / Area</th>
                                <th>Users Notified</th>
                            </tr>
                        </thead>
                        <tbody>
                            <%
                        int lastID = -1;
                        string lastSendType = String.Empty;
                        for ( int i = 0; i < Data.Rows.Count; i++ ) {
                            DataRow dr = Data.Rows[i];
                            %>
                            <tr>
                                <th><%= ReportingTools.CleanData( dr["PropertyName"] ) %></th>
                                <td><%= ReportingTools.CleanData( dr["SurveyName"] ) %></td>
                                <td><%= ReportingTools.CleanData( dr["ReasonDescription"] ) %></td>
                                <td>
                                    <a href="#" class="btn btn-success pull-right ad" data-p="<%= dr["PropertySurveyReasonID"] %>" title="Add user to the <%= dr["SurveyName"] %> - <%= ReportingTools.CleanData( dr["ReasonDescription"] ) %> for <%= ReportingTools.CleanData( dr["PropertyName"] ) %>">Add <i class="fa fa-plus-circle"></i></a>
                                    <%
                                bool first = true;
                                int curID = (int)dr["PropertySurveyReasonID"];
                                while ( i < Data.Rows.Count 
                                        && (dr["PropertySurveyReasonID"].Equals(curID) || lastID == -1)
                                        && !dr["UserID"].Equals( DBNull.Value ) ) {
                                    if ( first || !lastSendType.Equals( dr["PropertySurveyReasonID"].ToString() + "-" + dr["SendType"].ToString() ) ) { %>
                                    <%= !first ? "</ul>" : String.Empty %>
                                    <ul>
                                        <li class="send-type-title"><%= dr["SendType"].Equals(3) ? "BCC" :
                                                                    dr["SendType"].Equals(2) ? "CC" : "To" %>:</li>
                                        <% } %>
                                        <li><%= ReportingTools.CleanData( dr["FirstName"] ) %> <%= ReportingTools.CleanData( dr["LastName"] ) %> <a href="#" class="btn btn-danger btn-xs rm" data-p="<%= dr["PropertySurveyReasonID"] %>" data-u="<%= dr["UserID"] %>" data-s="<%= dr["SendType"] %>" title="Remove <%= ReportingTools.CleanData( dr["FirstName"] ) %> from this notification"><i class="fa fa-minus-circle"></i></a></li>
                                        <%
                                    lastID = (int)dr["PropertySurveyReasonID"];
                                    lastSendType = dr["PropertySurveyReasonID"].ToString() + "-" + dr["SendType"].ToString();
                                    i++;
                                    first = false;
                                    if ( i < Data.Rows.Count ) {
                                        dr = Data.Rows[i];
                                    } else {
                                        break;
                                    }
                                }
                                if ( !first ) {
                                    i--; //This adjusts for when we went an extra record forward with the while loop's i++
                                        %>
                                    </ul>
                                    <% } %>
                                </td>
                            </tr>
                            <%
                            lastID = (int)dr["PropertySurveyReasonID"];
                            lastSendType = dr["PropertySurveyReasonID"].ToString() + "-" + dr["SendType"].ToString();
                        } %>
                        </tbody>
                    </table>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterScripts" runat="server">
    <div class="modal fade" id="add-modal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">Add User To Notification</h4>
                </div>
                <div class="modal-body">
                    <table id="modal-results" class="table table-striped table-bordered results">
                        <thead>
                            <tr>
                                <th>Property</th>
                                <th>Survey</th>
                                <th>Reason / Area</th>
                                <th>User</th>
                                <th>Send Type</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="keep">
                                    <sc:DynamicDropDownList runat="server" ID="ddlUserSearch" CssClass="form-control" EnableViewState="false"></sc:DynamicDropDownList>
                                </td>
                                <td class="keep">
                                    <asp:DropDownList runat="server" ID="ddlSendType" CssClass="form-control">
                                        <asp:ListItem Text="To" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="CC" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="BCC" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <asp:HiddenField runat="server" ID="hdnPSRID" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <asp:Button runat="server" ID="btnAddUser" OnClick="btnAddUser_Click" CssClass="btn btn-primary" Text="Add User" />
                </div>
            </div>
        </div>
    </div>
    <script>
        $('#results').on('click', 'a.ad', function (evt) {
            var $btn = $(this),
                $tds = $btn.parents('tr').find('*:lt(3)'),
                $mtr = $('#modal-results > tbody > tr');
            $mtr.find('> :not(.keep)').remove();
            $mtr.prepend($tds.clone());
            $('#<%= hdnPSRID.ClientID %>').val($btn.data('p'));

        $.getJSON(
            "/Admin/NotificationManagement/?a=1&t=2",
            { p: $btn.data('p') },
            function (data) {
                if (data.s === 0) {
                    // constructs the suggestion engine
                    var $ddl = $('#<%= ddlUserSearch.ClientID %>');
                    $ddl.empty();
                    $.each(data.ubh, function (i, u) {
                        $ddl.append('<option value="' + u.i + '">' + u.n + '</option>');
                    });
                    $('#add-modal').modal('show');
                } else {
                    alert("We were unable to load the user list. Please refresh the page and try again.");
                }
            });
        evt.preventDefault();
    });
        $('#results').on("click", "a.rm", function (evt) {
            if (confirm("Are you sure you want to remove this user?")) {
                var $li = $(this).parents('li');
                $.post("/Admin/NotificationManagement/?a=1&t=1",
                        {
                            p: $(this).data('p'),
                            u: $(this).data('u'),
                            s: $(this).data('s')
                        },
                        function (data) {
                            if (data.s === 0) {
                                var $ul = $li.parent();
                                if ($ul.find('li:lt(3)').length == 2) {
                                    $ul.remove();
                                } else {
                                    $li.remove();
                                }
                            } else {
                                alert('Unable to remove the user. ' + data.msg);
                                if (data.s === 4) {
                                    var $ul = $li.parent();
                                    if ($ul.find('li:lt(3)').length == 2) {
                                        $ul.remove();
                                    } else {
                                        $li.remove();
                                    }
                                }
                            }
                        },
                        'json');
            }
            evt.preventDefault();
        });
    </script>
</asp:Content>
