using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class FichaDeIngreso
    {
        public int codigoFicha { get; set; }
        public int codigoAnimal { get; set; }
        public DateTime fecha { get; set; }

        public FichaDeIngreso() { }

        public FichaDeIngreso(int codigoFicha, int codigoAnimal, DateTime fecha)
        {
            this.codigoFicha = codigoFicha;
            this.codigoAnimal = codigoAnimal;
            this.fecha = fecha;
        }
    }
}
