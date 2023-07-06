using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Meterias
{
    public class Materia
    {
        public int idMateria { get; set; }
        public string nombre { get; set; }

        public Materia() { }

        public Materia(int IdMateria, string Nombre)
        {
            idMateria = IdMateria;
            nombre  = Nombre;
        }

        public Materia(string Nombre)
        {
            nombre = Nombre;
        }
    }
}