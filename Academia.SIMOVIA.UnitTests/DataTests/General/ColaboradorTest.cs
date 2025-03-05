using Academia.SIMOVIA.WebAPI._Features.General.DomainRequirements;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.General;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using Xunit;

namespace Academia.SIMOVIA.UnitTests.DataTests.General
{
    public class ColaboradorTest : TheoryData<Colaboradores, RegistroColaboradorDomainRequirement, bool, string>
    {
        private readonly Colaboradores _colaboradorBase;
        private readonly RegistroColaboradorDomainRequirement _requirementBase;

        public ColaboradorTest()
        {
            _colaboradorBase = ColaboradorBase();
            _requirementBase = DomainRequirementBase();

            Add(ColaboradorConDatosIncompletos(), RequirementValido(), false, Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", "DNI, Nombres, Apellidos, Correo Electrónico, Teléfono, Sexo, Fecha de Nacimiento, Dirección Exacta, Latitud, Longitud, Estado Civil, Cargo, Ciudad, Usuario Creación, Sucursales"));
            Add(ColaboradorConDniExcedido(), RequirementValido(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "DNI"));
            Add(ColaboradorConNombresExcedidos(), RequirementValido(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Nombres"));
            Add(ColaboradorConApellidosExcedidos(), RequirementValido(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Apellidos"));
            Add(ColaboradorConCorreoExcedido(), RequirementValido(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Correo Electrónico"));
            Add(ColaboradorConTelefonoExcedido(), RequirementValido(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Teléfono"));
            Add(ColaboradorConDireccionExcedida(), RequirementValido(), false, Mensajes.LONGITUD_INVALIDA.Replace("@campo", "Dirección Exacta"));
            Add(ColaboradorConFechaNacimientoInvalida(), RequirementValido(), false, Mensajes.INGRESAR_VALIDO.Replace("@campo", "Fecha de Nacimiento"));
            Add(ColaboradorConSexoInvalido(), RequirementValido(), false, Mensajes.INGRESAR_VALIDO.Replace("@campo", "sexo"));
            Add(ColaboradorConDistanciaInvalida(), RequirementValido(), false, Mensajes.DISTANCIA_INVALIDA.Replace("@distanciakm", "0, 55"));
            Add(ColaboradorConDniDuplicado(), RequirementDniDuplicado(), false, Mensajes.CAMPO_EXISTENTE.Replace("@Campo", "DNI"));
            Add(ColaboradorConEstadoCivilNoExiste(), RequirementEstadoCivilNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Estado Civil"));
            Add(ColaboradorConCargoNoExiste(), RequirementCargoNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Cargo"));
            Add(ColaboradorConCiudadNoExiste(), RequirementCiudadNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Ciudad"));
            Add(ColaboradorConUsuarioNoExiste(), RequirementUsuarioNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Usuario"));
            Add(ColaboradorConSucursalNoExiste(), RequirementSucursalNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Sucursal ID 99"));

            Add(_colaboradorBase,_requirementBase, true, null);
        }

        #region Colaboradores
        Colaboradores ColaboradorBase() => new Colaboradores
        {
            DNI = "0265485555522",
            Nombres = "Juan Francisco",
            Apellidos = "Pineda",
            CorreoElectronico = "jaunfran@gmail.com",
            Telefono = "12548451",
            Sexo = "M",
            FechaNacimiento = DateTime.Today.AddYears(-30),
            DireccionExacta = "Las Lomas",
            Latitud = 15.252m,
            Longitud = -80.236m,
            EstadoCivilId = 1,
            CargoId = 1,
            CiudadId = 2,
            UsuarioCreacionId = 1,
            ColaboradoresPorSucursal = new List<ColaboradoresPorSucursal>
            {
                new ColaboradoresPorSucursal { SucursalId = 1, DistanciaKm = 34.6m }
            }
        };

        Colaboradores DuplicarColaborador(Colaboradores original) => new Colaboradores
        {
            DNI = original.DNI,
            Nombres = original.Nombres,
            Apellidos = original.Apellidos,
            CorreoElectronico = original.CorreoElectronico,
            Telefono = original.Telefono,
            Sexo = original.Sexo,
            FechaNacimiento = original.FechaNacimiento,
            DireccionExacta = original.DireccionExacta,
            Latitud = original.Latitud,
            Longitud = original.Longitud,
            EstadoCivilId = original.EstadoCivilId,
            CargoId = original.CargoId,
            CiudadId = original.CiudadId,
            UsuarioCreacionId = original.UsuarioCreacionId,
            ColaboradoresPorSucursal = new List<ColaboradoresPorSucursal>(original.ColaboradoresPorSucursal)
        };

        Colaboradores ModificarColaborador(Action<Colaboradores> modificador)
        {
            var colaborador = DuplicarColaborador(_colaboradorBase);
            modificador(colaborador);
            return colaborador;
        }

        Colaboradores ColaboradorConDatosIncompletos() => ModificarColaborador(c =>
        {
            c.DNI = "";
            c.Nombres = "";
            c.Apellidos = "";
            c.CorreoElectronico = "";
            c.Telefono = "";
            c.Sexo = "";
            c.FechaNacimiento = default;
            c.DireccionExacta = "";
            c.Latitud = 0;
            c.Longitud = 0;
            c.EstadoCivilId = 0;
            c.CargoId = 0;
            c.CiudadId = 0;
            c.UsuarioCreacionId = 0;
            c.ColaboradoresPorSucursal = new List<ColaboradoresPorSucursal>();
        });

        Colaboradores ColaboradorConDniExcedido() => ModificarColaborador(c => c.DNI = new string('1', 14));

        Colaboradores ColaboradorConNombresExcedidos()=> ModificarColaborador(c => c.Nombres = new string('A', 51));

        Colaboradores ColaboradorConApellidosExcedidos()=> ModificarColaborador(c => c.Apellidos = new string('B', 51));

        Colaboradores ColaboradorConCorreoExcedido()=> ModificarColaborador(c => c.CorreoElectronico = new string('C', 61));

        Colaboradores ColaboradorConTelefonoExcedido() => ModificarColaborador(c => c.Telefono = new string('9', 9));

        Colaboradores ColaboradorConDireccionExcedida() => ModificarColaborador(c => c.DireccionExacta = new string('D', 101));

        Colaboradores ColaboradorConFechaNacimientoInvalida() => ModificarColaborador(c => c.FechaNacimiento = DateTime.Today.AddYears(-100));

        Colaboradores ColaboradorConSexoInvalido()=> ModificarColaborador(c => c.Sexo = "X");

        Colaboradores ColaboradorConDistanciaInvalida() => ModificarColaborador(c =>
        {
            c.ColaboradoresPorSucursal = new List<ColaboradoresPorSucursal>
            {
                new ColaboradoresPorSucursal { SucursalId = 1, DistanciaKm = 0 },
                new ColaboradoresPorSucursal { SucursalId = 2, DistanciaKm = 55 }
            };
        });

        Colaboradores ColaboradorConDniDuplicado() => ModificarColaborador(c => c.DNI = "1804200502571");

        Colaboradores ColaboradorConEstadoCivilNoExiste()=> ModificarColaborador(c => c.EstadoCivilId = 99);

        Colaboradores ColaboradorConCargoNoExiste() => ModificarColaborador(c => c.CargoId = 99);

        Colaboradores ColaboradorConCiudadNoExiste()=> ModificarColaborador(c => c.CiudadId = 99);

        Colaboradores ColaboradorConUsuarioNoExiste() => ModificarColaborador(c => c.UsuarioCreacionId = 99);

        Colaboradores ColaboradorConSucursalNoExiste() => ModificarColaborador(c =>
        {
            c.ColaboradoresPorSucursal = new List<ColaboradoresPorSucursal>
            {
                new ColaboradoresPorSucursal { SucursalId = 99, DistanciaKm = 34.6m }
            };
        });
        #endregion

        #region DomainRequirement
        RegistroColaboradorDomainRequirement RequirementValido() => _requirementBase;

        RegistroColaboradorDomainRequirement DomainRequirementBase() => new RegistroColaboradorDomainRequirement
        {
            DniExiste = false,
            CorreoExiste = false,
            EstadoCivilExiste = true,
            CargoExiste = true,
            CiudadExiste = true,
            UsuarioExiste = true,
            SucursalesNoExistentes = new List<int>()
        };

        RegistroColaboradorDomainRequirement DuplicarRequirement(RegistroColaboradorDomainRequirement original) => new RegistroColaboradorDomainRequirement
        {
            DniExiste = original.DniExiste,
            CorreoExiste = original.CorreoExiste,
            EstadoCivilExiste = original.EstadoCivilExiste,
            CargoExiste = original.CargoExiste,
            CiudadExiste = original.CiudadExiste,
            UsuarioExiste = original.UsuarioExiste,
            SucursalesNoExistentes = new List<int>(original.SucursalesNoExistentes)
        };

        RegistroColaboradorDomainRequirement ModificarRequirement(Action<RegistroColaboradorDomainRequirement> modificador)
        {
            var requirement = DuplicarRequirement(_requirementBase);
            modificador(requirement);
            return requirement;
        }

        RegistroColaboradorDomainRequirement RequirementDniDuplicado() => ModificarRequirement(r => r.DniExiste = true);

        RegistroColaboradorDomainRequirement RequirementEstadoCivilNoExiste() => ModificarRequirement(r => r.EstadoCivilExiste = false);

        RegistroColaboradorDomainRequirement RequirementCargoNoExiste() => ModificarRequirement(r => r.CargoExiste = false);

        RegistroColaboradorDomainRequirement RequirementCiudadNoExiste()=> ModificarRequirement(r => r.CiudadExiste = false);

        RegistroColaboradorDomainRequirement RequirementUsuarioNoExiste()=> ModificarRequirement(r => r.UsuarioExiste = false);

        RegistroColaboradorDomainRequirement RequirementSucursalNoExiste() => ModificarRequirement(r => r.SucursalesNoExistentes = new List<int> { 99 });
        #endregion
    }
}
