<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordChange.aspx.cs" Inherits="GCC_Web_Portal.PasswordChange" %>
<%@ Import Namespace="WebsiteUtilities" %>
<!DOCTYPE html>
<html lang="en">
  <head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>GCC Password Update</title>

    <asp:PlaceHolder runat="server">     
          <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:BundleReference runat="server" Path="~/Content/bootstrap" />
    <link href="~/Content/login.css" rel="stylesheet">
    
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
  </head>
<body>
    <div class="site-wrapper">
        <div class="site-wrapper-inner">
            <div class="cover-container">
                <form id="form" runat="server" class="inner cover form-horizontal" role="form" method="post">
                    <h1 class="cover-heading">Password Change</h1>

                    <p class="lead">Please enter a new password.</p>
                    <% if ( !HideForm ) { %>
                    <div class="alert alert-info">Passwords must be between 6 and 20 characters with at least one upper case letter, one lower case letter and one number.</div>
                    <% } %>
                    <sc:MessageManager id="mmMessages" runat="server"></sc:MessageManager>
                    <% if ( !HideForm ) { %>
                    <div class="form-group">
                        <label class="col-sm-offset-1 col-sm-4 control-label" for="inputUsername">Email</label>
                        <div class="col-sm-4" style="line-height:30px">
                            <%= ReportingTools.CleanData( UserData.Email ) %>
                        </div>
                    </div>
					<% if (ShowOldPassword) { %>
					<div class="form-group">
						<label class="col-sm-offset-1 col-sm-4 control-label" for="txtOldPwd">Old Password</label>
						<div class="col-sm-4">
							<input type="password" ID="txtOldPwd" class="form-control" name="pwd" maxlength="50" value="" />
						</div>
					</div>
					<% } %>
					<div class="form-group">
						<label class="col-sm-offset-1 col-sm-4 control-label" for="txtNewPass1">New Password</label>
						<div class="col-sm-4">
							<input type="password" class="form-control" id="txtNewPass1" name="newpwd1" maxlength="50" value="" />
						</div>
					</div>
					<div class="form-group">
						<label class="col-sm-offset-1 col-sm-4 control-label" for="txtNewPass2">Confirm Password</label>
						<div class="col-sm-4">
							<input type="password" class="form-control" id="txtNewPass2" name="newpwd2" maxlength="50" value="" />
						</div>
					</div>
                    <div class="form-group text-center">
						<input type="hidden" name="login" value="true" />
						<input class="btn btn-lg btn-info" type="submit" value="Save" />
						<a href="/?d=1" class="btn btn-lg btn-default">Cancel</a>
						<% if (ShowOldPassword) { %>
                        <br /><br />
						<a href="/ResetPassword?email=<%= UserData.Email %>">Forgot Old Password?</a>
					    <% } %>
					    
					</div>
                    <% } %>
                </form>
            </div>
        </div>
    </div>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/jquery") %>
        <%: Scripts.Render("~/bundles/bootstrap") %>
    </asp:PlaceHolder>
    <script>
        /*!
         * IE10 viewport hack for Surface/desktop Windows 8 bug
         * Copyright 2014 Twitter, Inc.
         * Licensed under the Creative Commons Attribution 3.0 Unported License. For
         * details, see http://creativecommons.org/licenses/by/3.0/.
         */
        // See the Getting Started docs for more information:
        // http://getbootstrap.com/getting-started/#support-ie10-width

        (function () {
            'use strict';
            if (navigator.userAgent.match(/IEMobile\/10\.0/)) {
                var msViewportStyle = document.createElement('style')
                msViewportStyle.appendChild(
                    document.createTextNode(
                    '@-ms-viewport{width:auto!important}'
                    )
                )
                document.querySelector('head').appendChild(msViewportStyle)
            }
        })();
    </script>
</body>
</html>