using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academia.SIMOVIA.WebAPI._Features.General
{
    public class GeneralService
    {
        private readonly UnitOfWorkBuilder _unitOfWorkBuilder;
        private readonly GeneralDomainService _generalDomainService;
        private readonly IMapper _mapper;
        public GeneralService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, GeneralDomainService generalDomainService)
        {
            _unitOfWorkBuilder = unitOfWorkBuilder;
            _mapper = mapper;
            _generalDomainService = generalDomainService;
        }
        #region Cargos
        public async Task<Response<List<CargosDto>>> ObtenerCargos()
        {
            try
            {
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var listado = await unitOfWork.Repository<Cargos>().AsQueryable()
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
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var listado = await unitOfWork.Repository<EstadosCiviles>().AsQueryable()
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
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var listado = await unitOfWork.Repository<Ciudades>().AsQueryable()
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
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var listado = await unitOfWork.Repository<Colaboradores>().AsQueryable()
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
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var colaborador = await unitOfWork.Repository<Colaboradores>().AsQueryable()
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

        

        private async Task<Response<int>> GuardarColaborador(ColaboradorDto colaboradorDto)
        {
            var nuevoColaborador = _mapper.Map<Colaboradores>(colaboradorDto);

            try
            {
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                unitOfWork.Repository<Colaboradores>().Add(nuevoColaborador);
                await unitOfWork.SaveChangesAsync();

                return new Response<int> { Exitoso = true, Mensaje = Mensajes.MSJ05.Replace("@Entidad", "Colaborador"), Data = nuevoColaborador.ColaboradorId };
            }
            catch (Exception)
            {
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.MSJ07.Replace("@Entidad", "colaborador") };
            }
        }

        public async Task<Response<int>> RegistrarColaborador(ColaboradorDto colaboradorDto)
        {
            var validacion = await _generalDomainService.ValidarRegistrarDatosColaborador(colaboradorDto);
            if (!validacion.Exitoso)
            {
                return validacion;
            }

            return await GuardarColaborador(colaboradorDto);
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
            await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

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

            bool dniExiste = await unitOfWork.Repository<Colaboradores>().AsQueryable().AnyAsync(c => c.DNI == colaboradorDto.DNI && c.ColaboradorId != colaboradorDto.ColaboradorId);
            if (dniExiste)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ02.Replace("@Campo", "DNI") };
            }

            bool correoExiste = await unitOfWork.Repository<Colaboradores>().AsQueryable().AnyAsync(c => c.CorreoElectronico == colaboradorDto.CorreoElectronico && c.ColaboradorId != colaboradorDto.ColaboradorId);
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
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var colaborador = await unitOfWork.Repository<Colaboradores>().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorDto.ColaboradorId);
                if (colaborador == null)
                {
                    return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ10.Replace("@Entidad", "colaborador") }; 
                }

                _mapper.Map(colaboradorDto, colaborador);

                colaborador.FechaModificacion = DateTime.Now;
                colaborador.UsuarioModificacionId = 1;

                await unitOfWork.SaveChangesAsync();

                return new Response<string> { Exitoso = true, Mensaje = Mensajes.MSJ05.Replace("@Entidad", "Colaborador") }; 
            }
            catch (Exception)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ07.Replace("@Entidad", "colaborador") }; 
            }
        }

        public async Task<Response<string>> ValidarColaboradorParaEliminar(int colaboradorId)
        {
            await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

            var colaborador = await unitOfWork.Repository<Colaboradores>().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);
            if (colaborador == null)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ10.Replace("@Entidad", "colaborador") };
            }

            bool estaEnUso = await unitOfWork.Repository<Usuarios>().AsQueryable().AnyAsync(u => u.ColaboradorId == colaboradorId);
            bool estaEnUsoSucursal = await unitOfWork.Repository<ColaboradoresPorSucursal>().AsQueryable().AnyAsync(u => u.ColaboradorId == colaboradorId);
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
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var colaborador = await unitOfWork.Repository<Colaboradores>().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);
                colaborador.Estado = false;

                await unitOfWork.SaveChangesAsync();

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
