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

        public bllAnimal()
        {
            dal = new dalAnimal();
            bllBitacora = new bllBitacora();
        }

        public void AltaAnimal(string especie, string raza, string nombre, string tañamo, string sexo, string estadoDeAdopcion, bool vivo)
        {
            int codigoAnimal = dal.GenerarCodigoAnimalUnico();
            Animal animal = new Animal(codigoAnimal, especie, raza, nombre, tañamo, sexo, estadoDeAdopcion, vivo);
            dal.Alta(animal);
            bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion animales", "Animal dado de alta", 2);
        }

        public void Modificar(string codigo, string especie = null, string raza = null, string nombre = null, string tamaño = null, string sexo = null, string estadoDeAdopcion = null, bool? vivo = null)
        {
            Animal animal = BuscarAnimalPorCodigo(codigo);
            if (animal == null)
            {
                throw new Exception("No se encontró un animal con el código proporcionado.");
            }

            if (especie != null) animal.especie = especie;
            if (raza != null) animal.raza = raza;
            if (nombre != null) animal.nombre = nombre;
            if (tamaño != null) animal.tamaño = tamaño;
            if (sexo != null) animal.sexo = sexo;
            if (estadoDeAdopcion != null) animal.estadoAdopcion = estadoDeAdopcion;
            if (vivo != null)
            {
                animal.vivo = vivo.Value;
                bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion animales", "Animal dado de baja", 2);
            }
            else
            {
                bllBitacora.Alta(claseSession.Gestor.RetornarUsuarioSession().nombreUsuario, "Gestion animales", "Animal modificado", 2);
            }

            dal.Modificar(animal);
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
