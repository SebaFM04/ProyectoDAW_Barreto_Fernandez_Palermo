using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class bllAnimal
    {
        dalAnimal dal;

        public bllAnimal()
        {
            dal = new dalAnimal();
        }

        public void AltaAnimal(int codigoAnimal, string especie, string raza, string nombre, string tamano, string sexo, string estadoDeAdopcion, bool vivo)
        {

            if (string.IsNullOrEmpty(especie) || string.IsNullOrWhiteSpace(especie)) throw new Exception("Ingrese la especie");
            if (string.IsNullOrEmpty(raza) || string.IsNullOrWhiteSpace(raza)) throw new Exception("Ingrese la raza");
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrWhiteSpace(nombre)) throw new Exception("Ingrese la nombre");
            if (string.IsNullOrEmpty(tamano) || string.IsNullOrWhiteSpace(tamano)) throw new Exception("Ingrese la tamaño");
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrWhiteSpace(sexo)) throw new Exception("Ingrese la sexo");
            if (string.IsNullOrEmpty(estadoDeAdopcion) || string.IsNullOrWhiteSpace(estadoDeAdopcion)) throw new Exception("Ingrese la estado");

            Animal animal = new Animal(codigoAnimal + 1, especie, raza, nombre, tamano, sexo, estadoDeAdopcion, vivo);
            dal.Alta(animal);
        }

        public void Modificar(string codigo, string especie = null, string raza = null, string nombre = null, string tamano = null, string sexo = null, string estadoDeAdopcion = null, bool? vivo = null)
        {
            if (string.IsNullOrEmpty(especie) || string.IsNullOrWhiteSpace(especie)) throw new Exception("Ingrese la especie");
            if (string.IsNullOrEmpty(raza) || string.IsNullOrWhiteSpace(raza)) throw new Exception("Ingrese la raza");
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrWhiteSpace(nombre)) throw new Exception("Ingrese la nombre");
            if (string.IsNullOrEmpty(tamano) || string.IsNullOrWhiteSpace(tamano)) throw new Exception("Ingrese la tamaño");
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrWhiteSpace(sexo)) throw new Exception("Ingrese la sexo");
            if (string.IsNullOrEmpty(estadoDeAdopcion) || string.IsNullOrWhiteSpace(estadoDeAdopcion)) throw new Exception("Ingrese la estado");

            Animal animal = BuscarAnimalPorCodigo(codigo);

            if (animal == null) throw new Exception("No se encontró un animal con el código proporcionado.");

            animal.especie = especie;
            animal.raza = raza;
            animal.nombre = nombre;
            animal.tamaño = tamano;
            animal.sexo = sexo;
            animal.estadoAdopcion = estadoDeAdopcion;

            dal.Modificar(animal);
        }

        public void Baja(string codigo)
        {
            if (VerificarAnimalAdoptado(codigo)) throw new Exception("No se puede borrar porque esta adoptado");

            dal.Baja(codigo);
        }

        public bool ValidarExistenciaAnimal(string codigo)
        {
            return BuscarAnimalPorCodigo(codigo) != null;
        }

        public bool VerificarAnimalAdoptado(string codigo)
        {
            return BuscarAnimalPorCodigo(codigo).estadoAdopcion == "Adoptado";
        }

        public string RetornarEstadoDelAnimal(string codigo)
        {
            return BuscarAnimalPorCodigo(codigo).estadoAdopcion;
        }

        public bool VerificarAnimalVivo(string codigo)
        {
            return BuscarAnimalPorCodigo(codigo).vivo;
        }

        public Animal BuscarAnimalPorCodigo(string codigo)
        {
            return dal.ObtenerAnimalPorCodigo(codigo);
        }

        public List<Animal> RetornarAnimales()
        {
            return dal.RetornarAnimal();
        }
    }
}
