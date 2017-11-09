using SharedClasses;
using System;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace GCC_Web_Portal
{
    public partial class StaffSurveySelection : AuthenticatedPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master.IsPropertyUser)
            {
                Response.Redirect("/SurveyS/" + User.PropertyShortCode.ToString());
                return;
            }
            Master.HideAllFilters = true;
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            GCCPropertyShortCode prop = (GCCPropertyShortCode)ddlProperty.SelectedValue.StringToInt();
            GEISurveyLanguage GEILIndex;
            if (Master.IsPropertyUser)
            {

                if (ddlProperty.SelectedValue == "18" || ddlProperty.SelectedValue == "19")
                {
                    GEILIndex = (GEISurveyLanguage)ddlSurveyLang.SelectedValue.StringToInt();
                }
                else
                {
                    GEILIndex = (GEISurveyLanguage)1;
                }
                Session["SurveyLang"] = GEILIndex;
                Response.Redirect("/SurveyS/" + User.PropertyShortCode.ToString());
                return;
            }

            if (ddlProperty.SelectedValue == "18" || ddlProperty.SelectedValue == "19")
            {
                GEILIndex = (GEISurveyLanguage)ddlSurveyLang.SelectedValue.StringToInt();
            }
            else
            {
                GEILIndex = (GEISurveyLanguage)1;
            }

            Session["SurveyLang"] = GEILIndex;

            Response.Redirect("/SurveyS/" + prop.ToString());
            return;
        }

        protected void ddlProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProperty.SelectedIndex > 0)
            {


                if (ddlProperty.SelectedValue == "18" || ddlProperty.SelectedValue == "19")
                {
                    ddlSurveyLang.Items.Clear();



                    ddlSurveyLang.Items.Add(new ListItem("English", "1"));
                    ddlSurveyLang.Items.Add(new ListItem("French", "2"));

                }

                else
                {
                    ddlSurveyLang.Items.Clear();
                    ddlSurveyLang.Items.Add(new ListItem("English", "1"));

                }

            }
        }
    }
}