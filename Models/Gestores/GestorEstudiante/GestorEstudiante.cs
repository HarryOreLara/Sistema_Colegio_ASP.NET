using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Back_JBG.Models.Clases.Response;
using Back_JBG.Models.Clases.Seguridad;
using Back_JBG.Models.Clases.Estudiantes;
using Back_JBG.Models.Clases.Auth;
using Back_JBG.Models.Clases.Usuarios;
using Back_JBG.Models.Clases.Funciones;

namespace Back_JBG.Models.Gestores.GestorEstudiante
{
    public class GestorEstudiante
    {
        string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();
        Security objSecurity = new Security();
        Response usuarioResponse = new Response();

        //INSERT
        public Response Add_Estudiante(Estudiante estudiante)
        {
            List<SubClases> listaMandada = new List<SubClases>();
            Response Lara = new Response();


            using (SqlConnection conec = new SqlConnection(conexion))
            {
                SqlCommand sqlCommand = conec.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.CommandText = "Estudiante_Add";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@nombre", estudiante.nombre);
                sqlCommand.Parameters.AddWithValue("@apellido", estudiante.apellido);
                sqlCommand.Parameters.AddWithValue("@dni", estudiante.dni);
                sqlCommand.Parameters.AddWithValue("@edad", estudiante.edad);
                sqlCommand.Parameters.AddWithValue("@direccion", estudiante.direccion);
                sqlCommand.Parameters.AddWithValue("@telefono", estudiante.telefono);
                sqlCommand.Parameters.AddWithValue("@email", estudiante.email);
                sqlCommand.Parameters.AddWithValue("@genero", estudiante.genero);
                sqlCommand.Parameters.AddWithValue("@fechaNacimiento", estudiante.fechaNacimiento);
                sqlCommand.Parameters.AddWithValue("@fechaIngreso", estudiante.fechaIngreso);
                sqlCommand.Parameters.AddWithValue("@fechaSalida", estudiante.fechaSalida);
                sqlCommand.Parameters.AddWithValue("@estado", estudiante.estado);
                sqlCommand.Parameters.AddWithValue("@idGrado", estudiante.idGrado);
                sqlCommand.Parameters.AddWithValue("@idSeccion", estudiante.idSeccion);
                sqlCommand.Parameters.AddWithValue("@idApoderado", estudiante.idApoderado);
                sqlCommand.Parameters.AddWithValue("@idRango", estudiante.idRango);

                string token = estudiante.token;
                //int idEstudiante = estudiante.idEstudiante;
                int idApoderado = estudiante.idApoderado;
                string estado = estudiante.estado;
                int idRango = estudiante.idRango;
                string nombre = estudiante.nombre;
                string apellido = estudiante.apellido;
                int dni = estudiante.dni;

                string usuario_new = generarUsuario(nombre, apellido);
                string password_new = generarPassword(dni);//Aca la contraseña ya esta con hash


                List<EstudianteMostrar> listaEstudianteMostrar = new List<EstudianteMostrar>();

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
                            error_msg = dni.ToString()
                        };

                        Response responses = new Response()
                        {
                            status = "ok",
                            result = resultes
                        };

                        Lara = responses;
                    }

                    listaEstudianteMostrar = Get_Estudiante_Dni(dni);

                    int idPerteneciente = listaEstudianteMostrar[0].idEstudiante;

                    Add_Usuario_Estudiante(usuario_new, password_new, estado, idRango, idPerteneciente);

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



        //Solo busca el token y solo quiere resultados, que si hay algo con el tokjen mas no esta interesado en los valores de los resultados
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

        public Response Add_Usuario_Estudiante(string usuario, string password, string estado, int idRango, int idPerteneciente)
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

        //Aca se une las dos primera palabras y el nombre: OreHarry
        private string generarUsuario(string nombre, string apellido)
        {
            char primeraLetra = primeraLetra_x_palabra(nombre);//----S

            string primerApellido = dividirFraseEnDos(apellido, 0);

            string segundoApellido = dividirFraseEnDos(apellido, 1);


            char segundoApellidoLetra = primeraLetra_x_palabra(segundoApellido);

            string new_user = primeraLetra + primerApellido + segundoApellidoLetra;
            return new_user;
        }

        //Aca se divide el apelldio en las dos primeras Palabras
        private string dividirFraseEnDos(string palabra, int posicion)
        {
            string inputString = palabra;
            string[] palabras = inputString.Split(' ');
            string new_palabra = palabras[posicion];

            return new_palabra;
        }

