using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General
{
    public class PaisesMap : IEntityTypeConfiguration<Paises>
    {
        public void Configure(EntityTypeBuilder<Paises> builder)
        {
            builder.ToTable("Paises", "General");
            builder.HasKey(x => x.PaisId);

            builder.Property(x => x.PaisId)
           .HasColumnName("Pais_Id")
           .IsRequired();

            builder.Property(x => x.Codigo)
                   .HasColumnName("Codigo")
                   .HasMaxLength(3)
                   .IsRequired();

            builder.Property(x => x.Descripcion)
                   .HasColumnName("Descripcion")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.CodigoTelefonico)
                   .HasColumnName("Codigo_Telefonico")
                   .HasMaxLength(5)
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

            #region Relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.PaisesCreados)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.PaisesModificados)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Relaciones uno a muchos
            builder.HasMany(x => x.MonedasPorPais)
                   .WithOne(mp => mp.Pais)
                   .HasForeignKey(mp => mp.PaisId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Estados)
                   .WithOne(e => e.Pais)
                   .HasForeignKey(e => e.PaisId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
