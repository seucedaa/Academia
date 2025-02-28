using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Viaje
{
    public class SolicitudesMap : IEntityTypeConfiguration<Solicitudes>
    {
        public void Configure(EntityTypeBuilder<Solicitudes> builder)
        {
            builder.ToTable("Solicitudes", "Viaje");
            builder.HasKey(x => x.SolicitudId);

            builder.Property(x => x.SolicitudId)
           .HasColumnName("Solicitud_Id")
           .IsRequired();

            builder.Property(x => x.Descripcion)
                   .HasColumnName("Descripcion")
                   .HasMaxLength(80)
                   .IsRequired();

            builder.Property(x => x.Fecha)
                   .HasColumnName("Fecha")
                   .IsRequired();

            builder.Property(x => x.UsuarioId)
                   .HasColumnName("Usuario_Id")
                   .IsRequired();

            builder.Property(x => x.ViajeEncabezadoId)
                   .HasColumnName("ViajeEncabezado_Id")
                   .IsRequired(false); 
            builder.Property(x => x.SucursalId)
                   .HasColumnName("Sucursal_Id")
                   .IsRequired(); 

            builder.Property(x => x.EstadoSolicitudId)
                   .HasColumnName("EstadoSolicitud_Id")
                   .IsRequired();

            builder.Property(x => x.UsuarioAprobadoId)
                   .HasColumnName("Usuario_Aprobado")
                   .IsRequired(false);

            builder.Property(x => x.FechaAprobado)
                   .HasColumnName("Fecha_Aprobado")
                   .IsRequired(false);

            builder.Property(x => x.Estado)
                   .HasColumnName("Estado")
                   .HasDefaultValue(true)
                   .IsRequired();

            builder.Property(x => x.AgregarViajeSiguiente).HasColumnName("Agregar_Viaje_Siguiente").HasDefaultValue(true).IsRequired();

            builder.Property(x => x.FechaViaje).HasColumnName("Fecha_Viaje").IsRequired();

            #region Relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.Usuario)
                   .WithMany(u => u.Solicitudes)
                   .HasForeignKey(x => x.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioAprobado)
                   .WithMany(u => u.SolicitudesModificadas)
                   .HasForeignKey(x => x.UsuarioAprobadoId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Relaciones uno a muchos
            builder.HasOne(x => x.ViajeEncabezado)
                   .WithMany(v => v.Solicitudes)
                   .HasForeignKey(x => x.ViajeEncabezadoId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Sucursal)
                   .WithMany(v => v.Solicitudes)
                   .HasForeignKey(x => x.SucursalId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.EstadoSolicitud)
                   .WithMany(e => e.Solicitudes)
                   .HasForeignKey(x => x.EstadoSolicitudId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
