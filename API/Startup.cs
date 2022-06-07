using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using API.Extensions;

// to tidy up the code within the startup.cs we use something called 
// extension methods. These methods enable us to add methods to 
// exisitng types without creating an ew derived type or modify the type
// you do this by creating an ew folder in the API called extensions

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        
        public Startup(IConfiguration config)
        {
            // this in constructos usually arent needed
            // add an underscore to the config above
            _config = config;
            // Configuration = configuration;
        }

        // public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // ********** EVERYTHING FROM HERE ********
            // // when using API we need to scope services
            // services.AddScoped<ITokenService, TokenService>();
            // // below is lamba expression
            // //inside options you write the below in the param. GetCOnnection is going
            // // to look for its Connectionstring in its name
            // // this is set up this way to access the database via ConnectionString
            // services.AddDbContext<DataContext>(options =>
            // {
            //     options.UseSqlite(_config.GetConnectionString("DefaultConnection"));
            // });
            // middleware for authentivation starts here
            // ******** TO HERE is in AppServExt *********


            services.AddApplicationServices(_config);
            services.AddControllers();
            services.AddCors();

            // ********** EVERYTHING FROM HERE ********
            // services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            // // congigs after this will be chained on
            //     .AddJwtBearer(options =>
            //     {
            //         options.TokenValidationParameters = new TokenValidationParameters
            //         {
            //             ValidateIssuerSigningKey = true,
            //             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"])),
            //             ValidateIssuer = false,
            //             ValidateAudience = false,
            //         };
            //     });
            // ******** TO HERE is in IdServices *********

            services.AddIdentityServices(_config);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            // Cors needs to be inbetween routing and endpoints. User 
            // cors needs to come before UseAuthentication
            // x is the policy
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
            // UseAuthentication needs to come before UseAuthorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
