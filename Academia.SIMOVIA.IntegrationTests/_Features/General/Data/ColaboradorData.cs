using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.IntegrationTests._Features.General.Data
{
    public static class ColaboradorData
    {
        public static Roles RolAdministrador => new()
        {
            RolId = 1,
            Descripcion = "Administrador",
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };

        public static Cargos CargoGerente => new()
        {
            CargoId = 1,
            Descripcion = "Gerente",
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };

        public static Colaboradores Colaborador => new()
        {
            ColaboradorId = 1,
            DNI = "1234567890123",
            Nombres = "Juan",
            Apellidos = "Pérez",
            CorreoElectronico = "juan.perez@email.com",
            Telefono = "12345678",
            Sexo = "M",
            FechaNacimiento = DateTime.Now,
            DireccionExacta = "Los Angeles",
            Latitud = 15.25m,
            Longitud = -88.235m,
            EstadoCivilId = 1,
            CargoId = 1,
            CiudadId = 1,
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };

        public static Usuarios ObtenerUsuario(string usuario)
        {
            byte[] claveCifrada;
            using (SHA512 sha512 = SHA512.Create())
                claveCifrada = sha512.ComputeHash(Encoding.UTF8.GetBytes("sua"));

            return new Usuarios
            {
                UsuarioId = 1,
                Usuario = usuario,
                Clave = claveCifrada,
                EsAdministrador = false,
                ColaboradorId = 1,
                RolId = 1,
                UsuarioCreacionId = 1,
                FechaCreacion = DateTime.Now
            };
        }

        public static Pantallas PantallaDashboard => new()
        {
            PantallaId = 1,
            Descripcion = "Dashboard",
            DireccionURL = "/dashboard",
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.Now
        };

        public static PantallasPorRoles PermisoPantalla => new()
        {
            RolId = 1,
            PantallaId = 1
        };
    }
}
