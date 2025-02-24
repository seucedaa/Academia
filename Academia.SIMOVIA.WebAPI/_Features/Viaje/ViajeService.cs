using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje
{
    public class ViajeService
    {
        private readonly SIMOVIAContext _context;
        private readonly IMapper _mapper;
        public ViajeService(SIMOVIAContext simovia, IMapper mapper)
        {
            _context = simovia;
            _mapper = mapper;
        }
        #region Sucursales
        public async Task<Response<List<SucursalesDto>>> ObtenerSucursales()
        {
            try
            {
                var listado = await _context.Sucursales
                    .Where(c => c.Estado)
                    .ToListAsync();

                var sucursalesDto = _mapper.Map<List<SucursalesDto>>(listado);

                return new Response<List<SucursalesDto>>
                {
                    Exitoso = true,
                    Data = sucursalesDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<SucursalesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "sucursales")
                };
            }
            catch (Exception)
            {
                return new Response<List<SucursalesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
                };
            }
        }
        public async Task<Response<SucursalDto>> ObtenerSucursalPorId(int id)
        {
            try
            {
                var sucursal = await _context.Sucursales
                    .Where(c => c.SucursalId == id)
                    .Include(c => c.Ciudad)
                    .FirstOrDefaultAsync();

                if (sucursal == null)
                {
                    return new Response<SucursalDto>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.MSJ10.Replace("@Entidad", "sucursal")
                    };
                }

                var sucursalDto = _mapper.Map<SucursalDto>(sucursal);

                return new Response<SucursalDto>
                {
                    Exitoso = true,
                    Data = sucursalDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<SucursalDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "sucursal")
                };
            }
            catch (Exception)
            {
                return new Response<SucursalDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
                };
            }
        }

        private async Task<Response<int>> ValidarRegistrarDatosSucursal(SucursalDto sucursalDto)
        {
            var camposObligatorios = new List<(string Campo, bool EsValido)>
            {
                ("DNI", !string.IsNullOrEmpty(sucursalDto.Descripcion)),
                ("Nombres", !string.IsNullOrEmpty(sucursalDto.Telefono)),
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

            bool descripcionExiste = await _context.Sucursales.AnyAsync(c => c.Descripcion == sucursalDto.Descripcion);
            if (descripcionExiste)
            {
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.MSJ02.Replace("@Campo", "DNI") };
            }
            return new Response<int> { Exitoso = true };
        }

        private async Task<Response<int>> GuardarSucursal(SucursalDto sucursalDto)
        {
            var nuevoSucursal = _mapper.Map<Sucursales>(sucursalDto);

            try
            {
                _context.Sucursales.Add(nuevoSucursal);
                await _context.SaveChangesAsync();

                return new Response<int> { Exitoso = true, Mensaje = Mensajes.MSJ05.Replace("@Entidad", "Sucursal"), Data = nuevoSucursal.SucursalId };
            }
            catch (Exception)
            {
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.MSJ07.Replace("@Entidad", "sucursal") };
            }
        }

        public async Task<Response<int>> RegistrarSucursal(SucursalDto sucursalDto)
        {
            var validacion = await ValidarRegistrarDatosSucursal(sucursalDto);
            if (!validacion.Exitoso)
            {
                return validacion;
            }

            return await GuardarSucursal(sucursalDto);
        }
        #endregion
    }
}
