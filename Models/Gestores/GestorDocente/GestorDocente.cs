using Back_JBG.Models.Clases;
using Back_JBG.Models.Clases.Auth;
using Back_JBG.Models.Clases.Funciones;
using Back_JBG.Models.Clases.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Gestores.GestorDocente
{
    public class GestorDocente
    {
        string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();


        //GET
        //INSERT
        public Response Insert_Docente(Docente docente)
        {
            Response res = new Response();
            List<Docente> listDocente = new List<Docente>();
            List<SubClases> listaMandada = new List<SubClases>();
            List<Docente> docenteMostar = new List<Docente>();
            Funciones funciones = new Funciones();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                SqlCommand sqlCommand = conec.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.CommandText = "Docente_Insert";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@nombre", docente.nombre);
                sqlCommand.Parameters.AddWithValue("@apellido", docente.apellido);
                sqlCommand.Parameters.AddWithValue("@dni", docente.dni);
                sqlCommand.Parameters.AddWithValue("@edad", docente.edad);
                sqlCommand.Parameters.AddWithValue("@direccion", docente.direccion);
                sqlCommand.Parameters.AddWithValue("@telefono", docente.telefono);
                sqlCommand.Parameters.AddWithValue("@email", docente.email);
                sqlCommand.Parameters.AddWithValue("@genero", docente.genero);
                sqlCommand.Parameters.AddWithValue("@nacimiento", docente.fechaNacimiento);
                sqlCommand.Parameters.AddWithValue("@fechaIngreso", docente.fechaIngreso);
                sqlCommand.Parameters.AddWithValue("@fechaSalida", docente.fechaSalida);
                sqlCommand.Parameters.AddWithValue("@estado", docente.estado);
                sqlCommand.Parameters.AddWithValue("@idRango", docente.idRango);

                string token = docente.token;
                int dni = docente.dni;
                string nombre = docente.nombre;
                string apellido = docente.apellido;
                string estado = docente.estado;
                int idRango = docente.idRango;

                string usuario_new = funciones.generarUsuario(nombre, apellido);
                string password_new = funciones.generarPassword(dni);

                try
                {
                    conec.Open();
                    if (token.Length == 0)
                    {
                        Result resultes = new Result()
                        {
                            error_id = "401",
                            error_msg = "No se envio el token o token vacio"
                        };

                        Response responses = new Response()
                        {
                            status = "error",
                            result = resultes
                        };

                        res = responses;
                        return res;

                    }


                    listaMandada = funciones.buscarToken(token);//Busca el token y valida si esta activo

                    if (listaMandada.Count == 0)
                    {
                        Result resultes = new Result()
                        {
                            error_id = "401",
                            error_msg = "No existe el token en la base de datos"
                        };

                        Response responses = new Response()
                        {
                            status = "error",
                            result = resultes
                        };

                        res = responses;
                        return res;
                    }


                    if (sqlCommand.ExecuteNonQuery() >= 1)
                    {
                        Result resultes = new Result()
                        {
                            error_id = "100",
                            error_msg = docente.dni.ToString()
                        };

                        Response responses = new Response()
                        {
                            status = "ok",
                            result = resultes
                        };

                        res = responses;
                    }


                    //Get_Docente_Dni
                    docenteMostar = Get_Docente_Dni(dni);
                    int us_id = docenteMostar[0].idDocente;

                    funciones.Add_Usuario_All(usuario_new, password_new, estado, idRango, us_id);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return res;
        }

        //Solo busca el token y solo quiere resultados, que si hay algo con el tokjen mas no esta interesado en los valores de los resultados
        //private List<SubClases> buscarToken(string token)
        //{

        //    string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();

        //    List<SubClases> listaToken = new List<SubClases>();

        //    using (SqlConnection conec = new SqlConnection(conexion))
        //    {
        //        conec.Open();
        //        SqlCommand sqlCommand = conec.CreateCommand();
        //        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
        //        sqlCommand.CommandText = "Buscar_Token";
        //        sqlCommand.CommandType = CommandType.StoredProcedure;

        //        sqlCommand.Parameters.AddWithValue("@token", token);

        //        try
        //        {
        //            SqlDataReader dataReader = sqlCommand.ExecuteReader();

        //            while (dataReader.Read())
        //            {
        //                int idToken = dataReader.GetInt32(0);
        //                int idUsuario = dataReader.GetInt32(1);
        //                string token_t = dataReader.GetString(2).Trim();
        //                string estado_t = dataReader.GetString(3).Trim();
        //                DateTime fecha_t = dataReader.GetDateTime(4);

        //                SubClases subClases = new SubClases(idToken, idUsuario, token_t);
        //                listaToken.Add(subClases);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex);
        //        }
        //        finally
        //        {
        //            sqlCommand.Parameters.Clear();
        //            conec.Close();
        //        }
        //    }

        //    return listaToken;
        //}


        public List<Docente> Get_Docente_Dni(int dni_bd)
        {
            List<Docente> listaDocente = new List<Docente>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Get_Docente_Dni";
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@dni", dni_bd);

                SqlDataReader dataReader = sqlComm.ExecuteReader();


                //@nombre, @apellido, @dni, @edad, @direccion, @telefono, @email, @genero, 
                //@nacimiento, @fechaIngreso, @fechaSalida,@estado, @idRango)

                while (dataReader.Read())
                {
                    int idDocente = dataReader.GetInt32(0);
                    string nombre = dataReader.GetString(1);
                    string apellido = dataReader.GetString(2);
                    int dni = dataReader.GetInt32(3);
                    int edad = dataReader.GetInt32(4);
                    string direccion = dataReader.GetString(5);
                    int telefono = dataReader.GetInt32(6);
                    string email = dataReader.GetString(7);
                    string genero = dataReader.GetString(8);
                    DateTime fechaNacimiento = dataReader.GetDateTime(9);
                    DateTime fechaIngreso = dataReader.GetDateTime(10);
                    DateTime fechaSalida = dataReader.GetDateTime(11);
                    string estado = dataReader.GetString(12);
                    int idRango = dataReader.GetInt32(13);


                    Docente estudianteMostrar = new Docente(idDocente, nombre, apellido, dni, edad, direccion, telefono, email, genero, fechaNacimiento,
                        fechaIngreso, fechaSalida, estado, idRango);

                    listaDocente.Add(estudianteMostrar);

                }
                dataReader.Close();
                conec.Close();
            }
            return listaDocente;
        }

        //GET ALL
        public List<Docente> Get_All_Docente()
        {
            List<Docente> listaDocente = new List<Docente>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Get_All_Docente";
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataReader dataReader = sqlComm.ExecuteReader();
                //idDocente, nombre, apellido, dni, edad, direccion, telefono, email, genero, nacimiento, fechaIngreso, fechaSalida, estado, idRango
                while (dataReader.Read())
                {
                    int idDocente = dataReader.GetInt32(0);
                    string nombre = dataReader.GetString(1);
                    string apellido = dataReader.GetString(2);
                    int dni = dataReader.GetInt32(3);
                    int edad = dataReader.GetInt32(4);
                    string direccion = dataReader.GetString(5);
                    int telefono = dataReader.GetInt32(6);
                    string email = dataReader.GetString(7);
                    string genero = dataReader.GetString(8);
                    DateTime fechaNacimiento = dataReader.GetDateTime(9);
                    DateTime fechaIngreso = dataReader.GetDateTime(10);
                    DateTime fechaSalida = dataReader.GetDateTime(11);
                    string estado = dataReader.GetString(12);
                    int idRango = dataReader.GetInt32(13);

                    Docente estudianteMostrar = new Docente(idDocente, nombre, apellido, dni, edad, direccion, telefono, email, genero, fechaNacimiento,
                    fechaIngreso, fechaSalida, estado, idRango);

                    listaDocente.Add(estudianteMostrar);
                }
                dataReader.Close();
                conec.Close();

            }
            return listaDocente;

        }
        //DELETE
        //UPDATE
    }
}