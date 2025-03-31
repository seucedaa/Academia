using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General
{
    public class CiudadesMap : IEntityTypeConfiguration<Ciudades>
    {
        public void Configure(EntityTypeBuilder<Ciudades> builder)
        {
            builder.ToTable("Ciudades", "General");
            builder.HasKey(x => x.CiudadId);

            builder.Property(x => x.CiudadId)
           .HasColumnName("Ciudad_Id")
           .IsRequired();

            builder.Property(x => x.Codigo)
                   .HasColumnName("Codigo")
                   .HasMaxLength(5)
                   .IsRequired();

            builder.Property(x => x.Descripcion)
                   .HasColumnName("Descripcion")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.EstadoId)
                   .HasColumnName("Estado_Id")
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

            #region relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.Estado)
                   .WithMany(e => e.Ciudades)
                   .HasForeignKey(x => x.EstadoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.CiudadesCreadas)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.CiudadesModificadas)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region relaciones uno a muchos
            builder.HasMany(x => x.Colaboradores)
                   .WithOne(c => c.Ciudad)
                   .HasForeignKey(c => c.CiudadId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Sucursales)
                   .WithOne(s => s.Ciudad)
                   .HasForeignKey(s => s.CiudadId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Transportistas)
                   .WithOne(t => t.Ciudad)
                   .HasForeignKey(t => t.CiudadId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
