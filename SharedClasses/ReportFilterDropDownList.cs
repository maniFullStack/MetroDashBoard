using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace SharedClasses {
    public class ReportFilterDropDownList : DropDownList, IReportFilter {
        public string Label { get; set; }
        public string SessionKey { get; set; }
        public string DBColumn { get; set; }
        public MessageManager MessageManager { get; private set; }
        public bool IsActive {
            get {
                return SelectedIndex != -1;
            }
        }
        public string GetSelectedFilterText() {
            return SelectedItem.Text;
        }

        public void Save() {
            SessionWrapper.Add( SessionKey, SelectedIndex );
        }

        public void Clear() {
            SessionWrapper.Remove( SessionKey );
            SelectedIndex = -1;
        }

        protected override void OnLoad( EventArgs e ) {
            base.OnLoad( e );
            if ( !String.IsNullOrEmpty( SessionKey ) && SelectedIndex == -1 ) {
                SelectedIndex = SessionWrapper.Get( SessionKey, -1 );
            }
        }

        public void AddToQuery( SQLParamList sqlParams ) {
            if ( IsActive ) {
                sqlParams.Add( "@" + DBColumn, SelectedValue );
            }
        }

        public ReportFilterDropDownList() {
            MessageManager = new MessageManager();
        }

        protected override void Render( HtmlTextWriter writer ) {
            MessageManager.RenderControl( writer );
            base.Render( writer );
        }
    }
}
