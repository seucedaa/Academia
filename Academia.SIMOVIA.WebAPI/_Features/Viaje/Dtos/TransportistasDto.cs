using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class TransportistasDto
    {
        public TransportistasDto()
        {
            DNI = string.Empty;
            Nombres = string.Empty;
            Apellidos = string.Empty;
            Telefono = string.Empty;
            CiudadDescripcion = string.Empty;
            MonedaSimbolo = string.Empty;
        }

        public int TransportistaId { get; set; }
        public string DNI { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public decimal Tarifa { get; set; }
        public string CiudadDescripcion { get; set; }
        public string MonedaSimbolo { get; set; }
    }
}
