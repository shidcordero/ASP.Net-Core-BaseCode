using System;
using System.Data.SqlClient;
using Data.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        /// Used to seed data to database
        /// </summary>
        /// <param name="modelBuilder">Instance of modelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Region>().HasData(
                new { RegionId = 1, RegionName = "EU", RegionCode = "0", RegionKey = "EU", Description = "Europe" },
                new { RegionId = 2, RegionName = "KOREA", RegionCode = "5", RegionKey = "AS", Description = "Korea" }
            );
        }
    }
}