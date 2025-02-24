using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
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
