using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.General.Dtos
{
    [ExcludeFromCodeCoverage]
    public class SucursalesPorColaboradorDto
    {
        public SucursalesPorColaboradorDto()
        {
            Sucursales = new List<ColaboradorPorSucursalDto>();
        }
        public int ColaboradorId { get; set; }
        public int UsuarioModificacionId { get; set; }
        public List<ColaboradorPorSucursalDto> Sucursales { get; set; }
    }
}
