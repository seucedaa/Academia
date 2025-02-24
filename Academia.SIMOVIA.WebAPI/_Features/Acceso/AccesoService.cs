using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
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
        private readonly SIMOVIAContext _context;
        private readonly IMapper _mapper;
        public AccesoService(SIMOVIAContext simovia, IMapper mapper)
        {
            _context = simovia;
            _mapper = mapper;
        }
        #region Roles
        public async Task<Response<List<RolesDto>>> ObtenerRoles()
        {
            try
            {
                var listado = await _context.Roles
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
                var listado = await _context.Usuarios
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


        private async Task<Response<string>> ValidarInicioSesion(InicioSesionDto login)
        {
            if (string.IsNullOrEmpty(login.Usuario) || string.IsNullOrEmpty(login.Clave))
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ08 };
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Usuario == login.Usuario);

            if (usuario == null)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ01_Credenciales_Incorrectas };
            }

            byte[] claveCifrada;
            using (SHA512 sha512 = SHA512.Create())
            {
                claveCifrada = sha512.ComputeHash(Encoding.UTF8.GetBytes(login.Clave));
            }

            if (!usuario.Clave.SequenceEqual(claveCifrada))
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ01_Credenciales_Incorrectas };
            }

            if (!usuario.Estado)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ03.Replace("@Entidad", "Usuario") };
            }

            return new Response<string> { Exitoso = true };
        }

        private async Task<SesionUsuarioDto> ObtenerDatosSesionUsuario(string usuarioNombre)
        {
            var usuario = await _context.Usuarios
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
            var resultadoValidacion = await ValidarInicioSesion(login);
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
            return await _context.Pantallas
                .Where(p => usuario.EsAdministrador || usuario.Colaborador.Cargo.CargoId == 1 ||
                            _context.PantallasPorRoles.Any(ppr => ppr.RolId == usuario.RolId && ppr.PantallaId == p.PantallaId && p.Estado))
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
            if (string.IsNullOrEmpty(usuarioDto.Usuario) || string.IsNullOrEmpty(usuarioDto.Clave) ||
                usuarioDto.ColaboradorId <= 0 || usuarioDto.RolId <= 0 || usuarioDto.UsuarioGuardaId <= 0)
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ09 };
            }

            bool existe = await _context.Usuarios.AnyAsync(u => u.Usuario == usuarioDto.Usuario);
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
                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();
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
