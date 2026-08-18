using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Idioma
    {
        public Idioma() { }

        public Idioma(int pCodigo, string pNombre, bool pIsDisponible)
        {
            codigo = pCodigo;
            nombre = pNombre;
            isDisponible = pIsDisponible;
        }

        public int codigo { get; set; }
        public string nombre { get; set; }
        public bool isDisponible { get; set; }

        public override string ToString()
        {
            return nombre;
        }
    }
}
