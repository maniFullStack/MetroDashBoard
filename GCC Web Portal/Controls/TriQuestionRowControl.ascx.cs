using SharedClasses;
using System;
using System.Text;
using WebsiteUtilities;

namespace GCC_Web_Portal.Controls
{
    public partial class TriQuestionRowControl : System.Web.UI.UserControl, ISurveyControl<int>
    {
        /// <summary>
        /// Gets or sets whether to show the last column, "Don't Know". Defaults to false (don't show it).
        /// </summary>
        public bool ShowNAColumn { get; set; }

        /// <summary>
        /// Returns true if the label for this row is not null or blank.
        /// </summary>
        protected bool HasLabel
        {
            get
            {
                return !String.IsNullOrEmpty(Label);
            }
        }

        /// <summary>
        /// Gets or sets the selected value of the control. 0 is considered "Don't Know" and 5 is "Excellent" (or equivalent name). If nothing is selected, -1 will be returned.
        /// </summary>
        public int SelectedValue
        {
            get
            {
                if (radAnswer0.Checked)
                {
                    return 0;
                }
                else if (radAnswer1.Checked)
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
                else
                {
                    return -1;
                }
            }
            set
            {
                radAnswer0.Checked = (value == 0) && ShowNAColumn;
                radAnswer1.Checked = (value == 1);
                radAnswer2.Checked = (value == 2);
                radAnswer3.Checked = (value == 3);
            }
        }

        /// <summary>
        /// Gets or sets the label shown in the left column.
        /// </summary>
        public string Label
        {
            get
            {
                return lblRowLabel.Text;
            }
            set
            {
                lblRowLabel.Text = value;
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
        /// Sets the answer labels. Must be an array of 4 items from "Don't Know" to "Excellent".
        /// </summary>
        /// <param name="labels"></param>
        public void SetAnswerLabels(string[] labels)
        {
            radAnswer0.Text = labels[0];
            radAnswer1.Text = labels[1];
            radAnswer2.Text = labels[2];
            radAnswer3.Text = labels[3];
        }

        public TriQuestionRowControl()
        {
            ShowNAColumn = false;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SetAnswerLabels(new string[] { "Don't Know / N/A", "Not Likely", "Possibly", "Very Likely" });
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