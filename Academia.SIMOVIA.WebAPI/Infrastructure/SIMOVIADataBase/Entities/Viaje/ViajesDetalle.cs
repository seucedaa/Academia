using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    [ExcludeFromCodeCoverage]
    public class ViajesDetalle
    {
        public ViajesDetalle()
        {
            Estado = true;
        }

        public int ViajeDetalleId { get; set; }
        public int ViajeEncabezadoId { get; set; }
        public int ColaboradorId { get; set; }
        public bool Estado { get; set; }

        public virtual ViajesEncabezado ViajeEncabezado { get; set; }
        public virtual Colaboradores Colaborador { get; set; }
    }
}
