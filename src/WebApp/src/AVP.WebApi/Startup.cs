using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AVP.WebApi.Services;
using AVP.WebApi.Config;
using AVP.DataAccess;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

namespace AVP.WebApi
{
    /// <summary>
    /// Initial startup class that handles are service configuration and injection.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initial startup and build of service
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        /// <summary>
        /// Root configuration object for the application
        /// </summary>
        public IConfigurationRoot Configuration { get; }
        
        /// <summary>
        /// Configure services and objects for dependency injection
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            // Add CORS Policy
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var sendGridOptions = Configuration.GetSection(nameof(SendGridOptions));
            var twilioOptions = Configuration.GetSection(nameof(TwilioOptions));
            var exchangeOptions = Configuration.GetSection(nameof(ExchangeOptions));
            var dbOptions = Configuration.GetSection(nameof(DBConnectionOptions));

            services.Configure<DbConnectionSettings>(options => {
                options.ConnectionString = dbOptions[nameof(DbConnectionSettings.ConnectionString)];
                options.Schema = dbOptions[nameof(DbConnectionSettings.Schema)];
            } );

            // Configure Exchange
            services.Configure<ExchangeOptions>(options =>
            {
                options.UserName = exchangeOptions[nameof(ExchangeOptions.UserName)];
                options.Password = exchangeOptions[nameof(ExchangeOptions.Password)];
                options.HostName = exchangeOptions[nameof(ExchangeOptions.HostName)];
                options.Port = Convert.ToInt32(exchangeOptions[nameof(ExchangeOptions.Port)]);
                options.EnableSSL = Convert.ToBoolean(exchangeOptions[nameof(ExchangeOptions.EnableSSL)]);
                options.EmailSubject = exchangeOptions[nameof(ExchangeOptions.EmailSubject)];
            });

            // Configure SendGrid
            services.Configure<SendGridOptions>(options =>
            {
                options.SendGridApiKey = sendGridOptions[nameof(SendGridOptions.SendGridApiKey)];
                options.FromEmail = sendGridOptions[nameof(SendGridOptions.FromEmail)];
            });

            // Configure EmailHost
            services.Configure<SendGridOptions>(options =>
            {
                options.SendGridApiKey = sendGridOptions[nameof(SendGridOptions.SendGridApiKey)];
                options.FromEmail = sendGridOptions[nameof(SendGridOptions.FromEmail)];
            });

            // Configure Twilio
            services.Configure<TwilioOptions>(options =>
            {
                options.AccountSid = twilioOptions[nameof(TwilioOptions.AccountSid)];
                options.AuthToken = twilioOptions[nameof(TwilioOptions.AuthToken)];
                options.MsgServiceSid = twilioOptions[nameof(TwilioOptions.MsgServiceSid)];
            });

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingOptions[nameof(JwtIssuerOptions.Secret)]));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
                {
                    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                    options.Secret = jwtAppSettingOptions[nameof(JwtIssuerOptions.Secret)];
                    options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminUser",
                                  policy => policy.RequireClaim("IsAdmin", "true"));
            });

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddWebApiConventions();

            services.AddTransient<IAuthService, AuthService>();
            services.AddSingleton<IDAO, DAO>();
            services.AddTransient<ISmsService, SmsService>();
            services.AddTransient<IEmailService, EmailService>();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Thunderstruck API", Version = "v1" });

                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "AVP.WebApi.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // 
        /// <summary>
        /// This method gets called by the runtime. Used to configure the HTTP request pipeline
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        /// <param name="loggerFactory">ILoggerFactory</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // USE CORS
            app.UseCors("CorsPolicy");

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();


            //JWT Config            
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingOptions[nameof(JwtIssuerOptions.Secret)]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });
            //End JWT Config

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUi(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Thunderstruck API V1");
            });
        }
    }
}
