using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General
{
    public class MonedasMap : IEntityTypeConfiguration<Monedas>
    {
        public void Configure(EntityTypeBuilder<Monedas> builder)
        {
            builder.ToTable("Monedas", "General");
            builder.HasKey(x => x.MonedaId);

            builder.Property(x => x.MonedaId)
           .HasColumnName("Moneda_Id")
           .IsRequired();

            builder.Property(x => x.Nombre)
                   .HasColumnName("Nombre")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.Simbolo)
                   .HasColumnName("Simbolo")
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

            builder.Property(x => x.Estado)
                   .HasColumnName("Estado")
                   .HasDefaultValue(true)
                   .IsRequired();

            #region Relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.MonedasCreadas)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.MonedasModificadas)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Relaciones uno a muchos
            builder.HasMany(x => x.MonedasPorPais)
                   .WithOne(mp => mp.Moneda)
                   .HasForeignKey(mp => mp.MonedaId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
