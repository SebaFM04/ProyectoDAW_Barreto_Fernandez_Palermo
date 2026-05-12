using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class IntermediaVacunaAnimal
    {
        public IntermediaVacunaAnimal(int pCodigo,string pCodigoVacuna, int pCodigoAnimal, string pNombre, DateTime pFechaAplicacion, DateTime pProximaAplicacion)
        : this(pCodigoVacuna ,pCodigoAnimal,  pNombre,  pFechaAplicacion,  pProximaAplicacion)
        {
            codigo = pCodigo;
        }

        public IntermediaVacunaAnimal(string pCodigoVacuna, int pCodigoAnimal, string pNombre, DateTime pFechaAplicacion, DateTime pProximaAplicacion)
        {
            codigoVacuna = pCodigoVacuna;
            codigoAnimal = pCodigoAnimal;
            nombreVacuna = pNombre;
            fechaAplicacion = pFechaAplicacion;
            fechaProximaAplicacion = pProximaAplicacion;
        }

        public int codigo { get; set; }
        public string codigoVacuna { get; set; }
        public int codigoAnimal { get; set; }
        public string nombreVacuna { get; set; }
        public DateTime fechaAplicacion { get; set; }
        public DateTime fechaProximaAplicacion { get; set; }
    }
}
