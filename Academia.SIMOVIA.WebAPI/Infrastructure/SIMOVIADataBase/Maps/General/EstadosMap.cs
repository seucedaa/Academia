using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General
{
    public class EstadosMap : IEntityTypeConfiguration<Estados>
    {
        public void Configure(EntityTypeBuilder<Estados> builder)
        {
            builder.ToTable("Estados", "General");
            builder.HasKey(x => x.EstadoId);

            builder.Property(x => x.EstadoId)
           .HasColumnName("Estado_Id")
           .IsRequired();

            builder.Property(x => x.Codigo)
                   .HasColumnName("Codigo")
                   .HasMaxLength(3)
                   .IsRequired();

            builder.Property(x => x.Descripcion)
                   .HasColumnName("Descripcion")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.PaisId)
                   .HasColumnName("Pais_Id")
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

            #region Relaciones uno a muchos
            builder.HasOne(x => x.Pais)
                   .WithMany(p => p.Estados) 
                   .HasForeignKey(x => x.PaisId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.EstadosCreados) 
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.EstadosModificados) 
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
