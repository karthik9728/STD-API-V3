using MaxiShop.Application.Common;
using MaxiShop.Application.Services;
using MaxiShop.Application.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxiShop.Application
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
