using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Rol
    {
        public int ID { get; set; }
        public string Nombre { get; set; }

        public List<AccesoPermiso> Permisos { get; set; }

        public Rol()
        {
            Permisos = new List<AccesoPermiso>();
        }

        public Rol(int id, string nombre)
        {
            ID = id;
            Nombre = nombre;
            Permisos = new List<AccesoPermiso>();
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
