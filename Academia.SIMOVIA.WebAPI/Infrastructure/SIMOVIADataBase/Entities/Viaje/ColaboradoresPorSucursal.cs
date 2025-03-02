using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    public class ColaboradoresPorSucursal
    {
        public int ColaboradorPorSucursalId { get; set; }
        public decimal DistanciaKm { get; set; }
        public int ColaboradorId { get; set; }
        public int SucursalId { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Colaboradores Colaborador { get; set; }
        public Sucursales Sucursal { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
    }
}
