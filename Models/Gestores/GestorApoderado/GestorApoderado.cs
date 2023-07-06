using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Back_JBG.Models.Clases.Response;
using Back_JBG.Models.Clases.Seguridad;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Back_JBG.Models.Clases.Estudiantes;
using Back_JBG.Models.Clases.Auth;
using Back_JBG.Models.Clases.Apoderados;
using Back_JBG.Models.Clases.Usuarios;
using Back_JBG.Models.Clases.Funciones;

namespace Back_JBG.Models.Gestores.GestorApoderado
{
    public class GestorApoderado
    {
        string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();
        Security objSecurity = new Security();
        Response usuarioResponse = new Response();
        Generador generador = new Generador();

        //INSERT
        public Response Add_Apoderado(Apoderados apoderados)
        {
            List<SubClases> listaMandada = new List<SubClases>();
            Response Lara = new Response();

            List<Apoderados> listaApoderadoMostrar = new List<Apoderados>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                SqlCommand sqlCommand = conec.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.CommandText = "Apoderado_Add";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@nombre", apoderados.nombre);
                sqlCommand.Parameters.AddWithValue("@apellido", apoderados.apellido);
                sqlCommand.Parameters.AddWithValue("@dni", apoderados.dni);
                sqlCommand.Parameters.AddWithValue("@genero", apoderados.genero);
                sqlCommand.Parameters.AddWithValue("@edad", apoderados.edad);
                sqlCommand.Parameters.AddWithValue("@telefono", apoderados.telefono);
                sqlCommand.Parameters.AddWithValue("@email", apoderados.email);
                sqlCommand.Parameters.AddWithValue("@idRango", apoderados.idRango);

                string token = apoderados.token;
                int idApoderado = apoderados.idApoderado;
                string estado = "activo";
                int idRango = apoderados.idRango;
                string nombre = apoderados.nombre;
                string apellido = apoderados.apellido;
                int dni = apoderados.dni;

                string usuario_new = generador.generarUsuario(nombre, apellido);
                string password_new = generador.generarPassword(dni);//Aca la contraseña ya esta con hash

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

                        Lara = responses;
                        return Lara;

                    }

                    listaMandada = buscarToken(token);//Busca el token y valida si esta activo

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

                        Lara = responses;
                        return Lara;
                    }


                    if (sqlCommand.ExecuteNonQuery() >= 1)
                    {
                        Result resultes = new Result()
                        {
                            error_id = "100",
                            error_msg = idApoderado.ToString()
                        };

                        Response responses = new Response()
                        {
                            status = "ok",
                            result = resultes
                        };

                        Lara = responses;
                    }

                    listaApoderadoMostrar = Get_One_Apoderado_x_DNI(dni);
                    int us_id = listaApoderadoMostrar[0].idApoderado;

