using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using SERVICIOS;

namespace BLL
{
    public class bllAnimal
    {
        dalAnimal dal;
        bllBitacora bllBitacora;
        bllDigitoVerificador bllDigitoVerificador;

        public bllAnimal()
        {
            dal = new dalAnimal();
            bllBitacora = new bllBitacora();
            bllDigitoVerificador = new bllDigitoVerificador();
        }

        public void AltaAnimal(string especie, string raza, string nombre, string tamano, string sexo, string estadoDeAdopcion, bool vivo)
        {
            if (string.IsNullOrWhiteSpace(especie)) throw new Exception("Ingrese la especie");
            if (string.IsNullOrWhiteSpace(raza)) throw new Exception("Ingrese la raza");
            if (string.IsNullOrWhiteSpace(nombre)) throw new Exception("Ingrese el nombre");
            if (string.IsNullOrWhiteSpace(tamano)) throw new Exception("Ingrese el tamaño");
            if (string.IsNullOrWhiteSpace(sexo)) throw new Exception("Ingrese el sexo");
            if (string.IsNullOrWhiteSpace(estadoDeAdopcion)) throw new Exception("Ingrese el estado");

            int codigoAnimal = dal.GenerarCodigoAnimalUnico();
            Animal animal = new Animal(codigoAnimal, especie, raza, nombre, tamano, sexo, estadoDeAdopcion, vivo);
            dal.Alta(animal);
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion animales", "Animal dado de alta", 2);
            RecalcularDigitoAnimal();
        }

        private void RecalcularDigitoAnimal()
        {
            bllDigitoVerificador.CalcularDVAnimal();
        }

        public void Modificar(string codigo, string especie = null, string raza = null, string nombre = null, string tamano = null, string sexo = null, string estadoDeAdopcion = null, bool? vivo = null)
        {
            if (string.IsNullOrWhiteSpace(especie)) throw new Exception("Ingrese la especie");
            if (string.IsNullOrWhiteSpace(raza)) throw new Exception("Ingrese la raza");
            if (string.IsNullOrWhiteSpace(nombre)) throw new Exception("Ingrese el nombre");
            if (string.IsNullOrWhiteSpace(tamano)) throw new Exception("Ingrese el tamaño");
            if (string.IsNullOrWhiteSpace(sexo)) throw new Exception("Ingrese el sexo");
            if (string.IsNullOrWhiteSpace(estadoDeAdopcion)) throw new Exception("Ingrese el estado");

            Animal animal = BuscarAnimalPorCodigo(codigo);

            if (animal == null) throw new Exception("No se encontró un animal con el código proporcionado.");

            animal.especie = especie;
            animal.raza = raza;
            animal.nombre = nombre;
            animal.tamaño = tamano;
            animal.sexo = sexo;
            animal.estadoAdopcion = estadoDeAdopcion;
            if (vivo != null) animal.vivo = vivo.Value;

            dal.Modificar(animal);
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion animales", "Animal modificado", 2);
            RecalcularDigitoAnimal();
        }

        public void Baja(string codigo)
        {
            if (VerificarAnimalAdoptado(codigo)) throw new Exception("No se puede borrar porque esta adoptado");

            dal.Baja(codigo);
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion animales", "Animal dado de baja", 2);
            RecalcularDigitoAnimal();
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
