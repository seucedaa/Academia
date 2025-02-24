using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Viaje
{
    public class TransportistasMap : IEntityTypeConfiguration<Transportistas>
    {
        public void Configure(EntityTypeBuilder<Transportistas> builder)
        {
            builder.ToTable("Transportistas", "Viaje");
            builder.HasKey(x => x.TransportistaId);

            builder.Property(x => x.TransportistaId)
           .HasColumnName("Transportista_Id")
           .IsRequired();

            builder.Property(x => x.DNI)
                   .HasColumnName("DNI")
                   .HasMaxLength(13)
                   .IsRequired();

            builder.Property(x => x.Nombres)
                   .HasColumnName("Nombres")
                   .HasMaxLength(60)
                   .IsRequired();

            builder.Property(x => x.Apellidos)
                   .HasColumnName("Apellidos")
                   .HasMaxLength(60)
                   .IsRequired();

            builder.Property(x => x.Telefono)
                   .HasColumnName("Telefono")
                   .HasMaxLength(8)
                   .IsRequired();

            builder.Property(x => x.Tarifa)
                   .HasColumnName("Tarifa")
                   .HasColumnType("decimal(6,2)")
                   .IsRequired();

            builder.Property(x => x.CiudadId)
                   .HasColumnName("Ciudad_Id")
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
            builder.HasIndex(x => x.DNI)
                   .IsUnique()
                   .HasDatabaseName("UQ_Transportistas_DNI");
            #endregion


            #region Relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.Ciudad)
                   .WithMany(c => c.Transportistas)
                   .HasForeignKey(x => x.CiudadId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.TransportistasCreados)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.TransportistasModificados)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Relaciones uno a muchos
            builder.HasMany(x => x.Viajes)
                   .WithOne(v => v.Transportista)
                   .HasForeignKey(v => v.TransportistaId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
