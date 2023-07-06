using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Apoderados
{
    public class Apoderados
    {
        public int idApoderado { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int dni { get; set; }
        public string genero { get; set; }
        public int edad { get; set; }
        public int telefono { get; set; }
        public string email { get; set; }
        public int idRango { get; set; }
        public string token { get; set; }


        public Apoderados() { }

        public Apoderados(int IdApoderado, string Nombre, string Apellido, int Dni, string Genero, int Edad, int Telefono, string Email, int IdRango)
        {
            idApoderado = IdApoderado;
            nombre = Nombre;
            apellido = Apellido;
            dni = Dni;
            genero = Genero;
            edad = Edad;
            telefono = Telefono;
            email = Email;
            idRango = IdRango;
        }


        public Apoderados(string Nombre, string Apellido, int Dni, string Genero, int Edad, int Telefono, string Email, int IdRango)
        {
            nombre = Nombre;
            apellido = Apellido;
            dni = Dni;
            genero = Genero;
            edad = Edad;
            telefono = Telefono;
            email = Email;
            idRango = IdRango;
        }

    }
}