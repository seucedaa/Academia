using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos
{
    [ExcludeFromCodeCoverage]
    public class UsuariosDto
    {
        public UsuariosDto()
        {
            Usuario = string.Empty;
            Clave = Array.Empty<byte>();
        }

        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public byte[] Clave { get; set; }
        public bool EsAdministrador { get; set; }
        public int ColaboradorId { get; set; }
        public int RolId { get; set; }
    }
}
