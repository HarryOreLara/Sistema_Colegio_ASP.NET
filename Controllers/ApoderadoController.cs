using Back_JBG.Models.Clases.Apoderados;
using Back_JBG.Models.Clases.Response;
using Back_JBG.Models.Gestores.GestorApoderado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Back_JBG.Controllers
{
    public class ApoderadoController : ApiController
    {
        // GET: api/Apoderado
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Apoderado/5
        public List<Apoderados> Get(int id)
        {
            GestorApoderado gestorApoderado = new GestorApoderado();
            return gestorApoderado.Get_One_Apoderado(id);
        }

        // POST: api/Apoderado
        public Response Post([FromBody]Apoderados apoderados)
        {
            GestorApoderado gestorApoderado = new GestorApoderado();
            Response res = gestorApoderado.Add_Apoderado(apoderados);
            return res;
        }

        // PUT: api/Apoderado/5
        public Response Put(int id, [FromBody]Apoderados apoderados)
        {
            GestorApoderado gestorApoderado = new GestorApoderado();
            Response res = gestorApoderado.Update_Apoderado(id, apoderados);
            return res;
        }

        // DELETE: api/Apoderado/5
        public void Delete(int id)
        {
        }
    }
}
