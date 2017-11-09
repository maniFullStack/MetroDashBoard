using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace SharedClasses {
    [ToolboxData( "<{0}:MessageManager runat=server></{0}:MessageManager>" )]
    public class MessageManager : WebControl, INamingContainer {
        public enum MessageManagerDisplayType {
            Default,
            Error,
            Info,
            Success,
            Alert,
            Warning
        }

        [Bindable( true )]
        [Category( "Appearance" )]
        [DefaultValue( "" )]
        [Localizable( true )]
        public string ErrorMessage { get; set; }
        [Bindable( true )]
        [Category( "Appearance" )]
        [DefaultValue( "" )]
        [Localizable( true )]
        public string InfoMessage { get; set; }
        [Bindable( true )]
        [Category( "Appearance" )]
        [DefaultValue( "" )]
        [Localizable( true )]
        public string SuccessMessage { get; set; }
        [Bindable( true )]
        [Category( "Appearance" )]
        [DefaultValue( "" )]
        [Localizable( true )]
        public string FrSuccessMessage { get; set; }
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string AlertMessage { get; set; }
        [Bindable( true )]
        [Category( "Appearance" )]
        [DefaultValue( "" )]
        [Localizable( true )]
        public string WarningMessage { get; set; }
        [Bindable( true )]
        [Category( "Appearance" )]
        [DefaultValue( "" )]
        [Localizable( true )]
        public string TitleOverride { get; set; }
        [Bindable( true )]
        [Category( "Appearance" )]
        [DefaultValue( "" )]
        [Localizable( true )]
        public bool ShowCloseButton { get; set; }
        [Bindable( true )]
        [Category( "Appearance" )]
        [DefaultValue( "" )]
        [Localizable( true )]
        public string CSSStyle { get; set; }
        public MessageManagerDisplayType DisplayAs { get; set; }

        public bool IsVisible {
            get {
                if ( !String.IsNullOrEmpty( ErrorMessage ) || DisplayAs == MessageManagerDisplayType.Error ) {
                    return true;
                }
                if ( !String.IsNullOrEmpty( AlertMessage ) || DisplayAs == MessageManagerDisplayType.Alert ) {
                    return true;
                }
                if ( !String.IsNullOrEmpty( WarningMessage ) || DisplayAs == MessageManagerDisplayType.Warning ) {
                    return true;
                }
                if ( !String.IsNullOrEmpty( InfoMessage ) || DisplayAs == MessageManagerDisplayType.Info ) {
                    return true;
                }
                if ( !String.IsNullOrEmpty( SuccessMessage ) || DisplayAs == MessageManagerDisplayType.Success ) {
                    return true;
                }
                return false;
            }
        }

        [TemplateContainer( typeof( MessageManager ) )]
        [PersistenceMode( PersistenceMode.InnerProperty )]
        public ITemplate Message { get; set; }

        [Bindable( false )]
        [Localizable( false )]
        public string Contents { get; private set; }

        protected override void OnPreRender( EventArgs e ) {
            base.OnPreRender( e );
            if ( Message != null ) {
                Control temp = new Control();
                Message.InstantiateIn( temp );
                StringBuilder sb = new StringBuilder();
                using ( StringWriter stringWriter = new StringWriter( sb ) ) {
                    using ( HtmlTextWriter textWriter = new HtmlTextWriter( stringWriter ) ) {
                        temp.RenderControl( textWriter );
                    }
                }
                Contents = sb.ToString();
            }
        }


        protected override void RenderContents( HtmlTextWriter output ) {
            if ( !String.IsNullOrEmpty( ErrorMessage ) || DisplayAs == MessageManagerDisplayType.Error ) {
                WriteTemplate( "alert alert-danger", "glyphicon glyphicon-remove-circle", "Error", ErrorMessage, output );
            }
            if ( !String.IsNullOrEmpty( AlertMessage ) || DisplayAs == MessageManagerDisplayType.Alert ) {
                WriteTemplate( "alert alert-danger", "glyphicon glyphicon-alert", "Alert", AlertMessage, output );
            }
            if ( !String.IsNullOrEmpty( WarningMessage ) || DisplayAs == MessageManagerDisplayType.Warning ) {
                WriteTemplate( "alert alert-warning", "glyphicon glyphicon-warning-sign", "Warning", WarningMessage, output );
            }
            if ( !String.IsNullOrEmpty( InfoMessage ) || DisplayAs == MessageManagerDisplayType.Info ) {
                WriteTemplate( "alert alert-info", "glyphicon glyphicon-exclamation-sign", "Information", InfoMessage, output );
            }
            if ( !String.IsNullOrEmpty( SuccessMessage ) || DisplayAs == MessageManagerDisplayType.Success ) {
                WriteTemplate( "alert alert-success", "glyphicon glyphicon-ok", "Success!", SuccessMessage, output );
            }
            if (!String.IsNullOrEmpty(FrSuccessMessage) || DisplayAs == MessageManagerDisplayType.Success)
            {
                WriteTemplate("alert alert-success", "glyphicon glyphicon-ok", "C’est terminé!", FrSuccessMessage, output);
            }
        }

        private void WriteTemplate( string mainClasses, string iconClasses, string label, string message, HtmlTextWriter output ) {
            output.WriteBeginTag( "div" );
            output.WriteAttribute( "class", mainClasses );
            output.WriteAttribute( "style", CSSStyle );
            output.Write( HtmlTextWriter.TagRightChar );
            if (ShowCloseButton) {
                output.Write( "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>" );
            }
            output.WriteFullBeginTag( "p" );
            output.WriteBeginTag( "span" );
            output.WriteAttribute( "class", iconClasses );
            output.Write( HtmlTextWriter.TagRightChar );
            output.WriteEndTag( "span" );
            output.WriteFullBeginTag( "strong" );
            output.Write( " " + ( TitleOverride != null ? TitleOverride : label ) );
            output.WriteEndTag( "strong" );
            output.WriteBreak();
            output.WriteFullBeginTag( "span" );
            output.Write( String.IsNullOrEmpty( Contents ) ? message : Contents );
            output.WriteEndTag( "span" );
            output.WriteEndTag( "p" );
            output.WriteEndTag( "div" );
        }
    }
}