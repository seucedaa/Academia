using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Academia.SIMOVIA.WebAPI._Features.Acceso
{
    public class AccesoDomainService
    {
        private readonly UnitOfWorkBuilder _unitOfWorkBuilder;
        public AccesoDomainService(UnitOfWorkBuilder unitOfWorkBuilder)
        {
            _unitOfWorkBuilder = unitOfWorkBuilder;
        }
        public async Task<Response<string>> ValidarInicioSesion(InicioSesionDto login)
        {

            if (string.IsNullOrEmpty(login.Usuario) || string.IsNullOrEmpty(login.Clave))
            {
                return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ08 };
            }

            //if (usuario == null)
            //{
            //    return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ01_Credenciales_Incorrectas };
            //}

            byte[] claveCifrada;
            using (SHA512 sha512 = SHA512.Create())
            {
                claveCifrada = sha512.ComputeHash(Encoding.UTF8.GetBytes(login.Clave));
            }

            //if (!usuario.Clave.SequenceEqual(claveCifrada))
            //{
            //    return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ01_Credenciales_Incorrectas };
            //}

            //if (!usuario.Estado)
            //{
            //    return new Response<string> { Exitoso = false, Mensaje = Mensajes.MSJ03.Replace("@Entidad", "Usuario") };
            //}

            return new Response<string> { Exitoso = true };
        }
    }
}
