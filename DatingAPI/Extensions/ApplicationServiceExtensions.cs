using DatingAPI.Data;
using DatingAPI.Interfaces;
using DatingAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        //extending IServiceCollection using this keyword, returning IServiceCollection as well.
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //adding our datacontext class as a service in our container.
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });//this connection will be set in appsettings.Development.json
               //unlike SQL, we don't need server name, port number or security info in SQL Lite connection string

            //add service to enable CORS so that angular client can work with our requests without throwing '"HttpErrorResponse"' error.
            services.AddCors();

            //add tokenservice to the services container

            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}