using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Acceso
    {
        string conn;

        public Acceso()
        {
            conn = "Data Source=.;Initial Catalog=dawRefugio;Integrated Security=True";
        }

        public List<T> RetornarLista<T>(string query, Func<SqlDataReader, T> mapFunc, Dictionary<string, object> parametros = null)
        {
            List<T> lista = new List<T>();
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                AsignarParametros(cmd, parametros);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(mapFunc(reader));
                    }
                }
            }
            return lista;
        }

        public void Query(string query, Dictionary<string, object> parametros = null)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                AsignarParametros(cmd, parametros);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public object EjecutarEscalar(string query, Dictionary<string, object> parametros = null)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                AsignarParametros(cmd, parametros);
                connection.Open();
                return cmd.ExecuteScalar();
            }
        }

        private void AsignarParametros(SqlCommand cmd, Dictionary<string, object> parametros)
        {
            cmd.Parameters.Clear();
            if (parametros != null)
            {
                foreach (var parametro in parametros)
                {
                    cmd.Parameters.AddWithValue(parametro.Key, parametro.Value ?? DBNull.Value);
                }
            }
        }
    }
}
