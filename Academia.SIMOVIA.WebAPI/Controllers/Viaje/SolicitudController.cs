using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Academia.SIMOVIA.WebAPI.Controllers.Viaje
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudController : Controller
    {
        private readonly ViajeService _viajeService;

        public SolicitudController(ViajeService vajeService)
        {
            _viajeService = vajeService;
        }

        [HttpGet("ObtenerSolicitudes")]
        public async Task<IActionResult> ObtenerSolicitudes()
        {
            var resultado = await _viajeService.ObtenerSolicitudes();
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpPost("RegistrarSolicitud")]
        public async Task<IActionResult> RegistrarSolicitud([FromBody] SolicitudDto solicitudDto)
        {
            var resultado = await _viajeService.RegistrarSolicitud(solicitudDto);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpPut("ProcesarSolicitud")]
        public async Task<IActionResult> ProcesarSolicitud([FromBody] ProcesarSolicitudDto solicitudDto)
        {
            var resultado = await _viajeService.ProcesarSolicitud(solicitudDto);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpPost("RegistrarViajePorSolicitud")]
        public async Task<IActionResult> RegistrarViajePorSolicitud([FromBody] RegistrarViajePorSolicitudDto dto)
        {
            var resultado = await _viajeService.RegistrarViajePorSolicitud(dto.Viaje, dto.SolicitudId);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpPut("RechazarSolicitud/{solicitudId}")]
        public async Task<IActionResult> RechazarSolicitud([FromRoute] int solicitudId)
        {
            var resultado = await _viajeService.RechazarSolicitud(solicitudId);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }


        [HttpPost("CancelarSolicitud")]
        public async Task<IActionResult> CancelarSolicitud([FromBody] CancelarSolicitudViajeDto solicitudDto)
        {
            var resultado = await _viajeService.CancelarSolicitud(solicitudDto);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
        [HttpPost("ProcesarCancelacionSolicitud")]
        public async Task<IActionResult> ProcesarCancelacionSolicitud([FromBody] ProcesarCancelarSolicitudDto solicitudDto)
        {
            var resultado = await _viajeService.ProcesarCancelacionSolicitud(solicitudDto);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }


    }
}
