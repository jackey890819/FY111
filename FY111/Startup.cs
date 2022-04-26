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
using FY111.Models.DriveCourse;
using Microsoft.AspNetCore.Authentication.Cookies;      //cookies
using Newtonsoft.Json;

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
            /*services.AddControllersWithViews()  // �ѨMjson bug
                    .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );
            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 
            });*/
            services.AddControllersWithViews();
            services.AddDirectoryBrowser();

            services.AddControllers();

            services.AddMvc();
            services.AddSession();
            // FY111��Ʈw�]�w
            services.AddDbContext<FY111Context>(opt =>
            {
                opt.UseMySQL(Configuration.GetConnectionString("default"));
            });
            // drive_course��Ʈw�]�w
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
            
            app.UseHttpsRedirection();  // �N Http �ন https �� Middleware
            app.UseStaticFiles();       // �B�z�R�A�ɮ�
            // app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();    // ����
            app.UseAuthorization();     // ���v
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
