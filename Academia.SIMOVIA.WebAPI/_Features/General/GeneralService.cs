using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI._Features.General.DomainRequirements;
using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using AutoMapper;
using Farsiman.Infraestructure.Core.Entity.Standard;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace Academia.SIMOVIA.WebAPI._Features.General
{
    public class GeneralService
    {
        private readonly UnitOfWorkBuilder _unitOfWorkBuilder;
        private readonly Farsiman.Domain.Core.Standard.Repositories.IUnitOfWork _unitOfWork;
        private readonly GeneralDomainService _generalDomainService;
        private readonly IMapper _mapper;
        public GeneralService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, GeneralDomainService generalDomainService)
        {
            _unitOfWorkBuilder = unitOfWorkBuilder;
            _mapper = mapper;
            _generalDomainService = generalDomainService;
            _unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();
        }
        #region Cargos
        public async Task<Response<List<CargosDto>>> ObtenerCargos()
        {
            try
            {

                var listado = await _unitOfWork.Repository<Cargos>().AsQueryable()
                    .Where(c => c.Estado)
                    .ToListAsync();

                if (!listado.Any())
                {
                    return new Response<List<CargosDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "cargos")
                    };
                }

                var cargosDto = _mapper.Map<List<CargosDto>>(listado);

                return new Response<List<CargosDto>>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_EXITOSO.Replace("@Entidad", "Cargos"),
                    Data = cargosDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<CargosDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_LISTADO.Replace("@entidad", "cargos")
                };
            }
            catch (Exception)
            {
                return new Response<List<CargosDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }

        #endregion

        #region Estados Civiles
        public async Task<Response<List<EstadosCivilesDto>>> ObtenerEstadosCiviles()
        {
            try
            {
                var listado = await _unitOfWork.Repository<EstadosCiviles>().AsQueryable()
                    .Where(c => c.Estado).ToListAsync();

                if (!listado.Any())
                {
                    return new Response<List<EstadosCivilesDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "estados civiles")
                    };
                }

                var estadosCivilesDto = _mapper.Map<List<EstadosCivilesDto>>(listado);

                return new Response<List<EstadosCivilesDto>>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_EXITOSO.Replace("@Entidad", "Estados Civiles"),
                    Data = estadosCivilesDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<EstadosCivilesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_LISTADO.Replace("@entidad", "estados civiles")
                };
            }
            catch (Exception)
            {
                return new Response<List<EstadosCivilesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }

        #endregion

        #region Ciudades
        public async Task<Response<List<CiudadesDto>>> ObtenerCiudades()
        {
            try
            {
                var listado = await _unitOfWork.Repository<Ciudades>().AsQueryable()
                    .ToListAsync();

                if (!listado.Any())
                {
                    return new Response<List<CiudadesDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "ciudades")
                    };
                }

                var ciudadesDto = _mapper.Map<List<CiudadesDto>>(listado);

                return new Response<List<CiudadesDto>>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_EXITOSO.Replace("@Entidad", "Ciudades"),
                    Data = ciudadesDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<CiudadesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_LISTADO.Replace("@entidad", "ciudades")
                };
            }
            catch (Exception)
            {
                return new Response<List<CiudadesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }

        #endregion

        #region Colaboradores
        public async Task<Response<List<ColaboradoresDto>>> ObtenerColaboradores()
        {
            try
            {
                List<Colaboradores> listado = await _unitOfWork.Repository<Colaboradores>().AsQueryable()
                 .Where(c => c.Estado)
                 .Include(c => c.EstadoCivil) 
                 .Include(c => c.Cargo)       
                 .Include(c => c.Ciudad)      
                 .ToListAsync();

                if (!listado.Any())
                {
                    return new Response<List<ColaboradoresDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "colaboradores")
                    };
                }

                var colaboradoresDto = _mapper.Map<List<ColaboradoresDto>>(listado);

                return new Response<List<ColaboradoresDto>>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_EXITOSO.Replace("@Entidad", "Colaboradores"),
                    Data = colaboradoresDto
                };
            }
            catch (DbUpdateException) 
            {
                return new Response<List<ColaboradoresDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_LISTADO.Replace("@entidad", "colaboradores")
                };
            }
            catch (Exception) 
            {
                return new Response<List<ColaboradoresDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }

        public async Task<Response<ColaboradoresDto>> ObtenerColaborador(int colaboradorId)
        {
            try
            {
                var colaborador = await _unitOfWork.Repository<Colaboradores>().AsQueryable()
                    .Where(c => c.ColaboradorId == colaboradorId && c.Estado)
                    .Include(c => c.EstadoCivil)
                    .Include(c => c.Cargo)
                    .Include(c => c.Ciudad)
                    .FirstOrDefaultAsync();

                if (colaborador == null)
                {
                    return new Response<ColaboradoresDto>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.NO_EXISTE.Replace("@Entidad", "Colaborador")
                    };
                }

                var colaboradorDto = _mapper.Map<ColaboradoresDto>(colaborador);

                return new Response<ColaboradoresDto>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_INDEPENDIENTE.Replace("@Entidad", "Colaborador"),
                    Data = colaboradorDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<ColaboradoresDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_INDEPENDIENTE.Replace("@entidad", "colaborador")
                };
            }
            catch (Exception)
            {
                return new Response<ColaboradoresDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }

        public async Task<Response<List<ColaboradoresPorSucursalDto>>> ObtenerColaboradoresDisponibles(int sucursalId, DateTime? fecha)
        {
            try
            {

                var fechaConsulta = fecha ?? DateTime.Today;

                bool sucursalExiste = await _unitOfWork.Repository<Sucursales>().AsQueryable().AnyAsync(s => s.SucursalId == sucursalId);

                if (!sucursalExiste)
                    return new Response<List<ColaboradoresPorSucursalDto>>{ Exitoso = false, Mensaje = Mensajes.NO_EXISTE.Replace("@Entidad", "Sucursal")};

                var colaboradoresEnSucursal = await _unitOfWork.Repository<ColaboradoresPorSucursal>().AsQueryable()
                    .Where(cs => cs.SucursalId == sucursalId)
                    .Select(cs => new
                    { cs.ColaboradorId, cs.DistanciaKm }).ToListAsync();

                if (!colaboradoresEnSucursal.Any() || sucursalId == null)
                {
                    return new Response<List<ColaboradoresPorSucursalDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "colaboradores")
                    };
                }

                var colaboradoresEnSucursalIds = colaboradoresEnSucursal.Select(cs => cs.ColaboradorId).ToList();

                var viajesIds = await _unitOfWork.Repository<ViajesEncabezado>()
                    .AsQueryable()
                    .Where(v => v.SucursalId == sucursalId && v.FechaHora.Date == fechaConsulta.Date)
                    .Select(v => v.ViajeEncabezadoId)
                    .ToListAsync();

                var colaboradoresEnViaje = await _unitOfWork.Repository<ViajesDetalle>()
                    .AsQueryable()
                    .Where(vd => viajesIds.Contains(vd.ViajeEncabezadoId))
                    .Select(vd => vd.ColaboradorId)
                    .ToListAsync();

                var colaboradoresDisponibles = await _unitOfWork.Repository<Colaboradores>()
                    .AsQueryable()
                    .Where(c => colaboradoresEnSucursalIds.Contains(c.ColaboradorId) && !colaboradoresEnViaje.Contains(c.ColaboradorId))
                    .ToListAsync();

                var distanciasDict = colaboradoresEnSucursal.ToDictionary(cs => cs.ColaboradorId, cs => cs.DistanciaKm);

                var colaboradoresDto = _mapper.Map<List<ColaboradoresPorSucursalDto>>(colaboradoresDisponibles, opt => opt.Items["Distancias"] = distanciasDict);
                if (!colaboradoresDto.Any())
                {
                    return new Response<List<ColaboradoresPorSucursalDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "colaboradores")
                    };
                }
                return new Response<List<ColaboradoresPorSucursalDto>>
                {
                    Exitoso = true,
                    Data = colaboradoresDto
                };
            }
            catch (Exception)
            {
                return new Response<List<ColaboradoresPorSucursalDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }

        public async Task<Response<List<ColaboradoresDto>>> ObtenerColaboradoresPorSucursales(List<int> sucursalesIds)
        {
            try
            {
                var listado = await _unitOfWork.Repository<Colaboradores>().AsQueryable()
                    .Where(c => c.Estado && c.ColaboradoresPorSucursal.Any(cs => sucursalesIds.Contains(cs.SucursalId)))
                    .Include(c => c.EstadoCivil)
                    .Include(c => c.Cargo)
                    .Include(c => c.Ciudad)
                    .ToListAsync();


                if (!listado.Any())
                {
                    return new Response<List<ColaboradoresDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "colaboradores")
                    };
                }

                var colaboradoresDto = _mapper.Map<List<ColaboradoresDto>>(listado);

                return new Response<List<ColaboradoresDto>>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.LISTADO_EXITOSO.Replace("@Entidad", "Colaboradores"),
                    Data = colaboradoresDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<ColaboradoresDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_LISTA.Replace("@entidad", "colaboradores")
                };
            }
            catch (Exception)
            {
                return new Response<List<ColaboradoresDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }
        private async Task<RegistroColaboradorDomainRequirement> CrearRegistroColaboradorDomainRequirement(Colaboradores colaborador)
        {
            bool dniExiste = await DatabaseHelper.ExisteRegistroEnBD<Colaboradores>(_unitOfWorkBuilder, c => c.DNI == colaborador.DNI);
            bool correoExiste = await DatabaseHelper.ExisteRegistroEnBD<Colaboradores>(_unitOfWorkBuilder, c => c.CorreoElectronico == colaborador.CorreoElectronico);
            bool estadoCivilExiste = await DatabaseHelper.ExisteRegistroEnBD<EstadosCiviles>(_unitOfWorkBuilder, e => e.EstadoCivilId == colaborador.EstadoCivilId);
            bool cargoExiste = await DatabaseHelper.ExisteRegistroEnBD<Cargos>(_unitOfWorkBuilder, c => c.CargoId == colaborador.CargoId);
            bool ciudadExiste = await DatabaseHelper.ExisteRegistroEnBD<Ciudades>(_unitOfWorkBuilder, c => c.CiudadId == colaborador.CiudadId);
            bool usuarioExiste = await DatabaseHelper.ExisteRegistroEnBD<Usuarios>(_unitOfWorkBuilder, u=> u.UsuarioId == colaborador.UsuarioCreacionId);

            var sucursalIds = colaborador.ColaboradoresPorSucursal.Select(s => s.SucursalId).Distinct().ToList();
            var sucursalesExistentes = await _unitOfWork.Repository<Sucursales>()
                .AsQueryable()
                .Where(s => sucursalIds.Contains(s.SucursalId))
                .Select(s => s.SucursalId)
                .ToListAsync();

            var sucursalesNoExistentes = sucursalIds.Except(sucursalesExistentes).ToList();

            return RegistroColaboradorDomainRequirement.Fill(dniExiste, correoExiste, estadoCivilExiste, cargoExiste, ciudadExiste, usuarioExiste, sucursalesNoExistentes);
        }


        public async Task<Response<Colaboradores>> RegistrarColaborador(ColaboradorDto colaboradorDto)
        {
            var colaboradorEntidad = _mapper.Map<Colaboradores>(colaboradorDto);

            var sucursalesAsignadas = colaboradorEntidad.ColaboradoresPorSucursal = _mapper.Map<List<ColaboradoresPorSucursal>>(colaboradorDto.Sucursales)
                 ?? new List<ColaboradoresPorSucursal>();

            var domainRequeriment = await CrearRegistroColaboradorDomainRequirement(colaboradorEntidad);
            var validacionDominio = _generalDomainService.ValidarColaboradorParaRegistro(colaboradorEntidad, domainRequeriment);

            if (!validacionDominio.Exitoso)
                return validacionDominio;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _unitOfWork.Repository<Colaboradores>().Add(colaboradorEntidad);

                if (!await _unitOfWork.SaveChangesAsync()) 
                {
                    await _unitOfWork.RollBackAsync();
                    return new Response<Colaboradores>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo", "el").Replace("@entidad", "colaborador")
                    };
                }

                int colaboradorId = colaboradorEntidad.ColaboradorId;

                sucursalesAsignadas.ToList().ForEach(cs =>
                {
                    cs.ColaboradorId = colaboradorId;
                    cs.ColaboradorPorSucursalId = 0;
                });

                _unitOfWork.Repository<ColaboradoresPorSucursal>().AddRange(sucursalesAsignadas);
                if (!await _unitOfWork.SaveChangesAsync()) 
                {
                    await _unitOfWork.RollBackAsync();
                    return new Response<Colaboradores>{ Exitoso = false, Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo","el").Replace("@entidad", "colaborador")};
                }

                await _unitOfWork.CommitAsync();

                return new Response<Colaboradores>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.CREADO_EXITOSAMENTE.Replace("@Entidad", "Colaborador"),
                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackAsync();
                return new Response<Colaboradores>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo","el").Replace("@entidad", "colaborador")
                };
            }
        }

        #endregion
    }
}
