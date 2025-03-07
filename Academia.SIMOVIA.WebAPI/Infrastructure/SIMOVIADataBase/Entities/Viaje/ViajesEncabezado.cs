using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    [ExcludeFromCodeCoverage]
    public class ViajesEncabezado
    {
        public ViajesEncabezado()
        {
            Estado = true;
            ViajesDetalle = new HashSet<ViajesDetalle>();
            Puntuaciones = new HashSet<Puntuaciones>();
            Solicitudes = new HashSet<Solicitudes>();
        }

        public int ViajeEncabezadoId { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal DistanciaTotalKm { get; set; }
        public decimal TarifaTransportista { get; set; }
        public decimal Total { get; set; }
        public int SucursalId { get; set; }
        public int TransportistaId { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Sucursales Sucursal { get; set; }
        public virtual Transportistas Transportista { get; set; }
        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ICollection<ViajesDetalle> ViajesDetalle { get; set; }
        public virtual ICollection<Puntuaciones> Puntuaciones { get;set; }
        public virtual ICollection<Solicitudes> Solicitudes { get;set; }
    }
}
