namespace Academia.SIMOVIA.WebAPI.Helpers
{
    public class Response<T>
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }
        public T Data { get; set; }
    }
}
