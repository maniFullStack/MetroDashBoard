<%@ Page Title="" Language="C#" MasterPageFile="~/Dashboard.Master" AutoEventWireup="true" CodeBehind="SocialMediaDashboard.aspx.cs" Inherits="GCC_Web_Portal.Reports.SocialMediaDashboard"
    AllowedGroups="ForumAdmin,CorporateManagement,PropertyManagers,HRStaff,CorporateMarketing" %>
<%@ MasterType VirtualPath="~/Dashboard.Master" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="WebsiteUtilities" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Register Src="~/Controls/DateRangeFilterControl.ascx" TagPrefix="uc1" TagName="DateRangeFilterControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContentHeader" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="SidebarListItems" runat="server"></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
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
        <div class="col-md-12">
            <div class="box box-info">
                <div class="box-header with-border">
                    <i class="fa fa-bullhorn"></i>
                    <h3 class="box-title">Social Media & Text Analytics</h3>
                </div>
                <div class="box-body border-radius-none">
                    <sc:MessageManager ID="mmMessage" runat="server"></sc:MessageManager>
                    <asp:Button Text="Reload Data" runat="server" OnClick="btnReloadData_Click" ID="btnReloadData" />
                    <a href="SocialMediaData.aspx?competitor=0">View Raw Data</a>
                </div>
            </div>
        </div>
    </div>
    <div class="row">        
        <div class="col-md-6">
            <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">Social Media Mention Trending - Property</h3>
                </div>
                <div class="box-body border-radius-none" style="">
                    <%--<canvas id="property-mentiontrending"  height="230" style="height:230px"></canvas>--%>
                    <div class="chart" id="property-mentiontrending-property" style="height:400px;"></div>
                </div>
            </div>
        </div>          
        <div class="col-md-6">
            <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">Social Media Mention - Source</h3>
                </div>
                <div class="box-body border-radius-none" style="">
                    <%--<canvas id="property-mentiontrending"  height="230" style="height:230px"></canvas>--%>
                    <div class="chart" id="property-mentiontrending-source" style="height:400px;"></div>
                </div>
            </div>
        </div>   
    </div>    
    <div class="row"> 
        <div class="col-md-6">
            <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">Social Media Mention Overall Distribution</h3>
                </div>
                <div class="box-body border-radius-none" style="">
                    <%--<canvas id="property-mentiontrending"  height="230" style="height:230px"></canvas>--%>
                    <div class="chart" id="property-mentionoverall" style="height:400px;"></div>
                </div>
            </div>
        </div>       
        <div class="col-md-6">
            <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">Social Media Sentiment Trending</h3>
                </div>
                <div class="box-body border-radius-none" style="">
                    <%--<canvas id="property-mentiontrending"  height="230" style="height:230px"></canvas>--%>
                    <div class="chart" id="property-sentimenttrending" style="height:400px;"></div>
                </div>
            </div>
        </div>  
    </div>
    <div class="row"> 
        <div class="col-md-6">
            <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">Top Keywords</h3>
                </div>
                <div class="box-body border-radius-none" style="">
