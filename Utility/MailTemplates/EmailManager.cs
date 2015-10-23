using System;
using System.Net.Mail;

namespace Utility.MailTemplates
{
    
    public static class EmailManager
    {

        private static string sMailSendTo = string.Empty;
        private static string sMailCcTo = string.Empty;
        private static string sMailBccTo = string.Empty;
        private static string sMailSubject = string.Empty;
        private static string sMailBody = string.Empty;


        /// <summary>
        /// This method used to send mail only by using mailto ,smtp server  information
        /// </summary>
        /// <param name="emailSendTo"></param>
        /// <param name="SmtpServer"></param>
        /// <returns></returns>
        public static bool SendMail(string sEmailSendTo, string SmtpAddress)
        {
            bool sendStatus = false;

            try
            {
                sendStatus = SendMail(sEmailSendTo, sMailCcTo, sMailBccTo, sMailSubject, sMailBody, SmtpAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return sendStatus;
        }






        /// <summary>
        /// This method used to send mail by using mailto,mailbody,smtp server information
        /// </summary>
        /// <param name="emailSendTo"></param>
        /// <param name="emailBody"></param>
        /// <param name="SmtpServer"></param>
        /// <returns></returns>
        public static bool SendMail(string sEmailSendTo, string sEmailBody, string SmtpAddress)
        {
            bool sendStatus = false;

            try
            {
                sendStatus = SendMail(sEmailSendTo, sMailCcTo, sMailBccTo, sMailSubject, sEmailBody, SmtpAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return sendStatus;
        }



        /// <summary>
        /// This method used to send mail by using mailto,mailSubject,mailbody,smtp server information
        /// </summary>
        /// <param name="emailSendTo"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <param name="SmtpServer"></param>
        /// <returns></returns>
        public static bool SendMail(string sEmailSendTo, string sEmailSubject, string sEmailBody, string SmtpAddress)
        {
            bool sendStatus = false;

            try
            {
                sendStatus = SendMail(sEmailSendTo, sMailCcTo, sMailBccTo, sEmailSubject, sEmailBody, SmtpAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return sendStatus;
        }



        /// <summary>
        /// This method used to send mail by using mailto,mailCcTo,mailSubject,mailbody,smtp server information
        /// </summary>
        /// <param name="emailSendTo"></param>
        /// <param name="emailCcTo"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <param name="SmtpServer"></param>
        /// <returns></returns>
        public static bool SendMail(string sEmailSendTo, string sEmailCcTo, string sEmailSubject, string sEmailBody, string SmtpAddress)
        {
            bool bSendStatus = false;

            try
            {
                bSendStatus = SendMail(sEmailSendTo, sEmailCcTo, sMailBccTo, sEmailSubject, sEmailBody, SmtpAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bSendStatus;
        }





        /// <summary>
        /// This method is the root method for sending mail containing all information-To,cc,Bcc,Subject,body,smtpserver information
        /// </summary>
        /// <param name="emailSendTo"></param>
        /// <param name="emailCcTo"></param>
        /// <param name="emailBccTo"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <param name="SmtpServer"></param>
        /// <returns></returns>
        public static bool SendMail(string sEmailSendTo, string sEmailCcTo, string sEmailBccTo, string sEmailSubject, string sEmailBody, string SmtpAddress)
        {
            bool bSendStatus = false;

            try
            {
                MailMessage objMail = new MailMessage();

                objMail.To.Add(sEmailSendTo);
                objMail.CC.Add(sEmailCcTo);
                objMail.CC.Add(sEmailBccTo);
                objMail.Subject = sEmailSubject;
                objMail.Body = sEmailBody;

                SmtpClient objSmtp = new System.Net.Mail.SmtpClient(SmtpAddress);

                objSmtp.Send(objMail);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            bSendStatus = true;

            return bSendStatus;
        }

    }
}
