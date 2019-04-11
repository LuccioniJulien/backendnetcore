﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using BaseApi.Helper;
using BaseApi.Models;
using dotenv.net.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace BaseApi {
#pragma warning disable CS1591
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices (IServiceCollection services) {
            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_2);
            services.AddDotEnv ();
            services.AddNpgsqlContext ();
            services.AddJWT ();
            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new Info { Title = "My API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine (AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments (xmlPath);
            });
        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            app.UseCors (x => x
                .AllowAnyOrigin ()
                .AllowAnyMethod ()
                .AllowAnyHeader ());
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseHsts ();
            }
            Migrate (app);
            app.UseSwagger ();
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection ();
            app.UseMvc ();
            app.UseAuthentication ();
        }

        private static void Migrate (IApplicationBuilder app) {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory> ()
                .CreateScope ()) {
                using (var context = serviceScope.ServiceProvider.GetService<DBcontext> ()) {
                    try {
                        context.Database.Migrate ();

                    } catch (Exception e) {
                        Console.WriteLine (e.Message);
                    }
                }
            }
        }
    }
}