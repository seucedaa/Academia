namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    public class SucursalDto
    {
        public int SucursalId { get; set; }
        public string Descripcion { get; set; }
        public string Telefono { get; set; }
        public string DireccionExacta { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int CiudadId { get; set; }
        public int UsuarioGuardaId { get; set; }
    }
}
