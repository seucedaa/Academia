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
                var sucursales = await _unitOfWork.Repository<Sucursales>().AsQueryable()
                    .Where(c => c.Estado)
                    .Include(c => c.Ciudad)
                    .ToListAsync();

                if (!sucursales.Any())
                {
                    return new Response<List<SucursalesDto>> { Exitoso = false, Mensaje = Mensajes.Sin_Registros.Replace("@Entidad", "sucursales") };
                }

                string apiKey = "AIzaSyCXBavP9hXM9SGr4m6GsQdC98rmUqBQUU0"; 
                string origins = $"{latitud},{longitud}";
                string destinations = string.Join("|", sucursales.Select(s => $"{s.Latitud},{s.Longitud}"));

                string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origins}&destinations={destinations}&key={apiKey}&units=metric";

                using (HttpClient client = new HttpClient())
                {
                    var response = await client.GetStringAsync(url);
                    var googleResponse = JsonConvert.DeserializeObject<RespuestaMatrizDistanciaDto>(response);

                    if (googleResponse.Estado != "OK")
                    {
                        return new Response<List<SucursalesDto>> { Exitoso = false, Mensaje = "Error al consultar Google Maps API." };
                    }

                    var sucursalesCercanas = new List<SucursalesDto>();
                    for (int i = 0; i < sucursales.Count; i++)
                    {
                        var distancia = googleResponse.Filas[0].Elementos[i].Distancia.Valor / 1000.0;

                        if (distancia > 0 && distancia <= 50)
                        {
                            sucursalesCercanas.Add(_mapper.Map<SucursalesDto>(sucursales[i]));
                        }
                    }

                    return new Response<List<SucursalesDto>>
                    {
                        Exitoso = true,
                        Data = sucursalesCercanas
                    };
                }
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
