using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;

namespace Academia.SIMOVIA.WebAPI._Features.General.Dtos
{
    public class ColaboradorDto
    {
        public string DNI { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string DireccionExacta { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int EstadoCivilId { get; set; }
        public int CargoId { get; set; }
        public int CiudadId { get; set; }
        public int UsuarioCreacionId { get; set; }
        public List<ColaboradorPorSucursalDto> Sucursales { get; set; }
    }
    
}
