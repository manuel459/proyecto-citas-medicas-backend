﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Consulta_medica.Dto.Request
{
    public class MedicoRequestDto
    {
        public string? Codmed { get; set; }
        public string? Codes { get; set; }
        //public string? Idtip { get; set; }
        public string? Nombre { get; set; }
        public string? sApellidos { get; set; }
        public string? Sexo { get; set; }
        public DateTime? Nac { get; set; }
        public string? Correo { get; set; }
        public string? Pswd { get; set; }
        public int? Dni { get; set; }
        public string? Idhor { get; set; }
       // public string? Asis { get; set; }
        //public string? NomEspecialidad { get; set; }
        public int nEstado { get; set; }                   

        public bool bActiveNotificaciones { get; set; }
    }
}
