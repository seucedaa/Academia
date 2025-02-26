namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    public class ColaboradoresPorSucursalDto
    {
        public int ColaboradorId { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string DireccionExacta { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string CiudadDescripcion { get; set; }
        public decimal DistanciaKm { get; set; }
    }
}
