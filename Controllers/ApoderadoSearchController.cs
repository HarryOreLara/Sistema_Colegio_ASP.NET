using Back_JBG.Models.Clases.Apoderados;
using Back_JBG.Models.Gestores.GestorApoderado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Back_JBG.Controllers
{
    public class ApoderadoSearchController : ApiController
    {
        // GET: api/ApoderadoSearch
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ApoderadoSearch/5
        public List<Apoderados> Get(int id)
        {
            GestorApoderado gestorApoderado = new GestorApoderado();
            return gestorApoderado.Get_One_Apoderado_x_DNI(id);
        }

        // POST: api/ApoderadoSearch
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ApoderadoSearch/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApoderadoSearch/5
        public void Delete(int id)
        {
        }
    }
}
