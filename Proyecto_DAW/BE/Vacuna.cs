using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Vacuna
    {
        public Vacuna(string pCodigo, string pNombre, bool pActivo = true)
        {
            codigoVacuna = pCodigo;
            nombreVacuna = pNombre;
            activo = pActivo;
        }        

        public string codigoVacuna { get; set; }
        public string nombreVacuna { get; set; }
        public bool activo { get; set; }
    }
}
