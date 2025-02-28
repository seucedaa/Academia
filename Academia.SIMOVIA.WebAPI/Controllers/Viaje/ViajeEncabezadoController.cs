using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI._Features.General;
using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Microsoft.AspNetCore.Mvc;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;

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
        [HttpGet("Ruta/{viajeEncabezadoId}")]
        public async Task<IActionResult> ObtenerRutaViaje([FromRoute] int viajeEncabezadoId)
        {
            var resultado = await _viajeService.ObtenerRutaViaje(viajeEncabezadoId);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpGet("ObtenerViajesDisponibles/{sucursalId?}/{fecha?}")]
        public async Task<IActionResult> ObtenerViajesDisponibles([FromRoute] int? sucursalId,[FromRoute] DateTime? fecha)
        {
            var resultado = await _viajeService.ObtenerViajesDisponibles(sucursalId, fecha);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
    }
}
