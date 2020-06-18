using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using DataBase.Entities;
using DataBase.Identity;
using DataBase.Interfaces;
using DataBase.Repositories;
using DataBase;
using API.Tokens;
using API.Actions;
using API.ViewModels.Settings;


namespace API
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
            services.AddControllers();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("DataBase")));
            services.AddDbContext<AppDbIdentityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("DataBase")));

            var authSettings = Configuration.GetSection(nameof(AuthSettings));
            services.Configure<AuthSettings>(authSettings);

            services.AddIdentity<AppUser, IdentityRole>(o =>
            {
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequiredLength = 10;
                

                o.User.RequireUniqueEmail = true;
                
                o.SignIn.RequireConfirmedEmail = true;
            })
             .AddEntityFrameworkStores<AppDbIdentityContext>()
             .AddDefaultTokenProviders();



            var secretKey = Configuration.GetSection("AuthSettings")["SecretKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var jwtAppSettingsOptions = Configuration.GetSection("JwtIssuerOptions");




            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<JwtSecurityOptions>(options =>
            {
                options.Issuer = jwtAppSettingsOptions["Issuer"];
                options.Audience = jwtAppSettingsOptions["Audience"];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingsOptions["Issuer"],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingsOptions["Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(confOptions =>
            {
                confOptions.ClaimsIssuer = jwtAppSettingsOptions["Issuer"];
                confOptions.TokenValidationParameters = tokenValidationParameters;
                confOptions.SaveToken = true;

                confOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim("rol", "api_access"));
            });

            services.AddCors();


            services.AddScoped<UserReposytory>();
            services.AddScoped<MessagesReposytory>();
            services.AddScoped<JwtFactory>();
            services.AddScoped<TokenFactory>();
            services.AddScoped<EmailTokenReposytory>();
            services.AddScoped<EmailActions>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
