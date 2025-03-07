using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ColaboradorUbicacionDto
    {
        public int ColaboradorId { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }

}
