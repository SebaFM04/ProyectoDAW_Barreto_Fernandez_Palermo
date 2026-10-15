using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Fila de la grilla de edición: una etiqueta con su texto en el idioma predeterminado
    // y su traducción (puede estar vacía) en el idioma elegido.
    public class Traduccion
    {
        public Traduccion(int pIdIdioma, int pIdEtiqueta, string pClave, string pTexto, string pTextoPredeterminado)
        {
            IdIdioma = pIdIdioma;
            IdEtiqueta = pIdEtiqueta;
            Clave = pClave;
            Texto = pTexto;
            TextoPredeterminado = pTextoPredeterminado;
        }

        public int IdIdioma { get; set; }
        public int IdEtiqueta { get; set; }
        public string Clave { get; set; }
        public string Texto { get; set; }
        public string TextoPredeterminado { get; set; }
    }
}
