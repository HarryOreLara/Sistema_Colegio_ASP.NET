using Back_JBG.Models.Clases.Meterias;
using Back_JBG.Models.Clases.Response;
using Back_JBG.Models.Clases.Seguridad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Gestores.GestorMateria
{
    public class GestorMateria
    {
        string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();
        Security objSecurity = new Security();
        Response usuarioResponse = new Response();

        //GET ALL
        public List<Materia> Get_All_Materia()
        {
            List<Materia> listaMaterias = new List<Materia>();
            using (SqlConnection conec = new SqlConnection(conexion))
            {
                try
                {
                    conec.Open();
                    SqlCommand sqlCommand = conec.CreateCommand();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlCommand.CommandText = "Get_All_Materias";
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    SqlDataReader dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        int idMateria = dataReader.GetInt32(0);
                        string nombre = dataReader.GetString(1);


                        Materia materia = new Materia(idMateria, nombre);

                        listaMaterias.Add(materia);

                    }
                    dataReader.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {

                    conec.Close();
                }
            }
            return listaMaterias;
        }
        //GET ONE
        //INSERT
        //UPDATE
        //DELETE
    }
}