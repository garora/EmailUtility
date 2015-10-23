using System;
using System.Collections;
using System.Configuration;
using System.Net.Mail;

namespace Utility.MailTemplates
{
    public class EmailService : IEmailService
    {
        private IEmailSettings _config;



        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        public EmailService()
        {
            _config = new EmailServiceSettings();
            this._config.AuthenticationPassword = ConfigurationSettings.AppSettings["SMTPAuthenticationPassword"].ToString();
            this._config.AuthenticationUserName = ConfigurationSettings.AppSettings["SMTPAuthenticationUserName"].ToString();
            this._config.SmptServer = ConfigurationSettings.AppSettings["SmtpServer"].ToString();
            this._config.Port = Convert.ToInt16(ConfigurationSettings.AppSettings["SmtpPort"]);
            this._config.EnableSSL = ConfigurationSettings.AppSettings["EnableSSL"].ToString();
            this._config.UseDefaultCredentials = ConfigurationSettings.AppSettings["UseDefaultCredentials"].ToString();
            if (ConfigurationSettings.AppSettings["SmtpPort"].ToString() == "")
            {
                this._config.UsePort = false;
            }
            this._config.IsAuthenticationRequired = true;
        }



        /// <summary>
        /// Initialize configuration using the configuration source supplied.
        /// The email service settings must be in a section named "EmailService".
        /// </summary>
        /// <param name="config"></param>
        /// <param name="emailServiceSectionName"></param>
        public EmailService(IDictionary config, string emailServiceSectionName)
        {
            var settings = EmailHelper.GetSettings(config, emailServiceSectionName);

            Init(settings);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="settings">The config.</param>
        public EmailService(IEmailSettings settings)
        {
            Init(settings);
        }


        /// <summary>
        /// Initialize the configuration.
        /// </summary>
        /// <param name="config"></param>
        public void Init(IEmailSettings config)
        {
            _config = config;
        }



        #region IEmailService Members
        /// <summary>
        /// The email service configuration object.
        /// </summary>
        public IEmailSettings Settings
        {
            get { return _config; }
            set { _config = value; }
        }


        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public bool Send(EmailMessage message)
        {
            MailMessage mailMessage = new MailMessage(message.From, message.To, message.Subject, message.Body);
            mailMessage.IsBodyHtml = message.IsHtml;
            return InternalSend(mailMessage, true, _config.AuthenticationUserName, _config.AuthenticationPassword);
        }


        /// <summary>
        /// Sends the message using the credentials and host/port supplied.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="useCredentials"></param>
        /// <param name="credentialsUser"></param>
        /// <param name="credentialsPassword"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        //public BoolMessageEx Send(EmailMessage message, bool useCredentials, string credentialsUser, string credentialsPassword, string host, int port)
        //{
        //    MailMessage mailMessage = new MailMessage(message.From, message.To, message.Subject, message.Body);
        //    mailMessage.IsBodyHtml = message.IsHtml;
        //    return InternalSendAndGetResult(mailMessage, useCredentials, credentialsUser, credentialsPassword, host, port);
        //}


        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="credentialsUser">The credentials user.</param>
        /// <param name="credentialsPassword">The credentials password.</param>
        /// <returns></returns>
        public bool Send(NotificationMessage message, string credentialsUser, string credentialsPassword)
        {
            MailMessage mailMessage = new MailMessage(_config.From, message.To, message.Subject, message.Body);
            mailMessage.IsBodyHtml = message.IsHtml;
            return InternalSend(mailMessage, true, credentialsUser, credentialsPassword);
        }


        /// <summary>
        /// Sends the mail message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool Send(NotificationMessage message)
        {
            MailMessage mailMessage = new MailMessage(_config.From, message.To, message.Subject, message.Body);
            mailMessage.IsBodyHtml = message.IsHtml;
            return InternalSend(mailMessage, Settings.IsAuthenticationRequired, Settings.AuthenticationUserName, Settings.AuthenticationPassword);
        }


        /// <summary>
        /// Mail the message using the native MailMessage class.
        /// </summary>
        /// <param name="message">The mail message</param>
        /// <param name="useCredentials">Whether or not to use credentials for security when sending emails.</param>
        /// <param name="credentialsUser">User name when using credentials.</param>
        /// <param name="credentialsPassword">Password when using credentials.</param>
        /// <returns></returns>
        public bool Send(MailMessage message, bool useCredentials, string credentialsUser, string credentialsPassword)
        {
            return InternalSend(message, useCredentials, credentialsUser, credentialsPassword);
        }


        /// <summary>
        /// Mail the message using the native MailMessage class.
        /// </summary>
        /// <param name="from">Who the email is from.</param>
        /// <param name="to">Who the email is being sent to.</param>
        /// <param name="subject">Subject of email.</param>
        /// <param name="body">Email body.</param>
        /// <param name="useCredentials">Whether or not to use credentials for security when sending emails.</param>
        /// <param name="credentialsUser">User name when using credentials.</param>
        /// <param name="credentialsPassword">Password when using credentials.</param>
        /// <returns></returns>
        public bool Send(string from, string to, string subject, string body,
            bool useCredentials, string credentialsUser, string credentialsPassword)
        {
            MailMessage message = new MailMessage(from, to, subject, body);
            message.IsBodyHtml = true;
            return InternalSend(message, useCredentials, credentialsUser, credentialsPassword);
        }


        /// <summary>
        /// Mail the message using the native MailMessage class and the credentials from the current configuration.
        /// </summary>
        /// <param name="mailMessage">The mail message</param>
        /// <returns></returns>
        public bool Send(MailMessage mailMessage)
        {
            return InternalSend(mailMessage, Settings.IsAuthenticationRequired, Settings.AuthenticationUserName, Settings.AuthenticationPassword);
        }
        #endregion



        /// <summary>
        /// Internals the send.
        /// </summary>
        /// <param name="mailMessage">The message.</param>
        /// <param name="useCredentials">if set to <c>true</c> [use credentials].</param>
        /// <param name="credentialsUser">The credentials user.</param>
        /// <param name="credentialsPassword">The credentials password.</param>
        /// <returns></returns>
        private bool InternalSend(MailMessage mailMessage, bool useCredentials, string credentialsUser, string credentialsPassword)
        {
            return InternalSend(mailMessage, useCredentials, credentialsUser, credentialsPassword, _config.SmptServer, _config.Port);
        }


        /// <summary>
        /// Internals the send.
        /// </summary>
        /// <param name="mailMessage">The message.</param>
        /// <param name="useCredentials">if set to <c>true</c> [use credentials].</param>
        /// <param name="smtpuser">The credentials user.</param>
        /// <param name="smtppassword">The credentials password.</param>
        /// <param name="smtphost">Smtp Host</param>
        /// <param name="smtpport">Port number</param>
        /// <returns></returns>
        private bool InternalSend(MailMessage mailMessage, bool useCredentials, string smtpuser, string smtppassword, string smtphost, int smtpport)
        {
            var result = InternalSendAndGetResult(mailMessage, useCredentials, smtpuser, smtppassword, smtphost, smtpport);
            return result.Success;
        }


        private BoolMessageEx InternalSendAndGetResult(MailMessage mailMessage, bool useCredentials, string smtpuser, string smtppassword, string smtphost, int smtpport)
        {
            bool sent = true;
            string message = string.Empty;
            Exception ex = null;
            try
            {
                string host = string.IsNullOrEmpty(smtphost) ? _config.SmptServer : smtphost;
                string authUser = string.IsNullOrEmpty(smtpuser) ? _config.AuthenticationUserName : smtpuser;
                string authPass = string.IsNullOrEmpty(smtppassword) ? _config.AuthenticationPassword : smtppassword;
                int port = smtpport == _config.Port ? _config.Port : smtpport;

                System.Net.Mail.SmtpClient client = null;

                if (_config.UsePort)
                {
                    client = new SmtpClient(host, port);
                }
                else
                {
                    client = new SmtpClient(host);
                }
                if (useCredentials)
                {
                    if (this._config.UseDefaultCredentials == "Y")
                    {
                        client.UseDefaultCredentials = true;
                    }
                    else
                    {
                        client.UseDefaultCredentials = false;
                    }
                    client.Credentials = new System.Net.NetworkCredential(authUser, authPass);
                }
                else
                {
                    client.UseDefaultCredentials = false;
                }

                if (this._config.EnableSSL == "Y")
                {
                    client.EnableSsl = true;
                }
                else
                {
                    client.EnableSsl = false;
                }

                client.Send(mailMessage);
            }
            catch (Exception exception)
            {
                //if(_logger != null) _logger.Error("Unable to send email.", exception, null);
                ex = exception;
                message = ex.Message;
                sent = false;
            }
            return new BoolMessageEx(sent, ex, message);
        }

    }

    internal class BoolMessageEx
    {
        public BoolMessageEx(bool sent, Exception ex, string message)
        {
            
        }

        public bool Success  { get { return true; }  }
    }
}
