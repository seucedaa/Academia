using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Viaje
{
    public class SucursalesMap : IEntityTypeConfiguration<Sucursales>
    {
        public void Configure(EntityTypeBuilder<Sucursales> builder)
        {
            builder.ToTable("Sucursales", "Viaje");
            builder.HasKey(x => x.SucursalId);

            builder.Property(x => x.SucursalId)
           .HasColumnName("Sucursal_Id")
           .IsRequired();

            builder.Property(x => x.Descripcion)
                   .HasColumnName("Descripcion")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.Telefono)
                   .HasColumnName("Telefono")
                   .HasMaxLength(8)
                   .IsRequired();

            builder.Property(x => x.DireccionExacta)
                   .HasColumnName("Direccion_Exacta")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Latitud)
                   .HasColumnName("Latitud")
                   .HasColumnType("decimal(19,15)")
                   .IsRequired();

            builder.Property(x => x.Longitud)
                   .HasColumnName("Longitud")
                   .HasColumnType("decimal(19,15)")
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
            builder.HasIndex(x => x.Descripcion)
                   .IsUnique()
                   .HasDatabaseName("UQ_Sucursales_Descripcion");
            #endregion


            #region Relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.Ciudad)
                   .WithMany(c => c.Sucursales)
                   .HasForeignKey(x => x.CiudadId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.SucursalesCreadas)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.SucursalesModificadas)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Relaciones uno a muchos
            builder.HasMany(x => x.ColaboradoresPorSucursal)
                   .WithOne(cps => cps.Sucursal)
                   .HasForeignKey(cps => cps.SucursalId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Viajes)
                   .WithOne(v => v.Sucursal)
                   .HasForeignKey(v => v.SucursalId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Solicitudes)
                   .WithOne(v => v.Sucursal)
                   .HasForeignKey(v => v.SucursalId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
