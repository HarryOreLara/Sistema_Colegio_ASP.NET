using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Estudiantes
{
    public class Estudiante
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
        public int idGrado { get; set; }
        public int idSeccion { get; set; }
        public int idApoderado { get; set; }
        public string nombreApoderado { get; set; }
        public int dniApoderado { get; set; }
        public int idRango { get; set; }
        public string token { get; set; }

        public Estudiante() { }


        public Estudiante(int IdEstudiante, string Nombre, string Apellido, int Dni, int Edad, string Direccion, int Telefono, string Email, string Genero, DateTime FechaNacimiento,
            DateTime FechaIngreso, DateTime FechaSalida, string Estado, int IdGrado, int IdSeccion, int IdApoderado, int IdRango)
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
            idGrado = IdGrado;
            idSeccion = IdSeccion;
            idApoderado = IdApoderado;
            idRango = IdRango;

        }

        public Estudiante( string Nombre, string Apellido, int Dni, int Edad, string Direccion, int Telefono, string Email, string Genero, DateTime FechaNacimiento,
                            DateTime FechaIngreso, DateTime FechaSalida, string Estado, int IdGrado, int IdSeccion, int IdApoderado, int IdRango)
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
            idGrado = IdGrado;
            idSeccion = IdSeccion;
            idApoderado = IdApoderado;
            idRango = IdRango;

        }


        public Estudiante(string Nombre, string Apellido, int Dni, int Edad, string Direccion, int Telefono, string Email, string Genero, DateTime FechaNacimiento,
                    DateTime FechaIngreso, DateTime FechaSalida, string Estado, int IdGrado, int IdSeccion, int IdApoderado, int IdRango, string NombreApoderado, int DniApoderado)
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
            idGrado = IdGrado;
            idSeccion = IdSeccion;
            idApoderado = IdApoderado;
            nombreApoderado = NombreApoderado;
            dniApoderado = DniApoderado;
            idRango = IdRango;

        }


    }
}