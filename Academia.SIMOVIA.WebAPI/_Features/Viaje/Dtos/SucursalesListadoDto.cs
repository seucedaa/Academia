﻿using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class SucursalesListadoDto
    {
        public int SucursalId { get; set; }
        public string Descripcion { get; set; }
        public string Telefono { get; set; }
        public string DireccionExacta { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string CiudadDescripcion { get; set; }
    }
}
