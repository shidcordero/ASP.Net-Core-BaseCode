using Data.Contracts;
using Data.Repositories;
using Domain.Contracts;
using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ASP
{
    public partial class Startup
    {
        private static void InjectDependencies(IServiceCollection services)
        {
            #region unit of work

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #endregion unit of work

            #region Services

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRegionService, RegionService>();

            #endregion Services

            #region Repositories

            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();

            #endregion Repositories
        }
    }
}