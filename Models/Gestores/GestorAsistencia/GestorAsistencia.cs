using Back_JBG.Models.Clases.Asistencias;
using Back_JBG.Models.Clases.Auth;
using Back_JBG.Models.Clases.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using IronPython.Runtime;
using IronPython.Modules;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;

namespace Back_JBG.Models.Gestores.GestorAsistencia
{

    public class GestorAsistencia
    {
        string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();


        public async Task<Response> Insert_Asistencia(List<AsistenciaPrincipal> asistenciaPrincipal)
        {
            List<SubClases> listaMandada = new List<SubClases>();
            Response Lara = new Response();

            List<AsistenciaPrincipal> listaPrueba = new List<AsistenciaPrincipal>();
            List<int> listaTelefonos = new List<int>();

            List<AsistenciaScript> asistenciaScripts = new List<AsistenciaScript>();
            List<AsistenciaScript> asistenciaScripts2 = new List<AsistenciaScript>();

            int[] vacio = { 1, 1, 1, 1, 1 };

            //Asistencia_Insert
            using (SqlConnection conec = new SqlConnection(conexion))
            {
                int contador = 0;
                conec.Open();
                SqlCommand sqlCommand = conec.CreateCommand();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlCommand.CommandText = "Asistencia_Insert";
                sqlCommand.CommandType = CommandType.StoredProcedure;

                //string token = asistenciaPrincipal[0].token;
                DateTime asistencia = asistenciaPrincipal[0].fechaAsistencia;
                DateTime fechaActual = DateTime.UtcNow;
                string mensaje = "No Enviado";

                try
                {
                    foreach (var assistencia in asistenciaPrincipal)
                    {
                        sqlCommand.Parameters.AddWithValue("@idEstudiante", assistencia.idEstudiante);
                        sqlCommand.Parameters.AddWithValue("@fechaAsistencia", fechaActual);
                        sqlCommand.Parameters.AddWithValue("@presencia", assistencia.presencia);
                        sqlCommand.Parameters.AddWithValue("@mensaje", mensaje);
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Parameters.Clear();
                        contador++;
                    }

                    if (contador >= 1)
                    {


                        Result resultes = new Result()
                        {
                            error_id = "100",
                            error_msg = contador.ToString()
                        };

                        Response responses = new Response()
                        {
                            status = "ok",
                            result = resultes
                        };
                        await enviarMensajeWhatsapAsync();
                        Lara = responses;
                    }
                    else
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


                    asistenciaScripts = Get_Telefonos();//Aca se encuentra el telefono, nombre y apoderado en un array de json listos para mandar al api de whatsap
                    //EjecutarScript(asistenciaScripts);
                    //otroScript(asistenciaScripts);
                    //tresScript(asistenciaScripts);


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


        //Obtiene a los estudiantes mandando por parametro el grado y la seccion de los que son
        public List<Asistencia> Get_Estudiante_GraSec(int grado, int seccion)
        {
            List<Asistencia> listaAsistencias = new List<Asistencia>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Get_Estudiantes_x_GradAndSec";
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@idGrado", grado);
                sqlComm.Parameters.AddWithValue("@idSeccion", seccion);

                SqlDataReader dataReader = sqlComm.ExecuteReader();

                while (dataReader.Read())
                {
                    int idEstudiante = dataReader.GetInt32(0);
                    string nombre = dataReader.GetString(1);
                    string apellido = dataReader.GetString(2);

                    Asistencia asistencia = new Asistencia(idEstudiante, nombre, apellido);
                    listaAsistencias.Add(asistencia);

                }
                dataReader.Close();
                conec.Close();

            }
            return listaAsistencias;
        }


        //Se actualizo el 10-03-2023 en donde ya no solo devuelve un numero, sino devuelve el telefono, el nombre estudiante y el nombre apoderado
        public List<AsistenciaScript> Get_Telefonos()//Esto me retorna a toda la lista de telefonos de los estudiantes que no asistieron
        {
            List<AsistenciaPrincipal> listaAsistencia = new List<AsistenciaPrincipal>();//Aca esta todos los datos de los Estudiantes Y Padres
            List<AsistenciaPrincipal> listaid = new List<AsistenciaPrincipal>();
            DateTime fechaActual = DateTime.UtcNow;

            int tamanio;
            int[] prueba = { 2, 8 };

            List<int> listaIdEstudiante = new List<int>();//Este tiene el id de todos los estudiantes que no asistieron
            List<int> listTelefonos = new List<int>();//Aca esta todos los telefonos de los padres

            List<AsistenciaScript> asistenciaScripts = new List<AsistenciaScript>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();

                listaid = Get_Asistencia_Pres_Msg(fechaActual);//Aca tengo el json de todos los estudiantes sin mensaje y que no asistieron
                tamanio = listaid.Count;//tamaño de la lista

                foreach (var obj in listaid)
                {
                    listaIdEstudiante.Add(obj.idEstudiante);
                }

                //ObtenerAlumnosPorLista
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Get_Telefonos_Apoderados_Estudiantes";
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@Ids", string.Join(",", listaIdEstudiante));


                SqlDataReader dataReader = sqlComm.ExecuteReader();
                while (dataReader.Read())
                {
                    int idEstudiante = dataReader.GetInt32(0);
                    string nombreEstudiante = dataReader.GetString(1);
                    int idApoderado = dataReader.GetInt32(2);
                    string nombreApoderado = dataReader.GetString(3);
                    int telefonoApoderado = dataReader.GetInt32(4);

                    AsistenciaPrincipal asistenciaPrincipal = new AsistenciaPrincipal(idEstudiante, nombreEstudiante, idApoderado, nombreApoderado, telefonoApoderado);
                    listaAsistencia.Add(asistenciaPrincipal);

                }

                dataReader.Close();

                foreach (var obj in listaAsistencia)
                {
                    listTelefonos.Add(obj.telefonoApoderado);
                    AsistenciaScript scripts = new AsistenciaScript(obj.telefonoApoderado, obj.nombreEstudiante, obj.nombreApoderado);
                    asistenciaScripts.Add(scripts);
                }

                conec.Close();

            }
            return asistenciaScripts;

        }

        public List<AsistenciaPrincipal> Get_Asistencia_Pres_Msg(DateTime fecha)//esto devuelve todos los idEstudiante
        {

            List<AsistenciaPrincipal> listaIdEstudiantes = new List<AsistenciaPrincipal>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Get_Asistencia_Fecha_Pres_Msg";
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@fecha", fecha);

                SqlDataReader dataReader = sqlComm.ExecuteReader();
                while (dataReader.Read())
                {
                    int idEstudiante = dataReader.GetInt32(1);
                    AsistenciaPrincipal asistenciaPrincipal = new AsistenciaPrincipal(idEstudiante);
                    listaIdEstudiantes.Add(asistenciaPrincipal);
                }

                dataReader.Close();
                conec.Close();

            }
            return listaIdEstudiantes;
        }


