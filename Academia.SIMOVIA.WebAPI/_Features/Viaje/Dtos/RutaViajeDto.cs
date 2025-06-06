﻿using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class RutaViajeDto
    {
        public decimal LatitudOrigen { get; set; }
        public decimal LongitudOrigen { get; set; }
        public List<WaypointDto> Waypoints { get; set; } = new List<WaypointDto>();
    }

    public class WaypointDto
    {
        public WaypointDto()
        {
            Colaborador = string.Empty;
            DireccionExacta = string.Empty;
        }
        public string Colaborador { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public decimal? DistanciaKm { get; set; }
        public string DireccionExacta { get; set; }
    }

}
