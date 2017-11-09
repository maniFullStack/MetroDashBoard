<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="GCC_Web_Portal.Login" %>
<%@ Import Namespace="WebsiteUtilities" %>
<!DOCTYPE html>
<html lang="en">
  <head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>GCC Login</title>

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
                <form id="frmLogin" runat="server" class="inner cover form-horizontal" role="form">
                    <h1 class="cover-heading">GCC Login</h1>
                    <p class="lead">This website is <strong>confidential</strong>. If you came here in error or do not belong here, please <a href="http://www.google.ca/">leave</a> immediately. Otherwise...<br /><br />Please enter your login credentials below.</p>
                    <sc:MessageManager id="mmMessages" runat="server"></sc:MessageManager>
                    <div class="form-group">
                        <label class="col-sm-offset-1 col-sm-4 control-label" for="inputUsername">Email</label>
                        <div class="col-sm-4">
                            <input type="text" class="form-control" id="inputUsername" name="inputUsername" value="<%= RequestVars.PostEncoded("inputUsername", String.Empty) %>" placeholder="Email" autocomplete="off" aria-autocomplete="none">
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-offset-1 col-sm-4 control-label" for="inputPassword">Password</label>
                        <div class="col-sm-4">
                            <input type="password" class="form-control" id="inputPassword" name="inputPassword" placeholder="Password" autocomplete="off" aria-autocomplete="none">
                        </div>
                    </div>
                    <div class="form-group text-center">
                        <input type="hidden" name="inputEmail" value="1" />
                        <input id="btnLogin" type="submit" class="btn btn-lg btn-info" value="Sign In">
                        <a href="/ResetPassword" class="btn btn-lg btn-default">Reset Password</a>
                    </div>
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