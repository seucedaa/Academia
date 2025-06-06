﻿using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Academia.SIMOVIA.WebAPI._Features.Acceso
{
    public class AccesoService
    {
        private readonly IMapper _mapper;
        private readonly AccesoDomainService _accesoDomainService;
        private readonly Farsiman.Domain.Core.Standard.Repositories.IUnitOfWork _unitOfWork;

        public AccesoService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, AccesoDomainService accesoDomainService)
        {
            _mapper = mapper;
            _accesoDomainService = accesoDomainService;
            _unitOfWork = unitOfWorkBuilder.BuildDbSIMOVIA();
        }

        #region Usuarios
        public async Task<Response<List<UsuariosDto>>> ObtenerUsuarios()
        {
            try
            {
                var listado = await _unitOfWork.Repository<Usuarios>().AsQueryable()
                    .Where(u => u.Estado)
                    .ToListAsync();

                if (listado.Count == 0)
                {
                    return new Response<List<UsuariosDto>>
                    {
                        Exitoso = false,
                        Mensaje = Mensajes.SIN_REGISTROS.Replace("@entidad", "usuarios")
                    };
                }

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
                    Mensaje = Mensajes.ERROR_LISTADO.Replace("@entidad", "usuarios")
                };
            }
            catch (Exception)
            {
                return new Response<List<UsuariosDto>>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_GENERAL
                };
            }
        }

        private async Task<SesionUsuarioDto> ObtenerDatosSesionUsuario(string usuarioNombre)
        {
            Usuarios? usuario = await _unitOfWork.Repository<Usuarios>().AsQueryable()
                .Include(u => u.Rol)
                .Include(u => u.Colaborador!)
                    .ThenInclude(c => c!.Cargo)
                .Include(u => u.Colaborador!.ColaboradoresPorSucursal)
                .FirstOrDefaultAsync(u => u.Usuario == usuarioNombre);

            if (usuario == null || usuario.Colaborador == null)
                return new SesionUsuarioDto { Pantallas = new List<PantallaDto>() };

            List<PantallaDto> pantallas = await ObtenerPantallasPermitidas(usuario);

            SesionUsuarioDto sesionUsuarioDto = _mapper.Map<SesionUsuarioDto>(usuario);
            sesionUsuarioDto.Pantallas = pantallas;

            return sesionUsuarioDto;
        }

        public async Task<Response<SesionUsuarioDto>> InicioSesion(InicioSesionDto login)
        {
            try
            {
                Usuarios? usuario = await _unitOfWork.Repository<Usuarios>()
                    .AsQueryable()
                    .FirstOrDefaultAsync(u => u.Usuario == login.Usuario);

                if (usuario == null)
                    return new Response<SesionUsuarioDto> { Exitoso = false, Mensaje = Mensajes.CREDENCIALES_INCORRECTAS };

                Response<string> resultadoValidacion = _accesoDomainService.ValidarInicioSesion(login, usuario);

                if (!resultadoValidacion.Exitoso)
                    return new Response<SesionUsuarioDto> { Exitoso = false, Mensaje = resultadoValidacion.Mensaje };

                SesionUsuarioDto usuarioSesion = await ObtenerDatosSesionUsuario(login.Usuario);

                return new Response<SesionUsuarioDto>
                {
                    Exitoso = true,
                    Mensaje = Mensajes.SESION_EXITOSA,
                    Data = usuarioSesion
                };
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("base de datos"))
                    return new Response<SesionUsuarioDto> { Exitoso = false, Mensaje = Mensajes.ERROR_BASE_DE_DATOS };
                return new Response<SesionUsuarioDto> { Exitoso = false, Mensaje = Mensajes.ERROR_GENERAL };
            }
        }

        private async Task<List<PantallaDto>> ObtenerPantallasPermitidas(Usuarios? usuario)
        {
            if (usuario == null) return new List<PantallaDto>();

            bool esAdministrador = usuario.EsAdministrador;
            bool tieneCargoValido = usuario.Colaborador?.Cargo?.CargoId == 1;

            return await (from pantallas in _unitOfWork.Repository<Pantallas>().AsQueryable()
                          where esAdministrador || tieneCargoValido ||
                          (from pantallaPorRol in _unitOfWork.Repository<PantallasPorRoles>().AsQueryable()
                           where pantallaPorRol.RolId == usuario.RolId && pantallaPorRol.PantallaId == pantallas.PantallaId
                           && pantallas.Estado
                           select pantallaPorRol).Any()
                          select new PantallaDto
                          {
                              PantallaId = pantallas.PantallaId,
                              Descripcion = pantallas.Descripcion,
                              DireccionURL = pantallas.DireccionURL
                          }).ToListAsync();
        }


        #endregion
    }
}
