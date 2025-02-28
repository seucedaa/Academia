namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    public class DistanceMatrixApiResponseDto
    {
        public string status { get; set; }
        public List<FilaDto> rows { get; set; }
    }

    public class FilaDto
    {
        public List<ElementoDto> elements { get; set; }
    }

    public class ElementoDto
    {
        public DistanciaTiempoDto distance { get; set; }
        public DistanciaTiempoDto duration { get; set; }
        public string status { get; set; }
    }

    public class DistanciaTiempoDto
    {
        public string text { get; set; }
        public int value { get; set; }
    }

}
