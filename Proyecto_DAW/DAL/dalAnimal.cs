using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BE;
using System.Threading.Tasks;

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
            string query = "INSERT INTO Animal " +
                         "(codigoAnimal, especie, raza, nombre, tamaño, sexo, estadoAdopcion, vivo) " +
                         "VALUES (@codigoAnimal, @especie, @raza, @nombre, @tamaño,@sexo, @estadoAdopcion, @vivo)";
            EjecutarQueryConEntidad(animal, query);
        }

        public int GenerarCodigoAnimalUnico()
        {
            Random random = new Random();
            int codigo;
            bool existe;
            do
            {
                codigo = random.Next(10000, 99999); // 5 dígitos
                string query = "SELECT COUNT(*) FROM Animal WHERE codigoAnimal = @codigoAnimal";
                var parametros = new Dictionary<string, object> { { "@codigoAnimal", codigo } };
                object resultado = dal.EjecutarEscalar(query, parametros);
                existe = Convert.ToInt32(resultado) > 0;
            }
            while (existe);
            return codigo;
        }

        public void Modificar(Animal animal)
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
