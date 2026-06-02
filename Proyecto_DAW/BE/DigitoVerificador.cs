using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class DigitoVerificador
    {
        public string nombreTabla { get; set; }
        public string horizontal { get; set; }
        public string vertical { get; set; }

        public DigitoVerificador(string pNombre, string pHorizontal, string pVertical)
        {
            nombreTabla = pNombre;
            horizontal = pHorizontal;
            vertical = pVertical;
        }
    }
}
