using Data.Base;
using Data.Contracts;
using Data.Models.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        /// <summary>
        /// Gets email template by id
        /// </summary>
        /// <param name="id">Holds the email template id</param>
        /// <returns>EmailTemplate Entity Model</returns>
        public async Task<EmailTemplate> FindById(int id)
        {
            return await GetDbSet<EmailTemplate>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TemplateId == id);
        }

        /// <summary>
        /// Gets email template by template name
        /// </summary>
        /// <param name="templateName">Holds the template name</param>
        /// <returns>EmailTemplate Entity Model</returns>
        public async Task<EmailTemplate> FindByTemplateName(string templateName)
        {
            return await GetDbSet<EmailTemplate>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TemplateName == templateName);
        }
    }
}