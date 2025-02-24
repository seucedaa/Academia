using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Maps.General
{
    public class ColaboradoresMap : IEntityTypeConfiguration<Colaboradores>
    {
        public void Configure(EntityTypeBuilder<Colaboradores> builder)
        {
            builder.ToTable("Colaboradores", "General");
            builder.HasKey(x => x.ColaboradorId);

            builder.Property(x => x.ColaboradorId)
           .HasColumnName("Colaborador_Id")
           .IsRequired();

            builder.Property(x => x.DNI)
                   .HasColumnName("DNI")
                   .HasMaxLength(13)
                   .IsRequired();

            builder.Property(x => x.Nombres)
                   .HasColumnName("Nombres")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.Apellidos)
                   .HasColumnName("Apellidos")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.CorreoElectronico)
                   .HasColumnName("Correo_Electronico")
                   .HasMaxLength(60)
                   .IsRequired();

            builder.Property(x => x.Telefono)
                   .HasColumnName("Telefono")
                   .HasMaxLength(8)
                   .IsRequired();

            builder.Property(x => x.Sexo)
                   .HasColumnName("Sexo")
                   .HasMaxLength(1)
                   .IsRequired();

            builder.Property(x => x.FechaNacimiento)
                   .HasColumnName("Fecha_Nacimiento")
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

            builder.Property(x => x.EstadoCivilId)
                   .HasColumnName("EstadoCivil_Id")
                   .IsRequired();

            builder.Property(x => x.CiudadId)
                   .HasColumnName("Ciudad_Id")
                   .IsRequired();

            builder.Property(x => x.CargoId)
                   .HasColumnName("Cargo_Id")
                   .IsRequired();

            builder.Property(x => x.UsuarioCreacionId)
                   .HasColumnName("Usuario_Creacion");

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
            builder.HasIndex(x => x.DNI)
                   .IsUnique()
                   .HasDatabaseName("UQ_Colaboradores_DNI");

            builder.HasIndex(x => x.CorreoElectronico)
                   .IsUnique()
                   .HasDatabaseName("UQ_Colaboradores_CorreoElectronico");
            #endregion


            #region relaciones uno a uno o uno a muchos
            builder.HasOne(x => x.EstadoCivil)
                   .WithMany(e => e.Colaboradores)
                   .HasForeignKey(x => x.EstadoCivilId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Ciudad)
                   .WithMany(c => c.Colaboradores)
                   .HasForeignKey(x => x.CiudadId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Cargo)
                   .WithMany(c => c.Colaboradores)
                   .HasForeignKey(x => x.CargoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioCreacion)
                   .WithMany(u => u.ColaboradoresCreados)
                   .HasForeignKey(x => x.UsuarioCreacionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UsuarioModificacion)
                   .WithMany(u => u.ColaboradoresModificados)
                   .HasForeignKey(x => x.UsuarioModificacionId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region relaciones uno a muchos
            builder.HasMany(x => x.Usuarios)
                   .WithOne(u => u.Colaborador)
                   .HasForeignKey(u => u.ColaboradorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ColaboradoresPorSucursal)
                   .WithOne(cs => cs.Colaborador)
                   .HasForeignKey(cs => cs.ColaboradorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ViajesDetalle)
                   .WithOne(vd => vd.Colaborador)
                   .HasForeignKey(vd => vd.ColaboradorId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
