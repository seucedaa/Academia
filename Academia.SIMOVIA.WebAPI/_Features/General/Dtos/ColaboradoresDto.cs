using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.General.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ColaboradoresDto
    {
        public ColaboradoresDto()
        {
            DNI = string.Empty;
            Nombres = string.Empty;
            Apellidos = string.Empty;
            CorreoElectronico = string.Empty;
            Telefono = string.Empty;
            Sexo = string.Empty;
            FechaNacimiento = string.Empty;
            DireccionExacta = string.Empty;
            EstadoCivilDescripcion = string.Empty;
            CargoDescripcion = string.Empty;
            CiudadDescripcion = string.Empty;
        }

        public int ColaboradorId { get; set; }
        public string DNI { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string Sexo { get; set; }
        public string FechaNacimiento { get; set; }
        public string DireccionExacta { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string EstadoCivilDescripcion { get; set; }
        public string CargoDescripcion { get; set; }
        public string CiudadDescripcion { get; set; }
    }
}
