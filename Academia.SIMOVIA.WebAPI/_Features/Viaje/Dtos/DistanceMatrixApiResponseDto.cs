using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    [ExcludeFromCodeCoverage]
    public class DistanceMatrixApiResponseDto
    {
        public DistanceMatrixApiResponseDto()
        {
            status = string.Empty;
            rows = new List<FilaDto>();
        }

        public string status { get; set; }
        public List<FilaDto> rows { get; set; }
    }

    public class FilaDto
    {
        public FilaDto()
        {
            elements = new List<ElementoDto>();
        }

        public List<ElementoDto> elements { get; set; }
    }

    public class ElementoDto
    {
        public ElementoDto()
        {
            distance = new DistanciaTiempoDto();
            duration = new DistanciaTiempoDto();
            status = string.Empty;
        }

        public DistanciaTiempoDto distance { get; set; }
        public DistanciaTiempoDto duration { get; set; }
        public string status { get; set; }
    }

    public class DistanciaTiempoDto
    {
        public DistanciaTiempoDto()
        {
            text = string.Empty;
        }

        public string text { get; set; }
        public int value { get; set; }
    }
}
