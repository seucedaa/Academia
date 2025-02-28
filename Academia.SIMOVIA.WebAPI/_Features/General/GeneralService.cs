using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
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

        public async Task<Response<int>> RegistrarColaborador(ColaboradorDto colaboradorDto)
        {
            
            var validacion = await _generalDomainService.ValidarRegistrarDatosColaborador(colaboradorDto);
            if (!validacion.Exitoso)
                return validacion;

            var validacionDatosBd = await ValidarDatosColaborador(colaboradorDto);
            if (!validacionDatosBd.Exitoso)
                return validacionDatosBd;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var nuevoColaborador = _mapper.Map<Colaboradores>(colaboradorDto);
                _unitOfWork.Repository<Colaboradores>().Add(nuevoColaborador);

                if (!await _unitOfWork.SaveChangesAsync()) 
                {
                    await _unitOfWork.RollBackAsync();
                    return new Response<int>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo", "el").Replace("@entidad", "colaborador")
                    };
                }

                int colaboradorId = nuevoColaborador.ColaboradorId;

                var colaboradorSucursales = _mapper.Map<List<ColaboradoresPorSucursal>>(colaboradorDto.Sucursales);
                colaboradorSucursales.ForEach(cs => cs.ColaboradorId = colaboradorId);

                _unitOfWork.Repository<ColaboradoresPorSucursal>().AddRange(colaboradorSucursales);
                if (!await _unitOfWork.SaveChangesAsync()) 
                {
                    await _unitOfWork.RollBackAsync();
                    return new Response<int>{ Exitoso = false, Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo","el").Replace("@entidad", "colaborador")};
                }

                await _unitOfWork.CommitAsync();

                return new Response<int>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.CREADO_EXITOSAMENTE.Replace("@Entidad", "Colaborador"),
                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackAsync();
                return new Response<int>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_CREAR.Replace("@articulo","el").Replace("@entidad", "colaborador")
                };
            }
        }

        private async Task<Response<int>> ValidarDatosColaborador(ColaboradorDto colaboradorDto)
        {
            bool dniExiste = await _unitOfWork.Repository<Colaboradores>().AsQueryable().AnyAsync(c => c.DNI == colaboradorDto.DNI);
            bool correoExiste = await _unitOfWork.Repository<Colaboradores>().AsQueryable().AnyAsync(c => c.CorreoElectronico == colaboradorDto.CorreoElectronico);

            var camposDuplicados = new List<string>();

            if (dniExiste) camposDuplicados.Add("DNI");
            if (correoExiste) camposDuplicados.Add("Correo Electrónico");

            if (camposDuplicados.Any())
            {
                string mensaje = camposDuplicados.Count == 1 ? Mensajes.CAMPO_EXISTENTE.Replace("@Campo", camposDuplicados.First())
                    : Mensajes.CAMPOS_EXISTENTES.Replace("@Campos", string.Join(", ", camposDuplicados));

                return new Response<int> { Exitoso = false, Mensaje = mensaje };
            }

            bool estadoCivilExiste = await _unitOfWork.Repository<EstadosCiviles>().AsQueryable().AnyAsync(ec => ec.EstadoCivilId == colaboradorDto.EstadoCivilId);
            bool cargoExiste = await _unitOfWork.Repository<Cargos>().AsQueryable().AnyAsync(c => c.CargoId == colaboradorDto.CargoId);
            bool ciudadExiste = await _unitOfWork.Repository<Ciudades>().AsQueryable().AnyAsync(c => c.CiudadId == colaboradorDto.CiudadId);
            bool usuarioExiste = await _unitOfWork.Repository<Usuarios>().AsQueryable().AnyAsync(u => u.UsuarioId == colaboradorDto.UsuarioCreacionId);

            var camposInvalidos = new List<string>();

            if (!estadoCivilExiste) camposInvalidos.Add("Estado Civil");
            if (!cargoExiste) camposInvalidos.Add("Cargo");
            if (!ciudadExiste) camposInvalidos.Add("Ciudad");
            if (!usuarioExiste) camposInvalidos.Add("Usuario");

            if (camposInvalidos.Any())
            {
                string mensaje = camposInvalidos.Count == 1 ? Mensajes.NO_EXISTE.Replace("@Entidad", camposInvalidos.First())
                    : Mensajes.CAMPOS_NO_EXISTEN.Replace("@Campos", string.Join(", ", camposInvalidos));

                return new Response<int> { Exitoso = false, Mensaje = mensaje };
            }

            var sucursalIds = colaboradorDto.Sucursales.Select(s => s.SucursalId).Distinct().ToList();

            var sucursalesExistentes = await _unitOfWork.Repository<Sucursales>()
                .AsQueryable()
                .Where(s => sucursalIds.Contains(s.SucursalId))
                .Select(s => s.SucursalId)
                .ToListAsync();

            var sucursalesNoExistentes = sucursalIds.Except(sucursalesExistentes).ToList();

            if (sucursalesNoExistentes.Any())
            {
                string mensaje = sucursalesNoExistentes.Count == 1
                    ? Mensajes.NO_EXISTE.Replace("@Entidad", $"Sucursal ID {sucursalesNoExistentes.First()}")
                    : Mensajes.CAMPOS_NO_EXISTEN.Replace("@Campos", $"Sucursales ID {string.Join(", ", sucursalesNoExistentes)}");

                return new Response<int> { Exitoso = false, Mensaje = mensaje };
            }

            return new Response<int> { Exitoso = true };
        }
        #endregion
    }
}
