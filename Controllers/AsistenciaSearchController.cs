using Back_JBG.Models.Clases.Asistencias;
using Back_JBG.Models.Gestores.GestorAsistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Back_JBG.Controllers
{
    public class AsistenciaSearchController : ApiController
    {
        // GET: api/AsistenciaSearch
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AsistenciaSearch/5
        public List<AsistenciaPrincipal> Get(DateTime fecha, int grado, int seccion)
        {
            GestorAsistencia gestorAsistencia = new GestorAsistencia();
            return gestorAsistencia.despliegueAsistencias(fecha, grado, seccion);

        }

        // POST: api/AsistenciaSearch
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/AsistenciaSearch/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AsistenciaSearch/5
        public void Delete(int id)
        {
        }
    }
}
