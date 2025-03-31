using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using System.Security.Cryptography;
using System.Text;

namespace Academia.SIMOVIA.IntegrationTests.Data.Acceso
{
    public class UsuariosData
    {
        public static Usuarios UsuarioPrueba => new Usuarios
        {
            UsuarioId = 1,
            Usuario = "sua",
            Clave = GenerarClaveHash("sua"),  // Generar la clave de manera segura
            EsAdministrador = false,
            ColaboradorId = 1,
            RolId = 1,
            UsuarioCreacionId = 1,
            FechaCreacion = DateTime.UtcNow
        };

        private static byte[] GenerarClaveHash(string clave)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(Encoding.UTF8.GetBytes(clave));
            }
        }


    }
}
