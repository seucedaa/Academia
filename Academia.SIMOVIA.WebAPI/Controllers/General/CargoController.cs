using Academia.SIMOVIA.WebAPI._Features.General;
using Microsoft.AspNetCore.Mvc;

namespace Academia.SIMOVIA.WebAPI.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly GeneralService _generalService;
        public CargoController(GeneralService generalService)
        {
            _generalService = generalService;
        }

        [HttpGet("ObtenerCargos")]
        public async Task<IActionResult> ObtenerCargos()
        {
            var resultado = await _generalService.ObtenerCargos();
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }
    }
}
