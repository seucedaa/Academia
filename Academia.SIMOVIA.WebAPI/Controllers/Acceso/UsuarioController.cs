using Academia.SIMOVIA.WebAPI._Features.Acceso;
using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Academia.SIMOVIA.WebAPI.Controllers.Acceso
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly AccesoService _accesoService;
        public UsuarioController(AccesoService accesoService) {
            _accesoService = accesoService;
        }

        [HttpGet("ObtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var resultado = await _accesoService.ObtenerUsuarios();
            return resultado.Exitoso ? Ok(resultado) : StatusCode(500, resultado.Mensaje);
        }

        [HttpPost("InicioSesion")]
        public async Task<IActionResult> InicioSesion([FromBody] InicioSesionDto login)
        {
            var resultado = await _accesoService.InicioSesion(login);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }


        [HttpPost("RegistrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioDto usuarioDto)
        {
            var resultado = await _accesoService.RegistrarUsuario(usuarioDto);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

    }
}
