using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    public class ViajeDto
    {
        public int ViajeEncabezadoId { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal TarifaTransportista { get; set; }
        public int SucursalId { get; set; }
        public int TransportistaId { get; set; }
        public int UsuarioGuardaId { get; set; }
        public List<ViajeDetallesDto> Colaboradores { get; set; }
    }

    public class ViajeDetallesDto
    {
        public int ColaboradorId { get; set; }
    }
}
