using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {
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
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Colaborador.Telefono))
            .ReverseMap();
            #endregion

            #region Generales
            CreateMap<CargosDto, Cargos>().ReverseMap();
            CreateMap<EstadosCivilesDto, EstadosCiviles>().ReverseMap();
            CreateMap<CiudadesDto, Ciudades>().ReverseMap();

            CreateMap<Colaboradores, ColaboradoresDto>()
                .ForMember(dest => dest.EstadoCivilDescripcion, opt => opt.MapFrom(src => src.EstadoCivil.Descripcion))
                .ForMember(dest => dest.CargoDescripcion, opt => opt.MapFrom(src => src.Cargo.Descripcion))
                .ForMember(dest=>dest.CiudadDescripcion,opt=>opt.MapFrom(src =>src.Ciudad.Descripcion))
                .ForMember(dest => dest.Sexo, opt => opt.MapFrom(src => src.Sexo == "F" ? "Femenino" : "Masculino"))
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento.ToString("dd/MM/yyyy")))
                .ReverseMap();
            CreateMap<ColaboradorDto, Colaboradores>()
             .ForMember(dest => dest.UsuarioCreacionId, opt => opt.MapFrom((src, dest) => src.ColaboradorId == 0 ? src.UsuarioGuardaId : dest.UsuarioCreacionId))
             .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom((src, dest) => src.ColaboradorId == 0 ? DateTime.UtcNow : dest.FechaCreacion))
             .ForMember(dest => dest.UsuarioModificacionId, opt => opt.MapFrom(src => src.ColaboradorId != 0 ? src.UsuarioGuardaId : (int?)null))
             .ForMember(dest => dest.FechaModificacion, opt => opt.MapFrom(src => src.ColaboradorId != 0 ? DateTime.UtcNow : (DateTime?)null)) 
             .ReverseMap();

            #endregion

            #region Viaje
            CreateMap<Sucursales, SucursalesDto>()
                .ForMember(dest => dest.CiudadDescripcion, opt => opt.MapFrom(src => src.Ciudad.Descripcion))
                .ReverseMap();
            CreateMap<SucursalDto, Sucursales>()
             .ForMember(dest => dest.UsuarioCreacionId, opt => opt.MapFrom((src, dest) => src.SucursalId == 0 ? src.UsuarioGuardaId : dest.UsuarioCreacionId))
             .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom((src, dest) => src.SucursalId == 0 ? DateTime.UtcNow : dest.FechaCreacion))
             .ForMember(dest => dest.UsuarioModificacionId, opt => opt.MapFrom(src => src.SucursalId != 0 ? src.UsuarioGuardaId : (int?)null))
             .ForMember(dest => dest.FechaModificacion, opt => opt.MapFrom(src => src.SucursalId != 0 ? DateTime.UtcNow : (DateTime?)null))
             .ReverseMap();
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
