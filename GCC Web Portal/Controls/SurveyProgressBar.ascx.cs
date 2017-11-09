using System;

namespace GCC_Web_Portal.Controls
{
    public partial class SurveyProgressBar : System.Web.UI.UserControl
    {
        public int MaxValue { get; set; }
        public int CurrentValue { get; set; }

        public int Percentage
        {
            get
            {
                double val = ((double)CurrentValue / (double)MaxValue) * 100.0;
                val = Math.Min(val, 100); //Make sure we don't go over 100%
                return (int)Math.Round(val);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}