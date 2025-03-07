using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Acceso;
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
using Academia.SIMOVIA.WebAPI._Features.Viaje.Enums;
using System.Linq.Expressions;
using Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements;
using Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements.Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje
{
    public class ViajeService
    {
        private readonly UnitOfWorkBuilder _unitOfWorkBuilder;
        private readonly IMapper _mapper;
        private readonly ViajeDomainService _viajeDomainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUbicacionService _ubicacionService;
        public ViajeService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, ViajeDomainService viajeDomainService, IUnitOfWork unitOfWork,IUbicacionService ubicacionService)
        {
            _unitOfWorkBuilder = unitOfWorkBuilder;
            _mapper = mapper;
            _viajeDomainService = viajeDomainService;
            _unitOfWork = unitOfWork;
            _ubicacionService = ubicacionService;
        }
        #region Sucursales
        public async Task<Response<List<SucursalesListadoDto>>> ObtenerSucursales()
        {
            try
            {
                var listado = await _unitOfWork.Repository<Sucursales>().AsQueryable()
                 .Where(c => c.Estado)
                 .Include(c => c.Ciudad)
                 .ToListAsync();

                if (!listado.Any())
                {
                    return new Response<List<SucursalesListadoDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "sucursales")
                    };
                }

                var sucursalesDto = _mapper.Map<List<SucursalesListadoDto>>(listado);

                return new Response<List<SucursalesListadoDto>>
                {
                    Exitoso = true,
                    Data = sucursalesDto,
                    Mensaje = Mensajes.LISTADO_EXITOSO.Replace("@Entidad", "Sucursales"),
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<SucursalesListadoDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_LISTA.Replace("@entidad", "sucursales")
                };
            }
            catch (Exception)
            {
                return new Response<List<SucursalesListadoDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }
        public async Task<Response<List<SucursalesDto>>> ObtenerSucursalesCercanas(decimal latitud, decimal longitud)
        {
            
            var validacion = _viajeDomainService.ValidarUbicacion(latitud,longitud);
            if (!validacion.Exitoso)
                return validacion;

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
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "sucursales")
                    };
                }

                DistanceMatrixApiResponseDto googleResponse = await _ubicacionService.ObtenerDistanciasSucursales(latitud, longitud, sucursales);

                if (googleResponse?.rows == null || !googleResponse.rows.Any() || googleResponse.rows[0].elements == null)
                {
                    return new Response<List<SucursalesDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.ERROR_DISTANCIA
                    };
                }

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
                    }).ToList();

                if (!sucursalesCercanas.Any())
                {
                    return new Response<List<SucursalesDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "sucursales")
                    };
                }

                return new Response<List<SucursalesDto>>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_EXITOSO.Replace("@Entidad", "Sucursales"),
                    Data = sucursalesCercanas
                };
            }
            catch (Exception)
            {
                return new Response<List<SucursalesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
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

                if (!listado.Any())
                {
                    return new Response<List<SucursalesDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "sucursales")
                    };
                }

                var sucursalesDto = _mapper.Map<List<SucursalesDto>>(listado);

                return new Response<List<SucursalesDto>>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_EXITOSO.Replace("@Entidad", "Sucursales"),
                    Data = sucursalesDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<SucursalesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_LISTA.Replace("@entidad", "sucursales")
                };
            }
            catch (Exception)
            {
                return new Response<List<SucursalesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }

        public async Task<Response<SucursalesDto>> ObtenerSucursal(int sucursalId)
        {
            try
            {
                var sucursal = await _unitOfWork.Repository<Sucursales>().AsQueryable()
                 .Where(s => s.SucursalId == sucursalId && s.Estado)
                 .Include(c => c.Ciudad)
                 .FirstOrDefaultAsync();

                if (sucursal == null)
                {
                    return new Response<SucursalesDto>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.NO_EXISTE.Replace("@Entidad", "Sucursal")
                    };
                }

                var sucursalDto = _mapper.Map<SucursalesDto>(sucursal);

                return new Response<SucursalesDto>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_INDEPENDIENTE.Replace("@Entidad", "Sucursal"),
                    Data = sucursalDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<SucursalesDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_INDEPENDIENTE.Replace("@entidad", "sucursal")
                };
            }
            catch (Exception)
            {
                return new Response<SucursalesDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }
        private async Task<RegistroSucursalDomainRequirement> CrearRegistroSucursalDomainRequirement(Sucursales sucursal)
        {
            bool descripcionExiste = await DatabaseHelper.ExisteRegistroEnBD<Sucursales>(_unitOfWorkBuilder, c => c.Descripcion == sucursal.Descripcion);

            bool ubicacionExiste = await DatabaseHelper.ExisteRegistroEnBD<Sucursales>(_unitOfWorkBuilder,
                c => c.Latitud == sucursal.Latitud && c.Longitud == sucursal.Longitud);

            bool ciudadExiste = await DatabaseHelper.ExisteRegistroEnBD<Ciudades>(_unitOfWorkBuilder,
                c => c.CiudadId == sucursal.CiudadId);

            bool usuarioExiste = await DatabaseHelper.ExisteRegistroEnBD<Usuarios>(_unitOfWorkBuilder,
                u => u.UsuarioId == sucursal.UsuarioCreacionId);

            return RegistroSucursalDomainRequirement.Fill(descripcionExiste, ubicacionExiste, ciudadExiste, usuarioExiste);
        }

        public async Task<Response<Sucursales>> RegistrarSucursal(SucursalDto sucursalDto)
        {
            Sucursales? sucursalEntidad = _mapper.Map<Sucursales>(sucursalDto);

            var domainRequeriment = await CrearRegistroSucursalDomainRequirement(sucursalEntidad);
            var validacionDominio = _viajeDomainService.ValidarSucursalParaRegistro(sucursalEntidad, domainRequeriment);
            if (!validacionDominio.Exitoso)
                return validacionDominio;
            try
            {
                _unitOfWork.Repository<Sucursales>().Add(sucursalEntidad);
                _unitOfWork.SaveChanges();

                return new Response<Sucursales> { Exitoso = true, Mensaje = Mensajes.CREADO_EXITOSAMENTE.Replace("@Entidad", "Sucursal") };
            }
            catch (Exception ex)
            {
                return new Response<Sucursales>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo", "la").Replace("@entidad", "sucursal")
                };
            }
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

                if (!listado.Any())
                {
                    return new Response<List<ViajesDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "viajes")
                    };
                }

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
                    Mensaje = Mensajes.ERROR_LISTADO.Replace("@entidad", "viajes")
                };
            }
            catch (Exception)
            {
                return new Response<List<ViajesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }

        private async Task<List<int>> ObtenerColaboradoresNoDisponibles(int sucursalId, DateTime fecha, List<int> colaboradoresIds)
        {
            var colaboradoresDisponibles = await _unitOfWork.Repository<ColaboradoresPorSucursal>()
                .AsQueryable()
                .Where(cs => cs.SucursalId == sucursalId)
                .Select(cs => cs.ColaboradorId)
                .ToListAsync();

            var viajesIds = await _unitOfWork.Repository<ViajesEncabezado>()
                .AsQueryable()
                .Where(v => v.SucursalId == sucursalId && v.FechaHora.Date == fecha.Date)
                .Select(v => v.ViajeEncabezadoId)
                .ToListAsync();

            var colaboradoresEnViaje = await _unitOfWork.Repository<ViajesDetalle>()
                .AsQueryable()
                .Where(vd => viajesIds.Contains(vd.ViajeEncabezadoId))
                .Select(vd => vd.ColaboradorId)
                .ToListAsync();

            return colaboradoresIds
                .Where(c => !colaboradoresDisponibles.Contains(c) || colaboradoresEnViaje.Contains(c))
                .ToList();
        }

        private async Task<RegistroViajeDomainRequirement> CrearRegistroViajeDomainRequirement(ViajesEncabezado viaje)
        {
            bool sucursalExiste = await DatabaseHelper.ExisteRegistroEnBD<Sucursales>(_unitOfWorkBuilder, s => s.SucursalId == viaje.SucursalId);
            bool transportistaExiste = await DatabaseHelper.ExisteRegistroEnBD<Transportistas>(_unitOfWorkBuilder, t => t.TransportistaId == viaje.TransportistaId);
            bool usuarioExiste = await DatabaseHelper.ExisteRegistroEnBD<Usuarios>(_unitOfWorkBuilder, u => u.UsuarioId == viaje.UsuarioCreacionId);

            var usuario = await _unitOfWork.Repository<Usuarios>()
                .AsQueryable()
                .Where(u => u.UsuarioId == viaje.UsuarioCreacionId)
                .Select(u => new { u.EsAdministrador })
                .FirstOrDefaultAsync();

            bool usuarioEsGerente = await _unitOfWork.Repository<Colaboradores>()
                .AsQueryable()
                .Where(c => c.ColaboradorId == viaje.UsuarioCreacionId)
                .Select(c => c.CargoId == 1)
                .FirstOrDefaultAsync();

            var colaboradorIds = viaje.ViajesDetalle.Select(c => c.ColaboradorId).Distinct().ToList();

            var colaboradoresExistentes = await _unitOfWork.Repository<Colaboradores>()
                .AsQueryable()
                .Where(c => colaboradorIds.Contains(c.ColaboradorId))
                .Select(c => c.ColaboradorId)
                .ToListAsync();

            var colaboradoresNoExistentes = colaboradorIds.Except(colaboradoresExistentes).ToList();

            var colaboradoresNoDisponibles = await ObtenerColaboradoresNoDisponibles(viaje.SucursalId, viaje.FechaHora, colaboradorIds);

            return RegistroViajeDomainRequirement.Fill(
                sucursalExiste,
                transportistaExiste,
                usuarioExiste,
                usuario?.EsAdministrador ?? false,
                usuarioEsGerente,
                colaboradoresNoExistentes,
                colaboradoresNoDisponibles
            );
        }


        public async Task<Response<ViajesEncabezado>> RegistrarViaje(ViajeDto viajeDto)
        {
            var viajeEntidad = _mapper.Map<ViajesEncabezado>(viajeDto);

            var viajeDetalles = viajeEntidad.ViajesDetalle = _mapper.Map<List<ViajesDetalle>>(viajeDto.Colaboradores)
                ?? new List<ViajesDetalle>();

            var domainRequeriment = await CrearRegistroViajeDomainRequirement(viajeEntidad);
            var validacionDominio = _viajeDomainService.ValidarViajeParaRegistro(viajeEntidad, domainRequeriment);

            if (!validacionDominio.Exitoso)
                return validacionDominio;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                //await Task.Delay(TimeSpan.FromSeconds(10));
                var sucursal = await _unitOfWork.Repository<Sucursales>()
                    .AsQueryable()
                    .Where(s => s.SucursalId == viajeEntidad.SucursalId)
                    .Select(s => new { s.Latitud, s.Longitud })
                    .FirstOrDefaultAsync();

                var colaboradores = await _unitOfWork.Repository<Colaboradores>()
                    .AsQueryable()
                    .Where(c => viajeDetalles.Select(v => v.ColaboradorId).Contains(c.ColaboradorId))
                    .Select(c => new { c.ColaboradorId, c.Latitud, c.Longitud })
                    .ToListAsync();

                if (!colaboradores.Any())
                    return new Response<ViajesEncabezado> { Exitoso = false, Mensaje = Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", "Colaboradores") };

                decimal distanciaTotalKm = 0;
                int totalColaboradores = colaboradores.Count;
                int maximoSolicitudes = 25;

                for (int i = 0; i < totalColaboradores; i += maximoSolicitudes)
                {
                    var cantidadColaboradores = colaboradores.Skip(i).Take(maximoSolicitudes).ToList();
                    decimal distanciaCantidad = await _ubicacionService.CalcularDistanciaViaje(
                        sucursal.Latitud, sucursal.Longitud,
                        cantidadColaboradores.Select(c => (c.Latitud, c.Longitud)).ToList()
                    );

                    var resultadoValidacion = _viajeDomainService.ValidarDistancia(distanciaCantidad, distanciaTotalKm);
                    if (!resultadoValidacion.Exitoso)
                        return resultadoValidacion;

                    distanciaTotalKm += distanciaCantidad;
                }
                var transportistaTarifa = await _unitOfWork.Repository<Transportistas>().AsQueryable().Where(t => t.TransportistaId == viajeEntidad.TransportistaId)
                    .Select(t => t.Tarifa)
                    .FirstOrDefaultAsync();

                decimal tarifa = transportistaTarifa;
                decimal total = distanciaTotalKm * tarifa;

                viajeEntidad.DistanciaTotalKm = distanciaTotalKm;
                viajeEntidad.TarifaTransportista = tarifa;
                viajeEntidad.Total = total;
                _unitOfWork.Repository<ViajesEncabezado>().Add(viajeEntidad);

                if (!await _unitOfWork.SaveChangesAsync())
                {
                    await _unitOfWork.RollBackAsync();
                    return new Response<ViajesEncabezado>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo","el").Replace("@entidad", "viaje")
                    };
                }

                int viajeId = viajeEntidad.ViajeEncabezadoId;

                viajeDetalles.ToList().ForEach(vd =>
                {
                    vd.ViajeEncabezadoId = viajeId;
                    vd.ViajeDetalleId = 0;
                });

                _unitOfWork.Repository<ViajesDetalle>().AddRange(viajeDetalles);
                if (!await _unitOfWork.SaveChangesAsync())
                {
                    await _unitOfWork.RollBackAsync();
                    return new Response<ViajesEncabezado> { Exitoso = false, Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo", "el").Replace("@entidad", "viaje") };
                }

                await _unitOfWork.CommitAsync();

                return new Response<ViajesEncabezado>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.CREADO_EXITOSAMENTE.Replace("@Entidad", "Viaje")
                };
            }
            catch (DbUpdateException)
            {
                await _unitOfWork.RollBackAsync();
                return new Response<ViajesEncabezado>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_BASE_DE_DATOS
                };
            }
            catch (TimeoutException)
            {
                await _unitOfWork.RollBackAsync();
                return new Response<ViajesEncabezado> { Exitoso = false, Mensaje = Mensajes.SERVIDOR_NO_RESPONDE };
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackAsync();
                return new Response<ViajesEncabezado>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo", "el").Replace("@entidad", "viaje")
                };
            }
        }

        public async Task<Response<RutaViajeDto>> ObtenerViaje(int viajeEncabezadoId)
        {
            var viaje = await _unitOfWork.Repository<ViajesEncabezado>()
                .AsQueryable()
                .Where(v => v.ViajeEncabezadoId == viajeEncabezadoId)
                .Select(v => new { v.SucursalId })
                .FirstOrDefaultAsync();

            if (viaje == null)
            {
                return new Response<RutaViajeDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.NO_EXISTE.Replace("@Entidad", "Viaje")
                };
            }

            var sucursal = await _unitOfWork.Repository<Sucursales>()
                .AsQueryable()
                .Where(s => s.SucursalId == viaje.SucursalId)
                .Select(s => new { s.Latitud, s.Longitud })
                .FirstOrDefaultAsync();

            if (sucursal == null)
            {
                return new Response<RutaViajeDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.NO_EXISTE.Replace("@Entidad", "Sucursal")
                };
            }

            var colaboradores = await _unitOfWork.Repository<ViajesDetalle>()
                .AsQueryable()
                .Where(vd => vd.ViajeEncabezadoId == viajeEncabezadoId && vd.Estado)
                .Join(_unitOfWork.Repository<Colaboradores>().AsQueryable(),
                    vd => vd.ColaboradorId,
                    c => c.ColaboradorId,
                    (vd, c) => new
                    {
                        ColaboradorId = c.ColaboradorId,
                        Colaborador = c.Nombres + " " + c.Apellidos,
                        Latitud = c.Latitud,
                        Longitud = c.Longitud,
                        DireccionExacta = c.DireccionExacta
                    })
                .ToListAsync();

            if (!colaboradores.Any())
            {
                return new Response<RutaViajeDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "colaboradores en el viaje")
                };
            }

            var distanciasDict = await _unitOfWork.Repository<ColaboradoresPorSucursal>()
                .AsQueryable()
                .Where(cs => cs.SucursalId == viaje.SucursalId && colaboradores.Select(c => c.ColaboradorId).Contains(cs.ColaboradorId))
                .ToDictionaryAsync(cs => cs.ColaboradorId, cs => cs.DistanciaKm);

            var waypoints = colaboradores.Select(c => new WaypointDto
            {
                Colaborador = c.Colaborador,
                Latitud = c.Latitud,
                Longitud = c.Longitud,
                DistanciaKm = distanciasDict.ContainsKey(c.ColaboradorId) ? distanciasDict[c.ColaboradorId] : null, 
                DireccionExacta = c.DireccionExacta
            }).ToList();

            var ruta = new RutaViajeDto
            {
                LatitudOrigen = sucursal.Latitud,
                LongitudOrigen = sucursal.Longitud,
                Waypoints = waypoints
            };

            return new Response<RutaViajeDto>
            {
                Exitoso = true,
                Mensaje = Mensajes.LISTADO_INDEPENDIENTE.Replace("@Entidad", "Detalle Viaje"),
                Data = ruta
            };
        }


        public async Task<Response<List<ViajesDto>>> ObtenerViajesDisponibles(int? sucursalId, DateTime? fecha)
        {
            try
            {
                if (sucursalId == null)
                {
                    return new Response<List<ViajesDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "viajes")
                    };
                }

                var fechaConsulta = fecha ?? DateTime.Today;

                var viajesDisponibles = await _unitOfWork.Repository<ViajesEncabezado>().AsQueryable()
                    .Where(v => v.SucursalId == sucursalId && v.FechaHora.Date == fechaConsulta.Date)
                    .Include(v => v.Sucursal)
                    .Include(v => v.Transportista)
                    .ToListAsync();

                var viajesFiltrados = viajesDisponibles
                    .Where(v => v.DistanciaTotalKm < 100)
                    .ToList();

                if (!viajesFiltrados.Any())
                {
                    return new Response<List<ViajesDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "viajes")
                    };
                }
                var viajesDto = _mapper.Map<List<ViajesDto>>(viajesFiltrados);

                return new Response<List<ViajesDto>>
                {
                    Exitoso = true,
                    Data = viajesDto
                };
            }
            catch (Exception)
            {
                return new Response<List<ViajesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
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

                if (!listado.Any())
                {
                    return new Response<List<TransportistasDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "transportistas")
                    };
                }

                var transportistasDto = _mapper.Map<List<TransportistasDto>>(listado);

                return new Response<List<TransportistasDto>>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_EXITOSO.Replace("@Entidad", "Transportistas"),
                    Data = transportistasDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<TransportistasDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_LISTADO.Replace("@entidad", "transportistas")
                };
            }
            catch (Exception)
            {
                return new Response<List<TransportistasDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }
        #endregion

        #region Reporte
        public async Task<Response<ViajeReporteResponseDto>> ObtenerReporteViajes(int transportistaId, DateTime fechaInicio, DateTime fechaFin)
        {
            bool transportistaExiste = await _unitOfWork.Repository<Transportistas>().AsQueryable().AnyAsync(t => t.TransportistaId == transportistaId);
            if (!transportistaExiste)
            {
                return new Response<ViajeReporteResponseDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.NO_EXISTE.Replace("@Entidad", "Transportista")
                };
            }
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
                                         && viajeEncabezado.Estado == true
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
                                           ViajeEncabezadoId = viajeEncabezado.ViajeEncabezadoId,
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
                    Mensaje = Mensajes.LISTADO_INDEPENDIENTE.Replace("@Entidad", "Reporte"),
                    Data = new ViajeReporteResponseDto
                    {
                        Encabezados = encabezados,
                        Detalles = detalles,
                        TotalPagar = totalPagar
                    }
                };
            }
            catch (Exception)
            {
                return new Response<ViajeReporteResponseDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_INDEPENDIENTE.Replace("@entidad", "reporte de viajes")
                };
            }
        }


        #endregion

        #region Solicitudes
        public async Task<Response<List<SolicitudesDto>>> ObtenerSolicitudes()
        {
            try
            {
                var listado = await _unitOfWork.Repository<Solicitudes>().AsQueryable()
                 .Include(s => s.Usuario)
                 .Include(v=>v.ViajeEncabezado)
                 .Include(s=> s.Sucursal)
                 .Include(s=> s.EstadoSolicitud)
                 .Where(s=>s.Estado)
                 .ToListAsync();

                if (!listado.Any())
                {
                    return new Response<List<SolicitudesDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "solicitudes")
                    };
                }

                var viajesDto = _mapper.Map<List<SolicitudesDto>>(listado);

                return new Response<List<SolicitudesDto>>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_EXITOSO.Replace("@Entidad", "Solicitudes"),
                    Data = viajesDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<SolicitudesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_LISTADO.Replace("@entidad", "viajes")
                };
            }
            catch (Exception)
            {
                return new Response<List<SolicitudesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }

        public async Task<Response<int>> RegistrarSolicitud(SolicitudDto solicitudDto)
        {

            var nuevaSolicitud = _mapper.Map<Solicitudes>(solicitudDto);

            var horaSolicitud = solicitudDto.FechaViaje.Date.AddHours(solicitudDto.FechaViaje.Hour);

            var viajeExistente = await _unitOfWork.Repository<ViajesEncabezado>()
                .AsQueryable()
                .Where(v => v.FechaHora == horaSolicitud && v.SucursalId == solicitudDto.SucursalId)
                .FirstOrDefaultAsync();

            if (viajeExistente != null)
            {
                nuevaSolicitud.ViajeEncabezadoId = viajeExistente.ViajeEncabezadoId;
                nuevaSolicitud.FechaViaje = viajeExistente.FechaHora;
            }
            else
            {
                nuevaSolicitud.ViajeEncabezadoId = nuevaSolicitud.ViajeEncabezadoId == 0 ? null : nuevaSolicitud.ViajeEncabezadoId;
            }
            nuevaSolicitud.EstadoSolicitudId = (int)EstadosSolicitud.Pendiente;

            nuevaSolicitud.Fecha = DateTime.Now;

            try
            {
                _unitOfWork.Repository<Solicitudes>().Add(nuevaSolicitud);
                if (!await _unitOfWork.SaveChangesAsync())
                {
                    return new Response<int>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo", "la").Replace("@entidad", "solicitud")
                    };
                }
                return new Response<int> { Exitoso = true, Mensaje = Mensajes.CREADO_EXITOSAMENTE.Replace("@Entidad", "Solicitud") };
            }
            catch (Exception)
            {
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo", "la").Replace("@entidad", "solicitud") };
            }
        }
        public async Task<Response<int>> RechazarSolicitud(int solicitudId)
        {
            var solicitud = await _unitOfWork.Repository<Solicitudes>()
                .FirstOrDefaultAsync(s => s.SolicitudId == solicitudId);

            if (solicitud == null)
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.NO_EXISTE.Replace("@Entidad", "Solicitud") };

            solicitud.EstadoSolicitudId = (int)EstadosSolicitud.Rechazado;
            try
            {
                _unitOfWork.Repository<Solicitudes>().Update(solicitud);
                if (!await _unitOfWork.SaveChangesAsync())
                {
                    return new Response<int>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.ERROR_RECHAZAR.Replace("@articulo", "la").Replace("@entidad", "solicitud de cancelación")
                    };
                }

                return new Response<int> { Exitoso = true, Mensaje = Mensajes.RECHAZADO_EXITOSAMENTE.Replace("@Entidad", "Solicitud") };
            }
            catch (Exception ex)
            {
                return new Response<int>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_RECHAZAR.Replace("@articulo", "la").Replace("@entidad", "solicitud")
                };
            }
        }

        public async Task<Response<int>> ProcesarCancelacionSolicitud(ProcesarCancelarSolicitudDto solicitudDto)
        {
            var solicitud = await _unitOfWork.Repository<Solicitudes>()
                .FirstOrDefaultAsync(s => s.SolicitudId == solicitudDto.SolicitudId);

            if (solicitud == null)
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.NO_EXISTE.Replace("@Entidad", "Solicitud")};

            var viaje = await _unitOfWork.Repository<ViajesEncabezado>()
                .FirstOrDefaultAsync(v => v.ViajeEncabezadoId == solicitud.ViajeEncabezadoId);

            if (viaje == null)
                return new Response<int> { Exitoso = false, Mensaje = "El viaje asociado a la solicitud no existe." };

            var colaboradorDetalle = await _unitOfWork.Repository<ViajesDetalle>()
                .FirstOrDefaultAsync(vd => vd.ViajeEncabezadoId == viaje.ViajeEncabezadoId && vd.ColaboradorId == solicitud.UsuarioId);

            if (colaboradorDetalle == null)
                return new Response<int> { Exitoso = false, Mensaje = "El colaborador no está en el viaje." };

            var distanciaColaborador = await _unitOfWork.Repository<ColaboradoresPorSucursal>()
                .AsQueryable()
                .Where(cs => cs.ColaboradorId == solicitud.UsuarioId && cs.SucursalId == viaje.SucursalId)
                .Select(cs => cs.DistanciaKm)
                .FirstOrDefaultAsync();

            if (distanciaColaborador == null)
                return new Response<int> { Exitoso = false, Mensaje = "No se pudo determinar la distancia del colaborador." };

            if (viaje.DistanciaTotalKm - distanciaColaborador <= 0)
                return new Response<int> { Exitoso = false, Mensaje = "No se puede eliminar este colaborador, ya que dejaría el viaje sin distancia." };

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                viaje.DistanciaTotalKm -= distanciaColaborador;
                viaje.Total = viaje.DistanciaTotalKm * viaje.TarifaTransportista;
                _unitOfWork.Repository<ViajesEncabezado>().Update(viaje);

                colaboradorDetalle.Estado = false;
                _unitOfWork.Repository<ViajesDetalle>().Update(colaboradorDetalle);

                solicitud.EstadoSolicitudId = (int)EstadosSolicitud.Aceptado;
                solicitud.FechaAprobado = DateTime.UtcNow;
                solicitud.UsuarioAprobadoId = solicitudDto.UsuarioAprobadoId;
                _unitOfWork.Repository<Solicitudes>().Update(solicitud);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return new Response<int> { Exitoso = true, Mensaje = "La solicitud de cancelación fue aceptada y el colaborador fue eliminado del viaje." };
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackAsync();
                return new Response<int> { Exitoso = false, Mensaje = "Error al procesar la solicitud de cancelación." };
            }
        }

        #endregion
    }
}
