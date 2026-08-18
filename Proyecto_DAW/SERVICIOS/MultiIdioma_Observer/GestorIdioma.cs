using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SERVICIOS.MultiIdioma_Observer
{
    // Sujeto concreto del patrón Observer.
    // Singleton "por sesión HTTP": cada usuario navegando el sitio tiene su
    // propio GestorIdioma en Session, para no pisar el idioma de otros
    // usuarios conectados al mismo tiempo (equivalente web del Singleton
    // por proceso que se usa en la versión WinForms).
    public class GestorIdioma : ISujetoIdioma
    {
        private const string SESSION_KEY = "GestorIdioma";

        public static GestorIdioma Instancia
        {
            get
            {
                var gestor = HttpContext.Current.Session[SESSION_KEY] as GestorIdioma;
                if (gestor == null)
                {
                    gestor = new GestorIdioma();
                    HttpContext.Current.Session[SESSION_KEY] = gestor;
                }
                return gestor;
            }
        }

        // Estado
        private int codigoIdiomaActual;
        private Dictionary<string, string> traducciones;
        private readonly List<IObservadorIdioma> observadores;

        private GestorIdioma()
        {
            codigoIdiomaActual = 1; // Español por defecto
            traducciones = new Dictionary<string, string>();
            observadores = new List<IObservadorIdioma>();
        }

        public int CodigoIdiomaActual => codigoIdiomaActual;

        // Traducción
        // Clave = "NombreFormulario|NombreControl". Si el idioma actual todavía
        // no tiene esa traducción cargada (por ejemplo, un idioma recién dado
        // de alta por un usuario), se devuelve "[NombreControl]" para dejar
        // explícito en la UI qué falta traducir.
        public string Traducir(string nombreFormulario, string nombreControl)
        {
            string clave = ClaveDe(nombreFormulario, nombreControl);
            if (traducciones.TryGetValue(clave, out string texto) && !string.IsNullOrEmpty(texto))
                return texto;

            return $"[{nombreControl}]";
        }

        public static string ClaveDe(string nombreFormulario, string nombreControl)
        {
            return $"{nombreFormulario}|{nombreControl}";
        }

        // Gestión de observadores
        public void Suscribir(IObservadorIdioma observador)
        {
            if (observador == null) return;
            if (!observadores.Contains(observador))
                observadores.Add(observador);
        }

        public void Desuscribir(IObservadorIdioma observador)
        {
            if (observador != null)
                observadores.Remove(observador);
        }

        public void Notificar()
        {
            foreach (var obs in new List<IObservadorIdioma>(observadores))
            {
                try { obs.ActualizarIdioma(); }
                catch { /* no interrumpir a los demás observadores suscriptos en este request */ }
            }
        }

        // Cambio de idioma: reemplaza el diccionario de traducciones vigente
        // (idioma sin ninguna traducción cargada todavía = diccionario vacío,
        // y Traducir() resuelve cada clave a "[NombreControl]" automáticamente)
        // y notifica a todos los observadores suscriptos en el request actual.
        public void CambiarIdioma(int codigoIdioma, Dictionary<string, string> nuevasTraducciones)
        {
            codigoIdiomaActual = codigoIdioma;
            traducciones = nuevasTraducciones ?? new Dictionary<string, string>();
            Notificar();
        }
    }
}
