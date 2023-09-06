using Consulta_medica.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consulta_medica.Interfaces
{
    public interface IDiagnosticoRepository
    {
        public Task<bool> getUpdDiagnostico(DiagnosticoRequestPdfDto request);
    }
}
