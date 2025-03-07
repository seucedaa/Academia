using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class DirectionsApiResponseDto
    {
        public int value { get; set; }
    }

    public class RutaDto
    {
        public List<LegDto> legs { get; set; } 
    }

    public class LegDto
    {
        public DirectionsApiResponseDto distance { get; set; } 
    }

    public class RutaGoogleDto
    {
        public List<RutaDto> routes { get; set; }

    }
}
