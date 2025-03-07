using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ViajeReporteEncabezadoDto
    {
        public int ViajeEncabezadoId { get; set; }
        public string FechaHora { get; set; }
        public decimal DistanciaTotalKm { get; set; }
        public string TarifaTransportista { get; set; }
        public string Total { get; set; }
        public string Sucursal { get; set; }
        public string Transportista { get; set; }
        public string Pais { get; set; }
        public string Moneda { get; set; }
        public string MonedaSimbolo { get; set; }
        public string FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
    }

    public class ViajeReporteDetalleDto
    {
        public int ViajeEncabezadoId { get; set; }
        public int ColaboradorId { get; set; }
        public string Colaborador { get; set; }
        public decimal? DistanciaKm { get; set; }
        public string DireccionExacta { get; set; }
    }

    public class TotalPagarDto
    {
        public string TotalPagar { get; set; }
    }

    public class ViajeReporteResponseDto
    {
        public List<ViajeReporteEncabezadoDto> Encabezados { get; set; }
        public List<ViajeReporteDetalleDto> Detalles { get; set; }
        public TotalPagarDto TotalPagar { get; set; }
    }

}
