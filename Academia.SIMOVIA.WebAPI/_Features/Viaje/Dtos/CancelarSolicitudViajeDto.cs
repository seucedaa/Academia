﻿using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class CancelarSolicitudViajeDto
    {
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public int? ViajeEncabezadoId { get; set; }
    }
}
