using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sparrow.Web.Managers;
using Sparrow.Web.Models;

namespace Sparrow.Web.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void RegisterAuthentication(IServiceCollection services)
        {
            var jwtSettings = JwtSettings.New(Configuration);

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidIssuer = jwtSettings.Issuer,
                       ValidAudience = jwtSettings.Audience,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey))
                   };
               })
               .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
               {
                   options.LoginPath = "/User/Login";
                   options.AccessDeniedPath = "/Error/Forbidden";
               });
        }

        private void RegisterManagers(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                var connStr = Configuration.GetConnectionString("Default");
                options.UseMySQL(connStr);
                //options.UseLazyLoadingProxies();
            });

            services.AddTransient<UserManager>()
                .AddTransient<AuthenticationManager>()
                ;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<SystemConfig>(Configuration.GetSection("SystemConfig"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
