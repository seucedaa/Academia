using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso
{
    public class Usuarios
    {
        public Usuarios()
        {
            Usuario = string.Empty;
            Estado = true;

            Notificaciones = new HashSet<Notificaciones>();
            Solicitudes = new HashSet<Solicitudes>();

            UsuariosCreados = new HashSet<Usuarios>();
            UsuariosModificados = new HashSet<Usuarios>();
            PantallasCreadas = new HashSet<Pantallas>();
            PantallasModificadas = new HashSet<Pantallas>();
            RolesCreados = new HashSet<Roles>();
            RolesModificados = new HashSet<Roles>();
            SucursalesCreadas = new HashSet<Sucursales>();
            SucursalesModificadas = new HashSet<Sucursales>();
            TransportistasCreados = new HashSet<Transportistas>();
            TransportistasModificados = new HashSet<Transportistas>();
            ViajesEncabezadoCreados = new HashSet<ViajesEncabezado>();
            ViajesEncabezadoModificados = new HashSet<ViajesEncabezado>();
            EstadosSolicitudesCreados = new HashSet<EstadosSolicitudes>();
            EstadosSolicitudesModificados = new HashSet<EstadosSolicitudes>();
            PuntuacionesCreadas = new HashSet<Puntuaciones>();
            PuntuacionesModificadas = new HashSet<Puntuaciones>();
            SolicitudesModificadas = new HashSet<Solicitudes>();
            CargosCreados = new HashSet<Cargos>();
            CargosModificados = new HashSet<Cargos>();
            EstadosCivilesCreados = new HashSet<EstadosCiviles>();
            EstadosCivilesModificados = new HashSet<EstadosCiviles>();
            PaisesCreados = new HashSet<Paises>();
            PaisesModificados = new HashSet<Paises>();
            MonedasCreadas = new HashSet<Monedas>();
            MonedasModificadas = new HashSet<Monedas>();
            EstadosCreados = new HashSet<Estados>();
            EstadosModificados = new HashSet<Estados>();
            CiudadesCreadas = new HashSet<Ciudades>();
            CiudadesModificadas = new HashSet<Ciudades>();
            ColaboradoresCreados = new HashSet<Colaboradores>();
            ColaboradoresModificados = new HashSet<Colaboradores>();
        }

        public int UsuarioId { get; set; }
        public string Usuario { get; set; }
        public byte[] Clave { get; set; }
        public bool EsAdministrador { get; set; }
        public int ColaboradorId { get; set; }
        public int RolId { get; set; }
        public int UsuarioCreacionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? UsuarioModificacionId { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; }

        public virtual Colaboradores Colaborador { get; set; }
        public virtual Roles Rol { get; set; }
        public virtual Usuarios UsuarioCreacion { get; set; }
        public virtual Usuarios UsuarioModificacion { get; set; }

        public virtual ICollection<Notificaciones> Notificaciones { get; set; }
        public virtual ICollection<Solicitudes> Solicitudes { get; set; }

        #region Relaciones de Auditoria
        public virtual ICollection<Usuarios> UsuariosCreados { get; set; }  
        public virtual ICollection<Usuarios> UsuariosModificados { get; set; }
        public virtual ICollection<Pantallas> PantallasCreadas { get; set; }
        public virtual ICollection<Pantallas> PantallasModificadas { get; set; }
        public virtual ICollection<Roles> RolesCreados { get; set; }
        public virtual ICollection<Roles> RolesModificados { get; set; }   
        public virtual ICollection<Sucursales> SucursalesCreadas { get; set; }   
        public virtual ICollection<Sucursales> SucursalesModificadas { get; set; }   
        public virtual ICollection<Transportistas> TransportistasCreados { get; set; }   
        public virtual ICollection<Transportistas> TransportistasModificados{ get; set; }   
        public virtual ICollection<ViajesEncabezado> ViajesEncabezadoCreados{ get; set; }   
        public virtual ICollection<ViajesEncabezado> ViajesEncabezadoModificados{ get; set; }   
        public virtual ICollection<EstadosSolicitudes> EstadosSolicitudesCreados{ get; set; }   
        public virtual ICollection<EstadosSolicitudes> EstadosSolicitudesModificados{ get; set; }   
        public virtual ICollection<Puntuaciones> PuntuacionesCreadas{ get; set; }   
        public virtual ICollection<Puntuaciones> PuntuacionesModificadas{ get; set; }   
        public virtual ICollection<Solicitudes> SolicitudesModificadas{ get; set; }   
        public virtual ICollection<Cargos> CargosCreados{ get; set; }   
        public virtual ICollection<Cargos> CargosModificados{ get; set; }   
        public virtual ICollection<EstadosCiviles> EstadosCivilesCreados{ get; set; }   
        public virtual ICollection<EstadosCiviles> EstadosCivilesModificados{ get; set; }   
        public virtual ICollection<Paises> PaisesCreados{ get; set; }   
        public virtual ICollection<Paises> PaisesModificados{ get; set; }   
        public virtual ICollection<Monedas> MonedasCreadas{ get; set; }   
        public virtual ICollection<Monedas> MonedasModificadas{ get; set; }   
        public virtual ICollection<Estados> EstadosCreados{ get; set; }   
        public virtual ICollection<Estados> EstadosModificados{ get; set; }   
        public virtual ICollection<Ciudades> CiudadesCreadas { get; set; }
        public virtual ICollection<Ciudades> CiudadesModificadas{ get; set; }   
        public virtual ICollection<Colaboradores> ColaboradoresCreados{ get; set; }   
        public virtual ICollection<Colaboradores> ColaboradoresModificados{ get; set; }
        #endregion

    }
}
