using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalFichaMedica
    {
        Acceso dal;

        public dalFichaMedica()
        {
            dal = new Acceso();
        }

        public int GenerarCodigoFichaMedicaUnico()
        {
            Random random = new Random();
            int codigo;
            bool existe;
            do
            {
                codigo = random.Next(10000, 99999);
                string query = "SELECT COUNT(*) FROM FichaMedica WHERE codigo = @codigo";
                var parametros = new Dictionary<string, object> { { "@codigo", codigo } };
                object resultado = dal.EjecutarEscalar(query, parametros);
                existe = Convert.ToInt32(resultado) > 0;
            }
            while (existe);
            return codigo;
        }

        public void Alta(FichaMedica ficha)
        {
            string query = "INSERT INTO FichaMedica (codigo, codigoAnimal, fecha, castrado, dieta, medicamento, observaciones) " +
                            "VALUES (@codigo, @codigoAnimal, @fecha, @castrado, @dieta, @medicamento, @observaciones)";
            EjecutarQueryConEntidad(ficha, query);
        }

        public List<FichaMedica> RetornarFichasPorAnimal(int codigoAnimal)
        {
            string query = "SELECT * FROM FichaMedica WHERE codigoAnimal = @codigoAnimal ORDER BY fecha DESC";
            var parametros = new Dictionary<string, object>
            {
                { "@codigoAnimal", codigoAnimal }
            };
            return dal.RetornarLista(query, MapearFichaMedica, parametros);
        }

        public FichaMedica ObtenerUltimaFichaPorAnimal(int codigoAnimal)
        {
            return RetornarFichasPorAnimal(codigoAnimal).FirstOrDefault(); // ya viene ordenado DESC por fecha
        }

        private void EjecutarQueryConEntidad(FichaMedica ficha, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(ficha, propiedadesIncluir);
            dal.Query(query, parametros);
        }

        private FichaMedica MapearFichaMedica(SqlDataReader reader)
        {
            return new FichaMedica(
                Convert.ToInt32(reader["codigo"]),
                Convert.ToInt32(reader["codigoAnimal"]),
                Convert.ToDateTime(reader["fecha"]),
                Convert.ToBoolean(reader["castrado"]),
                reader["dieta"] == DBNull.Value ? null : reader["dieta"].ToString(),
                reader["medicamento"] == DBNull.Value ? null : reader["medicamento"].ToString(),
                reader["observaciones"] == DBNull.Value ? null : reader["observaciones"].ToString()
            );
        }
    }
}
