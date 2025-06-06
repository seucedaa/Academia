﻿using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class SolicitudesDto
    {
        public SolicitudesDto()
        {
            Descripcion = string.Empty;
            Usuario = string.Empty;
            ViajeEncabezado = string.Empty;
            Sucursal = string.Empty;
            EstadoSolicitud = string.Empty;
        }
        public int SolicitudId { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string? ViajeEncabezado { get; set; }
        public string Sucursal { get; set; }
        public string EstadoSolicitud { get; set; }
        public bool AgregarViajeSiguiente { get; set; }
    }
}
