using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    [ExcludeFromCodeCoverage]
    public class ColaboradoresPorSucursal
    {

        public ColaboradoresPorSucursal()
        {
            Colaborador = new Colaboradores();
            Sucursal = new Sucursales();
            UsuarioModificacion = new Usuarios();
        }
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
