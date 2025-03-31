using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.General.Dtos
{
    [ExcludeFromCodeCoverage]
    public class EstadosCivilesDto
    {
        public EstadosCivilesDto()
        {
            Descripcion = string.Empty;
        }
        public int EstadoCivilId { get; set; }
        public string Descripcion { get; set; }
    }
}
