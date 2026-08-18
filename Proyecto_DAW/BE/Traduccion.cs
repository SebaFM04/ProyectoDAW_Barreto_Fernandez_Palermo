using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Fila "plana" con el detalle necesario tanto para armar el diccionario
    // en tiempo de ejecución (GestorIdioma) como para listar en una pantalla
    // de administración de traducciones (control + formulario + texto).
    public class Traduccion
    {
        public Traduccion() { }

        public int codigoControl { get; set; }
        public string nombreControl { get; set; }
        public string nombreFormulario { get; set; }
        public int codigoIdioma { get; set; }
        public string textoTraducido { get; set; }
    }
}
