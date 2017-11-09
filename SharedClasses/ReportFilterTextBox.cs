using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace SharedClasses {
    public class ReportFilterTextBox : TextBox, IReportFilter {
        /// <summary>
        /// The label shown beside the box.
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// The session key value.
        /// </summary>
        public string SessionKey { get; set; }
        /// <summary>
        /// The database column affected by this filter.
        /// </summary>
        public string DBColumn { get; set; }
        public bool IsActive {
            get {
                return !String.IsNullOrEmpty( Text );
            }
        }
        /// <summary>
        /// If true, the value will be matched exactly, otherwise, a LIKE clause will be applied with surrounding %'s. '%' and '_' will be automatically escaped.
        /// </summary>
        //public bool IsExactSearch { get; set; }

        public Action<ReportFilterTextBox, SQLParamList> ReplaceAddToQuery { get; set; }
        public string GetSelectedFilterText() {
            return Text;
        }

        public void Save() {
            SessionWrapper.Add( SessionKey, Text );
        }

        public void Clear() {
            SessionWrapper.Remove( SessionKey );
            Text = String.Empty;
        }
        protected override void OnLoad( EventArgs e ) {
            base.OnLoad( e );
            if ( !String.IsNullOrEmpty( SessionKey ) && String.IsNullOrEmpty( Text ) ) {
                string ret = SessionWrapper.Get( SessionKey, String.Empty);
                if ( !String.IsNullOrEmpty( ret ) ) {
                    Text = ret;
                }
            }
        }

        public void AddToQuery( SQLParamList sqlParams ) {
            if ( ReplaceAddToQuery != null ) {
                ReplaceAddToQuery( this, sqlParams );
            } else {
                if ( IsActive ) {
                    //if ( IsExactSearch ) {
                    //    sqlParams.Add( "@" + DBColumn, Text );
                    //} else {
                    sqlParams.Add( "@" + DBColumn, Text.Replace( @"\", @"\\" ).Replace( @"%", @"\%" ).Replace( @"_", @"\_" ) );
                    //}
                }
            }
        }

        public MessageManager MessageManager { get; private set; }

        public ReportFilterTextBox() {
            MessageManager = new MessageManager();
        }
        protected override void Render( System.Web.UI.HtmlTextWriter writer ) {
            MessageManager.RenderControl( writer );
            base.Render( writer );
        }
    }
}