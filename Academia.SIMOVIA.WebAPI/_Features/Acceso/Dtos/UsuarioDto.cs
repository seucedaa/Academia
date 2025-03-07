using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos
{
    [ExcludeFromCodeCoverage]
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public bool EsAdministrador { get; set; }
        public int ColaboradorId { get; set; }
        public int RolId { get; set; }
        public int UsuarioGuardaId { get; set; }
    }
}
