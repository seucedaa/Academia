using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Academia.SIMOVIA.WebAPI._Features.Acceso
{
    public class AccesoService
    {
        private readonly IMapper _mapper;
        private readonly AccesoDomainService _accesoDomainService;
        private readonly UnitOfWorkBuilder _unitOfWorkBuilder;
        public AccesoService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, AccesoDomainService accesoDomainService)
        {
            _mapper = mapper;
            _accesoDomainService = accesoDomainService;
            _unitOfWorkBuilder = unitOfWorkBuilder;
        }
        #region Roles
        public async Task<Response<List<RolesDto>>> ObtenerRoles()
        {
            try
            {
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                var listado = await unitOfWork.Repository<Roles>().AsQueryable()
                    .Where(r => r.Estado) 
                    .ToListAsync();

                var rolesDto = _mapper.Map<List<RolesDto>>(listado);

                return new Response<List<RolesDto>>
                {
                    Exitoso = true,
                    Data = rolesDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<RolesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "roles")
                };
            }
            catch (Exception)
            {
                return new Response<List<RolesDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
                };
            }
        }

        #endregion

        #region Usuarios
        public async Task<Response<List<UsuariosDto>>> ObtenerUsuarios()
        {
            try
            {
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();
                var listado = await unitOfWork.Repository<Usuarios>().AsQueryable()
                    .Where(u => u.Estado) 
                    .ToListAsync();

                var usuariosDto = _mapper.Map<List<UsuariosDto>>(listado);

                return new Response<List<UsuariosDto>>
                {
                    Exitoso = true,
                    Data = usuariosDto
                };
            }
            catch (DbUpdateException)
            {
                return new Response<List<UsuariosDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ13.Replace("@entidad", "usuarios")
                };
            }
            catch (Exception)
            {
                return new Response<List<UsuariosDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.MSJ06
                };
            }
        }

        private async Task<SesionUsuarioDto> ObtenerDatosSesionUsuario(string usuarioNombre)
        {
            await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

            var usuario = await unitOfWork.Repository<Usuarios>().AsQueryable()
                .Include(u => u.Rol)
                .Include(u => u.Colaborador)
                .ThenInclude(c => c.Cargo)
                .FirstOrDefaultAsync(u => u.Usuario == usuarioNombre);

            var pantallas = await ObtenerPantallasPermitidas(usuario);

            var sesionUsuarioDto = _mapper.Map<SesionUsuarioDto>(usuario);
            sesionUsuarioDto.Pantallas = pantallas;

            return sesionUsuarioDto;
        }

        public async Task<Response<SesionUsuarioDto>> InicioSesion(InicioSesionDto login)
        {
            var resultadoValidacion = await _accesoDomainService.ValidarInicioSesion(login);
            if (!resultadoValidacion.Exitoso)
            {
                return new Response<SesionUsuarioDto> { Exitoso = false, Mensaje = resultadoValidacion.Mensaje };
            }

            var usuarioSesion = await ObtenerDatosSesionUsuario(login.Usuario);

            return new Response<SesionUsuarioDto>
            {
                Exitoso = true,
                Mensaje = "Inicio de sesión exitoso",
                Data = usuarioSesion
            };
        }


        private async Task<List<PantallaDto>> ObtenerPantallasPermitidas(Usuarios usuario)
        {
            await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

            return await unitOfWork.Repository<Pantallas>().AsQueryable()
                .Where(p => usuario.EsAdministrador || usuario.Colaborador.Cargo.CargoId == 1 ||
                            unitOfWork.Repository<PantallasPorRoles>().AsQueryable().Any(ppr => ppr.RolId == usuario.RolId && ppr.PantallaId == p.PantallaId && p.Estado))
                .Select(p => new PantallaDto
                {
                    PantallaId = p.PantallaId,
                    Descripcion = p.Descripcion,
                    DireccionURL = p.DireccionURL
                })
                .ToListAsync();
        }

        private async Task<Response<string>> ValidarRegistrarDatosUsuario(UsuarioDto usuarioDto)
        {
            await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

            if (string.IsNullOrEmpty(usuarioDto.Usuario) || string.IsNullOrEmpty(usuarioDto.Clave) ||
                usuarioDto.ColaboradorId <= 0 || usuarioDto.RolId <= 0 || usuarioDto.UsuarioGuardaId <= 0)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ09 };
            }

            bool existe = await unitOfWork.Repository<Usuarios>().AsQueryable().AnyAsync(u => u.Usuario == usuarioDto.Usuario);
            if (existe)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ02.Replace("@Campo", "usuario") };
            }

            return new Response<string> { Exitoso = true }; 
        }

        private async Task<Response<string>> GuardarUsuario(UsuarioDto usuarioDto)
        {
            var nuevoUsuario = _mapper.Map<Usuarios>(usuarioDto);

            try
            {
                await using var unitOfWork = _unitOfWorkBuilder.BuildDbSIMOVIA();

                unitOfWork.Repository<Usuarios>().Add(nuevoUsuario);
                await unitOfWork.SaveChangesAsync();
                return new Response<string> { Exitoso = true, Mensaje = Mensajes.MSJ05.Replace("@Entidad", "Usuario") };
            }
            catch (Exception)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ07.Replace("@Entidad", "usuario") };
            }
        }

        public async Task<Response<string>> RegistrarUsuario(UsuarioDto usuarioDto)
        {
            var validacionResultado = await ValidarRegistrarDatosUsuario(usuarioDto);
            if (!validacionResultado.Exitoso)
            {
                return validacionResultado; 
            }

            return await GuardarUsuario(usuarioDto);
        }

        #endregion
    }
}
