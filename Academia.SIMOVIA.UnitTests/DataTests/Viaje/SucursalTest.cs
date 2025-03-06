using Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements;
using Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements.Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.UnitTests.DataTests.Viaje
{
    public class SucursalTest : TheoryData<Sucursales, RegistroSucursalDomainRequirement, bool, string>
    {
        private readonly Sucursales _sucursalBase;
        private readonly RegistroSucursalDomainRequirement _requirementBase;

        public SucursalTest()
        {
            _sucursalBase = SucursalBase();
            _requirementBase = DomainRequirementBase();

            Add(SucursalConCamposObligatoriosFaltantes(), RequirementValido(), false, Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", "Descripción, Teléfono, Dirección Exacta, Latitud, Longitud, Ciudad, Usuario Creación"));
            Add(SucursalConCampoObligatorioFaltante(), RequirementValido(), false, Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", "Descripción"));
            Add(SucursalConTelefonoNoNumerico(), RequirementValido(), false, Mensajes.INGRESAR_VALIDO.Replace("@campo", "Teléfono"));
            Add(SucursalConDescripcionExcedida(), RequirementValido(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Descripción"));
            Add(SucursalConTelefonoExcedido(), RequirementValido(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Teléfono"));
            Add(SucursalConDireccionExcedida(), RequirementValido(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Dirección Exacta"));
            Add(SucursalConCamposExcedidos(), RequirementValido(), false, Mensajes.LONGITUDES_INVALIDAS.Replace("@campos", "Descripción, Teléfono"));
            Add(SucursalConUbicacionInvalida(), RequirementValido(), false, Mensajes.INGRESAR_VALIDA.Replace("@campo", "latitud"));
            Add(SucursalConLongitudInvalida(), RequirementValido(), false, Mensajes.INGRESAR_VALIDA.Replace("@campo", "longitud"));
            Add(SucursalConLatitudInvalidaBaja(), RequirementValido(), false, Mensajes.INGRESAR_VALIDA.Replace("@campo", "latitud"));
            Add(SucursalConLatitudInvalidaAlta(), RequirementValido(), false, Mensajes.INGRESAR_VALIDA.Replace("@campo", "latitud"));
            Add(SucursalConLongitudInvalidaBaja(), RequirementValido(), false, Mensajes.INGRESAR_VALIDA.Replace("@campo", "longitud"));
            Add(SucursalConLongitudInvalidaAlta(), RequirementValido(), false, Mensajes.INGRESAR_VALIDA.Replace("@campo", "longitud"));


            Add(SucursalConDescripcionDuplicada(), RequirementDescripcionDuplicada(), false, Mensajes.CAMPO_EXISTENTE.Replace("@Campo", "Sucursal"));
            Add(SucursalConUbicacionDuplicada(), RequirementUbicacionDuplicada(), false, Mensajes.CAMPO_EXISTENTE.Replace("@Campo", "Ubicación de la sucursal"));
            Add(SucursalConCiudadInexistente(), RequirementCiudadNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Ciudad"));
            Add(SucursalConUsuarioInexistente(), RequirementUsuarioNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Usuario"));

            Add(_sucursalBase, _requirementBase, true, null);
        }

        #region Sucursales

        private Sucursales SucursalBase() => new Sucursales
        {
            Descripcion = "Sucursal Central",
            Telefono = "22334455",
            DireccionExacta = "Avenida Principal 123",
            Latitud = 14.6349m,
            Longitud = -90.5069m,
            CiudadId = 1,
            UsuarioCreacionId = 1
        };

        private Sucursales DuplicarSucursal(Sucursales original) => new Sucursales
        {
            Descripcion = original.Descripcion,
            Telefono = original.Telefono,
            DireccionExacta = original.DireccionExacta,
            Latitud = original.Latitud,
            Longitud = original.Longitud,
            CiudadId = original.CiudadId,
            UsuarioCreacionId = original.UsuarioCreacionId
        };

        private Sucursales ModificarSucursal(Action<Sucursales> modificador)
        {
            var sucursal = DuplicarSucursal(_sucursalBase);
            modificador(sucursal);
            return sucursal;
        }

        private Sucursales SucursalConCamposObligatoriosFaltantes() => ModificarSucursal(s =>
        {
            s.Descripcion = "";
            s.Telefono = "";
            s.DireccionExacta = "";
            s.Latitud = 0;
            s.Longitud = 0;
            s.CiudadId = 0;
            s.UsuarioCreacionId = 0;
        });

        private Sucursales SucursalConCampoObligatorioFaltante() => ModificarSucursal(s => s.Descripcion = "");

        private Sucursales SucursalConTelefonoNoNumerico() => ModificarSucursal(s => s.Telefono = "ABC123");

        private Sucursales SucursalConDescripcionExcedida() => ModificarSucursal(s => s.Descripcion = new string('X', 51));

        private Sucursales SucursalConTelefonoExcedido() => ModificarSucursal(s => s.Telefono = new string('9', 9));

        private Sucursales SucursalConDireccionExcedida() => ModificarSucursal(s => s.DireccionExacta = new string('D', 101));
        private Sucursales SucursalConCamposExcedidos() => ModificarSucursal(s =>
        {
            s.Descripcion = new string('A', 51);
            s.Telefono = new string('1', 9);
        });
        private Sucursales SucursalConUbicacionInvalida() => ModificarSucursal(s =>
        {
            s.Latitud = 100.0000m;  
            s.Longitud = 200.0000m; 
        });
        private Sucursales SucursalConLongitudInvalida() => ModificarSucursal(s =>
        {
            s.Latitud = 14.6349m;
            s.Longitud = 200.0000m;
        });
        private Sucursales SucursalConLatitudInvalidaBaja() => ModificarSucursal(s =>
        {
            s.Latitud = -91m;  
            s.Longitud = -90.5069m;
        });

        private Sucursales SucursalConLatitudInvalidaAlta() => ModificarSucursal(s =>
        {
            s.Latitud = 91m; 
            s.Longitud = -90.5069m;
        });
        private Sucursales SucursalConLongitudInvalidaBaja() => ModificarSucursal(s =>
        {
            s.Latitud = 14.6349m;
            s.Longitud = -181m;  
        });
        private Sucursales SucursalConLongitudInvalidaAlta() => ModificarSucursal(s =>
        {
            s.Latitud = 14.6349m;
            s.Longitud = 181m;  
        });


        private Sucursales SucursalConDescripcionDuplicada() => ModificarSucursal(s => s.Descripcion = "Sucursal Duplicada");

        private Sucursales SucursalConUbicacionDuplicada() => ModificarSucursal(s =>
        {
            s.Latitud = 14.6349m;  
            s.Longitud = -90.5069m;
        });

        private Sucursales SucursalConCiudadInexistente() => ModificarSucursal(s => s.CiudadId = 99);

        private Sucursales SucursalConUsuarioInexistente() => ModificarSucursal(s => s.UsuarioCreacionId = 99);

        #endregion

        #region DomainRequirement

        private RegistroSucursalDomainRequirement RequirementValido() => _requirementBase;

        private RegistroSucursalDomainRequirement DomainRequirementBase() => new RegistroSucursalDomainRequirement
        {
            DescripcionExiste = false,
            UbicacionExiste = false,
            CiudadExiste = true,
            UsuarioExiste = true
        };

        private RegistroSucursalDomainRequirement DuplicarRequirement(RegistroSucursalDomainRequirement original) => new RegistroSucursalDomainRequirement
        {
            DescripcionExiste = original.DescripcionExiste,
            UbicacionExiste = original.UbicacionExiste,
            CiudadExiste = original.CiudadExiste,
            UsuarioExiste = original.UsuarioExiste
        };

        private RegistroSucursalDomainRequirement ModificarRequirement(Action<RegistroSucursalDomainRequirement> modificador)
        {
            var requirement = DuplicarRequirement(_requirementBase);
            modificador(requirement);
            return requirement;
        }

        private RegistroSucursalDomainRequirement RequirementDescripcionDuplicada() => ModificarRequirement(r => r.DescripcionExiste = true);

        private RegistroSucursalDomainRequirement RequirementUbicacionDuplicada() => ModificarRequirement(r => r.UbicacionExiste = true);

        private RegistroSucursalDomainRequirement RequirementCiudadNoExiste() => ModificarRequirement(r => r.CiudadExiste = false);

        private RegistroSucursalDomainRequirement RequirementUsuarioNoExiste() => ModificarRequirement(r => r.UsuarioExiste = false);

        #endregion
    }
}
