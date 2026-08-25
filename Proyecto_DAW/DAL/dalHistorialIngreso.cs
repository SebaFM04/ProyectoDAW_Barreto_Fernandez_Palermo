using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalHistorialIngreso
    {
        Acceso dal;

        public dalHistorialIngreso()
        {
            dal = new Acceso();
        }

        public int GenerarCodigoHistorialUnico()
        {
            Random random = new Random();
            int codigo;
            bool existe;
            do
            {
                codigo = random.Next(10000, 99999);
                string query = "SELECT COUNT(*) FROM HistorialIngreso WHERE codigoHistorial = @codigoHistorial";
                var parametros = new Dictionary<string, object> { { "@codigoHistorial", codigo } };
                object resultado = dal.EjecutarEscalar(query, parametros);
                existe = Convert.ToInt32(resultado) > 0;
            }
            while (existe);
            return codigo;
        }

        public void Alta(HistorialIngreso historial)
        {
            string query = "INSERT INTO HistorialIngreso (codigoHistorial, codigoFicha, fecha, motivo) " +
                            "VALUES (@codigoHistorial, @codigoFicha, @fecha, @motivo)";
            EjecutarQueryConEntidad(historial, query);
        }

        public List<HistorialIngreso> RetornarHistorialPorFicha(int codigoFicha)
        {
            string query = "SELECT * FROM HistorialIngreso WHERE codigoFicha = @codigoFicha ORDER BY fecha DESC";
            var parametros = new Dictionary<string, object>
            {
                { "@codigoFicha", codigoFicha }
            };
            return dal.RetornarLista(query, MapearHistorial, parametros);
        }

        private void EjecutarQueryConEntidad(HistorialIngreso historial, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(historial, propiedadesIncluir);
            dal.Query(query, parametros);
        }

        private HistorialIngreso MapearHistorial(SqlDataReader reader)
        {
            return new HistorialIngreso(
                Convert.ToInt32(reader["codigoHistorial"]),
                Convert.ToInt32(reader["codigoFicha"]),
                Convert.ToDateTime(reader["fecha"]),
                reader["motivo"].ToString()
            );
        }
    }
}
