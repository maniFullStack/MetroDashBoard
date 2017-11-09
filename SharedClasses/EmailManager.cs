using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using WebsiteUtilities;
using System.Net.Mime;
using System.Text;

namespace SharedClasses {
    public class EmailManager {
        /// <summary>
        /// Generates email message based on a template 
        /// </summary>
        /// <typeparam name="T">Model type for template string replacements</typeparam>
        /// <param name="template">path to template file relative to the executable</param>
        /// <param name="model">Model Data for template replacements - only properties in a readable scope are used in replacements(privates are hidden from replacements)</param>
        /// <returns></returns>
        public static MailMessage CreateEmailFromTemplate<T>( string template, T model )
            where T : class {


            ReplaceTemplate tpl = new ReplaceTemplate( template );
            var props = model.GetType().GetProperties().Where( q => q.CanRead );
            foreach ( PropertyInfo p in props ) {
                tpl.AddReplacementValue( p.Name, GetPropertyValue( model, p.Name ).ToString() );
            }

            MailMessage msg = new MailMessage {
                Body = tpl.GetTemplate()
            };

            //optional AppSettings values 

            SetOptionalDefaults( msg );


            return msg;
        }
        public static MailMessage CreateEmailFromTemplate<T>( string htmlTemplateFile, string textTemplateFile, T model )
            where T : class {


            ReplaceTemplate htmlTpl = new ReplaceTemplate( htmlTemplateFile );
            ReplaceTemplate txtTpl = new ReplaceTemplate( textTemplateFile );
            var props = model.GetType().GetProperties().Where( q => q.CanRead );
            foreach ( PropertyInfo p in props ) {
                txtTpl.AddReplacementValue( p.Name, GetPropertyValue( model, p.Name ).ToString() );
                htmlTpl.AddReplacementValue( p.Name, GetPropertyValue( model, p.Name ).ToString() );
            }

            MailMessage msg = new MailMessage();

            msg.AlternateViews.Add( AlternateView.CreateAlternateViewFromString( htmlTpl.GetTemplate(), null, MediaTypeNames.Text.Html ) );
            msg.AlternateViews.Add( AlternateView.CreateAlternateViewFromString( txtTpl.GetTemplate(), null, MediaTypeNames.Text.Plain ) );

            //optional AppSettings values 

            SetOptionalDefaults( msg );


            return msg;
        }

        private static void SetOptionalDefaults( MailMessage msg ) {
            try {
                if ( ConfigurationManager.AppSettings["MailFromAddress"] != null
                 && ConfigurationManager.AppSettings["MailFromName"] != null ) {
                    msg.From = new MailAddress(
                        ConfigurationManager.AppSettings["MailFromAddress"],
                        ConfigurationManager.AppSettings["MailFromName"]
                        );
                }
            } catch ( ArgumentNullException ex ) {
                msg.From = new MailAddress( "noreply@gcgamingsurvey.com", "Gaming Survey" );
            }
        }

        private static object GetPropertyValue( object src, string propName ) {
            return src.GetType().GetProperty( propName ).GetValue( src, null );
        }
    }


    public static class MailExtensions {
        public static void Send( this MailMessage msg ) {
            SmtpClient smtp = new SmtpClient();
            smtp.Send( msg );
        }
    }
}