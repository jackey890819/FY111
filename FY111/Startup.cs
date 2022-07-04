using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FY111.Models.FY111;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;      //cookies
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FY111.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using FY111.Areas.Identity.Data;
using FY111.Extensions;
using FY111.Interfaces;
using FY111.Services;

namespace FY111
{
    public class Startup
    {
        private string _policyName = "CorsPolicy";
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_policyName, policy =>
                {
                    policy.WithOrigins("http://localhost")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
                //options.AddPolicy(name: _policyName, builder =>
                //{
                //    builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
                //    //builder.AllowAnyOrigin()
                //    //    .AllowAnyHeader()
                //    //    .AllowAnyMethod();
                //});
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddDirectoryBrowser();
            services.AddControllers();
            services.AddSession();
            // FY111 Database
            services.AddDbContext<FY111Context>(opt =>
            {
                opt.UseMySQL(Configuration.GetConnectionString("fy111"));
            });

            #region Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] {
                "zh-TW",        // Chinese - Taiwan
                "en-US",        // English - United States
                "zh-CN"         // Chinese - China
            };
                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });
            #endregion Localization

            // JWT
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.ConfigureJWT(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();       
            // app.UseCookiePolicy();
            app.UseRouting();


            #region Localization
            var supportedCultures = new[] {
                "zh-TW",        // Chinese - Taiwan
                "en-US",        // English - United States
                "zh-CN"         // Chinese - China
            };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);    // Localization Middleware
            #endregion Localization


            app.UseAuthentication();    // Authentication Middleware
            app.UseAuthorization();     // Authorization Middleware
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
