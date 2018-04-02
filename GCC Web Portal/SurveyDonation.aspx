<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyDonation.aspx.cs" Inherits="GCC_Web_Portal.SurveyDonation" MasterPageFile="~/Survey.Master" %>
<%@ MasterType VirtualPath="~/Survey.Master" %>
<%@ Import Namespace="SharedClasses" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="System.Resources" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Register Src="~/Controls/QuestionRowControl.ascx" TagPrefix="uc1" TagName="QuestionRowControl" %>
<%@ Register Src="~/Controls/ScaleQuestionControl.ascx" TagPrefix="uc1" TagName="ScaleQuestionControl" %>
<%@ Register Src="~/Controls/YesNoControl.ascx" TagPrefix="uc1" TagName="YesNoControl" %>
<%@ Register Src="~/Controls/YesNoControlFrench.ascx" TagPrefix="uc1" TagName="YesNoControlFrench" %>
<asp:Content runat="server" ContentPlaceHolderID="headContent">
    
     <% if (Session["CurrentUI"] == "fr-CA") { %>
    <style>
        #header-logo {
            background-image:url('../../Images/headers/DonationHeader_French.jpg');
            width:756px;
            height:113px;
        }
    </style>
     <% } else { %>
    <style>
        #header-logo {
            background-image:url('../../Images/headers/DonationHeader.jpg');
            width:756px;
            height:113px;
        }
    </style>
     <%  }  %>

