using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class CertificadoAdopcion
    {
        public string codigo { get; set; }
        public string dni { get; set; }
        public int codigoAnimal { get; set; }
        public string especie { get; set; }
        public string raza { get; set; }
        public string nombreAnimal { get; set; }
        public string nombreAdoptante { get; set; }
        public string apellidoAdoptante { get; set; }
        public DateTime fecha { get; set; }

        public CertificadoAdopcion() { }

        public CertificadoAdopcion(string codigo, string dni, int codigoAnimal, string especie, string raza,
            string nombreAnimal, string nombreAdoptante, string apellidoAdoptante, DateTime fecha)
        {
            this.codigo = codigo;
            this.dni = dni;
            this.codigoAnimal = codigoAnimal;
            this.especie = especie;
            this.raza = raza;
            this.nombreAnimal = nombreAnimal;
            this.nombreAdoptante = nombreAdoptante;
            this.apellidoAdoptante = apellidoAdoptante;
            this.fecha = fecha;
        }
    }
}
