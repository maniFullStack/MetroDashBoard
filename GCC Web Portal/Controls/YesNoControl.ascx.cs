using SharedClasses;
using System;
using System.Text;
using WebsiteUtilities;

namespace GCC_Web_Portal.Controls
{
    public partial class YesNoControl : System.Web.UI.UserControl, ISurveyControl<int>
    {
        /// <summary>
        /// Gets or sets the selected value of the control. 0 is considered "No" and 1 is "Yes". If nothing is selected, -1 will be returned.
        /// </summary>
        public int SelectedValue
        {
            get
            {
                if (radYes.Checked)
                {
                    return 1;
                }
                else if (radNo.Checked)
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
                radYes.Checked = (value == 1);
                radNo.Checked = (value == 0);
            }
        }

        /// <summary>
        /// Gets the message manager instance for this row.
        /// </summary>
        public MessageManager MessageManager
        {
            get
            {
                return mmMessage;
            }
        }

        public string SessionKey { get; set; }

        public string DBColumn { get; set; }

        public string DBValue
        {
            get
            {
                return SelectedValue.ToString();
            }
        }

        public int GetValue()
        {
            return SelectedValue;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!String.IsNullOrEmpty(SessionKey) && SelectedValue == -1)
            {
                var sVal = SessionWrapper.Get<SurveySessionControl<int>>(SessionKey, null);
                if (sVal != null)
                {
                    SelectedValue = sVal.Value;
                }
            }
        }

        public void PrepareQuestionForDB(StringBuilder columnList, SQLParamList sqlParams)
        {
                      

            if (SelectedValue != -1)
            {
                columnList.AppendFormat(",[{0}]", DBColumn);
                sqlParams.Add("@" + DBColumn, GetValue());
            }
        }


    }
}