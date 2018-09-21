using Data.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmailTemplateConstants = Data.Utilities.Constants.EmailTemplate;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Function to modify database structure or seed data
        /// </summary>
        /// <param name="modelBuilder">Instance of modelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Region>().HasData(
                new
                {
                    RegionId = 1,
                    RegionName = "EU",
                    RegionCode = "0",
                    RegionKey = "EU",
                    Description = "Europe"
                },
                new
                {
                    RegionId = 2,
                    RegionName = "KOREA",
                    RegionCode = "5",
                    RegionKey = "AS",
                    Description = "Korea"
                }
            );

            modelBuilder.Entity<EmailTemplate>().HasData(
                new
                {
                    TemplateId = 1,
                    EmailTemplateConstants.ForgotPassword.TemplateName,
                    EmailTemplateConstants.ForgotPassword.Subject,
                    EmailTemplateConstants.ForgotPassword.Body
                },
                new
                {
                    TemplateId = 2,
                    EmailTemplateConstants.Exception.TemplateName,
                    EmailTemplateConstants.Exception.Subject,
                    EmailTemplateConstants.Exception.Body
                }
            );
        }
    }
}