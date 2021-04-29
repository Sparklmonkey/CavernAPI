using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PetBaseData.API.Data;
using PetBaseData.API.Repositories;
using PetBaseData.API.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetBaseData.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Sparklmonkey PetCavern",
                    Version = "v1"
                });

                config.OperationFilter<AddRequiredHeaderParameter>();
            });

            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddScoped<IPetContext, PetContext>();
            services.AddScoped<IPetObjectRepository, PetObjectRepository>();
            services.AddScoped<IUserDataContext, UserDataContext>();
            services.AddScoped<IUserLoginRepository, UserLoginRepository>();
            services.AddScoped<IUserManagementRepository, UserManagementRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c =>
                {
                    c.SerializeAsV2 = true;
                });
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetBaseData.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
