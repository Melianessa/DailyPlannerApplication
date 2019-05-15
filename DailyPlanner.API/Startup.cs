﻿using System;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "DailyPlanner.API";
                });
            services.AddScoped<IDataRepository<User>, UserRepository>();
            services.AddScoped<IDataRepository<Event>, EventRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventBase<Event>, EventRepository>();
            services.AddScoped<DbContext, PlannerDbContext>();
            services.AddDbContext<PlannerDbContext>(opts =>
                opts.UseSqlServer(Configuration["ConnectionString:DailyPlannerDB"]));
            services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonFormatters().AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            try
            {
                app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
