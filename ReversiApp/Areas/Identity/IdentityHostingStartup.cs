using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReversiApp.Areas.Identity.Data;
using ReversiApp.Data;

[assembly: HostingStartup(typeof(ReversiApp.Areas.Identity.IdentityHostingStartup))]
namespace ReversiApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ReversiAppContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ReversiAppContextConnection")));

                services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ReversiAppContext>();
            });
        }
    }
}