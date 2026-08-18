using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Adoptante
    {
        public Adoptante(string pDni, string pNombre, string pApellido, string pTelefono, int pEdad, string pDomicilio, bool pMascotas, bool pActivo)
        {
            dni = pDni;
            nombre = pNombre;
            apellido = pApellido;
            telefono = pTelefono;
            edad = pEdad;
            domicilio = pDomicilio;
            mascotas = pMascotas;
            activo = pActivo;
        }

        public string dni { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string telefono { get; set; }
        public int edad { get; set; }
        public string domicilio { get; set; }
        public bool mascotas { get; set; }
        public bool activo { get; set; }
    }
}
