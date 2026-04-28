using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Vacuna
    {
        public Vacuna(int pCodigo_941lp, int pCodigoAnimal, string pNombre, DateTime pFechaAplicacion, DateTime pProximaAplicacion)
        : this(pCodigoAnimal,  pNombre,  pFechaAplicacion,  pProximaAplicacion)
        {
            codigoVacuna = pCodigo_941lp;
        }

        public Vacuna(int pCodigoAnimal, string pNombre, DateTime pFechaAplicacion, DateTime pProximaAplicacion)
        { 
            codigoAnimal = pCodigoAnimal;
            nombreVacuna = pNombre;
            fechaAplicacion = pFechaAplicacion;
            fechaProximaAplicacion = pProximaAplicacion;
        }        

        public int codigoVacuna { get; set; }
        public int codigoAnimal { get; set; }
        public string nombreVacuna { get; set; }
        public DateTime fechaAplicacion { get; set; }
        public DateTime fechaProximaAplicacion { get; set; }
    }
}