                    Add_Usuario_Apoderado(usuario_new, password_new, estado, idRango, us_id);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    sqlCommand.Parameters.Clear();
                    conec.Close();
                }

            }
            return Lara;
        }

        private List<SubClases> buscarToken(string token)
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
    
        public Response Add_Usuario_Apoderado(string usuario, string password, string estado, int idRango, int idPerteneciente)
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
                        Result resultes = new Result()
                        {
                            error_id = "100",
                            error_msg = "Todo Correcto"
                        };

                        Response responses = new Response()
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

        //GET



        //GET_ONE-----Apoderados lo mandaremos con su DNI
        public List<Apoderados> Get_One_Apoderado(int dni_bd)
        {
            List<Apoderados> listaApoderado = new List<Apoderados>();


            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();

                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Apoderado_One_Get";
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@dni", dni_bd);


                //idApoderado, nombre, apellido, dni, genero, edad, telefono, email, idRango
                try
                {
                    SqlDataReader dr = sqlComm.ExecuteReader();
                    while (dr.Read())
                    {
                        int idApoderado = dr.GetInt32(0);
                        string nombre = dr.GetString(1);
                        string apellido = dr.GetString(2);
                        int dni = dr.GetInt32(3);
                        string genero = dr.GetString(4);
                        int edad = dr.GetInt32(5);
                        int telefono = dr.GetInt32(6);
                        string email = dr.GetString(7);
                        int idRango = dr.GetInt32(8);

                        Apoderados apoderados = new Apoderados(idApoderado, nombre, apellido, dni, genero, edad, telefono, email, idRango);
                        listaApoderado.Add(apoderados);
                    }

                    dr.Close();
                    conec.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            return listaApoderado;
        }


        //GET_ONE_x_DNI
        public List<Apoderados> Get_One_Apoderado_x_DNI(int dni_bd)
        {
            List<Apoderados> listaApoderado = new List<Apoderados>();


            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();

                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Apoderado_One_Get_x_DNI";
                sqlComm.CommandType = CommandType.StoredProcedure;

                sqlComm.Parameters.AddWithValue("@dni", dni_bd);


                //idApoderado, nombre, apellido, dni, genero, edad, telefono, email, idRango
                try
                {
                    SqlDataReader dr = sqlComm.ExecuteReader();
                    while (dr.Read())
                    {
                        int idApoderado = dr.GetInt32(0);
                        string nombre = dr.GetString(1);
                        string apellido = dr.GetString(2);
                        int dni = dr.GetInt32(3);
                        string genero = dr.GetString(4);
                        int edad = dr.GetInt32(5);
                        int telefono = dr.GetInt32(6);
                        string email = dr.GetString(7);
                        int idRango = dr.GetInt32(8);

                        Apoderados apoderados = new Apoderados(idApoderado, nombre, apellido, dni, genero, edad, telefono, email, idRango);
                        listaApoderado.Add(apoderados);
                    }

                    dr.Close();
                    conec.Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            return listaApoderado;
        }


        //GET ONE ID

        



        //UPDATE
        public Response Update_Apoderado(int id, Apoderados apoderados)
        {
            List<Estudiante> estudianteList = new List<Estudiante>();
            Response resp = new Response();
            List<SubClases> listaMandada = new List<SubClases>();
            Funciones funciones = new Funciones();
            List<EstudianteMostrar> listaEstudianteMostrar = new List<EstudianteMostrar>();
            Response Lara = new Response();

            List<Apoderados> listaApoderadoMostrar = new List<Apoderados>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                SqlCommand sqlCommand = conec.CreateCommand();
                sqlCommand.CommandText = "Apoderado_Update";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idApoderado", id);
                sqlCommand.Parameters.AddWithValue("@nombre", apoderados.nombre);
                sqlCommand.Parameters.AddWithValue("@apellido", apoderados.apellido);
                sqlCommand.Parameters.AddWithValue("@dni", apoderados.dni);
                sqlCommand.Parameters.AddWithValue("@genero", apoderados.genero);
                sqlCommand.Parameters.AddWithValue("@edad", apoderados.edad);
                sqlCommand.Parameters.AddWithValue("@telefono", apoderados.telefono);
                sqlCommand.Parameters.AddWithValue("@email", apoderados.email);
                sqlCommand.Parameters.AddWithValue("@idRango", apoderados.idRango);

                string token = apoderados.token;
                int idApoderado = apoderados.idApoderado;
                string estado = "activo";
                int idRango = apoderados.idRango;
                string nombre = apoderados.nombre;
                string apellido = apoderados.apellido;
                int dni = apoderados.dni;

                string usuario_update = funciones.generarUsuario(nombre, apellido);
                string password_update = funciones.generarPassword(dni);//Aca la contraseña ya esta con hash


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

                        resp = responses;
                        return resp;

                    }
                    listaMandada = funciones.buscarToken(token);

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

                        resp = responses;
                        return resp;
                    }

                    if (sqlCommand.ExecuteNonQuery() >= 1)
                    {
                        Result resultes = new Result()
                        {
                            error_id = "100",
                            error_msg = apoderados.dni.ToString()
                        };

                        Response responses = new Response()
                        {
                            status = "ok",
                            result = resultes
                        };

                        resp = responses;
                    }

                    listaApoderadoMostrar = Get_One_Apoderado_x_DNI(dni);
                    int us_id = listaApoderadoMostrar[0].idApoderado;

                    funciones.Update_Usuario_All(usuario_update, password_update, estado, idRango, us_id);

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                finally
                {
                    sqlCommand.Parameters.Clear();
                    conec.Close();
                }
                return resp;
            }
        }
    }
}