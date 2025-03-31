using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.General.Dtos
{
    [ExcludeFromCodeCoverage]
    public class CargosDto
    {
        public CargosDto()
        {
            Descripcion = string.Empty;
        }
        public int CargoId { get; set; }
        public string Descripcion { get; set; }
    }
}
