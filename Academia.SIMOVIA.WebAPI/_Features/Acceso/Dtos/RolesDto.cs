using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos
{
    [ExcludeFromCodeCoverage]
    public class RolesDto
    {
        public RolesDto()
        {
            Descripcion = string.Empty;
        }

        public int RolId { get; set; }
        public string Descripcion { get; set; }
    }
}
