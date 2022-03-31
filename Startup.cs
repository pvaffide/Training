//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.AspNetCore.Components.Server;

using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;

//using System.Security.Claims;
using System.Text.Json.Serialization;
//using System.Threading.Tasks;

using Training.Services;

using Training.Data;

namespace Training
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static void ConfigureDb(DbContextOptionsBuilder opt, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("TrainingDB");
            opt.UseSqlServer(connectionString);
#if DEBUG
            opt.EnableSensitiveDataLogging();
#endif
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddServerSideBlazor();
            // PVA : Test a supprimer
            services.AddSingleton<WeatherForecastService>();

            services.AddTransient<ProfilService>();

            //services.AddSingleton<ValidateAuthenticationMiddleware>();

            //services.AddTransient<UtilisateurNameService>();


            //services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            //services.AddScoped<CurrentUserProvider>();
            services.AddScoped<Radzen.DialogService>();
            services.AddScoped<Radzen.NotificationService>();
            services.AddScoped<ITrainingNotificationService, TrainingNotificationService>();
            //services.AddValidators();
            //services.AddScoped<ProfilService>();

            services.AddAutoMapper(typeof(AutoMapperConfig));
            services.AddDbContextFactory<TrainingDbContext>(opt =>
            {
                ConfigureDb(opt, Configuration);
            }, ServiceLifetime.Scoped /*<== nécessaire pour obtenir l'utilisateur depusi le context*/);

            services.AddMemoryCache();

            //services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
            //services.AddAuthorization(
            //    options =>
            //{
            //    options.AddPolicy("APIAccessAllowed", policy =>
            //        policy.RequireAuthenticatedUser().RequireRole(nameof(RoleUtilisateur.Offline_Writer)));
            //}
            //);
            //services.AddScoped<IClaimsTransformation, ClaimsTransformer>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
