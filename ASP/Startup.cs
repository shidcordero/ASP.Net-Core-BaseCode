using AutoMapper;
using Data;
using Data.Configurations;
using Data.Utilities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace ASP
{
    public partial class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">services instance</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(Constants.AppConfig.ConnectionString),
                    b => b.MigrationsAssembly(Constants.AppConfig.MigrationAssembly)));

            //initialize configuration
            services.Configure<EmailConfiguration>(Configuration.GetSection(Constants.ConfigurationString.Email));
            services.Configure<ExceptionEmailConfiguration>(Configuration.GetSection(Constants.ConfigurationString.ExceptionEmail));

            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //for login
            ConfigureAuth(services);
            //inject dependencies
            InjectDependencies(services);
            //configure auto mapper
            ConfigureMapper(services);

            services.AddAutoMapper();
            services.AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">application instance</param>
        /// <param name="env">hosting environment instance</param>
        /// <param name="loggerFactory">for logging</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseDefaultFiles();

            //add route
            ConfigureRoute(app);
            //add logger
            ConfigureLogger(loggerFactory);
        }
    }
}