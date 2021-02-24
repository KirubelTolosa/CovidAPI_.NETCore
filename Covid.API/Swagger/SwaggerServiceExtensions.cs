using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Covid.API.Swagger
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "Release 2.0",
                    Title = "Covid-19 API",
                    Description = "This covid-19 Rest-API can be used to retrieve data related to the ongoing covid pandemic.",
                    Contact = new OpenApiContact { Email = "info@kirubeltolosa.com", Name = "Kirubel Tolosa", Url = new Uri("https://github.com/KirubelTolosa") }                    
                });

                //c.SwaggerDoc("v2", new OpenApiInfo
                //{
                //    Version = "2020.0.0.2",
                //    Title = "Sample API",
                //    Description = "This API is to demonstrate Identity Server 4 authentication.",
                //    Contact = new OpenApiContact { Email = "yordanos.alemu@gmail.com", Name = "Yordanos Alemu", Url = new Uri("https://www.bandlay.com") }
                //});

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\", replace {token} with your access token value.",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                //you Don't need this if you are not implementing operation filter.
                c.OperationFilter<AuthorizeCheckOperationFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Sample API V1");
                c.SwaggerEndpoint("../swagger/v2/swagger.json", "Sample API V2");

                c.RoutePrefix = ""; // serve the UI at root

                c.EnableFilter();

                //c.DocExpansion(DocExpansion.None);
            });

            return app;
        }
    }
}
