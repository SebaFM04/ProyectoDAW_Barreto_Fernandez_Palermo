using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalFichaDeIngreso
    {
        Acceso dal;

        public dalFichaDeIngreso()
        {
            dal = new Acceso();
        }

        public int GenerarCodigoFichaUnico()
        {
            Random random = new Random();
            int codigo;
            bool existe;
            do
            {
                codigo = random.Next(10000, 99999);
                string query = "SELECT COUNT(*) FROM FichaDeIngreso WHERE codigoFicha = @codigoFicha";
                var parametros = new Dictionary<string, object> { { "@codigoFicha", codigo } };
                object resultado = dal.EjecutarEscalar(query, parametros);
                existe = Convert.ToInt32(resultado) > 0;
            }
            while (existe);
            return codigo;
        }

        public List<FichaDeIngreso> RetornarTodas()
        {
            return dal.RetornarLista("SELECT * FROM FichaDeIngreso", MapearFicha);
        }

        public void Alta(FichaDeIngreso ficha)
        {
            string query = "INSERT INTO FichaDeIngreso (codigoFicha, codigoAnimal, fecha) " +
                            "VALUES (@codigoFicha, @codigoAnimal, @fecha)";
            EjecutarQueryConEntidad(ficha, query);
        }

        public FichaDeIngreso ObtenerFichaPorCodigoAnimal(int codigoAnimal)
        {
            string query = "SELECT * FROM FichaDeIngreso WHERE codigoAnimal = @codigoAnimal";
            var parametros = new Dictionary<string, object>
            {
                { "@codigoAnimal", codigoAnimal }
            };
            var fichas = dal.RetornarLista(query, MapearFicha, parametros);
            return fichas.FirstOrDefault();
        }

        private void EjecutarQueryConEntidad(FichaDeIngreso ficha, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(ficha, propiedadesIncluir);
            dal.Query(query, parametros);
        }

        private FichaDeIngreso MapearFicha(SqlDataReader reader)
        {
            return new FichaDeIngreso(
                Convert.ToInt32(reader["codigoFicha"]),
                Convert.ToInt32(reader["codigoAnimal"]),
                Convert.ToDateTime(reader["fecha"])
            );
        }
    }
}
