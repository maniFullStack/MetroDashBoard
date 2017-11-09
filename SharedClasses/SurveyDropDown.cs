using System;
using System.Text;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace SharedClasses {
    public class SurveyDropDown : DropDownList, ISurveyControl<string> {
        public string SessionKey { get; set; }
        public string DBColumn { get; set; }
        public string DBValue {
            get {
                return SelectedValue;
            }
        }

        public string GetValue() {
            return SelectedValue;
        }

        public MessageManager MessageManager { get; set; }
        public SurveyDropDown() {
            MessageManager = new MessageManager();
        }

        protected override void OnLoad( EventArgs e ) {
            base.OnLoad( e );
            if ( !String.IsNullOrEmpty( SessionKey ) && String.IsNullOrEmpty( SelectedValue ) ) {
                var sVal = SessionWrapper.Get<SurveySessionControl<string>>( SessionKey, null );
                if ( sVal != null ) {
                    SelectedValue = sVal.Value;
                }
            }
        }

        protected override void Render( System.Web.UI.HtmlTextWriter writer ) {
            MessageManager.RenderControl( writer );
            base.Render( writer );
        }
        public void PrepareQuestionForDB( StringBuilder columnList, SQLParamList sqlParams ) {
            columnList.AppendFormat( ",[{0}]", DBColumn );
            sqlParams.Add( "@" + DBColumn, GetValue() );
        }
    }
}
