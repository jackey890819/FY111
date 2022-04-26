using System;
using FY111.Areas.Identity.Data;
using FY111.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FY111.Areas.Identity.IdentityHostingStartup))]
namespace FY111.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<FY111UserDbContext>(options =>
                    options.UseMySQL(
                        context.Configuration.GetConnectionString("FY111UserDbContextConnection")));

                services.AddDefaultIdentity<FY111User>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<FY111UserDbContext>();
            });
        }
    }
}