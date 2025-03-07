using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos
{
    [ExcludeFromCodeCoverage]
    public class InicioSesionDto
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
    }
}