        public List<AsistenciaPrincipal> despliegueAsistencias(DateTime fechaAssitencia, int grado, int seccion)
        {
            List<AsistenciaPrincipal> listAsistenciaPrincipal = new List<AsistenciaPrincipal>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "DespliegeAsistencias";
                sqlComm.CommandType = CommandType.StoredProcedure;
                sqlComm.Parameters.AddWithValue("@fecha", fechaAssitencia);
                sqlComm.Parameters.AddWithValue("@grado", grado);
                sqlComm.Parameters.AddWithValue("@seccion", seccion);

                SqlDataReader dataReader = sqlComm.ExecuteReader();
                while (dataReader.Read())
                {
                    int idEstudiante = dataReader.GetInt32(0);
                    string nombreEstudiante = dataReader.GetString(1);
                    string apellidoEstudiante = dataReader.GetString(2);
                    int dniEstudiante = dataReader.GetInt32(3);
                    string presencia = dataReader.GetString(4);
                    string mensaje = dataReader.GetString(5);

                    AsistenciaPrincipal asistenciaPrincipal = new AsistenciaPrincipal(idEstudiante, nombreEstudiante, apellidoEstudiante, dniEstudiante, presencia, mensaje);
                    listAsistenciaPrincipal.Add(asistenciaPrincipal);
                }

                dataReader.Close();
                conec.Close();

            }
            return listAsistenciaPrincipal;
        }


