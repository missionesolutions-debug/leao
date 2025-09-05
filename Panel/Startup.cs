using Data;
using Framework.Infrastructure;
using Framework.Repositories;
using Framework.Repositories.Factories.xCode;
using Framework.Repositories.Interface.Core;
using Framework.Repositories.Services.Implementations.IAServices;
using Framework.Services.Interfaces.IAServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Panel.Services.Streaming.PandaVideo;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Painel
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

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));

            #region Assemblies

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ChatIARepository, ChatIARepository>();
            services.AddScoped<ChatIAItemRepository, ChatIAItemRepository>();

            services.AddHttpClient<IOpenAiService, OpenAiService>();
            services.AddHttpClient<IOpenAiRunsService, OpenAiRunsService>();
            services.AddHttpClient<IOpenAiThreadsService, OpenAiThreadsService>();

            services.AddScoped<IPageSectionRepository, PageSectionRepository>();
            services.AddScoped<IPageSectionItemRepository, PageSectionItemRepository>();
            services.AddScoped<PandaVideoService, PandaVideoService>();
            services.AddScoped<BlobService, BlobService>();

            // Carregar o assembly da biblioteca de classes "Framework"
            Assembly FrameworkAssembly = Assembly.Load("Framework.Repositories");

            // Obter todos os tipos de reposit�rios no assembly "Factory"
            var repositoryTypes = FrameworkAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Factory"));

            // Registrar os reposit�rios com escopo
            foreach (var repositoryType in repositoryTypes)
            {
                services.AddScoped(repositoryType);
            }

            // Obter todos os tipos de services no assembly "Framework"
            var servicesTypes = FrameworkAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"))
                    .Where(t => !t.GetConstructors().Any(c => c.GetParameters().Any(p => p.ParameterType == typeof(string))));

            // Registrar as services com escopo
            foreach (var servicesType in servicesTypes)
            {
                services.AddScoped(servicesType);
            }
            #endregion

            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(options =>
            {
                options.DetailedErrors = true;
            });

            services.AddMudServices();

            services.AddHttpContextAccessor();

            // Configuração de sessão
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication("CookieAuthentication")
                .AddCookie("CookieAuthentication", config =>
                {
                    config.Cookie.Name = "UserLoginCookie";
                    config.LoginPath = "/Login/Authentication";
                    config.AccessDeniedPath = "/Login/AccessDenied";
                });

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddRazorComponents().AddInteractiveServerComponents()
            .AddHubOptions(options =>
            {
                options.MaximumReceiveMessageSize = 10 * 1024 * 1024; // 10 MB
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Quem � voc�?
            app.UseAuthentication();

            #region Culture
            var cultureInfo = new CultureInfo("pt-BR");
            var culture = CultureInfo.CreateSpecificCulture("pt-BR");

            var dateformat = new DateTimeFormatInfo
            {
                ShortDatePattern = "dd/MM/yyyy",
                LongDatePattern = "dd/MM/yyyy hh:mm:ss tt"
            };

            culture.DateTimeFormat = dateformat;

            var supportedCultures = new[]
            {
                culture
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            #endregion

            // Verifica Permiss�es
            app.UseAuthorization();

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Middleware de sessão
            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
