using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsiteUtilities;

namespace SharedClasses {
    public class ReportFilterListBox : ListBox, IReportFilter {
        public string Label { get; set; }
        public string SessionKey { get; set; }
        public string DBColumn { get; set; }
        public MessageManager MessageManager { get; private set; }
        public bool IsActive {
            get {
                return SelectedIndex != -1;
            }
        }
        public bool IsNumericTableType { get; set; }
        public string GetSelectedFilterText() {
            StringBuilder sb = new StringBuilder();
            foreach ( ListItem li in Items ) {
                if ( li.Selected ) {
                    sb.AppendFormat( ", {0}", li.Text );
                }
            }
            if ( sb.Length > 2 ) {
                sb.Remove( 0, 2 );
            }
            return sb.ToString();
        }

        public void Save() {
            List<string> selected = new List<string>();
            foreach ( ListItem li in Items ) {
                if ( li.Selected ) {
                    selected.Add( li.Value );
                }
            }
            SessionWrapper.Add( SessionKey, selected );
        }

        public void Clear() {
            if ( OnClear != null && OnClear() ) {
                return;
            }
            SessionWrapper.Remove( SessionKey );
            SelectedIndex = -1;
        }

        public Action<ReportFilterListBox, SQLParamList> ReplaceAddToQuery { get; set; }

        /// <summary>
        /// Runs when the control is cleared. Return true to cancel without clearing it.
        /// </summary>
        public Func<bool> OnClear { get; set; }

        protected override void OnLoad( EventArgs e ) {
            base.OnLoad( e );
            if ( !String.IsNullOrEmpty( SessionKey ) && SelectedIndex == -1 ) {
                List<string> selectedValues = SessionWrapper.Get( SessionKey, new List<string>() );
                foreach ( string str in selectedValues ) {
                    foreach ( ListItem li in Items ) {
                        if ( li.Value.Equals( str ) ) {
                            li.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        public void AddToQuery( SQLParamList sqlParams ) {
            if ( ReplaceAddToQuery != null ) {
                ReplaceAddToQuery( this, sqlParams );
            } else {
                if ( IsActive ) {
                    if ( IsNumericTableType ) {
                        DataTable dt = new DataTable();
                        dt.Columns.Add( "Number", typeof( Int32 ) );
                        foreach ( int i in GetSelectedIndices() ) {
                            DataRow dr = dt.NewRow();
                            dr["Number"] = Conversion.StringToInt(Items[i].Value, -1);
                            dt.Rows.Add(dr);
                        }
                        SqlParameter sqlP = new SqlParameter("@" + DBColumn, dt);
                        sqlP.SqlDbType = SqlDbType.Structured;
                        sqlParams.Add( sqlP );
                    } else {
                        StringBuilder sb = new StringBuilder();
                        foreach ( int i in GetSelectedIndices() ) {
                            sb.AppendFormat( ",{0}", Items[i].Value );
                        }
                        if ( sb.Length > 1 ) {
                            sb.Remove( 0, 1 );
                        }
                        sqlParams.Add( "@" + DBColumn, sb.ToString() );
                    }
                }
            }
        }

        public ReportFilterListBox() {
            MessageManager = new MessageManager();
        }

        protected override void Render( HtmlTextWriter writer ) {
            MessageManager.RenderControl( writer );
            base.Render( writer );
        }

    }
}
