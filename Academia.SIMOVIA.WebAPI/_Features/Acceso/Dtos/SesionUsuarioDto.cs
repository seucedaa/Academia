using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos
{
    [ExcludeFromCodeCoverage]
    public class SesionUsuarioDto
    {
        public SesionUsuarioDto()
        {
            Usuario = string.Empty;
            RolDescripcion = string.Empty;
            NombreColaborador = string.Empty;
            CorreoElectronico = string.Empty;
            Pantallas = new List<PantallaDto>();
            Sucursales = new List<int>();
        }

        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public bool EsAdministrador { get; set; }
        public int RolId { get; set; }
        public string RolDescripcion { get; set; }
        public string NombreColaborador { get; set; }
        public string CorreoElectronico { get; set; }
        public List<PantallaDto> Pantallas { get; set; }
        public List<int> Sucursales { get; set; }
    }

    public class PantallaDto
    {
        public PantallaDto()
        {
            Descripcion = string.Empty;
            DireccionURL = string.Empty;
        }

        public int PantallaId { get; set; }
        public string Descripcion { get; set; }
        public string DireccionURL { get; set; }
    }
}
