using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class FichaMedica
    {
        public int codigo { get; set; }
        public int codigoAnimal { get; set; }
        public DateTime fecha { get; set; }
        public bool castrado { get; set; }
        public string dieta { get; set; }
        public string medicamento { get; set; }
        public string observaciones { get; set; }

        public FichaMedica() { }

        public FichaMedica(int codigo, int codigoAnimal, DateTime fecha, bool castrado, string dieta, string medicamento, string observaciones)
        {
            this.codigo = codigo;
            this.codigoAnimal = codigoAnimal;
            this.fecha = fecha;
            this.castrado = castrado;
            this.dieta = dieta;
            this.medicamento = medicamento;
            this.observaciones = observaciones;
        }
    }
}
