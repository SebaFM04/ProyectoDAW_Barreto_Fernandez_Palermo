using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class dalAnimal
    {
        Acceso dal;

        public dalAnimal()
        {
            dal = new Acceso();
        }

        public void Alta(Animal animal)
        {
            try
            {
                string query = "INSERT INTO Animal " +
                             "(codigoAnimal, especie, raza, nombre, tamaño, sexo, estadoAdopcion, vivo) " +
                             "VALUES (@codigoAnimal, @especie, @raza, @nombre, @tamaño,@sexo, @estadoAdopcion, @vivo)";

                EjecutarQueryConEntidad(animal, query);

            }
            catch (Exception ex) { throw new Exception("Error al guardar animal en la BD: " + ex.Message); }
        }

        public void Modificar(Animal animal)
        {
            try
            {
                string query = "UPDATE Animal SET especie = @especie, raza = @raza,nombre = @nombre, tamaño = @tamaño, sexo = @sexo, estadoAdopcion = @estadoAdopcion, vivo = @vivo" +
                    " WHERE codigoAnimal = @codigoAnimal";

                var propiedadesAIncluir = new List<string>
                {
                    "especie",
                    "raza",
                    "nombre",
                    "tamaño",
                    "sexo",
                    "estadoAdopcion",
                    "vivo",
                    "codigoAnimal"
                };

                EjecutarQueryConEntidad(animal, query, propiedadesAIncluir);
            }
            catch (Exception ex) { throw new Exception("Error al actualizar animal en la BD: " + ex.Message); }
        }

        public void Baja(string codigo)
        {
            try
            {
                string query = "DELETE FROM Animal " +
                               "WHERE codigoAnimal=@codigoAnimal";

                var parametros = new Dictionary<string, object>
                {
                    { "@codigoAnimal", codigo }
                };

                dal.Query(query, parametros);
            }
            catch (Exception ex) { throw new Exception("Error al eliminar animal en la BD: " + ex.Message); }
        }

        private void EjecutarQueryConEntidad(Animal animal, string query, List<string> propiedadesIncluir = null)
        {
            Dictionary<string, object> parametros = ParametroHelper.CrearParametros(animal, propiedadesIncluir);
            dal.Query(query, parametros);
        }

        public Animal ObtenerAnimalPorCodigo(string codigo)
        {
            string query = "SELECT * FROM Animal WHERE codigoAnimal = @codigoAnimal";
            
            var parametros = new Dictionary<string, object>
            {
                { "@codigoAnimal", codigo }
            };

            var animales = dal.RetornarLista(query, MapearAnimal, parametros);

            return animales.FirstOrDefault();
        }

        public List<Animal> RetornarAnimal()
        {
            List<Animal> animal = dal.RetornarLista("SELECT * FROM Animal", MapearAnimal);
            return animal;
        }

        private Animal MapearAnimal(SqlDataReader reader)
        {
            return new Animal(

                Convert.ToInt32(reader["codigoAnimal"]),
                reader["especie"].ToString(),
                reader["raza"].ToString(),
                reader["nombre"].ToString(),
                reader["tamaño"].ToString(),
                reader["sexo"].ToString(),
                reader["estadoAdopcion"].ToString(),
                Convert.ToBoolean(reader["vivo"])
            );
        }
    }
}
