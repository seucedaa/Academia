using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Acceso
{
    public class RolesMap : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles", "Acceso");
            builder.HasKey(x => x.RolId);

            builder.Property(x => x.RolId)  
           .HasColumnName("Rol_Id")  
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
                   .HasDatabaseName("UQ_Roles_Descripcion");
            #endregion

            #region Relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.RolesCreados)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.RolesModificados)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Relaciones uno a muchos
            builder.HasMany(x => x.PantallasPorRoles)
                   .WithOne(pr => pr.Rol)
                   .HasForeignKey(pr => pr.RolId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Usuarios)
                   .WithOne(u => u.Rol)
                   .HasForeignKey(u => u.RolId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
