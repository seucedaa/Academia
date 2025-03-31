using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class ViajeDto
    {
        public ViajeDto()
        {
            Colaboradores = new List<ViajeDetallesDto>();
        }
        public DateTime FechaHora { get; set; }
        public int SucursalId { get; set; }
        public int TransportistaId { get; set; }
        public int UsuarioCreacionId { get; set; }
        public List<ViajeDetallesDto> Colaboradores { get; set; }
    }

    public class ViajeDetallesDto
    {
        public int ColaboradorId { get; set; }
    }
}
