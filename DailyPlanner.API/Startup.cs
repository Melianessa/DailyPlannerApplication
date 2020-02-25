using System;
using DailyPlanner.DomainClasses;
using DailyPlanner.DomainClasses.Interfaces;
using DailyPlanner.DomainClasses.Models;
using DailyPlanner.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DailyPlanner.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["Url:Identity"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "DailyPlanner.API";
                });
            services.AddScoped<IDataRepository<User>, UserRepository>();
            services.AddScoped<IDataRepository<Event>, EventRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventBase, EventRepository>();
            services.AddScoped<DbContext, PlannerDbContext>();
            services.AddDbContext<PlannerDbContext>(opts =>
                opts.UseSqlServer(Configuration["ConnectionString:DailyPlannerDB"]));
            services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonFormatters()
                .AddAuthorization(
                opt => opt.AddPolicy("Client", policy => policy.RequireClaim("Role", "Client"
                   )))
                .AddAuthorization(
                opt => opt.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"
                   )));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
