using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI._Features.Viaje.Enums;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje
{
    public class ViajeDomainService
    {
        private readonly UbicacionService _ubicacionService;
        public ViajeDomainService(UbicacionService ubicacionService)
        {
            _ubicacionService = ubicacionService;
        }
        public async Task<Response<int>> ValidarRegistrarDatosSucursal(SucursalDto sucursalDto)
        {
            var camposFaltantes = new List<string>();

            if (string.IsNullOrEmpty(sucursalDto.Descripcion)) camposFaltantes.Add("Descripción");
            if (string.IsNullOrEmpty(sucursalDto.Telefono)) camposFaltantes.Add("Teléfono");
            if (string.IsNullOrEmpty(sucursalDto.DireccionExacta)) camposFaltantes.Add("Dirección Exacta");
            if (sucursalDto.Latitud == 0) camposFaltantes.Add("Latitud");
            if (sucursalDto.Longitud == 0) camposFaltantes.Add("Longitud");
            if (sucursalDto.CiudadId <= 0) camposFaltantes.Add("Ciudad");
            if (sucursalDto.UsuarioCreacionId <= 0) camposFaltantes.Add("Usuario Creación");

            if (camposFaltantes.Any())
            {
                string mensaje = camposFaltantes.Count == 1
                    ? Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", camposFaltantes.First())
                    : Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", string.Join(", ", camposFaltantes));

                return new Response<int> { Exitoso = false, Mensaje = mensaje };
            }

            if (!sucursalDto.Telefono.All(char.IsDigit))
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDO.Replace("@campo", "Teléfono") };

            decimal latitud = sucursalDto.Latitud;
            decimal longitud = sucursalDto.Longitud;

            if (latitud == 0 && longitud == 0)
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDOS };

            if (latitud < -90 || latitud > 90)
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDA.Replace("@campo", "latitud") };

            if (longitud < -180 || longitud > 180)
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDA.Replace("@campo", "longitud") };

            return new Response<int> { Exitoso = true };
        }


        public async Task<Response<int>> ValidarRegistrarDatosViaje(ViajeDto viajeDto)
        {
            var camposFaltantes = new List<string>();

            if (viajeDto.FechaHora == default) camposFaltantes.Add("Fecha y Hora");
            if (viajeDto.SucursalId <= 0) camposFaltantes.Add("Sucursal");
            if (viajeDto.TransportistaId <= 0) camposFaltantes.Add("Transportista");
            if (viajeDto.UsuarioCreacionId <= 0) camposFaltantes.Add("Usuario Creación");
            if (viajeDto.Colaboradores == null || !viajeDto.Colaboradores.Any()) camposFaltantes.Add("Colaboradores");

            if (camposFaltantes.Any())
            {
                string mensaje = camposFaltantes.Count == 1
                    ? Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", camposFaltantes.First())  
                    : Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", string.Join(", ", camposFaltantes)); 

                return new Response<int> { Exitoso = false, Mensaje = mensaje };
            }

            var fechaActual = DateTime.Today;
            var fechaMinima = fechaActual.AddYears(-1);
            var fechaMaxima = fechaActual.AddMonths(1);

            if (viajeDto.FechaHora < fechaMinima || viajeDto.FechaHora > fechaMaxima)
            {
                return new Response<int>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.INGRESAR_VALIDO.Replace("@campo", "Fecha y Hora de Viaje")
                };
            }

            var colaboradoresDuplicados = viajeDto.Colaboradores
                .GroupBy(c => c.ColaboradorId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (colaboradoresDuplicados.Any())
            {
                string mensaje = colaboradoresDuplicados.Count == 1
                    ? Mensajes.ASIGNAR_VARIOS.Replace("@articulo", "el").Replace("@entidad", $"Colaborador ID {colaboradoresDuplicados.First()}")
                    : Mensajes.CAMPOS_DUPLICADOS.Replace("@Campos", $"Colaboradores ID {string.Join(", ", colaboradoresDuplicados)}");

                return new Response<int> { Exitoso = false, Mensaje = mensaje };
            }

            return new Response<int> { Exitoso = true };
        }



        public Response<int> ValidarDistancia(decimal distanciaCantidad, decimal distanciaTotalKm)
        {
            if (distanciaCantidad < 0)
            {
                return new Response<int>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.ERROR_DISTANCIA
                };
            }

            if (distanciaTotalKm + distanciaCantidad > 100)
            {
                return new Response<int>
                {
                    Exitoso = false,
                    Mensaje = Mensajes.DISTANCIA_EXCEDIDA.Replace("@distanciakm", (distanciaTotalKm + distanciaCantidad).ToString())
                };
            }

            return new Response<int> { Exitoso = true };
        }

        public async Task<Response<int>> ValidarRegistrarDatosSolicitud(SolicitudDto solicitudDto)
        {
            var camposFaltantes = new List<string>();

            if (string.IsNullOrEmpty(solicitudDto.Descripcion)) camposFaltantes.Add("Descripción");
            if (solicitudDto.UsuarioId <= 0) camposFaltantes.Add("Usuario");
            if (solicitudDto.FechaViaje == default) camposFaltantes.Add("Fecha del viaje");

            if (camposFaltantes.Any())
            {
                string mensaje = camposFaltantes.Count == 1
                    ? Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", camposFaltantes.First()) 
                    : Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", string.Join(", ", camposFaltantes)); 

                return new Response<int> { Exitoso = false, Mensaje = mensaje };
            }

            return new Response<int> { Exitoso = true };
        }


        public Response<int> ValidarDatosSolicitud(SolicitudDto solicitudDto)
        {
            var camposFaltantes = new List<string>();

            if (string.IsNullOrEmpty(solicitudDto.Descripcion))
                camposFaltantes.Add("Descripción");

            if (solicitudDto.UsuarioId <= 0)
                camposFaltantes.Add("Usuario");

            if (solicitudDto.FechaViaje == default)
                camposFaltantes.Add("Fecha de Viaje");

            if (camposFaltantes.Any())
            {
                string mensaje = camposFaltantes.Count == 1
                    ? Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", camposFaltantes.First()) 
                    : Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", string.Join(", ", camposFaltantes));

                return new Response<int> { Exitoso = false, Mensaje = mensaje };
            }

            return new Response<int> { Exitoso = true };
        }


        public async Task<Response<int>> ValidarDistanciaEnViaje(ViajesEncabezado viaje, Sucursales sucursal, List<ColaboradorUbicacionDto> viajeDetalles, int usuarioId)
        {
            if (viaje == null)
                return new Response<int> { Exitoso = false, Mensaje = "El viaje no existe." };

            if (sucursal == null)
                return new Response<int> { Exitoso = false, Mensaje = "La sucursal del viaje no existe." };

            var colaboradores = viajeDetalles
                .Select(d => (d.Latitud, d.Longitud))
                .ToList();

            decimal nuevaDistancia = await _ubicacionService.CalcularDistanciaViaje(
                sucursal.Latitud,
                sucursal.Longitud,
                colaboradores.Select(c => (c.Latitud, c.Longitud)).ToList()
            );

            return nuevaDistancia > 100
                ? new Response<int> { Exitoso = false, Mensaje = "El viaje supera la distancia máxima permitida." }
                : new Response<int> { Exitoso = true };
        }

        public async Task<Response<int>> ValidarDatosCancelarSolicitud(CancelarSolicitudViajeDto solicitudDto)
        {
            var camposFaltantes = new List<string>();

            if (string.IsNullOrEmpty(solicitudDto.Descripcion))
                camposFaltantes.Add("Descripción");

            if (solicitudDto.UsuarioId <= 0)
                camposFaltantes.Add("Usuario");

            if (solicitudDto.ViajeEncabezadoId == null)
                camposFaltantes.Add("Viaje");

            if (camposFaltantes.Any())
            {
                string mensaje = camposFaltantes.Count == 1
                    ? Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", camposFaltantes.First()) 
                    : Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", string.Join(", ", camposFaltantes)); 

                return new Response<int> { Exitoso = false, Mensaje = mensaje };
            }

            return new Response<int> { Exitoso = true };
        }
        
        public async Task<Response<List<SucursalesDto>>> ValidarUbicacion(decimal latitud, decimal longitud)
        {
            if (latitud == 0 && longitud == 0)
                return new Response<List<SucursalesDto>> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDOS };

            if (latitud < -90 || latitud > 90)
                return new Response<List<SucursalesDto>> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDA.Replace("@campo", "latitud") };

            if (longitud < -180 || longitud > 180)
                return new Response<List<SucursalesDto>> { Exitoso = false, Mensaje = Mensajes.INGRESAR_VALIDA.Replace("@campo", "longitud") };

            return new Response<List<SucursalesDto>> { Exitoso = true };
        }


    }
}
