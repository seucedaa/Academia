using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ViajesDto
    {
        public ViajesDto()
        {
            SucursalDescripcion = string.Empty;
            Transportista = string.Empty;
        }
        public int ViajeEncabezadoId { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal DistanciaTotalKm { get; set; }
        public decimal TarifaTransportista { get; set; }
        public decimal Total { get; set; }
        public string SucursalDescripcion { get; set; }
        public string Transportista { get; set; }
    }
}
