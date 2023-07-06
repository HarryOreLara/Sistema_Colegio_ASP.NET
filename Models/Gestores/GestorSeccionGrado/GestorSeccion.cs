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
    public class GestorSeccion
    {
        //Get_Seccion_All
        string conexion = ConfigurationManager.ConnectionStrings["BDLocal"].ToString();

        Response usuarioResponse = new Response();

        public List<Seccion> Get_Seccion_All()
        {
            List<Seccion> listaSecciones = new List<Seccion>();

            using (SqlConnection conec = new SqlConnection(conexion))
            {
                conec.Open();
                SqlCommand sqlComm = conec.CreateCommand();
                sqlComm.CommandText = "Get_Seccion_All";
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataReader dataReader = sqlComm.ExecuteReader();

                while (dataReader.Read())
                {
                    int idSeccion = dataReader.GetInt32(0);
                    string nombreSeccion = dataReader.GetString(1);

                    Seccion seccion = new Seccion(idSeccion, nombreSeccion);
                    listaSecciones.Add(seccion);
                }
                dataReader.Close();
                conec.Close();


            }
            return listaSecciones;
        }
    }
}