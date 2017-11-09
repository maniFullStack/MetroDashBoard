using SharedClasses;
using System;
using System.Text;
using WebsiteUtilities;

namespace GCC_Web_Portal.Controls
{
    public partial class YesNoControlFrench : System.Web.UI.UserControl, ISurveyControl<int>
    {
        /// <summary>
        /// Gets or sets the selected value of the control. 0 is considered "No" and 1 is "Yes". If nothing is selected, -1 will be returned.
        /// </summary>
        public int SelectedValue_F
        {
            get
            {
                if (radYes_F.Checked)
                {
                    return 1;
                }
                else if (radNo_F.Checked)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                radYes_F.Checked = (value == 1);
                radNo_F.Checked = (value == 0);
            }
        }

        /// <summary>
        /// Gets the message manager instance for this row.
        /// </summary>
        public MessageManager MessageManager
        {
            get
            {
                return mmMessage_F;
            }
        }

        public string SessionKey { get; set; }

        public string DBColumn { get; set; }

        public string DBValue
        {
            get
            {
                return SelectedValue_F.ToString();
            }
        }

        public int GetValue()
        {
            return SelectedValue_F;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!String.IsNullOrEmpty(SessionKey) && SelectedValue_F == -1)
            {
                var sVal = SessionWrapper.Get<SurveySessionControl<int>>(SessionKey, null);
                if (sVal != null)
                {
                    SelectedValue_F = sVal.Value;
                }
            }
        }


        public void PrepareQuestionForDB(StringBuilder columnList, SQLParamList sqlParams)
        {
            if (SelectedValue_F != -1)
            {
                columnList.AppendFormat(",[{0}]", DBColumn);
                sqlParams.Add("@" + DBColumn, GetValue());
            }
        }

    }
}