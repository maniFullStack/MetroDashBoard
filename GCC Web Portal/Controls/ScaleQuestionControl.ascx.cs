using SharedClasses;
using System;
using System.Text;
using WebsiteUtilities;

namespace GCC_Web_Portal.Controls
{
    public partial class ScaleQuestionControl : System.Web.UI.UserControl, ISurveyControl<int>
    {
        /// <summary>
        /// Gets or sets the selected value of the control. 1 is considered "Very Dissatisfied" and 5 is "Extremely Satisfied" (or equivalent name). If nothing is selected, -1 will be returned.
        /// </summary>
        public int SelectedValue
        {
            get
            {
                if (radAnswer1.Checked)
                {
                    return 1;
                }
                else if (radAnswer2.Checked)
                {
                    return 2;
                }
                else if (radAnswer3.Checked)
                {
                    return 3;
                }
                else if (radAnswer4.Checked)
                {
                    return 4;
                }
                else if (radAnswer5.Checked)
                {
                    return 5;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                radAnswer1.Checked = (value == 1);
                radAnswer2.Checked = (value == 2);
                radAnswer3.Checked = (value == 3);
                radAnswer4.Checked = (value == 4);
                radAnswer5.Checked = (value == 5);
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

        /// <summary>
        /// Sets the answer labels. Must be an array of 5 items from "Very Dissatisfied" to "Extremely Satisfied" (bottom to top).
        /// </summary>
        /// <param name="labels"></param>
        public void SetAnswerLabels(string[] labels)
        {
            radAnswer1.Text = "&nbsp;" + labels[0];
            radAnswer2.Text = "&nbsp;" + labels[1];
            radAnswer3.Text = "&nbsp;" + labels[2];
            radAnswer4.Text = "&nbsp;" + labels[3];
            radAnswer5.Text = "&nbsp;" + labels[4];
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SetAnswerLabels(new string[] { "Very Dissatisfied", "Dissatisfied", "Satisfied", "Very Satisfied", "Extremely Satisfied" });
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