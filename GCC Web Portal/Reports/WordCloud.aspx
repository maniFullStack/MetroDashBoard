<%@ Page Title="Home" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="WordCloud.aspx.cs" Inherits="GCC_Web_Portal.Reports.WordCloud"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,HRStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Import Namespace="TagCloud" %>
<%@ Import Namespace="System.Collections.Generic" %>
<asp:Content runat="server" ContentPlaceHolderID="head">
    <style>
        /* TagCloud
----------------------------------------------------------*/

.TagCloud			/* Applies to the entire tag cloud */
{
	font-family:Trebuchet MS;
	border:1px solid #888;
	padding:3px; 
	text-align:center;
}

.TagCloud > span	/* Applies to each tag of the tag cloud */
{
	margin-right:3px;
	text-align:center;
}

.TagCloud > span.TagWeight1	/* Applies to the largest tags */
{
	font-size:3em;
}

.TagCloud > span.TagWeight2
{
	font-size:2.5em;
}

.TagCloud > span.TagWeight3
{
	font-size:2em;
}

.TagCloud > span.TagWeight4
{
	font-size:1.5em;
}

.TagCloud > span.TagWeight5	/* Applies to the smallest tags */
{
	font-size:1em;
}

    </style>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="MainContentHeader">
    <h1><i class="fa fa-cloud"></i> Word Cloud Report</h1>
    <ol class="breadcrumb">
        <li><a href="/">Home</a></li>
        <li class="active">Word Cloud</li>
    </ol>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
<% if ( Data == null ) { %>
<div class="row">
    <div class="col-md-6 col-md-offset-3">
        <div class="box box-danger box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">Error</h3>
            </div>
            <div class="box-body">
                  Unable to load the data. Please try again.
            </div>
        </div>
    </div>
</div>
<% } else { %>
    <div class="row">
        <div class="col-md-12 col-lg-6">
            <div class="box box-success">
                <div class="box-body border-radius-none">
                    <% if ( Data.Count == 0 ) { %>
                    <p>No results found for these filters.</p>
                    <% } else { %>
                    <%= new TagCloud( Data,
					        new TagCloudGenerationRules
					        {
                                Order = TagCloudOrder.Random,
						        TagToolTipFormatString = "Word Count: {0}",
						        TagUrlFormatString = IsHRUser ? String.Empty : "/Admin/Surveys/?sf=1&fltTextSearch={0}"
					        }
                        ) %>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
<% } %>
</asp:Content>