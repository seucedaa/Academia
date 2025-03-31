using Academia.SIMOVIA.WebAPI._Features.Acceso;
using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Academia.SIMOVIA.WebAPI.Controllers.Acceso
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AccesoService _accesoService;
        public UsuarioController(AccesoService accesoService)
        {
            _accesoService = accesoService;
        }

        [HttpGet("ObtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var resultado = await _accesoService.ObtenerUsuarios();
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpPost("InicioSesion")]
        public async Task<IActionResult> InicioSesion([FromBody] InicioSesionDto login)
        {
            var resultado = await _accesoService.InicioSesion(login);

            if (resultado.Exitoso)
                return Ok(resultado);

            switch (resultado.Mensaje)
            {
                case Mensajes.ERROR_BASE_DE_DATOS:
                    return StatusCode(StatusCodes.Status500InternalServerError, resultado);
                default:
                    return BadRequest(resultado);
            }
        }

    }
}
