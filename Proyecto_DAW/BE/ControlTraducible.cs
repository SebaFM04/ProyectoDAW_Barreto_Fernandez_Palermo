using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class ControlTraducible
    {
        public ControlTraducible() { }

        public ControlTraducible(int pCodigo, string pNombreControl, string pNombreFormulario)
        {
            codigo = pCodigo;
            nombreControl = pNombreControl;
            nombreFormulario = pNombreFormulario;
        }

        public int codigo { get; set; }
        public string nombreControl { get; set; }
        public string nombreFormulario { get; set; }
    }
}