        private char primeraLetra_x_palabra(string palabra)
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

        private string generarPassword(int dniPassword)//Pasword generada por dni
        {
            //string salt = "jbg";
            string password_new = objSecurity.EncodePassword(dniPassword.ToString());//Contraseña Hasheada

            return password_new;
        }




        //Estudiante_Get_Dni
        public List<EstudianteMostrar> Get_Estudiante_Dni(int dni_bd)
        {
            List<EstudianteMostrar> listaEstudiante = new List<EstudianteMostrar>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Estudiante_Get_Dni";
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@dni", dni_bd);

                SqlDataReader dataReader = sqlComm.ExecuteReader();


                while (dataReader.Read())
                {
                    int idEstudiante = dataReader.GetInt32(0);
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
                    string grado = dataReader.GetString(13);
                    string seccion = dataReader.GetString(14);
                    string apoderado = dataReader.GetString(15);
                    string rango = dataReader.GetString(16);

                    EstudianteMostrar estudianteMostrar = new EstudianteMostrar(idEstudiante, nombre, apellido, dni, edad, direccion, telefono, email, genero, fechaNacimiento,
                        fechaIngreso, fechaSalida, estado, grado, seccion, apoderado, rango);

                    listaEstudiante.Add(estudianteMostrar);

                }
                dataReader.Close();
                conec.Close();
            }
            return listaEstudiante;
        }




