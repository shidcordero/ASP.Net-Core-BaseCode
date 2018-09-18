using System.Threading.Tasks;
using Data.Models.Entities;

namespace Data.Contracts
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate> FindById(int templateId);

        Task<EmailTemplate> FindByTemplateName(string templateName);
    }
}