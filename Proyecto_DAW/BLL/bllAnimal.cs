using BE;
using DAL;
using SERVICIOS;
using SERVICIOS.MultiIdioma_Observer;
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

        public bllAnimal()
        {
            dal = new dalAnimal();
            bllBitacora = new bllBitacora();
            bllDigitoVerificador = new bllDigitoVerificador();
        }

        public void AltaAnimal(string especie, string raza, string nombre, string tamano, string sexo, string estadoDeAdopcion, bool vivo)
        {
            if (string.IsNullOrWhiteSpace(especie)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_ESPECIE", "Ingrese la especie"));
            if (string.IsNullOrWhiteSpace(raza)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_RAZA", "Ingrese la raza"));
            if (string.IsNullOrWhiteSpace(nombre)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_NOMBRE", "Ingrese el nombre"));
            if (string.IsNullOrWhiteSpace(tamano)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_TAMANO", "Ingrese el tamaño"));
            if (string.IsNullOrWhiteSpace(sexo)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_SEXO", "Ingrese el sexo"));
            if (string.IsNullOrWhiteSpace(estadoDeAdopcion)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_ESTADO", "Ingrese el estado"));

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
            if (string.IsNullOrWhiteSpace(especie)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_ESPECIE", "Ingrese la especie"));
            if (string.IsNullOrWhiteSpace(raza)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_RAZA", "Ingrese la raza"));
            if (string.IsNullOrWhiteSpace(nombre)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_NOMBRE", "Ingrese el nombre"));
            if (string.IsNullOrWhiteSpace(tamano)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_TAMANO", "Ingrese el tamaño"));
            if (string.IsNullOrWhiteSpace(sexo)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_SEXO", "Ingrese el sexo"));
            if (string.IsNullOrWhiteSpace(estadoDeAdopcion)) throw new Exception(GestorIdioma.Msg("MSG_INGRESE_ESTADO", "Ingrese el estado"));

            Animal animal = BuscarAnimalPorCodigo(codigo);

            if (animal == null) throw new Exception(GestorIdioma.Msg("MSG_ANIMAL_NO_ENCONTRADO", "No se encontró un animal con el código proporcionado."));

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
            if (VerificarAnimalAdoptado(codigo)) throw new Exception(GestorIdioma.Msg("MSG_ANIMAL_ADOPTADO_NO_BORRAR", "No se puede borrar porque esta adoptado"));

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
                return GestorIdioma.Msg("MSG_XML_INVALIDO", "El archivo no es un XML válido o no tiene la estructura esperada (<Animales><Animal>...).");
            }
            catch (Exception)
            {
                return GestorIdioma.Msg("MSG_XML_ERROR_INESPERADO", "Ocurrió un error inesperado al leer el archivo.");
            }

            if (wrapper == null || wrapper.Listado == null || wrapper.Listado.Count == 0)
            {
                return GestorIdioma.Msg("MSG_XML_SIN_ANIMALES", "El archivo XML no contiene animales para importar.");
            }

            int insertados = 0;
            StringBuilder errores = new StringBuilder();

            foreach (Animal a in wrapper.Listado)
            {
                if (string.IsNullOrEmpty(a.nombre) || string.IsNullOrEmpty(a.especie))
                {
                    errores.AppendLine(GestorIdioma.Msg("MSG_XML_REGISTRO_OMITIDO", "Se omitió un registro sin nombre o especie."));
                    continue;
                }

                dal.Alta(a);
                insertados++;
            }

            string mensajeResultado = GestorIdioma.Msg("MSG_XML_IMPORT_RESULTADO", "Se importaron {0} animales. {1}");
            return string.Format(mensajeResultado, insertados, errores);
        }
    }
}
