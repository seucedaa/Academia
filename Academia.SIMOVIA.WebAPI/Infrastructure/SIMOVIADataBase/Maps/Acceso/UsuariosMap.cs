using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Acceso
{
    public class UsuariosMap : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.ToTable("Usuarios", "Acceso");
            builder.HasKey(x => x.UsuarioId);

            builder.Property(x => x.UsuarioId)
           .HasColumnName("Usuario_Id")
           .IsRequired();

            builder.Property(x => x.Usuario)
                   .HasColumnName("Usuario")
                   .HasMaxLength(80)
                   .IsRequired();

            builder.Property(x => x.Clave)
                   .HasColumnName("Clave")
                   .IsRequired()
                   .HasMaxLength(64);

            builder.Property(x => x.EsAdministrador)
                   .HasColumnName("Es_Administrador")
                   .HasDefaultValue(false)
                   .IsRequired();

            builder.Property(x => x.ColaboradorId)
                   .HasColumnName("Colaborador_Id")
                   .IsRequired();

            builder.Property(x => x.RolId)
                   .HasColumnName("Rol_Id")
                   .IsRequired();

            builder.Property(x => x.UsuarioCreacionId)
                   .HasColumnName("Usuario_Creacion")
                   .IsRequired();

            builder.Property(x => x.FechaCreacion)
                   .HasColumnName("Fecha_Creacion")
                   .IsRequired();

            builder.Property(x => x.UsuarioModificacionId)
                   .HasColumnName("Usuario_Modificacion")
                   .IsRequired(false);

            builder.Property(x => x.FechaModificacion)
                   .HasColumnName("Fecha_Modificacion")
                   .IsRequired(false);

            builder.Property(x => x.Estado)
                   .HasColumnName("Estado")
                   .HasDefaultValue(true)
                   .IsRequired();

            #region Indices
            builder.HasIndex(x => x.Usuario)
                   .IsUnique()
                   .HasDatabaseName("UQ_Usuarios_Usuario");
            #endregion

            #region Relaciones Uno a uno o a muchos
            builder.HasOne(x => x.Colaborador)
                   .WithMany(c => c.Usuarios)
                   .HasForeignKey(x => x.ColaboradorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Rol)
                   .WithMany(r => r.Usuarios)
                   .HasForeignKey(x => x.RolId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.UsuariosCreados)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.UsuariosModificados)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Relaciones uno a muchos



            builder.HasMany(x => x.Notificaciones)
                   .WithOne(n => n.Receptor)
                   .HasForeignKey(n => n.ReceptorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Solicitudes)
                   .WithOne(s => s.Usuario)
                   .HasForeignKey(s => s.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.SolicitudesModificadas)
                   .WithOne(s => s.UsuarioAprobado)
                   .HasForeignKey(s => s.UsuarioAprobadoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.PantallasCreadas)
                   .WithOne(p => p.UsuarioCreacion)
                   .HasForeignKey(p => p.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.PantallasModificadas)
                   .WithOne(p => p.UsuarioModificacion)
                   .HasForeignKey(p => p.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.RolesCreados)
                   .WithOne(r => r.UsuarioCreacion)
                   .HasForeignKey(r => r.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.RolesModificados)
                   .WithOne(r => r.UsuarioModificacion)
                   .HasForeignKey(r => r.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.SucursalesCreadas)
                   .WithOne(s => s.UsuarioCreacion)
                   .HasForeignKey(s => s.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.SucursalesModificadas)
                   .WithOne(s => s.UsuarioModificacion)
                   .HasForeignKey(s => s.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.TransportistasCreados)
                   .WithOne(t => t.UsuarioCreacion)
                   .HasForeignKey(t => t.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.TransportistasModificados)
                   .WithOne(t => t.UsuarioModificacion)
                   .HasForeignKey(t => t.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ViajesEncabezadoCreados)
                   .WithOne(v => v.UsuarioCreacion)
                   .HasForeignKey(v => v.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ViajesEncabezadoModificados)
                   .WithOne(v => v.UsuarioModificacion)
                   .HasForeignKey(v => v.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.EstadosSolicitudesCreados)
                   .WithOne(es => es.UsuarioCreacion)
                   .HasForeignKey(es => es.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.EstadosSolicitudesModificados)
                   .WithOne(es => es.UsuarioModificacion)
                   .HasForeignKey(es => es.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.PuntuacionesCreadas)
                   .WithOne(p => p.UsuarioCreacion)
                   .HasForeignKey(p => p.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.PuntuacionesModificadas)
                   .WithOne(p => p.UsuarioModificacion)
                   .HasForeignKey(p => p.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.UsuariosCreados)
                   .WithOne(u => u.UsuarioCreacion)
                   .HasForeignKey(u => u.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.UsuariosModificados)
                   .WithOne(u => u.UsuarioModificacion)
                   .HasForeignKey(u => u.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CargosCreados)
                   .WithOne(c => c.UsuarioCreacion)
                   .HasForeignKey(c => c.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CargosModificados)
                   .WithOne(c => c.UsuarioModificacion)
                   .HasForeignKey(c => c.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.EstadosCivilesCreados)
                   .WithOne(ec => ec.UsuarioCreacion)
                   .HasForeignKey(ec => ec.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.EstadosCivilesModificados)
                   .WithOne(ec => ec.UsuarioModificacion)
                   .HasForeignKey(ec => ec.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.PaisesCreados)
                   .WithOne(p => p.UsuarioCreacion)
                   .HasForeignKey(p => p.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.PaisesModificados)
                   .WithOne(p => p.UsuarioModificacion)
                   .HasForeignKey(p => p.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MonedasCreadas)
                   .WithOne(m => m.UsuarioCreacion)
                   .HasForeignKey(m => m.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.MonedasModificadas)
                   .WithOne(m => m.UsuarioModificacion)
                   .HasForeignKey(m => m.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.EstadosCreados)
                   .WithOne(e => e.UsuarioCreacion)
                   .HasForeignKey(e => e.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.EstadosModificados)
                   .WithOne(e => e.UsuarioModificacion)
                   .HasForeignKey(e => e.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CiudadesCreadas)
                   .WithOne(c => c.UsuarioCreacion)
                   .HasForeignKey(c => c.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.CiudadesModificadas)
                   .WithOne(c => c.UsuarioModificacion)
                   .HasForeignKey(c => c.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ColaboradoresCreados)
                   .WithOne(col => col.UsuarioCreacion)
                   .HasForeignKey(col => col.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ColaboradoresModificados)
                   .WithOne(col => col.UsuarioModificacion)
                   .HasForeignKey(col => col.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }
    }
}
