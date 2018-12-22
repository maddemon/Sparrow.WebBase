using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SystemConfig>(Configuration.GetSection("SystemConfig"));
            RegisterAuthentication(services);
            RegisterDbContext(services);
            RegisterManagers(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Error;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
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
                app.UseHsts();
            }
            app.UseExceptionHandler("/Home/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        private void RegisterAuthentication(IServiceCollection services)
        {
            var builder = services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
               {
                   options.LoginPath = "/User/Login";
                   options.AccessDeniedPath = "/Error/Forbidden";
               });


            var jwtConfig = Configuration.Get<SystemConfig>().JwtConfig;
            if (jwtConfig != null)
            {
                builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtConfig.Issuer,
                        ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey))
                    };
                });
            }
        }

        private void RegisterDbContext(IServiceCollection services)
        {
            var config = Configuration.Get<SystemConfig>();
            services.AddDbContext<DataContext>(options =>
            {
                options.UseMySQL(config.DbConnectionString);
                options.UseLazyLoadingProxies();
            });

        }

        private void RegisterManagers(IServiceCollection services)
        {
            var assembly = Assembly.LoadFrom("Sparrow.Web.dll");
            foreach (var type in assembly.GetTypes())
            {
                if (type.FullName.EndsWith("Manager"))
                {
                    services.AddTransient(type);
                }
            }
        }

    }
}
