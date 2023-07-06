using Back_JBG.Models.Clases.Asistencias;
using Back_JBG.Models.Clases.Response;
using Back_JBG.Models.Gestores.GestorAsistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Back_JBG.Controllers
{
    public class AsistenciaController : ApiController
    {
        // GET: api/Asistencia
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Asistencia/5
        public List<Asistencia> Get(int grado, int seccion)
        {
            GestorAsistencia gestorAsistencia = new GestorAsistencia();
            return gestorAsistencia.Get_Estudiante_GraSec(grado, seccion);
        }

        // POST: api/Asistencia
        public async Task<Response> Post([FromBody] List<AsistenciaPrincipal> asistenciaPrincipal)
        {
            DateTime fechaActual = DateTime.UtcNow;
            GestorAsistencia gestorAsistencia = new GestorAsistencia();
            Response res = await gestorAsistencia.Insert_Asistencia(asistenciaPrincipal);
            return res;

            //GestorAsistencia gestorAsistencia = new GestorAsistencia();
            //return gestorAsistencia.Get_Telefonos();


        }

        // PUT: api/Asistencia/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Asistencia/5
        public void Delete(int id)
        {
        }
    }
}
