using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Auth
{
    public class Auth
    {
        public string usuario { get; set; }
        public string password { get; set; }

        public Auth() { }

        public Auth(string Usuario, string Password)
        {
            usuario = Usuario;
            password = Password;
        }
    }
}