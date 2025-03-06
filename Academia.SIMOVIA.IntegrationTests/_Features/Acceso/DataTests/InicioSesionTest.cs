using Academia.SIMOVIA.WebAPI._Features.Acceso.Dtos;
using Academia.SIMOVIA.WebAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Academia.SIMOVIA.IntegrationTests._Features.Acceso.DataTests
{
    public class InicioSesionTest : TheoryData<InicioSesionDto, string, (HttpStatusCode, string?)>
    {
        public InicioSesionTest()
        {
            Add(new InicioSesionDto { Usuario = "sua", Clave = "sua" },
                "DatabaseDown",(HttpStatusCode.InternalServerError, Mensajes.ERROR_BASE_DE_DATOS));

            Add(new InicioSesionDto { Usuario = "usuario_tarde", Clave = "sua" },
                "Timeout",(HttpStatusCode.RequestTimeout, Mensajes.SERVIDOR_NO_RESPONDE));

            Add(new InicioSesionDto { Usuario = "sua", Clave = "sua" },
                "ExcepcionNoControlada",(HttpStatusCode.InternalServerError, "Ocurrió un error inesperado"));
        }
    }
}
