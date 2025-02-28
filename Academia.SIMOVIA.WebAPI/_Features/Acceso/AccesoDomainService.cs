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
            if (string.IsNullOrEmpty(login.Usuario) || string.IsNullOrEmpty(login.Clave))
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.CREDENCIALES_OBLIGATORIAS };

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

    }
}
