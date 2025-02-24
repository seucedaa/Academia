using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    public class ColaboradoresPorSucursal
    {
        public int ColaboradorPorSucursalId { get; set; }
        public decimal DistanciaKm { get; set; }
        public int ColaboradorId { get; set; }
        public int SucursalId { get; set; }

        public virtual Colaboradores Colaborador { get; set; }
        public Sucursales Sucursal { get; set; }
    }
}
