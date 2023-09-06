using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using Consulta_medica.Models;

namespace Consulta_medica.Application.Interfaces
{
    public interface IUserService
    {
        UserResponseDto Auth(LogeoRequestDto model);
        string GetToken<T>(string codigo, string email, string Idtip);
        Task<Response> restorePassword(restorePasswordRequestDto request);
        Task<Response> GetUser(RequestGenericFilter request, string usuario);
        Task<Response> AddUser(Usuarios request);
        Task<Response> UpdateUser(Usuarios request);
        Task<Response> DeleteUser(int id);
    }
}
