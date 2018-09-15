using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #region Auth 
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(o =>
               {
                   o.TokenValidationParameters = new TokenValidationParameters
                   {
                       NameClaimType = JwtClaimTypes.Name,
                       RoleClaimType = JwtClaimTypes.Role,

                       ValidIssuer = "http://localhost:44319",
                       ValidAudience = "api",
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is a security key"))

                       /***********************************TokenValidationParameters的参数默认值***********************************/
                       // RequireSignedTokens = true,
                       // SaveSigninToken = false,
                       // ValidateActor = false,
                       // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                       // ValidateAudience = true,
                       // ValidateIssuer = true, 
                       // ValidateIssuerSigningKey = false,
                       // 是否要求Token的Claims中必须包含Expires
                       // RequireExpirationTime = true,
                       // 允许的服务器时间偏移量
                       // ClockSkew = TimeSpan.FromSeconds(300),
                       // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                       // ValidateLifetime = true
                   };
               });
            #endregion

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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
