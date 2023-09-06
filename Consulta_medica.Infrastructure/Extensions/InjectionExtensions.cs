using Consulta_medica.Infrastructure.Interfaces;
using Consulta_medica.Infrastructure.Repositories;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using Consulta_medica.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Materiagris.Smart.Infrastructure.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(consulta_medicaContext).Assembly.FullName;
            services.AddDbContext<consulta_medicaContext>(
                op => op.UseSqlServer(
                    configuration.GetConnectionString("consulta_medica"), b => b.MigrationsAssembly(assembly)), ServiceLifetime.Transient);

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