        //GET
        public List<EstudianteMostrar> Get_Estudiante()
        {
            List<EstudianteMostrar> listaEstudiante = new List<EstudianteMostrar>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Estudiante_Get";
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataReader dataReader = sqlComm.ExecuteReader();

                while (dataReader.Read())
                {
                    int idEstudiante = dataReader.GetInt32(0);
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
                    string grado = dataReader.GetString(13);
                    string seccion = dataReader.GetString(14);
                    string apoderado = dataReader.GetString(15);
                    string rango = dataReader.GetString(16);

                    EstudianteMostrar estudianteMostrar = new EstudianteMostrar(idEstudiante, nombre, apellido, dni, edad, direccion, telefono, email, genero, fechaNacimiento,
                        fechaIngreso, fechaSalida, estado, grado, seccion, apoderado, rango);

                    listaEstudiante.Add(estudianteMostrar);

                }
                dataReader.Close();
                conec.Close();
            }
            return listaEstudiante;
        }



        //Get Grado_Seccion
        public List<EstudianteMostrar> Get_Estudiante_Gra_Secc(int grado_fron, int seccion_fron)
        {
            List<EstudianteMostrar> listaEstudiante = new List<EstudianteMostrar>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Estudiante_Get_seccion_grado";
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@grado", grado_fron);
                sqlComm.Parameters.AddWithValue("@seccion", seccion_fron);

                SqlDataReader dataReader = sqlComm.ExecuteReader();

                while (dataReader.Read())
                {
                    int idEstudiante = dataReader.GetInt32(0);
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
                    string grado = dataReader.GetString(13);
                    string seccion = dataReader.GetString(14);
                    string apoderado = dataReader.GetString(15);
                    string rango = dataReader.GetString(16);

                    EstudianteMostrar estudianteMostrar = new EstudianteMostrar(idEstudiante, nombre, apellido, dni, edad, direccion, telefono, email, genero, fechaNacimiento,
                        fechaIngreso, fechaSalida, estado, grado, seccion, apoderado, rango);

                    listaEstudiante.Add(estudianteMostrar);

                }
                dataReader.Close();
                conec.Close();
            }
            return listaEstudiante;
        }



        ////GET ONE
        public List<Estudiante> Get_Estudiante_Id(int id)
        {
            List<Estudiante> estudianteList = new List<Estudiante>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Estudiante_Get_Id";
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@id", id);

                SqlDataReader dataReader = sqlComm.ExecuteReader();

                //e.nombre, e.apellido, e.dni, e.edad, e.direccion,e.telefono ,e.email, e.genero, e.nacimiento,
                //e.fechaIngreso, e.fechaSalida, e.estado, e.idGrado, e.idSeccion, a.idApoderado, e.idRango, a.nombre, a.dni
                while (dataReader.Read())
                {
                    string nombre = dataReader.GetString(0);
                    string apellido = dataReader.GetString(1);
                    int dni = dataReader.GetInt32(2);
                    int edad = dataReader.GetInt32(3);
                    string direccion = dataReader.GetString(4);
                    int telefono = dataReader.GetInt32(5);
                    string email = dataReader.GetString(6);
                    string genero = dataReader.GetString(7);
                    DateTime fechaNacimiento = dataReader.GetDateTime(8);
                    DateTime fechaIngreso = dataReader.GetDateTime(9);
                    DateTime fechaSalida = dataReader.GetDateTime(10);
                    string estado = dataReader.GetString(11);
                    int grado = dataReader.GetInt32(12);
                    int seccion = dataReader.GetInt32(13);
                    int idApoderado = dataReader.GetInt32(14);
                    int idRango = dataReader.GetInt32(15);
                    string nombreApoderado = dataReader.GetString(16);
                    int dniApoderado = dataReader.GetInt32(17);


                    Estudiante estudianteMostrar = new Estudiante(nombre, apellido, dni, edad, direccion, telefono, email, genero, fechaNacimiento,
                        fechaIngreso, fechaSalida, estado, grado, seccion, idApoderado, idRango, nombreApoderado, dniApoderado);

                    estudianteList.Add(estudianteMostrar);

                }
            }
            return estudianteList;

        }


        ////UPDATE
        public Response Update_Estudiante(int id, Estudiante estudiante)
        {
            List<Estudiante> estudianteList = new List<Estudiante>();
            Response resp = new Response();
            List<SubClases> listaMandada = new List<SubClases>();
            Funciones funciones = new Funciones();
            List<EstudianteMostrar> listaEstudianteMostrar = new List<EstudianteMostrar>();


            using (SqlConnection conec = new SqlConnection(conexion))
            {

                SqlCommand sqlCommand = conec.CreateCommand();
                sqlCommand.CommandText = "Estudiante_Update";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@idEstudiante", id);
                sqlCommand.Parameters.AddWithValue("@nombre", estudiante.nombre);
                sqlCommand.Parameters.AddWithValue("@apellido", estudiante.apellido);
                sqlCommand.Parameters.AddWithValue("@dni", estudiante.dni);
                sqlCommand.Parameters.AddWithValue("@edad", estudiante.edad);
                sqlCommand.Parameters.AddWithValue("@direccion", estudiante.direccion);
                sqlCommand.Parameters.AddWithValue("@telefono", estudiante.telefono);
                sqlCommand.Parameters.AddWithValue("@email", estudiante.email);
                sqlCommand.Parameters.AddWithValue("@genero", estudiante.genero);
                sqlCommand.Parameters.AddWithValue("@nacimiento", estudiante.fechaNacimiento);
                sqlCommand.Parameters.AddWithValue("@fechaIngreso", estudiante.fechaIngreso);
                sqlCommand.Parameters.AddWithValue("@fechaSalida", estudiante.fechaSalida);
                sqlCommand.Parameters.AddWithValue("@estado", estudiante.estado);
                sqlCommand.Parameters.AddWithValue("@idGrado", estudiante.idGrado);
                sqlCommand.Parameters.AddWithValue("@idSeccion", estudiante.idSeccion);
                sqlCommand.Parameters.AddWithValue("@idApoderado", estudiante.idApoderado);
                sqlCommand.Parameters.AddWithValue("@idRango", estudiante.idRango);


                string token = estudiante.token;
                int idApoderado = estudiante.idApoderado;
                string estado = estudiante.estado;
                int idRango = estudiante.idRango;
                string nombre = estudiante.nombre;
                string apellido = estudiante.apellido;
                int dni = estudiante.dni;

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
                            error_msg = estudiante.dni.ToString()
                        };

                        Response responses = new Response()
                        {
                            status = "ok",
                            result = resultes
                        };


                        resp = responses;
                    }

                    listaEstudianteMostrar = Get_Estudiante_Dni(dni);//El metodo trae al estudiante actualizado por su dni y lo asigna a la variable


                    int idPerteneciente = listaEstudianteMostrar[0].idEstudiante;//aca se encuentra la id que fue buscada por el dni del, esta es la id del estudiante actualizado, pero no del usuario
                    funciones.Update_Usuario_All(usuario_update, password_update, estado, idRango, idPerteneciente);



                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                finally
                {
                    sqlCommand.Parameters.Clear();
                    conec.Close();
                }


            }
            return resp;

        }



    }



}