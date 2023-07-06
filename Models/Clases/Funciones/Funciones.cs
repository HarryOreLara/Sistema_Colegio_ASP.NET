using Back_JBG.Models.Clases.Auth;
using Back_JBG.Models.Clases.Response;
using Back_JBG.Models.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Clases.Funciones
{

    public class Funciones
    {
        string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();
        Security objSecurity = new Security();
        Response.Response usuarioResponse = new Response.Response();

        public List<SubClases> buscarToken(string token)
        {

            string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();

            List<SubClases> listaToken = new List<SubClases>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlCommand = conec.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.CommandText = "Buscar_Token";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@token", token);

                try
                {
                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        int idToken = dataReader.GetInt32(0);
                        int idUsuario = dataReader.GetInt32(1);
                        string token_t = dataReader.GetString(2).Trim();
                        string estado_t = dataReader.GetString(3).Trim();
                        DateTime fecha_t = dataReader.GetDateTime(4);

                        SubClases subClases = new SubClases(idToken, idUsuario, token_t);
                        listaToken.Add(subClases);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    sqlCommand.Parameters.Clear();
                    conec.Close();
                }
            }

            return listaToken;
        }

        public Response.Response Add_Usuario_All(string usuario, string password, string estado, int idRango, int idPerteneciente)
        {
            using (SqlConnection conec = new SqlConnection(conexion))
            {
                SqlCommand sqlCommand = conec.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.CommandText = "Usuario_Add";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@usuario", usuario);
                sqlCommand.Parameters.AddWithValue("@password", password);
                sqlCommand.Parameters.AddWithValue("@estado", estado);
                sqlCommand.Parameters.AddWithValue("@idRango", idRango);
                sqlCommand.Parameters.AddWithValue("@idPerteneciente", idPerteneciente);

                try
                {
                    conec.Open();

                    if (sqlCommand.ExecuteNonQuery() >= 1)
                    {
                        Response.Result resultes = new Response.Result()
                        {
                            error_id = "100",
                            error_msg = "Todo Correcto"
                        };

                        Response.Response responses = new Response.Response()
                        {
                            status = "ok",
                            result = resultes
                        };

                        usuarioResponse = responses;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    sqlCommand.Parameters.Clear();
                    conec.Close();
                }

            }
            return usuarioResponse;

        }

        public string generarUsuario(string nombre, string apellido)
        {
            char primeraLetra = primeraLetra_x_palabra(nombre);//----S

            string primerApellido = dividirFraseEnDos(apellido, 0);

            string segundoApellido = dividirFraseEnDos(apellido, 1);


            char segundoApellidoLetra = primeraLetra_x_palabra(segundoApellido);

            string new_user = primeraLetra + primerApellido + segundoApellidoLetra;
            return new_user;
        }

        //Aca se divide el apelldio en las dos primeras Palabras
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


        public Response.Response Update_Usuario_All(string usuario, string password, string estado, int idRango, int idPerteneciente)
        {
            Response.Response Lara = new Response.Response();
            using (SqlConnection conec = new SqlConnection(conexion))
            {
                SqlCommand sqlCommand = conec.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.CommandText = "Usuario_Update";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idRangoUpdate", idRango);
                sqlCommand.Parameters.AddWithValue("@idPerteneceUpdate", idPerteneciente);
                sqlCommand.Parameters.AddWithValue("@usuario", usuario);
                sqlCommand.Parameters.AddWithValue("@password", password);
                sqlCommand.Parameters.AddWithValue("@estado", estado);
                sqlCommand.Parameters.AddWithValue("@idRango", idRango);
                sqlCommand.Parameters.AddWithValue("@idPerteneciente", idPerteneciente);

                try
                {
                    conec.Open();
                    if (sqlCommand.ExecuteNonQuery() >= 1)
                    {
                        Result resultes = new Result()
                        {
                            error_id = "100",
                            error_msg = "Todo Correcto"
                        };

                        Response.Response responses = new Response.Response()
                        {
                            status = "ok",
                            result = resultes
                        };

                        Lara = responses;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    sqlCommand.Parameters.Clear();
                    conec.Close();
                }
            }
            return Lara;
        }
    }
}