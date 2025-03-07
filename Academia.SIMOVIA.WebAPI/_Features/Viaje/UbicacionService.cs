using Academia.SIMOVIA.WebAPI._Features.Viaje.Dtos;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Newtonsoft.Json;
using System.Globalization;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje
{
    public class UbicacionService : IUbicacionService
    {
        private readonly string _urlDistanceMatrixApi;
        private readonly string _urlDirectionsApi;
        private readonly string _apiKey;
        private readonly IConfiguration _configuration;

        public UbicacionService(IConfiguration configuration)
        {
            _configuration = configuration;
            _urlDistanceMatrixApi = _configuration["DISTANCE_MATRIX_API_URL"];
            _urlDirectionsApi = _configuration["DIRECTIONS_API_URL"];
            _apiKey = _configuration["API_KEY"];
        }
        public async Task<DistanceMatrixApiResponseDto> ObtenerDistanciasSucursales(decimal latitud, decimal longitud, List<Sucursales> sucursales)
        {
            try
            {
                string origins = $"{latitud.ToString(CultureInfo.InvariantCulture)},{longitud.ToString(CultureInfo.InvariantCulture)}";
                string destinations = string.Join("|", sucursales.Select(s =>
                    $"{s.Latitud.ToString(CultureInfo.InvariantCulture)},{s.Longitud.ToString(CultureInfo.InvariantCulture)}"));

                string url = string.Format(_urlDistanceMatrixApi, origins, destinations, _apiKey);

                using HttpClient client = new HttpClient();
                string response = await client.GetStringAsync(url);

                return JsonConvert.DeserializeObject<DistanceMatrixApiResponseDto>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<decimal> CalcularDistanciaViaje(decimal latOrigen, decimal lonOrigen, List<(decimal Latitud, decimal Longitud)> colaboradores)
        {
            string origin = $"{latOrigen.ToString(CultureInfo.InvariantCulture)},{lonOrigen.ToString(CultureInfo.InvariantCulture)}";
            string waypoints = string.Join("|", colaboradores.Select(c => $"{c.Latitud.ToString(CultureInfo.InvariantCulture)},{c.Longitud.ToString(CultureInfo.InvariantCulture)}"));
            string destination = $"{colaboradores.Last().Latitud.ToString(CultureInfo.InvariantCulture)},{colaboradores.Last().Longitud.ToString(CultureInfo.InvariantCulture)}";
            string url = string.Format(_urlDirectionsApi, origin, destination, waypoints, _apiKey);

            using HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(url);
            RutaGoogleDto googleResponse = JsonConvert.DeserializeObject<RutaGoogleDto>(response);

            if (googleResponse.routes == null || !googleResponse.routes.Any())
                return -1;

            decimal distanciaKm = Convert.ToDecimal(googleResponse.routes[0].legs.Sum(l => l.distance.value) / 1000.0);
            return distanciaKm;
        }
    }
}
