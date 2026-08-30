using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public abstract class AccesoPermiso
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public char Tipo { get; set; }  
        public abstract List<Permiso> ObtenerPermisosSimples();

        public override string ToString()
        {
            return Nombre;
        }
    }
}
