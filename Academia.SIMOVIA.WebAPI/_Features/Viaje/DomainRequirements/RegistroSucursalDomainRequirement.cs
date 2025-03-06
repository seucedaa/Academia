using Academia.SIMOVIA.WebAPI.Utilities;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements
{
    namespace Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements
    {
        public class RegistroSucursalDomainRequirement
        {
            public static RegistroSucursalDomainRequirement Fill(
                bool descripcionExiste,
                bool ubicacionExiste,
                bool ciudadExiste,
                bool usuarioExiste)
            {
                return new RegistroSucursalDomainRequirement
                {
                    DescripcionExiste = descripcionExiste,
                    UbicacionExiste = ubicacionExiste,
                    CiudadExiste = ciudadExiste,
                    UsuarioExiste = usuarioExiste
                };
            }

            public bool DescripcionExiste { get; set; }
            public bool UbicacionExiste { get; set; }
            public bool CiudadExiste { get; set; }
            public bool UsuarioExiste { get; set; }

            public List<string> ObtenerErrores()
            {
                var errores = new List<string>();

                if (DescripcionExiste) errores.Add(Mensajes.CAMPO_EXISTENTE.Replace("@Campo", "Sucursal"));
                if (UbicacionExiste) errores.Add(Mensajes.CAMPO_EXISTENTE.Replace("@Campo", "Ubicación de la sucursal"));
                if (!CiudadExiste) errores.Add(Mensajes.NO_EXISTE.Replace("@Entidad", "Ciudad"));
                if (!UsuarioExiste) errores.Add(Mensajes.NO_EXISTE.Replace("@Entidad", "Usuario"));

                return errores;
            }

            public bool EsValido() => !ObtenerErrores().Any();
        }
    }

}
