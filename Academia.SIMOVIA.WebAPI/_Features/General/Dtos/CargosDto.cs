using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.General.Dtos
{
    [ExcludeFromCodeCoverage]
    public class CargosDto
    {
        public int CargoId { get; set; }
        public string Descripcion { get; set; }
    }
}
