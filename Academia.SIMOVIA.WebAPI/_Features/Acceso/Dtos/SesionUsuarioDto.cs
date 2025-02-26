namespace Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos
{
    public class SesionUsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public bool EsAdministrador { get; set; }
        public int RolId { get; set; }
        public string RolDescripcion { get; set; }
        public string NombreColaborador { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public List<PantallaDto> Pantallas { get; set; }
        public List<int> Sucursales { get; set; }
    }

    public class PantallaDto
    {
        public int PantallaId { get; set; }
        public string Descripcion { get; set; }
        public string DireccionURL { get; set; }
    }
}
