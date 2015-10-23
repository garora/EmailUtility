namespace Utility.MailTemplates
{
    public interface IEmailSettings
    {
        /// <summary>
        /// 
        /// </summary>
        string UseDefaultCredentials
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        string EnableSSL
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the SMTP server.
        /// </summary>
        /// <value>The SMPT server.</value>
        string SmptServer { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        string From { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [use port].
        /// </summary>
        /// <value><c>true</c> if [use port]; otherwise, <c>false</c>.</value>
        bool UsePort { get; set; }


        /// <summary>
        /// Gets or sets the name of the authentication user.
        /// </summary>
        /// <value>The name of the authentication user.</value>
        string AuthenticationUserName { get; set; }


        /// <summary>
        /// Gets or sets the authentication password.
        /// </summary>
        /// <value>The authentication password.</value>
        string AuthenticationPassword { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is authentication required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is authentication required; otherwise, <c>false</c>.
        /// </value>
        bool IsAuthenticationRequired { get; set; }


        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        int Port { get; set; }
    }

}
