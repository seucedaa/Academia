using Academia.SIMOVIA.WebAPI._Features.Acceso;
using Academia.SIMOVIA.WebAPI._Features.General;
using Microsoft.AspNetCore.Mvc;

namespace Academia.SIMOVIA.WebAPI.Controllers.Acceso
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
            private readonly AccesoService _accesoService;

        public RolController(AccesoService accesoService)
        {
            _accesoService = accesoService;
        }

        [HttpGet("ObtenerRoles")]
        public async Task<IActionResult> ObtenerRoles()
        {
            var resultado = await _accesoService.ObtenerRoles();
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
    }
}
