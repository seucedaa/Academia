using Academia.SIMOVIA.WebAPI._Features.Viaje.DomainRequirements;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Viaje;
using Academia.SIMOVIA.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using Xunit;

namespace Academia.SIMOVIA.UnitTests.DataTests.Viaje
{
    public class ViajeTest : TheoryData<ViajesEncabezado, RegistroViajeDomainRequirement, bool, string>
    {
        private readonly ViajesEncabezado _viajeBase;
        private readonly RegistroViajeDomainRequirement _requirementBase;

        public ViajeTest()
        {
            _viajeBase = ViajeBase();
            _requirementBase = DomainRequirementBase();

            Add(ViajeConCamposObligatoriosFaltantes(), RequirementValido(), false, Mensajes.CAMPOS_OBLIGATORIOS.Replace("@Campos", "Fecha y Hora, Sucursal, Transportista, Usuario Creación, Asignar colaboradores"));
            Add(ViajeSinSucursal(), RequirementValido(), false, Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", "Sucursal"));
            Add(ViajeConFechaInvalida(), RequirementValido(), false, Mensajes.INGRESAR_VALIDO.Replace("@campo", "Fecha y Hora de Viaje"));
            Add(ViajeConColaboradorDuplicado(), RequirementValido(), false, Mensajes.ASIGNAR_VARIOS.Replace("@articulo","el").Replace("@entidad", "Colaborador ID 3"));
            Add(ViajeConColaboradoresDuplicados(), RequirementValido(), false, Mensajes.ASIGNAR_VARIOS.Replace("@articulo","los").Replace("@entidad", "Colaboradores ID 3, 5"));
            Add(ViajeConSucursalInexistente(), RequirementSucursalNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Sucursal"));
            Add(ViajeConTransportistaInexistente(), RequirementTransportistaNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Transportista"));
            Add(ViajeConUsuarioInexistente(), RequirementUsuarioNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Usuario"));
            Add(ViajeBase(), RequirementUsuarioSinPermiso(), false, Mensajes.SIN_PERMISO);
            Add(ViajeBase(), RequirementUsuarioEsGerente(), true, null);
            Add(ViajeConColaboradorInexistente(), RequirementColaboradorNoExiste(), false, Mensajes.NO_EXISTE.Replace("@Entidad", "Colaborador ID 10"));
            Add(ViajeConColaboradoresInexistentes(), RequirementColaboradoresNoExisten(), false, Mensajes.CAMPOS_NO_EXISTEN.Replace("@Campos", "Colaboradores ID 10, 11"));
            Add(ViajeConColaboradorNoDisponible(), RequirementColaboradorNoDisponible(), false, Mensajes.COLABORADOR_NO_VALIDO.Replace("@colaboradorId", "4"));
            Add(ViajeConColaboradoresNoDisponibles(), RequirementColaboradoresNoDisponibles(), false, Mensajes.COLABORADORES_NO_VALIDOS.Replace("@colaboradoresIds", "4, 5"));
            Add(ViajeSinColaboradores(), RequirementValido(), false, Mensajes.CAMPO_OBLIGATORIO.Replace("@Campo", "Asignar colaboradores"));


            Add(_viajeBase, _requirementBase, true, null);
        }

        #region Viajes
        private ViajesEncabezado ViajeBase() => new ViajesEncabezado
        {
            FechaHora = DateTime.Today.AddDays(1),
            SucursalId = 1,
            TransportistaId = 2,
            UsuarioCreacionId = 1,
            ViajesDetalle = new List<ViajesDetalle>
            {
                new ViajesDetalle { ColaboradorId = 3 }
            }
        };

        private ViajesEncabezado DuplicarViaje(ViajesEncabezado original) => new ViajesEncabezado
        {
            FechaHora = original.FechaHora,
            SucursalId = original.SucursalId,
            TransportistaId = original.TransportistaId,
            UsuarioCreacionId = original.UsuarioCreacionId,
            ViajesDetalle = new List<ViajesDetalle>(original.ViajesDetalle)
        };


        private ViajesEncabezado ModificarViaje(Action<ViajesEncabezado> modificador)
        {
            var viaje = DuplicarViaje(_viajeBase);
            modificador(viaje);
            return viaje;
        }

        private ViajesEncabezado ViajeSinSucursal() => ModificarViaje(v =>
        {
            v.SucursalId = 0;
        });

        private ViajesEncabezado ViajeConCamposObligatoriosFaltantes() => ModificarViaje(v =>
        {
            v.FechaHora = default;
            v.SucursalId = 0;
            v.TransportistaId = 0;
            v.UsuarioCreacionId = 0;
            v.ViajesDetalle = new List<ViajesDetalle>();
        });

        private ViajesEncabezado ViajeConFechaInvalida() => ModificarViaje(v => v.FechaHora = DateTime.Today.AddYears(-2));

        private ViajesEncabezado ViajeConColaboradorDuplicado() => ModificarViaje(v =>
        {
            v.ViajesDetalle.Add(new ViajesDetalle { ColaboradorId = 3 });
        });
        
        private ViajesEncabezado ViajeConColaboradoresDuplicados() => ModificarViaje(v =>
        {
            v.ViajesDetalle.Add(new ViajesDetalle { ColaboradorId = 3 });
            v.ViajesDetalle.Add(new ViajesDetalle { ColaboradorId = 3 });
            v.ViajesDetalle.Add(new ViajesDetalle { ColaboradorId = 5 });
            v.ViajesDetalle.Add(new ViajesDetalle { ColaboradorId = 5 });
        });

        private ViajesEncabezado ViajeConSucursalInexistente() => ModificarViaje(v => v.SucursalId = 99);

        private ViajesEncabezado ViajeConTransportistaInexistente() => ModificarViaje(v => v.TransportistaId = 99);

        private ViajesEncabezado ViajeConUsuarioInexistente() => ModificarViaje(v => v.UsuarioCreacionId = 99);

        private ViajesEncabezado ViajeConColaboradorInexistente() => ModificarViaje(v =>
        {
            v.ViajesDetalle = new List<ViajesDetalle>
            {
                new ViajesDetalle { ColaboradorId = 10 },
            };
        });
        private ViajesEncabezado ViajeConColaboradoresInexistentes() => ModificarViaje(v =>
        {
            v.ViajesDetalle = new List<ViajesDetalle>
            {
                new ViajesDetalle { ColaboradorId = 10 },
                new ViajesDetalle { ColaboradorId = 11 }
            };
        });

        private ViajesEncabezado ViajeConColaboradorNoDisponible() => ModificarViaje(v =>
        {
            v.ViajesDetalle = new List<ViajesDetalle>
            {
                new ViajesDetalle { ColaboradorId = 4 },
            };
        });
        private ViajesEncabezado ViajeConColaboradoresNoDisponibles() => ModificarViaje(v =>
        {
            v.ViajesDetalle = new List<ViajesDetalle>
            {
                new ViajesDetalle { ColaboradorId = 4 },
                new ViajesDetalle { ColaboradorId = 5 }
            };
        });

        private ViajesEncabezado ViajeSinColaboradores() => ModificarViaje(v =>
        {
            v.ViajesDetalle = null;
        });


        #endregion

        #region DomainRequirement
        private RegistroViajeDomainRequirement RequirementValido() => _requirementBase;

        private RegistroViajeDomainRequirement DomainRequirementBase() => new RegistroViajeDomainRequirement
        {
            SucursalExiste = true,
            TransportistaExiste = true,
            UsuarioExiste = true,
            UsuarioEsAdministrador = true,
            UsuarioEsGerente = false,
            ColaboradoresNoExistentes = new List<int>(),
            ColaboradoresNoDisponibles = new List<int>()
        };

        private RegistroViajeDomainRequirement DuplicarRequirement(RegistroViajeDomainRequirement original) => new RegistroViajeDomainRequirement
        {
            SucursalExiste = original.SucursalExiste,
            TransportistaExiste = original.TransportistaExiste,
            UsuarioExiste = original.UsuarioExiste,
            UsuarioEsAdministrador = original.UsuarioEsAdministrador,
            UsuarioEsGerente = original.UsuarioEsGerente,
            ColaboradoresNoExistentes = new List<int>(original.ColaboradoresNoExistentes),
            ColaboradoresNoDisponibles = new List<int>(original.ColaboradoresNoDisponibles)
        };

        private RegistroViajeDomainRequirement ModificarRequirement(Action<RegistroViajeDomainRequirement> modificador)
        {
            var requirement = DuplicarRequirement(_requirementBase);
            modificador(requirement);
            return requirement;
        }

        private RegistroViajeDomainRequirement RequirementSucursalNoExiste() => ModificarRequirement(r => r.SucursalExiste = false);

        private RegistroViajeDomainRequirement RequirementTransportistaNoExiste() => ModificarRequirement(r => r.TransportistaExiste = false);

        private RegistroViajeDomainRequirement RequirementUsuarioNoExiste() => ModificarRequirement(r => r.UsuarioExiste = false);
        private RegistroViajeDomainRequirement RequirementUsuarioSinPermiso() => ModificarRequirement(r =>
        {
            r.UsuarioEsAdministrador = false;
            r.UsuarioEsGerente = false;
        });

        private RegistroViajeDomainRequirement RequirementUsuarioEsGerente() => ModificarRequirement(r =>
        {
            r.UsuarioEsAdministrador = false;
            r.UsuarioEsGerente = true;
        });

        private RegistroViajeDomainRequirement RequirementColaboradorNoExiste() => ModificarRequirement(r => r.ColaboradoresNoExistentes = new List<int> { 10 });
        private RegistroViajeDomainRequirement RequirementColaboradoresNoExisten() => ModificarRequirement(r => r.ColaboradoresNoExistentes = new List<int> { 10, 11 });

        private RegistroViajeDomainRequirement RequirementColaboradorNoDisponible() => ModificarRequirement(r => r.ColaboradoresNoDisponibles = new List<int> { 4 });
        private RegistroViajeDomainRequirement RequirementColaboradoresNoDisponibles() => ModificarRequirement(r => r.ColaboradoresNoDisponibles = new List<int> { 4, 5 });

        #endregion
    }
}