</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="mainContent">
    <h2><%= Lang.Donation_PageTitle %></h2>
     <% if (Session["CurrentUI"] == "fr-CA") { %>
            <asp:Button ID="btnEnglish" CssClass="btn btn-primary" runat="server" Text="English" OnClick="EnglishLinkButton_Click" />
            <br /><br />
       <% } else { %>
            <asp:Button ID="btnFrench" CssClass="btn btn-primary" runat="server" Text="Français" OnClick="FrenchLinkButton_Click" />
            <br /><br />
      <%  }  %>
    <%
        //===========================================================================
        //PAGE 1 - Everything
        //===========================================================================
    if ( Master.CurrentPage == 1) {%>
    <p><%= Lang.Donation_Q1Title %></p>
    <p class="question"><%= Lang.Donation_Q1 %></p>
    <sc:SurveyTextBox ID="Q1" runat="server" SessionKey="Q1" DBColumn="Q1" MaxLength="200" Size="50"></sc:SurveyTextBox>

    <p class="question"><%= Lang.Donation_Q2 %></p>

    <% if (Session["CurrentUI"] == "fr-CA") { %>
    <uc1:YesNoControlFrench runat="server" ID="Q2_F" SessionKey="Q2" DBColumn="Q2" />
    <% } else { %>
     <uc1:YesNoControl runat="server" ID="Q2" SessionKey="Q2" DBColumn="Q2" />
    <%  }  %>


    <p class="question"><%= Lang.Donation_Q3 %></p>
    <sc:SurveyTextBox ID="Q3" runat="server" SessionKey="Q3" DBColumn="Q3" MaxLength="200" Size="50"></sc:SurveyTextBox>

    <p class="question"><%= Lang.Donation_Q4 %></p>
    <sc:SurveyTextBox ID="Q4" runat="server" SessionKey="Q4" DBColumn="Q4" MaxLength="200" Size="50"></sc:SurveyTextBox>

    <p class="question"><%= Lang.Donation_Q5Title %></p>
    <div class="row" style="margin-top:3px;">
        <div class="col-xs-5"><%= Lang.Donation_Q5ContactName %></div>
        <div class="col-xs-7"><sc:SurveyTextBox ID="Q5Name" runat="server" SessionKey="Q5Name" DBColumn="Q5Name" MaxLength="200" Style="width:80%;"></sc:SurveyTextBox></div>
    </div>
    <div class="row" style="margin-top:3px;">
        <div class="col-xs-5"><%= Lang.Donation_Q5ContactTitle %></div>
        <div class="col-xs-7"><sc:SurveyTextBox ID="Q5Title" runat="server" SessionKey="Q5Title" DBColumn="Q5Title" MaxLength="200" Style="width:80%;"></sc:SurveyTextBox></div>
    </div>
    <div class="row" style="margin-top:3px;">
        <div class="col-xs-5"><%= Lang.Donation_Q5ContactPhone %></div>
        <div class="col-xs-7"><sc:SurveyTextBox ID="Q5Telephone" runat="server" SessionKey="Q5Telephone" DBColumn="Q5Telephone" MaxLength="200" Style="width:80%;"></sc:SurveyTextBox></div>
    </div>
    <div class="row" style="margin-top:3px;">
        <div class="col-xs-5"><%= Lang.Donation_Q5ContactEmail %></div>
        <div class="col-xs-7"><sc:SurveyTextBox ID="Q5Email" runat="server" SessionKey="Q5Email" DBColumn="Q5Email" MaxLength="200" Style="width:80%;"></sc:SurveyTextBox></div>
    </div>

    <p class="question"><%= Lang.Donation_Q6Title %></p>
    <div class="row" style="margin-top:3px;">
        <div class="col-xs-5"><%= Lang.Donation_Q6Street %></div>
        <div class="col-xs-7"><sc:SurveyTextBox ID="Q6Street" runat="server" SessionKey="Q6Street" DBColumn="Q6Street" MaxLength="200" Style="width:80%;"></sc:SurveyTextBox></div>
    </div>
    <div class="row" style="margin-top:3px;">
        <div class="col-xs-5"><%= Lang.Donation_Q6City %></div>
        <div class="col-xs-7"><sc:SurveyTextBox ID="Q6City" runat="server" SessionKey="Q6City" DBColumn="Q6City" MaxLength="200" Style="width:80%;"></sc:SurveyTextBox></div>
    </div>
    <div class="row" style="margin-top:3px;">
        <div class="col-xs-5"><%= Lang.Donation_Q6Province %></div>
        <div class="col-xs-7"><sc:SurveyTextBox ID="Q6Province" runat="server" SessionKey="Q6Province" DBColumn="Q6Province" MaxLength="200" Style="width:80%;"></sc:SurveyTextBox></div>
    </div>
    <div class="row" style="margin-top:3px;">
        <div class="col-xs-5"><%= Lang.Donation_Q6Postal %></div>
        <div class="col-xs-7"><sc:SurveyTextBox ID="Q6PostalCode" runat="server" SessionKey="Q6PostalCode" DBColumn="Q6PostalCode" MaxLength="200" Style="width:80%;"></sc:SurveyTextBox></div>
    </div>
    
    <p class="question"><%= Lang.Donation_Q7 %></p>
    <sc:SurveyTextBox ID="Q7" runat="server" SessionKey="Q7" DBColumn="Q7" MaxLength="200" Size="50"></sc:SurveyTextBox>

    <p class="question"><%= Lang.Donation_Q8 %></p>
    <sc:SurveyTextBox ID="Q8" runat="server" SessionKey="Q8" DBColumn="Q8" MaxLength="200" Size="50"></sc:SurveyTextBox>

    <p class="question"><%= Lang.Donation_Q9Title %></p>
    <sc:MessageManager runat="server" ID="mmQ9"></sc:MessageManager>
    <sc:SurveyRadioButton runat="server" ID="Q9A" GroupName="Q9" SessionKey="Q9A" DBColumn="Q9" DBValue="DONATION" Text="<%$Resources:Lang, Donation_Q9Donation %>" /><br />
    <sc:SurveyRadioButton runat="server" ID="Q9B" GroupName="Q9" SessionKey="Q9B" DBColumn="Q9" DBValue="SPONSORSHIP" Text="<%$Resources:Lang, Donation_Q9Sponsor %>" /><br />
    <sc:SurveyRadioButton runat="server" ID="Q9C" GroupName="Q9" SessionKey="Q9C" DBColumn="Q9" DBValue="OTHER" Text="<%$Resources:Lang, Donation_Q9Other %>" /><br />
    <p><%= Lang.Donation_Q9OtherTitle %></p>
    <sc:SurveyTextBox ID="Q9C_Explanation" runat="server" SessionKey="Q9C_Explanation" DBColumn="Q9C_Explanation" MaxLength="200" Style="width:80%" Rows="2" TextMode="MultiLine"></sc:SurveyTextBox>

    <p class="question"><%= Lang.Donation_Q10 %></p>
    <sc:SurveyTextBox ID="Q10" runat="server" SessionKey="Q10" DBColumn="Q10" MaxLength="200" Size="20"></sc:SurveyTextBox>


    <p class="question"><%= Lang.Donation_Q11 %>
    </p>
    <div class="row">
        <div class="col-xs-6 col-md-5"></div>
        <div class="col-xs-3 col-md-2 text-center"><%= Lang.Donation_Q11Past %></div>
        <div class="col-xs-3 col-md-2 text-center"><%= Lang.Donation_Q11Current %></div>
    </div>
    <div class="striped">
        <div class="row">
            <div class="col-xs-6 col-md-5">River Rock Casino Resort (Richmond, BC)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11A_PastSupport" SessionKey="Q11A_PastSupport" DBColumn="Q11A_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11A_CurrentRequest" SessionKey="Q11A_CurrentRequest" DBColumn="Q11A_CurrentRequest" DBValue="1" /></div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-5">Hard Rock Casino Vancouver (Coquitlam, BC)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11B_PastSupport" SessionKey="Q11B_PastSupport" DBColumn="Q11B_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11B_CurrentRequest" SessionKey="Q11B_CurrentRequest" DBColumn="Q11B_CurrentRequest" DBValue="1" /></div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-5">Hastings Racecourse and Slots (Vancouver, BC)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11C_PastSupport" SessionKey="Q11C_PastSupport" DBColumn="Q11C_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11C_CurrentRequest" SessionKey="Q11C_CurrentRequest" DBColumn="Q11C_CurrentRequest" DBValue="1" /></div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-5">Elements Casino (Surrey, BC)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11D_PastSupport" SessionKey="Q11D_PastSupport" DBColumn="Q11D_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11D_CurrentRequest" SessionKey="Q11D_CurrentRequest" DBColumn="Q11D_CurrentRequest" DBValue="1" /></div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-5">View Royal Casino (Victoria, BC)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11E_PastSupport" SessionKey="Q11E_PastSupport" DBColumn="Q11E_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11E_CurrentRequest" SessionKey="Q11E_CurrentRequest" DBColumn="Q11E_CurrentRequest" DBValue="1" /></div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-5">Nanaimo Casino (Nanaimo, BC)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11F_PastSupport" SessionKey="Q11F_PastSupport" DBColumn="Q11F_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11F_CurrentRequest" SessionKey="Q11F_CurrentRequest" DBColumn="Q11F_CurrentRequest" DBValue="1" /></div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-5">Chances Dawson Creek (Dawson Creek, BC)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11G_PastSupport" SessionKey="Q11G_PastSupport" DBColumn="Q11G_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11G_CurrentRequest" SessionKey="Q11G_CurrentRequest" DBColumn="Q11G_CurrentRequest" DBValue="1" /></div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-5">Maple Ridge Community Gaming Centre (Maple Ridge, BC)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11H_PastSupport" SessionKey="Q11H_PastSupport" DBColumn="Q11H_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11H_CurrentRequest" SessionKey="Q11H_CurrentRequest" DBColumn="Q11H_CurrentRequest" DBValue="1" /></div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-5">Chilliwack Bingo (Chilliwack, BC)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11I_PastSupport" SessionKey="Q11I_PastSupport" DBColumn="Q11I_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11I_CurrentRequest" SessionKey="Q11I_CurrentRequest" DBColumn="Q11I_CurrentRequest" DBValue="1" /></div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-5">Corporate Donation – Head Office (Richmond, BC)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11J_PastSupport" SessionKey="Q11J_PastSupport" DBColumn="Q11J_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11J_CurrentRequest" SessionKey="Q11J_CurrentRequest" DBColumn="Q11J_CurrentRequest" DBValue="1" /></div>
        </div>
        <div class="row">
            <div class="col-xs-6 col-md-5">Shorelines Slots at Kawartha Downs (Fraserville, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11K_PastSupport" SessionKey="Q11K_PastSupport" DBColumn="Q11K_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11K_CurrentRequest" SessionKey="Q11K_CurrentRequest" DBColumn="Q11K_CurrentRequest" DBValue="1" /></div>
        </div>

                <div class="row">
            <div class="col-xs-6 col-md-5">Shorelines Casino Belleville (Belleville, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11R_PastSupport" SessionKey="Q11R_PastSupport" DBColumn="Q11R_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11R_CurrentRequest" SessionKey="Q11R_CurrentRequest" DBColumn="Q11R_CurrentRequest" DBValue="1" /></div>
        </div>



        <% if (Session["CurrentUI"] == "fr-CA") { %>
        <div class="row">
            <div class="col-xs-6 col-md-5">Casino Shorelines à Thousand Islands (Gananoque, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11L_PastSupport" SessionKey="Q11L_PastSupport" DBColumn="Q11L_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11L_CurrentRequest" SessionKey="Q11L_CurrentRequest" DBColumn="Q11L_CurrentRequest" DBValue="1" /></div>
        </div>

        <div class="row">
            <div class="col-xs-6 col-md-5">Casino Nouveau-Brunswick (Moncton, NB)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11M_PastSupport" SessionKey="Q11M_PastSupport" DBColumn="Q11M_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11M_CurrentRequest" SessionKey="Q11M_CurrentRequest" DBColumn="Q11M_CurrentRequest" DBValue="1" /></div>
        </div>


        <div class="row">
            <div class="col-xs-6 col-md-5">Casino Woodbine (Woodbine, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11N_PastSupport" SessionKey="Q11N_PastSupport" DBColumn="Q11N_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11N_CurrentRequest" SessionKey="Q11N_CurrentRequest" DBColumn="Q11N_CurrentRequest" DBValue="1" /></div>
        </div>



        <%} else { %>

        <div class="row">
            <div class="col-xs-6 col-md-5">Shorelines Casino Thousand Islands (Gananoque, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11O_PastSupport" SessionKey="Q11O_PastSupport" DBColumn="Q11O_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11O_CurrentRequest" SessionKey="Q11O_CurrentRequest" DBColumn="Q11O_CurrentRequest" DBValue="1" /></div>
        </div>

        <div class="row">
            <div class="col-xs-6 col-md-5">Casino New Brunswick (Moncton, NB)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11P_PastSupport" SessionKey="Q11P_PastSupport" DBColumn="Q11P_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11P_CurrentRequest" SessionKey="Q11P_CurrentRequest" DBColumn="Q11P_CurrentRequest" DBValue="1" /></div>
        </div>

          <div class="row">
            <div class="col-xs-6 col-md-5">Casino Woodbine (Woodbine, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11Q_PastSupport" SessionKey="Q11Q_PastSupport" DBColumn="Q11Q_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11Q_CurrentRequest" SessionKey="Q11Q_CurrentRequest" DBColumn="Q11Q_CurrentRequest" DBValue="1" /></div>
        </div>


        <%} %>

        

        <div class="row">
            <div class="col-xs-6 col-md-5">Casino Nova Scotia (Halifax, NS)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11U_PastSupport" SessionKey="Q11U_PastSupport" DBColumn="Q11U_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11U_CurrentRequest" SessionKey="Q11U_CurrentRequest" DBColumn="Q11U_CurrentRequest" DBValue="1" /></div>
        </div>



        <div class="row">
            <div class="col-xs-6 col-md-5">Casino Nova Scotia (Sydney, NS)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11V_PastSupport" SessionKey="Q11V_PastSupport" DBColumn="Q11V_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11V_CurrentRequest" SessionKey="Q11V_CurrentRequest" DBColumn="Q11V_CurrentRequest" DBValue="1" /></div>
        </div>

 


        <div class="row">
            <div class="col-xs-6 col-md-5">Casino Ajax (Ajax, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11S_PastSupport" SessionKey="Q11S_PastSupport" DBColumn="Q11S_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11S_CurrentRequest" SessionKey="Q11S_CurrentRequest" DBColumn="Q11S_CurrentRequest" DBValue="1" /></div>
        </div>



        <div class="row">
            <div class="col-xs-6 col-md-5">Great Blue Heron Casino (Port Perry, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11T_PastSupport" SessionKey="Q11T_PastSupport" DBColumn="Q11T_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11T_CurrentRequest" SessionKey="Q11T_CurrentRequest" DBColumn="Q11T_CurrentRequest" DBValue="1" /></div>
        </div>

        <div class="row">
            <div class="col-xs-6 col-md-5">Elements Casino Brantford (Brantford, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11W_PastSupport" SessionKey="Q11W_PastSupport" DBColumn="Q11T_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11W_CurrentRequest" SessionKey="Q11W_CurrentRequest" DBColumn="Q11T_CurrentRequest" DBValue="1" /></div>
        </div>


        <div class="row">
            <div class="col-xs-6 col-md-5">Elements Casino Flamboro (Flamboro, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11X_PastSupport" SessionKey="Q11X_PastSupport" DBColumn="Q11T_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11X_CurrentRequest" SessionKey="Q11X_CurrentRequest" DBColumn="Q11T_CurrentRequest" DBValue="1" /></div>
        </div>


        <div class="row">
            <div class="col-xs-6 col-md-5">Elements Casino Grand River (Grand River, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11Y_PastSupport" SessionKey="Q11Y_PastSupport" DBColumn="Q11T_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11Y_CurrentRequest" SessionKey="Q11Y_CurrentRequest" DBColumn="Q11T_CurrentRequest" DBValue="1" /></div>
        </div>

        <div class="row">
            <div class="col-xs-6 col-md-5">Elements Casino Mohawk (Mohawk, ON)</div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11Z_PastSupport" SessionKey="Q11Z_PastSupport" DBColumn="Q11T_PastSupport" DBValue="1" /></div>
            <div class="col-xs-3 col-md-2 text-center"><sc:SurveyCheckBox runat="server" ID="Q11Z_CurrentRequest" SessionKey="Q11Z_CurrentRequest" DBColumn="Q11T_CurrentRequest" DBValue="1" /></div>
        </div>

    </div>



    <p class="question"><%= Lang.Donation_Q12 %></p>
    <sc:SurveyTextBox ID="Q12" runat="server" SessionKey="Q12" DBColumn="Q12" MaxLength="200" Style="width:80%" Rows="2" TextMode="MultiLine"></sc:SurveyTextBox>

    <p class="question"><%= Lang.Donation_Q13 %></p>
    <sc:SurveyTextBox ID="Q13" runat="server" SessionKey="Q13" DBColumn="Q13" MaxLength="1000" Style="width:80%" Rows="5" TextMode="MultiLine"></sc:SurveyTextBox>


    <p class="question"><%= Lang.Donation_Q14 %></p>
    <sc:SurveyTextBox ID="Q14" runat="server" SessionKey="Q14" DBColumn="Q14" MaxLength="1000" Style="width:80%" Rows="5" TextMode="MultiLine"></sc:SurveyTextBox>

    <div class="button-container">
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="<%$Resources:Lang, SFeedbackNextButton %>" />
    </div>
    <% }
        //===========================================================================
        //PAGE 2 - Final Confirmation
        //===========================================================================
        else if ( Master.CurrentPage == 2 ) { %>
    <p><%= Lang.Donation_Final1 %></p>
    <%if(Master.PropertyShortCode == GCCPropertyShortCode.WDB ||
         Master.PropertyShortCode == GCCPropertyShortCode.AJA || Master.PropertyShortCode == GCCPropertyShortCode.GBH ||
         Master.PropertyShortCode == GCCPropertyShortCode.SCTI || Master.PropertyShortCode == GCCPropertyShortCode.SSKD || Master.PropertyShortCode == GCCPropertyShortCode.SCBE
         Master.PropertyShortCode == GCCPropertyShortCode.ECB|| Master.PropertyShortCode == GCCPropertyShortCode.ECF|| Master.PropertyShortCode == GCCPropertyShortCode.ECGR|| Master.PropertyShortCode == GCCPropertyShortCode.ECM){ %>
    <p><%= Lang.Donation_Final2_Ontario %></p>
    <%}else{ %>
    <p><%= Lang.Donation_Final2 %></p>
    <%} %>
    <p><%= Lang.Donation_Final3 %></p>

    
    <div class="button-container">
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="<%$Resources:Lang, SFeedbackPreviousButton %>" />
        <asp:Button runat="server" OnClick="Next_Click" CssClass="btn btn-primary" Text="<%$Resources:Lang, SFeedbackDoneButton %>" />
    </div>
    <% }
        //===========================================================================
        //PAGE 99 - Thank You
        //===========================================================================
        else if ( Master.CurrentPage == 99 ) { %>
    <sc:MessageManager runat="server" ID="mmLastPage"></sc:MessageManager>
    <div class="button-container">
    <% if (!String.IsNullOrEmpty( mmLastPage.SuccessMessage)) { %>
        <a href="<%= PropertyTools.GetCasinoURL( Master.PropertyShortCode ) %>" class="btn btn-success"><%= Lang.DonationReturntoSite %></a>
    <% } else { %>
        <asp:Button runat="server" OnClick="Prev_Click" CssClass="btn btn-default" Text="<%$Resources:Lang, SFeedbackPreviousButton %>" />
    <% } %>
    </div>
    <% } %>
</asp:Content>