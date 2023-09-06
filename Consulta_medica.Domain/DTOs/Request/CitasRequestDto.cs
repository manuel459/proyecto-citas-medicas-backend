using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consulta_medica.Dto.Request
{
    public class CitasRequestDto
    {
        public int Id { get; set; }

        //Datos del Paciente 
        public int? Dnip { get; set; }
        public string? NombrePaciente { get; set; }
        public string? CorreoElectronico { get; set; }
        //End
        public string? Codmed { get; set; }
        public DateTime Feccit { get; set; }
        public int nEstado { get; set; }
        public string? Codes { get; set; }
        public string? NombreEspecialidad { get; set; }
        public string? NombreMedico { get; set; }
        public string? Hora { get; set; }
        public decimal? Costo { get; set; }
        public bool bActiveNotificaciones { get; set; }
        [NotMapped]
        public string? CorreoElectronicoMedico { get; set; }
    }

    public class Content
    {
        public TimeSpan contentCitas { get; set; }
        public bool status { get; set; }
    }

    public class DatosRequestCitasDto
    {
        public string? codmed { get; set; }
        public DateTime Feccit { get; set; }
    }

    public class CitasRequestDniDto 
    {
        public int? Dnip { get; set; }
    }

    public class CitasRequestCodmedDto 
    {
        public string codmed { get; set; }
    }

    public class CitasQueryDto 
    {
        public int id { get; set; }
        public int dnip { get; set; }
        public string? sNombre_Paciente { get; set; }
        public int nNumero_Paciente { get; set; }
        public string? codmed { get; set; }
        public string? sNombre_Medico { get; set; }
        public DateTime feccit { get; set; }
        public int nEstado { get; set; }
        public string? sEstado { get; set; }
        public string? hora { get; set; }
        public string? Codes { get; set; }
        public string? sNombre_Especialidad { get; set; }
        public int nEstado_Pago { get; set; }
        public string? sEstado_Pago { get; set; }
        public decimal costo { get; set; }
        public int? nId_Historico { get; set; }
        public string? Diagnostico { get; set; }
        public string? Receta { get; set; }
        public int nTotal_Historicos { get; set; }
        public string? urlBase { get; set; }
    }

    public class CitasQueryDtoReport
    {
        public int id { get; set; }
        public int dnip { get; set; }
        public string? sNombre_Paciente { get; set; }
        public string? codmed { get; set; }
        public string? sNombre_Medico { get; set; }
        public DateTime feccit { get; set; }
        public int nEstado { get; set; }
        public string? sEstado { get; set; }
        public string? hora { get; set; }
        public string? Codes { get; set; }
        public string? sNombre_Especialidad { get; set; }
        public int nEstado_Pago { get; set; }
        public string? sEstado_Pago { get; set; }
        public decimal costo { get; set; }
    }

    public class CitasPendientesMedico 
    {
        public string Nombre { get; set; }
        public string Nomp { get; set; } 
        public string Apellidos { get; set; } 
        public TimeSpan Hora { get; set; }
        public DateTime Feccit { get; set; }
    }

    public class Horarios 
    {
        public TimeSpan? Hinicio { get; set; } 
        public TimeSpan? Hfin { get; set; }
    }
    public class HistoriaMedica
    {
        public int idCita { get; set; }
        public string sNombre_Especialidad { get; set; }
        public string sNombre_Medico { get; set; }
        public int Dnip { get; set; }
        public string Nomp { get; set; }
        public DateTime Fecct { get; set; }
        public string Diagnostico { get; set; }
        public string Receta { get; set; }
    }
}
