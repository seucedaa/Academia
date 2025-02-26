using Academia.SIMOVIA.WebAPI._Features.General;
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
using Microsoft.Data.SqlClient;
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
                var listado = await _unitOfWork.Repository<Sucursales>().AsQueryable()
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

        public async Task<Response<List<SucursalesDto>>> ObtenerSucursalesPorIds(List<int> sucursalesIds)
        {
            try
            {
                var listado = await _unitOfWork.Repository<Sucursales>().AsQueryable()
                    .Where(c => c.Estado && sucursalesIds.Contains(c.SucursalId)) 
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
        public async Task<Response<List<ViajesDto>>> ObtenerViajes()
        {
            try
            {
                var listado = await _unitOfWork.Repository<ViajesEncabezado>().AsQueryable()
                 .Where(c => c.Estado)
                 .Include(v => v.Sucursal)
                 .Include(v => v.Transportista)
                 .ToListAsync();

                var viajesDto = _mapper.Map<List<ViajesDto>>(listado);

                return new Response<List<ViajesDto>>
                {
                    Exitoso = true,
                    Data = viajesDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<ViajesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "viajes")
                };
            }
            catch (Exception)
            {
                return new Response<List<ViajesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
                };
            }
        }

        private async Task<double> CalcularDistanciaGoogle(double latOrigen, double lonOrigen, List<(double Latitud, double Longitud)> colaboradores)
        {
            string apiKey = Environment.GetEnvironmentVariable("API_KEY");
            string origin = $"{latOrigen.ToString(CultureInfo.InvariantCulture)},{lonOrigen.ToString(CultureInfo.InvariantCulture)}";
            string waypoints = string.Join("|", colaboradores.Select(c => $"{c.Latitud.ToString(CultureInfo.InvariantCulture)},{c.Longitud.ToString(CultureInfo.InvariantCulture)}"));
            string destination = $"{colaboradores.Last().Latitud.ToString(CultureInfo.InvariantCulture)},{colaboradores.Last().Longitud.ToString(CultureInfo.InvariantCulture)}";
            string url = $"https://maps.googleapis.com/maps/api/directions/json?origin={origin}&destination={destination}&waypoints=optimize:true|{waypoints}&key={apiKey}&units=metric";

            using HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(url);
            RutaGoogleDto googleResponse = JsonConvert.DeserializeObject<RutaGoogleDto>(response);

            if (googleResponse.routes == null || !googleResponse.routes.Any())
            {
                return -1; 
            }

            double distanciaKm = googleResponse.routes[0].legs.Sum(l => l.distance.value) / 1000.0;
            return distanciaKm;
        }


        public async Task<Response<int>> RegistrarViaje(ViajeDto viajeDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var sucursal = await _unitOfWork.Repository<Sucursales>()
                    .AsQueryable()
                    .Where(s => s.SucursalId == viajeDto.SucursalId)
                    .Select(s => new { s.Latitud, s.Longitud })
                    .FirstOrDefaultAsync();

                if (sucursal == null)
                {
                    return new Response<int> { Exitoso = false, Mensaje = "Sucursal no encontrada." };
                }

                var colaboradores = await _unitOfWork.Repository<Colaboradores>()
                    .AsQueryable()
                    .Where(c => viajeDto.Colaboradores.Select(v => v.ColaboradorId).Contains(c.ColaboradorId))
                    .Select(c => new { c.ColaboradorId, c.Latitud, c.Longitud })
                    .ToListAsync();

                if (!colaboradores.Any())
                {
                    return new Response<int> { Exitoso = false, Mensaje = "No hay colaboradores en este viaje." };
                }

                double distanciaTotalKm = 0;
                int totalColaboradores = colaboradores.Count;
                int maximoSolicitudes = 25;

                for (int i = 0; i < totalColaboradores; i += maximoSolicitudes)
                {
                    var cantidadColaboradores = colaboradores.Skip(i).Take(maximoSolicitudes).ToList();
                    double distanciaCantidad = await CalcularDistanciaGoogle(
                        (double)sucursal.Latitud, (double)sucursal.Longitud,
                        cantidadColaboradores.Select(c => ((double)c.Latitud, (double)c.Longitud)).ToList()
                    );

                    if (distanciaCantidad < 0)
                    {
                        return new Response<int> { Exitoso = false, Mensaje = "Error al calcular la distancia del viaje." };
                    }

                    distanciaTotalKm += distanciaCantidad;

                    if (distanciaTotalKm > 100)
                    {
                        return new Response<int>
                        {
                            Exitoso = false,
                            Mensaje = $"La distancia total del viaje ({distanciaTotalKm} km) supera el límite de 100 km."
                        };
                    }
                }

                decimal tarifa = viajeDto.TarifaTransportista;
                decimal total = (decimal)distanciaTotalKm * tarifa;

                var nuevoViaje = _mapper.Map<ViajesEncabezado>(viajeDto);
                nuevoViaje.DistanciaTotalKm = (decimal)distanciaTotalKm;
                nuevoViaje.Total = total;
                _unitOfWork.Repository<ViajesEncabezado>().Add(nuevoViaje);

                if (!await _unitOfWork.SaveChangesAsync())
                {
                    await _unitOfWork.RollBackAsync();
                    return new Response<int>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.MSJ07.Replace("@Entidad", "viaje")
                    };
                }

                int viajeId = nuevoViaje.ViajeEncabezadoId;

                var viajeDetalles = _mapper.Map<List<ViajesDetalle>>(viajeDto.Colaboradores);
                viajeDetalles.ForEach(cs => cs.ViajeEncabezadoId = viajeId);

                _unitOfWork.Repository<ViajesDetalle>().AddRange(viajeDetalles);
                if (!await _unitOfWork.SaveChangesAsync())
                {
                    await _unitOfWork.RollBackAsync();
                    return new Response<int> { Exitoso = false, Mensaje = Mensajes.MSJ07.Replace("@Entidad", "viaje") };
                }

                await _unitOfWork.CommitAsync();

                return new Response<int>
                {
                    Exitoso = true,
                    Mensaje = $"Viaje registrado con éxito. Distancia total: {distanciaTotalKm} km.",
                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackAsync();
                return new Response<int>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ07.Replace("@Entidad", "viaje")
                };
            }
        }


        #endregion

        #region Transportistas
        public async Task<Response<List<TransportistasDto>>> ObtenerTransportistas()
        {
            try
            {
                var listado = await _unitOfWork.Repository<Transportistas>().AsQueryable()
                 .Where(c => c.Estado)
                 .Include(c => c.Ciudad)
                 .ToListAsync();

                var transportistasDto = _mapper.Map<List<TransportistasDto>>(listado);

                return new Response<List<TransportistasDto>>
                {
                    Exitoso = true,
                    Data = transportistasDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<TransportistasDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "transportistas")
                };
            }
            catch (Exception)
            {
                return new Response<List<TransportistasDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
                };
            }
        }
        #endregion

        #region Reporte
        public async Task<Response<ViajeReporteResponseDto>> ObtenerReporteViajes(int transportistaId, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var query = await (from viajeEncabezado in _unitOfWork.Repository<ViajesEncabezado>().AsQueryable()
                                   join usuario in _unitOfWork.Repository<Usuarios>().AsQueryable()
                                   on viajeEncabezado.UsuarioCreacionId equals usuario.UsuarioId
                                   join viajeDetalle in _unitOfWork.Repository<ViajesDetalle>().AsQueryable()
                                   on viajeEncabezado.ViajeEncabezadoId equals viajeDetalle.ViajeEncabezadoId
                                   join colaborador in _unitOfWork.Repository<Colaboradores>().AsQueryable()
                                   on viajeDetalle.ColaboradorId equals colaborador.ColaboradorId
                                   join sucursal in _unitOfWork.Repository<Sucursales>().AsQueryable()
                                   on viajeEncabezado.SucursalId equals sucursal.SucursalId
                                   join transportista in _unitOfWork.Repository<Transportistas>().AsQueryable()
                                   on viajeEncabezado.TransportistaId equals transportista.TransportistaId
                                   join ciudad in _unitOfWork.Repository<Ciudades>().AsQueryable()
                                   on transportista.CiudadId equals ciudad.CiudadId
                                   join estado in _unitOfWork.Repository<Estados>().AsQueryable()
                                   on ciudad.EstadoId equals estado.EstadoId
                                   join pais in _unitOfWork.Repository<Paises>().AsQueryable()
                                   on estado.PaisId equals pais.PaisId
                                   join monedaPorPais in _unitOfWork.Repository<MonedasPorPais>().AsQueryable()
                                   on pais.PaisId equals monedaPorPais.PaisId
                                   join moneda in _unitOfWork.Repository<Monedas>().AsQueryable()
                                   on monedaPorPais.MonedaId equals moneda.MonedaId
                                   where viajeEncabezado.FechaHora.Date >= fechaInicio.Date
                                         && viajeEncabezado.FechaHora.Date <= fechaFin.Date
                                         && viajeEncabezado.TransportistaId == transportistaId
                                         && monedaPorPais.Principal == true  
                                   orderby viajeEncabezado.FechaHora descending
                                   select new
                                   {
                                       ViajeEncabezado = new ViajeReporteEncabezadoDto
                                       {
                                           ViajeEncabezadoId = viajeEncabezado.ViajeEncabezadoId,
                                           FechaHora = viajeEncabezado.FechaHora.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                                           DistanciaTotalKm = viajeEncabezado.DistanciaTotalKm,
                                           TarifaTransportista = moneda.Simbolo + " " + viajeEncabezado.TarifaTransportista.ToString("N", CultureInfo.InvariantCulture),
                                           Total = moneda.Simbolo + " " + viajeEncabezado.Total.ToString("N", CultureInfo.InvariantCulture),
                                           Sucursal = sucursal.Descripcion,
                                           Transportista = transportista.DNI + " - " + transportista.Nombres + " " + transportista.Apellidos,
                                           Pais = pais.Descripcion,
                                           Moneda = moneda.Nombre,
                                           MonedaSimbolo = moneda.Simbolo,
                                           UsuarioCreacion = usuario.Usuario,
                                           FechaCreacion = viajeEncabezado.FechaCreacion.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                                       },
                                       ViajeDetalle = new ViajeReporteDetalleDto
                                       {
                                           ColaboradorId = colaborador.ColaboradorId,
                                           Colaborador = colaborador.DNI + " - " + colaborador.Nombres + " " + colaborador.Apellidos,
                                           DireccionExacta = colaborador.DireccionExacta,
                                           DistanciaKm = _unitOfWork.Repository<ColaboradoresPorSucursal>()
                                              .AsQueryable()
                                              .Where(cosu => cosu.ColaboradorId == colaborador.ColaboradorId && cosu.SucursalId == viajeEncabezado.SucursalId)
                                              .Select(cosu => cosu.DistanciaKm)
                                              .FirstOrDefault()
                                       }
                                   }).ToListAsync();

                var encabezados = query.Select(ve => ve.ViajeEncabezado).DistinctBy(v => v.ViajeEncabezadoId).ToList();

                var detalles = query.Select(q => q.ViajeDetalle).ToList();

                var totalPagar = new TotalPagarDto
                {
                    TotalPagar = encabezados.FirstOrDefault()?.MonedaSimbolo + " " +
                                 encabezados.Sum(v => decimal.Parse(v.Total.Replace(encabezados.FirstOrDefault()?.MonedaSimbolo + " ", ""), CultureInfo.InvariantCulture))
                                 .ToString("N", CultureInfo.InvariantCulture)
                };

                return new Response<ViajeReporteResponseDto>
                {
                    Exitoso = true,
                    Data = new ViajeReporteResponseDto
                    {
                        Encabezados = encabezados,
                        Detalles = detalles,
                        TotalPagar = totalPagar
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<ViajeReporteResponseDto>
                {
                    Exitoso = false,
                    Mensaje = $"Error al obtener el reporte de viajes: {ex.Message}"
                };
            }
        }


        #endregion
    }
}
