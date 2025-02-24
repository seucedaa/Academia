using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje
{
    public class ViajeDomainService
    {
        private readonly UnitOfWorkBuilder _unitOfWorkBuilder;

        public ViajeDomainService(UnitOfWorkBuilder unitOfWorkBuilder)
        {
            _unitOfWorkBuilder = unitOfWorkBuilder;
        }

        public async Task<Response<int>> ValidarRegistrarDatosSucursal(SucursalDto sucursalDto)
        {

            var camposObligatorios = new List<(string Campo, bool EsValido)>
            {
                ("Descripcion", !string.IsNullOrEmpty(sucursalDto.Descripcion)),
                ("Telefono", !string.IsNullOrEmpty(sucursalDto.Telefono)),
                ("Dirección Exacta", !string.IsNullOrEmpty(sucursalDto.DireccionExacta)),
                ("Latitud", sucursalDto.Latitud != 0),
                ("Longitud", sucursalDto.Longitud != 0),
                ("Ciudad", sucursalDto.CiudadId > 0),
                ("Usuario Creacion", sucursalDto.UsuarioGuardaId > 0)
            };

            var campoFaltante = camposObligatorios.FirstOrDefault(c => !c.EsValido);
            if (campoFaltante.Campo != null && !campoFaltante.EsValido)
            {
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.MSJ09.Replace("@Campo", campoFaltante.Campo) };
            }

            
            return new Response<int> { Exitoso = true };
        }
    }
}
