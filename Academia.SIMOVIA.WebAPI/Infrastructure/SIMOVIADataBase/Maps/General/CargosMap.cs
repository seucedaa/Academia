using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General
{
    public class CargosMap : IEntityTypeConfiguration<Cargos>
    {
        public void Configure(EntityTypeBuilder<Cargos> builder)
        {
            builder.ToTable("Cargos", "General");
            builder.HasKey(x => x.CargoId);

            builder.Property(x => x.CargoId)
           .HasColumnName("Cargo_Id")
           .IsRequired();

            builder.Property(x => x.Descripcion)
                   .HasColumnName("Descripcion")
                   .HasMaxLength(50)
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
                   .HasDatabaseName("UQ_Cargos_Descripcion");
            #endregion


            #region relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.CargosCreados)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.CargosModificados)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region relaciones uno a muchos
            builder.HasMany(x => x.Colaboradores)
                   .WithOne(c => c.Cargo)
                   .HasForeignKey(c => c.CargoId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
