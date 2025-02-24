using Academia.SIMOVIA.WebAPI._Features.Viaje;
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
            return resultado.Exitoso ? Ok(resultado) : StatusCode(500, resultado.Mensaje);
        }
    }
}
