using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

using DataBase.Entities;
using DataBase.Identity;
using DataBase.Interfaces;
using DataBase.Repositories;
using DataBase;
using API.Tokens;


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


            services.AddScoped<UserReposytory>();
            services.AddScoped<MessagesReposytory>();
            services.AddScoped<JwtFactory>();
            services.AddScoped<TokenFactory>();

            services.Configure<JwtSecurityOptions>(options =>
            {
                options.Issuer = jwtAppSettingsOptions["Issuer"];
                options.Audience = jwtAppSettingsOptions["Audience"];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
