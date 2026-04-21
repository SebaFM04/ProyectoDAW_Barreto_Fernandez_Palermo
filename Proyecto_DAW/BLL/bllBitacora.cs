using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BE;

namespace BLL
{
    public class bllBitacora
    {
        dalBitacora dalBitacora;

        public bllBitacora()
        {
            dalBitacora = new dalBitacora();
        }

        public void Alta(string login, string modulo, string evento, int criticidad)
        {
            Evento nuevoEvento = new Evento(login, DateTime.Now.Date, DateTime.Now.TimeOfDay, modulo, evento, criticidad);
            dalBitacora.Alta(nuevoEvento);
        }

        public List<Evento> Filtros(Dictionary<string, string> filtros)
        {
            return dalBitacora.Filtros(filtros);
        }

        public List<Evento> RetornarEventos()
        {
            List<Evento> aux = new List<Evento>();
            foreach (Evento c in dalBitacora.RetornarEventos())
            {
                aux.Add(new Evento(c.codigo, c.login, c.fecha, c.hora, c.modulo, c.evento, c.criticidad));
            }
            return aux;
        }
    }
}
