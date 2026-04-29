using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalVacuna
    {
        Acceso dal;

        public dalVacuna()
        {
            dal = new Acceso();
        }

        public void Alta(Vacuna vacuna)
        {
            string query = "INSERT INTO Vacuna " +
                         "(codigoVacuna, nombreVacuna) " +
                         "VALUES (@codigoVacuna, @nombreVacuna)";
            EjecutarQueryConEntidad(vacuna, query);
        }

        public void Modificar(Vacuna vacuna)
        {
            string query = "UPDATE Vacuna SET  nombreVacuna = @nombreVacuna WHERE codigoVacuna = @codigoVacuna";
           
            var props = new List<string>
            {
                "nombreVacuna", "codigoVacuna"
            };      
            EjecutarQueryConEntidad(vacuna, query, props);
        }

        private void EjecutarQueryConEntidad(Vacuna vacuna, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(vacuna, propiedadesIncluir);
            dal.Query(query, parametros);
        }

        public bool ValidarVacuna(string codigoVacuna)
        {
            string query = "SELECT COUNT(*) FROM Vacuna WHERE codigoVacuna = @codigoVacuna";
            var parametros = new Dictionary<string, object>
            {
                { "@codigoVacuna", codigoVacuna }
            };
            int count = Convert.ToInt32(dal.EjecutarEscalar(query, parametros));
            return count > 0;
        }

        public Vacuna ObtenerVacuna(string codigoVacuna)
        {
            string query = "SELECT * FROM Vacuna WHERE codigoVacuna = @codigoVacuna";
            var parametros = new Dictionary<string, object>
            {
                { "@codigoVacuna", codigoVacuna }
            };
            var vacunas = dal.RetornarLista(query, MapearVacuna, parametros);
            return vacunas.FirstOrDefault();
        }

        public List<Vacuna> RetornarVacunas()
        {
            List<Vacuna> vacunas = dal.RetornarLista("SELECT * FROM Vacuna", MapearVacuna);
            return vacunas;
        }

        private Vacuna MapearVacuna(SqlDataReader reader)
        {
            return new Vacuna(
                reader["codigoVacuna"].ToString(),
                reader["nombreVacuna"].ToString()
            );
        }
    }
}
