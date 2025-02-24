using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Viaje
{
    public class ColaboradoresPorSucursalMap : IEntityTypeConfiguration<ColaboradoresPorSucursal>
    {
        public void Configure(EntityTypeBuilder<ColaboradoresPorSucursal> builder)
        {
            builder.ToTable("ColaboradoresPorSucursal", "Viaje");
            builder.HasKey(x => x.ColaboradorPorSucursalId);

            builder.Property(x => x.ColaboradorPorSucursalId)
           .HasColumnName("ColaboradorPorSucursal_Id")
           .IsRequired();

            builder.Property(x => x.DistanciaKm)
                   .HasColumnName("Distancia_Km")
                   .HasColumnType("decimal(4,2)")
                   .IsRequired();

            builder.Property(x => x.ColaboradorId)
                   .HasColumnName("Colaborador_Id")
                   .IsRequired();

            builder.Property(x => x.SucursalId)
                   .HasColumnName("Sucursal_Id")
                   .IsRequired();

            #region Relaciones uno a muchos
            builder.HasOne(x => x.Colaborador)
                   .WithMany(c => c.ColaboradoresPorSucursal)
                   .HasForeignKey(x => x.ColaboradorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sucursal)
                   .WithMany(s => s.ColaboradoresPorSucursal)
                   .HasForeignKey(x => x.SucursalId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
