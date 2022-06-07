using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

// when creating an extension method the class itseld 
// need to be static
// we need to add services from our startup into this class file. We add all the necessary usings and then
namespace API.Extensions
{
    public static class ApplicationServiceExtentions
    // we want to return an IserviceCollection which 
    // iswhat we are extending. we then give the 
    // Icollection a name of services
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // when using API we need to scope services
            services.AddScoped<ITokenService, TokenService>();
            // below is lamba expression
            //inside options you write the below in the param. GetCOnnection is going
            // to look for its Connectionstring in its name
            // this is set up this way to access the database via ConnectionString
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}