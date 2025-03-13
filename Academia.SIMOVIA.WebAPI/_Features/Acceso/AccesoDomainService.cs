using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace Academia.SIMOVIA.WebAPI._Features.Acceso
{
    public class AccesoDomainService
    {
        public Response<string> ValidarInicioSesion(InicioSesionDto login, Usuarios usuario)
        {
            Response<string> validacionLongitudes = ValidarLongitudesCampos(login);
            if (!validacionLongitudes.Exitoso)
                return validacionLongitudes;

            if (string.IsNullOrEmpty(login.Usuario) || string.IsNullOrEmpty(login.Clave))
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.CREDENCIALES_OBLIGATORIAS };
            if (usuario == null)
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.CREDENCIALES_INCORRECTAS };
            if (usuario.UsuarioId == 0) 
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.CREDENCIALES_INCORRECTAS };

            byte[] claveCifrada;
            using (SHA512 sha512 = SHA512.Create())
                claveCifrada = sha512.ComputeHash(Encoding.UTF8.GetBytes(login.Clave));

            if (!usuario.Clave.SequenceEqual(claveCifrada))
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.CREDENCIALES_INCORRECTAS };

            if (!usuario.Estado)
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.DESACTIVADO.Replace("@Entidad", "Usuario") };

            return new Response<string> { Exitoso = true };
        }

        private Response<string> ValidarLongitudesCampos(InicioSesionDto login)
        {
            var errores = new List<string>();

            if (login.Usuario.Length > 80)
                errores.Add(Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Usuario").Replace("@longitud", "80"));

            if (login.Clave.Length > 64)
                errores.Add(Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Clave").Replace("@longitud", "64"));

            if (errores.Any())
            {
                return new Response<string> { Exitoso = false, Mensaje = string.Join(" ", errores) };
            }

            return new Response<string> { Exitoso = true };
        }


    }
}
