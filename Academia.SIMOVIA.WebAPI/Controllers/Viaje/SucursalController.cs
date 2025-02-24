using Microsoft.AspNetCore.Mvc;
using Academia.SIMOVIA.WebAPI._Features.Viaje;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;

namespace Academia.SIMOVIA.WebAPI.Controllers.Viaje
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly ViajeService _viajeService;
        public SucursalController(ViajeService viajeService)
        {
            _viajeService = viajeService;
        }

        [HttpGet("ObtenerSucursales")]
        public async Task<IActionResult> ObtenerSucursales()
        {
            var resultado = await _viajeService.ObtenerSucursales();
            return resultado.Exitoso ? Ok(resultado) : StatusCode(500, resultado.Mensaje);
        }

        [HttpGet("ObtenerSucursal/{id}")]
        public async Task<IActionResult> ObtenerSucursal(int id)
        {
            var resultado = await _viajeService.ObtenerSucursalPorId(id);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpPost("RegistrarSucursal")]
        public async Task<IActionResult> RegistrarSucursal([FromBody] SucursalDto SucursalDto)
        {
            var resultado = await _viajeService.RegistrarSucursal(SucursalDto);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }


        //[HttpPut("EditarSucursal")]
        //public async Task<IActionResult> EditarSucursal([FromBody] SucursalDto SucursalDto)
        //{
        //    var resultado = await _viajeService.EditarSucursal(SucursalDto);
        //    return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        //}
    }
}
