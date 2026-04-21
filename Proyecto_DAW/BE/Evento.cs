using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Evento
    {
        public Evento(string cod, string pDni, DateTime f, TimeSpan h, string mod, string ev, int crit)
        : this(pDni, f, h, mod, ev, crit)
        {
            codigo = cod;
        }

        public Evento(string pDni, DateTime f, TimeSpan h, string mod, string ev, int crit)
        {
            login = pDni;
            fecha = f;
            hora = h;
            modulo = mod;
            evento = ev;
            criticidad = crit;
        }

        public string codigo { get; set; }
        public string login { get; set; }
        public DateTime fecha { get; set; }
        public TimeSpan hora { get; set; }
        public string modulo { get; set; }
        public string evento { get; set; }
        public int criticidad { get; set; }
    }
}
