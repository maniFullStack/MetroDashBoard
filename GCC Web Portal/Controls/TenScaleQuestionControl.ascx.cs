using SharedClasses;
using System;
using System.Text;
using WebsiteUtilities;

namespace GCC_Web_Portal.Controls
{
    public partial class TenScaleQuestionControl : System.Web.UI.UserControl, ISurveyControl<int>
    {
		public bool HideZero { get; set; } 

        /// <summary>
        /// Gets or sets the selected value of the control. 1 is considered "Very Dissatisfied" and 5 is "Extremely Satisfied" (or equivalent name). If nothing is selected, -1 will be returned.
        /// </summary>
        public int SelectedValue
        {
            get
            {
                if (OLG1.Checked && !HideZero )
                {
                    return 0;
                }
                else if (OLG2.Checked)
                {
                    return 1;
                }
                else if (OLG3.Checked)
                {
                    return 2;
                }
                else if (OLG4.Checked)
                {
                    return 3;
                }
                else if (OLG5.Checked)
                {
                    return 4;
                }
                else if (OLG6.Checked)
                {
                    return 5;
                }
                else if (OLG7.Checked)
                {
                    return 6;
                }
                else if (OLG8.Checked)
                {
                    return 7;
                }
                else if (OLG9.Checked)
                {
                    return 8;
                }
                else if (OLG10.Checked)
                {
                    return 9;
                }
                else if (OLG11.Checked)
                {
                    return 10;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                OLG1.Checked = (value == 0);
                OLG2.Checked = (value == 1);
                OLG3.Checked = (value == 2);
                OLG4.Checked = (value == 3);
                OLG5.Checked = (value == 4);
                OLG6.Checked = (value == 5);
                OLG7.Checked = (value == 6);
                OLG8.Checked = (value == 7);
                OLG9.Checked = (value == 8);
                OLG10.Checked = (value == 9);
                OLG11.Checked = (value == 10);
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
        /// Sets the answer labels. Must be an array of 10 items from "1" to "10" (left to right).
        /// </summary>
        /// <param name="labels"></param>
        public void SetAnswerLabels(string[] labels)
        {
            if( !HideZero) OLG1.Text = "&nbsp;" + labels[0] + "&nbsp;" ;
            OLG2.Text = "&nbsp;" + labels[1] + "&nbsp;";
            OLG3.Text = "&nbsp;" + labels[2] + "&nbsp;";
            OLG4.Text = "&nbsp;" + labels[3] + "&nbsp;";
            OLG5.Text = "&nbsp;" + labels[4] + "&nbsp;";
            OLG6.Text = "&nbsp;" + labels[5] + "&nbsp;";
            OLG7.Text = "&nbsp;" + labels[6] + "&nbsp;";
            OLG8.Text = "&nbsp;" + labels[7] + "&nbsp;";
            OLG9.Text = "&nbsp;" + labels[8] + "&nbsp;";
            OLG10.Text = "&nbsp;" + labels[9] + "&nbsp;";
            OLG11.Text = "&nbsp;" + labels[10] + "&nbsp;";
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
			SetAnswerLabels(new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" });
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