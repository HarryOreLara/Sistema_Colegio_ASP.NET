using Back_JBG.Models.Clases.Meterias;
using Back_JBG.Models.Gestores.GestorMateria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Back_JBG.Controllers
{
    public class MateriaController : ApiController
    {
        // GET: api/Materia
        public IEnumerable<Materia> Get()
        {
            GestorMateria gestorMateria = new GestorMateria();
            //return new string[] { "value1", "value2" };
            return gestorMateria.Get_All_Materia();
        }

        // GET: api/Materia/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Materia
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Materia/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Materia/5
        public void Delete(int id)
        {
        }
    }
}
