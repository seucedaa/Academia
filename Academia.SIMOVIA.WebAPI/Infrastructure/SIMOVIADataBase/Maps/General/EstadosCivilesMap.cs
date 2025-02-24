using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General
{
    public class EstadosCivilesMap : IEntityTypeConfiguration<EstadosCiviles>
    {
        public void Configure(EntityTypeBuilder<EstadosCiviles> builder)
        {
            builder.ToTable("EstadosCiviles", "General");
            builder.HasKey(x => x.EstadoCivilId);

            builder.Property(x => x.EstadoCivilId)
                .HasColumnName("EstadoCivil_Id")
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
                   .HasDatabaseName("UQ_EstadosCiviles_Descripcion");
            #endregion


            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.EstadosCivilesCreados)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.EstadosCivilesModificados)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Colaboradores)
                   .WithOne(c => c.EstadoCivil)
                   .HasForeignKey(c => c.EstadoCivilId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Descripcion)
                .IsUnique()
                .HasDatabaseName("UQ_EstadosCiviles_Descripcion");
        }
    }
}
