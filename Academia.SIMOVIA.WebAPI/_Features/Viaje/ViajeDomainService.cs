using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements;
using Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements.Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Enums;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje
{
    public class ViajeDomainService
    {

        public Response<Sucursales> ValidarSucursalParaRegistro(Sucursales sucursal, RegistroSucursalDomainRequirement domainRequirement)
        {

            Response<Sucursales> validacionDatos = ValidarRegistrarDatosSucursal(sucursal);
            if (!validacionDatos.Exitoso)
                return validacionDatos;

            Response<Sucursales> validacionLongitudes = ValidarLongitudesCamposSucursal(sucursal);
            if (!validacionLongitudes.Exitoso)
                return validacionLongitudes;

            var validacionUbicacion = ValidarUbicacionInterna(sucursal.Latitud, sucursal.Longitud);
            if (!validacionUbicacion.Exitoso)
                return new Response<Sucursales> { Exitoso = false, Mensaje = validacionUbicacion.Mensaje };

            if (!domainRequirement.EsValido())
            {
                return new Response<Sucursales>
                {
                    Exitoso = false,
                    Mensaje = string.Join(" ", domainRequirement.ObtenerErrores())
                };
            }


            return new Response<Sucursales> { Exitoso = true };
        }

        public Response<Sucursales> ValidarRegistrarDatosSucursal(Sucursales sucursal)
        {
            var camposFaltantes = new List<string>();

            if (string.IsNullOrEmpty(sucursal.Descripcion)) camposFaltantes.Add("Descripción");
            if (string.IsNullOrEmpty(sucursal.Telefono)) camposFaltantes.Add("Teléfono");
            if (string.IsNullOrEmpty(sucursal.DireccionExacta)) camposFaltantes.Add("Dirección Exacta");
            if (sucursal.Latitud == 0) camposFaltantes.Add("Latitud");
            if (sucursal.Longitud == 0) camposFaltantes.Add("Longitud");
            if (sucursal.CiudadId <= 0) camposFaltantes.Add("Ciudad");
            if (sucursal.UsuarioCreacionId <= 0) camposFaltantes.Add("Usuario Creación");

            if (camposFaltantes.Any())
            {
                string mensaje = camposFaltantes.Count == 1 ? Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", camposFaltantes.First())
                    : Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", string.Join(", ", camposFaltantes));

                return new Response<Sucursales> { Exitoso = false, Mensaje = mensaje };
            }

            if (!sucursal.Telefono.All(char.IsDigit))
                return new Response<Sucursales> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDO.Replace("@campo", "Teléfono") };

            return new Response<Sucursales> { Exitoso = true };
        }

        private Response<Sucursales> ValidarLongitudesCamposSucursal(Sucursales sucursal)
        {
            var errores = new List<string>();

            if (sucursal.Descripcion.Length > 50) errores.Add("Descripción");
            if (sucursal.Telefono.Length > 8) errores.Add("Teléfono");
            if (sucursal.DireccionExacta.Length > 100) errores.Add("Dirección Exacta");

            if (errores.Any())
            {
                string mensaje = errores.Count == 1 ? Mensajes.LONGITUD_INVALIDA.Replace("@campo", errores.First()) :
                    Mensajes.LONGITUDES_INVALIDAS.Replace("@campos", string.Join(", ", errores));

                return new Response<Sucursales> { Exitoso = false, Mensaje = mensaje };
            }

            return new Response<Sucursales> { Exitoso = true };
        }

        public Response<ViajesEncabezado> ValidarViajeParaRegistro(ViajesEncabezado viaje,RegistroViajeDomainRequirement domainRequirement)
        {
            Response<ViajesEncabezado> validacionDatos = ValidarCamposObligatorios(viaje);
            if (!validacionDatos.Exitoso)
                return validacionDatos;

            Response<ViajesEncabezado> validacionDatosIngresados = ValidarDatosIngresados(viaje);
            if (!validacionDatosIngresados.Exitoso)
                return validacionDatosIngresados;

            Response<ViajesEncabezado> validacionColaboradores = ValidarColaboradoresAsignados(viaje);
            if (!validacionColaboradores.Exitoso)
                return validacionColaboradores;

            if (!domainRequirement.EsValido())
            {
                return new Response<ViajesEncabezado>
                {
                    Exitoso = false,
                    Mensaje = string.Join(" ", domainRequirement.ObtenerErrores())
                };
            }

            return new Response<ViajesEncabezado> { Exitoso = true };
        }

        private Response<ViajesEncabezado> ValidarCamposObligatorios(ViajesEncabezado viaje)
        {
            var camposFaltantes = new List<string>();

            if (viaje.FechaHora == default) camposFaltantes.Add("Fecha y Hora");
            if (viaje.SucursalId <= 0) camposFaltantes.Add("Sucursal");
            if (viaje.TransportistaId <= 0) camposFaltantes.Add("Transportista");
            if (viaje.UsuarioCreacionId <= 0) camposFaltantes.Add("Usuario Creación");
            if (viaje.ViajesDetalle == null || !viaje.ViajesDetalle.Any()) camposFaltantes.Add("Asignar colaboradores");

            if (camposFaltantes.Any())
            {
                string mensaje = camposFaltantes.Count == 1
                    ? Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", camposFaltantes.First())
                    : Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", string.Join(", ", camposFaltantes));

                return new Response<ViajesEncabezado> { Exitoso = false, Mensaje = mensaje };
            }

            return new Response<ViajesEncabezado> { Exitoso = true };
        }

        private Response<ViajesEncabezado> ValidarDatosIngresados(ViajesEncabezado viaje)
        {
            var fechaActual = DateTime.Today;
            var fechaMinima = fechaActual.AddYears(-1);
            var fechaMaxima = fechaActual.AddMonths(1);

            if (viaje.FechaHora < fechaMinima || viaje.FechaHora > fechaMaxima)
            {
                return new Response<ViajesEncabezado>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.INGRESAR_VALIDO.Replace("@campo", "Fecha y Hora de Viaje")
                };
            }

            return new Response<ViajesEncabezado> { Exitoso = true };
        }

        private Response<ViajesEncabezado> ValidarColaboradoresAsignados(ViajesEncabezado viaje)
        {
            var colaboradoresDuplicados = viaje.ViajesDetalle
                .GroupBy(c => c.ColaboradorId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (colaboradoresDuplicados.Any())
            {
                string mensaje = colaboradoresDuplicados.Count == 1
                    ? Mensajes.ASIGNAR_VARIOS.Replace("@articulo", "el").Replace("@entidad", $"Colaborador ID {colaboradoresDuplicados.First()}")
                    : Mensajes.ASIGNAR_VARIOS.Replace("@articulo", "los").Replace("@entidad", $"Colaboradores ID {string.Join(", ", colaboradoresDuplicados)}");

                return new Response<ViajesEncabezado> { Exitoso = false, Mensaje = mensaje };
            }

            return new Response<ViajesEncabezado> { Exitoso = true };
        }


        public Response<ViajesEncabezado> ValidarDistancia(decimal distanciaCantidad, decimal distanciaTotalKm)
        {
            if (distanciaCantidad < 0)
            {
                return new Response<ViajesEncabezado>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_DISTANCIA
                };
            }

            if (distanciaTotalKm + distanciaCantidad > 100)
            {
                return new Response<ViajesEncabezado>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.DISTANCIA_EXCEDIDA.Replace("@distanciakm", (distanciaTotalKm + distanciaCantidad).ToString())
                };
            }

            return new Response<ViajesEncabezado> { Exitoso = true };
        }
        public Response<List<SucursalesDto>> ValidarUbicacion(decimal latitud, decimal longitud)
        {
            var validacionUbicacion = ValidarUbicacionInterna(latitud, longitud);
            if (!validacionUbicacion.Exitoso)
                return new Response<List<SucursalesDto>> { Exitoso = false, Mensaje = validacionUbicacion.Mensaje };

            return new Response<List<SucursalesDto>> { Exitoso = true };
        }

        private Response<object> ValidarUbicacionInterna(decimal latitud, decimal longitud)
        {
            if (latitud < -90 || latitud > 90)
                return new Response<object> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDA.Replace("@campo", "latitud") };

            if (longitud < -180 || longitud > 180)
                return new Response<object> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDA.Replace("@campo", "longitud") };

            return new Response<object> { Exitoso = true };
        }
    }
}
