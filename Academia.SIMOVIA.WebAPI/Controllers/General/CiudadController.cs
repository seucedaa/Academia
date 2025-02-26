using Academia.SIMOVIA.WebAPI._Features.General;
using Microsoft.AspNetCore.Mvc;

namespace Academia.SIMOVIA.WebAPI.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    public class CiudadController : ControllerBase
    {
        private readonly GeneralService _generalService;
        public CiudadController(GeneralService generalService)
        {
            _generalService = generalService;
        }

        [HttpGet("ObtenerCiudades")]
        public async Task<IActionResult> ObtenerCiudades()
        {
            var resultado = await _generalService.ObtenerCiudades();
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
    }
}
