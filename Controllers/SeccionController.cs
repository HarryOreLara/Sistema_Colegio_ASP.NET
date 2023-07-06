using Back_JBG.Models.Clases.GradosSecciones;
using Back_JBG.Models.Gestores.GestorSeccionGrado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Back_JBG.Controllers
{
    public class SeccionController : ApiController
    {
        // GET: api/Seccion
        public IEnumerable<Seccion> Get()
        {
            GestorSeccion gestorSeccion = new GestorSeccion();
            return gestorSeccion.Get_Seccion_All();
        }

        // GET: api/Seccion/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Seccion
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Seccion/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Seccion/5
        public void Delete(int id)
        {
        }
    }
}
