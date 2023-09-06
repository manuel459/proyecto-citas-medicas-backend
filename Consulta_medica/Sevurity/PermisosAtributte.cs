using Consulta_medica.Infrastructure.Enum;
using Consulta_medica.Infrastructure.SP;
using Consulta_medica.Models;
using Consulta_medica.Sevurity.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Consulta_medica.Sevurity
{
    public class PermisosAtributte: ActionFilterAttribute
    {
        private readonly PermissionOperator _permissionOperator;
        private readonly string[] _permissions;

        public PermisosAtributte(PermissionOperator permissionOperator, params string[] permissions)
        {
            _permissionOperator = permissionOperator;
            _permissions = permissions;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string authorizationHeader = context.HttpContext.Request.Headers["Authorization"];

            string token = authorizationHeader.Substring("Bearer ".Length);

            var jwtToken = new JwtSecurityToken(token);
            var idTip = jwtToken.Payload.FirstOrDefault(x => x.Key.Equals("actort")).Value;

            using (var _dbContext = new consulta_medicaContext())
            {
                switch (_permissionOperator)
                {
                    case PermissionOperator.And:
                        //var listado = _dbContext.Permisos.Where(x => x.idtip == idTip.ToString()).Select(x => x.sSlug).ToList();

                        var listado = (from pr in _dbContext.Permisos_Roles
                                         join p in _dbContext.Permisos
                                         on pr.nId_Permiso equals p.nId
                                         where pr.idtip.Equals(idTip) 
                                               && p.nEstado.Equals((int)GenericEnumRepository.Activo)
                                               && pr.nEstado.Equals((int)GenericEnumRepository.Activo)
                                       select p.sSlug).ToList();

                        foreach (var item in _permissions)
                        {
                            bool result = listado.FirstOrDefault(x => x == item) != null;

                            if (!result)
                            {
                                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                                return;
                            }
                        }
                        break;
                    case PermissionOperator.Or:
                        //var listado2 = _dbContext.Permisos.Where(x => x.idtip == idTip.ToString() && _permissions.Contains(x.sSlug)).ToList();

                        var listado2 = (from pr in _dbContext.Permisos_Roles
                                       join p in _dbContext.Permisos
                                       on pr.nId_Permiso equals p.nId
                                       where p.nEstado.Equals((int)GenericEnumRepository.Activo)
                                             && pr.nEstado.Equals((int)GenericEnumRepository.Activo)
                                             && pr.idtip.Equals(idTip) 
                                             && _permissions.Contains(p.sSlug)
                                        select p.sSlug).ToList();

                        if (listado2.Count() == 0)
                        {
                            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                            return;
                        }
                        break;
                }
             }

             base.OnActionExecuting(context);

        }
    }
}
