using Academia.SIMOVIA.WebAPI._Features.General.DomainRequirements;
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
        public Response<Colaboradores> ValidarColaboradorParaRegistro(
        Colaboradores colaborador,
        RegistroColaboradorDomainRequirement domainRequirement)
        {
            Response<Colaboradores> validacionDatos = ValidarRegistrarDatosColaborador(colaborador);
            if (!validacionDatos.Exitoso)
                return validacionDatos;

            Response<Colaboradores> validacionLongitudes = ValidarLongitudesCampos(colaborador);
            if (!validacionLongitudes.Exitoso)
                return validacionLongitudes;

            Response<Colaboradores> datosIngresadosValidos = ValidarDatosIngresadosColaborador(colaborador);
            if (!datosIngresadosValidos.Exitoso)
                return datosIngresadosValidos;

            Response<Colaboradores> sucursalesAsignadasValidas = ValidarSucursalesAsignadas(colaborador);
            if (!sucursalesAsignadasValidas.Exitoso)
                return sucursalesAsignadasValidas;

            if (!domainRequirement.EsValido())
            {
                return new Response<Colaboradores>
                {
                    Exitoso = false,
                    Mensaje = string.Join(" ", domainRequirement.ObtenerErrores())
                };
            }

            return new Response<Colaboradores> { Exitoso = true };
        }

        public Response<Colaboradores> ValidarRegistrarDatosColaborador(Colaboradores colaboradorDto)
        {
            List<string> camposFaltantes = new List<string>();

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
            if (colaboradorDto.ColaboradoresPorSucursal == null || !colaboradorDto.ColaboradoresPorSucursal.Any()) camposFaltantes.Add("Asignar sucursales");

            if (camposFaltantes.Any())
            {
                string mensaje = camposFaltantes.Count == 1 ? Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", camposFaltantes.First()) 
                    : Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", string.Join(", ", camposFaltantes)); 

                return new Response<Colaboradores> { Exitoso = false, Mensaje = mensaje };
            }

            return new Response<Colaboradores> { Exitoso = true };
        }

        private Response<Colaboradores> ValidarLongitudesCampos(Colaboradores colaborador)
        {
            var errores = new List<string>();

            if (colaborador.DNI.Length > 13) errores.Add("DNI");
            if (colaborador.Nombres.Length > 50) errores.Add("Nombres");
            if (colaborador.Apellidos.Length > 50) errores.Add("Apellidos");
            if (colaborador.CorreoElectronico.Length > 60) errores.Add("Correo Electrónico");
            if (colaborador.Telefono.Length > 8) errores.Add("Teléfono");
            if (colaborador.DireccionExacta.Length > 100) errores.Add("Dirección Exacta");

            if (errores.Any())
            {
                string mensaje = errores.Count == 1 ? Mensajes.LONGITUD_INVALIDA.Replace("@campo", errores.First()) :
                    Mensajes.LONGITUDES_INVALIDAS.Replace("@campos", string.Join(", ", errores));

                return new Response<Colaboradores> { Exitoso = false, Mensaje = mensaje };
            }

            return new Response<Colaboradores> { Exitoso = true };
        }


        public Response<Colaboradores> ValidarDatosIngresadosColaborador(Colaboradores colaboradorDto)
        {
            DateTime fechaActual = DateTime.Today;
            DateTime fechaMinima = fechaActual.AddYears(-90);
            DateTime fechaMaxima = fechaActual;

            if (colaboradorDto.FechaNacimiento < fechaMinima || colaboradorDto.FechaNacimiento > fechaMaxima)
            {
                return new Response<Colaboradores>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.INGRESAR_VALIDO.Replace("@campo", "Fecha de Nacimiento")
                };
            }

            if (colaboradorDto.Sexo.ToUpper() != "M" && colaboradorDto.Sexo.ToUpper() != "F")
            {
                return new Response<Colaboradores> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDO.Replace("@campo", "sexo") };
            }

            return new Response<Colaboradores> { Exitoso = true };
        }

        public Response<Colaboradores> ValidarSucursalesAsignadas(Colaboradores colaboradorDto)
        {
            var sucursalesDuplicadas = colaboradorDto.ColaboradoresPorSucursal
                .GroupBy(cs => cs.SucursalId)
                .Where(g => g.Count() > 1)
                .ToList();

            if (sucursalesDuplicadas.Any())
            {
                return new Response<Colaboradores>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ASIGNAR_VARIOS.Replace("@articulo", "la").Replace("@entidad", "sucursal")
                };
            }

            var distanciaInvalida = colaboradorDto.ColaboradoresPorSucursal.Where(s => s.DistanciaKm <= 0 || s.DistanciaKm > 50)
                .Select(s => s.DistanciaKm.ToString(CultureInfo.InvariantCulture)).ToList();

            if (distanciaInvalida.Any())
            {
                return new Response<Colaboradores>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.DISTANCIA_INVALIDA.Replace("@distanciakm", string.Join(", ", distanciaInvalida))
                };
            }
            return new Response<Colaboradores> { Exitoso = true };
        }
    }
}
