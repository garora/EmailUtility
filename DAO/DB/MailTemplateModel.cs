namespace DAO.DB
{
    public class MailTemplateModel
    {
        public virtual int id { get; set; }
        public virtual string Template { get; set; }
        public virtual int TemplateId { get; set; }
        public virtual string Subject { get; set; }
        public virtual string HtmlBody { get; set; }
        public virtual string TextBody { get; set; }
        public virtual bool IsActive { get; set; }
    }
}