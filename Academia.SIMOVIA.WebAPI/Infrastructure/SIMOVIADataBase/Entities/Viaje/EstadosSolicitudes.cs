﻿using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje
{
    public class EstadosSolicitudes
    {
        public EstadosSolicitudes()
        {
            Descripcion = string.Empty;
            Estado = true;

            Solicitudes = new HashSet<Solicitudes>();
        }

        public int EstadoSolicitudId { get; set; }
        public string Descripcion { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ICollection<Solicitudes> Solicitudes { get; set; }
    }
}
