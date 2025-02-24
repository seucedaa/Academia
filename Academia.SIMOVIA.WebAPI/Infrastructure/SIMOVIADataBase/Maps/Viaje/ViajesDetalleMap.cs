using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Viaje
{
    public class ViajesDetalleMap : IEntityTypeConfiguration<ViajesDetalle>
    {
        public void Configure(EntityTypeBuilder<ViajesDetalle> builder)
        {
            builder.ToTable("ViajesDetalle", "Viaje");
            builder.HasKey(x => x.ViajeDetalleId);

            builder.Property(x => x.ViajeDetalleId)
           .HasColumnName("ViajeDetalle_Id")
           .IsRequired();

            builder.Property(x => x.ViajeEncabezadoId)
                   .HasColumnName("ViajeEncabezado_Id")
                   .IsRequired();

            builder.Property(x => x.ColaboradorId)
                   .HasColumnName("Colaborador_Id")
                   .IsRequired();

            builder.Property(x => x.Estado)
                   .HasColumnName("Estado")
                   .HasDefaultValue(true)
                   .IsRequired();

            #region Relaciones uno a muchos
            builder.HasOne(x => x.ViajeEncabezado)
                   .WithMany(v => v.ViajesDetalle)
                   .HasForeignKey(x => x.ViajeEncabezadoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Colaborador)
                   .WithMany(c => c.ViajesDetalle)
                   .HasForeignKey(x => x.ColaboradorId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
