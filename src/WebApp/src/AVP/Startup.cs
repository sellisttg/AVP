using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using IdentityServer4.Models;
//using IdentityServer4.Services.InMemory;
using AVP.Configuration;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityModel;

namespace AVP
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //connection string for DB
            const string connectionString = @"Data Source=TTGLT-132\SQLSERVER2014;Initial Catalog=AVP2017;Integrated Security=True";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // ASP.NET Identity DbContext
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // ASP.NET Identity Registrations
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            //setup identity server
            services.AddIdentityServer()
                  .AddOperationalStore(
                      builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                  //.AddInMemoryClients(Clients.Get())
                  //.AddInMemoryIdentityResources(Resources.GetIdentityResources())
                  //.AddInMemoryApiResources(Resources.GetApiResources())
                  .AddConfigurationStore(
                      builder => builder.UseSqlServer(connectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                  //.AddInMemoryUsers(Users.Get())
                  .AddAspNetIdentity<IdentityUser>()
                  .AddTemporarySigningCredential();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseDeveloperExceptionPage();

            InitializeDbTestData(app);

            app.UseIdentity();
            app.UseIdentityServer();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

        }

        private static void InitializeDbTestData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                if (!context.Clients.Any())
                {
                    
                    foreach (var client in Clients.Get())
                    {
                       // context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in AVP.Configuration.Resources.GetIdentityResources())
                    {
                       // context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in AVP.Configuration.Resources.GetApiResources())
                    {
                       // context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (!userManager.Users.Any())
                {
                    var identityUser = new IdentityUser("sam")
                    {
                        Id = "5BE86359-073C-434B-AD2D-A3932222DABE",
                        Email = "sellis@trinitytg.com",
                        AccessFailedCount = 0,
                        EmailConfirmed = true,
                        LockoutEnabled = false,
                        PhoneNumberConfirmed = true,
                        TwoFactorEnabled = false
                       
                    };

                    identityUser.Claims.Add(new IdentityUserClaim<string>
                    {
                        UserId = identityUser.Id,
                        ClaimType = JwtClaimTypes.Email,
                        ClaimValue = "sellis@trinitytg.com",
                    });

                    identityUser.Claims.Add(new IdentityUserClaim<string>
                    {
                        UserId = identityUser.Id,
                        ClaimType = JwtClaimTypes.Role,
                        ClaimValue = "admin",
                    });

                   
                    userManager.CreateAsync(identityUser, "Password#1!").Wait();
                    

                }
            }
        }
    }
}
