using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Microsoft.AspNetCore.Mvc;

namespace Academia.SIMOVIA.WebAPI.Controllers.Viaje
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportistaController : Controller
    {
        private readonly ViajeService _viajeService;
        public TransportistaController(ViajeService viajeService)
        {
            _viajeService = viajeService;
        }

        [HttpGet("ObtenerTransportistas")]
        public async Task<IActionResult> ObtenerTransportistas()
        {
            var resultado = await _viajeService.ObtenerTransportistas();
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
    }
}
