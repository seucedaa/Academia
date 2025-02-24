using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.Acceso
{
    public class PantallasPorRolesMap : IEntityTypeConfiguration<PantallasPorRoles>
    {
        public void Configure(EntityTypeBuilder<PantallasPorRoles> builder)
        {
            builder.ToTable("PantallasPorRoles", "Acceso");
            builder.HasKey(x => x.PantallaPorRolId);

            builder.Property(x => x.PantallaPorRolId)
           .HasColumnName("PantallaPorRol_Id")
           .IsRequired();

            builder.Property(x => x.PantallaId)
                   .HasColumnName("Pantalla_Id")
                   .IsRequired();

            builder.Property(x => x.RolId)
                   .HasColumnName("Rol_Id")
                   .IsRequired();

            #region Relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.Pantalla)
                   .WithMany(p => p.PantallasPorRoles)
                   .HasForeignKey(x => x.PantallaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Rol)
                   .WithMany(r => r.PantallasPorRoles)
                   .HasForeignKey(x => x.RolId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
