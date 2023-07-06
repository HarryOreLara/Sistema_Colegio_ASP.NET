using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases
{
    public class Docente
    {
        public int idDocente { get; set; }
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
        public int idRango { get; set; }

        public string token { get; set; }

        public Docente() { }

        public Docente(int IdDocente, string Nombre, string Apellido, int Dni, int Edad, string Direccion, int Telefono, string Email, string Genero, DateTime FechaNacimiento,
            DateTime FechaIngreso, DateTime FechaSalida,string Estado, int IdRango)
        {
            idDocente = IdDocente;
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
            idRango = IdRango;
        }


        public Docente(string Nombre, string Apellido, int Dni, int Edad, string Direccion, int Telefono, string Email, string Genero, DateTime FechaNacimiento,
                        DateTime FechaIngreso, DateTime FechaSalida,string Estado, int IdRango)
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
            idRango = IdRango;
        }

    }
}