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
using FY111.Models;
using Microsoft.EntityFrameworkCore;
using FY111.Models.FY111;
using FY111.Models.DriveCourse;
using Microsoft.AspNetCore.Authentication.Cookies;      //cookies

namespace FY111
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews();
            services.AddControllersWithViews()  // 解決json bug
                    .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );
            services.AddDirectoryBrowser();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options => {
                        options.AccessDeniedPath = "/Home/AccessDeny";
                        options.LoginPath = "/Home/Login";  // 登入頁面
                    });
            services.AddControllers();
            services.AddMvc();
            services.AddSession();
            // FY111資料庫設定
            services.AddDbContext<FY111Context>(opt =>
            {
                opt.UseMySQL(Configuration.GetConnectionString("default"));
            });
            // drive_course資料庫設定
            services.AddDbContext<drive_courseContext>(opt =>
            {
                opt.UseMySQL(Configuration.GetConnectionString("drive_course"));
            });

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
            
            app.UseHttpsRedirection();  // 將 Http 轉成 https 的 Middleware
            app.UseStaticFiles();       // 處理靜態檔案
            // app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();    // 驗證
            app.UseAuthorization();     // 授權
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
