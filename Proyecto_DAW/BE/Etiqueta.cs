using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Etiqueta
    {
        public Etiqueta(int pId, string pClave, string pDescripcion = null)
        {
            ID = pId;
            Clave = pClave;
            Descripcion = pDescripcion;
        }

        public int ID { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
    }
}
