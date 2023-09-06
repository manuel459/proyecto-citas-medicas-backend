using Consulta_medica.Domain.DTOs.Request;
using Consulta_medica.Domain.DTOs.Response;
using Consulta_medica.Dto.Request;
using Consulta_medica.Models;

namespace Consulta_medica.Interfaces
{
    public interface IUserServiceRepository
    {
        Task<bool> restorePasswordMedico(restoreUserDto request);
        Task<bool> restorePasswordUsuario(restoreUserDto request);
        Task<restoreUserDto> getEmail(string sEmail);
        Task<IEnumerable<UserListResponseDto>> getUser(RequestGenericFilter request, string usuario);
        Task<bool> AddUser(Usuarios request);
        Task<bool> UpdateUser(Usuarios request);
        Task<bool> DeleteUser(int id);
    }
}
