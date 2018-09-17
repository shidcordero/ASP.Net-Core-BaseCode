using AutoMapper;
using Data.Models.Entities;
using Data.ViewModels.Region;
using Microsoft.Extensions.DependencyInjection;

namespace ASP
{
    public partial class Startup
    {
        private static void ConfigureMapper(IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Region, RegionViewModel>().ReverseMap();
            });

            services.AddSingleton(sp => mapperConfiguration.CreateMapper());
        }
    }
}