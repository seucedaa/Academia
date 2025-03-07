using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Utilities;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace Academia.SIMOVIA.UnitTests.DataTests.Acceso
{
    public class InicioSesionTest : TheoryData<InicioSesionDto, Usuarios, bool, string>
    {
        private readonly byte[] _claveCifrada;

        public InicioSesionTest()
        {
            _claveCifrada = HashClave("sua2");

            Add(UsuarioConLongitudExcedida(), UsuarioNoEncontrado(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Usuario").Replace("@longitud", "80"));
            Add(ClaveConLongitudExcedida(), UsuarioNoEncontrado(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Clave").Replace("@longitud", "64"));
            Add(CredencialesVacias(), UsuarioNoEncontrado(), false, Mensajes.CREDENCIALES_OBLIGATORIAS);
            Add(Credenciales(), UsuarioNoEncontrado(), false, Mensajes.CREDENCIALES_INCORRECTAS);
            Add(Credenciales(), ValidarCredenciales(), false, Mensajes.CREDENCIALES_INCORRECTAS);
            Add(CredencialesCorrectas(), UsuarioCorrectoDesactivado(), false, Mensajes.DESACTIVADO.Replace("@Entidad", "Usuario"));

            Add(CredencialesCorrectas(), ValidarCredenciales(), true, null);
        }

        private byte[] HashClave(string clave)
        {
            using (SHA512 sha512 = SHA512.Create())
                return sha512.ComputeHash(Encoding.UTF8.GetBytes(clave));
        }

        public InicioSesionDto UsuarioConLongitudExcedida() => new InicioSesionDto
        {
            Usuario = new string('A', 81),
            Clave = "clave123"
        };

        public InicioSesionDto ClaveConLongitudExcedida() => new InicioSesionDto
        {
            Usuario = "usuario",
            Clave = new string('B', 65)
        };
        public InicioSesionDto CredencialesVacias() => new InicioSesionDto
        {
            Usuario = "",
            Clave = ""
        };

        public InicioSesionDto Credenciales() => new InicioSesionDto
        {
            Usuario = "usuario",
            Clave = "clave"
        };

        public InicioSesionDto CredencialesCorrectas() => new InicioSesionDto
        {
            Usuario = "sua",
            Clave = "sua2"
        };
            
        public Usuarios UsuarioNoEncontrado() => new Usuarios
        {
            UsuarioId = 0
        };

        public Usuarios ValidarCredenciales() => new Usuarios
        {
            UsuarioId = 1,
            Clave = _claveCifrada
        };

        public Usuarios UsuarioCorrectoDesactivado() => new Usuarios
        {
            UsuarioId = 2,
            Clave = _claveCifrada,
            Estado = false
        };
    }
}
