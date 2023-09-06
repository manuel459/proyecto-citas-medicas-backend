using Consulta_medica.Interfaces;

namespace Consulta_medica.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICitasMedicasReporteRepository CitasMedicasReporte { get; }
        ICitasMedicasRepository Citas { get; }
        IConfiguracionesRepository Configuraciones { get; }
        IDiagnosticoRepository Diagnostico { get; }
        IGenerarPDF generarPDF { get; }
        IMedicosRepository Medicos { get; }
        IPacienteRepository Pacientes { get; }
        IPagosRepository Pagos { get; }
        IUserServiceRepository User { get; }
    }
}
