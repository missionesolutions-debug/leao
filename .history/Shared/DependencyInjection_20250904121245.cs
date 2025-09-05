using Data;
using Framework.Data;
using Framework.Infrastructure;
using Framework.Infrastructure.Mailing.MessageProvider;
using Framework.Repositories;
using Framework.Repositories.Base;
using Framework.Repositories.Interface;
using Framework.Repositories.Interface.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DatabaseConnection"),
                    sqlServerOptions => sqlServerOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                ).AddInterceptors(new SqlCommandInterceptor())
            );

            #region Assemblies
            services.AddScoped<IEmailService, EmailService>();
            //services.AddScoped<IAccountService, AccountService>();
            //services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IPaginaRepository, PaginaRepository>();
            services.AddScoped<IPageSectionRepository, PageSectionRepository>();
            services.AddScoped<IPageSectionItemRepository, PageSectionItemRepository>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(BlobService), typeof(BlobService));

            #region Repositories
            Assembly FrameworkRepositories = Assembly.Load("Framework.Repositories");

            var repositoryTypes = FrameworkRepositories.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && (t.Name.EndsWith("Factory") || t.Name.EndsWith("Repository")));

            foreach (var repositoryType in repositoryTypes)
            {
                services.AddScoped(repositoryType);
            }
            #endregion

            #endregion

            return services;
        }
    }
}
