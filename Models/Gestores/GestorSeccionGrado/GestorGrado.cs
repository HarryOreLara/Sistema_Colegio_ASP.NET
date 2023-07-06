using Back_JBG.Models.Clases.GradosSecciones;
using Back_JBG.Models.Clases.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Back_JBG.Models.Gestores.GestorSeccionGrado
{
    public class GestorGrado
    {
        string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();

        Response usuarioResponse = new Response();

        public List<Grado> Get_Grado_All()
        {
            List<Grado> listaGrados = new List<Grado>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Get_Grado_All";
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataReader dataReader = sqlComm.ExecuteReader();

                while (dataReader.Read())
                {
                    int idGrado = dataReader.GetInt32(0);
                    string nombreGrado = dataReader.GetString(1);

                    Grado grado = new Grado(idGrado, nombreGrado);
                    listaGrados.Add(grado);
                }
                dataReader.Close();
                conec.Close();


            }
            return listaGrados;
        }
    }
}