using Back_JBG.Models.Clases;
using Back_JBG.Models.Clases.Response;
using Back_JBG.Models.Gestores.GestorDocente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Back_JBG.Controllers
{
    public class DocenteController : ApiController
    {
        // GET: api/Docente
        public IEnumerable<Docente> Get()
        {
            GestorDocente gestorDocente = new GestorDocente();
            return gestorDocente.Get_All_Docente();
           //return new string[] { "value1", "value2" };
        }

        // GET: api/Docente/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Docente
        public Response Post([FromBody]Docente docente)
        {
            GestorDocente gestorDocente = new GestorDocente();
            Response res = gestorDocente.Insert_Docente(docente);
            return res;
        }

        // PUT: api/Docente/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Docente/5
        public void Delete(int id)
        {
        }
    }
}
