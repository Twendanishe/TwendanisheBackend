using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.EntityFrameworkCore;
using Twendanishe.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Twendanishe.Models;
using Microsoft.AspNetCore.Identity;
using Twendanishe.Business;

namespace Twendanishe
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetValue<string>("TokenIssuer"),
                        ValidAudience = Configuration.GetValue<string>("TokenAudience"),
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SecurityKey")))
                    };
                });

            //services.AddEntityFrameworkSqlServer()
            //    .AddDbContext<TwendanisheContext>(options =>
            //        options.UseSqlServer(Configuration.GetConnectionString("TwendanisheDatabase")));

            //services.AddIdentity<User, IdentityRole>(Configuration)
            //    .AddEntityFrameworkStores<TwendanisheContext>();
            services.AddDbContext<TwendanisheContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("TwendanisheDatabase")));

            services.AddMvc();

            services.AddSingleton(Configuration);

            // Configure Business Services
            services.AddSingleton<RideService>();
            services.AddSingleton<DestinationService>();
            services.AddSingleton<LocationService>();
            services.AddSingleton<AccountService>();
            services.AddSingleton<HashingService>();
            services.AddSingleton<WhereIsMyTransportApiService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
