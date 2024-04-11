using KnilServer.Application.Common;
using KnilServer.Application.Services;
using KnilServer.Application.Services.Interfaces;
using KnilServer.Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnilServer.Application
{
    public static class ApplicationRegister
    {
        public static IServiceCollection AddApplicationsService(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IContactServices, ContactService>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
