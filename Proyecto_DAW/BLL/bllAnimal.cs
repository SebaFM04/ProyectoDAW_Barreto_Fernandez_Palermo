using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BLL
{
    public class bllAnimal
    {
        dalAnimal dal;
        bllBitacora bllBitacora;
        bllDigitoVerificador bllDigitoVerificador;
        bllFichaDeIngreso bllFichaDeIngreso;

        public bllAnimal()
        {
            dal = new dalAnimal();
            bllBitacora = new bllBitacora();
            bllDigitoVerificador = new bllDigitoVerificador();
            bllFichaDeIngreso = new bllFichaDeIngreso();
        }

        public AccionSql ConstruirAccionMarcarAdoptado(string codigo)
        {
            Animal animal = BuscarAnimalPorCodigo(codigo);
            if (animal == null) throw new Exception("No se encontró un animal con el código proporcionado.");

            animal.estadoAdopcion = "Adoptado";
            return dal.ConstruirAccionModificar(animal);
        }

        public AccionSql ConstruirAccionCambiarEstadoAdopcion(string codigo, string nuevoEstado)
        {
            Animal animal = BuscarAnimalPorCodigo(codigo);
            if (animal == null) throw new Exception("No se encontró un animal con el código proporcionado.");

            animal.estadoAdopcion = nuevoEstado;
            return dal.ConstruirAccionModificar(animal);
        }

        public int AltaAnimal(string especie, string raza, string nombre, string tamano, string sexo, string estadoDeAdopcion, bool vivo)
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

            return codigoAnimal;
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

            if (bllFichaDeIngreso.TieneFicha(int.Parse(codigo))) // <- agregar este bloque
                throw new Exception("No se puede borrar el animal porque tiene una ficha de ingreso con historial.");

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

        public void MarcarComoAdoptado(string codigo)
        {
            Animal animal = BuscarAnimalPorCodigo(codigo);
            if (animal == null) throw new Exception("No se encontró un animal con el código proporcionado.");

            animal.estadoAdopcion = "Adoptado";
            dal.Modificar(animal);
            RecalcularDigitoAnimal();
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

        public string ExportarAnimalesXML()
        {
            List<Animal> animales = dal.RetornarAnimal(); // ajustá al nombre real de tu DAL

            AnimalesXML wrapper = new AnimalesXML();
            wrapper.Listado = animales;

            XmlSerializer serializer = new XmlSerializer(typeof(AnimalesXML));
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, wrapper);
                return sw.ToString();
            }
        }

        public string ImportarAnimalesXML(Stream archivoXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AnimalesXML));
            AnimalesXML wrapper;

            try
            {
                wrapper = (AnimalesXML)serializer.Deserialize(archivoXml);
            }
            catch (InvalidOperationException)
            {
                // Esto salta tanto si el archivo no es XML como si es XML pero no tiene el formato <Animales><Animal>...
                return "El archivo no es un XML válido o no tiene la estructura esperada (<Animales><Animal>...).";
            }
            catch (Exception)
            {
                return "Ocurrió un error inesperado al leer el archivo.";
            }

            if (wrapper == null || wrapper.Listado == null || wrapper.Listado.Count == 0)
            {
                return "El archivo XML no contiene animales para importar.";
            }

            int insertados = 0;
            StringBuilder errores = new StringBuilder();

            foreach (Animal a in wrapper.Listado)
            {
                if (string.IsNullOrEmpty(a.nombre) || string.IsNullOrEmpty(a.especie))
                {
                    errores.AppendLine("Se omitió un registro sin nombre o especie.");
                    continue;
                }

                dal.Alta(a);
                insertados++;
            }

            return $"Se importaron {insertados} animales. {errores}";
        }
    }
}
