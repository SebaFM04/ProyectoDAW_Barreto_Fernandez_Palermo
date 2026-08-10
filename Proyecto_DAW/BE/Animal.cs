using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BE
{
    [XmlRoot("Animal")]
    public class Animal
    {
        [XmlElement("CodigoAnimal")]
        public int codigoAnimal { get; set; }

        [XmlElement("Especie")]
        public string especie { get; set; }

        [XmlElement("Raza")]
        public string raza { get; set; }

        [XmlElement("Nombre")]
        public string nombre { get; set; }

        private string _tamano;

        [XmlElement("Tamano")] // sin ñ, para evitar líos de encoding en el XML
        public string tamaño
        {
            get { return _tamano; }
            set { _tamano = value; }
        }

        [XmlIgnore] // propiedad derivada, no la serializamos
        public string tamano
        {
            get { return _tamano; }
        }

        [XmlElement("Sexo")]
        public string sexo { get; set; }

        [XmlElement("EstadoAdopcion")]
        public string estadoAdopcion { get; set; }

        [XmlElement("Vivo")]
        public bool vivo { get; set; }

        // Constructor requerido por XmlSerializer
        public Animal() { }

        public Animal(int pCodigoAnimal, string pEspecie, string pRaza, string pNombre, string pTamaño, string pSexo, string pEstadoDeAdopcion, bool pVivo)
        {
            codigoAnimal = pCodigoAnimal;
            especie = pEspecie;
            raza = pRaza;
            nombre = pNombre;
            tamaño = pTamaño;
            sexo = pSexo;
            estadoAdopcion = pEstadoDeAdopcion;
            vivo = pVivo;
        }
    }
}