<% if ( Data != null && Data.Tables.Count > 3 && Data.Tables[3] != null && Data.Tables[3].Rows.Count > 0 ) { %>
                    <table class="table">
                        <thead>
                            <tr>
                                <th>Keyword</th>
                                <th>Total Mentions</th>
                                <th>Sentiment Score</th>
                                <th>Positive Mentions</th>
                                <th>Negative Mentions</th>
                            </tr>
                        </thead>
                        <tbody>
<% foreach ( DataRow dr in Data.Tables[3].Rows ) { %>
                            <tr>
                                <td><%= ReportingTools.CleanData( dr["keyword"] ) %></td>
                                <td><a href="SocialMediaData.aspx?keyword=<%= dr["keyword"].ToString() %>&competitor=0"><%= ReportingTools.CleanData( dr["mentions"] ) %></a></td>
                                <td><%= dr["sentiment_score"].ToString() %></td>
                                <td><a href="SocialMediaData.aspx?keyword=<%= dr["keyword"].ToString() %>&positive=1&competitor=0"><%= dr["Positive"].ToString() %></a></td>
                                <td><a href="SocialMediaData.aspx?keyword=<%= dr["keyword"].ToString() %>&negative=1&competitor=0"><%= dr["Negative"].ToString() %></a></td>
                            </tr>
<% } %>
                        </tbody>
                    </table>
<% } else { %>
                    <p class="text-info">No data found.</p>
<% } %>
                </div>
            </div>
        </div> 
    </div>
    <div class="row">
        <div class="col-md-12">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <i class="fa fa-bullhorn"></i>
                    <h3 class="box-title">Competitor Social Media & Text Analytics</h3>
                </div>
                <div class="box-body border-radius-none" style="">
                    <a href="SocialMediaData.aspx?competitor=1">View Raw Data</a>
                </div>
            </div>
        </div>
    </div>
    <div class="row">        
        <div class="col-md-6">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <h3 class="box-title">Social Media Mention Trending - Source</h3>
                </div>
                <div class="box-body border-radius-none" style="">
                    <%--<canvas id="property-mentiontrending"  height="230" style="height:230px"></canvas>--%>
                    <div class="chart" id="competitor-mentiontrending-source" style="height:400px;"></div>
                </div>
            </div>
        </div>  
        <div class="col-md-6">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <h3 class="box-title">Social Media Mention Trending - Property</h3>
                </div>
                <div class="box-body border-radius-none" style="">
                    <%--<canvas id="property-mentiontrending"  height="230" style="height:230px"></canvas>--%>
                    <div class="chart" id="competitor-mentiontrending-property" style="height:400px;"></div>
                </div>
            </div>
        </div>   
    </div>
    <div class="row">  
        <div class="col-md-6">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <h3 class="box-title">Social Media Mention Overall Distribution</h3>
                </div>
                <div class="box-body border-radius-none" style="">
                    <%--<canvas id="property-mentiontrending"  height="230" style="height:230px"></canvas>--%>
                    <div class="chart" id="competitor-mentionoverall" style="height:400px;"></div>
                </div>
            </div>
        </div>      
        <div class="col-md-6">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <h3 class="box-title">Social Media Sentiment Trending</h3>
                </div>
                <div class="box-body border-radius-none" style="">
                    <%--<canvas id="property-mentiontrending"  height="230" style="height:230px"></canvas>--%>
                    <div class="chart" id="competitor-sentimenttrending" style="height:400px;width:100%!important"></div>
                </div>
            </div>
        </div>  
    </div>    
    <div class="row">     
        <div class="col-md-6">
            <div class="box box-danger">
                <div class="box-header with-border">
                    <h3 class="box-title">Top Keywords</h3>
                </div>
                <div class="box-body border-radius-none" style="">
<% if ( Data != null && Data.Tables.Count > 7 && Data.Tables[7] != null && Data.Tables[7].Rows.Count > 0 ) { %>
                    <table class="table">
                        <thead>
                            <tr>
                                <th>Keyword</th>
                                <th>Total Mentions</th>
                                <th>Sentiment Score</th>
                                <th>Positive Mentions</th>
                                <th>Negative Mentions</th>
                            </tr>
                        </thead>
                        <tbody>
<% foreach ( DataRow dr in Data.Tables[7].Rows ) { %>
                            <tr>
                                <td><%= ReportingTools.CleanData( dr["keyword"] ) %></td>
                                <td><a href="SocialMediaData.aspx?keyword=<%= dr["keyword"].ToString() %>&competitor=1"><%= ReportingTools.CleanData( dr["mentions"] ) %></a></td>
                                <td><%= dr["sentiment_score"].ToString() %></td>
                                <td><a href="SocialMediaData.aspx?keyword=<%= dr["keyword"].ToString() %>&positive=1&competitor=1"><%= dr["Positive"].ToString() %></a></td>
                                <td><a href="SocialMediaData.aspx?keyword=<%= dr["keyword"].ToString() %>&negative=1&competitor=1"><%= dr["Negative"].ToString() %></a></td>
                            </tr>
<% } %>
                        </tbody>
                    </table>
<% } else { %>
                    <p class="text-info">No data found.</p>
<% } %>
                </div>
            </div>
        </div> 
    </div>
<% } %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FooterScripts" runat="server">
    <script src="//code.highcharts.com/highcharts.js"></script>
    <script src="/Scripts/no-data-to-display.js"></script>
