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

using Autofac;

using Infrustructure;
using Infrustructure.Tokens;
using Infrustructure.Data;
using Infrustructure.Identity;
using Core;
using API.Actions;
using API.ViewModels.Settings;
using API.Presenters;



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

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("Infrastructure")));
            services.AddDbContext<AppDbIdentityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("Infrastructure")));

            var authSettings = Configuration.GetSection(nameof(AuthSettings));
            services.Configure<AuthSettings>(authSettings);

            services.AddIdentityCore<AppUser>(o =>
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

            services.AddScoped<EmailActions>();
            services.AddScoped<RegisterUserPresenter>();
            services.AddScoped<LoginUserPresenter>();
            services.AddScoped<RefreshTokenPresenter>();

            services.AddOptions();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new InfrustructureModule());
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
