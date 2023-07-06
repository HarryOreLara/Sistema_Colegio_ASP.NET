using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Asistencias
{
    public class AsistenciaScript
    {
        public int telefono { get; set; }
        public string nombre { get; set; }
        public string apoderado { get; set; }

        public AsistenciaScript() { }

        public AsistenciaScript(int Telefono, string Nombre, string Apoderado)
        {
            telefono = Telefono;
            nombre = Nombre;
            apoderado = Apoderado;
        }
    }
}