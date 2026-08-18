using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class dalAdoptante
    {
        Acceso dal;

        public dalAdoptante()
        {
            dal = new Acceso();
        }

        public void Alta(Adoptante adoptante)
        {
            string query = "INSERT INTO Adoptante " +
                         "(dni, nombre, apellido, telefono, edad,domicilio,mascotas, activo ) " +
                         "VALUES (@dni, @nombre, @apellido, @telefono, @edad, @domicilio, @mascotas, @activo)";
            EjecutarQueryConEntidad(adoptante, query);
        }

        public void Modificar(Adoptante adoptante)
        {
            string query = "UPDATE Adoptante SET nombre = @nombre, apellido = @apellido,  telefono = @telefono,edad = @edad,domicilio = @domicilio, mascotas = @mascotas, activo = @activo WHERE dni = @dni";
            // Lista de propiedades usadas en la consulta
            var props = new List<string>
            {
                "nombre",
                "apellido",
                "telefono",
                "edad",
                "domicilio",
                "mascotas",
                "activo",
                "dni"
            };

            EjecutarQueryConEntidad(adoptante, query, props);
        }

        private void EjecutarQueryConEntidad(Adoptante adoptante, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(adoptante, propiedadesIncluir);
            dal.Query(query, parametros);
        }

        public bool ValidarDni(string dni)
        {
            string query = "SELECT COUNT(*) FROM Adoptante WHERE dni = @dni";
            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni }
            };
            int count = Convert.ToInt32(dal.EjecutarEscalar(query, parametros));
            return count > 0;
        }

        public Adoptante ObtenerAdoptantePorDni(string dni)
        {
            string query = "SELECT * FROM Adoptante WHERE dni = @dni";
            var parametros = new Dictionary<string, object>
            {
                { "@dni", dni }
            };
            var adoptante = dal.RetornarLista(query, MapearAdoptante, parametros);
            return adoptante.FirstOrDefault();
        }

        public List<Adoptante> RetornarAdoptantes()
        {
            List<Adoptante> adoptante = dal.RetornarLista("SELECT * FROM Adoptante", MapearAdoptante);
            return adoptante;
        }

        private Adoptante MapearAdoptante(SqlDataReader reader)
        {

            return new Adoptante(
                reader["dni"].ToString(),
                reader["nombre"].ToString(),
                reader["apellido"].ToString(),
                reader["telefono"].ToString(),
                Convert.ToInt32(reader["edad"]),
                reader["domicilio"].ToString(),
                Convert.ToBoolean(reader["mascotas"]),
                Convert.ToBoolean(reader["activo"])
            );
        }
    }
}
