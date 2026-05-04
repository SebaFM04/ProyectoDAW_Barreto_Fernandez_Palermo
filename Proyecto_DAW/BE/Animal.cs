using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Animal
    {
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

        public int codigoAnimal { get; set; }
        public string especie { get; set; }
        public string raza { get; set; }
        public string nombre { get; set; }
        public string tamaño { get; set; }
        public string sexo { get; set; }
        public string estadoAdopcion { get; set; }
        public bool vivo { get; set; }
    }
}
