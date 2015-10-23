using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DB;

namespace Utility.Common
{
    public class MailTemplate
    {
        public enum ETemplateList
        {
            NewRegistration = 1,
            UserActivation = 2,
            ForgetUserId = 3,
            ForgetPassword = 4
        }

        private string _mailBody = string.Empty;
        private string _mailSubject = string.Empty;

        public MailTemplate(MailTemplate.ETemplateList templateName)
        {
            using (var repository = new Repository())
            {
                var mailtemplate =
                    repository.GetAll<MailTemplateModel>(
                        ).First(x => x.TemplateId == Convert.ToInt16(templateName) && x.IsActive);

                if (mailtemplate == null)
                    throw new Exception("Template Not Found in template list.");

                MailSubject = mailtemplate.Subject;
                MailBody = mailtemplate.HtmlBody;
            }
        }


        public string MailBody
        {
            get { return _mailBody; }
            private set { _mailBody = value; }
        }

        public string MailSubject
        {
            get { return _mailSubject; }
            private set { _mailSubject = value; }
        }
    }

    //ToDo - Implement Repository code which fetche templates from Database
    public class Repository : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public IEnumerable<T> GetAll<T>()
        {
            return new List<T>().ToList();
        }
    }
}