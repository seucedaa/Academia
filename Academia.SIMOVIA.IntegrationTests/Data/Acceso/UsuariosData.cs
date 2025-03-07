using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.IntegrationTests.Data.Acceso
{
    public class UsuariosData
    {
        public static Usuarios UsuarioPrueba
        {
            get
            {
                byte[] claveCifrada;
                using (SHA512 sha512 = SHA512.Create())
                    claveCifrada = sha512.ComputeHash(Encoding.UTF8.GetBytes("sua"));

                return new Usuarios
                {
                    UsuarioId = 1,
                    Usuario = "sua",
                    Clave = claveCifrada,
                    EsAdministrador = false,
                    ColaboradorId = 1,
                    RolId = 1,
                    UsuarioCreacionId = 1,
                    FechaCreacion = DateTime.Now
                };
            }
        }
    }
}
