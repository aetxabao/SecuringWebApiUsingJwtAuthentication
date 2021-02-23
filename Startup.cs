using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SecuringWebApiUsingJwtAuthentication.Handlers;
using SecuringWebApiUsingJwtAuthentication.Helpers;
using SecuringWebApiUsingJwtAuthentication.Interfaces;
using SecuringWebApiUsingJwtAuthentication.Requirements;
using SecuringWebApiUsingJwtAuthentication.Services;
using SecuringWebApiUsingJwtAuthentication.Data;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace SecuringWebApiUsingJwtAuthentication
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
            services.AddDbContext<CustomersDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("SWAUJAContext")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = TokenHelper.Issuer,
                        ValidAudience = TokenHelper.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(TokenHelper.Secret))
                    };

                });

            services.AddAuthorization(options =>
                {
                    options.AddPolicy("OnlyNonBlockedCustomer", policy =>
                    {
                        policy.Requirements.Add(new CustomerBlockedStatusRequirement(false));

                    });
                });

            services.AddSingleton<IAuthorizationHandler, CustomerBlockedStatusHandler>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddControllers();
            // services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo
            //     {
            //         Title = "TodoApi",
            //         Description = "Un API simple para gestionar tareas",
            //         Version = "v1"
            //     });

            //     // Set the comments path for the Swagger JSON and UI.
            //     var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //     c.IncludeXmlComments(xmlPath);

            // });

            services.AddSwaggerGen(c =>
            {
                // configure SwaggerDoc and others
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SecuringWebApiUsingJwtAuthentication",
                    Description = "Un API para consultar ordenes de clientes",
                    Version = "v1"
                });

                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Escribe **_sólo_** el token JWT sin precederlo de Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, new string[] { }}
                });

                // add Basic Authentication
                // var basicSecurityScheme = new OpenApiSecurityScheme
                // {
                //     Type = SecuritySchemeType.Http,
                //     Scheme = "basic",
                //     Reference = new OpenApiReference { Id = "BasicAuth", Type = ReferenceType.SecurityScheme }
                // };
                // c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                // c.AddSecurityRequirement(new OpenApiSecurityRequirement
                // {
                //     {basicSecurityScheme, new string[] { }}
                // });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SecuringWebApiUsingJwtAuthentication v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
