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

                var cargosDto = _mapper.Map<List<CargosDto>>(listado);

                return new Response<List<CargosDto>>
                {
                    Exitoso = true,
                    Data = cargosDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<CargosDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "cargos")
                };
            }
            catch (Exception)
            {
                return new Response<List<CargosDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
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
                    .Where(c => c.Estado)
                    .ToListAsync();

                var estadosCivilesDto = _mapper.Map<List<EstadosCivilesDto>>(listado);

                return new Response<List<EstadosCivilesDto>>
                {
                    Exitoso = true,
                    Data = estadosCivilesDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<EstadosCivilesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "estados civiles")
                };
            }
            catch (Exception)
            {
                return new Response<List<EstadosCivilesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
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

                var ciudadesDto = _mapper.Map<List<CiudadesDto>>(listado);

                return new Response<List<CiudadesDto>>
                {
                    Exitoso = true,
                    Data = ciudadesDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<CiudadesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "ciudades")
                };
            }
            catch (Exception)
            {
                return new Response<List<CiudadesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
                };
            }
        }

        #endregion

        #region Colaboradores
        public async Task<Response<List<ColaboradoresDto>>> ObtenerColaboradores()
        {
            try
            {
                var listado = await _unitOfWork.Repository<Colaboradores>().AsQueryable()
                 .Where(c => c.Estado)
                 .Include(c => c.EstadoCivil) 
                 .Include(c => c.Cargo)       
                 .Include(c => c.Ciudad)      
                 .ToListAsync();

                var colaboradoresDto = _mapper.Map<List<ColaboradoresDto>>(listado);

                return new Response<List<ColaboradoresDto>>
                {
                    Exitoso = true,
                    Data = colaboradoresDto
                };
            }
            catch (DbUpdateException) 
            {
                return new Response<List<ColaboradoresDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "colaboradores")
                };
            }
            catch (Exception) 
            {
                return new Response<List<ColaboradoresDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
                };
            }
        }

        public async Task<Response<ColaboradorDto>> ObtenerColaboradorPorId(int id)
        {
            try
            {
                var colaborador = await _unitOfWork.Repository<Colaboradores>().AsQueryable()
                    .Where(c => c.ColaboradorId == id)
                    .Include(c => c.EstadoCivil)
                    .Include(c => c.Cargo)
                    .Include(c => c.Ciudad)
                    .FirstOrDefaultAsync();

                if (colaborador == null)
                {
                    return new Response<ColaboradorDto>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.MSJ10.Replace("@Entidad", "colaborador")
                    };
                }

                var colaboradorDto = _mapper.Map<ColaboradorDto>(colaborador);

                return new Response<ColaboradorDto>
                {
                    Exitoso = true,
                    Data = colaboradorDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<ColaboradorDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "colaborador")
                };
            }
            catch (Exception)
            {
                return new Response<ColaboradorDto>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
                };
            }
        }

        public async Task<Response<List<ColaboradoresPorSucursalDto>>> ObtenerColaboradoresDisponibles(int? sucursalId, DateTime? fecha)
        {
            try
            {
                var fechaConsulta = fecha ?? DateTime.Today;

                var colaboradoresEnSucursal = await _unitOfWork.Repository<ColaboradoresPorSucursal>()
                    .AsQueryable()
                    .Where(cs => cs.SucursalId == sucursalId)
                    .Select(cs => new
                    {
                        cs.ColaboradorId,
                        cs.DistanciaKm
                    })
                    .ToListAsync();

                if (!colaboradoresEnSucursal.Any() || sucursalId == null)
                {
                    return new Response<List<ColaboradoresPorSucursalDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.Sin_Registros.Replace("@Entidad", "colaboradores")
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
                    Mensaje = Mensajes.MSJ06
                };
            }
        }



        public async Task<Response<int>> RegistrarColaborador(ColaboradorDto colaboradorDto)
        {
            bool dniExiste = await _unitOfWork.Repository<Colaboradores>().AsQueryable().AnyAsync(c => c.DNI == colaboradorDto.DNI);
            if (dniExiste)
            {
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.MSJ02.Replace("@Campo", "DNI") };
            }

            bool correoExiste = await _unitOfWork.Repository<Colaboradores>().AsQueryable().AnyAsync(c => c.CorreoElectronico == colaboradorDto.CorreoElectronico);
            if (correoExiste)
            {
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.MSJ02.Replace("@Campo", "correo electrónico") };
            }

            var validacion = await _generalDomainService.ValidarRegistrarDatosColaborador(colaboradorDto);
            if (!validacion.Exitoso)
            {
                return validacion;
            }

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
                        Mensaje = Mensajes.MSJ07.Replace("@Entidad", "colaborador")
                    };
                }

                int colaboradorId = nuevoColaborador.ColaboradorId;

                var colaboradorSucursales = _mapper.Map<List<ColaboradoresPorSucursal>>(colaboradorDto.Sucursales);
                colaboradorSucursales.ForEach(cs => cs.ColaboradorId = colaboradorId);


                _unitOfWork.Repository<ColaboradoresPorSucursal>().AddRange(colaboradorSucursales);
                if (!await _unitOfWork.SaveChangesAsync()) 
                {
                    await _unitOfWork.RollBackAsync();
                    return new Response<int>{ Exitoso = false, Mensaje = Mensajes.MSJ07.Replace("@Entidad", "colaborador")};
                }

                _unitOfWork.CommitAsync();

                return new Response<int>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.MSJ05.Replace("@Entidad", "Colaborador"),
                };
            }
            catch (Exception)
            {
                _unitOfWork.RollBackAsync();
                return new Response<int>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ07.Replace("@Entidad", "colaborador")
                };
            }
        }
        public async Task<Response<string>> EditarColaborador(ColaboradorDto colaboradorDto)
        {
            var resultadoValidacion = await ValidarEditarColaborador(colaboradorDto);
            if (!resultadoValidacion.Exitoso)
            {
                return resultadoValidacion;
            }

            return await ActualizarColaborador(colaboradorDto);
        }

        private async Task<Response<string>> ValidarEditarColaborador(ColaboradorDto colaboradorDto)
        {
            var camposObligatorios = new List<(string Campo, bool EsValido)>
            {
                ("Colaborador Id", colaboradorDto.ColaboradorId > 0),
                ("Nombres", !string.IsNullOrEmpty(colaboradorDto.Nombres)),
                ("Apellidos", !string.IsNullOrEmpty(colaboradorDto.Apellidos)),
                ("Correo Electrónico", !string.IsNullOrEmpty(colaboradorDto.CorreoElectronico)),
                ("Teléfono", !string.IsNullOrEmpty(colaboradorDto.Telefono)),
                ("Sexo", !string.IsNullOrEmpty(colaboradorDto.Sexo)),
                ("Fecha de Nacimiento", colaboradorDto.FechaNacimiento != default),
                ("Dirección Exacta", !string.IsNullOrEmpty(colaboradorDto.DireccionExacta)),
                ("Latitud", colaboradorDto.Latitud != 0),
                ("Longitud", colaboradorDto.Longitud != 0),
                ("Estado Civil", colaboradorDto.EstadoCivilId > 0),
                ("Cargo", colaboradorDto.CargoId > 0),
                ("Ciudad", colaboradorDto.CiudadId > 0),
                ("Usuario Modificacion", colaboradorDto.UsuarioGuardaId > 0)
            };
            var campoFaltante = camposObligatorios.FirstOrDefault(c => !c.EsValido);
            if (campoFaltante.Campo != null && !campoFaltante.EsValido)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ09.Replace("@Campo", campoFaltante.Campo) };
            }

            bool dniExiste = await _unitOfWork.Repository<Colaboradores>().AsQueryable().AnyAsync(c => c.DNI == colaboradorDto.DNI && c.ColaboradorId != colaboradorDto.ColaboradorId);
            if (dniExiste)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ02.Replace("@Campo", "DNI") };
            }

            bool correoExiste = await _unitOfWork.Repository<Colaboradores>().AsQueryable().AnyAsync(c => c.CorreoElectronico == colaboradorDto.CorreoElectronico && c.ColaboradorId != colaboradorDto.ColaboradorId);
            if (correoExiste)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ02.Replace("@Campo", "Correo Electrónico") };
            }

            return new Response<string> { Exitoso = true };
        }


        private async Task<Response<string>> ActualizarColaborador(ColaboradorDto colaboradorDto)
        {
            try
            {
                var colaborador = await _unitOfWork.Repository<Colaboradores>().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorDto.ColaboradorId);
                if (colaborador == null)
                {
                    return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ10.Replace("@Entidad", "colaborador") }; 
                }

                _mapper.Map(colaboradorDto, colaborador);

                colaborador.FechaModificacion = DateTime.Now;
                colaborador.UsuarioModificacionId = 1;

                await _unitOfWork.SaveChangesAsync();

                return new Response<string> { Exitoso = true, Mensaje = Mensajes.MSJ05.Replace("@Entidad", "Colaborador") }; 
            }
            catch (Exception)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ07.Replace("@Entidad", "colaborador") }; 
            }
        }

        public async Task<Response<string>> ValidarColaboradorParaEliminar(int colaboradorId)
        {
            var colaborador = await _unitOfWork.Repository<Colaboradores>().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);
            if (colaborador == null)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ10.Replace("@Entidad", "colaborador") };
            }

            bool estaEnUso = await _unitOfWork.Repository<Usuarios>().AsQueryable().AnyAsync(u => u.ColaboradorId == colaboradorId);
            bool estaEnUsoSucursal = await _unitOfWork.Repository<ColaboradoresPorSucursal>().AsQueryable().AnyAsync(u => u.ColaboradorId == colaboradorId);
            if (estaEnUso || estaEnUsoSucursal)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ11.Replace("@Entidad", "colaborador") };
            }

            return new Response<string> { Exitoso = true };
        }

        public async Task<Response<string>> DesactivarColaborador(int colaboradorId)
        {
            var validacion = await ValidarColaboradorParaEliminar(colaboradorId);
            if (!validacion.Exitoso)
            {
                return validacion;
            }

            try
            {
                var colaborador = await _unitOfWork.Repository<Colaboradores>().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);
                colaborador.Estado = false;

                await _unitOfWork.SaveChangesAsync();

                return new Response<string> { Exitoso = true, Mensaje = Mensajes.MSJ12.Replace("@Entidad", "Colaborador desactivado") };
            }
            catch (Exception)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ06 };
            }
        }



        #endregion
    }
}
