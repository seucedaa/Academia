using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Microsoft.AspNetCore.Mvc;

namespace Academia.SIMOVIA.WebAPI.Controllers.Viaje
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : Controller
    {
        private readonly ViajeService _viajeService;

        public ReporteController(ViajeService viajeService)
        {
            _viajeService = viajeService;
        }

        [HttpGet("Reporte/{transportistaId}/{fechaInicio:datetime}/{fechaFin:datetime}")]
        public async Task<IActionResult> ObtenerReporteViajes([FromRoute] int transportistaId,[FromRoute] DateTime fechaInicio,
            [FromRoute] DateTime fechaFin)
        {
            var resultado = await _viajeService.ObtenerReporteViajes(transportistaId, fechaInicio, fechaFin);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
    }
}
