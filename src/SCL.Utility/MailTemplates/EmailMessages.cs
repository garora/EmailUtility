using System.Collections.Generic;

namespace Utility.MailTemplates
{
    public class NotificationMessage
    {
        public string To;
        public string From;
        public string Subject;
        public string Body;
        public string MessageTemplateId;
        public bool IsHtml = true;
        public IDictionary<string, string> Values;


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="messageTemplateId"></param>
        public NotificationMessage(IDictionary<string, string> values, string to, string from, string subject, string messageTemplateId)
        {
            To = to;
            From = from;
            Subject = subject;
            Values = values;
            MessageTemplateId = messageTemplateId;
        }
    }



    /// <summary>
    /// Basic email message.
    /// </summary>
    public class EmailMessage
    {

        /// <summary>
        /// From email address.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// To email address.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Subject of email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body of email.
        /// </summary>
        public string Body { get; set; }


        /// <summary>
        /// Whether or not the body message contains html.
        /// </summary>
        public bool IsHtml { get; set; }
    }

}
