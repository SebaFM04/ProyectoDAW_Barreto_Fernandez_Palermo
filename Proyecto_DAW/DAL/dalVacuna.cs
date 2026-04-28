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
                         "(codigoVacuna, codigoAnimal, nombreVacuna, fechaAplicacion, fechaProximaAplicacion) " +
                         "VALUES (@codigoVacuna, @codigoAnimal, @nombreVacuna, @fechaAplicacion, @fechaProximaAplicacion)";
            EjecutarQueryConEntidad(vacuna, query);
        }

        public void Modificar(Vacuna vacuna)
        {
            string query = "UPDATE Vacuna SET codigoAnimal = @codigoAnimal, nombreVacuna = @nombreVacuna, fechaAplicacion = @fechaAplicacion, fechaProximaAplicacion = @fechaProximaAplicacion WHERE codigoVacuna = @codigoVacuna";
           
            var props = new List<string>
            {
                "codigoAnimal", "nombreVacuna", "fechaAplicacion", "fechaProximaAplicacion", "codigoVacuna"
            };      
            EjecutarQueryConEntidad(vacuna, query, props);
        }

        private void EjecutarQueryConEntidad(Vacuna vacuna, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(vacuna, propiedadesIncluir);
            dal.Query(query, parametros);
        }

        public bool ValidarVacuna(string dni)
        {
            string query = "SELECT COUNT(*) FROM Usuario WHERE dni = @dni";
            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni }
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
                Convert.ToInt32(reader["codigoVacuna"]),
                Convert.ToInt32(reader["codigoAnimal"]),
                reader["nombreVacuna"].ToString(),
                Convert.ToDateTime(reader["fechaAplicacion"]),
                Convert.ToDateTime(reader["fechaProximaAplicacion"])
            );
        }
    }
}
