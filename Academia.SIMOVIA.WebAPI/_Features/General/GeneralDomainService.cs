using Academia.SIMOVIA.WebAPI._Features.General.Dtos;
using Academia.SIMOVIA.WebAPI.Helpers;
using Academia.SIMOVIA.WebAPI.Infrastructure;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Academia.SIMOVIA.WebAPI._Features.General
{
    public class GeneralDomainService
    {

        public async Task<Response<int>> ValidarRegistrarDatosColaborador(ColaboradorDto colaboradorDto)
        {
            
            var camposObligatorios = new List<(string Campo, bool EsValido)>
            {
                ("DNI", !string.IsNullOrEmpty(colaboradorDto.DNI)),
                ("Nombres", !string.IsNullOrEmpty(colaboradorDto.Nombres)),
                ("Apellidos", !string.IsNullOrEmpty(colaboradorDto.Apellidos)),
                ("Correo Electrónico", !string.IsNullOrEmpty(colaboradorDto.CorreoElectronico)),
                ("Teléfono", !string.IsNullOrEmpty(colaboradorDto.Telefono)),
                ("Sexo", !string.IsNullOrEmpty(colaboradorDto.Sexo)),
                ("Fecha de Nacimiento", colaboradorDto.FechaNacimiento != default),
                ("Dirección Exacta", !string.IsNullOrEmpty(colaboradorDto.DireccionExacta)),
                ("Latitud", colaboradorDto.Latitud != 0),
                ("Longitud", colaboradorDto.Longitud != 0),
                ("Estado Civil", colaboradorDto.EstadoCivilId > 0),
                ("Cargo", colaboradorDto.CargoId > 0),
                ("Ciudad", colaboradorDto.CiudadId > 0),
                ("Usuario Creacion", colaboradorDto.UsuarioGuardaId > 0),
                ("Sucursales", colaboradorDto.Sucursales != null && colaboradorDto.Sucursales.Any())
            };

            var campoFaltante = camposObligatorios.FirstOrDefault(c => !c.EsValido);
            if (campoFaltante.Campo != null && !campoFaltante.EsValido)
            {
                return new Response<int> { Exitoso = false, Mensaje = Mensajes.MSJ09.Replace("@Campo", campoFaltante.Campo) };
            }

            return new Response<int> { Exitoso = true };
        }
    }
}
