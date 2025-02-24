using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Acceso
{
    public class PantallasMap : IEntityTypeConfiguration<Pantallas>
    {
        public void Configure(EntityTypeBuilder<Pantallas> builder)
        {
            builder.ToTable("Pantallas", "Acceso");
            builder.HasKey(x => x.PantallaId);

            builder.Property(x => x.PantallaId)
           .HasColumnName("Pantalla_Id")
           .IsRequired();

            builder.Property(x => x.Descripcion)
                   .HasColumnName("Descripcion")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.DireccionURL)
                   .HasColumnName("Direccion_URL")
                   .HasMaxLength(80)
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
                    .HasDatabaseName("UQ_Pantallas_Descripcion");

            builder.HasIndex(x => x.DireccionURL)
                   .IsUnique()
                   .HasDatabaseName("UQ_Pantallas_Direccion_URL");

            #endregion

            #region Relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.PantallasCreadas)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.PantallasModificadas)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Relaciones uno a muchos
            builder.HasMany(x => x.PantallasPorRoles)
                   .WithOne(pr => pr.Pantalla)
                   .HasForeignKey(pr => pr.PantallaId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
