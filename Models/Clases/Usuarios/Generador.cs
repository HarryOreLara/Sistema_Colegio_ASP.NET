using Back_JBG.Models.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Back_JBG.Models.Clases.Response;

namespace Back_JBG.Models.Clases.Usuarios
{
    public class Generador
    {
        Security objSecurity = new Security();
        Response.Response usuarioResponse = new Response.Response();
        Response.Result usuarioResult = new Response.Result();

        public string generarUsuario(string nombre, string apellido)
        {


            char primeraLetra = primeraLetra_x_palabra(nombre);//----S

            string primerApellido = dividirFraseEnDos(apellido, 0);

            string segundoApellido = dividirFraseEnDos(apellido, 1);


            char segundoApellidoLetra = primeraLetra_x_palabra(segundoApellido);

            string new_user = primeraLetra + primerApellido + segundoApellidoLetra;
            return new_user;
        }

        public string dividirFraseEnDos(string palabra, int posicion)
        {
            string inputString = palabra;
            string[] palabras = inputString.Split(' ');
            string new_palabra = palabras[posicion];

            return new_palabra;
        }

        public char primeraLetra_x_palabra(string palabra)
        {
            string palabras = palabra;
            //char primeraLetra;
            if (palabras.Length != 0)
            {
                char[] letras = palabras.ToCharArray();

                char primerLetra = letras[0];
                return primerLetra;


            }
            char solo = 'J';
            return solo;
        }

        public string generarPassword(int dniPassword)//Pasword generada por dni
        {
            //string salt = "jbg";
            string password_new = objSecurity.EncodePassword(dniPassword.ToString());//Contraseña Hasheada

            return password_new;
        }
    }
}