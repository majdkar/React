using System.Globalization;
using System.Linq;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Server.Hubs;
using SchoolV01.Server.Middlewares;
using SchoolV01.Shared.Constants.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SchoolV01.Shared.Constants.Application;
using Microsoft.Extensions.Logging;
using SchoolV01.Infrastructure.Contexts;
using System;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Server.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<BlazorHeroContext>();

                    if (context.Database.IsSqlServer())
                    {
                        //context.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                    throw;
                }

                using var serviceScope = app.ApplicationServices.CreateScope();

                var initializer = serviceScope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
                initializer.Initialize();

                //var jobs = serviceScope.ServiceProvider.GetRequiredService<IJobsService>();
                //foreach (var job in jobs)
                //{
                //    job.RunScheduleJobs();
                //}

                //// requires using Microsoft.Extensions.Configuration;
                //// Set password with the Secret Manager tool.
                //// dotnet user-secrets set SeedUserPW <pw>

                //var testUserPw = builder.Configuration.GetValue<string>("SeedUserPW");

                //await SeedData.Initialize(services, testUserPw);1-s 2-g 3-search 4-

            }

            return app;
        }
        internal static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler(o => { });
            }
            return app;
        }

        internal static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", typeof(Program).Assembly.GetName().Name);
                options.RoutePrefix = "swagger";
                options.DisplayRequestDuration();
            });
        }

        internal static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
            => app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapHub<SignalRHub>(ApplicationConstants.SignalR.HubUrl);
                endpoints.MapHealthChecks("/health").RequireAuthorization();
            });

        internal static IApplicationBuilder UseRequestLocalizationByCulture(this IApplicationBuilder app)
        {
            var supportedCultures = LocalizationConstants.SupportedLanguages.Select(l => new CultureInfo(l.Code)).ToArray();
            app.UseRequestLocalization(options =>
            {
                options.SupportedUICultures = supportedCultures;
                options.SupportedCultures = supportedCultures;
                options.DefaultRequestCulture = new RequestCulture(supportedCultures.First());
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            app.UseMiddleware<RequestCultureMiddleware>();

            return app;
        }
    }
}