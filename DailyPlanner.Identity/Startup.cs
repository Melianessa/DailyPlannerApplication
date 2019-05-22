using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using DailyPlanner.Identity.Overrides;
using DailyPlanner.Identity.Repositories;
using DailyPlanner.Repository;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DailyPlanner.Identity
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            X509Certificate2 cert = null;
            using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                certStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    // Replace below with your cert's thumbprint
                    "adee19ef17ba7e8cb9a77db9cd7fcb1a5ac30b7f",
                    false);
                // Get the first cert with the thumbprint
                if (certCollection.Count > 0)
                {
                    cert = certCollection[0];
                }
            }

            // Fallback to local file for development
            if (cert == null)
            {
                cert = new X509Certificate2(Path.Combine(Environment.ContentRootPath, Configuration["Cert:CertificatePath"]), Configuration["Cert:CertificatePassword"]);
            }
            services.AddDbContext<PlannerDbContext>(opts =>
                opts.UseSqlServer(Configuration["ConnectionString:DailyPlannerDB"]));
            var builder = services.AddIdentityServer()
                .AddSigningCredential(cert)
                //.AddInMemoryClients(new List<Client>
                //{
                //    new Client
                //    {
                //        ClientId = "DailyPlanner.API",
                //        ClientSecrets = {new Secret("QFEGrbte46bgbcfht!3cvgfnf".Sha256())},
                //        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                //        AllowedScopes =
                //        {
                //            "DailyPlanner.API",
                //            IdentityServerConstants.StandardScopes.Email,
                //            IdentityServerConstants.StandardScopes.OpenId
                //        },
                //        AllowOfflineAccess = false,
                //        AccessTokenType = AccessTokenType.Jwt,
                //        // 12 hour
                //        AccessTokenLifetime = 60 * 60 * 12,
                //        //AllowedCorsOrigins = Configuration.GetSection("IDP:AllowSpecificOrigin").Get<string[]>()
                //    }
                //})
                .AddInMemoryApiResources(new List<ApiResource>
                {
                    new ApiResource("DailyPlanner.API", "Daily Planner")
                    {
                        Scopes = new[]
                        {
                            new Scope("DailyPlanner.API", "Daily Planner"),
                            new Scope(IdentityServerConstants.StandardScopes.Email),
                            new Scope(IdentityServerConstants.StandardScopes.Profile),
                            new Scope(IdentityServerConstants.StandardScopes.Phone),
                            new Scope(IdentityServerConstants.StandardScopes.OpenId)
                        }
                    },
                    new ApiResource("DailyPlanner.Web", "Daily Planner")
                    {
                        Scopes = new[]
                        {
                            new Scope("DailyPlanner.Web", "Daily Planner"),
                            //new Scope(IdentityServerConstants.StandardScopes.Email),
                            //new Scope(IdentityServerConstants.StandardScopes.Profile),
                            //new Scope(IdentityServerConstants.StandardScopes.Phone),
                            //new Scope(IdentityServerConstants.StandardScopes.OpenId)
                        }
                    }
                })
                .AddProfileService<ProfileService>()
                .AddClientStore<ClientStore>();

            //services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            services.AddScoped<UserAuthRepository>();
            services.AddScoped<UserRepository>();
            //services.AddScoped<PersistedGrantRepository>();

            if (Environment.IsDevelopment())
            {
                //builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            services.AddAuthentication()
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "<insert here>";
                    options.ClientSecret = "<insert here>";
                })
                .AddOpenIdConnect("oidc", "OpenID Connect", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                    options.SaveTokens = true;

                    options.Authority = "https://demo.identityserver.io/";
                    options.ClientId = "implicit";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });
            //.AddIdentityServerAuthentication(options =>
            //{
            //    options.Authority = "https://localhost:44321";
            //    options.RequireHttpsMetadata = false;
            //    options.ApiName = "DailyPlanner.API";
            //});
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowSpecificOrigin", builder => builder
            //        .WithOrigins(Configuration.GetSection("IDP:AllowSpecificOrigin").Get<string[]>())
            //        .AllowAnyHeader()
            //        .AllowAnyMethod());
            //});
            //// Change login URL
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/account/login";

            //    // Change cookie timeout to expire in 15 seconds
            //    options.ExpireTimeSpan = TimeSpan.FromSeconds(1500);
            //});
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            //app.UseCors("AllowSpecificOrigin");
            //app.UseIdentityServer();
            //app.UseAuthentication();
            //app.UseMvc();

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();
        }
    }
}
