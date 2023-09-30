using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
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
    public class PacienteRepository : IPacienteRepository
    {
        public readonly consulta_medicaContext _context;
        public readonly IConfiguration _configuration;
        public PacienteRepository(consulta_medicaContext context, IConfiguration configuration) 
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<PacienteResponseDto>> GetPacientes(RequestGenericFilter request, string usuario)
        {

            using var connection = new SqlConnection(_configuration.GetConnectionString("consulta_medica"));

            var parameters = new
            {
                sDni = request.numFilter.Equals(0) ? request.textFilter : null,
                sNombrePaciente = request.numFilter.Equals(1) ? request.textFilter : null,
                sNumero = request.numFilter.Equals(2) ? request.textFilter : null,
                sFilterOne = request.sFilterOne,
                sFilterTwo = request.sFilterTwo,
                dFechaInicio = request.dFechaInicio,
                dFechaFin = request.dFechaFin,
                sUsuario = usuario
            };

            var result = await connection.QueryAsync<PacienteResponseDto>(storeProcedure.SP_LISTADO_PACIENTE, parameters, commandType: CommandType.StoredProcedure);

            connection.Close();
            return result;
        }

        public SqlGenericParameters filterGeneric(RequestGenericFilter request)
        {
            SqlGenericParameters generic = new();
            generic.pFilterOne = new SqlParameter("@sFilterOne", request.sFilterOne != null ? request.sFilterOne : DBNull.Value);
            generic.pFilterTwo = new SqlParameter("@sFilterTwo", request.sFilterTwo != null ? request.sFilterTwo : DBNull.Value);
            return generic;
        }

        public async Task<bool> AddPaciente(PacienteRequestDto request)
        {
            Paciente opaciente = new Paciente();
            opaciente.Dnip = request.Dnip;
            opaciente.Idtip = "U003";
            opaciente.Nomp = request.Nomp;
            opaciente.Apellidos = request.Apellidos;
            opaciente.Numero = request.Numero;
            opaciente.Edad = request.edad;
            opaciente.correoElectronico = request.correoElectronico;
            _context.Paciente.Add(opaciente);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdatePaciente(PacienteRequestDto request)
        {
            var id = await _context.Paciente.Where(x => x.Dnip == request.Dnip).FirstOrDefaultAsync();
            if (id is not null)
            {
                id.Dnip = request.Dnip;
                id.Nomp = request.Nomp;
                id.Numero = request.Numero;
                id.Edad = request.edad;
                id.correoElectronico = request.correoElectronico;
                var result = await _context.SaveChangesAsync();

                return result > 0;
            }
            else 
            {
                return false;
            }
        }

        public async Task<bool> DeletePaciente(int id)
        {
            var paciente = await _context.Paciente.FirstOrDefaultAsync(x => x.Dnip == id);
            _context.Paciente.Remove(paciente);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Paciente>> getPacientes() 
        {
            var response = await _context.Paciente.ToListAsync();
            return response;
        }
    }
}
