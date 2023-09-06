using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
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
    public class MedicosRepository : IMedicosRepository
    {
        private readonly consulta_medicaContext _context;
        private readonly IConfiguration _configuration;
        public MedicosRepository(consulta_medicaContext context, IConfiguration configuration )
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Medico>> GetMedicos(RequestGenericFilter request, string usuario)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("consulta_medica"));

            var parameters = new
            {
                sDni = request.numFilter.Equals(0) ? request.textFilter : null,
                sNombreMedico = request.numFilter.Equals(1) ? request.textFilter : null,
                sCodigoMedico = request.numFilter.Equals(2) ? request.textFilter : null,
                sFilterOne =  request.sFilterOne,
                sFilterTwo = request.sFilterTwo,
                sUsuario = usuario
            };

            var result = await connection.QueryAsync<Medico>(storeProcedure.SP_LISTADO_MEDICOS, parameters, commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task<(bool,string)> AddMedico(MedicoRequestDto request)
        {
            var codmed = (from c in _context.Medico
                          orderby c.Codmed descending
                          select c.Codmed).FirstOrDefault();

            string[] ID = codmed.Split("M0");
            int Id = Convert.ToInt32(ID[1]) + 1;
            var IdFinal = Convert.ToString("M0" + Id);

            Medico omedico = new Medico();
            omedico.Codmed = IdFinal;
            omedico.Codes = request.Codes;
            omedico.Idtip = "U002";
            omedico.Nombre = request.Nombre;
            omedico.sApellidos = request.sApellidos;
            omedico.Sexo = request.Sexo;
            omedico.Nac = request.Nac;
            omedico.Correo = request.Correo;
            omedico.Pswd = request.Pswd;
            omedico.Dni = request.Dni;
            omedico.Idhor = request.Idhor;
            omedico.Asis = "No";
            omedico.nEstado = (int)GenericEnumRepository.Activo;
            _context.Medico.Add(omedico);
            var result = await _context.SaveChangesAsync();
    
            return (result > 0, omedico.Codmed);
        }
        public async Task<Medico> BusinessHours(string Codmed) 
        {
            var medico = await _context.Medico.FirstOrDefaultAsync(x => x.Codmed == Codmed);

            return medico;
        }
        public async Task<IEnumerable<HorarioDto>> ObtenerInfoHorario(string Codmed)
        {
            var paramCodMed = new SqlParameter("@Codmed", string.IsNullOrEmpty(Codmed) ? DBNull.Value : Codmed);

            var horarioObject = await _context.Set<HorarioDto>().FromSqlRaw("EXECUTE sp_obtener_horario @Codmed", parameters: new[] { paramCodMed }).ToListAsync();

            return horarioObject;
        }

        public async Task<bool> UpdateBodyHorario(string Codmed, string bodyMessage) 
        {
            var medico = await _context.Medico.FirstOrDefaultAsync(x => x.Codmed == Codmed);

            medico.sBodyHorario = bodyMessage;
            var recordAffected = await _context.SaveChangesAsync();
            return recordAffected > 0;
        }

        public async Task<bool> UpdateMedico(MedicoRequestDto request)
        {
            var omedico = await _context.Medico.FirstOrDefaultAsync(x => x.Codmed == request.Codmed);

            omedico.Codes = request.Codes ?? omedico.Codes;
            omedico.Nombre = request.Nombre ?? omedico.Nombre;
            omedico.sApellidos = request.sApellidos ?? omedico.sApellidos;
            omedico.Sexo = request.Sexo ?? omedico.Sexo;
            omedico.Nac = request.Nac ?? omedico.Nac;
            omedico.Correo = request.Correo ?? omedico.Correo;
            omedico.Pswd = request.Pswd ?? omedico.Pswd;
            omedico.Dni = request.Dni ?? omedico.Dni;
            omedico.Idhor = request.Idhor ?? omedico.Idhor;
            omedico.nEstado = request.nEstado;
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteMedico(string id, int nEstado)
        {
            var medico = await _context.Medico.FirstOrDefaultAsync(x => x.Codmed == id);
            medico.nEstado = nEstado;
            var recordAffected = await _context.SaveChangesAsync();
            return recordAffected > 0;
        }

        public string getEmailMedico(string CodMed) 
        {
            var result = _context.Medico.FirstOrDefault(x => x.Codmed.Equals(CodMed));

            return result.Correo;
        }
    }
}