<% if ( Data != null && Data.Tables.Count > 0 && Data.Tables[0] != null ) { %>
<script>
    var data, chart;
    data = [
<%
       string categories = ""; 
    foreach (DataColumn dc in Data.Tables[0].Columns)
    {
        //data needs to look like this:
        //{
        //    name: 'Tokyo',
        //    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        //}
        if (dc.ColumnName != "published_date")
        {
            Response.Write("{");
            Response.Write(String.Format("name: '{0}', data: [", dc.ColumnName));
            categories = ""; 
            foreach (DataRow dr in Data.Tables[0].Rows)
            {
                categories += string.Format("'{0}'", dr["published_date"]);
                Response.Write(String.Format("{0}", ( String.IsNullOrEmpty(dr[dc.ColumnName].ToString()) ? "0" : dr[dc.ColumnName].ToString() )));
                if (Data.Tables[0].Rows[Data.Tables[0].Rows.Count - 1] != dr)
                {
                    Response.Write(",");
                    categories += ",";
                }
            }
            Response.Write("]}");
            if (Data.Tables[0].Columns[Data.Tables[0].Columns.Count - 1].ColumnName != dc.ColumnName)
            {
                Response.Write(",");
            }
        }
    }
%>];
    $(function () {
        $('#property-mentiontrending-source').highcharts({
            title: {
                text: false,
                x: -20 //center
            },
            xAxis: {
                categories: [<%= categories %>]
            },
            yAxis: {
                title: {
                    text: false
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            series: data,
            credits: {
                enabled: false
            },
        });
    });
</script>
<% } %>
<% if ( Data != null && Data.Tables.Count > 0 && Data.Tables[8] != null ) { %>
<script>
    var data8, chart8;
    data8 = [
<%
    string categories = "";
    ReportFilterListBox ddlProperty = Master.GetFilter<ReportFilterListBox>("fltProperty");
    Dictionary<string, bool> useProperty = new Dictionary<string, bool>();
    if (!String.IsNullOrWhiteSpace(ddlProperty.SelectedValue))
    {
        foreach (ListItem item in ddlProperty.Items)
        {
            if (item.Selected)
            {
                useProperty.Add(item.Text, true);
            }
        }
    }
    foreach (DataColumn dc in Data.Tables[8].Columns)
    {
        //data needs to look like this:
        //{
        //    name: 'Tokyo',
        //    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        //}
        if (dc.ColumnName != "published_date" && ( String.IsNullOrWhiteSpace(ddlProperty.SelectedValue) || useProperty.ContainsKey(dc.ColumnName) ) )
        {
            Response.Write("{");
            Response.Write(String.Format("name: '{0}', data: [", dc.ColumnName));
            categories = "";
            foreach (DataRow dr in Data.Tables[8].Rows)
            {
                categories += string.Format("'{0}'", dr["published_date"]);
                Response.Write(String.Format("{0}", ( String.IsNullOrEmpty(dr[dc.ColumnName].ToString()) ? "0" : dr[dc.ColumnName].ToString() )));
                if (Data.Tables[8].Rows[Data.Tables[8].Rows.Count - 1] != dr)
                {
                    Response.Write(",");
                    categories += ",";
                }
            }
            Response.Write("]}");
            if (Data.Tables[8].Columns[Data.Tables[8].Columns.Count - 1].ColumnName != dc.ColumnName)
            {
                Response.Write(",");
            }
        }
    }
%>];
    $(function () {
        $('#property-mentiontrending-property').highcharts({
            title: {
                text: false,
                x: -20 //center
            },
            xAxis: {
                categories: [<%= categories %>]
            },
            yAxis: {
                title: {
                    text: false
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            series: data8,
            credits: {
                enabled: false
            },
        });
    });
</script>
<% } %>
<% if (Data != null && Data.Tables.Count > 1 && Data.Tables[1] != null)
   { %>
<script>
    var data2, chart2;
    data2 = [
<%
        // Data values need to look like this:
        //{
        //    name: "Firefox",
        //    y: 10.38
        //}
       foreach (DataRow dr in Data.Tables[1].Rows)
       {
           Response.Write("{");
           Response.Write(String.Format("name:\"{0}\",y: {1}", dr["source"], dr["mentions"]));
           Response.Write("}");
           if (Data.Tables[1].Rows[Data.Tables[1].Rows.Count - 1] != dr)
           {
               Response.Write(",");
           }
       }
%>
    ];
    $('#property-mentionoverall').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: false
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                }
            }
        },
        series: [{
            name: "Source",
            colorByPoint: true,
            data: data2
        }],
        credits: {
            enabled: false
        },
    });
