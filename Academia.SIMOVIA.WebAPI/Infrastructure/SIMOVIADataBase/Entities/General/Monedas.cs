﻿using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using System.Diagnostics.CodeAnalysis;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General
{
    [ExcludeFromCodeCoverage]
    public class Monedas
    {
        public Monedas()
        {
            Nombre = string.Empty;
            Simbolo = string.Empty;
            Estado = true;
            UsuarioCreacion = new Usuarios();
            UsuarioModificacion = new Usuarios();
            MonedasPorPais = new HashSet<MonedasPorPais>();
        }

        public int MonedaId { get; set; }
        public string Nombre { get; set; }
        public string Simbolo { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }
        public virtual ICollection<MonedasPorPais> MonedasPorPais { get; set; }
    }
}
