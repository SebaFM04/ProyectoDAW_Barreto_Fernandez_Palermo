using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HistorialIngreso
    {
        public int codigoHistorial { get; set; }
        public int codigoFicha { get; set; }
        public DateTime fecha { get; set; }
        public string motivo { get; set; }

        public HistorialIngreso() { }

        public HistorialIngreso(int codigoHistorial, int codigoFicha, DateTime fecha, string motivo)
        {
            this.codigoHistorial = codigoHistorial;
            this.codigoFicha = codigoFicha;
            this.fecha = fecha;
            this.motivo = motivo;
        }
    }
}
