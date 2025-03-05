﻿using Academia.SIMOVIA.UnitTests.DataTests.Acceso;
using Academia.SIMOVIA.WebAPI._Features.Acceso;
using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI.Infrastructure.SIMOVIADataBase.Entities.Acceso;
using Academia.SIMOVIA.WebAPI.Utilities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.UnitTests
{
    public class AccesoDomainServiceTests
    {
        private readonly AccesoDomainService _accesoDomainService;
        public AccesoDomainServiceTests()
        {
            _accesoDomainService = new AccesoDomainService();
        }

        [Theory]
        [ClassData(typeof(InicioSesionTest))]
        public void Dado_Datos_Inicio_Sesion_Cuando_Se_Valida_DebeRetornarElResultadoEsperado(InicioSesionDto login, Usuarios usuario, bool esperadoExito, string esperadoMensaje)
        {
            var resultado = _accesoDomainService.ValidarInicioSesion(login, usuario);

            Assert.Equal(esperadoExito, resultado.Exitoso);
            Assert.Equal(esperadoMensaje, resultado.Mensaje);

            resultado.Should().BeEquivalentTo(new { Exitoso = esperadoExito, Mensaje = esperadoMensaje });

        }
    }
}
