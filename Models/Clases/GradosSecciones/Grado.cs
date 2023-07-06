using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.GradosSecciones
{
    public class Grado
    {
        public int idGrado { get; set; }
        public string nombreGrado { get; set; }


        public Grado() { }

        public Grado(int IdGrado, string NombreGrado)
        {
            idGrado = IdGrado;
            nombreGrado = NombreGrado;
        }

        public Grado(string NombreGrado)
        {
            nombreGrado = NombreGrado;
        }
    }
}