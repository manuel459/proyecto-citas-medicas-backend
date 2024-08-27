using Consulta_medica.Infrastructure.Interfaces;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using Consulta_medica.Repository;
using Microsoft.Extensions.Configuration;

namespace Consulta_medica.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly consulta_medicaContext _context;
        private readonly IConfiguration _config;
        public ICitasMedicasReporteRepository CitasMedicasReporte { get; private set; }

        public ICitasMedicasRepository Citas { get; private set; }

        public IConfiguracionesRepository Configuraciones { get; private set; }

        public IDiagnosticoRepository Diagnostico { get; private set; }

        public IGenerarPDF generarPDF { get; private set; }

        public IMedicosRepository Medicos { get; private set; }

        public IPacienteRepository Pacientes { get; private set; }

        public IPagosRepository Pagos { get; private set; }

        public IUserServiceRepository User { get; private set; }

        public INotificationRepository Notification { get; private set; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public UnitOfWork(consulta_medicaContext context, IConfiguration config) 
        {
            _context = context;
            _config = config;

            CitasMedicasReporte = new CitasMedicasReporteRepository(_context);
            Citas = new CitasMedicasRepository(_context,_config);
            Configuraciones = new ConfiguracionesRepository(_context);
            Diagnostico = new DiagnosticoRepository(_context);
            generarPDF = new GenerarPDF(_context, _config);
            Medicos = new MedicosRepository(_context,_config);
            Pacientes = new PacienteRepository(_context, _config);
            User = new UserServiceRepository(_context, _config);
            Notification = new NotificationRepository(_context);
        }
    }
}
