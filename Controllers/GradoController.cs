using Back_JBG.Models.Clases.Estudiantes;
using Back_JBG.Models.Clases.GradosSecciones;
using Back_JBG.Models.Gestores.GestorEstudiante;
using Back_JBG.Models.Gestores.GestorSeccionGrado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Back_JBG.Controllers
{
    public class GradoController : ApiController
    {
        // GET: api/Grado
        public IEnumerable<Grado> Get()
        {
            GestorGrado gestorGrado = new GestorGrado();
            return gestorGrado.Get_Grado_All();
        }

        // GET: api/Grado/grado/seccion
        public List<EstudianteMostrar> Get(int grado, int seccion)
        {
            GestorEstudiante gestorEstudiante = new GestorEstudiante();
            return gestorEstudiante.Get_Estudiante_Gra_Secc(grado, seccion);
        }

        // POST: api/Grado
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Grado/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Grado/5
        public void Delete(int id)
        {
        }
    }
}
