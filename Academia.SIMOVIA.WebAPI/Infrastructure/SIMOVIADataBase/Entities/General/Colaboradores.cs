using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General
{
    public class Colaboradores
    {
        public Colaboradores()
        {
            DNI = string.Empty;
            Nombres = string.Empty;
            Apellidos = string.Empty;
            CorreoElectronico = string.Empty;
            Telefono = string.Empty;
            Sexo = string.Empty;
            DireccionExacta = string.Empty;
            Estado = true;

            Usuarios = new HashSet<Usuarios>();
            ColaboradoresPorSucursal = new HashSet<ColaboradoresPorSucursal>();
            ViajesDetalle = new HashSet<ViajesDetalle>();
        }

        public int ColaboradorId { get; set; }
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
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual EstadosCiviles EstadoCivil { get;set; }
        public virtual Ciudades Ciudad { get; set; }
        public virtual Cargos Cargo { get; set; }
        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ICollection<Usuarios> Usuarios { get; set; }
        public virtual ICollection<ColaboradoresPorSucursal> ColaboradoresPorSucursal { get; set; }
        public virtual ICollection<ViajesDetalle> ViajesDetalle { get; set; }
    }
}
