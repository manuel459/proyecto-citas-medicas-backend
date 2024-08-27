using Consulta_medica.Application.Interfaces;
using Consulta_medica.Application.Services;
using Consulta_medica.Application.Validation;
using Consulta_medica.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Consulta_medica.Application.Extensions
{
    public static class InjectionConnection
    {
        public static IServiceCollection AddInjectionAplication(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddScoped<ICitasMedicasService, CitasMedicasService>();
            services.AddScoped<IMedicosService, MedicosService>();
            services.AddScoped<IPacienteServices, PacienteService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDiagnosticoService, DiagnosticoService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ValidationPaciente>();
            services.AddScoped<ValidationMedik>();
            services.AddScoped<ValidationCitas>();
            services.AddScoped<ValidateRestorePassword>();
            return services;
        }
    }
}