        public void EjecutarScript(List<AsistenciaScript> asistenciaScripts)
        {
            ScriptEngine engine = Python.CreateEngine();
            var searchPaths = engine.GetSearchPaths();
            searchPaths.Add(@"D:\Proyectos Visual Studio Code\Python\Dark\Scripts");
            engine.SetSearchPaths(searchPaths);
            var scriptSource = engine.CreateScriptSourceFromFile(@"D:\Proyectos Visual Studio 2019\JBG_WEB\msjWhats.py");
            ScriptScope scope = engine.CreateScope();


            var parametros = asistenciaScripts;
            var listaPython = new List<AsistenciaScript>(parametros.Count);
            foreach(var item in parametros)
            {
                listaPython.Add(item);
            }

            scope.SetVariable("params", asistenciaScripts);
            var result = scriptSource.Execute(scope);

        }

        public void otroScript(List<AsistenciaScript> asistenciaScripts)
        {
            string scriptSource = @"D:\Proyectos Visual Studio 2019\JBG_WEB\msjWhats.py";
            ScriptRuntime py = Python.CreateRuntime();
            dynamic pyProgram = py.UseFile(scriptSource);

            pyProgram.enviarMensaje(asistenciaScripts);
        }

        public void tresScript(List<AsistenciaScript> asistenciaScripts)
        {
            var engine = IronPython.Hosting.Python.CreateEngine();
            var scope = engine.CreateScope();
            engine.Execute("import pyautogui", scope);
            engine.Execute("import webbrowser ", scope);
            engine.Execute("import json", scope);
            engine.Execute("import time", scope);
            engine.ExecuteFile(@"D:\Proyectos Visual Studio 2019\JBG_WEB\msjWhats.py", scope);

            var parametros = asistenciaScripts;
            scope.SetVariable("parametros", parametros);
            engine.ExecuteFile(@"D:\Proyectos Visual Studio 2019\JBG_WEB\msjWhats.py", scope);
        }


        //async void EjemploMetodoAsincronico()
        //{
        //    // llamar al método asincrónico y esperar a que se complete
        //    await enviarMensajeWhatsapAsync();

        //    // continuar con el resto del código después de que se complete el método asincrónico
        //}

        async Task enviarMensajeWhatsapAsync()
        {
            //Token
            string token = "EAACksiX1ufoBAPArfeZCJYNJDK4O7ZCHbDNZB1CRObFuZCFlmS4E7OQ4iuu21INflSZAreS4YZAzGz9ezXbGtY8qWZCpUFbMU9jupTtiAQ8SVQgZAnZCX6UK2rXMY0vRivgShSLnZAKSu8997IQepAmH0cqGmioPMpZCC0I04up2RSiHepH91rU2twV9nan58sOBx1HpKAyE5xOgCZCwJeszLccG";
            //Identificador de número de teléfono
            string idTelefono = "108974948786715";
            //Nuestro telefono
            //string telefono = "51933766315";
            string[] telefonos = { "51933766315", "51931070632", "51961479986"};

            for(int i=0; i<telefonos.Length; i++)
            {
                HttpClient client = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v15.0/" + idTelefono + "/messages");

                request.Headers.Add("Authorization", "Bearer " + token);

                request.Content = new StringContent("{ \"messaging_product\": \"whatsapp\", \"to\": \"" + telefonos[i] + "\", \"type\": \"template\", \"template\": { \"name\": \"hello_world\", \"language\": { \"code\": \"en_US\" } } }");
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                //response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
            }


        }
    }
}