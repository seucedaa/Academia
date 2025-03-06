using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje
{
    public interface IUbicacionService
    {
        Task<decimal> CalcularDistanciaViaje(decimal latOrigen, decimal lonOrigen, List<(decimal Latitud, decimal Longitud)> colaboradores);
        Task<DistanceMatrixApiResponseDto> ObtenerDistanciasSucursales(decimal latitud, decimal longitud, List<Sucursales> sucursales);
    }
}