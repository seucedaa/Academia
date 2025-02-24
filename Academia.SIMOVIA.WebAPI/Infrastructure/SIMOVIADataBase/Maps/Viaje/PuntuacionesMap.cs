using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Viaje
{
    public class PuntuacionesMap : IEntityTypeConfiguration<Puntuaciones>
    {
        public void Configure(EntityTypeBuilder<Puntuaciones> builder)
        {
            builder.ToTable("Puntuaciones", "Viaje");
            builder.HasKey(x => x.PuntuacionId);

            builder.Property(x => x.PuntuacionId)
           .HasColumnName("Puntuacion_Id")
           .IsRequired();

            builder.Property(x => x.Cantidad)
                   .HasColumnName("Cantidad")
                   .IsRequired();

            builder.Property(x => x.Descripcion)
                   .HasColumnName("Descripcion")
                   .HasMaxLength(80)
                   .IsRequired(false);

            builder.Property(x => x.ViajeEncabezadoId)
                   .HasColumnName("ViajeEncabezado_Id")
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

            #region Relaciones uno a muchos
            builder.HasOne(x => x.ViajeEncabezado)
                   .WithMany(v => v.Puntuaciones)
                   .HasForeignKey(x => x.ViajeEncabezadoId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.PuntuacionesCreadas)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.PuntuacionesModificadas)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