</script>
<% } %>
<% if (Data != null && Data.Tables.Count > 2 && Data.Tables[2] != null)
   { %>
<script>
    var data3;
    data3 = [
<%
       string categories = "";
       foreach (DataColumn dc in Data.Tables[2].Columns)
    {
        //data needs to look like this:
        //{
        //    name: 'Tokyo',
        //    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        //}
        if (dc.ColumnName != "published_date")
        {
            string color = "";
            if (dc.ColumnName == "Positive")
            {
                color = "#00ff00";
            }
            else if (dc.ColumnName == "Negative")
            {
                color = "#ff0000";
            }
            else
            {
                color = "#CCCCCC";
            }
            Response.Write("{");
            Response.Write(String.Format("name: '{0}', color: '{1}', data: [", dc.ColumnName, color));
            categories = "";
            foreach (DataRow dr in Data.Tables[2].Rows)
            {
                categories += string.Format("'{0}'", dr["published_date"]);
                Response.Write(String.Format("{0}", ( String.IsNullOrEmpty(dr[dc.ColumnName].ToString()) ? "0" : dr[dc.ColumnName].ToString() )));
                if (Data.Tables[2].Rows[Data.Tables[2].Rows.Count - 1] != dr)
                {
                    Response.Write(",");
                    categories += ",";
                }
            }
            Response.Write("]}");
            if (Data.Tables[2].Columns[Data.Tables[2].Columns.Count - 1].ColumnName != dc.ColumnName)
            {
                Response.Write(",");
            }
        }
    }
%>];
    $(function () {
        $('#property-sentimenttrending').highcharts({
            chart: {
                type: 'column'
            },
            title: {
                text: false
            },
            xAxis: {
                categories: [<%= categories %>]
            },
            yAxis: {
                min: 0,
                title: {
                    text: false
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
            },
            legend: {
                align: 'right',
                x: -30,
                verticalAlign: 'top',
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                borderColor: '#CCC',
                borderWidth: 1,
                shadow: false
            },
            tooltip: {
                formatter: function () {
                    return '<b>' + this.x + '</b><br/>' +
                        this.series.name + ': ' + this.y + '<br/>' +
                        'Total: ' + this.point.stackTotal;
                }
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                        style: {
                            textShadow: '0 0 3px black'
                        }
                    }
                }
            },
            series: data3,
            credits: {
                enabled: false
            },
        });
    });
</script>
<% } %>
<% if ( Data != null && Data.Tables.Count > 4 && Data.Tables[4] != null ) { %>
<script>
    var data4, chart4;
    data4 = [
<%
       string categories = ""; 
    foreach (DataColumn dc in Data.Tables[4].Columns)
    {
        //data needs to look like this:
        //{
        //    name: 'Tokyo',
        //    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        //}
        if (dc.ColumnName != "published_date")
        {
            Response.Write("{");
            Response.Write(String.Format("name: '{0}', data: [", dc.ColumnName));
            categories = "";
            foreach (DataRow dr in Data.Tables[4].Rows)
            {
                categories += string.Format("'{0}'", dr["published_date"]);
                Response.Write(String.Format("{0}", ( String.IsNullOrEmpty(dr[dc.ColumnName].ToString()) ? "0" : dr[dc.ColumnName].ToString() )));
                if (Data.Tables[4].Rows[Data.Tables[4].Rows.Count - 1] != dr)
                {
                    Response.Write(",");
                    categories += ",";
                }
            }
            Response.Write("]}");
            if (Data.Tables[4].Columns[Data.Tables[4].Columns.Count - 1].ColumnName != dc.ColumnName)
            {
                Response.Write(",");
            }
        }
    }
%>];
    $(function () {
        $('#competitor-mentiontrending-source').highcharts({
            title: {
                text: false,
                x: -20 //center
            },
            xAxis: {
                categories: [<%= categories %>]
            },
            yAxis: {
                title: {
                    text: false
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            series: data4,
            credits: {
                enabled: false
            },
        });
    });
</script>
<% } %>
<% if (Data != null && Data.Tables.Count > 5 && Data.Tables[5] != null)
   { %>
<script>
    var data5;
    data5 = [
<%
        // Data values need to look like this:
        //{
        //    name: "Firefox",
        //    y: 10.38
        //}
       foreach (DataRow dr in Data.Tables[5].Rows)
       {
           Response.Write("{");
           Response.Write(String.Format("name:\"{0}\",y: {1}", dr["source"], dr["mentions"]));
           Response.Write("}");
           if (Data.Tables[5].Rows[Data.Tables[5].Rows.Count - 1] != dr)
           {
               Response.Write(",");
           }
       }
%>
    ];
    $('#competitor-mentionoverall').highcharts({
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: false
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                    style: {
                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                    }
                }
            }
        },
        series: [{
            name: "Source",
            colorByPoint: true,
            data: data5
        }],
        credits: {
            enabled: false
        },
    });
