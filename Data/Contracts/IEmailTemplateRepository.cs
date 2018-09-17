using Data.Models.Entities;

namespace Data.Contracts
{
    public interface IEmailTemplateRepository
    {
        EmailTemplate FindById(int templateId);

        EmailTemplate FindByTemplateName(string templateName);
    }
}