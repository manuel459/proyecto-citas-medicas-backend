using Consulta_medica.Infrastructure.Interfaces;
using Consulta_medica.Infrastructure.Repositories;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using Consulta_medica.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Materiagris.Smart.Infrastructure.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(consulta_medicaContext).Assembly.FullName;
            //services.AddDbContext<consulta_medicaContext>(
            //    op => op.UseSqlServer(configuration.GetConnectionString("consulta_medica"), b => b.MigrationsAssembly(assembly))
            //    , ServiceLifetime.Transient);


            services.AddDbContext<consulta_medicaContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("consulta_medica"), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(assembly);
                    //sqlOptions.EnableRetryOnFailure(
                    //    maxRetryCount: 5, // Número máximo de reintentos (puedes ajustar este número)
                    //    maxRetryDelay: TimeSpan.FromSeconds(30), // Tiempo máximo de espera entre reintentos
                    //    errorNumbersToAdd: null // Lista opcional de números de error adicionales a considerar transitorios
                    //);
                });

            }, ServiceLifetime.Transient);

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ICitasMedicasReporteRepository, CitasMedicasReporteRepository>();
            services.AddScoped<ICitasMedicasRepository, CitasMedicasRepository>();
            services.AddScoped<IConfiguracionesRepository, ConfiguracionesRepository>();
            services.AddScoped<IDiagnosticoRepository, DiagnosticoRepository>();
            services.AddScoped<IGenerarPDF, GenerarPDF>();
            services.AddScoped<IMedicosRepository, MedicosRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IPagosRepository, PagosRepository>();
            services.AddScoped<IUserServiceRepository, UserServiceRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}