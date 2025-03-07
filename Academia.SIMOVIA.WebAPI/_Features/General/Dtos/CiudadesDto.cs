using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.General.Dtos
{
    [ExcludeFromCodeCoverage]
    public class CiudadesDto
    {
        public int CiudadId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}
