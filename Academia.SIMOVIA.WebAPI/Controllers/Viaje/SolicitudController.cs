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
        //[HttpGet("ObtenerSolicitudesViajeAsignado")]
        //public async Task<IActionResult> ObtenerSolicitudesViajeAsignadoObtenerSolicitudes()
        //{
        //    var resultado = await _viajeService.ObtenerSolicitudesViajeAsignado();
        //    return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        //}
        //[HttpGet("ObtenerSolicitudesViajeNoAsignado")]
        //public async Task<IActionResult> ObtenerSolicitudesViajeNoAsignado()
        //{
        //    var resultado = await _viajeService.ObtenerSolicitudesViajeNoAsignado();
        //    return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        //}
        //[HttpGet("ObtenerSolicitudesCancelacionViaje")]
        //public async Task<IActionResult> ObtenerSolicitudesCancelacionViaje()
        //{
        //    var resultado = await _viajeService.ObtenerSolicitudesCancelacionViaje();
        //    return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        //}

        [HttpPost("RegistrarSolicitud")]
        public async Task<IActionResult> RegistrarSolicitud([FromBody] SolicitudDto solicitudDto)
        {
            var resultado = await _viajeService.RegistrarSolicitud(solicitudDto);
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

        [HttpPost("AceptarCancelarViaje")]
        public async Task<IActionResult> AceptarCancelarViaje([FromBody] ProcesarCancelarSolicitudDto solicitudDto)
        {
            var resultado = await _viajeService.ProcesarCancelacionSolicitud(solicitudDto);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }


    }
}
