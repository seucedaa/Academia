using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Viaje
{
    public class ViajesEncabezadoMap : IEntityTypeConfiguration<ViajesEncabezado>
    {
        public void Configure(EntityTypeBuilder<ViajesEncabezado> builder)
        {
            builder.ToTable("ViajesEncabezado", "Viaje");
            builder.HasKey(x => x.ViajeEncabezadoId);

            builder.Property(x => x.ViajeEncabezadoId)
           .HasColumnName("ViajeEncabezado_Id")
           .IsRequired();

            builder.Property(x => x.FechaHora)
                   .HasColumnName("FechaHora")
                   .IsRequired();

            builder.Property(x => x.DistanciaTotalKm)
                   .HasColumnName("Distancia_Total_Km")
                   .HasColumnType("decimal(5,2)")
                   .IsRequired();

            builder.Property(x => x.TarifaTransportista)
                   .HasColumnName("Tarifa_Transportista")
                   .HasColumnType("decimal(6,2)")
                   .IsRequired();

            builder.Property(x => x.Total)
                   .HasColumnName("Total")
                   .HasColumnType("decimal(8,2)")
                   .IsRequired();

            builder.Property(x => x.SucursalId)
                   .HasColumnName("Sucursal_Id")
                   .IsRequired();

            builder.Property(x => x.TransportistaId)
                   .HasColumnName("Transportista_Id")
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

            #region Relaciones uno a muchos
            builder.HasOne(x => x.Sucursal)
                   .WithMany(s => s.Viajes)
                   .HasForeignKey(x => x.SucursalId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Transportista)
                   .WithMany(t => t.Viajes)
                   .HasForeignKey(x => x.TransportistaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.ViajesEncabezadoCreados)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.ViajesEncabezadoModificados)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
