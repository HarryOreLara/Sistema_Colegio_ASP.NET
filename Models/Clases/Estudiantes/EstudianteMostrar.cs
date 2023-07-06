using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Estudiantes
{
    public class EstudianteMostrar
    {
        public int idEstudiante { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int dni { get; set; }
        public int edad { get; set; }
        public string direccion { get; set; }
        public int telefono { get; set; }
        public string email { get; set; }
        public string genero { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public DateTime fechaIngreso { get; set; }
        public DateTime fechaSalida { get; set; }
        public string estado { get; set; }
        public string grado { get; set; }
        public string seccion { get; set; }
        public string apoderado { get; set; }
        public string rango { get; set; }
        public string token { get; set; }

        public EstudianteMostrar() { }

        public EstudianteMostrar(int IdEstudiante, string Nombre, string Apellido, int Dni, int Edad, string Direccion, int Telefono, string Email, string Genero, DateTime FechaNacimiento,
                DateTime FechaIngreso, DateTime FechaSalida, string Estado, string Grado, string Seccion, string Apoderado, string Rango)
        {
            idEstudiante = IdEstudiante;
            nombre = Nombre;
            apellido = Apellido;
            dni = Dni;
            edad = Edad;
            direccion = Direccion;
            telefono = Telefono;
            email = Email;
            genero = Genero;
            fechaNacimiento = FechaNacimiento;
            fechaIngreso = FechaIngreso;
            fechaSalida = FechaSalida;
            estado = Estado;
            grado = Grado;
            seccion = Seccion;
            apoderado = Apoderado;
            rango = Rango;

        }

        public EstudianteMostrar(string Nombre, string Apellido, int Dni, int Edad, string Direccion, int Telefono, string Email, string Genero, DateTime FechaNacimiento,
                DateTime FechaIngreso, DateTime FechaSalida, string Estado, string Grado, string Seccion, string Apoderado, string Rango)
        {
            nombre = Nombre;
            apellido = Apellido;
            dni = Dni;
            edad = Edad;
            direccion = Direccion;
            telefono = Telefono;
            email = Email;
            genero = Genero;
            fechaNacimiento = FechaNacimiento;
            fechaIngreso = FechaIngreso;
            fechaSalida = FechaSalida;
            estado = Estado;
            grado = Grado;
            seccion = Seccion;
            apoderado = Apoderado;
            rango = Rango;

        }

    }
}