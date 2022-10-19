using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Moodswing.Domain.Models.Authentication;
using Moodswing.Service.Authentication;
using System.Collections.Generic;
using Moodswing.Application.CrossCutting.DataBase;
using Moodswing.Application.CrossCutting;
using Moodswing.Domain.Models.User;

namespace Moodswing.Application
{
    public class Startup
    {
        private const string BEARER = "Bearer";
        private const string SET_TOKEN = "Set your Token";
        private const string AUTHORIZATION = "Authorization";
        private const string USER_API = "UserApi:Url";

        private const string DEFAULT = "Default";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddControllers()
                .AddNewtonsoftJson();

            PostgresDatabase.ConnectionString = Configuration.GetConnectionString(DEFAULT);
            
            services
                .InitializeDatabase()
                .AddGenericRepository()
                .AddScheduleStrategyConfiguration()
                .AddMapper()
                .AddScheduleService();
            
            services.AddHttpClient();

            services.AddSingleton(new UserObjectApi() { BaseUrl = Configuration.GetSection(USER_API).Value});

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Moodswing.Application", Version = "v1" });
                c.AddSecurityDefinition(BEARER, new OpenApiSecurityScheme()
                {
                    Description = SET_TOKEN,
                    Name = AUTHORIZATION,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = BEARER
                                }
                            },
                            new List<string>()
                    }
                });

            });

            AddAuthentication(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => 
            { 
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moodswing.Application v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddAuthentication(IServiceCollection service)
        {
            service.AddScoped<ISigningConfigurations, SigningConfigurations>();

            service.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    //options.Authority = "https://localhost:6001/";
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SigningConfigurations().Key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });
            service.AddAuthorization();
        }
    }
}
