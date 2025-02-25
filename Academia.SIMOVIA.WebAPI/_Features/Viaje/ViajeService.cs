using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using AutoMapper;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje
{
    public class ViajeService
    {
        private readonly UnitOfWorkBuilder _unitOfWorkBuilder;
        private readonly IMapper _mapper;
        private readonly ViajeDomainService _viajeDomainService;
        private readonly Farsiman.Domain.Core.Standard.Repositories.IUnitOfWork _unitOfWork;
        public ViajeService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, ViajeDomainService viajeDomainService)
        {
            _unitOfWorkBuilder = unitOfWorkBuilder;
            _mapper = mapper;
            _viajeDomainService = viajeDomainService;
            _unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();
        }
        #region Sucursales
        public async Task<Response<List<SucursalesDto>>> ObtenerSucursales()
        {
            try
            {
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var listado = await unitOfWork.Repository<Sucursales>().AsQueryable()
                 .Where(c => c.Estado)
                 .Include(c => c.Ciudad)
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
        public async Task<Response<List<SucursalesDto>>> ObtenerSucursalesCercanas(double latitud, double longitud)
        {
            try
            {
                List<Sucursales> sucursales = await _unitOfWork.Repository<Sucursales>()
                    .AsQueryable()
                    .Where(c => c.Estado)
                    .Include(c => c.Ciudad)
                    .ToListAsync();

                if (!sucursales.Any())
                {
                    return new Response<List<SucursalesDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.Sin_Registros.Replace("@Entidad", "sucursales")
                    };
                }

                string apiKey = Environment.GetEnvironmentVariable("API_KEY");
                string origins = $"{latitud.ToString(CultureInfo.InvariantCulture)},{longitud.ToString(CultureInfo.InvariantCulture)}";
                string destinations = string.Join("|", sucursales.Select(s =>
                    $"{s.Latitud.ToString(CultureInfo.InvariantCulture)},{s.Longitud.ToString(CultureInfo.InvariantCulture)}"));

                string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origins}&destinations={destinations}&key={apiKey}&units=metric";

                using HttpClient client = new HttpClient();
                string response = await client.GetStringAsync(url);
                RespuestaMatrizDistanciaDto googleResponse = JsonConvert.DeserializeObject<RespuestaMatrizDistanciaDto>(response);

                List<SucursalesDto> sucursalesCercanas = sucursales
                    .Select((sucursal, index) =>
                    {
                        ElementoDto elemento = googleResponse.rows[0].elements[index];
                        return elemento.status == "OK" && elemento.distance != null
                               ? new { Sucursal = sucursal, DistanciaKm = Math.Round(elemento.distance.value / 1000.0, 1) }
                               : null;
                    }).Where(s => s != null && s.DistanciaKm > 0 && s.DistanciaKm <= 50)
                    .Select(s =>
                    {
                        SucursalesDto dto = _mapper.Map<SucursalesDto>(s.Sucursal);
                        dto.DistanciaKm = s.DistanciaKm;
                        return dto;
                    })
                    .ToList();

                return new Response<List<SucursalesDto>>
                {
                    Exitoso = true,
                    Data = sucursalesCercanas
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
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var sucursal = await unitOfWork.Repository<Sucursales>().AsQueryable()
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

        private async Task<Response<int>> GuardarSucursal(SucursalDto sucursalDto)
        {
            var nuevoSucursal = _mapper.Map<Sucursales>(sucursalDto);

            try
            {
                _unitOfWork.Repository<Sucursales>().Add(nuevoSucursal);
                _unitOfWork.SaveChanges();

                return new Response<int> { Exitoso = true, Mensaje = Mensajes.MSJ05.Replace("@Entidad", "Sucursal")};
            }
            catch (Exception ex)
            {
                return new Response<int> { Exitoso = false, Mensaje = ex.ToString()};
            }
        }

        public async Task<Response<int>> RegistrarSucursal(SucursalDto sucursalDto)
        {

            bool descripcionExiste = await _unitOfWork.Repository<Sucursales>().AsQueryable().AnyAsync(c => c.Descripcion == sucursalDto.Descripcion);
            if (descripcionExiste)
            {
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.MSJ14.Replace("@Campo", "Sucursal") };
            }
            var validacion = await _viajeDomainService.ValidarRegistrarDatosSucursal(sucursalDto);
            if (!validacion.Exitoso)
            {
                return validacion;
            }

            return await GuardarSucursal(sucursalDto);
        }
        #endregion

        #region Viajes Encabezado
        public async Task<Response<List<SucursalesDto>>> ObtenerViajes()
        {
            try
            {
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var listado = await unitOfWork.Repository<Sucursales>().AsQueryable()
                 .Where(c => c.Estado)
                 .Include(c => c.Ciudad)
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
        public async Task<Response<SucursalDto>> ObtenerViajePorId(int id)
        {
            try
            {
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var viaje = await unitOfWork.Repository<ViajesEncabezado>().AsQueryable()
                    .Where(c => c.ViajeEncabezadoId == id)
                    .FirstOrDefaultAsync();

                if (viaje == null)
                {
                    return new Response<SucursalDto>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.MSJ10.Replace("@Entidad", "viaje")
                    };
                }

                var viajeDto = _mapper.Map<SucursalDto>(viaje);

                return new Response<SucursalDto>
                {
                    Exitoso = true,
                    Data = viajeDto
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
        #endregion
    }
}
