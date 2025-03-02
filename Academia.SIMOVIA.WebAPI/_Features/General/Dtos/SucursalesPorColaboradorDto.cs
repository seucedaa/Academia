using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;

namespace Academia.SIMOVIA.WebAPI._Features.General.Dtos
{
    public class SucursalesPorColaboradorDto
    {
        public int ColaboradorId { get; set; }
        public int UsuarioModificacionId { get; set; }
        public List<ColaboradorPorSucursalDto> Sucursales { get; set; }
    }
}
