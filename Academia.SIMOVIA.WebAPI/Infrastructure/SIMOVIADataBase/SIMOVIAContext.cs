using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Viaje;
using Microsoft.EntityFrameworkCore;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase
{
    public class SimoviaContext : DbContext
    {
        public SimoviaContext(DbContextOptions<SimoviaContext> options) : base(options)
        {

        }

        #region Acceso
        public DbSet<Pantallas> Pantallas => Set<Pantallas>();
        public DbSet<PantallasPorRoles> PantallasPorRoles => Set<PantallasPorRoles>();
        public DbSet<Roles> Roles => Set<Roles>();
        public DbSet<Usuarios> Usuarios => Set<Usuarios>();
        #endregion

        #region Generales
        public DbSet<Cargos> Cargos => Set<Cargos>();
        public DbSet<Ciudades> Ciudades => Set<Ciudades>();
        public DbSet<Colaboradores> Colaboradores => Set<Colaboradores>();
        public DbSet<Estados> Estados => Set<Estados>();
        public DbSet<EstadosCiviles> EstadosCiviles => Set<EstadosCiviles>();
        public DbSet<Monedas> Monedas => Set<Monedas>();
        public DbSet<MonedasPorPais> MonedasPorPais => Set<MonedasPorPais>();
        public DbSet<Paises> Paises => Set<Paises>();
        #endregion

        #region Viaje
        public DbSet<ColaboradoresPorSucursal> ColaboradoresPorSucursal => Set<ColaboradoresPorSucursal>();
        public DbSet<EstadosSolicitudes> EstadosSolicitudes => Set<EstadosSolicitudes>();
        public DbSet<Notificaciones> Notificaciones => Set<Notificaciones>();
        public DbSet<Puntuaciones> Puntuaciones => Set<Puntuaciones>();
        public DbSet<Solicitudes> Solicitudes => Set<Solicitudes>();
        public DbSet<Sucursales> Sucursales => Set<Sucursales>();
        public DbSet<Transportistas> Transportistas => Set<Transportistas>();
        public DbSet<ViajesDetalle> ViajesDetalle => Set<ViajesDetalle>();
        public DbSet<ViajesEncabezado> ViajesEncabezado => Set<ViajesEncabezado>();
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Acceso
            modelBuilder.ApplyConfiguration(new PantallasMap());
            modelBuilder.ApplyConfiguration(new PantallasPorRolesMap());
            modelBuilder.ApplyConfiguration(new RolesMap());
            modelBuilder.ApplyConfiguration(new UsuariosMap());
            #endregion

            #region Generales
            modelBuilder.ApplyConfiguration(new CargosMap());
            modelBuilder.ApplyConfiguration(new CiudadesMap());
            modelBuilder.ApplyConfiguration(new ColaboradoresMap());
            modelBuilder.ApplyConfiguration(new EstadosMap());
            modelBuilder.ApplyConfiguration(new EstadosCivilesMap());
            modelBuilder.ApplyConfiguration(new MonedasMap());
            modelBuilder.ApplyConfiguration(new MonedasPorPaisMap());
            modelBuilder.ApplyConfiguration(new PaisesMap());
            #endregion

            #region Viajes
            modelBuilder.ApplyConfiguration(new ColaboradoresPorSucursalMap());
            modelBuilder.ApplyConfiguration(new EstadosSolicitudesMap());
            modelBuilder.ApplyConfiguration(new NotificacionesMap());
            modelBuilder.ApplyConfiguration(new PuntuacionesMap());
            modelBuilder.ApplyConfiguration(new SolicitudesMap());
            modelBuilder.ApplyConfiguration(new SucursalesMap());
            modelBuilder.ApplyConfiguration(new TransportistasMap());
            modelBuilder.ApplyConfiguration(new ViajesDetalleMap());
            modelBuilder.ApplyConfiguration(new ViajesEncabezadoMap());
            #endregion
        }
    }
}
