using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Academia.SIMOVIA.WebAPI.Controllers.Viaje
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViajeEncabezadoController : Controller
    {
        private readonly ViajeService _viajeService;
        public ViajeEncabezadoController(ViajeService viajeService)
        {
            _viajeService = viajeService;
        }

        [HttpGet("ObtenerViajes")]
        public async Task<IActionResult> ObtenerViajes()
        {
            var resultado = await _viajeService.ObtenerViajes();
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpPost("RegistrarViaje")]
        public async Task<IActionResult> RegistrarViaje([FromBody] ViajeDto viajeDto)
        {
            var resultado = await _viajeService.RegistrarViaje(viajeDto);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
        [HttpGet("ObtenerViaje/{viajeEncabezadoId}")]
        public async Task<IActionResult> ObtenerViaje([FromRoute] int viajeEncabezadoId)
        {
            var resultado = await _viajeService.ObtenerViaje(viajeEncabezadoId);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpGet("ObtenerViajesPorSucursales/{sucursalesIds}")]
        public async Task<IActionResult> ObtenerViajesPorSucursales([FromRoute] string sucursalesIds)
        {
            var ids = sucursalesIds.Split(',').Select(int.Parse).ToList();
            var resultado = await _viajeService.ObtenerViajesPorSucursales(ids);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpGet("ObtenerViajesDisponibles/{sucursalId?}/{fecha?}")]
        public async Task<IActionResult> ObtenerViajesDisponibles([FromRoute] int? sucursalId, [FromRoute] DateTime? fecha)
        {
            var resultado = await _viajeService.ObtenerViajesDisponibles(sucursalId, fecha);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
    }
}
