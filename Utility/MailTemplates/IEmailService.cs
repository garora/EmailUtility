using System.Net.Mail;

namespace Utility.MailTemplates
{
    public interface IEmailService
    {
        /// <summary>
        /// Configuration information needed for sending emails.
        /// </summary>
        IEmailSettings Settings { get; set; }


        /// <summary>
        /// Emails the message using default values from Settings.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Send(EmailMessage message);


        /// <summary>
        /// Emails the message using the connection strings supplied.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="useCredentials"></param>
        /// <param name="credentialsUser"></param>
        /// <param name="credentialsPassword"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        //BoolMessageEx Send(EmailMessage message, bool useCredentials, string credentialsUser, string credentialsPassword, string host, int port);


        /// <summary>
        /// Sends an email using the data from the message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Send(NotificationMessage message);


        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="credentialsUser">The credentials user.</param>
        /// <param name="credentialsPassword">The credentials password.</param>
        /// <returns></returns>
        bool Send(NotificationMessage message, string credentialsUser, string credentialsPassword);


        /// <summary>
        /// Send the mailmessage.
        /// </summary>
        /// <param name="message">The mail message</param>
        /// <param name="useCredentials">Whether or not to use credentials for security when sending emails.</param>
        /// <param name="credentialsUser">User name when using credentials.</param>
        /// <param name="credentialsPassword">Password when using credentials.</param>
        /// <returns></returns>        
        bool Send(MailMessage message, bool useCredentials, string credentialsUser, string credentialsPassword);


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
        bool Send(string from, string to, string subject, string body, bool useCredentials, string credentialsUser, string credentialsPassword);


        /// <summary>
        /// Mail the message using the native MailMessage class and the credentials from the current configuration.
        /// </summary>
        /// <param name="mailMessage">The mail message</param>
        /// <returns></returns>
        bool Send(MailMessage mailMessage);
    }
}
