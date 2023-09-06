using Consulta_medica.Dto.Request;
using Consulta_medica.Infrastructure.Enum;
using Consulta_medica.Infrastructure.SP;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Consulta_medica.Repository
{
    public class CitasMedicasRepository : ICitasMedicasRepository
    {
        private readonly consulta_medicaContext _context;
        private readonly IConfiguration _configuration;
        public CitasMedicasRepository(consulta_medicaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<IEnumerable<CitasQueryDto>> GetCitas(RequestGenericFilter request, string usuario)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("consulta_medica"));

            var parameters = new 
            { 
                nCodigoCita = request.numFilter.Equals(0)?request.textFilter:null, 
                sNombreMedico = request.numFilter.Equals(1) ? request.textFilter :null,
                sNombrePaciente = request.numFilter.Equals(2) ? request.textFilter : null,
                sNombre_Especialidad = request.numFilter.Equals(3) ? request.textFilter : null,
                sFilterOne = request.sFilterOne,
                sFilterTwo = request.sFilterTwo,
                dFechaInicio = request.dFechaInicio,
                dFechaFin = request.dFechaFin,
                sUsuario = usuario
            };

            var result = await connection.QueryMultipleAsync(storeProcedure.SP_LISTADO_CITAS, parameters, commandType: CommandType.StoredProcedure);

            var queryReponse = (await result.ReadAsync<CitasQueryDto>()).ToList();

            var count = (await result.ReadAsync<object>()).FirstOrDefault();

            foreach (var item in queryReponse)
            {
                if (!string.IsNullOrEmpty(item.urlBase))
                {
                    byte[] contenidoPdfBytes = File.ReadAllBytes(item.urlBase);

                    string archivoPdfBase64 = Convert.ToBase64String(contenidoPdfBytes);

                    item.urlBase = archivoPdfBase64;
                }
            }

            return queryReponse;
        }
        public SqlGenericParameters filterGeneric(RequestGenericFilter request) 
        {
            SqlGenericParameters generic = new();
            generic.pFilterOne = new SqlParameter("@sFilterOne", request.sFilterOne != null?request.sFilterOne:DBNull.Value);
            generic.pFilterTwo = new SqlParameter("@sFilterTwo", request.sFilterTwo != null?request.sFilterTwo:DBNull.Value);
            return generic;
        }
        public async Task<(Citas, bool)> AddCitas(CitasRequestDto request)
        {
            var FinalHora = request.Hora?.Split(" ");

            Citas ocitas = new()
            {
                Dnip = request.Dnip,
                Codmed = request.Codmed,
                Feccit = request.Feccit,
                Estado = (int)GenericEnumRepository.PendienteAtencion,
                nEstado_Pago = (int)GenericEnumRepository.PendientePago,
                Codes = request.Codes,
                Hora = TimeSpan.Parse(FinalHora[0]?.ToString())
            };
            _context.Citas.Add(ocitas);

            return (ocitas, await _context.SaveChangesAsync() > 0);
        }
        public async Task<bool> UpdateCitas(CitasRequestDto request)
        {
            var cita = await _context.Citas.FirstOrDefaultAsync(x => x.Id == request.Id);
            var HoraFinal = request.Hora.Split(" ");
            //UPDATE
            cita.Id = request.Id;
            cita.Dnip = request.Dnip;
            cita.Codmed = request.Codmed;
            cita.Feccit = request.Feccit;
            cita.Estado = (int)GenericEnumRepository.PendienteAtencion;
            cita.Codes = request.Codes;
            cita.Hora = TimeSpan.Parse(HoraFinal[0].ToString());
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteCitas(int id)
        {
            var lst = await _context.Citas.FirstOrDefaultAsync(x => x.Id == id);
            _context.Citas.Remove(lst);
            var recordAffected = await _context.SaveChangesAsync();
            return recordAffected > 0;
        }

        public async Task<Horarios> consultarHorario(DatosRequestCitasDto request) 
        {
            var response = await (from m in _context.Medico
                                 join h in _context.Horario
                                 on m.Idhor equals h.Idhor
                                 where m.Codmed == request.codmed
                                 select new Horarios { Hinicio = h.Hinicio, Hfin = h.Hfin }).FirstOrDefaultAsync();

            return response;
        }

        public async Task<IEnumerable<TimeSpan>> citasRegistradas(DatosRequestCitasDto request) 
        {
            var citasRegistradas = await (from m in _context.Medico
                                          join c in _context.Citas
                                          on m.Codmed equals c.Codmed
                                          where m.Codmed == request.codmed
                                          && c.Feccit == request.Feccit
                                          select c.Hora).ToListAsync();
            return citasRegistradas;
        }
        public async Task<IEnumerable<HistoriaMedica>> obtenerHistoriaMedica(int dnip) 
        {
            var obtenerCita = await (from h in _context.HistorialMedico
                                     join e in _context.Especialidad
                                     on h.Codes equals e.Codes
                                     join m in _context.Medico
                                     on h.Codmed equals m.Codmed
                                     join p in _context.Paciente
                                     on h.Dnip equals p.Dnip
                                     where h.Dnip == dnip
                                     orderby h.Fecct descending
                                     select new HistoriaMedica
                                     {
                                         idCita = h.idCita,
                                         sNombre_Especialidad = e.Nombre,
                                         sNombre_Medico = m.Nombre,
                                         Dnip = h.Dnip,
                                         Nomp = p.Nomp,
                                         Fecct = h.Fecct,
                                         Diagnostico = h.Diagnostico,
                                         Receta = h.Receta
                                     }).ToListAsync();

            return obtenerCita;
        }

        public async Task<IEnumerable<CitasRequestDto>> getCitasPending(DateTime FechaMañana)
        {

            var requestList = await(from c in _context.Citas
                                    join p in _context.Paciente
                                    on c.Dnip equals p.Dnip
                                    join m in _context.Medico
                                    on c.Codmed equals m.Codmed
                                    join e in _context.Especialidad
                                    on c.Codes equals e.Codes
                                    where c.Feccit == FechaMañana
                                    && c.Estado == (int)GenericEnumRepository.PendienteAtencion // PENDIENTE 
                                    select new CitasRequestDto
                                    {
                                        Id = c.Id,
                                        Dnip = c.Dnip,
                                        NombrePaciente = p.Nomp,
                                        CorreoElectronico = p.correoElectronico,
                                        CorreoElectronicoMedico = m.Correo,
                                        Codmed = c.Codmed,
                                        Feccit = (DateTime)c.Feccit,
                                        nEstado = c.Estado,
                                        Codes = c.Codes,
                                        NombreMedico = m.Nombre + " " + m.sApellidos,
                                        NombreEspecialidad = e.Nombre,
                                        Hora = c.Hora.ToString("hh\\:mm"),
                                        Costo = e.Costo

                                    }).ToListAsync();

            return requestList;
        }

        public async Task<IEnumerable<CitasPendientesMedico>> getCitasPendingByCodmed(DateTime FechaPrevia, string Codmed) 
        {
            var citas_Pendientes = await (from c in _context.Citas
                                          join p in _context.Paciente
                                          on c.Dnip equals p.Dnip
                                          join e in _context.Especialidad
                                          on c.Codes equals e.Codes
                                          where c.Feccit == FechaPrevia
                                          && c.Codmed == Codmed
                                          select new CitasPendientesMedico 
                                          { 
                                              Nombre = e.Nombre, 
                                              Nomp = p.Nomp, 
                                              Apellidos = p.Apellidos, 
                                              Hora = c.Hora,
                                              Feccit = (DateTime)c.Feccit 
                                          }).ToListAsync();

            return citas_Pendientes;
        }
    }
}
