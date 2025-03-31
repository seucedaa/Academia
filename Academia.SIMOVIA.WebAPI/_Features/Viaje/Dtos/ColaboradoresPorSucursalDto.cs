using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ColaboradoresPorSucursalDto
    {
        public ColaboradoresPorSucursalDto()
        {
            DNI = string.Empty;
            Nombre = string.Empty;
            CorreoElectronico = string.Empty;
            Telefono = string.Empty;
            DireccionExacta = string.Empty;
            CiudadDescripcion = string.Empty;
            DistanciaKm = 0;
            Latitud = 0;
            Longitud = 0;
        }

        public int ColaboradorId { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string DireccionExacta { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string CiudadDescripcion { get; set; }
        public decimal DistanciaKm { get; set; }
    }
}
