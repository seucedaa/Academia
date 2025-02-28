﻿using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Acceso;
using Academia.SIMOVIA.WebAPI._Features.General;
using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Microsoft.AspNetCore.Mvc;
using Academia.SIMOVIA.WebAPI._Features.Viaje;

namespace Academia.SIMOVIA.WebAPI.Controllers.General
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly GeneralService _generalService;
        public ColaboradorController(GeneralService accesoService)
        {
            _generalService = accesoService;
        }

        [HttpGet("ObtenerColaboradores")]
        public async Task<IActionResult> ObtenerColaboradores()
        {
            var resultado = await _generalService.ObtenerColaboradores();
            return resultado.Exitoso ? Ok(resultado) :  BadRequest(resultado.Mensaje);
        }

        [HttpGet("ObtenerColaboradoresDisponibles")]
        public async Task<IActionResult> ObtenerColaboradoresDisponibles([FromQuery] int sucursalId, [FromQuery] DateTime? fecha)
        {
            var resultado = await _generalService.ObtenerColaboradoresDisponibles(sucursalId, fecha);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

        [HttpPost("RegistrarColaborador")]
        public async Task<IActionResult> RegistrarColaborador([FromBody] ColaboradorDto colaboradorDto)
        {
            var resultado = await _generalService.RegistrarColaborador(colaboradorDto);
            return resultado.Exitoso ? Ok(resultado) : BadRequest(resultado.Mensaje);
        }

    }
}
