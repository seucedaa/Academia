using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General;
using AutoMapper;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            #region Acceso
            CreateMap<RolesDto, Roles>().ReverseMap();
            CreateMap<UsuariosDto, Usuarios>().ReverseMap();
            CreateMap<UsuarioDto, Usuarios>()
             .ForMember(dest => dest.Clave, opt => opt.MapFrom(src => ConvertirClaveACifrada(src.Clave)))
             .ForMember(dest => dest.UsuarioCreacionId, opt => opt.MapFrom((src, dest) => src.UsuarioId == 0 ? src.UsuarioGuardaId : dest.UsuarioCreacionId))
             .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom((src, dest) => src.UsuarioId == 0 ? DateTime.UtcNow : dest.FechaCreacion))
             .ForMember(dest => dest.UsuarioModificacionId, opt => opt.MapFrom(src => src.UsuarioId != 0 ? src.UsuarioGuardaId : (int?)null))
             .ForMember(dest => dest.FechaModificacion, opt => opt.MapFrom(src => src.UsuarioId != 0 ? DateTime.UtcNow : (DateTime?)null))
             .ReverseMap();
            CreateMap<Usuarios, SesionUsuarioDto>()
             .ForMember(dest => dest.RolDescripcion, opt => opt.MapFrom(src => src.Rol.Descripcion))
             .ForMember(dest => dest.NombreColaborador, opt => opt.MapFrom(src => src.Colaborador.Nombres + " " + src.Colaborador.Apellidos))
             .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.Colaborador.CorreoElectronico))
             .ForMember(dest => dest.Sucursales, opt => opt.MapFrom(src => src.Colaborador.ColaboradoresPorSucursal.Select(cs => cs.SucursalId)))
             .ReverseMap();
            #endregion

            #region Generales
            CreateMap<CargosDto, Cargos>().ReverseMap();
            CreateMap<EstadosCivilesDto, EstadosCiviles>().ReverseMap();
            CreateMap<CiudadesDto, Ciudades>().ReverseMap();

            CreateMap<Colaboradores, ColaboradoresDto>()
             .ForMember(dest => dest.EstadoCivilDescripcion, opt => opt.MapFrom(src => src.EstadoCivil.Descripcion))
             .ForMember(dest => dest.CargoDescripcion, opt => opt.MapFrom(src => src.Cargo.Descripcion))
             .ForMember(dest => dest.CiudadDescripcion, opt => opt.MapFrom(src => src.Ciudad.Descripcion))
             .ForMember(dest => dest.Sexo, opt => opt.MapFrom(src => src.Sexo == "F" ? "Femenino" : "Masculino"))
             .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento.ToString("dd/MM/yyyy")))
             .ReverseMap();
            CreateMap<ColaboradorDto, Colaboradores>()
             .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom((src, dest) => DateTime.UtcNow)).ReverseMap();
            CreateMap<SucursalesPorColaboradorDto, ColaboradoresPorSucursal>().ReverseMap();
            CreateMap<ColaboradorPorSucursalDto, ColaboradoresPorSucursal>().ForMember(cps => cps.ColaboradorPorSucursalId, ent => ent.Ignore());

            CreateMap<Colaboradores, ColaboradoresPorSucursalDto>()
             .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombres + " " + src.Apellidos))
             .ForMember(dest => dest.CiudadDescripcion, opt => opt.MapFrom(src => src.Ciudad.Descripcion))
             .ForMember(dest => dest.DistanciaKm, opt => opt.MapFrom((src, dest, destMember, context) =>
               context.Items.ContainsKey("Distancias") ? ((Dictionary<int, decimal>)context.Items["Distancias"]).GetValueOrDefault(src.ColaboradorId, 0): 0)).ReverseMap();
            #endregion

            #region Viaje
            CreateMap<Sucursales, SucursalesDto>()
             .ForMember(dest => dest.CiudadDescripcion, opt => opt.MapFrom(src => src.Ciudad.Descripcion)).ReverseMap();
            CreateMap<Sucursales, SucursalesListadoDto>()
              .ForMember(dest => dest.CiudadDescripcion, opt => opt.MapFrom(src => src.Ciudad.Descripcion)).ReverseMap();
            CreateMap<SucursalDto, Sucursales>().ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom((src, dest) => DateTime.UtcNow))
             .ReverseMap();
            CreateMap<Transportistas, TransportistasDto>()
            .ForMember(dest => dest.CiudadDescripcion, opt => opt.MapFrom(src => src.Ciudad.Descripcion))
            .ForMember(dest => dest.MonedaSimbolo, opt => opt.MapFrom(src => src.Ciudad.Estado.Pais.MonedasPorPais
               .Where(mp => mp.Principal)
               .Select(mp => mp.Moneda.Simbolo)
               .FirstOrDefault()
            ))
            .ReverseMap();

            CreateMap<ViajesEncabezado, ViajesDto>()
             .ForMember(dest => dest.SucursalDescripcion, opt => opt.MapFrom(src => src.Sucursal.Descripcion))
             .ForMember(dest => dest.Transportista, opt => opt.MapFrom(src => src.Transportista.Nombres + " " + src.Transportista.Apellidos))
             .ForMember(dest => dest.FechaHora, opt => opt.MapFrom(src => src.FechaHora.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture)))
             .ReverseMap();
            CreateMap<ViajeDto, ViajesEncabezado>()
             .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom((src, dest) => DateTime.UtcNow)).ReverseMap();
            CreateMap<ViajeDetallesDto, ViajesDetalle>().ReverseMap();
            CreateMap<ViajesEncabezado, ViajeReporteEncabezadoDto>().ReverseMap();
            CreateMap<Solicitudes, SolicitudesDto>()
             .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario.Usuario))
             .ForMember(dest => dest.EstadoSolicitud, opt => opt.MapFrom(src => src.EstadoSolicitud.Descripcion))
             .ForMember(dest => dest.ViajeEncabezado, opt => opt.MapFrom(src => "Viaje " + src.ViajeEncabezado.Sucursal.Descripcion + " - " +src.ViajeEncabezado.FechaHora.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture)))
             .ForMember(soli => soli.Sucursal, dto => dto.MapFrom(sucu => sucu.Sucursal.Descripcion))
             .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.Fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)))
             .ReverseMap();
            CreateMap<SolicitudDto, Solicitudes>().ReverseMap();
            CreateMap<ProcesarSolicitudDto, Solicitudes>().ReverseMap();
            #endregion
        }

        private byte[] ConvertirClaveACifrada(string clave)
        {
            if (string.IsNullOrEmpty(clave)) return null;

            using (SHA512 sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(Encoding.UTF8.GetBytes(clave));
            }
        }
    }
}
