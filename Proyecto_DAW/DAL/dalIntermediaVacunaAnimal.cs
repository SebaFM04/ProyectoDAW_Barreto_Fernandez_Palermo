using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalIntermediaVacunaAnimal
    {
        Acceso dal;

        public dalIntermediaVacunaAnimal()
        {
            dal = new Acceso();
        }

        public void Alta(IntermediaVacunaAnimal  intermedia)
        {
            string query = "INSERT INTO IntermediaVacunaAnimal " +
                         "(codigoVacuna, codigoAnimal, nombreVacuna, fechaAplicacion, fechaProximaAplicacion) " +
                         "VALUES (@codigoVacuna, @codigoAnimal, @nombreVacuna, @fechaAplicacion, @fechaProximaAplicacion)";
            EjecutarQueryConEntidad(intermedia, query);
        }

        public void Modificar(IntermediaVacunaAnimal intermedia)
        {
            string query = "UPDATE IntermediaVacunaAnimal SET fechaAplicacion = @fechaAplicacion, fechaProximaAplicacion = @fechaProximaAplicacion WHERE codigo = @codigo";

            var props = new List<string>
            {
                "fechaAplicacion", "fechaProximaAplicacion", "codigo"
            };
            EjecutarQueryConEntidad(intermedia, query, props);
        }

        private void EjecutarQueryConEntidad(IntermediaVacunaAnimal intermedia, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(intermedia, propiedadesIncluir);
            dal.Query(query, parametros);
        }

        public IntermediaVacunaAnimal ObtenerItermediaVacunaAnimal(string codigo)
        {
            string query = "SELECT * FROM IntermediaVacunaAnimal WHERE codigo= @codigo";
            var parametros = new Dictionary<string, object>
            {
                { "@codigo", codigo }
            };
            var intermedia = dal.RetornarLista(query, MapearInterdiaVacunaAnimal, parametros);
            return intermedia.FirstOrDefault();
        }

        public List<IntermediaVacunaAnimal> RetornarIntermediaVacunaAnimal()
        {
            List<IntermediaVacunaAnimal> intermedia = dal.RetornarLista("SELECT * FROM IntermediaVacunaAnimal", MapearInterdiaVacunaAnimal);
            return intermedia;
        }

        private IntermediaVacunaAnimal MapearInterdiaVacunaAnimal(SqlDataReader reader)
        {
            return new IntermediaVacunaAnimal(
                int.Parse(reader["codigo"].ToString()),
                reader["codigo"].ToString(),
                Convert.ToInt32(reader["codigoAnimal"]),
                reader["nombreVacuna"].ToString(),
                Convert.ToDateTime(reader["fechaAplicacion"]),
                Convert.ToDateTime(reader["fechaProximaAplicacion"])
            );
        }
    }
}
