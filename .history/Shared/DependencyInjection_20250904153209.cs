using Data;
using Framework.Infrastructure;
using Framework.Repositories;
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
                )
            );

            #region Assemblies
            // Adicione os serviços que realmente existem nos seus projetos
            // Exemplo:
            // services.AddScoped<IEmailService, EmailService>();

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