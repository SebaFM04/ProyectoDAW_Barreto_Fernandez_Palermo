using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Familia : AccesoPermiso
    {
        private List<AccesoPermiso> hijos;

        public Familia()
        {
            Tipo = 'C';
            hijos = new List<AccesoPermiso>();
        }

        public Familia(string nombre)
        {
            Nombre = nombre;
            Tipo = 'C';
            hijos = new List<AccesoPermiso>();
        }

        public Familia(int id, string nombre, string clave)
        {
            ID = id;
            Nombre = nombre;
            Clave = clave;
            Tipo = 'C';
            hijos = new List<AccesoPermiso>();
        }

        public void Agregar(AccesoPermiso hijo)
        {
            hijos.Add(hijo);
        }

        public void Quitar(AccesoPermiso hijo)
        {
            hijos.Remove(hijo);
        }

        public List<AccesoPermiso> ObtenerHijos()
        {
            return hijos;
        }

        public override List<Permiso> ObtenerPermisosSimples()
        {
            List<Permiso> permisos = new List<Permiso>();
            foreach (AccesoPermiso hijo in hijos)
            {
                permisos.AddRange(hijo.ObtenerPermisosSimples());
            }
            return permisos;
        }
    }
}
