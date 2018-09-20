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

        public async Task<EmailTemplate> FindById(int id)
        {
            return await GetDbSet<EmailTemplate>().AsNoTracking().FirstOrDefaultAsync(x => x.TemplateId == id);
        }

        public async Task<EmailTemplate> FindByTemplateName(string templateName)
        {
            return await GetDbSet<EmailTemplate>().AsNoTracking().FirstOrDefaultAsync(x => x.TemplateName == templateName);
        }
    }
}