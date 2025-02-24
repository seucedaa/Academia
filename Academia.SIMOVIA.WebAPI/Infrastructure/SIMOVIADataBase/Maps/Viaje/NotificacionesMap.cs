using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Viaje
{
    public class NotificacionesMap : IEntityTypeConfiguration<Notificaciones>
    {
        public void Configure(EntityTypeBuilder<Notificaciones> builder)
        {
            builder.ToTable("Notificaciones", "Viaje");
            builder.HasKey(x => x.NotificacionId);

            builder.Property(x => x.NotificacionId)
           .HasColumnName("Notificacion_Id")
           .IsRequired();

            builder.Property(x => x.Titulo)
                   .HasColumnName("Titulo")
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(x => x.Descripcion)
                   .HasColumnName("Descripcion")
                   .HasMaxLength(150)
                   .IsRequired();

            builder.Property(x => x.Fecha)
                   .HasColumnName("Fecha")
                   .IsRequired();

            builder.Property(x => x.ReceptorId)
                   .HasColumnName("Receptor_Id")
                   .IsRequired();

            builder.Property(x => x.SolicitudId)
                   .HasColumnName("Solicitud_Id")
                   .IsRequired();

            #region Relaciones uno a muchos
            builder.HasOne(x => x.Receptor)
                   .WithMany(u => u.Notificaciones)
                   .HasForeignKey(x => x.ReceptorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Solicitud)
                   .WithMany(s => s.Notificaciones)
                   .HasForeignKey(x => x.SolicitudId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
