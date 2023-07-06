using Back_JBG.Models.Clases.Response;
using Back_JBG.Models.Clases.Seguridad;
using Back_JBG.Models.Clases.Usuarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Gestores.GestorAuth
{
    public class GestorAuth
    {
        public Response GetResponse(string usuario, string password)
        {
            Response Lara = new Response();
            int us_Id = 0;
            string us_correo = "";
            string us_password = "";
            string us_estado = "";
            int us_idRango = 0;
            int us_idPerteneciente = 0;

            string token_str;

            //LISTA A EVALUAR
            List<Usuario> lista = new List<Usuario>();
            Security objSecurity = new Security();


            string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlCommand sqlCommand = conn.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.CommandText = "Usuario_x_Correo";//buscara dentor de la tabla usuario cuando se cumpla el correo
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@usuario", usuario);

                try
                {
                    conn.Open();
                    SqlDataReader dr = sqlCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        int idUsuario = dr.GetInt32(0);
                        string usuario_bd = dr.GetString(1).Trim();
                        string password_bd = dr.GetString(2).Trim();
                        string estado = dr.GetString(3).Trim();
                        int idRango = dr.GetInt32(4);
                        int idPerteneciente = dr.GetInt32(5);

                        Usuario usuario1 = new Usuario(idUsuario, usuario_bd, password_bd, estado, idRango, idPerteneciente);
                        lista.Add(usuario1);
                        //guardarUsuarioToken(Usuario_Token usuario_Token, lista);
                    }
                    dr.Close();
                    conn.Close();

                    foreach (var obj in lista)
                    {
                        us_Id = obj.idUsuario;
                        us_correo = obj.usuario;
                        us_password = obj.password;
                        us_estado = obj.estado;
                        us_idRango = obj.idRango;
                        us_idPerteneciente = obj.idPerteneciente;
                    }

                    if (us_correo.Length != 0 || password.Length != 0)
                    {
                        string contraHash = objSecurity.EncodePassword(password);
                        if (usuario == us_correo && contraHash == us_password)
                        {

                            token_str = add_usuarioTOken(us_Id, us_estado);//aca asigna el token a la variable, el token puro

                            Result resultes = new Result()
                            {
                                error_id = "100",
                                error_msg = token_str
                            };

                            Response responses = new Response()
                            {
                                status = "ok",
                                result = resultes
                            };

                            Lara = responses;
                        }
                        else
                        {
                            //condicion = "Correo o Contraseña Incorrectos";//error200
                            Result resultes = new Result()
                            {
                                error_id = "200",
                                error_msg = "Datos Incorrectos"
                            };

                            Response error200 = new Response()
                            {
                                status = "error",
                                result = resultes
                            };

                            Lara = error200;
                        }

                    }
                    else
                    {
                        //condicion = "Contraseña o correo Vacios";//error400
                        Result resultes = new Result()
                        {
                            error_id = "400",
                            error_msg = "Datos Incompletos o con formato incorrecto"
                        };

                        Response error400 = new Response()
                        {
                            status = "error",
                            result = resultes
                        };
                        Lara = error400;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            return Lara;
        }
        public string add_usuarioTOken(int id_usuario, string estado_usuario)
        {
            string token_string;
            string token = "";
            string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();


            using (SqlConnection conec = new SqlConnection(conexion))
            {
                SqlCommand sqlCommand = conec.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.CommandText = "UsuarioToken_Add";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idUsuario", id_usuario);
                sqlCommand.Parameters.AddWithValue("@token", token_string = Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
                sqlCommand.Parameters.AddWithValue("@estado", estado_usuario);
                //sqlCommand.Parameters.AddWithValue("@Fecha", DateTime.Now.ToLongTimeString());
                sqlCommand.Parameters.AddWithValue("@fecha", DateTime.Now.ToString());
                try
                {
                    conec.Open();
                    sqlCommand.ExecuteNonQuery();
                    token = token_string;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    token = "Error";
                    throw;
                }
                finally
                {
                    sqlCommand.Parameters.Clear();
                    conec.Close();
                }
            }
            return token;
        }
    }
}