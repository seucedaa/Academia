using Microsoft.AspNetCore.Mvc;
using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;

namespace Academia.SIMOVIA.WebAPI.Controllers.Viaje
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly ViajeService _viajeService;
        public SucursalController(ViajeService viajeService)
        {
            _viajeService = viajeService;
        }

        [HttpGet("ObtenerSucursales")]
        public async Task<IActionResult> ObtenerSucursales()
        {
            var resultado = await _viajeService.ObtenerSucursales();
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpGet("ObtenerSucursalesCercanas/{latitud}/{longitud}")]
        public async Task<IActionResult> ObtenerSucursalesCercanas([FromRoute] decimal latitud, [FromRoute] decimal longitud)
        {
            var resultado = await _viajeService.ObtenerSucursalesCercanas(latitud, longitud);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpGet("ObtenerSucursalesPorIds/{sucursalesIds}")]
        public async Task<IActionResult> ObtenerSucursalesPorIds([FromRoute] string sucursalesIds)
        {
            var ids = sucursalesIds.Split(',').Select(int.Parse).ToList();
            var resultado = await _viajeService.ObtenerSucursalesPorIds(ids);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }


        [HttpPost("RegistrarSucursal")]
        public async Task<IActionResult> RegistrarSucursal([FromBody] SucursalDto SucursalDto)
        {
            var resultado = await _viajeService.RegistrarSucursal(SucursalDto);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
    }
}
