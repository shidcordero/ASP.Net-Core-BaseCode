using Data.Base;
using Data.Contracts;
using Data.Models.Entities;
using System.Linq;

namespace Data.Repositories
{
    public class EmailTemplateRepository : BaseRepository, IEmailTemplateRepository
    {
        /// <summary>
        /// Instantiate UnitOfWork
        /// </summary>
        /// <param name="unitOfWork">Holds the instance of UnitOfWork interface</param>
        public EmailTemplateRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public EmailTemplate FindById(int templateId)
        {
            return GetDbSet<EmailTemplate>().Find(templateId);
        }

        public EmailTemplate FindByTemplateName(string templateName)
        {
            return GetDbSet<EmailTemplate>().FirstOrDefault(x => x.TemplateName == templateName);
        }
    }
}