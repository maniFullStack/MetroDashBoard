<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurveyProgressBar.ascx.cs" Inherits="GCC_Web_Portal.Controls.SurveyProgressBar" %>
<div class="progress survey-progress-bar">
  <div class="progress-bar progress-bar-success progress-bar-striped" role="progressbar" aria-valuenow="<%= Percentage %>" aria-valuemin="0" aria-valuemax="100" style="width: <%= Percentage %>%; min-width: 2em;">
    <%= Percentage %>%
  </div>
</div>