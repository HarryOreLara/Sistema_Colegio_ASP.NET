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

namespace Back_JBG.Models.Gestores.GestorUsuario
{
    public class GestorUsuario
    {
        public Response Add_Usuario(Usuario usuario) 
        {
            string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();
            Security objSecurity = new Security();
            Response usuarioResponse = new Response();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                SqlCommand sqlCommand = conec.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.CommandText = "Usuario_Add";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@usuario", usuario.usuario);
                sqlCommand.Parameters.AddWithValue("@password", objSecurity.EncodePassword(usuario.password));
                sqlCommand.Parameters.AddWithValue("@estado", usuario.estado);
                sqlCommand.Parameters.AddWithValue("@idRango", usuario.idRango);
                sqlCommand.Parameters.AddWithValue("@idPerteneciente", usuario.idPerteneciente);

                try
                {
                    conec.Open();
                    if (usuario.password != usuario.passwordTwo)
                    {
                        Result resultes = new Result()
                        {
                            error_id = "401",
                            error_msg = "Contraseñas Incorrectas"
                        };

                        Response responses = new Response()
                        {
                            status = "error",
                            result = resultes
                        };

                        usuarioResponse = responses;
                        return usuarioResponse;
                    }

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
    }
}