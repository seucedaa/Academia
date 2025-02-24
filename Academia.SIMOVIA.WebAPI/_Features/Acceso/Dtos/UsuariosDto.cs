namespace Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos
{
    public class UsuariosDto
    {
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public byte[] Clave { get; set; }
        public bool EsAdministrador { get; set; }
        public int ColaboradorId { get; set; }
        public int RolId { get; set; }
    }
}
