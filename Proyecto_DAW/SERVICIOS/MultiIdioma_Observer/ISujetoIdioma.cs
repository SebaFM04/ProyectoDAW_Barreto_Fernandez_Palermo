using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS.MultiIdioma_Observer
{
    public interface ISujetoIdioma
    {
        void Suscribir(IObservadorIdioma obs);
        void Desuscribir(IObservadorIdioma obs);
        void Notificar();
    }
}