</script>
<% } %>
<% if (Data != null && Data.Tables.Count > 6 && Data.Tables[6] != null)
   { %>
<script>
    var data6;
    data6 = [
<%
       string categories = "";
       foreach (DataColumn dc in Data.Tables[6].Columns)
    {
        //data needs to look like this:
        //{
        //    name: 'Tokyo',
        //    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        //}
        if (dc.ColumnName != "published_date")
        {
            string color = "";
            if (dc.ColumnName == "Positive")
            {
                color = "#00ff00";
            }
            else if (dc.ColumnName == "Negative")
            {
                color = "#ff0000";
            }
            else
            {
                color = "#CCCCCC";
            }
            Response.Write("{");
            Response.Write(String.Format("name: '{0}', color: '{1}', data: [", dc.ColumnName, color));
            categories = "";
            foreach (DataRow dr in Data.Tables[6].Rows)
            {
                categories += string.Format("'{0}'", dr["published_date"]);
                Response.Write(String.Format("{0}", ( String.IsNullOrEmpty(dr[dc.ColumnName].ToString()) ? "0" : dr[dc.ColumnName].ToString() )));
                if (Data.Tables[6].Rows[Data.Tables[6].Rows.Count - 1] != dr)
                {
                    Response.Write(",");
                    categories += ",";
                }
            }
            Response.Write("]}");
            if (Data.Tables[6].Columns[Data.Tables[6].Columns.Count - 1].ColumnName != dc.ColumnName)
            {
                Response.Write(",");
            }
        }
    }
%>];
    $(function () {
        $('#competitor-sentimenttrending').highcharts({
            chart: {
                type: 'column'
            },
            title: {
                text: false
            },
            xAxis: {
                categories: [<%= categories %>]
            },
            yAxis: {
                min: 0,
                title: {
                    text: false
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
            },
            legend: {
                align: 'right',
                x: -30,
                verticalAlign: 'top',
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                borderColor: '#CCC',
                borderWidth: 1,
                shadow: false
            },
            tooltip: {
                formatter: function () {
                    return '<b>' + this.x + '</b><br/>' +
                        this.series.name + ': ' + this.y + '<br/>' +
                        'Total: ' + this.point.stackTotal;
                }
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                        style: {
                            textShadow: '0 0 3px black'
                        }
                    }
                }
            },
            series: data6,
            credits: {
                enabled: false
            },
        });
    });
</script>
<% } %><% if ( Data != null && Data.Tables.Count > 0 && Data.Tables[9] != null ) { %>
<script>
    var data9, chart9;
    data9 = [
<%
    string categories = "";
    foreach (DataColumn dc in Data.Tables[9].Columns)
    {
        //data needs to look like this:
        //{
        //    name: 'Tokyo',
        //    data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
        //}
        if (dc.ColumnName != "published_date")
        {
            Response.Write("{");
            Response.Write(String.Format("name: '{0}', data: [", dc.ColumnName));
            categories = "";
            foreach (DataRow dr in Data.Tables[9].Rows)
            {
                categories += string.Format("'{0}'", dr["published_date"]);
                Response.Write(String.Format("{0}", ( String.IsNullOrEmpty(dr[dc.ColumnName].ToString()) ? "0" : dr[dc.ColumnName].ToString() )));
                if (Data.Tables[9].Rows[Data.Tables[9].Rows.Count - 1] != dr)
                {
                    Response.Write(",");
                    categories += ",";
                }
            }
            Response.Write("]}");
            if (Data.Tables[9].Columns[Data.Tables[9].Columns.Count - 1].ColumnName != dc.ColumnName)
            {
                Response.Write(",");
            }
        }
    }
%>];
    $(function () {
        $('#competitor-mentiontrending-property').highcharts({
            title: {
                text: false,
                x: -20 //center
            },
            xAxis: {
                categories: [<%= categories %>]
            },
            yAxis: {
                title: {
                    text: false
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            series: data9,
            credits: {
                enabled: false
            },
        });
    });
</script>
<% } %>
</asp:Content>
