using Consulta_medica.Dto.Request;
using Consulta_medica.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Interfaces
{
    public interface ICitasMedicasReporteRepository
    {
        Task<IEnumerable<CitasMedicasReporteResponse>> getReporte(RequestFilterDtoNew request);
    }
}
