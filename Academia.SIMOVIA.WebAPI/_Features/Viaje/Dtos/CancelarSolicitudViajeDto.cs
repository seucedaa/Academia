namespace Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos
{
    public class CancelarSolicitudViajeDto
    {
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public int? ViajeEncabezadoId { get; set; }
    }
}
