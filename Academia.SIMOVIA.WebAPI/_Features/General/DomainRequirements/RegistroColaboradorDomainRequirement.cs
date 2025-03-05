using Academia.SIMOVIA.WebAPI.Utilities;

namespace Academia.SIMOVIA.WebAPI._Features.General.DomainRequirements
{
    public class RegistroColaboradorDomainRequirement
    {
        public static RegistroColaboradorDomainRequirement Fill(
            bool dniExiste,
            bool correoExiste,
            bool estadoCivilExiste,
            bool cargoExiste,
            bool ciudadExiste,
            bool usuarioExiste,
            List<int> sucursalesNoExistentes)
        {
            return new RegistroColaboradorDomainRequirement
            {
                DniExiste = dniExiste,
                CorreoExiste = correoExiste,
                EstadoCivilExiste = estadoCivilExiste,
                CargoExiste = cargoExiste,
                CiudadExiste = ciudadExiste,
                UsuarioExiste = usuarioExiste,
                SucursalesNoExistentes = sucursalesNoExistentes
            };
        }

        public bool DniExiste { get; set; }
        public bool CorreoExiste { get; set; }
        public bool EstadoCivilExiste { get; set; }
        public bool CargoExiste { get; set; }
        public bool CiudadExiste { get; set; }
        public bool UsuarioExiste { get; set; }
        public List<int>? SucursalesNoExistentes { get; set; }

        public List<string> ObtenerErrores()
        {
            List<string> errores = new List<string>();

            if (DniExiste) errores.Add(Mensajes.CAMPO_EXISTENTE.Replace("@Campo", "DNI"));
            if (CorreoExiste) errores.Add(Mensajes.CAMPO_EXISTENTE.Replace("@Campo", "Correo Electrónico"));
            if (!EstadoCivilExiste) errores.Add(Mensajes.NO_EXISTE.Replace("@Entidad", "Estado Civil"));
            if (!CargoExiste) errores.Add(Mensajes.NO_EXISTE.Replace("@Entidad", "Cargo"));
            if (!CiudadExiste) errores.Add(Mensajes.NO_EXISTE.Replace("@Entidad", "Ciudad"));
            if (!UsuarioExiste) errores.Add(Mensajes.NO_EXISTE.Replace("@Entidad", "Usuario"));

            if (SucursalesNoExistentes.Any())
            {
                errores.Add(SucursalesNoExistentes.Count == 1 ? Mensajes.NO_EXISTE.Replace("@Entidad", $"Sucursal ID {SucursalesNoExistentes.First()}")
                    : Mensajes.CAMPOS_NO_EXISTEN.Replace("@Campos", $"Sucursales ID {string.Join(", ", SucursalesNoExistentes)}"));
            }

            return errores;
        }

        public bool EsValido() => !ObtenerErrores().Any();
    }

}
