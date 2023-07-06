using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Asistencias
{
    public class AsistenciaPrincipal
    {
        //Esto estara instanciado a asistencia search controller
        //Tendra la propiedad para obtener el numero de los apoderados en GestorAsitencia
        public int idAsistencia { get; set; }
        public int idEstudiante { get; set; }
        public DateTime fechaAsistencia { get; set; }
        public int idApoderado { get; set; }
        public string nombreEstudiante { get; set; }
        public string nombreApoderado { get; set; }
        public int telefonoApoderado { get; set; }
        public string presencia { get; set; }
        public string token { get; set; }
        public string mensaje { get; set; }
        public string apellidoEstudiante { get; set; }
        public int dni { get; set; }

        public AsistenciaPrincipal() { }
        public AsistenciaPrincipal(int IdAsistencia, int IdEstudiante, DateTime FechaAsitencia, string Mensaje)
        {
            idAsistencia = IdAsistencia;
            idEstudiante = IdEstudiante;
            fechaAsistencia = FechaAsitencia;
            mensaje = Mensaje;
        }

        public AsistenciaPrincipal(int IdEstudiante, DateTime FechaAsitencia)
        {
            idEstudiante = IdEstudiante;
            fechaAsistencia = FechaAsitencia;
        }

        public AsistenciaPrincipal(int IdEstudiante, string NombreEstudiante, int IdApoderado, string NombreApoderado, int TelefonoApoderado)
        { 
            
            idEstudiante = IdEstudiante;
            nombreEstudiante = NombreEstudiante;
            idApoderado = IdApoderado;
            nombreApoderado = NombreApoderado;
            telefonoApoderado = TelefonoApoderado;
        }

        public AsistenciaPrincipal(int IdEstudiante)
        {
            idEstudiante = IdEstudiante;
        }


        public AsistenciaPrincipal(int IdEstudiante, string NombreEstudiante, string Apellido, int Dni,string Presencia, string Mensaje)
        {
            idEstudiante = IdEstudiante;
            nombreEstudiante = NombreEstudiante;
            apellidoEstudiante = Apellido;
            dni = Dni;
            presencia = Presencia;
            mensaje = Mensaje;
        }
    }
}