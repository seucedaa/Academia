using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Viaje
{
    public class EstadosSolicitudesMap : IEntityTypeConfiguration<EstadosSolicitudes>
    {
        public void Configure(EntityTypeBuilder<EstadosSolicitudes> builder)
        {
            builder.ToTable("EstadosSolicitudes", "Viaje");
            builder.HasKey(x => x.EstadoSolicitudId);

            builder.Property(x => x.EstadoSolicitudId)
           .HasColumnName("EstadoSolicitud_Id")
           .IsRequired();

            builder.Property(x => x.Descripcion)
                   .HasColumnName("Descripcion")
                   .HasMaxLength(20)
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
            builder.HasIndex(x => x.Descripcion)
                   .IsUnique()
                   .HasDatabaseName("UQ_EstadosSolicitudes_Descripcion");
            #endregion

            #region Relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.EstadosSolicitudesCreados)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.EstadosSolicitudesModificados)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Relaciones uno a muchos
            builder.HasMany(x => x.Solicitudes)
                   .WithOne(s => s.EstadoSolicitud)
                   .HasForeignKey(s => s.EstadoSolicitudId);
            #endregion
        }
    }
}
