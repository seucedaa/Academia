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
        public RutaDto()
        {
            legs = new List<LegDto>();
        }
        public List<LegDto> legs { get; set; }
    }

    public class LegDto
    {
        public LegDto()
        {
            distance = new DirectionsApiResponseDto();
        }
        public DirectionsApiResponseDto distance { get; set; }
    }

    public class RutaGoogleDto
    {
        public RutaGoogleDto()
        {
            routes = new List<RutaDto>();
        }
        public List<RutaDto> routes { get; set; }
    }
}
