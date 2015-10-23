using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Net.Mail;
using System.Text;
using Utility.MailTemplates;

namespace Utility.Common
{
    /// <summary>
    ///     Utility class
    /// </summary>
    public class Utility
    {
        #region "Create New Password"

        public static string GenerateNewPassword(string empCode)
        {
            return string.Empty; //Wireup Generate password code
        }

        #endregion

        #region "Emails Notifications"

        public static void SendNotificationTouserForPasswordChanges(string userName, string userEmail,
            string newPassword, MailTemplate.ETemplateList mailTemplate)
        {
            SendForgetPasswordEmail(userName, userEmail, newPassword, mailTemplate);
        }

        #endregion

        #region "IList To DataSet Conversion"

        /// <summary>
        ///     Converts List for sepcific type to Dataset
        /// </summary>
        /// <typeparam name="T">Type of Entity for a table exists in DB </typeparam>
        /// <param name="list">List of Generic collection for Type T</param>
        /// <returns>DataSet based on supplied Generic List</returns>
        public static DataSet ToDataSet<T>(IList<T> list)
        {
            var elementType = typeof (T);
            var ds = new DataSet();
            var t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T 
            foreach (var propInfo in elementType.GetProperties())
            {
                var colType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                // colType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;


                t.Columns.Add(propInfo.Name, colType);
            }

            //go through each property on T and add each value to the table 
            foreach (var item in list)
            {
                var row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }

        #endregion

        #region Date Comparison functions

        public static bool IsGreaterThan(DateTime dateFirst, DateTime dateSecond)
        {
            var timeSpan = GetTimeSpan(dateFirst, dateSecond);
            return timeSpan.Ticks > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="dateFirst"></param>
        /// <param name="dateSecond"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool IsGreaterThan(DateTime dateFirst, DateTime? dateSecond)
        {
            var timeSpan = new TimeSpan();

            if (dateSecond.HasValue)
            {
                timeSpan = GetTimeSpan(dateFirst, Convert.ToDateTime(dateSecond));
            }
            return timeSpan.Ticks > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="dateFirst"></param>
        /// <param name="dateSecond"></param>
        /// <returns></returns>
        public static bool IsLessThan(DateTime dateFirst, DateTime dateSecond)
        {
            var timeSpan = GetTimeSpan(dateFirst, dateSecond);
            return timeSpan.Ticks < 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="dateFirst"></param>
        /// <param name="dateSecond"></param>
        /// <returns></returns>
        public static bool IsEqualTo(DateTime dateFirst, DateTime dateSecond)
        {
            var timeSpan = GetTimeSpan(dateFirst, dateSecond);
            return timeSpan.Ticks == 0;
        }

        #endregion

        #region "Private Helper Methods"

        private static void SendForgetPasswordEmail(string userName, string userEmail, string newPassword,
            MailTemplate.ETemplateList mailTemplate)
        {
            MailMessage message;
            var objService = InitializeEmailService(out message, userEmail);

            var objTemplateMgr = GetMailTemplate(mailTemplate);

            message.Subject = objTemplateMgr.MailSubject.Replace("UserName~", userName);

            var messageBody = new StringBuilder(objTemplateMgr.MailBody);
            messageBody.Replace("UserName~", userName);
            messageBody.Replace("NewPassword~", newPassword);

            message.IsBodyHtml = true;

            message.Body = messageBody.ToString();
            objService.Send(message);
        }

        private static EmailService InitializeEmailService(out MailMessage message, string emailId)
        {
            var objService = new EmailService();
            IEmailSettings objSettings = new EmailServiceSettings
            {
                AuthenticationUserName =
                    ConfigurationManager.AppSettings["SMTPAuthenticationUserName"],
                AuthenticationPassword =
                    ConfigurationManager.AppSettings["SMTPAuthenticationPassword"],
                From =
                    ConfigurationManager.AppSettings["SMTPAuthenticationUserName"],
                IsAuthenticationRequired = true,
                Port = Convert.ToInt16(ConfigurationManager.AppSettings["SmptPort"]),
                SmptServer = ConfigurationManager.AppSettings["SmtpServer"],
                UsePort = true,
                UseDefaultCredentials = "Y",
                EnableSSL = "Y"
            };

            objService.Settings = objSettings;
            message = new MailMessage
            {
                From =
                    new MailAddress(ConfigurationManager.AppSettings["FromEmailID"])
            };


            emailId = GetDefaultEmailIdIfNeedToSendMailOnDefaultEmailId(emailId);

            message.To.Add(new MailAddress(emailId));
            return objService;
        }

        private static string GetDefaultEmailIdIfNeedToSendMailOnDefaultEmailId(string emailId)
        {
            if (ConfigurationManager.AppSettings["SendDefaultMail"] == "Y")
                emailId = ConfigurationManager.AppSettings["DefaultMail"];
            return emailId;
        }

        private static MailTemplate GetMailTemplate(MailTemplate.ETemplateList mailTemplate)
        {
            return new MailTemplate(mailTemplate);
        }

        private static TimeSpan GetTimeSpan(DateTime dateFirst, DateTime dateSecond)
        {
            return dateFirst - dateSecond;
        }

        #endregion
    }
}