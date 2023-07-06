using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Asistencias
{
    public class Asistencia
    {
        public int idEstudiante { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }


        public Asistencia() { }

        public Asistencia(int IdEstudiante, string Nombre, string Apellido)
        {
            idEstudiante = IdEstudiante;
            nombre = Nombre;
            apellido = Apellido;
        }

        public Asistencia(string Nombre, string Apellido)
        {
            nombre = Nombre;
            apellido = Apellido;
        }
    }
}