using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General
{
    public class MonedasPorPaisMap : IEntityTypeConfiguration<MonedasPorPais>
    {
        public void Configure(EntityTypeBuilder<MonedasPorPais> builder)
        {
            builder.ToTable("MonedasPorPais", "General");
            builder.HasKey(x => x.MonedaPorPaisId);

            builder.Property(x => x.MonedaPorPaisId)
           .HasColumnName("MonedaPorPais_Id")
           .IsRequired();

            builder.Property(x => x.PaisId)
                   .HasColumnName("Pais_Id")
                   .IsRequired();

            builder.Property(x => x.MonedaId)
                   .HasColumnName("Moneda_Id")
                   .IsRequired();

            builder.Property(x => x.Principal)
                   .HasColumnName("Principal")
                   .HasDefaultValue(false)
                   .IsRequired();

            #region Relaciones uno a muchos
            builder.HasOne(x => x.Pais)
                   .WithMany(p => p.MonedasPorPais)
                   .HasForeignKey(x => x.PaisId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Moneda)
                   .WithMany(m => m.MonedasPorPais)
                   .HasForeignKey(x => x.MonedaId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
