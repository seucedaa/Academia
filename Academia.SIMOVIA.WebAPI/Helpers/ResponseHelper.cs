namespace Academia.SIMOVIA.WebAPI.Helpers
{
    public static class ResponseHelper
    {
        public static Response<T> RespuestaExito<T>(T data, string mensaje = "")
        {
            return new Response<T>
            {
                Exitoso = true,
                Mensaje = mensaje,
                Data = data
            };
        }

        public static Response<T> RespuestaError<T>(string mensaje)
        {
            return new Response<T>
            {
                Exitoso = false,
                Mensaje = mensaje,
                Data = default
            };
        }
    }
}
