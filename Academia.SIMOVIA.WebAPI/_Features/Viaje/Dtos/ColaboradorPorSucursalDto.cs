using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ColaboradorPorSucursalDto
    {
        public int SucursalId { get; set; }
        public decimal DistanciaKm { get; set; }
    }
}
