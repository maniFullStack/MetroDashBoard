using SharedClasses;
using System;
using System.Text;
using WebsiteUtilities;

namespace GCC_Web_Portal.Controls
{
    public partial class ScaleQuestionControlFrench : System.Web.UI.UserControl, ISurveyControl<int>
    {
        /// <summary>
        /// Gets or sets the selected value of the control. 1 is considered "Very Dissatisfied" and 5 is "Extremely Satisfied" (or equivalent name). If nothing is selected, -1 will be returned.
        /// </summary>
        public int SelectedValue_F
        {
            get
            {
                if (radAnswer1_F.Checked)
                {
                    return 1;
                }
                else if (radAnswer2_F.Checked)
                {
                    return 2;
                }
                else if (radAnswer3_F.Checked)
                {
                    return 3;
                }
                else if (radAnswer4_F.Checked)
                {
                    return 4;
                }
                else if (radAnswer5_F.Checked)
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
                radAnswer1_F.Checked = (value == 1);
                radAnswer2_F.Checked = (value == 2);
                radAnswer3_F.Checked = (value == 3);
                radAnswer4_F.Checked = (value == 4);
                radAnswer5_F.Checked = (value == 5);
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

        /// <summary>
        /// Sets the answer labels. Must be an array of 5 items from "Very Dissatisfied" to "Extremely Satisfied" (bottom to top).
        /// </summary>
        /// <param name="labels"></param>
        public void SetAnswerLabels(string[] labels)
        {
            radAnswer1_F.Text = "&nbsp;" + labels[0];
            radAnswer2_F.Text = "&nbsp;" + labels[1];
            radAnswer3_F.Text = "&nbsp;" + labels[2];
            radAnswer4_F.Text = "&nbsp;" + labels[3];
            radAnswer5_F.Text = "&nbsp;" + labels[4];
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SetAnswerLabels(new string[] { "Très insatisfait", "Insatisfait", "Satisfait", "Très satisfait", "Extrêmement satisfait" });
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