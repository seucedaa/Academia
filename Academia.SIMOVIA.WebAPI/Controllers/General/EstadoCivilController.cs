using Academia.SIMOVIA.WebAPI._Features.General;
using Microsoft.AspNetCore.Mvc;

namespace Academia.SIMOVIA.WebAPI.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoCivilController : ControllerBase
    {
        private readonly GeneralService _generalService;
        public EstadoCivilController(GeneralService generalService)
        {
            _generalService = generalService;
        }

        [HttpGet("ObtenerEstadosCiviles")]
        public async Task<IActionResult> ObtenerEstadosCiviles()
        {
            var resultado = await _generalService.ObtenerEstadosCiviles();
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
    }
}
