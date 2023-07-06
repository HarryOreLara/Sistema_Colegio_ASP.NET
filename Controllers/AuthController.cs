using Back_JBG.Models.Clases.Auth;
using Back_JBG.Models.Clases.Response;
using Back_JBG.Models.Gestores.GestorAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Back_JBG.Controllers
{
    public class AuthController : ApiController
    {
        // GET: api/Auth
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Auth/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Auth
        public Response Post([FromBody]Auth auth)
        {
            GestorAuth gestorAuth = new GestorAuth();
            Response res = gestorAuth.GetResponse(auth.usuario, auth.password);
            return res;
        }

        // PUT: api/Auth/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Auth/5
        public void Delete(int id)
        {
        }
    }
}
