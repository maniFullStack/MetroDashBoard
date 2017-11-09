using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace WebsiteUtilities
{
    /// <summary>
    /// Log Emails to Database
    /// </summary>
    public class EmailLog
    {
        /// <summary>
        /// Take data from msg for log
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool LogEmail(MailMessage msg)
        {
            string emailTo = msg.To.ToString();
            string emailFrom = msg.From.ToString();
            string subject = msg.Subject.ToString();
            string project = msg.Body.ToString();

            SQLParamList sqlParams = new SQLParamList()
                        .Add("@To", emailTo)
                        .Add("@From", emailFrom)
                        .Add("@Subject", subject)
                        .Add("@Project", project)
                        .Add("@Timestamp", System.DateTime.Now.ToString());

            SQLDatabaseWeb sql = new SQLDatabaseWeb();

            int logEmail = sql.NonQuery(
                                    "INSERT INTO [dbo].[EmailLog] (toEmail, fromEmail, emailSubject, project, timestamp) VALUES (@To, @From, @Subject, @Project, @Timestamp)",
                                    sqlParams);

            if (!sql.HasError && logEmail > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }



}
