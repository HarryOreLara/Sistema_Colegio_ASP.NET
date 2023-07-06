using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.GradosSecciones
{
    public class Seccion
    {
        public int idSeccion { get; set; }
        public string nombreSeccion { get; set; }


        public Seccion() { }


        public Seccion(int IdSeccion, string NombreSeccion)
        {
            idSeccion = IdSeccion;
            nombreSeccion = NombreSeccion;
        }


        public Seccion(string NombreSeccion)
        {
            nombreSeccion = NombreSeccion;
        }

    }
}