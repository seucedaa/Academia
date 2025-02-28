using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Academia.SIMOVIA.WebAPI._Features.General
{
    public class GeneralDomainService
    {
        public async Task<Response<int>> ValidarRegistrarDatosColaborador(ColaboradorDto colaboradorDto)
        {
            var camposFaltantes = new List<string>();

            if (string.IsNullOrEmpty(colaboradorDto.DNI)) camposFaltantes.Add("DNI");
            if (string.IsNullOrEmpty(colaboradorDto.Nombres)) camposFaltantes.Add("Nombres");
            if (string.IsNullOrEmpty(colaboradorDto.Apellidos)) camposFaltantes.Add("Apellidos");
            if (string.IsNullOrEmpty(colaboradorDto.CorreoElectronico)) camposFaltantes.Add("Correo Electrónico");
            if (string.IsNullOrEmpty(colaboradorDto.Telefono)) camposFaltantes.Add("Teléfono");
            if (string.IsNullOrEmpty(colaboradorDto.Sexo)) camposFaltantes.Add("Sexo");
            if (colaboradorDto.FechaNacimiento == default) camposFaltantes.Add("Fecha de Nacimiento");
            if (string.IsNullOrEmpty(colaboradorDto.DireccionExacta)) camposFaltantes.Add("Dirección Exacta");
            if (colaboradorDto.Latitud == 0) camposFaltantes.Add("Latitud");
            if (colaboradorDto.Longitud == 0) camposFaltantes.Add("Longitud");
            if (colaboradorDto.EstadoCivilId <= 0) camposFaltantes.Add("Estado Civil");
            if (colaboradorDto.CargoId <= 0) camposFaltantes.Add("Cargo");
            if (colaboradorDto.CiudadId <= 0) camposFaltantes.Add("Ciudad");
            if (colaboradorDto.UsuarioCreacionId <= 0) camposFaltantes.Add("Usuario Creación");
            if (colaboradorDto.Sucursales == null || !colaboradorDto.Sucursales.Any()) camposFaltantes.Add("Sucursales");

            if (camposFaltantes.Any())
            {
                string mensaje = camposFaltantes.Count == 1
                    ? Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", camposFaltantes.First()) 
                    : Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", string.Join(", ", camposFaltantes)); 

                return new Response<int> { Exitoso = false, Mensaje = mensaje };
            }

            var fechaActual = DateTime.Today;
            var fechaMinima = fechaActual.AddYears(-90); 
            var fechaMaxima = fechaActual;

            if (colaboradorDto.FechaNacimiento < fechaMinima || colaboradorDto.FechaNacimiento > fechaMaxima)
            {
                return new Response<int>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.INGRESAR_VALIDO.Replace("@campo", "Fecha de Nacimiento")
                };
            }


            if (colaboradorDto.Sexo.ToUpper() != "M" && colaboradorDto.Sexo.ToUpper() != "F")
            {
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDO.Replace("@campo", "sexo") };
            }

            var sucursalesDuplicadas = colaboradorDto.Sucursales
                .GroupBy(cs => cs.SucursalId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (sucursalesDuplicadas.Any())
            {
                return new Response<int>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ASIGNAR_VARIOS.Replace("@articulo", "la").Replace("@entidad", "sucursal")
                };
            }

            var distanciaInvalida = colaboradorDto.Sucursales
                .Where(s => s.DistanciaKm <= 0 || s.DistanciaKm > 50)
                .Select(s => s.DistanciaKm.ToString(CultureInfo.InvariantCulture)) 
                .ToList();

            if (distanciaInvalida.Any())
            {
                return new Response<int>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.DISTANCIA_INVALIDA.Replace("@distanciakm", string.Join(", ", distanciaInvalida))
                };
            }

            return new Response<int> { Exitoso = true };
        }

    }
}
