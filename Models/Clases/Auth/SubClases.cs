using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Auth
{
    public class SubClases
    {
        public int token_id { get; set; }
        public int usuario_id { get; set; }
        public string token_t { get; set; }


        public SubClases()
        {

        }

        public SubClases(int Token_ID, int Usuario_ID, string Token_t)
        {
            token_id = Token_ID;
            usuario_id = Usuario_ID;
            token_t = Token_t;
        }
    }
}