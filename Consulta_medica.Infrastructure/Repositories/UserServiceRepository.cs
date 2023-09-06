using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Domain.DTOs.Response;
using Consulta_medica.Dto.Request;
using Consulta_medica.Infrastructure.SP;
using Consulta_medica.Interfaces;
using Consulta_medica.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;

namespace Consulta_medica.Repository
{
    public class UserServiceRepository : IUserServiceRepository
    {
        private readonly consulta_medicaContext _context;
        private readonly IConfiguration _configuration;

        public UserServiceRepository(consulta_medicaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<bool> restorePasswordUsuario(restoreUserDto request) 
        {
            var result = await _context.Usuarios.FirstOrDefaultAsync(x => x.sCorreo.Equals(request.sEmail));

            result.sPswd = request.sPswd;

           var recordAffected = await _context.SaveChangesAsync();

            return recordAffected > 0;
        }

        public async Task<bool> restorePasswordMedico(restoreUserDto request)
        {
            var result = await _context.Medico.FirstOrDefaultAsync(x => x.Correo.Equals(request.sEmail));

            result.Pswd = request.sPswd;

            var recordAffected = await _context.SaveChangesAsync();

            return recordAffected > 0;
        }

        public async Task<restoreUserDto> getEmail(string sEmail) 
        {

            using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("consulta_medica"));

            connection.Open();

            string sqlQuery = "select * from (\r\nselect sCorreo sEmail , sPswd sPswd , nIptip sRol from Usuarios u\r\nunion all\r\nselect m.correo , m.pswd, m.idtip from medico m\r\n)a where sEmail = @sEmail;";

            // Dapper query and object mapping
            var result = connection.QueryFirstOrDefault<restoreUserDto>(sqlQuery, new { sEmail = sEmail });

            return result;
        }

        public async Task<IEnumerable<UserListResponseDto>> getUser(RequestGenericFilter request, string usuario)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("consulta_medica"));

            var parameters = new
            {
                sDni = request.numFilter.Equals(0) ? request.textFilter : null,
                sNombreUsuario = request.numFilter.Equals(1) ? request.textFilter : null,
                sCodigoUser = request.numFilter.Equals(2) ? request.textFilter : null,
                sEmail = request.numFilter.Equals(3) ? request.textFilter : null,
                sFilterOne = request.sFilterOne,
                sFilterTwo = request.sFilterTwo,
                sUsuario = usuario
            };

            var result = await connection.QueryAsync<UserListResponseDto>(storeProcedure.SP_LISTADO_USER, parameters, commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task<bool> AddUser(Usuarios request)
        {
           await _context.AddAsync(request);

           var insert = await _context.SaveChangesAsync();
            
           return insert > 0;
        }

        public async Task<bool> UpdateUser(Usuarios request)
        {
            _context.Update(request);

            var recordAffected = await _context.SaveChangesAsync();

            return recordAffected > 0;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.nIdUser == id);
            
            _context.Remove(user);

            var delete = await _context.SaveChangesAsync();

            return delete > 0;
        }
       
    }
}
