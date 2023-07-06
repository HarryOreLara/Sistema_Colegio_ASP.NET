using Back_JBG.Models.Clases.Estudiantes;
using Back_JBG.Models.Clases.Response;
using Back_JBG.Models.Gestores.GestorEstudiante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Back_JBG.Controllers
{
    public class EstudianteController : ApiController
    {
        // GET: api/Estudiante
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Estudiante/5
        public List<Estudiante> Get(int id)
        {
            GestorEstudiante gestorEstudiante = new GestorEstudiante();
            List<Estudiante> estudianteUnico = gestorEstudiante.Get_Estudiante_Id(id);
            return estudianteUnico;
        }

        // POST: api/Estudiante
        public Response Post([FromBody]Estudiante estudiante)
        {
            GestorEstudiante gestorEstudiante = new GestorEstudiante();
            Response res = gestorEstudiante.Add_Estudiante(estudiante);
            return res;
        }

        // PUT: api/Estudiante/5
        public Response Put(int id, [FromBody]Estudiante estudiante)
        {
            GestorEstudiante gestorEstudiante = new GestorEstudiante();
            Response res = gestorEstudiante.Update_Estudiante(id, estudiante);
            return res;
        }

        // DELETE: api/Estudiante/5
        public void Delete(int id)
        {
        }
    }
}
