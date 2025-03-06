using Academia.SIMOVIA.WebAPI.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements
{
    public class RegistroViajeDomainRequirement
    {
        public static RegistroViajeDomainRequirement Fill(
          bool sucursalExiste,
          bool transportistaExiste,
          bool usuarioExiste,
          bool usuarioEsAdministrador,
          bool usuarioEsGerente,
          List<int> colaboradoresNoExistentes,
          List<int> colaboradoresNoDisponibles)
        {
            return new RegistroViajeDomainRequirement
            {
                SucursalExiste = sucursalExiste,
                TransportistaExiste = transportistaExiste,
                UsuarioExiste = usuarioExiste,
                UsuarioEsAdministrador = usuarioEsAdministrador,
                UsuarioEsGerente = usuarioEsGerente,
                ColaboradoresNoExistentes = colaboradoresNoExistentes,
                ColaboradoresNoDisponibles = colaboradoresNoDisponibles
            };
        }


        public bool SucursalExiste { get; set; }
        public bool TransportistaExiste { get; set; }
        public bool UsuarioExiste { get; set; }
        public bool UsuarioEsAdministrador { get; set; }
        public bool UsuarioEsGerente { get; set; }
        public List<int> ColaboradoresNoExistentes { get; set; }
        public List<int> ColaboradoresNoDisponibles { get; set; }

        public List<string> ObtenerErrores()
        {
            var errores = new List<string>();

            if (!SucursalExiste) errores.Add(Mensajes.NO_EXISTE.Replace("@Entidad", "Sucursal"));
            if (!TransportistaExiste) errores.Add(Mensajes.NO_EXISTE.Replace("@Entidad", "Transportista"));
            if (!UsuarioExiste) errores.Add(Mensajes.NO_EXISTE.Replace("@Entidad", "Usuario"));

            if (!UsuarioEsAdministrador && !UsuarioEsGerente)
                errores.Add(Mensajes.SIN_PERMISO);

            if (ColaboradoresNoExistentes.Any())
            {
                errores.Add(ColaboradoresNoExistentes.Count == 1 ? Mensajes.NO_EXISTE.Replace("@Entidad", $"Colaborador ID {ColaboradoresNoExistentes.First()}")
                    : Mensajes.CAMPOS_NO_EXISTEN.Replace("@Campos", $"Colaboradores ID {string.Join(", ", ColaboradoresNoExistentes)}"));
            }

            if (ColaboradoresNoDisponibles.Any())
            {
                errores.Add(ColaboradoresNoDisponibles.Count == 1 ? Mensajes.COLABORADOR_NO_VALIDO.Replace("@colaboradorId", ColaboradoresNoDisponibles.First().ToString())
                    : Mensajes.COLABORADORES_NO_VALIDOS.Replace("@colaboradoresIds", string.Join(", ", ColaboradoresNoDisponibles)));
            }

            return errores;
        }


        public bool EsValido() => !ObtenerErrores().Any();
    }
}
