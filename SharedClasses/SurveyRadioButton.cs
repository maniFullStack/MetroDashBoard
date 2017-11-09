using System;
using System.Text;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace SharedClasses {
    public class SurveyRadioButton : RadioButton, ISurveyControl<bool> {
        public string SessionKey { get; set; }
        public string DBColumn { get; set; }
        public string DBValue { get; set; }

        public bool GetValue() {
            return Checked;
        }
        
        public MessageManager MessageManager { get; set; }
        public SurveyRadioButton() {
            MessageManager = new MessageManager();
        }

        protected override void OnLoad( EventArgs e ) {
            base.OnLoad( e );
            if ( !String.IsNullOrEmpty( SessionKey ) && !Checked ) {
                var sVal = SessionWrapper.Get<SurveySessionControl<bool>>( SessionKey, null );
                if ( sVal != null ) {
                    Checked = sVal.Value;
                }
            }
        }

        protected override void Render( System.Web.UI.HtmlTextWriter writer ) {
            MessageManager.RenderControl( writer );
            base.Render( writer );
        }
        public void PrepareQuestionForDB( StringBuilder columnList, SQLParamList sqlParams ) {
            if ( Checked && !String.IsNullOrEmpty( DBColumn ) ) {
                columnList.AppendFormat( ",[{0}]", DBColumn );
                sqlParams.Add( "@" + DBColumn, DBValue );
            }
        }
    }
}
