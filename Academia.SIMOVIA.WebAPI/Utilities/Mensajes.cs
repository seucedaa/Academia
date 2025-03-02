namespace Academia.SIMOVIA.WebAPI.Utilities
{
    public static class Mensajes
    {
        #region Descriptivos
        public const string SESION_EXITOSA = "Inicio de sesión exitoso";
        public const string CREDENCIALES_INCORRECTAS = "Credenciales incorrectas.";
        public const string ERROR_GENERAL = "Ha ocurrido un error al intentar procesar la solicitud.";
        public const string CREDENCIALES_OBLIGATORIAS = "Usuario y contraseña son obligatorios.";
        public const string INGRESAR_VALIDOS = "Ingrese datos válidos.";
        public const string ERROR_DISTANCIA = "Error al calcular la distancia del viaje.";
        public const string VIAJES_NO_DISPONIBLES = "No hay viajes disponibles en la fecha y hora solicitada.";
        public const string SIN_PERMISO = "No tiene permiso para registrar un viaje.";
        #endregion

        #region Parametros
        public const string LISTADO_EXITOSO = "@Entidad listados exitosamente.";
        public const string CAMPO_EXISTENTE = "@Campo ya existe.";
        public const string CAMPOS_EXISTENTES = "@Campos ya existen.";
        public const string INGRESAR_VALIDA = "Ingrese una @campo válida.";
        public const string INGRESAR_VALIDO = "Ingrese @campo válido.";
        public const string DESACTIVADO = "@Entidad desactivado.";
        public const string CREADO_EXITOSAMENTE = "@Entidad registrado exitosamente.";
        public const string ASIGNADOS_EXITOSAMENTE = "@Entidad asignados exitosamente.";
        public const string ERROR_CREAR = "Ha ocurrido un error al registrar @articulo @entidad.";
        public const string ERROR_ASIGNAR= "Ha ocurrido un error al asignar @articulo @entidad.";
        public const string ERROR_RECHAZAR= "Ha ocurrido un error al rechazar @articulo @entidad.";
        public const string NO_EXISTE = "@Entidad no existe.";
        public const string RECHAZADO_EXITOSAMENTE = "@Entidad rechazada exitosamente.";
        public const string ERROR_LISTADO = "Error al obtener los @entidad.";
        public const string ERROR_LISTA = "Error al obtener las @entidad.";
        public const string ERROR_INDEPENDIENTE = "Error al obtener el @entidad.";
        public const string SIN_REGISTROS = "No hay @entidad disponibles.";
        public const string EN_USO = "No se puede eliminar el @entidad porque está en uso.";
        public const string DISTANCIA_EXCEDIDA = "La distancia total del viaje (@distanciakm km) fuera del rango aceptado (1 - 100 km).";
        public const string DISTANCIA_INVALIDA = "La distancia @distanciakm km está fuera del rango aceptado (1 - 50 km).";
        public const string ASIGNAR_VARIOS = "No se puede asignar @articulo @entidad más de una vez.";
        public const string CAMPOS_DUPLICADOS = "@Campos ya están duplicados.";
        public const string CAMPOS_NO_EXISTEN = "@Campos no existen.";
        public const string CAMPO_OBLIGATORIO = "@Campo es obligatorio.";
        public const string CAMPOS_OBLIGATORIOS = "@Campos son obligatorios.";
        public const string COLABORADOR_NO_VALIDO = "El colaborador con ID @colaboradorId no pertenece a la sucursal o ya tiene un viaje asignado.";
        public const string COLABORADORES_NO_VALIDOS = "Los Colaboradores con ID @colaboradoresIds no pertenecen a la sucursal o ya tienen un viaje asignado.";
        public const string LISTADO_INDEPENDIENTE = "@Entidad listado exitosamente.";
        public const string ACEPTADA = "La solicitud ya ha sido aceptada y no puede ser modificada.";
        #endregion
    }
}
