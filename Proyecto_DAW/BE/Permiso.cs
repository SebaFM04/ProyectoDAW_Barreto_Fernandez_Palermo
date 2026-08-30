using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Permiso : AccesoPermiso
    {
        public Permiso()
        {
            Tipo = 'S';
        }

        public Permiso(int id, string nombre, string clave)
        {
            ID = id;
            Nombre = nombre;
            Clave = clave;
            Tipo = 'S';
        }

        public override List<Permiso> ObtenerPermisosSimples()
        {
            return new List<Permiso> { this };
        }
    }
}
